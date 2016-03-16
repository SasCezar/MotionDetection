using System;

namespace MotionDetection
{
	public enum SensorTypeEnum
	{
		Accelerometer1,
		Accelerometer2,
		Accelerometer3,
		Gyroscope1,
		Gyroscope2,
		Gyroscope3,
		Magnetometer1,
		Magnetometer2,
		Magnetometer3,
		Quaternion0,
		Quaternion1,
		Quaternion2,
		Quaternion3,
	}

	public class DataViewModel
	{
		public float Value { get; set; }
		public int Time { get; set; }
		public SensorTypeEnum SensorType { get; set; }
	}

	public class DataEventArgs : EventArgs
	{
		public DataViewModel SensorData;
	}
}