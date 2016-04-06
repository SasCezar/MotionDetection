using System.Windows;
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
			DataManipulation dataManipulator = new DataManipulation();
			DataReceiver dataReceiver = new DataReceiver(dataManipulator);
			ConnectionCommand command = new ConnectionCommand(dataReceiver);
			ViewModelWindow viewModel = new ViewModelWindow(command, dataManipulator);
			MainWindow form = new MainWindow(viewModel);
			form.Show();

		}
	}
}