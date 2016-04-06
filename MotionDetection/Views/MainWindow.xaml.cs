using System.Windows;
using MotionDetection.ViewModels;

namespace MotionDetection.Views
{
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow(ViewModelWindow viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;
		
		}

	}
}