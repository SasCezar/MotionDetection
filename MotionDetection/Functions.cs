using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionDetection {
	class Functions {
		
		public static double[][] Modulo(double[][][] A) {
			double[][] B = new double[A[0].Length][];
			for(int i = 0; i<A[0].Length; i++) {
				for (int j = 0; j<A[0][i].Length; j++) {
					B[i][j] = Math.Sqrt(Math.Pow(A[0][i][j], 2) + Math.Pow(A[1][i][j], 2) + Math.Pow(A[2][i][j], 2));
				}
			}
			return B;
		}

		public static double[][][] Smoothing(double[][][] A) { // fatto con finestra di 10*2+1
			double[][][] B = new double[A.Length][][];
			for(int i = 0; i<A.Length; i++) {
				for (int j = 0; j<A[i].Length; j++) {
					for (int k = 0; k<A[i][j].Length; k++) {
						if (k>10 && (k+10)<A[i][j].Length) {
							// o facciamo un bel for? k-10..k+10
							B[i][j][k] = (B[i][j][k - 10] + B[i][j][k - 9] + B[i][j][k - 8] + B[i][j][k - 7] + B[i][j][k - 6] + B[i][j][k - 5] + B[i][j][k - 4] + B[i][j][k - 3] + B[i][j][k - 2] + B[i][j][k - 1] + B[i][j][k] + B[i][j][k + 10] + B[i][j][k + 9] + B[i][j][k + 8] + B[i][j][k + 7] + B[i][j][k + 6] + B[i][j][k + 5] + B[i][j][k + 4] + B[i][j][k + 3] + B[i][j][k + 2] + B[i][j][k + 1])/21;
						} else {
							if (k<10) {
								for (int tmp = 0; tmp<k+10; tmp++) {
									B[i][j][k] += B[tmp][j][k];
								}
								B[i][j][k] = B[i][j][k] / (k+10);
							}
							else {
								for (int tmp = k - 10; tmp<A[i][j].Length; tmp++) {
									B[i][j][k] += B[tmp][j][k];
								}
								B[i][j][k] = B[i][j][k] / (A[i][j].Length - k + 10);
							}
						}
					}
				}
			}
			return B;
		}

		public static double[][] RI(double[][] A) { // A = sampwin[][] o 
			double[][] B = new double[A.Length][];
			for(int i = 0; i<A.Length; i++) {
				for (int j = 0; j<A[i].Length-1; j++) {
					B[i][j] = (B[i][j + 1] - B[i][j]) / B[i][j];
				}
			}
			return B;
		}

		public static double Average(double[] A) {
			double B = 0;
			for (int i = 0; i < A.Length; i++) {
				B += A[i];
			}
			B = B / A.Length;
			return B;
		}

		public static double SD(double[] A) {
			double B = 0;
			double Avg = Average(A);
			for (int i= 0; i < A.Length; i++) {
				B = Math.Pow(A[i] - Avg, 2);
			}
			B = B / A.Length;
			B = Math.Sqrt(B);
			return B;
		}

		public static double[][][] Eulero(double[][][] A) {
			/*
				0 = Roll (i.e. φ),
				1 = Pitch (i.e. θ),
				2 = Yaw (i.e. ψ).
			*/
			double[][][] B = new double[3][][];
			for(int j = 0; j<A[0].Length; j++) {
				for(int k = 0; k<A[0][j].Length; k++) {
					B[0][j][k] = Math.Atan((2* A[11][j][k]* A[12][j][k]+2* A[9][j][k]* A[10][j][k])/(2* A[9][j][k]* A[9][j][k]+2* A[12][j][k]* A[12][j][k]-1));
					B[1][j][k] = -Math.Asin(2* A[10][j][k]* A[12][j][k]-2* A[9][j][k]* A[11][j][k]);
					B[2][j][k] = Math.Atan((2* A[10][j][k]* A[11][j][k]+2* A[9][j][k]* A[12][j][k])/(2* A[9][j][k]* A[9][j][k]+2* A[10][j][k]* A[10][j][k]-1));
				}
			}
			return B;
		}
	}
}
