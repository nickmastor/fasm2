using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace thebuttoncrises
{
    public partial class Form1 : Form
    {
        int Q = 0;
        public Form1()
        {
            
            InitializeComponent();
            comboBox1.Items.Add("maze");
            comboBox1.Items.Add("time");
        }

        private void CartesianChart1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
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

            int size = 0;
            int start = 0;
            List<double> firstcolumn = new List<double>();
            List<double> secondcolumn = new List<double>();
            List<double> thirdcolumn = new List<double>();

            string[] lines = System.IO.File.ReadAllLines($"{filename}");
            // Display the file contents by using a foreach loop.
            System.Console.WriteLine("Contents of WriteLines2.txt1 = ");
            foreach (string line in lines)
            {
                // Use a tab to indent each line of the file.
                double[] b = new double[3];

                int g = 0;
                string a = "";
                if (line.Contains("Spectrum duration [us]:"))
                {      //to found the start value and the max value

                    a = "Spectrum duration [us]:";

                    MessageBox.Show(a);
                    //Console.WriteLine(g);

                }
                else if (line.Contains("Spectrum delay [us]:"))
                {
                    a = "Spectrum delay [us]:";

                    g = get_number(line, a);
                    // Console.WriteLine(g);
                };
                if (start == 1)
                {

                    b = get_the_elements(line, Q);                               ///start finding the numbers i have to store
                    firstcolumn.Add(b[Q]);
                    secondcolumn.Add(b[2]);
                    size++;
                    // Console.WriteLine($"{b[Q]} {b[2]}");
                };
                if (line.Contains("time[ns]"))
                {
                    start = 1;



                };

            }
                cartesianChart1.Series = new SeriesCollection
            {
                    new LineSeries 
               
                {

                    Values = new ChartValues<ObservablePoint>
                   {
                    
              
                      

                       },
                }

            }; for (int i = 0; i < (size - 1) / 100; i++)
                {
                    _ = cartesianChart1.Series[0].Values.Add(new ObservablePoint(firstcolumn[i], secondcolumn[i]));
                }
            
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

                if (ele.Equals(' '))
                {

                    c[l] = Convert.ToDouble(line1.Substring(j - k, k));
                    /*Console.WriteLine(c[l]);
                    Console.WriteLine("i am in");*/
                    k = 0;
                    l++;
                }
                else
                {

                    k++;
                };
                j++;
            };
            b = c;
            return b;


        }
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox1.SelectedItem == "maze")
            {
                Q = 1;
            }
            else if (comboBox1.SelectedItem == "time")
            {
                Q = 0;
            };

        }

        private void Chart1_Click(object sender, EventArgs e)
        {

        }
    }
}



