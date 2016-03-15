using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MotionDetection
{
	class DataReceiver
	{
		public static void Start()
		{
			IPEndPoint ep = new IPEndPoint(IPAddress.Any, 45555);
			
			// Crea una socket di tipo TCP
			// per creare una connessione UDP occorre usare
			// esplicitamente la new Socket()
			TcpListener listener = new TcpListener(ep);
			listener.Start();

			var socket = listener.AcceptSocket(); // blocca
												  //Client(socket);

			// ParametrizedThreadStart è un delegate
			// Client è un delegate- handler

			Thread socketClientThread = new Thread(Read);
			socketClientThread.Start(socket);

			// esegue tutto (blocca)
		}


		public static void Read(object obj)
		{
			Socket socket = (Socket)obj;
			using (Stream stream = new NetworkStream(socket))
			using (BinaryReader reader = new BinaryReader(stream))
			{
				//ricevo la risposta
				//byte[] risposta = reader.ReadBytes(1); //lettura sullo stream (bloccante)
				//String output = "" + risposta;
				//MessageBox.Show(output);
				byte[] len = new byte[2];
				byte[] tem = new byte[3];
				int byteToRead;
				byte[] pacchetto;
				int numSensori;
				const int maxSensori = 10;

				while (!(tem[0] == 0xFF && tem[1] == 0x32)) // cerca la sequenza FF-32
				{
					tem[0] = tem[1];
					tem[1] = tem[2];
					byte[] read = reader.ReadBytes(1);
					tem[2] = read[0];
				}
				if (tem[2] != 0xFF) // modalità normale
				{
					byteToRead = tem[2]; // byte da leggere
				}
				else  // modalità extended-length
				{
					len = new byte[2];
					len = reader.ReadBytes(2);
					byteToRead = (len[0] * 256) + len[1]; // byte da leggere
				}

				byte[] data = new byte[byteToRead + 1];
				data = reader.ReadBytes(byteToRead + 1); // lettura dei dati

				if (tem[2] != 0xFF)
				{
					pacchetto = new byte[byteToRead + 4]; // creazione pacchetto
				}
				else
				{
					pacchetto = new byte[byteToRead + 6];
				}

				numSensori = (byteToRead - 2) / 52; // calcolo del numero di sensori
				pacchetto[0] = 0xFF; // copia dei primi elementi
				pacchetto[1] = 0x32;
				pacchetto[2] = tem[2];

				if (tem[2] != 0xFF)
				{
					data.CopyTo(pacchetto, 3); // copia dei dati
				}
				else
				{
					pacchetto[3] = len[0];
					pacchetto[4] = len[1];
					data.CopyTo(pacchetto, 5); // copia dei dati
				}


				List<List<double>> array = new List<List<double>>(); // salvataggio dati


				int[] t = new int[maxSensori];

				for (int x = 0; x < numSensori; x++)
				{
					array.Add(new List<double>()); // una lista per ogni sensore
					t[x] = 5 + (52 * x);
				}
				while (true)
				{
					for (int i = 0; i < numSensori; i++)
					{
						byte[] temp = new byte[4];
						for (int tr = 0; tr < 13; tr++)// 13 campi, 3 * 3 + 4
						{
							if (numSensori < 5)
							{
								temp[0] = pacchetto[t[i] + 3]; // lettura inversa
								temp[1] = pacchetto[t[i] + 2];
								temp[2] = pacchetto[t[i] + 1];
								temp[3] = pacchetto[t[i]];
							}
							else
							{
								temp[0] = pacchetto[t[i] + 5];
								temp[1] = pacchetto[t[i] + 4];
								temp[2] = pacchetto[t[i] + 3];
								temp[3] = pacchetto[t[i] + 2];
							}
							var valore = BitConverter.ToSingle(temp, 0); // conversione
							array[i].Add(valore); // memorizzazione
							t[i] += 4;
						}
					}


					for (int x = 0; x < numSensori; x++)
					{
						t[x] = 5 + (52 * x);
					}

					for (int j = 0; j < numSensori; j++)
					{
						for (int tr = 0; tr < 13; tr++)
						{
							// esempio output su console
							Console.Write(array[j][tr] + "; ");
						}
						Console.WriteLine();
						array[j].RemoveRange(0, 13); // cancellazione dati
					}

					Console.WriteLine();

					if (numSensori < 5) // lettura pacchetto seguente
					{
						pacchetto = reader.ReadBytes(byteToRead + 4);
					}
					else
					{
						pacchetto = reader.ReadBytes(byteToRead + 6);
					}
				}

			}

		}


	}
}



