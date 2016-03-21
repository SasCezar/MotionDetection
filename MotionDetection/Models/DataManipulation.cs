using System;

namespace MotionDetection.Models {
	class DataManipulation {
		
		public static double[,] Modulo(CircularBufferMatrix<double> A) {
			double[,] B = new double[A.Height, A.Depth];
			for(int i = 0; i<A.Height; i++) {
				for (int j = 0; j<A.Depth; j++) {
					B[i, j] = Math.Sqrt(Math.Pow(A[0, i, j], 2) + Math.Pow(A[1, i, j], 2) + Math.Pow(A[2, i, j], 2));
				}
			}
			return B;
		}

		public static double[,,] Smoothing(CircularBufferMatrix<double> A) { // fatto con finestra di 10*2+1
			double[,,] B = new double[A.Width, A.Height, A.Depth];
			int s = 5; // smoothing size 2*s+1
			for(int i = 0; i<A.Width; i++) {
				for (int j = 0; j<A.Height; j++) {
					for (int k = 0; k<A.Depth; k++) {
						if (k>10 && (k+10)<A.Depth) {
							// o facciamo un bel for? k-10..k+10
							B[i, j, k] = 0;
							for (int tmp=-s; tmp<s; tmp++) {
								B[i, j, k] += A[i, j, k+tmp];
							}
							B[i, j, k] = B[i, j, k] / (s*2+1);
						} else {
							if (k<s) {
								for (int tmp = 0; tmp<k+s; tmp++) {
									B[i, j, k] += A[i, j, tmp];
								}
								B[i, j, k] = B[i, j, k] / (k+s);
							}
							else {
								for (int tmp = k - s; tmp<A.Depth; tmp++) {
									B[i, j, k] += A[i, j, tmp];
								}
								B[i, j, k] = B[i, j, k] / (A.Depth - k + s);
							}
						}
					}
				}
			}
			return B;
		}

		public static double[,] DifferenceQuotient(CircularBufferMatrix<double> A, int Sensor) { // A = sampwin[,] o 
			double[,] B = new double[A.Height, A.Depth];
			for(int i = 0; i<A.Height; i++) {
				for (int j = 0; j<A.Depth-1; j++) {
					B[i, j] = (A[Sensor, i, j + 1] - A[Sensor, i, j]) / A[Sensor, i, j];
				}
			}
			return B;
		}

		public static double Average(CircularBufferMatrix<double> A, int Sensor, int SensorNo) {
			double B = 0;
			for (int i = 0; i < A.Depth; i++) {
				B += A[Sensor, SensorNo, i];
			}
			B = B / A.Depth;
			return B;
		}

		public static double StdDev(CircularBufferMatrix<double> A, int Sensor, int SensorNo) {
			double B = 0;
			double Avg = Average(A, Sensor, SensorNo);
			for (int i= 0; i < A.Depth; i++) {
				B += Math.Pow(A[Sensor, SensorNo, i] - Avg, 2);
			}
			B = B / A.Depth;
			B = Math.Sqrt(B);
			return B;
		}

		public static double[,,] Eulero(CircularBufferMatrix<double> A) {
			/*
				0 = Roll (i.e. φ),
				1 = Pitch (i.e. θ),
				2 = Yaw (i.e. ψ).
			*/
			double[,,] B = new double[3,A.Height,A.Depth];
			for(int j = 0; j<A.Height; j++) {
				for(int k = 0; k<A.Depth; k++) {
					B[0, j, k] = Math.Atan((2* A[11, j, k]* A[12, j, k]+2* A[9, j, k]* A[10, j, k])/(2* A[9, j, k]* A[9, j, k]+2* A[12, j, k]* A[12, j, k]-1));
					B[1, j, k] = -Math.Asin(2* A[10, j, k]* A[12, j, k]-2* A[9, j, k]* A[11, j, k]);
					B[2, j, k] = Math.Atan((2* A[10, j, k]* A[11, j, k]+2* A[9, j, k]* A[12, j, k])/(2* A[9, j, k]* A[9, j, k]+2* A[10, j, k]* A[10, j, k]-1));
				}
			}
			return B;
		}
	}
}
