using System;
using System.Threading;
using MotionDetection.Commands;
using MotionDetection.Models;
using MotionDetection.ViewModels;
using MotionDetection.Views;

namespace MotionDetection
{
	public class Main
	{
		public Main()
		{
            // TODO Verify data output
            var dataManipulator = new DataManipulation();
            var dataReceiver = new DataReceiver(dataManipulator);
            var command = new ConnectionCommand(dataReceiver);
            var viewModel = new ViewModelWindow(command, dataManipulator);
            var form = new MainWindow(viewModel);
            form.Show();
      //      CircularBuffer3DMatrix<String> buffer = new CircularBuffer3DMatrix<string>(1, 1, 75);
		    //for (int i = 0; i < 100; ++i)
		    //{
		    //    if (i < 25)
		    //    {
		    //        buffer[0, 0, i] = "A";
		    //    }
      //          else if (i < 50)
      //          {
      //              buffer[0, 0, i] = "B";
      //          }
      //          else if (i < 75)
      //          {
      //              buffer[0, 0, i] = "C";
      //          }
      //          else if (i < 100)
      //          {
      //              buffer[0, 0, i] = "D";
      //          }
                
      //      }
		    //    for (int time = 0; time < buffer.Time; ++time)
		    
		    //    {
		    //       var a = buffer[0, 0, time];
		    //    }
            
        }
	}
}