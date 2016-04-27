using System;
using System.Threading;
using MotionDetection.Commands;
using MotionDetection.Models;
using MotionDetection.Tests;
using MotionDetection.ViewModels;
using MotionDetection.Views;

namespace MotionDetection
{
	public class Main
	{
		public Main()
		{
			var dataReceiver = new DataReceiver();
			var command = new ConnectionCommand(dataReceiver);
			var signalProcessor = new SignalProcess();
			dataReceiver.OnDataReceivedEventHandler += signalProcessor.OnDataReceived;
			//dataReceiver.OnDataReceivedEventHandler += DataTesting.OnDataToPrint;
			var dataProcessor = new DataProcessor();
			signalProcessor.OnSignalProcessedEventHandler += dataProcessor.OnSignalSmoothed;
			var viewModel = new ViewModelWindow(command);
			dataProcessor.OnDataProcessedEventHandeler += viewModel.OnDataProcessed;
			var form = new MainWindow(viewModel);
	        form.Show(); 
		}
	}
}