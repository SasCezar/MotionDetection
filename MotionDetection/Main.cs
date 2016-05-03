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
			var dataProcessor = new DataProcessor();
			var motionRecognizer = new MotionRecognition();
			var orientationRecognizer = new OrientationRecognition();
			var viewModel = new ViewModelWindow(command);
			var form = new MainWindow(viewModel);

			dataReceiver.OnDataReceivedEventHandler += signalProcessor.OnDataReceived;
			signalProcessor.OnSignalProcessedEventHandler += dataProcessor.OnSignalSmoothed;
			dataProcessor.OnSingleDataProcessedEventHandeler += motionRecognizer.OnDataReceived;
			dataProcessor.OnMultipleDataProcessedEventHandeler += orientationRecognizer.OnDataReceived;
			motionRecognizer.OnPlotMovementEventHandeler += viewModel.OnDataProcessed;
			orientationRecognizer.OnPlotOrientation += viewModel.OnDataProcessed;
			dataProcessor.OnSingleDataProcessedEventHandeler += viewModel.OnDataProcessed;

			form.Show();
		}
	}
}