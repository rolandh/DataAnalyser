using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FordPCMEditor;
using System.IO;

namespace DataAnalyser
{
    public partial class XAxisMultiplierTextBox : Form
    {
        string[] csvHeaders;
        double[,] csvData;

        double[] MainGridViewxValues;
        double[] MainGridViewyValues;

        double[] secondaryXValues;
        double[] secondaryYValues;

        List<Double[]>[,] MainGridViewData;
        int[,] SecondaryGridViewData;

        public XAxisMultiplierTextBox()
        {
            InitializeComponent();
        }


        private void LoadButton_Click(object sender, EventArgs e)
        {

            var openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            if (result != DialogResult.OK) return;

            if(!LoadCsv(openFileDialog.FileName)) return;

            xAxisComboBox.Text = "";
            yAxisComboBox.Text = "";
            zAxisComboBox.Text = "";

            //Try and auto pick the axis
            foreach (string header in csvHeaders)
            {
                if (header.IndexOf("rpm", StringComparison.OrdinalIgnoreCase) >= 0) if(xAxisComboBox.Text.Length <= 0) xAxisComboBox.Text = header;
                if (header.IndexOf("cam", StringComparison.OrdinalIgnoreCase) >= 0) if (yAxisComboBox.Text.Length <= 0) yAxisComboBox.Text = header;
                if (header.IndexOf("trim", StringComparison.OrdinalIgnoreCase) >= 0) if (zAxisComboBox.Text.Length <= 0) zAxisComboBox.Text = header;
                if (header.IndexOf("absolute pressure", StringComparison.OrdinalIgnoreCase) >= 0) if (UComboBox.Text.Length <= 0) UComboBox.Text = header;
            }

            if(!String.IsNullOrEmpty(xAxisComboBox.Text) && !String.IsNullOrEmpty(yAxisComboBox.Text) && !String.IsNullOrEmpty(zAxisComboBox.Text))
            {
                PopulateMainGridView(xAxisComboBox.Text, yAxisComboBox.Text, zAxisComboBox.Text, UComboBox.Text);
            }
            
        }

        public void PopulateFilteredCellGridView(DataGridViewTuningCell data)
        {
            SecondaryGridView.Rows.Clear();
            SecondaryGridView.Columns.Clear();

            List<String> columns = new List<string>();
            double val = data.min;
            for (int x = 0; x < 10; x++) {
                columns.Add(String.Format("{0:0.00}", val));
                val += data.range / 10.0;
            }
            secondaryXValues = new double[10];


            int i = 0;
            SecondaryGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            foreach (string column in columns)
            {
                secondaryXValues[i] = Convert.ToDouble(column);

                DataGridViewTextBoxColumn cell = new DataGridViewTextBoxColumn();
                cell.HeaderText = column;
                cell.Name = column;
                cell.Width = 45;
                cell.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                SecondaryGridView.Columns.Add(cell);
                i++;
            }

            double minY = data.secondaryData.Min();
            double maxY = data.secondaryData.Max();

            double Range = maxY - minY;
            List<String> rows = new List<string>();
            val = minY;
            for (int y = 0; y < 10; y++)
            {
                rows.Add(String.Format("{0:0.00}", val));
                val += Range / 10.0;
            }

            secondaryYValues = new double[rows.Count];
            i = 0;
            foreach (string row in rows)
            {
                secondaryYValues[i] = Convert.ToDouble(row);
                i++;
            }
            SecondaryGridView.RowHeadersWidth = 60;

            SecondaryGridViewData = new int[data.secondaryData.Length, secondaryYValues.Length];
            for (i = 0; i < SecondaryGridView.Columns.Count; i++) for (int j = 0; j < secondaryYValues.Length; j++) SecondaryGridViewData[i, j] = 0;

            //Iterate each time sample and add it to the appropriate x,y list
            int min = int.MaxValue;
            int max = int.MinValue;

            for (i = 0; i < data.secondaryData.Length; i++)
            {
                double x = data.data[i];
                double y = data.secondaryData[i];

                int[] coordinates = GetNearestCoordinate(x, y, secondaryXValues, secondaryYValues);

                SecondaryGridViewData[coordinates[0], coordinates[1]]++;
            }

            for (i = 0; i < secondaryXValues.Length; i++)
            {
                for(int j = 0; j < secondaryYValues.Length; j++)
                {
                    if (SecondaryGridViewData[j, i] != 0){
                        if (SecondaryGridViewData[j, i] < min) min = SecondaryGridViewData[j, i];
                        if (SecondaryGridViewData[j, i] > max) max = SecondaryGridViewData[j, i];
                    }
                }
                
            }


            //Create a series of sums per x,y reference

            for (i = 0; i < secondaryYValues.Length; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.HeaderCell = new DataGridViewRowHeaderCell();
                row.HeaderCell.Value = secondaryYValues[i].ToString();
                for (int j = 0; j < secondaryXValues.Length; j++)
                {
                    DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
                    if (SecondaryGridViewData[j, i] != 0)
                    {
                        cell.Value = SecondaryGridViewData[j, i].ToString();
                        double scalar = 360.0 / (max - min);
                        double value = scalar * (SecondaryGridViewData[j, i] - min);

                        double H = (0.4 * (360.0 - value)) - 10.0;
                        double S = 0.9;
                        double B = 0.92;

                        cell.Style.BackColor = HSVtoRGB(H, S, B); ;
                    } else
                    {
                        cell.Value = "";
                    }



                    row.Cells.Add(cell);
                }

                SecondaryGridView.Rows.Add(row);
            }
        }

        public bool MainXValuesAreNullOrEmpty()
        {
            if (MainGridViewxValues == null) return true;
            if (MainGridViewxValues.Length <= 0) return true;
            return false;
        }

        public bool MainYValuesAreNullOrEmpty()
        {
            if (MainGridViewyValues == null) return true;
            if (MainGridViewyValues.Length <= 0) return true;
            return false;
        }

        public void PopulateMainGridView(string xHeader, string yHeader, string zHeader, string vHeader, bool exactMatch = false)
        {
            SecondaryGridView.Rows.Clear();
            SecondaryGridView.Columns.Clear();
            MainGridView.Rows.Clear();

            if(MainXValuesAreNullOrEmpty()) UpdateMainXAxis(ScaleForm.GenerateAxis(250.0, 6000.0, 250.0));
            if (MainYValuesAreNullOrEmpty()) UpdateMainYAxis(ScaleForm.GenerateAxis(-10.0, 45.0, 5.0));

            int i = 0;

            int xIndex = -1;
            int yIndex = -1;
            int zIndex = -1;
            int vIndex = -1;
            int numberOfHeaders = csvData.GetLength(1);
            for (i = 0; i < numberOfHeaders; i++)
            {

                if (exactMatch) {
                    if (csvHeaders[i].Equals(xHeader)) xIndex = i;
                    if (csvHeaders[i].Equals(yHeader)) yIndex = i;
                    if (csvHeaders[i].Equals(zHeader)) zIndex = i;
                    if (csvHeaders[i].Equals(vHeader)) vIndex = i;
                }
                else
                {
                    if (csvHeaders[i].Contains(xHeader)) xIndex = i;
                    if (csvHeaders[i].Contains(yHeader)) yIndex = i;
                    if (csvHeaders[i].Contains(zHeader)) zIndex = i;
                    if (csvHeaders[i].Contains(vHeader)) vIndex = i;
                }
                if (yIndex != -1 && xIndex != -1 && zIndex != -1 && vIndex != -1) break;
            }

            //We couldn't find the indexs
            if (yIndex == -1 || xIndex == -1 || zIndex == -1 || vIndex == -1)
            {
                MessageBox.Show("Error: Couldn't find index in file", "Error");
                return;
            }

            int height = csvData.GetLength(1);
            MainGridViewData = new List<double[]>[MainGridViewxValues.Length, MainGridViewyValues.Length];
            for (i = 0; i < MainGridView.Columns.Count; i++) for (int j = 0; j < MainGridViewyValues.Length; j++) MainGridViewData[i, j] = new List<double[]>();

            double uMult = 1.0;
            double yMult = 1.0;

            try {
                uMult = Convert.ToDouble(UAxisMultiplier.Text);
            } catch (Exception)
            {
                uMult = 1.0;
            }

            try
            {
                yMult = Convert.ToDouble(YAxisMultiplierTextBox.Text);
            }
            catch (Exception)
            {
                yMult = 1.0;
            }

            //Iterate each time sample and add it to the appropriate x,y list
            int numberOfSamples = csvData.GetLength(0);
            double minZ = double.MaxValue;
            double maxZ = double.MinValue;
            double range = 0.0;
            for (i = 0; i < numberOfSamples; i++)
            {
                double x = csvData[i, xIndex];
                double y = csvData[i, yIndex] * yMult;
                double z = csvData[i, zIndex];
                double v = csvData[i, vIndex];

                if (z < minZ) minZ = z;
                int[] coordinates = GetNearestCoordinate(x, y, MainGridViewxValues, MainGridViewyValues);
                double[] cell = new double[2];
                cell[0] = z;
                cell[1] = v * uMult;
                cell[1] = v;
                MainGridViewData[coordinates[0], coordinates[1]].Add(cell);
            }

            //Calculate our max Z value
            for (i = 0; i < MainGridViewyValues.Length; i++)
            {
                for (int j = 0; j < MainGridViewxValues.Length; j++)
                {
                    List<double[]> list = MainGridViewData[j, i];
                    double[] doubleArray = new double[list.Count];
                    for (int k = 0; k < doubleArray.Length; k++) doubleArray[k] = list[k][0];
                    if (doubleArray.Length != 0)
                    {
                        double average = doubleArray.Average();
                        if (average > maxZ) maxZ = average;
                    }
                }
            }

            range = maxZ - minZ;

            for (i = 0; i < MainGridViewyValues.Length; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.HeaderCell = new DataGridViewRowHeaderCell();
                row.HeaderCell.Value = MainGridViewyValues[i].ToString();
                for (int j = 0; j < MainGridViewxValues.Length; j++)
                {
                    double[] primaryArray = new double[MainGridViewData[j, i].Count];
                    double[] secondaryArray = new double[MainGridViewData[j, i].Count];

                    for (int k = 0; k < primaryArray.Length; k++)
                    {
                        primaryArray[k] = MainGridViewData[j, i][k][0];
                        secondaryArray[k] = MainGridViewData[j, i][k][1];
                    }


                    DataGridViewTuningCell cell = new DataGridViewTuningCell(secondaryArray, primaryArray, range, minZ);

                    row.Cells.Add(cell);
                }

                
                MainGridView.Rows.Add(row);
            }
            MainGridView.RowHeadersWidth = 60;
        }

        public class DataGridViewTuningColumn : DataGridViewTextBoxColumn
        {
            public DataGridViewTuningColumn()
            {
                this.CellTemplate = new DataGridViewTuningCell();
            }
        }

        public class DataGridViewTuningCell : DataGridViewTextBoxCell
        {
            public double average;
            public double standardDeviation;
            public double min;
            public double max;
            public int count;
            public readonly DataGridViewTuningCellFormat cellFormat = DataGridViewTuningCellFormat.Uninitialized;
            public double[] data = new double[0];
            public double[] secondaryData = new double[0];
            public double range;
            public double offset;

            public DataGridViewTuningCell()
            {
            }

            public DataGridViewTuningCell(double [] fuelTrimDataArray, double [] doubleArray, double range, double offset, DataGridViewTuningCellFormat format = DataGridViewTuningCellFormat.Average)
            {
                this.data = doubleArray;
                this.secondaryData = fuelTrimDataArray;
                this.range = range;
                this.offset = offset;
                this.count = doubleArray.Length;
                SetFormat(format);

            }

            public enum DataGridViewTuningCellFormat
            {
                Uninitialized = -1,
                Average = 0,
                StandardDeviation = 1,
                Minimum = 2,
                Maximum = 3,
                Count = 4,
            }

            public void SetFormat(DataGridViewTuningCellFormat newFormat)
            {
                if (newFormat == cellFormat) return;

                this.Value = "";
                if (data.Length == 0) return;

                average = data.Average();
                min = data.Min();
                max = data.Max();

                double sumOfSquaresOfDifferences = data.Select(val => (val - average) * (val - average)).Sum();
                this.standardDeviation = Math.Sqrt(sumOfSquaresOfDifferences / data.Length);

                double dataVal = 0.0;

                if (newFormat == DataGridViewTuningCellFormat.Average) dataVal = average;
                else if (newFormat == DataGridViewTuningCellFormat.Minimum) dataVal = min;
                else if (newFormat == DataGridViewTuningCellFormat.Maximum) dataVal = max;
                else if (newFormat == DataGridViewTuningCellFormat.StandardDeviation) dataVal = standardDeviation;
                else if (newFormat == DataGridViewTuningCellFormat.Count) dataVal = count;

                if (!double.IsNaN(dataVal) && !double.IsPositiveInfinity(dataVal) && !double.IsNegativeInfinity(dataVal) && !double.IsInfinity(dataVal))
                {
                    this.Value = String.Format("{0:0.00}", dataVal);

                    double scalar = 360.0 / range;
                    double value = scalar * (dataVal - offset);

                    double H = (0.4 * (360.0 - value)) - 10.0;
                    double S = 0.9;
                    double B = 0.92;

                    this.Style.BackColor = HSVtoRGB(H, S, B); ;

                }

            }
        }

        public int[] GetNearestCoordinate(double x, double y, double[] xVals, double[] yVals)
        {
            int[] results = new int[2];
            int xSmallestErrorIndex = 0;
            double xSmallestError = double.MaxValue;
            for (int i = 0; i < xVals.Length; i++)
            {
                double error = Math.Abs(xVals[i] - x);
                if (error < xSmallestError)
                {
                    xSmallestError = error;
                    xSmallestErrorIndex = i;
                }
            }
            int ySmallestErrorIndex = 0;
            double ySmallestError = double.MaxValue;
            for (int i = 0; i < yVals.Length; i++)
            {
                double error = Math.Abs(yVals[i] - y);
                if (error < ySmallestError)
                {
                    ySmallestError = error;
                    ySmallestErrorIndex = i;
                }
            }
            results[0] = xSmallestErrorIndex;
            results[1] = ySmallestErrorIndex;
            return results;
        }

        // Load a CSV file into an array of rows and columns.
        // Assume there may be blank lines but every line has
        // the same number of fields.
        private bool LoadCsv(string filename)
        {
            // Get the file's text.
            try {
                string whole_file = System.IO.File.ReadAllText(filename);
                // Split into lines.
                whole_file = whole_file.Replace('\n', '\r');
                string[] lines = whole_file.Split(new char[] { '\r' },
                    StringSplitOptions.RemoveEmptyEntries);

                // See how many rows and columns there are.
                int num_rows = lines.Length;
                int num_cols = lines[0].Split(',').Length;

                // Allocate the data array.
                csvHeaders = new string[num_cols];
                csvData = new double[num_rows - 1, num_cols];

                // Load the array.
                for (int r = 0; r < (num_rows); r++)
                {
                    string[] line_r = lines[r].Split(',');
                    for (int c = 0; c < num_cols; c++)
                    {
                        if (r == 0) csvHeaders[c] = line_r[c];
                        else csvData[r - 1, c] = Convert.ToDouble(line_r[c]);
                    }
                }
                xAxisComboBox.Items.Clear();
                yAxisComboBox.Items.Clear();
                foreach (string header in csvHeaders)
                {
                    xAxisComboBox.Items.Add(header);
                    yAxisComboBox.Items.Add(header);
                    zAxisComboBox.Items.Add(header);
                    UComboBox.Items.Add(header);
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message, "Error");
                return false;
            }


        }

        public void UpdateMainXAxisAndRegenerate(Double[] axis)
        {
            UpdateMainXAxis(axis);
            PopulateMainGridView(xAxisComboBox.Text, yAxisComboBox.Text, zAxisComboBox.Text, UComboBox.Text);
        }

        public void UpdateMainYAxisAndRegenerate(Double[] axis)
        {
            UpdateMainYAxis(axis);
            PopulateMainGridView(xAxisComboBox.Text, yAxisComboBox.Text, zAxisComboBox.Text, UComboBox.Text);
        }

        public void UpdateSecondaryXAxisAndRegenerate(Double[] axis)
        {
            UpdateMainXAxis(axis);
            PopulateMainGridView(xAxisComboBox.Text, yAxisComboBox.Text, zAxisComboBox.Text, UComboBox.Text);
        }

        public void UpdateSecondaryYAxisAndRegenerate(Double[] axis)
        {
            UpdateMainYAxis(axis);
            PopulateMainGridView(xAxisComboBox.Text, yAxisComboBox.Text, zAxisComboBox.Text, UComboBox.Text);
        }

        public void UpdateMainXAxis(double[] axis)
        {
            MainGridViewxValues = axis;
            MainGridView.Columns.Clear();
            MainGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            int i = 0;
            foreach (double value in MainGridViewxValues)
            {
                DataGridViewTuningColumn cell = new DataGridViewTuningColumn();
                cell.HeaderText = value.ToString();
                cell.Name = value.ToString();
                cell.Width = 45;
                cell.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                MainGridView.Columns.Add(cell);
                i++;
            }
        }

        public void UpdateMainYAxis(double [] axis)
        {
            MainGridView.Rows.Clear();
            MainGridViewyValues = axis;
        }

        private void ChangeMainAxisButton_Click(object sender, EventArgs e)
        {
            ScaleForm scaleForm;
            if (MainGridView.ColumnCount > 0)
            {
                List<double> existingAxis = new List<double>();
                foreach(DataGridViewTuningColumn column in MainGridView.Columns)
                {
                    if (column.HeaderCell != null) {
                        if (column.HeaderCell.Value != null)
                        {
                            if (!String.IsNullOrEmpty(column.HeaderCell.Value.ToString()))
                            {
                                double val;
                                if (Double.TryParse(column.HeaderCell.Value.ToString(), out val)) existingAxis.Add(val);
                            }
                        }
                    }
                }
                double[] existingDoubles = existingAxis.ToArray();
                scaleForm = new ScaleForm(UpdateMainXAxisAndRegenerate, existingDoubles);
            }
            else
            {
                scaleForm = new ScaleForm(UpdateMainXAxisAndRegenerate);
            }

            scaleForm.Show();
        }



        private void ChangeMainYAxisButton_Click(object sender, EventArgs e)
        {
            ScaleForm scaleForm;
            if (MainGridView.ColumnCount > 0)
            {
                List<double> existingAxis = new List<double>();
                foreach (DataGridViewRow row in MainGridView.Rows)
                {
                    if (row.HeaderCell != null)
                    {
                        if (row.HeaderCell.Value != null)
                        {
                            if (!String.IsNullOrEmpty(row.HeaderCell.Value.ToString()))
                            {
                                double val;
                                if (Double.TryParse(row.HeaderCell.Value.ToString(), out val)) existingAxis.Add(val);
                            }
                        }
                    }
                }
                double[] existingDoubles = existingAxis.ToArray();
                scaleForm = new ScaleForm(UpdateMainYAxisAndRegenerate, existingDoubles);
            }
            else
            {
                scaleForm = new ScaleForm(UpdateMainYAxisAndRegenerate);
            }

            scaleForm.Show();
        }

        private void MainGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ScaleForm scaleForm = new ScaleForm(UpdateMainXAxisAndRegenerate);
            scaleForm.Show();
        }

        private void MainGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ScaleForm scaleForm = new ScaleForm(UpdateMainYAxisAndRegenerate);
            scaleForm.Show();
        }

        private void SecondaryGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ScaleForm scaleForm = new ScaleForm(UpdateMainYAxisAndRegenerate);
            scaleForm.Show();
        }

        private void SecondaryGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ScaleForm scaleForm = new ScaleForm(UpdateSecondaryYAxisAndRegenerate);
            scaleForm.Show();
        }

        private void minButton_Click(object sender, EventArgs e)
        {
            double min = double.MaxValue;
            double max = double.MinValue;
            foreach (DataGridViewRow row in MainGridView.Rows)
            {
                foreach (DataGridViewTuningCell cell in row.Cells)
                {
                    if (cell.data.Length > 0)
                    {
                        if (cell.min < min) min = cell.min;
                        if (cell.min > max) max = cell.min;
                    }
                }
            }
            foreach (DataGridViewRow row in MainGridView.Rows)
            {
                foreach (DataGridViewTuningCell cell in row.Cells)
                {
                    cell.range = max - min;
                    cell.offset = min;
                    cell.SetFormat(DataGridViewTuningCell.DataGridViewTuningCellFormat.Minimum);
                }
            }
        }

        private void maxButton_Click(object sender, EventArgs e)
        {
            double min = double.MaxValue;
            double max = double.MinValue;
            foreach (DataGridViewRow row in MainGridView.Rows)
            {
                foreach (DataGridViewTuningCell cell in row.Cells)
                {
                    if (cell.data.Length > 0)
                    {
                        if (cell.max < min) min = cell.max;
                        if (cell.max > max) max = cell.max;
                    }
                }
            }
            foreach (DataGridViewRow row in MainGridView.Rows)
            {
                foreach (DataGridViewTuningCell cell in row.Cells)
                {
                    cell.range = max - min;
                    cell.offset = min;
                    cell.SetFormat(DataGridViewTuningCell.DataGridViewTuningCellFormat.Maximum);
                }
            }
        }

        private void countButton_Click(object sender, EventArgs e)
        {
            double min = double.MaxValue;
            double max = double.MinValue;
            foreach (DataGridViewRow row in MainGridView.Rows)
            {
                foreach (DataGridViewTuningCell cell in row.Cells)
                {
                    if (cell.data.Length > 0)
                    {
                        if (cell.count < min) min = (double)cell.count;
                        if (cell.count > max) max = (double)cell.count;
                    }
                }
            }
            foreach (DataGridViewRow row in MainGridView.Rows)
            {
                foreach (DataGridViewTuningCell cell in row.Cells)
                {
                    cell.range = max - min;
                    cell.offset = min;
                    cell.SetFormat(DataGridViewTuningCell.DataGridViewTuningCellFormat.Count);
                }
            }
        }

        private void SDButton_Click(object sender, EventArgs e)
        {
            //Update the range for the colour calculation
            double min = double.MaxValue;
            double max = double.MinValue;
            foreach (DataGridViewRow row in MainGridView.Rows)
            {
                foreach (DataGridViewTuningCell cell in row.Cells)
                {
                    if(cell.data.Length > 0){
                        if (cell.standardDeviation < min) min = cell.standardDeviation;
                        if (cell.standardDeviation > max) max = cell.standardDeviation;
                    }
                }
            }

            foreach (DataGridViewRow row in MainGridView.Rows)
            {
                foreach (DataGridViewTuningCell cell in row.Cells)
                {
                    cell.range = max - min;
                    cell.offset = min;
                    cell.SetFormat(DataGridViewTuningCell.DataGridViewTuningCellFormat.StandardDeviation);
                }
            }
        }

        private void averageButton_Click(object sender, EventArgs e)
        {
            double min = double.MaxValue;
            double max = double.MinValue;
            foreach (DataGridViewRow row in MainGridView.Rows)
            {
                foreach (DataGridViewTuningCell cell in row.Cells)
                {
                    if (cell.data.Length > 0)
                    {
                        if (cell.average < min) min = cell.average;
                        if (cell.average > max) max = cell.average;
                    }


                }
            }
            foreach (DataGridViewRow row in MainGridView.Rows)
            {
                foreach (DataGridViewTuningCell cell in row.Cells)
                {
                    cell.range = max - min;
                    cell.offset = min;
                    cell.SetFormat(DataGridViewTuningCell.DataGridViewTuningCellFormat.Average);
                }
            }
        }

        private void checkPulseButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in MainGridView.Rows) foreach (DataGridViewTuningCell cell in row.Cells) if (cell.Selected) PopulateFilteredCellGridView(cell);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            PopulateMainGridView(xAxisComboBox.Text, yAxisComboBox.Text, zAxisComboBox.Text, UComboBox.Text);
        }

        public static Color HSVtoRGB(double h, double s, double v)
        {
            double r, g, b, f, p, q, t;
            int i = 0;
            if (s == 0)
            {
                return Color.FromArgb((byte)(v * 255.0), (byte)(v * 255.0), (byte)(v * 255.0));
            }
            h /= 60;            // sector 0 to 5
            i = (int)Math.Floor(h);
            f = h - i;          // factorial part of h
            p = v * (1.0 - s);
            q = v * (1.0 - s * f);
            t = v * (1.0 - s * (1.0 - f));
            switch ((int)i)
            {
                case 0:
                    r = v;
                    g = t;
                    b = p;
                    break;
                case 1:
                    r = q;
                    g = v;
                    b = p;
                    break;
                case 2:
                    r = p;
                    g = v;
                    b = t;
                    break;
                case 3:
                    r = p;
                    g = q;
                    b = v;
                    break;
                case 4:
                    r = t;
                    g = p;
                    b = v;
                    break;
                default:        // case 5:
                    r = v;
                    g = p;
                    b = q;
                    break;
            }
            return Color.FromArgb((byte)(r * 255.0), (byte)(g * 255.0), (byte)(b * 255.0));
        }

        private void calculateAirmassButton_Click(object sender, EventArgs e)
        {
            if (fordPCMHelper != null)
            {
                if (fordPCMHelper.fileLoaded)
                {
                    int indexOfPulse,indexOfCalculatedLoad, indexOfMAP, indexOfTPS, indexOfCamAngle, indexOfRPM, indexOfMapPerAirmass, indexOfMapPerZeroAirmass, indexOfAirMass, indexOfFuelMass, indexOfCalculatedInjectorPW, indexOfCalculatedAFR;


                    indexOfCamAngle = Array.FindIndex(csvHeaders, x => (x.IndexOf("cam angle", StringComparison.OrdinalIgnoreCase) >= 0));
                    indexOfRPM = Array.FindIndex(csvHeaders, x => (x.IndexOf("rpm", StringComparison.OrdinalIgnoreCase) >= 0));
                    indexOfTPS = Array.FindIndex(csvHeaders, x => (x.IndexOf("ETC throttle", StringComparison.OrdinalIgnoreCase) >= 0));
                    indexOfMAP = Array.FindIndex(csvHeaders, x => (x.IndexOf("MANIFOLD ABSOLUTE PRESSURE", StringComparison.OrdinalIgnoreCase) >= 0));
                    indexOfPulse = Array.FindIndex(csvHeaders, x => (x.IndexOf("FUEL PULSEWIDTH", StringComparison.OrdinalIgnoreCase) >= 0));

                    

                    if (indexOfMAP == -1)
                    {
                        MessageBox.Show("Couldn't find MAP entry in csv data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (indexOfCamAngle == -1)
                    {
                        MessageBox.Show("Couldn't find cam angle entry in csv data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (indexOfRPM == -1)
                    {
                        MessageBox.Show("Couldn't find rpm entry in csv data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (indexOfTPS == -1)
                    {
                        MessageBox.Show("Couldn't find TPS entry in csv data, we will assume commanded lambda is always 1.0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }

                    indexOfMapPerAirmass = Array.IndexOf(csvHeaders, "Map per Airmass");
                    indexOfMapPerZeroAirmass = Array.IndexOf(csvHeaders, "Map per Zero Airmass");
                    indexOfAirMass = Array.IndexOf(csvHeaders, "Calculated Airmass");
                    indexOfFuelMass = Array.IndexOf(csvHeaders, "Calculated Fuel Mass");
                    indexOfCalculatedInjectorPW = Array.IndexOf(csvHeaders, "Calculated Injector Pulsewidth");
                    indexOfCalculatedAFR = Array.IndexOf(csvHeaders, "Calculated Commanded AFR");
                    indexOfCalculatedLoad = Array.IndexOf(csvHeaders, "Calculated Load");

                    int arrayResizeAmount = 0;
                    if (indexOfMapPerAirmass == -1)
                    {
                        indexOfMapPerAirmass = csvHeaders.Length;
                        arrayResizeAmount++;
                    }

                    if (indexOfMapPerZeroAirmass == -1)
                    {
                        indexOfMapPerZeroAirmass = csvHeaders.Length + arrayResizeAmount;
                        arrayResizeAmount++;
                    }
                    if (indexOfAirMass == -1)
                    {
                        indexOfAirMass = csvHeaders.Length + arrayResizeAmount;
                        arrayResizeAmount++;
                    }
                    if (indexOfFuelMass == -1)
                    {
                        indexOfFuelMass = csvHeaders.Length + arrayResizeAmount;
                        arrayResizeAmount++;
                    }
                    if (indexOfCalculatedInjectorPW == -1)
                    {
                        indexOfCalculatedInjectorPW = csvHeaders.Length + arrayResizeAmount;
                        arrayResizeAmount++;
                    }
                    if (indexOfCalculatedAFR == -1)
                    {
                        indexOfCalculatedAFR = csvHeaders.Length + arrayResizeAmount;
                        arrayResizeAmount++;
                    }
                    if (indexOfCalculatedLoad == -1)
                    {
                        indexOfCalculatedLoad = csvHeaders.Length + arrayResizeAmount;
                        arrayResizeAmount++;
                    }


                    //Create new arrays with 3 extra values,
                    string[] newCsvHeaders = new string[csvHeaders.Length + arrayResizeAmount];
                    double[,] newCsvData = new double[csvData.GetLength(0), csvData.GetLength(1)+ arrayResizeAmount];

                    //Copy the previous data
                    Array.Copy(csvHeaders, newCsvHeaders, csvHeaders.Length);
                    Array.Copy(csvData, newCsvData, csvData.Length);

                    //Add the new headers
                    newCsvHeaders[indexOfMapPerAirmass] = "Map per Airmass";
                    newCsvHeaders[indexOfMapPerZeroAirmass] = "Map per Zero Airmass";
                    newCsvHeaders[indexOfAirMass] = "Calculated Airmass";
                    newCsvHeaders[indexOfFuelMass] = "Calculated Fuel Mass";
                    newCsvHeaders[indexOfCalculatedInjectorPW] = "Calculated Injector Pulsewidth";
                    newCsvHeaders[indexOfCalculatedAFR] = "Calculated Commanded AFR";
                    newCsvHeaders[indexOfCalculatedLoad] = "Calculated Load";

                    //get stoich
                    double stoichAFR;
                    if(!fordPCMHelper.TryGetDouble(2300, out stoichAFR))
                    {
                        MessageBox.Show("Couldn't read Stoich AFR value (2300), we will assume 14.64", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        stoichAFR = 14.64;
                    }

                    Boolean calculatePulseWidth = true;

                    //get low slope
                    double lowSlope;
                    if (!fordPCMHelper.TryGetDouble(12010, out lowSlope))
                    {
                        MessageBox.Show("Couldn't read Injector Low Slope (12010), we will not be able to calculate pulse width!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        calculatePulseWidth = false;
                    }
                    lowSlope /= 3600.0;

                    //get high slope
                    double highSlope;
                    if (!fordPCMHelper.TryGetDouble(12011, out highSlope))
                    {
                        MessageBox.Show("Couldn't read Injector High Slope (12010), we will not be able to calculate pulse width!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        calculatePulseWidth = false;
                    }
                    highSlope /= 3600.0;

                    //get offset
                    double breakpoint;
                    if (!fordPCMHelper.TryGetDouble(12012, out breakpoint))
                    {
                        MessageBox.Show("Couldn't read Injector breakpoint (12012), we will not be able to calculate pulse width!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        calculatePulseWidth = false;
                    }

                    //get offset
                    double offset;
                    if (!fordPCMHelper.TryGetDouble(32050, out offset))
                    {
                        MessageBox.Show("Couldn't read Injector offset at 12.0V (32050), we will not be able to calculate pulse width!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        //calculatePulseWidth = false;
                        offset = 0.00146499997936189;
                    }


                    double displacement;
                    if (!fordPCMHelper.TryGetDouble(50000, out displacement))
                    {
                        MessageBox.Show("Couldn't read engine displacement (50000), we will assume 4.0L (0.00172 lb)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        displacement = 0.00172;
                    }

                    Boolean lambdaErrorIssued = false;

                    //Calculate the new values
                    for(int i = 0; i < csvData.GetLength(0); i++)
                    {
                        double camAngle = csvData[i, indexOfCamAngle];
                        double rpm = csvData[i,indexOfRPM];
                        double map = csvData[i, indexOfMAP];
                        double pulse = csvData[i, indexOfPulse];
                        double TPS = -1.0;
                        double commandedLambda = 1.0;

                        //Get the commanded lambda value
                        if(indexOfTPS != -1)
                        {
                            TPS = csvData[i, indexOfTPS];
                            if (!fordPCMHelper.TryGetTableValue(rpm, TPS, 50151, out commandedLambda))
                            {
                                if (!lambdaErrorIssued) MessageBox.Show(String.Format("Failed to get commanded lambda for {0}rpm {1}TPS%, we will use 1.0 from now on and no longer report anymore errors of this type", rpm, TPS));
                                lambdaErrorIssued = true;
                                commandedLambda = 1.0;
                            }
                        }

                        //interpolate the map per airmass values
                        double mapPerAirmass;
                        double mapPerZeroAirmass;
                        if(!fordPCMHelper.TryGetTableValue(rpm, camAngle, 50051, out mapPerZeroAirmass))
                        {
                            MessageBox.Show(String.Format("Failed to get Map per Zero Airmass for {0}rpm {1} Cam Angle, this is a fatal error, giving up, sorry bro", rpm, camAngle));
                            return;
                        }
                        if (!fordPCMHelper.TryGetTableValue(rpm, camAngle, 50055, out mapPerAirmass))
                        {
                            MessageBox.Show(String.Format("Failed to get Map per Airmass for {0}rpm {1} Cam Angle, this is a fatal error, giving up, sorry bro", rpm, camAngle));
                            return;
                        }

                        //Calculate the airmass
                        double airMassLbs = (map - mapPerZeroAirmass) / mapPerAirmass;
                        newCsvData[i,indexOfAirMass] = airMassLbs;

                        //Calculate the load
                        newCsvData[i, indexOfCalculatedLoad] = airMassLbs / displacement;

                        //Calculate the fuelmass
                        double fuelMass = airMassLbs / (stoichAFR * commandedLambda);
                        newCsvData[i, indexOfFuelMass] = fuelMass;


                        //Calculate the pulse width
                        double pulseWidth;
                        //When below the breakpoint
                        //fuelmass=low*(ms-offset)
                        //ms=(fuelmass/low)+offset
                        if (fuelMass <= breakpoint)
                        {
                            pulseWidth = (fuelMass / lowSlope) + offset;
                        }
                        else
                        {
                            //fuelmass=high*(ms-(breakpoint*((1/low)-(1/high))+offset))
                            //ms=(breakpoint*((1/low)-(1/high))+(fuelmass/highslope)+offset
                            pulseWidth = (breakpoint * ((1 / lowSlope) - (1 / highSlope))) + (fuelMass / highSlope) + offset;
                        }

                        double pulseWidthMs = (pulseWidth-offset) * 1000.0 ;
                        double actualPulse = (pulse) * 1000.0;
                        newCsvData[i, indexOfCalculatedInjectorPW] = pulseWidth;

                        if(rpm > 4500)
                        {
                            int lol = 1;
                        }

                    }

                    return;
                } 
            }

            MessageBox.Show("You must load a .HPT file first!");
        }

        private void UComboBox_DragLeave(object sender, EventArgs e)
        {

        }
    }
}
