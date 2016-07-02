using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DataAnalyser
{
    public partial class MainForm : Form
    {
        string[] csvHeaders;
        double[][] csvData;

        double[] MainGridViewxValues;
        double[] MainGridViewyValues;

        double[] secondaryXValues;
        double[] secondaryYValues;


        List<Double[]>[,] MainGridViewData;
        int[,] SecondaryGridViewData;

        public MainForm()
        {
            InitializeComponent();
        }


        private void LoadButton_Click(object sender, EventArgs e)
        {


            var openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            if (result != DialogResult.OK) return;

            if(!HelperMethods.LoadCsv(openFileDialog.FileName, out csvHeaders, out csvData)) return;

            pcmSimulator1.csvData = null;
            pcmSimulator1.csvHeaders = null;

            xAxisComboBox.Items.Clear();
            yAxisComboBox.Items.Clear();
            UComboBox.Items.Clear();

            foreach (string header in csvHeaders)
            {
                xAxisComboBox.Items.Add(header);
                yAxisComboBox.Items.Add(header);
                zAxisComboBox.Items.Add(header);
                UComboBox.Items.Add(header);
            }


            //Try and auto pick the axis if they don't already have text in them
            foreach (string header in csvHeaders)
            {
                if (xAxisComboBox.Text.Length <= 0) if (header.IndexOf("rpm", StringComparison.OrdinalIgnoreCase) >= 0) if(xAxisComboBox.Text.Length <= 0) xAxisComboBox.Text = header;
                if (yAxisComboBox.Text.Length <= 0) if (header.IndexOf("cam", StringComparison.OrdinalIgnoreCase) >= 0) if (yAxisComboBox.Text.Length <= 0) yAxisComboBox.Text = header;
                if (zAxisComboBox.Text.Length <= 0) if (header.IndexOf("trim", StringComparison.OrdinalIgnoreCase) >= 0) if (zAxisComboBox.Text.Length <= 0) zAxisComboBox.Text = header;
                if (UComboBox.Text.Length <= 0) if (header.IndexOf("absolute pressure", StringComparison.OrdinalIgnoreCase) >= 0) if (UComboBox.Text.Length <= 0) UComboBox.Text = header;
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
            double val = data.data.Min();
            double range = data.data.Max() - val;
            int numberOfCells = Math.Min(10, data.data.Length);

            for (int x = 0; x < numberOfCells; x++)
            {
                columns.Add(String.Format("{0:0.00}", val));
                val += range / (double)numberOfCells;
            }
            secondaryXValues = new double[numberOfCells];


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
            minY = double.MaxValue;
            for (int index = 0; index < data.secondaryData.Length; index++) if (data.secondaryData[index] < minY) minY = data.secondaryData[index];

            double maxY = data.secondaryData.Max();

            double Range = maxY - minY;
            List<String> rows = new List<string>();
            val = minY;
            numberOfCells = Math.Min(10, data.secondaryData.Length);

            List<Double> rowDoubles = new List<Double>();

            for (int y = 0; y < numberOfCells; y++)
            {
                rows.Add(String.Format("{0:0.00}", val));
                rowDoubles.Add(val);
                val += Range / (double)numberOfCells;
            }

            secondaryYValues = new double[rows.Count];
            i = 0;
            foreach (double row in rowDoubles)
            {
                secondaryYValues[i] = row;
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
                for (int j = 0; j < secondaryYValues.Length; j++)
                {
                    if (SecondaryGridViewData[j, i] != 0)
                    {
                        if (SecondaryGridViewData[j, i] < min) min = SecondaryGridViewData[j, i];
                        if (SecondaryGridViewData[j, i] > max) max = SecondaryGridViewData[j, i];
                    }
                }

            }


            //Create a series of sums per x,y reference
            SecondaryGridView.RowHeadersWidth = 4;
            for (i = 0; i < secondaryYValues.Length; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.HeaderCell = new DataGridViewRowHeaderCell();
                row.HeaderCell.Value = secondaryYValues[i].ToString();
                int widthTemp = (row.HeaderCell.Value.ToString().Length + 3) * 10;
                if (widthTemp > SecondaryGridView.RowHeadersWidth) SecondaryGridView.RowHeadersWidth = widthTemp;

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

                        cell.Style.BackColor = HelperMethods.HSVtoRGB(H, S, B); ;
                    }
                    else
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

            if (MainXValuesAreNullOrEmpty()) UpdateMainXAxis(ScaleForm.GenerateAxis(250.0, 6000.0, 250.0));
            if (MainYValuesAreNullOrEmpty()) UpdateMainYAxis(ScaleForm.GenerateAxis(-10.0, 45.0, 10.0));

            int i = 0;

            int xIndex = -1;
            int yIndex = -1;
            int zIndex = -1;
            int uIndex = -1;
            int numberOfHeaders = csvData[0].Length;
            for (i = 0; i < numberOfHeaders; i++)
            {

                if (exactMatch)
                {
                    if (csvHeaders[i].Equals(xHeader)) xIndex = i;
                    if (csvHeaders[i].Equals(yHeader)) yIndex = i;
                    if (csvHeaders[i].Equals(zHeader)) zIndex = i;
                    if (csvHeaders[i].Equals(vHeader)) uIndex = i;
                }
                else
                {
                    if (csvHeaders[i].Contains(xHeader)) xIndex = i;
                    if (csvHeaders[i].Contains(yHeader)) yIndex = i;
                    if (csvHeaders[i].Contains(zHeader)) zIndex = i;
                    if (csvHeaders[i].Contains(vHeader)) uIndex = i;
                }
                if (yIndex != -1 && xIndex != -1 && zIndex != -1 && uIndex != -1) break;
            }

            //We couldn't find the indexs
            if (yIndex == -1 || xIndex == -1 || zIndex == -1 || uIndex == -1)
            {
                MessageBox.Show("Error: Couldn't find X/Y/Z index in file", "Error");
                return;
            }

            int height = csvData[0].Length;
            MainGridViewData = new List<double[]>[MainGridViewxValues.Length, MainGridViewyValues.Length];
            for (i = 0; i < MainGridView.Columns.Count; i++) for (int j = 0; j < MainGridViewyValues.Length; j++) MainGridViewData[i, j] = new List<double[]>();

            double uMult = 1.0;
            double yMult = 1.0;
            double xMult = 1.0;
            double zMult = 1.0;

            if (!double.TryParse(UAxisMultiplierTextBox.Text, out uMult)) uMult = 1.0;
            if (!double.TryParse(YAxisMultiplierTextBox.Text, out yMult)) yMult = 1.0;
            if (!double.TryParse(XAxisMultiplierTextBox.Text, out xMult)) xMult = 1.0;
            if (!double.TryParse(ZAxisMultiplierTextBox.Text, out zMult)) zMult = 1.0;

            double[][] csvDataCopy = csvData.Select(a => a.ToArray()).ToArray();

            double xsdignore, ysdignore, zsdignore, usdignore;

            if (sdFiltercheckBox.Checked == true)
            {
                if (double.TryParse(xsd.Text, out xsdignore)) RemoveOutliers(ref csvDataCopy, xIndex, xsdignore);
                if (double.TryParse(ysd.Text, out ysdignore)) RemoveOutliers(ref csvDataCopy, xIndex, ysdignore);
                if (double.TryParse(zsd.Text, out zsdignore)) RemoveOutliers(ref csvDataCopy, zIndex, zsdignore);
                if (double.TryParse(usd.Text, out usdignore)) RemoveOutliers(ref csvDataCopy, uIndex, usdignore);
            }

            //Iterate each time sample and add it to the appropriate x,y list
            int numberOfSamples = csvDataCopy.Length;
            double minZ = double.MaxValue;
            double range = 0.0;
            for (i = 0; i < numberOfSamples; i++)
            {
                double x = csvDataCopy[i][ xIndex] * xMult;
                double y = csvDataCopy[i][ yIndex] * yMult;
                double z = csvDataCopy[i][ zIndex] * zMult;
                double u = csvDataCopy[i][ uIndex] * uMult;

                //Skip the entry if it is invalid
                if (!HelperMethods.IsValidDouble(x) || !HelperMethods.IsValidDouble(y) || !HelperMethods.IsValidDouble(z)) continue;

                int[] coordinates = GetNearestCoordinate(x, y, MainGridViewxValues, MainGridViewyValues);
                double[] cell = new double[2];
                cell[0] = z;
                cell[1] = u;
                MainGridViewData[coordinates[0], coordinates[1]].Add(cell);
            }

            //Do not display cells with a count below this level
            double ignoreLowerLimitValue;
            if (!double.TryParse(ignoreLowerLimit.Text, out ignoreLowerLimitValue)) ignoreLowerLimitValue = Double.MinValue;

            double ignoreUpperLimitValue;
            if (!double.TryParse(ignoreUpperLimit.Text, out ignoreUpperLimitValue)) ignoreUpperLimitValue = Double.MaxValue;

            double ignoreCellCount;
            if (!double.TryParse(cellCountIgnoreTextBox.Text, out ignoreCellCount)) ignoreCellCount = 0.0;

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

                    if (primaryArray.Length < ignoreCellCount) primaryArray = new double[0];

                    DataGridViewTuningCell cell = new DataGridViewTuningCell(secondaryArray, primaryArray, range, minZ, MainGridView.currentMode, ignoreLowerLimitValue, ignoreUpperLimitValue);

                    row.Cells.Add(cell);

                }


                MainGridView.Rows.Add(row);
            }
            MainGridView.RowHeadersWidth = 60;

            MainGridView.SetFormat(MainGridView.currentMode, true);
        }



        public static List<double[]> ConditionData(List<double[]> unconditionedData)
        {
            List<double[]> conditionedData = new List<double[]>();
            foreach (double[] value in unconditionedData)
            {
                if (HelperMethods.IsValidDouble(value[0]) && HelperMethods.IsValidDouble(value[1])) conditionedData.Add(value);
            }

            return 
                conditionedData;
        }

        //Remove all outliers that are > 1 + Percentile or < 1 - percentile from the average
        public static int RemoveOutliers(ref double[][] data, int index, double standardDeviationMultiple)
        {
            int removedEntries = 0;
            //Split into two seperate arrays
            List<double> array = new List<double>();

            int length = data.GetLength(0);

            //We don't have enough samples to accurate check for outliers
            if (length < 30) return 0;

            //Copy the data we are working on to an array so we can calculate SD and our removal limits
            double average = 0.0;
            int count = 0;
            for (int i = 0; i < length; i++)
            {
                double value = data[i][index];
                if(HelperMethods.IsValidDouble(value))
                { 
                    average += value;
                    count++;
                    array.Add(value);
                }
            }
            average /= count;

            array.Sort();
            double median = array[array.Count / 2];

            double sumOfSquaresOfDifferences = array.Select(val => (val - average) * (val - average)).Sum();
            double standardDeviation = Math.Sqrt(sumOfSquaresOfDifferences / array.Count);

            //If we have no variability then nothing to remove
            if (standardDeviation == 0.0) return 0;

            //Remove any value > or < standardDeviationMultiple*standardDeviation1 from the median
            double upperLimit = (standardDeviationMultiple * standardDeviation) + median;
            double lowerLimit = median - (standardDeviationMultiple * standardDeviation);

            for (int i = 0; i < length; i++)
            {
                double value = data[i][index];
                //If the value is within range do nothing, if not zero this value so we ignore it
                if (value >= upperLimit || value <= lowerLimit)
                {
                    data[i][index] = double.NaN;
                    removedEntries++;
                }
            }

            return removedEntries;
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
            MainGridView.SetFormat(CellFormat.Minimum);
        }

        private void maxButton_Click(object sender, EventArgs e)
        {
           MainGridView.SetFormat(CellFormat.Maximum);
        }

        private void countButton_Click(object sender, EventArgs e)
        {
            MainGridView.SetFormat(CellFormat.Count);
        }

        private void SDButton_Click(object sender, EventArgs e)
        {
            MainGridView.SetFormat(CellFormat.StandardDeviation);
        }

        private void averageButton_Click(object sender, EventArgs e)
        {
            MainGridView.SetFormat(CellFormat.Average);
        }

        private void checkPulseButton_Click(object sender, EventArgs e)
        {

            if (MainGridView.SelectedCells.Count <= 0) return;
            int yIndex = MainGridView.SelectedCells[0].RowIndex;
            int xIndex = MainGridView.SelectedCells[0].ColumnIndex;


            //Update the gridview with the new selection
            PopulateMainGridView(xAxisComboBox.Text, yAxisComboBox.Text, zAxisComboBox.Text, UComboBox.Text);


            foreach (DataGridViewRow row in MainGridView.Rows)
            {
                foreach (DataGridViewTuningCell cell in row.Cells)
                {

                    if(yIndex == cell.RowIndex && xIndex == cell.ColumnIndex)
                    {
                        cell.Selected = true;
                        PopulateFilteredCellGridView(cell);
                        return;
                    }
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            PopulateMainGridView(xAxisComboBox.Text, yAxisComboBox.Text, zAxisComboBox.Text, UComboBox.Text);
        }


        private void UComboBox_DragLeave(object sender, EventArgs e)
        {

        }

        private void UAxisMultiplierTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
