using System;

namespace MotionDetection.Models
{
	public enum SensorType
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

	public enum SeriesType
	{
		ModAcceleration,
		ModGyroscopes,
		Turning,
		DeadReckoning,
	}

	public class DataEventArgs : EventArgs
	{
		public int SeriesType;
		public int SensorNumber;
		public double[] SensorData;
		public int Time;
	}

    public class MotionEventArgs : EventArgs
    {
        public Boolean[] MotionData;
        public int Time;
    }
}