
using MotionDetection.ViewModels;
using MotionDetection.Views;

namespace MotionDetection
{
	public class Main
	{
	    private ViewModelWindow ViewModel;
	    private MainWindow Form;

        public Main()
	    {
            ViewModel = new ViewModelWindow();
            Form = new MainWindow(ViewModel);
        }
       
	}
}
