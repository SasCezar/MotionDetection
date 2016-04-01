using MotionDetection.ViewModels;
using MotionDetection.Views;

namespace MotionDetection
{
	public class Main
	{
		private readonly MainWindow Form;
		private readonly ViewModelWindow ViewModel;

		public Main()
		{
			ViewModel = new ViewModelWindow();
			Form = new MainWindow(ViewModel);
			Form.Show();
		}
	}
}