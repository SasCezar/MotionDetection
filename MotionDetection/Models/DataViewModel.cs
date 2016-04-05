using System;

namespace MotionDetection.Models
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
		Quaternion3
	}

	public struct DataViewModel
	{
		// TODO? Implement SensorNumber
		public float Value { get; set; }
		public int Time { get; set; }
		public SensorTypeEnum SensorType { get; set; }
	}

	// TODO? Convert to and array of SensorData
	public class DataEventArgs : EventArgs
	{
		public DataViewModel SensorData;
	}
}