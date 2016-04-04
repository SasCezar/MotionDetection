using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MotionDetection.Models
{
	public delegate void OnDataReceived(object sender, DataEventArgs eventArgs);

	public delegate void MyDataHandler(object sender, EventArgs eventArgs);

	public class DataReceiver
	{
		public event OnDataReceived NewDataReceived;
		public event MyDataHandler NewData;

		public void Start()
		{
			var ep = new IPEndPoint(IPAddress.Any, 45555);
			var listener = new TcpListener(ep);
			listener.Start();
			var socket = listener.AcceptSocket();
			var socketClientThread = new Thread(Read);
			socketClientThread.Start(socket);
		}


		public void Read(object obj)
		{
			var socket = (Socket) obj;
			using (Stream stream = new NetworkStream(socket))
			using (var reader = new BinaryReader(stream))
			{
				var len = new byte[2];
				var tem = new byte[3];
				int byteToRead;
				const int maxNumberOfSensors = 10;

				while (!(tem[0] == 0xFF && tem[1] == 0x32)) // Cerca la sequenza FF-32
				{
					tem[0] = tem[1];
					tem[1] = tem[2];
					var read = reader.ReadBytes(1);
					tem[2] = read[0];
				}
				if (tem[2] != 0xFF) // Modalità normale
				{
					byteToRead = tem[2]; // Byte da leggere
				}
				else // Modalità extended-length
				{
					len = reader.ReadBytes(2);
					byteToRead = len[0]*256 + len[1]; // Byte da leggere
				}

				var data = reader.ReadBytes(byteToRead + 1); // Lettura dei dati

				var packet = tem[2] != 0xFF ? new byte[byteToRead + 4] : new byte[byteToRead + 6];

				var numOfSensors = (byteToRead - 2)/52;
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

				var buffer = new CircularBufferMatrix<double>(13, numOfSensors, 750); // Creazione Buffer

				var t = new int[maxNumberOfSensors];

				var time = 0;

				do
				{
					for (var x = 0; x < numOfSensors; x++)
					{
						t[x] = 5 + 52*x;
					}

					for (var i = 0; i < numOfSensors; i += numOfSensors)
					{
						var byteNumber = new byte[4];
						for (var tr = 0; tr < 1; tr++) // 13 campi, 3 * 3 + 4
						{
							if (numOfSensors < 5)
							{
								byteNumber[0] = packet[t[i] + 3]; // Lettura inversa
								byteNumber[1] = packet[t[i] + 2];
								byteNumber[2] = packet[t[i] + 1];
								byteNumber[3] = packet[t[i]];
							}
							else
							{
								byteNumber[0] = packet[t[i] + 5];
								byteNumber[1] = packet[t[i] + 4];
								byteNumber[2] = packet[t[i] + 3];
								byteNumber[3] = packet[t[i] + 2];
							}
							var valore = BitConverter.ToSingle(byteNumber, 0); // Conversione
							buffer[tr, i, time] = valore; // Memorizzazione

							var dataArgs = new DataEventArgs
							{
								SensorData = new DataViewModel
								{
									Value = valore,
									Time = time,
									SensorType = (SensorTypeEnum) tr
								}
							};

							NewDataReceived?.Invoke(this, dataArgs);

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