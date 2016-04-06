using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace MotionDetection
{
	/// <summary>
	///     Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private const int MINIMUM_SPLASH_TIME = 1500; // Miliseconds 
		private const int SPLASH_FADE_TIME = 500; // Miliseconds 

		protected override void OnStartup(StartupEventArgs e)
		{
			Main main = new Main();
			// Step 1 - Load the splash screen 
			var splash = new SplashScreen("Resources/SplashScreen.tif");
			splash.Show(false, true);

			// Step 2 - Start a stop watch 
			var timer = new Stopwatch();
			timer.Start();

			// Step 3 - Load your windows but don't show it yet 
			//base.OnStartup(e);
			//var main = new MainWindow();
	
			// Step 4 - Make sure that the splash screen lasts at least two seconds 
			timer.Stop();
			var remainingTimeToShowSplash = MINIMUM_SPLASH_TIME - (int) timer.ElapsedMilliseconds;
			if (remainingTimeToShowSplash > 0)
				Thread.Sleep(remainingTimeToShowSplash);

			// Step 5 - show the page 
			splash.Close(TimeSpan.FromMilliseconds(SPLASH_FADE_TIME));
	
		}
	}
}