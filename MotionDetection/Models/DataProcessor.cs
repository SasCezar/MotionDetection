using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotionDetection.Models
{
	public delegate void SingleDataProcessedEventHandeler(object sender, SingleDataEventArgs eventArgs);
	public delegate void MultipleDataProcessedEventHandeler(object sender, MultipleDataEventArgs eventArgs);

	public class DataProcessor
	{
		private Buffer3DMatrix<double> _buffer;
		private int _time;

		public event SingleDataProcessedEventHandeler OnSingleDataProcessedEventHandeler;
		public event MultipleDataProcessedEventHandeler OnMultipleDataProcessedEventHandeler;

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
					var dataArgsAccelerometers = new SingleDataEventArgs
					{
						UnityNumber = i,
						SeriesType = taskIndex,
						SensorOne = result,
						Time = _time
					};
					OnSingleDataProcessedEventHandeler?.Invoke(this, dataArgsAccelerometers);
					taskIndex++;
				}

				OnMultipleDataProcessedEventHandeler?.Invoke(this, new MultipleDataEventArgs()
				{
					SensorOne = _buffer.GetSubArray(i, (int)SensorType.Magnetometer1),
					SensorTwo = _buffer.GetSubArray(i, (int)SensorType.Magnetometer2),
					SensorThree = _buffer.GetSubArray(i, (int)SensorType.Magnetometer3),
					Time = _time,
					SeriesType = (int)SeriesType.Turning,
					UnityNumber = i
				});
			}
		}

		public IEnumerable<Task<double[]>> createTask(int unityNumber)
		{
			var tasks = new List<Task<double[]>>();
			var modAccTask =
				new Task<double[]>(() => DataManipulation.Modulo(_buffer.GetSubArray(unityNumber, (int) SensorType.Accelerometer1),
					_buffer.GetSubArray(unityNumber, (int) SensorType.Accelerometer2),
					_buffer.GetSubArray(unityNumber, (int) SensorType.Accelerometer3)));
			var modGyrTask =
				new Task<double[]>(() => DataManipulation.Modulo(_buffer.GetSubArray(unityNumber, (int) SensorType.Gyroscope1),
					_buffer.GetSubArray(unityNumber, (int) SensorType.Gyroscope2),
					_buffer.GetSubArray(unityNumber, (int) SensorType.Gyroscope3)));

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