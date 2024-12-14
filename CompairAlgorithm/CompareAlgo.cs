using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Application = System.Windows.Forms.Application;

public class CompareAlgo : Form
{
    private TextBox textBox1;
    private Chart chart1;
    private Button button1;

    public CompareAlgo()
    {
        this.ClientSize = new System.Drawing.Size(650, 500);
        textBox1 = new TextBox { Location = new System.Drawing.Point(20, 20)}; // Default max n
        chart1 = new Chart { Location = new System.Drawing.Point(20, 60), Size = new System.Drawing.Size(600, 400) };
        button1 = new Button { Text = "Start", Location = new System.Drawing.Point(125, 20) };
        //chart1.Dock = DockStyle.Fill;

        button1.Click += new EventHandler(button1_Click);

        this.Controls.Add(textBox1);
        this.Controls.Add(chart1);
        this.Controls.Add(button1);

        // Initialize Chart
        chart1.ChartAreas.Add(new ChartArea());
        chart1.ChartAreas[0].AxisX.Title = "n";
        chart1.ChartAreas[0].AxisY.Title = "ms";
        chart1.Legends.Add(new Legend());
    }

    private void button1_Click(object sender, EventArgs e)
    {
        try
        {
            int maxn = int.Parse(textBox1.Text);  // Get the max n value from input
            chart1.Series.Clear();  // Clear the previous series

            // Add series for the three algorithms
            Series algo1 = chart1.Series.Add("Algo1");
            algo1.ChartType = SeriesChartType.Line;
            algo1.MarkerStyle = MarkerStyle.Circle;

            Series algo2 = chart1.Series.Add("Algo2");
            algo2.ChartType = SeriesChartType.Line;
            algo2.MarkerStyle = MarkerStyle.Circle;

            Series algo3 = chart1.Series.Add("Algo3");
            algo3.ChartType = SeriesChartType.Line;
            algo3.MarkerStyle = MarkerStyle.Circle;

            Stopwatch watch = new Stopwatch();  // Create a Stopwatch instance for timing
            Random rand = new Random();  // Random number generator

            // Loop through array sizes doubling each time (n *= 2) 
            for (int n = 1; n <= maxn; n *= 2)
            {
                int[] a = new int[n];  // Create an array of size n
                for (int i = 0; i < n; i++)
                {
                    a[i] = rand.Next(0, 10000);  // Populate array with random integers
                }

                // Time and record results for MaxDiff1
                watch.Reset();
                watch.Start();
                MaxDiff1(a);
                watch.Stop();
                algo1.Points.AddXY(n, watch.ElapsedMilliseconds);

                // Time and record results for MaxDiff2
                watch.Reset();
                watch.Start();
                MaxDiff2(a);
                watch.Stop();
                algo2.Points.AddXY(n, watch.ElapsedMilliseconds);

                // Time and record results for MaxDiff3
                watch.Reset();
                watch.Start();
                MaxDiff3(a);
                watch.Stop();
                algo3.Points.AddXY(n, watch.ElapsedMilliseconds);

                chart1.Update();  // Update the chart to reflect the new points
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message);
        }
    }

    // Brute-force algorithm
    private int MaxDiff1(int[] a)
    {
        int maxdiff = 0;
        for (int i = 0; i < a.Length; i++)
            for (int j = 0; j < a.Length; j++)
            {
                int diff = Math.Abs(a[i] - a[j]);
                if (diff > maxdiff)
                    maxdiff = diff;
            }
        return maxdiff;
    }

    // Optimized brute-force (without recalculating pairs)
    private int MaxDiff2(int[] a)
    {
        int maxdiff = 0;
        for (int i = 0; i < a.Length; i++)
            for (int j = i + 1; j < a.Length; j++)
            {
                int diff = Math.Abs(a[i] - a[j]);
                if (diff > maxdiff)
                    maxdiff = diff;
            }
        return maxdiff;
    }

    // Efficient linear time algorithm
    private int MaxDiff3(int[] a)
    {
        int min = int.MaxValue;
        int max = int.MinValue;
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] > max) max = a[i];
            if (a[i] < min) min = a[i];
        }
        return max - min;
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new CompareAlgo());
    }

    private void InitializeComponent()
    {
            this.SuspendLayout();
            // 
            // Visualization
            // 
            this.ClientSize = new System.Drawing.Size(568, 405);
            this.Name = "Visualization";
            this.ResumeLayout(false);

    }
}
