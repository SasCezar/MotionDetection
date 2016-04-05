using System.Windows;
using MotionDetection.Models;
using MotionDetection.ViewModels;
using MotionDetection.Views;

namespace MotionDetection
{
	public class Main
	{
		private MainWindow Form;
		private ViewModelWindow _viewModel;
	    private CircularBufferMatrix<double> Buffer;

        public Main()
		{
			_viewModel = new ViewModelWindow();
			Form = new MainWindow(_viewModel);
			Form.Show();
			/* Test - Do Not Remove
			CircularBufferMatrix<double> matrice = new CircularBufferMatrix<double>(5, 5, 5) {[1, 1, 1] = 50.5};
			double[] ou = matrice.GetSubArray(1, 1);
			ou[1] = 10;
			MessageBox.Show("" + matrice[1,1,1] + " " + ou[1]);
			*/
            Buffer = new CircularBufferMatrix<double>(13, 5, 1000);
		}
	}
}