using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionDetection.Models
{
	public class CircularBuffer<T>
	{
		public int Size { get; set; }

		private T[] _buffer;

		public CircularBuffer(int size)
		{
			Size = size;
			_buffer = new T[Size];
		}

		public T this[int position]
		{
			get { return _buffer[position%Size]; }
			set { _buffer[position%Size] = value; }
		}
	}
}