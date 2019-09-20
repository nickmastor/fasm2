using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace THEFINALTEST
{
    
    public partial class Form1 : Form
    {
        int Q = 0;

        private const float CZoomScale = 4f;
        private int FZoomLevel = 0;

        int rg = 1;

        public Form1()
        {
            
            InitializeComponent();
            comboBox1.Items.Add("mass");
            comboBox1.Items.Add("time");
            comboBox2.Items.Add("x1");
            comboBox2.Items.Add("x4");
            comboBox2.Items.Add("x25");
            comboBox2.Items.Add("x100");
        }
        private void Button1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog test = new OpenFileDialog();
            
            test.Title = "open file";
            test.Filter = "text|*.txt";
            if (test.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {


                createChart(test.FileName, Q);
            }
        }
       
      
        private void createChart(string filename, int Q)
        {


            DataTable dt = new DataTable();
            dt.Columns.Add("X_Value", typeof(double));
            dt.Columns.Add("Y_Value", typeof(double));
            dt.Columns.Add("z", typeof(double));
            dt.Columns.Add("red", typeof(double));

            int size = 0;

            List<double> firstcolumn = new List<double>();
            List<double> secondcolumn = new List<double>();
            List<double> thirdcolumn = new List<double>();
            List<double> fourthcolumn = new List<double>();
            string[] lines = System.IO.File.ReadAllLines($"{filename}");
            // Display the file contents by using a foreach loop.
            // System.Console.WriteLine("Contents of WriteLines2.txt1 = ");
            int start = 0;
            foreach (string line in lines)
            {
                // Use a tab to indent each line of the file.
                double[] b = new double[3];
               
                int g = 0;
                string a = "";
                if (line.Contains("Spectrum duration [us]:"))
                {      //to found the start value and the max value

                    a = "Spectrum duration [us]:";
                  //  MessageBox.Show(a);
                    g = get_number(line, a);
                    //Console.WriteLine(g);

                };
                if (line.Contains("Spectrum delay [us]:"))
                {
                    a = "Spectrum delay [us]:";

                    g = get_number(line, a);
                    // Console.WriteLine(g);
                };
                if (start == 1)
                {

                    b = get_the_elements(line, Q);                               ///start finding the numbers i have to store

                    firstcolumn.Add(b[0]);
                    secondcolumn.Add(b[2] / 100000);
                    size++;
                    if (Math.Abs(b[2]) < (rg * 100000 / 256))
                    {
                        thirdcolumn.Add(((b[2] * rg) / 100000));
                        //MessageBox.Show(Convert.ToString(b[2] / 10000));
                    }
                    else { thirdcolumn.Add(0); }

                    if (thirdcolumn[size - 1] == 0)
                    {
                        fourthcolumn.Add(b[2] / 100000);

                    }
                    else
                    {
                        fourthcolumn.Add(((b[2] * rg) / 100000));
                    }

                    // Console.WriteLine($"{b[Q]} {b[2]}");
                };
                if (line.Contains("time[ns]"))
                {
                    start = 1;



                };



            }
            double max = secondcolumn.Max();
            double avg = secondcolumn.Average();
            double noisedeviationn = 0;
            double noisesize = 0;
            for (int j=0; j < size - 1; j++)
            {
                if (secondcolumn[j] < (max * 0.1 / 255))
                {
                    noisedeviationn = +(Math.Pow(secondcolumn[j] - avg, 2));
                    noisesize++;                //θεωρω οτι θορυβος ειναι κατω απο 0.1*Vmax/255  
                }
                };
            noisedeviationn = Math.Sqrt(noisedeviationn / noisesize);
            for (int i = 0; i < (size - 1) ; i++)
            {

                dt.Rows.Add(firstcolumn[i], secondcolumn[i], thirdcolumn[i], fourthcolumn[i]);

            }

            //this.chart1.Series["x1"].Points.AddXY = dt;
            chart1.DataSource = dt;



            /*chart1.DataSource. = { dt;,"x1"; }*/
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart1.Series["x1"].XValueMember = "X_Value";
            chart1.Series["x1"].YValueMembers = "Y_Value";
            chart1.Series["x1"].ChartType = SeriesChartType.Line;

            chart1.Series["xrg"].XValueMember = "X_Value";
            chart1.Series["xrg"].YValueMembers = "z";
            chart1.Series["xrg"].ChartType = SeriesChartType.Line;
            chart1.Series["final"].XValueMember = "X_Value";
            chart1.Series["final"].YValueMembers = "red";
            chart1.Series["final"].ChartType = SeriesChartType.Line;
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "";
            MessageBox.Show($"standard deviation ={noisedeviationn}");

        }

     
           
            private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox1.SelectedItem == "mass")
            {
                Q = 1;
            }
            else if (comboBox1.SelectedItem == "time")
            {
                Q = 0;
            };

        }

        public static int get_number(string line1, string notablePhrase)
        {

            string h = line1.Substring(notablePhrase.Length);
            int result = Convert.ToInt32(h);

            return result;



        }
        public static double[] get_the_elements(string line1, int a)
        {
            int j = 0, k = 0;
            int l = 0;

            double[] b = new double[3];
            double[] c = new double[3];
            foreach (char ele in line1)
            {

                if (ele.Equals(' ') || ele.Equals("\n") )
                {

                    c[l] = Convert.ToDouble(line1.Substring(j - k, k));
                    /*Console.WriteLine(c[l]);
                    Console.WriteLine("i am in");*/
                    k = 0;
                    l++;
                  
                }
                else
                {
                    if (l == 2)
                    {
                        c[l] = Convert.ToDouble(line1.Substring(j - k));
                    };
                    k++;
                };
                j++;
            };
            b = c;
            return b;


        }

        private void Chart1_Click(object sender, EventArgs e)
        {

        }

        private void Chart1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Axis ax = chart1.ChartAreas[0].AxisX;
            ax.ScaleView.Size = double.IsNaN(ax.ScaleView.Size) ?
                                (ax.Maximum - ax.Minimum) / 2 : ax.ScaleView.Size /= 2;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
         
        }

        private void Chart1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void Chart1_KeyDown(object sender, KeyEventArgs e)
        {
            Axis ax = chart1.ChartAreas[0].AxisX;
            ax.ScaleView.Size = double.IsNaN(ax.ScaleView.Size) ?
                                ax.Maximum : ax.ScaleView.Size *= 2;
            if (ax.ScaleView.Size > ax.Maximum - ax.Minimum)
            {
                ax.ScaleView.Size = ax.Maximum;
                ax.ScaleView.Position = 0;
            }
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox2.SelectedItem == "x1")
            {
                rg= 1;
            }
            else if (comboBox2.SelectedItem == "x4")
            {
                rg = 4;
            }else if(comboBox2.SelectedItem == "x25")
            {
                rg = 25;
            }else if (comboBox2.SelectedItem == "x100")
            {
                rg = 100;

            };
        }
    }
}
