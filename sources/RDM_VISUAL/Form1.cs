using RDM;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using ZedGraph;

namespace RDM_VISUAL
{
    public partial class Form1 : Form
    {
        #region Private data
        GraphPane pane;
        PointPairList list = new PointPairList();
        LineItem curve;
        double[]   V = new double[] { 2000, 1000, 8000 };
        double[]   S = new double[] { 1000, 1000, 1000 };
        double[][] A;
        double[]   R;
        double sigma = 0.5;
        #endregion

        #region Form methods
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Initialize
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            // ZedGraph
            this.pane = z1.GraphPane;
            pane.XAxis.IsShowGrid = true;
            pane.XAxis.GridDashOn = 10;
            pane.XAxis.GridDashOff = 5;

            pane.YAxis.IsShowGrid = true;
            pane.YAxis.GridDashOn = 10;
            pane.YAxis.GridDashOff = 5;

            pane.Title = "Range-difference method visualization";
            pane.XAxis.Title = "Coordinate X, m";
            pane.YAxis.Title = "Coordinate Y, m";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            // parsing
            try { sigma = double.Parse(textBox1.Text); }
            catch { label4.Text = "Invalid value, sigma = " + Math.Round(sigma, 6); }

            // fix receivers or not
            if (!checkBox1.Checked || A == null)
            {
                A = RDM5.GetReceivers(V, S, sigma);
            }
            else
            {
                V = RDM5.GetTarget(A, sigma);
            }
            R = RDM5.Solve(A, RDM5.GetTime(A, V));
            DrawGraph(A, V, R);
            DispSolution(A, V, R);
        }
        #endregion

        #region Private voids
        private void DrawGraph(double[][] receivers, double[] target, double[] solution)
        {
            // Clear graphPane
            pane.CurveList.Clear();

            // Receivers
            int dim = receivers.GetLength(0), i;
            list.Clear(); for (i = 0; i < dim; i++) list.Add(receivers[i][0], receivers[i][1]);
            curve = pane.AddCurve("Receiver", list, Color.Blue, SymbolType.Circle);
            curve.Line.IsVisible = false;
            curve.Symbol.Fill.Color = Color.Blue;
            curve.Symbol.Fill.Type = FillType.Solid;
            curve.Symbol.Size = 20;

            // Target
            list.Clear(); list.Add(target[0], target[1]);
            curve = pane.AddCurve("Target", list, Color.Red, SymbolType.Circle);
            curve.Line.IsVisible = false;
            curve.Symbol.Fill.Color = Color.Red;
            curve.Symbol.Fill.Type = FillType.Solid;
            curve.Symbol.Size = 25;

            // RDM solution
            list.Clear(); list.Add(solution[0], solution[1]);
            curve = pane.AddCurve("RDM", list, Color.Black, SymbolType.Circle);
            curve.Line.IsVisible = false;
            curve.Symbol.Fill.Color = Color.Yellow;
            curve.Symbol.Fill.Type = FillType.Solid;
            curve.Symbol.Size = 32;

            // Distances
            if (checkBox2.Checked)
            {
                for (i = 0; i < dim; i++)
                {
                    list.Clear();
                    list.Add(receivers[i][0], receivers[i][1]);
                    list.Add(target[0], target[1]);
                    curve = pane.AddCurve("", list, Color.Gray, SymbolType.None);
                    curve.Line.Style = DashStyle.Dash;
                    curve.Line.Width = 2;
                }
            }

            z1.AxisChange();
            z1.Invalidate();
        }

        private void DispSolution(double[][] receivers, double[] target, double[] solution)
        {
            richTextBox1.Text  = FormHelper.Disp(V, "Target: ");
            richTextBox1.Text += FormHelper.Disp(sigma, "Sigma: ");
            richTextBox1.Text += FormHelper.Disp(receivers, "Receiver: ");
            richTextBox1.Text += FormHelper.Disp(solution, "RDM: ");
            richTextBox1.Text += FormHelper.Disp(Vector.Accuracy(solution, target), "Accuracy: ");
            richTextBox1.Text += FormHelper.Disp(Vector.Loss(solution, target), "Loss: ", "");
        }
        #endregion
    }
}
