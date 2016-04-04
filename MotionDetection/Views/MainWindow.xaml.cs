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
			//Don't forget DataContext so we can bind these properties.
			DataContext = viewModel;
		}

	}
}