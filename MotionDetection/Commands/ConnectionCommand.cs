using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            throw new NotImplementedException();
        }

        public async void Execute(object parameter)
        {
            
        }

        public event EventHandler CanExecuteChanged;
    }
}
