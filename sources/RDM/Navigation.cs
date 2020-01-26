using System;

namespace RDM
{
    /// <summary>
    /// Uses for transformations in navigation systems.
    /// </summary>
    public static class Navigation
    {
        #region Constants
        /// <summary>
        /// Speed of light.
        /// </summary>
        public const double C = 299792458;
        /// <summary>
        /// Major axis of an ellipsoid.
        /// </summary>
        public const double A = 6378137.0;
        /// <summary>
        /// Ellipsoid scaling.
        /// </summary>
        public const double Q = 1.0 / 298.3;
        /// <summary>
        /// E^2.
        /// </summary>
        public const double E2 = 2 * Q - Q * Q;
        /// <summary>
        /// Constant Pi.
        /// </summary>
        private const double pi = 3.1415926535;
        /// <summary>
        /// Pi / 180.0.
        /// </summary>
        private const double psi = pi / 180.0;
        /// <summary>
        /// 180.0 / Pi.
        /// </summary>
        private const double ksi = 180.0 / pi;
        #endregion

        #region Private data
        /// <summary>
        /// Epsilon.
        /// </summary>
        private const double eps = 1e-128;
        /// <summary>
        /// Maximum number of iterations.
        /// </summary>
        private const int maxIterations = 128;
        #endregion

        #region Geodetic
        /// <summary>
        /// Converts geodetic coordinates to Cartesian coordinates.
        /// </summary>
        /// <param name="B">Geodetic latitude</param>
        /// <param name="L">Geodetic longitude</param>
        /// <param name="H">Geodetic height</param>
        /// <param name="radians">Radians or degrees</param>
        /// <returns>Vector { X, Y, Z }</returns>
        public static double[] FromGeodetic(double B, double L, double H, bool radians = false)
        {
            // radians or degrees
            if (!radians)
            {
                B *= psi;
                L *= psi;
            }

            // params
            double sinB = Math.Sin(B);
            double cosB = Math.Cos(B);
            double cosL = Math.Cos(L);
            double sinL = Math.Sin(L);
            double sinB2 = sinB * sinB;
            double u = Math.Sqrt(1 - E2 * sinB2);
            double N = A / u;

            // decart
            double X = (N + H) * cosB * cosL;
            double Y = (N + H) * cosB * sinL;
            double Z = ((1 - E2) * N + H) * sinB;

            return new double[] { X, Y, Z };
        }
        /// <summary>
        /// Converts geodetic coordinates to Cartesian coordinates.
        /// </summary>
        /// <param name="vector">Vector { geodetic latitude (B), geodetic longitude (L), geodetic height (H) }</param>
        /// <param name="radians">Radians or degrees</param>
        /// <returns>Vector { X, Y, Z }</returns>
        public static double[] FromGeodetic(double[] vector, bool radians = false)
        {
            // exception
            if (vector.Length != 3)
                throw new Exception("Invalid vector format");

            return Navigation.FromGeodetic(vector[0], vector[1], vector[2], radians);
        }
        /// <summary>
        /// Converts Cartesian coordinates to geodetic coordinates.
        /// </summary>
        /// <param name="X">X-coordinate</param>
        /// <param name="Y">Y-coordinate</param>
        /// <param name="Z">Z-coordinate</param>
        /// <param name="radians">Radians or degrees</param>
        /// <returns>Vector { geodetic latitude (B), geodetic longitude (L), geodetic height (H) }</returns>
        public static double[] ToGeodetic(double X, double Y, double Z, bool radians = false)
        {
            // params
            double B = double.NaN;
            double L = double.NaN;
            double H = double.NaN;
            double D = Math.Sqrt(X * X + Y * Y);
            double U = Math.Sin(B);

            // geodetic
            if (D == 0)
            {
                B = pi / 2 * Z / Math.Abs(Z);
                L = 0;
                H = Z * U - A * Math.Sqrt(1 - E2 * U * U);
            }
            else
            {
                double La = Math.Abs(Math.Asin(Y / D));

                if (Y < 0 && X > 0)
                {
                    L = 2 * pi - La;
                }
                else if (Y < 0 && X < 0)
                {
                    L = pi + La;
                }
                else if (Y > 0 && X < 0)
                {
                    L = pi - La;
                }
                else if (Y > 0 && X > 0)
                {
                    L = La;
                }
                else if (Y == 0 && X > 0)
                {
                    L = 0;
                }
                else
                {
                    L = pi;
                }

                if (Z == 0)
                {
                    B = 0;
                    H = D - A;
                }
                else
                {
                    // new params
                    double R = Math.Sqrt(X * X + Y * Y + Z * Z);
                    double C = Math.Asin(Z / R);
                    double P = E2 * A / (2 * R);
                    double s1 = 0, s2;
                    double b = 0;
                    double d = double.MaxValue;
                    double u = double.NaN;
                    double v = double.NaN;
                    int i = 0;

                    while (d >= eps)
                    {
                        b = C + s1;
                        u = Math.Sin(b);
                        v = Math.Sin(b * 2);
                        s2 = Math.Asin(P * v / Math.Sqrt(1 - E2 * u * u));
                        d = Math.Abs(s2 - s1);
                        s1 = s2;
                        i++;

                        if (i >= maxIterations)
                            break;
                    }

                    v = Math.Cos(b);
                    B = b;
                    H = D * v + Z * u - A * Math.Sqrt(1 - E2 * u * u);
                }
            }

            // radians or degrees
            if (!radians)
            {
                B *= ksi;
                L *= ksi;
            }

            return new double[] { B, L, H };
        }
        /// <summary>
        /// Converts Cartesian coordinates to geodetic coordinates.
        /// </summary>
        /// <param name="vector">Vector { X, Y, Z }</param>
        /// <param name="radians">Radians or degrees</param>
        /// <returns>Vector { geodetic latitude (B), geodetic longitude (L), geodetic height (H) }</returns>
        public static double[] ToGeodetic(double[] vector, bool radians = false)
        {
            // exception
            if (vector.Length != 3)
                throw new Exception("Invalid vector format");

            return Navigation.ToGeodetic(vector[0], vector[1], vector[2], radians);
        }
        #endregion

        #region Cylindrical
        /// <summary>
        /// Converts cylindrical coordinates to Cartesian coordinates.
        /// </summary>
        /// <param name="X">X-coordinate</param>
        /// <param name="Y">Y-coordinate</param>
        /// <param name="Z">Z-coordinate</param>
        /// <param name="radians">Radians or degrees</param>
        /// <returns>Vector { radial distance (ρ), azimuthal angle (φ), height (z) }</returns>
        /// <returns></returns>
        public static double[] ToCylindrical(double X, double Y, double Z, bool radians = false)
        {
            // polar
            double R = Math.Sqrt(X * X + Y * Y);
            double P = Math.Atan2(Y, X) + pi * U0(-X) * Sgn(Y);

            // radians or degrees
            if (!radians)
            {
                P *= ksi;
            }

            return new double[] { R, P, Z };
        }
        /// <summary>
        /// Converts cylindrical coordinates to Cartesian coordinates.
        /// </summary>
        /// <param name="vector">Vector { X, Y, Z }</param>
        /// <param name="radians">Radians or degrees</param>
        /// <returns>Vector { radial distance (ρ), azimuthal angle (φ), height (z) }</returns>
        public static double[] ToCylindrical(double[] vector, bool radians = false)
        {
            // exception
            if (vector.Length != 3)
                throw new Exception("Invalid vector format");

            return Navigation.ToCylindrical(vector[0], vector[1], vector[2], radians);
        }
        /// <summary>
        /// Converts Cartesian coordinates to cylindrical coordinates.
        /// </summary>
        /// <param name="R">Radial distance (ρ)</param>
        /// <param name="Phi">Azimuthal angle (φ)</param>
        /// <param name="Z">Height (z)</param>
        /// <param name="radians">Radians or degrees</param>
        /// <returns>Vector { X, Y, Z }</returns>

        public static double[] FromCylindrical(double R, double Phi, double Z, bool radians = false)
        {
            // params
            double P = Phi;

            // radians or degrees
            if (!radians)
            {
                P *= psi;
            }

            // cartesian
            double X = R * Math.Cos(P);
            double Y = R * Math.Sin(P);

            return new double[] { X, Y, Z };
        }
        /// <summary>
        /// Converts Cartesian coordinates to cylindrical coordinates.
        /// </summary>
        /// <param name="vector">Vector { radial distance (ρ), azimuthal angle (φ), height (z) }</param>
        /// <param name="radians">Radians or degrees</param>
        /// <returns>Vector { X, Y, Z }</returns>

        public static double[] FromCylindrical(double[] vector, bool radians = false)
        {
            // exception
            if (vector.Length != 3)
                throw new Exception("Invalid vector format");

            return Navigation.FromCylindrical(vector[0], vector[1], vector[2], radians);
        }
        #endregion

        #region Spherical
        /// <summary>
        /// Converts spherical coordinates to Cartesian coordinates.
        /// </summary>
        /// <param name="X">X-coordinate</param>
        /// <param name="Y">Y-coordinate</param>
        /// <param name="Z">Z-coordinate</param>
        /// <param name="radians">Radians or degrees</param>
        /// <returns>Vector { radial distance (ρ), polar angle (θ), azimuthal angle (φ) }</returns>
        public static double[] ToSpherical(double X, double Y, double Z, bool radians = false)
        {
            // polar
            double R = Math.Sqrt(X * X + Y * Y + Z * Z);
            double T = Math.Acos(Z / R);
            double P = Math.Atan2(Y, X) + pi * U0(-X) * Sgn(Y);

            // radians or degrees
            if (!radians)
            {
                T *= ksi;
                P *= ksi;
            }

            return new double[] { R, T, P };
        }
        /// <summary>
        /// Converts spherical coordinates to Cartesian coordinates.
        /// </summary>
        /// <param name="vector">Vector { X, Y, Z }</param>
        /// <param name="radians">Radians or degrees</param>
        /// <returns>Vector { radial distance (ρ), polar angle (θ), azimuthal angle (φ) }</returns>
        public static double[] ToSpherical(double[] vector, bool radians = false)
        {
            // exception
            if (vector.Length != 3)
                throw new Exception("Invalid vector format");

            return Navigation.ToSpherical(vector[0], vector[1], vector[2], radians);
        }
        /// <summary>
        /// Converts Cartesian coordinates to spherical coordinates.
        /// </summary>
        /// <param name="R">Radial distance (ρ)</param>
        /// <param name="Thetta">Polar angle (θ)</param>
        /// <param name="Phi">Azimuthal angle (φ)</param>
        /// <param name="radians">Radians or degrees</param>
        /// <returns>Vector { X, Y, Z }</returns>

        public static double[] FromSpherical(double R, double Thetta, double Phi, bool radians = false)
        {
            // params
            double T = Thetta;
            double P = Phi;

            // radians or degrees
            if (!radians)
            {
                T *= psi;
                P *= psi;
            }

            // cartesian
            double X = R * Math.Sin(T) * Math.Cos(P);
            double Y = R * Math.Sin(T) * Math.Sin(P);
            double Z = R * Math.Cos(T);

            return new double[] { X, Y, Z };
        }
        /// <summary>
        /// Converts Cartesian coordinates to spherical coordinates.
        /// </summary>
        /// <param name="vector">Vector { radial distance (ρ), polar angle (θ), azimuthal angle (φ) }</param>
        /// <param name="radians">Radians or degrees</param>
        /// <returns>Vector { X, Y, Z }</returns>

        public static double[] FromSpherical(double[] vector, bool radians = false)
        {
            // exception
            if (vector.Length != 3)
                throw new Exception("Invalid vector format");

            return Navigation.FromSpherical(vector[0], vector[1], vector[2], radians);
        }
        #endregion

        #region Heavyside function
        /// <summary>
        /// Heavyside function with zero point.
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Function</returns>
        private static double U0(double x)
        {
            if (x < 0)
                return 0;
            else if (x == 0)
                return 0;

            return 1;
        }
        #endregion

        #region Sign function
        /// <summary>
        /// Sign function with zero point.
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Function</returns>
        private static double Sgn(double x)
        {
            if (x > 0)
                return 1;
            else if (x == 0)
                return 0;

            return -1;
        }
        #endregion
    }
}
