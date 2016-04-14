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

	public enum SensorNumber
	{
		Sensor1,
		Sensor2,
		Sensor3,
		Sensor4,
		Sensor5,
	}

	public class DataEventArgs : EventArgs
	{
		public SensorTypeEnum SensorType;
		public SensorNumber SensorNumber;
		public double[] SensorData;
		public int Time;
	}
}