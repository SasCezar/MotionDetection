namespace MotionDetection.Models
{
	public class CircularBuffer<T>
	{
		private readonly T[] _buffer;

		public CircularBuffer(int size)
		{
			Size = size;
			_buffer = new T[Size];
		}

		public int Size { get; set; }

		public T this[int position]
		{
			get { return _buffer[position%Size]; }
			set { _buffer[position%Size] = value; }
		}
	}
}