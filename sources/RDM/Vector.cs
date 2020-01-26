using System;

namespace RDM
{
    /// <summary>
    /// Uses for vector operations.
    /// </summary>
    public static class Vector
    {
        #region Abs function
        /// <summary>
        /// Returns vector module.
        /// </summary>
        /// <param name="vector">Vector { X, Y, Z }</param>
        /// <param name="squared">Squared or not</param>
        /// <returns>Value</returns>
        public static double Abs(double[] vector, bool squared = false)
        {
            int length = vector.Length;
            double r = 0;

            for (int i = 0; i < length; i++)
            {
                r += vector[i] * vector[i];
            }

            if (squared)
                return r;

            return Math.Sqrt(r);
        }
        /// <summary>
        /// Returns matrix module.
        /// </summary>
        /// <param name="matrix">Matrix of vectors { X, Y, Z }</param>
        /// <param name="squared">Squared or not</param>
        /// <returns>vector { X, Y, Z }</returns>
        public static double[] Abs(double[][] matrix, bool squared = false)
        {
            int length = matrix.Length;
            double[] c = new double[length];

            for (int i = 0; i < length; i++)
            {
                c[i] = Vector.Abs(matrix[i], squared);
            }

            return c;
        }
        #endregion

        #region Summary function
        /// <summary>
        /// Returns sum of vector.
        /// </summary>
        /// <param name="vector">Vector { X, Y, Z }</param>
        /// <returns>Value</returns>
        public static double Sum(double[] vector)
        {
            int length = vector.Length;
            double r = 0;

            for (int i = 0; i < length; i++)
            {
                r += vector[i];
            }

            return r;
        }
        /// <summary>
        /// Returns sum of matrix.
        /// </summary>
        /// <param name="matrix">Matrix of vectors { X, Y, Z }</param>
        /// <returns>Vector { X, Y, Z }</returns>
        public static double[] Sum(double[][] matrix)
        {
            int length = matrix.Length;
            double[] c = new double[length];

            for (int i = 0; i < length; i++)
            {
                c[i] = Vector.Sum(matrix[i]);
            }

            return c;
        }
        #endregion

        #region Add/Sub functions
        /// <summary>
        /// Returns summary of two vectors.
        /// </summary>
        /// <param name="a">First vector { X, Y, Z }</param>
        /// <param name="b">Second vector { X, Y, Z }</param>
        /// <returns>Value</returns>
        public static double[] Add(double[] a, double[] b)
        {
            int length = a.Length;
            double[] c = new double[length];

            for (int i = 0; i < length; i++)
            {
                c[i] = a[i] + b[i];
            }
            return c;
        }
        /// <summary>
        /// Returns summary of two matrices.
        /// </summary>
        /// <param name="a">First matrix of vectors { X, Y, Z }</param>
        /// <param name="b">Second matrix of vectors { X, Y, Z }</param>
        /// <returns>Vector { X, Y, Z }</returns>
        public static double[][] Add(double[][] a, double[][] b)
        {
            int length = a.Length;
            double[][] c = new double[length][];

            for (int i = 0; i < length; i++)
            {
                c[i] = Vector.Add(a[i], b[i]);
            }

            return c;
        }

        /// <summary>
        /// Returns difference of two vectors.
        /// </summary>
        /// <param name="a">First vector { X, Y, Z }</param>
        /// <param name="b">Second vector { X, Y, Z }</param>
        /// <returns>Value</returns>
        public static double[] Sub(double[] a, double[] b)
        {
            int length = a.Length;
            double[] c = new double[length];

            for (int i = 0; i < length; i++)
            {
                c[i] = a[i] - b[i];
            }
            return c;
        }
        /// <summary>
        /// Returns difference of two matrices.
        /// </summary>
        /// <param name="a">First matrix of vectors { X, Y, Z }</param>
        /// <param name="b">Second matrix of vectors { X, Y, Z }</param>
        /// <returns>Vector { X, Y, Z }</returns>
        public static double[][] Sub(double[][] a, double[][] b)
        {
            int length = a.Length;
            double[][] c = new double[length][];

            for (int i = 0; i < length; i++)
            {
                c[i] = Vector.Sub(a[i], b[i]);
            }

            return c;
        }
        #endregion

        #region Error/Loss/similarity functions
        /// <summary>
        /// Returns loss function of two vectors.
        /// </summary>
        /// <param name="a">First vector { X, Y, Z }</param>
        /// <param name="b">Second vector { X, Y, Z }</param>
        /// <returns>Value</returns>
        public static double Loss(double[] a, double[] b)
        {
            int length = a.Length;
            double s = 0;

            for (int i = 0; i < length; i++)
            {
                s += Math.Abs(a[i] - b[i]);
            }

            return s;
        }
        /// <summary>
        /// Returns loss function of two matrices.
        /// </summary>
        /// <param name="a">First matrix of vectors { X, Y, Z }</param>
        /// <param name="b">Second matrix of vectors { X, Y, Z }</param>
        /// <returns>Vector { X, Y, Z }</returns>
        public static double[] Loss(double[][] a, double[][] b)
        {
            int length = a.Length;
            double[] c = new double[length];

            for (int i = 0; i < length; i++)
            {
                c[i] = Vector.Loss(a[i], b[i]);
            }

            return c;
        }
        /// <summary>
        /// Returns accuracy function of two vectors.
        /// </summary>
        /// <param name="a">First vector { X, Y, Z }</param>
        /// <param name="b">Second vector { X, Y, Z }</param>
        /// <returns>Value</returns>
        public static double Accuracy(double[] a, double[] b)
        {
            double c = Vector.Abs(a) / Vector.Abs(b);

            if (c > 1.0)
                return 1.0 / c;

            return c;
        }
        /// <summary>
        /// Returns accuracy function of two matrices.
        /// </summary>
        /// <param name="a">First matrix of vectors { X, Y, Z }</param>
        /// <param name="b">Second matrix of vectors { X, Y, Z }</param>
        /// <returns>Vector { X, Y, Z }</returns>
        public static double[] Accuracy(double[][] a, double[][] b)
        {
            int length = a.Length;
            double[] c = new double[length];

            for (int i = 0; i < length; i++)
            {
                c[i] = Vector.Accuracy(a[i], b[i]);
            }

            return c;
        }
        /// <summary>
        /// Returns similarity function of two vectors.
        /// </summary>
        /// <param name="a">First vector { X, Y, Z }</param>
        /// <param name="b">Second vector { X, Y, Z }</param>
        /// <returns>Value</returns>
        public static double Similarity(double[] a, double[] b)
        {
            int length = a.Length;
            double A = Vector.Abs(a);
            double B = Vector.Abs(b);
            double s = 0;

            for (int i = 0; i < length; i++)
                s += a[i] * b[i];

            return s / (A * B);
        }
        /// <summary>
        /// Returns similarity function of two matrices.
        /// </summary>
        /// <param name="a">First matrix of vectors { X, Y, Z }</param>
        /// <param name="b">Second matrix of vectors { X, Y, Z }</param>
        /// <returns>Vector { X, Y, Z }</returns>
        public static double[] Similarity(double[][] a, double[][] b)
        {
            int length = a.Length;
            double[] c = new double[length];

            for (int i = 0; i < length; i++)
            {
                c[i] = Vector.Similarity(a[i], b[i]);
            }

            return c;
        }
        #endregion

        #region Mean function
        /// <summary>
        /// Returns mean vector of two vectors.
        /// </summary>
        /// <param name="a">First vector { X, Y, Z }</param>
        /// <param name="b">Second vector { X, Y, Z }</param>
        /// <returns>Vector { X, Y, Z }</returns>
        public static double[] Mean(double[] a, double[] b)
        {
            int length = a.Length;
            double[] c = new double[length];

            for (int i = 0; i < length; i++)
            {
                c[i] = (a[i] + b[i]) / 2.0;
            }

            return c;
        }
        /// <summary>
        /// Returns mean matrix of two matrices.
        /// </summary>
        /// <param name="a">First matrix of vectors { X, Y, Z }</param>
        /// <param name="b">Second matrix of vectors { X, Y, Z }</param>
        /// <returns>Vector { X, Y, Z }</returns>
        public static double[][] Mean(double[][] a, double[][] b)
        {
            int length = a.Length;
            double[][] c = new double[length][];

            for (int i = 0; i < length; i++)
            {
                c[i] = Vector.Mean(a[i], b[i]);
            }

            return c;
        }
        /// <summary>
        /// Returns mean value of vector.
        /// </summary>
        /// <param name="vector">First vector { X, Y, Z }</param>
        /// <returns>Value</returns>
        public static double Mean(double[] vector)
        {
            return Vector.Sum(vector) / (double)vector.Length;
        }
        /// <summary>
        /// Returns mean value of matrix.
        /// </summary>
        /// <param name="matrix">Matrix of vectors { X, Y, Z }</param>
        /// <returns>Vector { X, Y, Z }</returns>
        public static double[] Mean(double[][] matrix)
        {
            int length = matrix.Length;
            double[] c = new double[length];

            for (int i = 0; i < length; i++)
            {
                c[i] = Vector.Mean(matrix[i]);
            }

            return c;
        }
        #endregion

        #region Distance functions
        /// <summary>
        /// Returns distance between two vectors.
        /// </summary>
        /// <param name="a">First vector { X, Y, Z }</param>
        /// <param name="b">Second vector { X, Y, Z }</param>
        /// <returns>Value</returns>
        public static double Distance(double[] a, double[] b)
        {
            double[] c = Vector.Sub(a, b);
            return Vector.Abs(c);
        }
        /// <summary>
        /// Returns distance between two matrices.
        /// </summary>
        /// <param name="a">First matrix of vectors { X, Y, Z }</param>
        /// <param name="b">Second matrix of vectors { X, Y, Z }</param>
        /// <returns>Vector { X, Y, Z }</returns>
        public static double[] Distance(double[][] a, double[][] b)
        {
            int length = a.Length;
            double[] c = new double[length];

            for (int i = 0; i < length; i++)
            {
                c[i] = Vector.Distance(a[i], b[i]);
            }

            return c;
        }
        /// <summary>
        /// Returns distances between matrix and vector.
        /// </summary>
        /// <param name="matrix">Matrix of vectors { X, Y, Z }</param>
        /// <param name="vector">Vector { X, Y, Z }</param>
        /// <returns>Vector { X, Y, Z }</returns>
        public static double[] Distance(double[][] matrix, double[] vector)
        {
            int length = matrix.GetLength(0);
            double[] c = new double[length];

            for (int i = 0; i < length; i++)
            {
                c[i] = Vector.Distance(matrix[i], vector);
            }

            return c;
        }
        #endregion

        #region Resize function
        /// <summary>
        /// Resizes a vector.
        /// </summary>
        /// <param name="vector">Vector { X, Y, Z }</param>
        /// <param name="length">Length</param>
        /// <returns>Value</returns>
        public static double[] Resize(double[] vector, int length)
        {
            int r0 = Math.Min(vector.GetLength(0), length);
            double[] c = new double[length];

            for (int i = 0; i < r0; i++)
                c[i] = vector[i];

            return c;
        }
        /// <summary>
        /// Resizes a matrix.
        /// </summary>
        /// <param name="matrix">Matrix of vectors { X, Y, Z }</param>
        /// <param name="length">Length</param>
        /// <returns>Value</returns>
        public static double[][] Resize(double[][] matrix, int length)
        {
            int length0 = matrix.Length;
            double[][] c = new double[length0][];

            for (int i = 0; i < length0; i++)
            {
                c[i] = Vector.Resize(matrix[i], length);
            }

            return c;
        }
        #endregion

        #region Solver
        /// <summary>
        /// Returns a vector corresponding to the solution of a system of linear algebraic equations: "Ax = b".
        /// </summary>
        /// <param name="A">Square matrix</param>
        /// <param name="b">Array</param>
        /// <returns>Array</returns>
        public static double[] Solve(double[][] A, double[] b)
        {
            int height = A.GetLength(0);
            int width = A[0].GetLength(0);

            if (height != width)
                throw new Exception("The matrix must be square");
            if (height != b.Length)
                throw new Exception("Vector length should be equal to the height of the matrix");

            double[][] B = new double[height][];
            int i, j, k, l;
            double[] x = (double[])b.Clone();
            double[] v, w;
            double temp;

            for (i = 0; i < height; i++)
            {
                B[i] = new double[width];

                for (j = 0; j < width; j++)
                    B[i][j] = A[i][j];
            }

            for (i = 0; i < height; i++)
            {
                w = B[i];
                temp = w[i];

                for (j = 0; j < width; j++)
                {
                    w[j] /= temp;
                }
                x[i] /= temp;

                for (k = i + 1; k < height; k++)
                {
                    v = B[k];
                    temp = v[i];

                    for (j = i; j < width; j++)
                    {
                        v[j] = v[j] - w[j] * temp;
                    }

                    x[k] -= x[i] * temp;
                    B[k] = v;
                }
            }

            for (i = 0; i < height; i++)
            {
                l = (height - 1) - i;
                w = B[l];

                for (k = 0; k < l; k++)
                {
                    v = B[k];
                    temp = v[l];

                    for (j = l; j < width; j++)
                    {
                        v[j] = v[j] - w[j] * temp;
                    }

                    x[k] -= x[l] * temp;
                    B[k] = v;
                }

                B[l] = w;
            }

            return x;
        }
        #endregion
    }
}
