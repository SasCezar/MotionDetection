using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MotionDetection
{
	internal class DataReceiver
	{
		public static void Start()
		{
			var ep = new IPEndPoint(IPAddress.Any, 45555);

			// Crea una socket di tipo TCP
			// per creare una connessione UDP occorre usare
			// esplicitamente la new Socket()
			var listener = new TcpListener(ep);
			listener.Start();

			var socket = listener.AcceptSocket(); // blocca
			//Client(socket);

			// ParametrizedThreadStart è un delegate
			// Client è un delegate- handler

			var socketClientThread = new Thread(Read);
			socketClientThread.Start(socket);

			// esegue tutto (blocca)
		}


		public static void Read(object obj)
		{
			var socket = (Socket) obj;
			using (Stream stream = new NetworkStream(socket))
			using (var reader = new BinaryReader(stream))
			{
				byte[] len = new byte[2];
				byte[] tem = new byte[3];
				int byteToRead;
				byte[] packet;
				int numOfSensors;
				const int maxNumberOfSensors = 10;

				while (!(tem[0] == 0xFF && tem[1] == 0x32)) // Cerca la sequenza FF-32
				{
					tem[0] = tem[1];
					tem[1] = tem[2];
					byte[] read = reader.ReadBytes(1);
					tem[2] = read[0];
				}
				if (tem[2] != 0xFF) // Modalità normale
				{
					byteToRead = tem[2]; // Byte da leggere
				}
				else // Modalità extended-length
				{
					len = new byte[2];
					len = reader.ReadBytes(2);
					byteToRead = len[0]*256 + len[1]; // Byte da leggere
				}

				byte[] data = reader.ReadBytes(byteToRead + 1); // Lettura dei dati

				packet = (tem[2] != 0xFF) ? new byte[byteToRead + 4] : new byte[byteToRead + 6]; // Creazione packetto

				numOfSensors = (byteToRead - 2)/52; // Calcolo del numero di sensori
				packet[0] = 0xFF; // Copia dei primi elementi
				packet[1] = 0x32;
				packet[2] = tem[2];

				if (tem[2] != 0xFF)
				{
					data.CopyTo(packet, 3); // Copia dei dati
				}
				else
				{
					packet[3] = len[0];
					packet[4] = len[1];
					data.CopyTo(packet, 5); // Copia dei dati
				}


				CircularBufferMatrix<float> buffer = new CircularBufferMatrix<float>(13, numOfSensors, 25); // Creazione Buffer


				int[] t = new int[maxNumberOfSensors];

				var time = 0;

				do
				{
					for (var x = 0; x < numOfSensors; x++)
					{
						t[x] = 5 + 52*x;
					}

					for (var i = 0; i < numOfSensors; i++)
					{
						byte[] temp = new byte[4];
						for (var tr = 0; tr < 13; tr++) // 13 campi, 3 * 3 + 4
						{
							if (numOfSensors < 5)
							{
								temp[0] = packet[t[i] + 3]; // Lettura inversa
								temp[1] = packet[t[i] + 2];
								temp[2] = packet[t[i] + 1];
								temp[3] = packet[t[i]];
							}
							else
							{
								temp[0] = packet[t[i] + 5];
								temp[1] = packet[t[i] + 4];
								temp[2] = packet[t[i] + 3];
								temp[3] = packet[t[i] + 2];
							}
							var valore = BitConverter.ToSingle(temp, 0); // Conversione
							buffer[tr, i, time] = valore; // Memorizzazione
							t[i] += 4;
						}
					}

					// Lettura pacchetto seguente
					packet = numOfSensors < 5 ? reader.ReadBytes(byteToRead + 4) : reader.ReadBytes(byteToRead + 6);
					time++; // Incremento contatore tempo
				} while (packet.Length != 0);
			}
		}
	}
}