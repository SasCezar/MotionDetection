namespace MotionDetection.Models
{
	public class CircularBuffer3DMatrix<T> : Buffer3DMatrix<T>
	{
		public CircularBuffer3DMatrix(int unityNumber, int sensorType, int time) : base(unityNumber, sensorType, time)
		{
			GlobalTime = 0;
		}

		public int GlobalTime { get; set; }

		public new T this[int unityNumber, int sensorType, int time]
		{
			get { return Buffer[unityNumber][sensorType][time%Time]; }
			set { Buffer[unityNumber][sensorType][time%Time] = value; }
		}
	}
}