
using MotionDetection.ViewModels;
using MotionDetection.Views;

namespace MotionDetection
{
	class Main
	{
        ViewModelWindow ViewModel = new ViewModelWindow();
        MainWindow Form = new MainWindow(ViewModel);
	}
}
