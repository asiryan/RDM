using System;

namespace RDM
{
    /// <summary>
    /// Defines range-differential method solver.
    /// </summary>
    public class RDMS
    {
        #region Private data
        /// <summary>
        /// Speed of light.
        /// </summary>
        public const double C = 299792458;
        /// <summary>
        /// Epsilon (0, 1).
        /// </summary>
        private double eps;
        /// <summary>
        /// Random generator.
        /// </summary>
        private static Random rand = new Random();
        /// <summary>
        /// Maximum value of iterations.
        /// </summary>
        private const int maxIterations = short.MaxValue;
        #endregion

        #region Class components
        /// <summary>
        /// Initializes range-differential method solver.
        /// </summary>
        /// <param name="eps">Epsilon (0, 1)</param>
        public RDMS(double eps = 1e-8)
        {
            Eps = eps;
        }
        /// <summary>
        /// Gets or sets epsilon value (0, 1).
        /// </summary>
        public double Eps
        {
            get
            {
                return this.eps;
            }
            set
            {
                if (value <= 0 || value >= 1)
                    throw new Exception("Invalid argument value");

                this.eps = value;
            }
        }
        /// <summary>
        /// Solves the navigation problem by the range-difference method.
        /// </summary>
        /// <param name="receivers">Matrix of five receivers</param>
        /// <param name="time">Vector of time</param>
        /// <returns>Vector { X, Y, Z }</returns>
        public double[] Solve(double[][] receivers, double[] time)
        {
            int length = receivers.GetLength(0);

            if (length < 2)
                throw new Exception("Number of receivers must be greater than 1");
            else if (length < 5)
                return RDM4(receivers, time, this.eps);
            else if (length == 5)
                return RDM5(receivers, time);
            else
            {
                int dim = 5;
                double[][] packet = new double[dim][];
                double[]   timerp = new double[dim]; 

                for (int i = 0; i < dim; i++)
                {
                    packet[i] = receivers[i];
                    timerp[i] = time[i];
                }

                return RDM5(receivers, time);
            }
        }
        #endregion

        #region Solvers rdm4, rdm5
        // **************************************************************
        //                          ALGORITHMS
        // **************************************************************
        // * RDM implementation for 5 (or more) time-synchronized 
        // receivers by solving a linearized system of equations [1].
        // --------------------------------------------------------------
        // * RDM implementation for 2, 3 and 4 time-synchronized 
        // receivers by solving a nonlinear system of equations [2,3].
        // 
        // 
        // **************************************************************
        //                          REFERENCES
        // **************************************************************
        // [1] I.V. Grin, R.A. Ershov, O.A. Morozov, V.R. Fidelman - 
        // "Evaluation of radio source’s coordinates based on solution 
        // of linearized system of equations by range-difference method"
        // --------------------------------------------------------------
        // [2] V.B. Burdin, V.A. Tyurin, S.A. Tyurin, V.M. Asiryan - 
        // "The estimation of target positioning by means of the 
        // range-difference method"
        // --------------------------------------------------------------
        // [3] E.P. Voroshilin, M.V. Mironov, V.A. Gromov - 
        // "The estimation of radio source positioning by means of the 
        // range-difference method using the multiposition passive 
        // satellite system"
        // **************************************************************

        /// <summary>
        /// Solves the navigation problem by the range-difference method (linear method).
        /// </summary>
        /// <param name="A">Matrix of five receivers</param>
        /// <param name="T">Vector of time</param>
        /// <returns>Vector { X, Y, Z }</returns>
        private static double[] RDM5(double[][] A, double[] T)
        {
            // Roots
            double[][] B = RDMS.Left(A, T);
            double[] F = RDMS.Right(A, T);
            double[] S = Vector.Solve(B, F);

            // Vector { X, Y, Z }
            return new double[] { S[0], S[1], S[2] };
        }
        /// <summary>
        /// Solves the navigation problem by the range-difference method (nonlinear method).
        /// </summary>
        /// <param name="A">Matrix</param>
        /// <param name="T">Vector of time</param>
        /// <returns>Vector { X, Y, Z }</returns>
        /// <param name="eps">Epsilon (0, 1)</param>
        private static double[] RDM4(double[][] A, double[] T, double eps = 1e-8)
        {
            // Params
            int count = A.GetLength(0);
            double[] V = new double[count];
            double[][] B;
            double[] F, S;

            // Roots
            for (int i = 0; i < maxIterations; i++)
            {
                // Ax = b
                B = RDMS.Left(A, T, V);
                F = RDMS.Right(A, T, V);
                S = Vector.Solve(B, F);
                V = Vector.Add(V, S);

                // Stop point
                if (Convergence(S, eps))
                    break;
            }

            // Vector { X, Y, Z }
            return Vector.Resize(V, 3);
        }
        /// <summary>
        /// Convergence.
        /// </summary>
        /// <param name="S">Addition vector</param>
        /// <param name="eps">Epsilon (0, 1)</param>
        /// <returns>Stop or not</returns>
        private static bool Convergence(double[] S, double eps = 1e-8)
        {
            int length = S.Length;
            bool b = true;

            for (int i = 0; i < length - 1; i++)
            {
                if (Math.Abs(S[i]) > eps)
                {
                    b = false; break;
                }
            }

            return b;
        }
        #endregion

        #region Solver states
        /// <summary>
        /// Returns a matrix "A" of a system of linear algebraic equations: "Ax = b".
        /// </summary>
        /// <param name="A">Matrix of </param>
        /// <param name="T">Vector of time</param>
        /// <param name="Rk">Current solution</param>
        /// <returns>Matrix</returns>
        private static double[][] Left(double[][] A, double[] T, double[] Rk)
        {
            int count = A.GetLength(0);
            double[][] R = new double[count][];
            double[][] H = new double[count][];

            // decompose
            for (int i = 0; i < count; i++)
            {
                R[i] = Vector.Resize(A[i], count - 1);
            }

            // distance
            double rk = Vector.Distance(R[0], Rk);

            // compute matrix
            for (int i = 0; i < count - 1; i++)
            {
                H[i] = new double[count];

                for (int j = 0; j < count - 1; j++)
                {
                    H[i][j] = R[i + 1][j] - R[0][j];
                }

                H[i][count - 1] = RDMS.C * (T[i + 1] - T[0]);
            }

            H[count - 1] = new double[count];

            for (int i = 0; i < count - 1; i++)
            {
                H[count - 1][i] = (R[0][i] - Rk[i]) / rk;
            }
            H[count - 1][count - 1] = 1.0;
            return H;
        }
        /// <summary>
        /// Returns a vector "b" of a system of linear algebraic equations: "Ax = b".
        /// </summary>
        /// <param name="A">Matrix of the multi-positioning of five receiving points</param>
        /// <param name="T">Vector of time</param>
        /// <param name="Rk">Current solution</param>
        /// <returns>Vector</returns>
        private static double[] Right(double[][] A, double[] T, double[] Rk)
        {
            int count = A.GetLength(0);
            double[][] R = new double[count][];
            double[]   P = new double[count];

            // decompose
            for (int i = 0; i < count; i++)
            {
                R[i] = Vector.Resize(A[i], count - 1);
                P[i] = Math.Pow(Vector.Abs(R[i]), 2);
            }

            // time delays
            double[] dT = new double[count - 1];

            for (int i = 0; i < count - 1; i++)
            {
                dT[i] = RDMS.C * (T[i + 1] - T[0]);
            }

            // distance
            double rk = Vector.Distance(R[0], Rk);

            // vectors
            double[] F = new double[count];
            double dF;
            double dP;

            // compute vector
            for (int i = 0; i < count - 1; i++)
            {
                dF = 0;

                for (int j = 0; j < count - 1; j++)
                {
                    dF += Rk[j] * (R[i + 1][j] - R[0][j]);
                }

                dP = -0.5 * (P[i + 1] - P[0] - dT[i] * dT[i]) + dT[i] * rk;
                F[i] = -(dF + dP);
            }

            F[count - 1] = -1.0 / rk;
            return F;
        }
        /// <summary>
        /// Returns a matrix "A" of a system of linear algebraic equations: "Ax = b".
        /// </summary>
        /// <param name="A">Matrix of </param>
        /// <param name="T">Vector of time</param>
        /// <returns>Matrix</returns>
        private static double[][] Left(double[][] A, double[] T)
        {
            // decompose
            int count = A.GetLength(0);
            double[][] R = new double[count][];
            double[][] H = new double[count - 1][];

            for (int i = 0; i < count; i++)
                R[i] = A[i];

            // compute matrix
            for (int i = 0; i < count - 1; i++)
            {
                H[i] = new double[count - 1];

                for (int j = 0; j < count - 2; j++)
                {
                    H[i][j] = R[0][j] - R[i + 1][j];
                }

                H[i][count - 2] = -RDMS.C * (T[0] - T[i + 1]);
            }
            return H;
        }
        /// <summary>
        /// Returns a vector "b" of a system of linear algebraic equations: "Ax = b".
        /// </summary>
        /// <param name="A">Matrix of the multi-positioning of five receiving points</param>
        /// <param name="T">Vector of time</param>
        /// <returns>Vector</returns>
        private static double[] Right(double[][] A, double[] T)
        {
            // decompose
            int count = A.GetLength(0);
            double[][] R = new double[count][];
            double[][] H = new double[count - 1][];
            double[]   P = new double[count];
            double[]  dT = new double[count - 1];
            double[]   F = new double[count - 1];
            
            for (int i = 0; i < count; i++)
            {
                R[i] = A[i];
                P[i] = Vector.Abs(R[i]);
            }

            // compute matrix
            for (int i = 0; i < count - 1; i++)
            {
                dT[i] = RDMS.C * (T[i + 1] - T[0]);
            }

            for (int i = 0; i < count - 1; i++)
            {
                F[i] = 0.5 * (P[0] * P[0] - P[i + 1] * P[i + 1] + dT[i] * dT[i]);
            }

            return F;
        }
        #endregion

        #region Generative methods
        /// <summary>
        /// Returns a matrix of receive points.
        /// </summary>
        /// <param name="vector">Vector { X, Y, Z }</param>
        /// <param name="scaling">Scaling vector { X, Y, Z }</param>
        /// <param name="sigma">Standard deviation</param>
        /// <param name="count">Number of receivers (>1)</param>
        /// <returns>Matrix</returns>
        public static double[][] GetReceiver(double[] vector, double[] scaling, double sigma = 0.5, int count = 5)
        {
            // params
            double X = vector[0];
            double Y = vector[1];
            double Z = vector[2];

            double dx = scaling[0];
            double dy = scaling[1];
            double dz = scaling[2];

            // compute
            double[][] R = new double[count][];
            double r0;
            double r1;
            double r2;

            for (int i = 0; i < count; i++)
            {
                r0 = 2 * (rand.NextDouble() - 0.5);
                r1 = 2 * (rand.NextDouble() - 0.5);
                r2 = rand.NextDouble();

                R[i] = new double[] { X - dx * r0, Y - dy * r1, Z - dz * r2 };
            }

            return R;
        }
        /// <summary>
        /// Returns a target vector.
        /// </summary>
        /// <param name="receivers">Matrix of five receivers</param>
        /// <param name="sigma">Standard deviation</param>
        /// <returns>Vector { X, Y, Z }</returns>
        public static double[] GetTarget(double[][] receivers, double sigma = 0.5)
        {
            // params
            int dim = 3, i, j;
            int length = receivers.GetLength(0);
            double[] tar = new double[3];
            double[] max = new double[3];
            double[] min = new double[3];
            double[] a;
            double r;

            // min/max vectors
            for (i = 0; i < dim; i++)
            {
                max[i] = double.MinValue;
                min[i] = double.MaxValue;
            }

            // finding extremums
            for (i = 0; i < length; i++)
            {
                a = receivers[i];

                for (j = 0; j < dim; j++)
                {
                    if (a[j] > max[j]) max[j] = a[j];
                    if (a[j] < min[j]) min[j] = a[j];
                }
            }

            // randomize
            for (j = 0; j < dim; j++)
            {
                r = 2.0 * (rand.NextDouble() - 0.5);
                tar[j] = (max[j] - min[j]) * r * sigma + min[j] + (max[j] - min[j]) / 2.0;
            }

            return tar;
        }
        /// <summary>
        /// Returns the propagation time of a wave between two points.
        /// </summary>
        /// <param name="a">First vector { X, Y, Z }</param>
        /// <param name="b">Second vector { X, Y, Z }</param>
        /// <returns>Value</returns>
        public static double GetTime(double[] a, double[] b)
        {
            return Vector.Distance(a, b) / RDMS.C;
        }
        /// <summary>
        /// Returns the propagation time of a wave between mtrix of five receivers and target vector.
        /// </summary>
        /// <param name="receivers">Matrix of five receivers</param>
        /// <param name="vector">Vector { X, Y, Z }</param>
        /// <returns>Vector { T0, T1, T2, T3 }</returns>
        public static double[] GetTime(double[][] receivers, double[] vector)
        {
            int length = receivers.GetLength(0);
            double[] c = new double[length];

            for (int i = 0; i < length; i++)
            {
                c[i] = RDMS.GetTime(receivers[i], vector);
            }

            return c;
        }
        #endregion
    }
}
