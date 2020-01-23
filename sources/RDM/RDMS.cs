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

            if (length <= 3)
                throw new Exception("Number of receivers must be greater than 3");
            else if (length == 4)
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
        /// <summary>
        /// Solves the navigation problem by the range-difference method (linear method).
        /// </summary>
        /// <param name="A">Matrix of five receivers</param>
        /// <param name="T">Vector of time</param>
        /// <returns>Vector { X, Y, Z }</returns>
        public static double[] RDM5(double[][] A, double[] T)
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
            double[] V = new double[4];
            double[][] B;
            double[] F, S;

            // Roots
            for (int i = 0; i < maxIterations; i++)
            {
                B = RDMS.Left(A, T, V);
                F = RDMS.Right(A, T, V);
                S = Vector.Solve(B, F);
                V = Vector.Add(V, S);

                // Stop point
                if (Convergence(S, eps))
                    break;
            }

            // Vector { X, Y, Z }
            return new double[] { V[0], V[1], V[2] };
        }
        /// <summary>
        /// Convergence.
        /// </summary>
        /// <param name="S">Addition vector</param>
        /// <param name="eps">Epsilon (0, 1)</param>
        /// <returns></returns>
        private static bool Convergence(double[] S, double eps = 1e-8)
        {
            int length = S.Length;
            bool b = true;

            for (int i = 0; i < length; i++)
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
            // decompose
            double[] R0 = A[0];
            double[] R1 = A[1];
            double[] R2 = A[2];
            double[] R3 = A[3];

            // distance
            double rk = Vector.Distance(R0, Rk);

            // vectors
            double[] H1 = new double[] { R1[0] - R0[0], R1[1] - R0[1], R1[2] - R0[2], RDMS.C * (T[1] - T[0]) };
            double[] H2 = new double[] { R2[0] - R0[0], R2[1] - R0[1], R2[2] - R0[2], RDMS.C * (T[2] - T[0]) };
            double[] H3 = new double[] { R3[0] - R0[0], R3[1] - R0[1], R3[2] - R0[2], RDMS.C * (T[3] - T[0]) };
            double[] H4 = new double[] { (R0[0] - Rk[0]) / rk, (R0[1] - Rk[1]) / rk, (R0[2] - Rk[2]) / rk, 1 };

            return new double[][] { H1, H2, H3, H4 };
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
            // decompose
            double[] R0 = A[0];
            double[] R1 = A[1];
            double[] R2 = A[2];
            double[] R3 = A[3];

            // modules
            double P0 = Math.Pow(Vector.Abs(R0), 2);
            double P1 = Math.Pow(Vector.Abs(R1), 2);
            double P2 = Math.Pow(Vector.Abs(R2), 2);
            double P3 = Math.Pow(Vector.Abs(R3), 2);

            // time delays
            double T0 = RDMS.C * (T[1] - T[0]);
            double T1 = RDMS.C * (T[2] - T[0]);
            double T2 = RDMS.C * (T[3] - T[0]);

            // distance
            double rk = Vector.Distance(R0, Rk);

            // vectors
            double F0 = Rk[0] * (R1[0] - R0[0]) + Rk[1] * (R1[1] - R0[1]) + Rk[2] * (R1[2] - R0[2]) - 0.5 * (P1 - P0 - T0 * T0) + T0 * rk;
            double F1 = Rk[0] * (R2[0] - R0[0]) + Rk[1] * (R2[1] - R0[1]) + Rk[2] * (R2[2] - R0[2]) - 0.5 * (P2 - P0 - T1 * T1) + T1 * rk;
            double F2 = Rk[0] * (R3[0] - R0[0]) + Rk[1] * (R3[1] - R0[1]) + Rk[2] * (R3[2] - R0[2]) - 0.5 * (P3 - P0 - T2 * T2) + T2 * rk;
            double F3 = 1.0 / rk;

            return new double[] { -F0, -F1, -F2, -F3 };
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
            double[] R0 = A[0];
            double[] R1 = A[1];
            double[] R2 = A[2];
            double[] R3 = A[3];
            double[] R4 = A[4];

            // vectors
            double[] H1 = new double[] { R0[0] - R1[0], R0[1] - R1[1], R0[2] - R1[2], -RDMS.C * (T[0] - T[1]) };
            double[] H2 = new double[] { R0[0] - R2[0], R0[1] - R2[1], R0[2] - R2[2], -RDMS.C * (T[0] - T[2]) };
            double[] H3 = new double[] { R0[0] - R3[0], R0[1] - R3[1], R0[2] - R3[2], -RDMS.C * (T[0] - T[3]) };
            double[] H4 = new double[] { R0[0] - R4[0], R0[1] - R4[1], R0[2] - R4[2], -RDMS.C * (T[0] - T[4]) };

            return new double[][] { H1, H2, H3, H4 };
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
            double[] R0 = A[0];
            double[] R1 = A[1];
            double[] R2 = A[2];
            double[] R3 = A[3];
            double[] R4 = A[4];

            // modules
            double P0 = Vector.Abs(R0);
            double P1 = Vector.Abs(R1);
            double P2 = Vector.Abs(R2);
            double P3 = Vector.Abs(R3);
            double P4 = Vector.Abs(R4);

            // time delays
            double T0 = RDMS.C * (T[1] - T[0]);
            double T1 = RDMS.C * (T[2] - T[0]);
            double T2 = RDMS.C * (T[3] - T[0]);
            double T3 = RDMS.C * (T[4] - T[0]);

            // vector
            double F0 = P0 * P0 - P1 * P1 + T0 * T0;
            double F1 = P0 * P0 - P2 * P2 + T1 * T1;
            double F2 = P0 * P0 - P3 * P3 + T2 * T2;
            double F3 = P0 * P0 - P4 * P4 + T3 * T3;

            return new double[] { F0 / 2, F1 / 2, F2 / 2, F3 / 2 };
        }
        #endregion

        #region Generative methods
        /// <summary>
        /// Returns a matrix of five receive points.
        /// </summary>
        /// <param name="vector">Vector { X, Y, Z }</param>
        /// <param name="scaling">Scaling vector { X, Y, Z }</param>
        /// <param name="sigma">Standard deviation</param>
        /// <param name="count">Number of receivers (>3)</param>
        /// <returns>Matrix</returns>
        public static double[][] GetReceivers(double[] vector, double[] scaling, double sigma = 0.5, int count = 5)
        {
            // params
            double X = vector[0];
            double Y = vector[1];
            double Z = vector[2];

            double dx = scaling[0];
            double dy = scaling[1];
            double dz = scaling[2];

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
