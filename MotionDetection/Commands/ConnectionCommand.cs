using System;
using System.Net.Sockets;
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
			if(Receiver.Socket != null) { 
				return !Receiver.Connected;
			}
			return true;
		}

		public async void Execute(object parameter)
		{
			await Task.Factory.StartNew(() => Receiver.Start());
		}

		// TODO Manages CanExecute form socket
		public event EventHandler CanExecuteChanged;
	}
}