using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotionDetection.Models
{
	public delegate void DataProcessedEventHandeler(object sender, PlotEventArgs eventArgs);

	public class DataProcessor
	{

		public event DataProcessedEventHandeler OnDataProcessedEventHandeler;

		private Buffer3DMatrix<double> _buffer;
		private int _time;

		public async void ProcessData()
		{
			for (var i = 0; i < Parameters.NumUnity; i++)
			{
				var tasks = createTask(i);

				foreach (var task in tasks)
				{
					task.RunSynchronously();
				}

				var allResults = await Task.WhenAll(tasks);

				var taskIndex = 0;
				foreach (var result in allResults)
				{

					var dataArgsAccelerometers = new PlotEventArgs
					{
						SensorNumber = i,
						SeriesType = taskIndex,
						SensorData = result,
						Time = _time
					};
					OnDataProcessedEventHandeler?.Invoke(this, dataArgsAccelerometers);
					taskIndex++;
				}
			}
		}

		public IEnumerable<Task<double[]>> createTask(int unityNumber)
		{
			var tasks = new List<Task<double[]>>();
			var modAccTask = new Task<double[]>(() => DataManipulation.Modulo(_buffer.GetSubArray(unityNumber, (int)SensorType.Accelerometer1),
				_buffer.GetSubArray(unityNumber, (int)SensorType.Accelerometer2), _buffer.GetSubArray(unityNumber, (int)SensorType.Accelerometer3)));
			var modGyrTask = new Task<double[]>(() => DataManipulation.Modulo(_buffer.GetSubArray(unityNumber, (int)SensorType.Gyroscope1),
				_buffer.GetSubArray((int)SensorType.Gyroscope2, unityNumber), _buffer.GetSubArray(unityNumber, (int)SensorType.Gyroscope3)));
	
			tasks.Add(modAccTask);
			tasks.Add(modGyrTask);

			return tasks;
		}

		public void OnSignalSmoothed(object sender, BufferEventArgs<double> eventargs)
		{
			_buffer = eventargs.Data;
			_time = eventargs.Time;
			ProcessData();
		}
	}
}