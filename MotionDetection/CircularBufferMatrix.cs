namespace MotionDetection
{
	public class CircularBufferMatrix<T>
	{
		private readonly T[,,] _buffer;

		public CircularBufferMatrix(int width, int height, int depth)
		{
			Width = width;
			Height = height;
			Depth = depth;
			_buffer = new T[width, height, depth];
		}

		public T this[int x, int y, int z]
		{
			set { _buffer[x, y, z%Depth] = value; }
			get { return _buffer[x, y, z%Depth]; }
		}

		#region Getters

		public int Width { get; }

		public int Height { get; }

		public int Depth { get; }

		#endregion
	}
}