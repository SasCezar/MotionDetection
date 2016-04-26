using System;
using System.Threading;
using MotionDetection.Commands;
using MotionDetection.Models;
using MotionDetection.ViewModels;
using MotionDetection.Views;

namespace MotionDetection
{
	public class Main
	{
		public Main()
		{
			//         // TODO Verify data output
			//         var dataManipulator = new DataManipulation();
			//var recognition = new MotionRecognition(dataManipulator);
			//         var dataReceiver = new DataReceiver();
			//         var command = new ConnectionCommand(dataReceiver);
			//         var viewModel = new ViewModelWindow(command, dataManipulator, recognition);
			//         var form = new MainWindow(viewModel);
			//         form.Show(); 

			var dataReceiver = new DataReceiver();
			var command = new ConnectionCommand(dataReceiver);
			var signalProcessor = new SignalProcess();
			dataReceiver.OnDataReceivedEventHandler += signalProcessor.OnDataReceived;
			var dataProcessor = new DataProcessor();
			signalProcessor.OnSignalProcessedEventHandler += dataProcessor.OnSignalSmoothed;
			var viewModel = new ViewModelWindow(command);
			dataProcessor.OnDataProcessedEventHandeler += viewModel.OnDataProcessed;
			var form = new MainWindow(viewModel);
	        form.Show(); 
		}
	}
}