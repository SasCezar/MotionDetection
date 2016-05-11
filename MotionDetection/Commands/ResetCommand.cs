using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MotionDetection.ViewModels;

namespace MotionDetection.Commands
{
	public class ResetCommand : ICommand
	{

		public ViewModelWindow ViewModel;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			ViewModel.ClearPlot();
		}

		public event EventHandler CanExecuteChanged;
	}
}
