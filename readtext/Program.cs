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

        public Form1()
        {
            
            InitializeComponent();
            comboBox1.Items.Add("mass");
            comboBox1.Items.Add("time");
        
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

             
                int size = 0;

                List<double> firstcolumn = new List<double>();
                List<double> secondcolumn = new List<double>();

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
                    MessageBox.Show(a);
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
                        secondcolumn.Add(b[2]);
                        size++;
                        // Console.WriteLine($"{b[Q]} {b[2]}");
                    };
                    if (line.Contains("time[ns]"))
                    {
                        start = 1;



                    };



                }
                for (int i = 0; i < (size - 1) / 1; i++)
                {
                    dt.Rows.Add(firstcolumn[i], secondcolumn[i]);

                }
                chart1.DataSource = dt;
                chart1.Series["Series1"].XValueMember = "X_Value";
                chart1.Series["Series1"].YValueMembers = "Y_Value";
                chart1.Series["Series1"].ChartType = SeriesChartType.Line;
                chart1.ChartAreas[0].AxisY.LabelStyle.Format = "";

            

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

        
    }
}
