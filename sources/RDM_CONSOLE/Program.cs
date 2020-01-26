using RDM;
using System;
using System.Globalization;
using System.Threading;

namespace RDM_CONSOLE
{
    class Program
    {
        #region Private data
        static double[] X = new double[] { 80, 20, 100 };
        static double[] S = new double[] { 700, 800, 500 };
        static double sigma = 0.5;
        static int count = 4;
        #endregion

        #region Main
        static void Main()
        {
            // Initialize
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            // Conversion from geodetic coordinates to Cartesian
            ConsoleHelper.Disp(X, "Target (Geodetic): ", "\n");
            double[] Y = Navigation.FromGeodetic(X);
            ConsoleHelper.Disp(Y, "Target (Cartesian): ", "\n");
            ConsoleHelper.Disp(S, "Scaling (Cartesian): ", "\n");
            ConsoleHelper.Disp(sigma, "Sigma: ", "\n");
            ConsoleHelper.Disp(count, "Receivers count: ");

            // Computing receivers and time difference
            double[][] A = RDMS.GetReceiver(Y, S, sigma, count);
            double[] T = RDMS.GetTime(A, Y);
            ConsoleHelper.Disp(A, "Receiver: ", "\n");
            ConsoleHelper.Disp(T, "Time delays: ", "\n");

            RDMS rdm = new RDMS(1e-12);
            // Range-difference method solution
            double[] R = rdm.Solve(A, T);
            ConsoleHelper.Disp(R, "RDM (Cartesian): ");
            ConsoleHelper.Disp(Vector.Accuracy(R, Y), "Accuracy: ", "\n");
            ConsoleHelper.Disp(Vector.Similarity(R, Y), "Similarity: ", "\n");
            ConsoleHelper.Disp(Vector.Loss(R, Y), "Loss: ");

            // Backward conversion to geodetic coordinates
            double[] Z = Navigation.ToGeodetic(R);
            ConsoleHelper.Disp(Z, "RDM (Geodetic): ");
            Console.ReadKey();
        }
        #endregion
    }
}
