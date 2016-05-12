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
			var resetCommand = new ResetCommand();
			var signalProcessor = new SignalProcess();
			var dataProcessor = new DataProcessor();
			var motionRecognizer = new MotionRecognition();
			var orientationRecognizer = new OrientationRecognition();
            var postureRecognizer = new PostureRecognition();
            var deadReckoningRecognizer = new DeadReckoningRecognition();
			var viewModel = new ViewModelWindow(command, resetCommand);
			var form = new MainWindow(viewModel);

			resetCommand.ViewModel = viewModel;

			dataReceiver.OnDataReceivedEventHandler += signalProcessor.OnDataReceived;
			signalProcessor.OnSignalProcessedEventHandler += dataProcessor.OnSignalSmoothed;
		    signalProcessor.OnSignalProcessedEventHandler += postureRecognizer.OnDataReceived;
			dataProcessor.OnSingleDataProcessedEventHandeler += motionRecognizer.OnDataReceived;
			dataProcessor.OnMultipleDataProcessedEventHandeler += orientationRecognizer.OnDataReceived;
			motionRecognizer.OnPlotMovementEventHandler += viewModel.OnDataProcessed;
			orientationRecognizer.OnPlotOrientation += viewModel.OnDataProcessed;
		    postureRecognizer.OnPostureRecognizedHandeler += dataProcessor.OnRecognized;
		    dataProcessor.OnDeadEventHandler += deadReckoningRecognizer.OnDataReceived;
		    postureRecognizer.OnPostureRecognizedHandeler += viewModel.OnDataProcessed;
			dataProcessor.OnSingleDataProcessedEventHandeler += viewModel.OnDataProcessed;
		    deadReckoningRecognizer.OnDeadReckoningRecognizedHandler += viewModel.OnDeadReckoningReceived;

			form.Show();
		}
	}
}