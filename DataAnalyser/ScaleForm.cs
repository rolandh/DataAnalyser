using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataAnalyser
{
    public partial class ScaleForm : Form
    {

        public delegate void UpdateAxis(double [] str);

        UpdateAxis updateAxis;

        double[] newAxis;

        public ScaleForm(UpdateAxis callback, Double[] axis = null)
        {
            InitializeComponent();

            if (axis != null) if (axis.Length > 0) UpdateText(axis);

            this.updateAxis = callback;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double start;
            double end;
            double step;
            if(Double.TryParse(startMaskedTextBox.Text, out start) && Double.TryParse(endMaskedTextBox.Text, out end) && Double.TryParse(stepMaskedTextBox.Text, out step))
            {
                if((start < end) && (step >= 0.0))
                {
                    newAxis = GenerateAxis(start, end, step);
                    UpdateText(newAxis);
                    return;
                }
                
            }
            MessageBox.Show("Error: invalid entry", "Error");
        }

        public void UpdateText(Double [] axis)
        {
            if (axis == null)
            {
                axisTextBox.Text = "";
                return;
            }
            if(axis.Length == 0)
            {
                axisTextBox.Text = "";
                return;
            }
            string text = "";
            for(int i = 0; i < axis.Length; i++)
            {
                if (i != 0) text += ", ";

                text += String.Format("{0:0.00}", axis[i].ToString());
            }
            foreach (Double d in axis) 
            axisTextBox.Text = text;
            startMaskedTextBox.Text = axis.Min().ToString();
            endMaskedTextBox.Text = axis.Max().ToString();
            double offset = 0;
            if(axis.Min() < 0) offset = axis.Min();

            stepMaskedTextBox.Text = ((int)((axis.Max()- offset) / Math.Abs(axis.Min()))).ToString();
        }


        public static Double[] GenerateAxis(double start, double end, double step)
        {
            if ((end < start) || (step <= 0.0)) return null;

            double range = end - start;
            int entries = (int)(range / step) + 1;
            Double [] axis = new Double[Math.Min(entries, 50)];
            double val = start;
            for (int i = 0; i < Math.Min(entries, 50); i++) {
                axis[i] = val;
                val += step;
            }
            return axis;
        }

        public bool ParseAxis()
        {
            //Convert the text to a double array
            string[] elements = axisTextBox.Text.Split(',');
            if (elements == null) return false;
            if (elements.Length == 0) return false;

            double[] newValues = new double[elements.Length];

            int i = 0;
            foreach(string element in elements)
            {
                double val;
                if (!double.TryParse(element, out val)) return false;
                newValues[i] = val;
                i++;
            }

            newAxis = newValues;

            //Check the axis is sequential from left to right

            for(i = 1; i < newAxis.Length; i++) if (newAxis[i] <= newAxis[i - 1]) return false;

            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (ParseAxis()) {
                updateAxis(newAxis);
                this.Close();
            }
            else
            {
                DialogResult result = MessageBox.Show("Error Parsing Axis, would you like to try again?", "Error", MessageBoxButtons.YesNo);
                if (result != DialogResult.Yes) this.Close();
            }
        }
    }
}
