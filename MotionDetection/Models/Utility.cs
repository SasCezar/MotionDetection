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

	public class PlotEventArgs : EventArgs
	{
		public int SeriesType;
		public int SensorNumber;
		public double[] SensorData;
		public int Time;
	}

    public class BufferEventArgs<T> : EventArgs
    {
        public Buffer3DMatrix<T> Data;
        public int Time;
    }

	public static class Parameters
	{
		public static int CircularBufferSize = 75;
		public static int NumSensor = 13;
		public static int StaticBufferSize = CircularBufferSize*2/3;
		public static int NumUnity = 5;
	}

	public class Utils
	{
		public static int FirstIndex(int i, int width)
		{
			return i - width/2 > 0 ? i - width/2 : 0;
		}

		public static int LastIndex(int i, int width, int size)
		{
			if (i + width/2 < size)
			{
				return i + width/2;
			}
			return size - 1;
		}
	}
}