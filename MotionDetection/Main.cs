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
			var dataManipulator = new DataManipulation();
			var dataReceiver = new DataReceiver(dataManipulator);
			var command = new ConnectionCommand(dataReceiver);
			var viewModel = new ViewModelWindow(command, dataManipulator);
			var form = new MainWindow(viewModel);
			form.Show();
		}
	}
}