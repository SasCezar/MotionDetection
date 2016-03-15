namespace MotionDetection
{
	public class CircularBufferMatrix<T>
	{
		private readonly T[,,] _buffer;

		#region Getters

		public int Width { get; }

		public int Height { get; }

		public int Depth { get; }

		#endregion

		public T this[int x, int y, int z]
		{
			set { _buffer[x, y, z % Depth] = value; }
			get { return _buffer[x, y, z % Depth]; }
		}

		public CircularBufferMatrix(int width, int height, int depth)
		{
			Width = width;
			Height = height;
			Depth = depth;
			_buffer = new T[width, height, depth];
		}
	}
}