using System;

namespace RDM
{
    /// <summary>
    /// Uses to solve the navigation problem by the range-differential method with five receive points.
    /// </summary>
    public static class RDM5
    {
        #region Constants
        /// <summary>
        /// Speed of light.
        /// </summary>
        public const double C = 299792458;
        #endregion

        #region Private data
        /// <summary>
        /// Random generator.
        /// </summary>
        private static Random rand = new Random();
        #endregion

        #region Range-difference method
        /// <summary>
        /// Solves the navigation problem by the range-difference method.
        /// </summary>
        /// <param name="receivers">Matrix of five receivers</param>
        /// <param name="time">Vector of time</param>
        /// <returns>Vector { X, Y, Z }</returns>
        public static double[] Solve(double[][] receivers, double[] time)
        {
            // Solution
            double[][] B = RDM5.Left(receivers, time);
            double[] F = RDM5.Right(receivers, time);
            double[] S = Vector.Solve(B, F);

            // Vector { X, Y, Z }
            return new double[] { S[0], S[1], S[2] };
        }
        /// <summary>
        /// Returns a matrix of five receive points.
        /// </summary>
        /// <param name="vector">Vector { X, Y, Z }</param>
        /// <param name="scaling">Scaling vector { X, Y, Z }</param>
        /// <param name="sigma">Standard deviation</param>
        /// <returns>Matrix</returns>
        public static double[][] GetReceivers(double[] vector, double[] scaling, double sigma = 0.5)
        {
            // params
            double X = vector[0];
            double Y = vector[1];
            double Z = vector[2];

            double dx = scaling[0];
            double dy = scaling[1];
            double dz = scaling[2];

            // randoms
            double r0 = rand.NextDouble() + sigma;
            double r1 = rand.NextDouble() + sigma;
            double r2 = rand.NextDouble() + sigma;
            double r3 = rand.NextDouble() + sigma;
            double r4 = rand.NextDouble() + sigma;

            // multi-positioning of five receiving points
            double[] R0 = new double[] { X - dx * r0/**/, Y - dy * r0, Z - dz * r0 };
            double[] R1 = new double[] { X - dx * r1/**/, Y + dy * r1, Z - dz * r1 };
            double[] R2 = new double[] { X + dx * r2/**/, Y - dy * r2, Z - dz * r2 };
            double[] R3 = new double[] { X + dx * r3/**/, Y + dy * r3, Z - dz * r3 };
            double[] R4 = new double[] { X + dx * r4 * 2, Y + dy * r4, Z - dz * r4 };

            return new double[][] { R0, R1, R2, R3, R4 };
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
            return Vector.Distance(a, b) / RDM5.C;
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
                c[i] = RDM5.GetTime(receivers[i], vector);
            }

            return c;
        }
        #endregion

        #region Solver private methods
        /// <summary>
        /// Returns a matrix "A" of a system of linear algebraic equations: "Ax = b".
        /// </summary>
        /// <param name="A">Matrix of </param>
        /// <param name="T"></param>
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
            double[] H1 = new double[] { R0[0] - R1[0], R0[1] - R1[1], R0[2] - R1[2], -RDM5.C * (T[0] - T[1]) };
            double[] H2 = new double[] { R0[0] - R2[0], R0[1] - R2[1], R0[2] - R2[2], -RDM5.C * (T[0] - T[2]) };
            double[] H3 = new double[] { R0[0] - R3[0], R0[1] - R3[1], R0[2] - R3[2], -RDM5.C * (T[0] - T[3]) };
            double[] H4 = new double[] { R0[0] - R4[0], R0[1] - R4[1], R0[2] - R4[2], -RDM5.C * (T[0] - T[4]) };

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
            double T0 = RDM5.C * (T[1] - T[0]);
            double T1 = RDM5.C * (T[2] - T[0]);
            double T2 = RDM5.C * (T[3] - T[0]);
            double T3 = RDM5.C * (T[4] - T[0]);

            // vector
            double F0 = P0 * P0 - P1 * P1 + T0 * T0;
            double F1 = P0 * P0 - P2 * P2 + T1 * T1;
            double F2 = P0 * P0 - P3 * P3 + T2 * T2;
            double F3 = P0 * P0 - P4 * P4 + T3 * T3;

            return new double[] { F0 / 2, F1 / 2, F2 / 2, F3 / 2 };
        }
        #endregion
    }
}
