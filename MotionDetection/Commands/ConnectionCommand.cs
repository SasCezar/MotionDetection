using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MotionDetection.Models;

namespace MotionDetection.Commands
{
	public class ConnectionCommand : ICommand
	{
		public DataReceiver Receiver;

		public ConnectionCommand(DataReceiver receiver)
		{
			Receiver = receiver;
		}

		public bool CanExecute(object parameter)
		{
			//throw new NotImplementedException();
			return true;
		}

		public async void Execute(object parameter)
		{
			await Task.Factory.StartNew(() => Receiver.Start());
		}

		public event EventHandler CanExecuteChanged;
	}
}