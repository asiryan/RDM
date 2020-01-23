using RDM;
using System;
using System.Globalization;
using System.Threading;

namespace RDM_CONSOLE
{
    class Program
    {
        static void Main()
        {
            // Initialize
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            // Conversion from geodetic coordinates to Cartesian
            double[] X = new double[] { 80, 20, 100 };
            ConsoleHelper.Disp(X, "Target (Geodetic): ", "\n");
            double[] Y = Coordinates.FromGeodetic(X);
            ConsoleHelper.Disp(Y, "Target (Cartesian): ", "\n");
            double[] S = new double[] { 1000, 1000, 1000 };
            ConsoleHelper.Disp(S, "Scaling (Cartesian): ", "\n");
            double sigma = 0.5;
            ConsoleHelper.Disp(sigma, "Sigma: ");

            // Computing receivers and time difference
            double[][] A = RDM5.GetReceivers(Y, S, sigma);
            double[] T = RDM5.GetTime(A, Y);
            ConsoleHelper.Disp(A, "Receiver: ", "\n");
            ConsoleHelper.Disp(T, "Time delays: ", "\n");

            // Range-difference method solution
            double[] R = RDM5.Solve(A, T);
            ConsoleHelper.Disp(R, "RDM (Cartesian): ");
            ConsoleHelper.Disp(Vector.Accuracy(R, Y), "Accuracy: ", "\n");
            ConsoleHelper.Disp(Vector.Loss(R, Y), "Loss: ");

            // Backward conversion to geodetic coordinates
            double[] Z = Coordinates.ToGeodetic(R);
            ConsoleHelper.Disp(Z, "RDM (Geodetic): ");
            Console.ReadKey();
        }
    }
}
