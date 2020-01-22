using System;

namespace RDM_CONSOLE
{
    /// <summary>
    /// Uses for console operations.
    /// </summary>
    public static class ConsoleHelper
    {
        #region Helper voids
        /// <summary>
        /// Writes line.
        /// </summary>
        /// <param name="v">Source</param>
        /// <param name="comment">Comment</param>
        public static void Disp(double v, string comment = "", string sep = "\n\n")
        {
            string s = comment + v.ToString() + sep;
            Console.Write(s);
            return;
        }
        /// <summary>
        /// Writes line.
        /// </summary>
        /// <param name="v">Source</param>
        /// <param name="comment">Comment</param>
        public static void Disp(double[] v, string comment = "", string sep = "\n\n")
        {
            string s = comment;

            for (int i = 0; i < v.Length; i++)
            {
                s += Math.Round(v[i], 6).ToString();

                if (i < v.Length - 1)
                    s += ", ";

            }

            s += sep;
            Console.Write(s);
            return;
        }
        /// <summary>
        /// Writes line.
        /// </summary>
        /// <param name="v">Source</param>
        /// <param name="comment">Comment</param>
        public static void Disp(double[][] v, string comment = "", string sep = "\n\n")
        {
            for (int i = 0; i < v.Length; i++)
            {
                Disp(v[i], comment, sep);
            }
            return;
        }
        #endregion
    }
}
