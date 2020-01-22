namespace RDM_VISUAL
{
    using System;

    /// <summary>
    /// Uses for form operations.
    /// </summary>
    public static class FormHelper
    {
        #region Helper voids
        /// <summary>
        /// Writes line.
        /// </summary>
        /// <param name="v">Source</param>
        /// <param name="comment">Comment</param>
        /// <param name="sep">Separator</param>
        public static string Disp(double v, string comment = "", string sep = "\n\n")
        {
            string s = comment + v.ToString() + sep;
            return s;
        }
        /// <summary>
        /// Writes line.
        /// </summary>
        /// <param name="v">Source</param>
        /// <param name="comment">Comment</param>
        /// <param name="sep">Separator</param>
        public static string Disp(double[] v, string comment = "", string sep = "\n\n")
        {
            string s = comment;

            for (int i = 0; i < v.Length; i++)
            {
                s += Math.Round(v[i], 6).ToString();

                if (i < v.Length - 1)
                    s += ", ";
            }

            s += sep;
            return s;
        }
        /// <summary>
        /// Writes line.
        /// </summary>
        /// <param name="v">Source</param>
        /// <param name="comment">Comment</param>
        /// <param name="sep">Separator</param>
        public static string Disp(double[][] v, string comment = "", string sep = "\n\n")
        {
            string s = "";

            for (int i = 0; i < v.Length; i++)
            {
                s += Disp(v[i], comment, sep);
            }
            return s;
        }
        #endregion
    }

}
