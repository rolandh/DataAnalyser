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
            xAxisComboBox.Text = "";
            yAxisComboBox.Text = "";
            zAxisComboBox.Text = "";
            UComboBox.Text = "";

            foreach (string header in csvHeaders)
            {
                xAxisComboBox.Items.Add(header);
                yAxisComboBox.Items.Add(header);
                zAxisComboBox.Items.Add(header);
                UComboBox.Items.Add(header);
            }


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

            pcmSimulator1.csvData = csvData;
            pcmSimulator1.csvHeaders = csvHeaders;
            pcmSimulator1.fordPCMHelper = fordPCMHelper;

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

                        cell.Style.BackColor = HSVtoRGB(H, S, B); ;
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
            if (MainYValuesAreNullOrEmpty()) UpdateMainYAxis(ScaleForm.GenerateAxis(-10.0, 45.0, 5.0));

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

            if(double.TryParse(xsd.Text, out xsdignore)) RemoveOutliers(ref csvDataCopy, xIndex, xsdignore);
            if(double.TryParse(ysd.Text, out ysdignore)) RemoveOutliers(ref csvDataCopy, xIndex, ysdignore);
            if(double.TryParse(zsd.Text, out zsdignore)) RemoveOutliers(ref csvDataCopy, zIndex, zsdignore);
            if(double.TryParse(usd.Text, out usdignore)) RemoveOutliers(ref csvDataCopy, uIndex, usdignore);


            //Iterate each time sample and add it to the appropriate x,y list
            int numberOfSamples = csvDataCopy.Length;
            double minZ = double.MaxValue;
            double maxZ = double.MinValue;
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

            //Calculate our max Z value
            for (i = 0; i < MainGridViewyValues.Length; i++)
            {
                for (int j = 0; j < MainGridViewxValues.Length; j++)
                {
                    //MainGridViewData[j, i] = RemoveOutliers(MainGridViewData[j, i], 3.0);

                    List<double[]> list = MainGridViewData[j, i];
                    double[] doubleArray = new double[list.Count];
                    for (int k = 0; k < doubleArray.Length; k++) doubleArray[k] = list[k][0];

                    if (doubleArray.Length != 0)
                    {
                        double average = doubleArray.Average();

                        if (HelperMethods.IsValidDouble(average))
                        {
                            if (average > maxZ) maxZ = average;
                            if (average < minZ) minZ = average;
                        }
                    }
                }
            }

            range = maxZ - minZ;

            //Do not display cells with a count below this level
            double ignoreValue;
            if (!double.TryParse(ignoreCellTextBox.Text, out ignoreValue)) ignoreValue = 0.0;

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

                    DataGridViewTuningCell cell = new DataGridViewTuningCell(secondaryArray, primaryArray, range, minZ, currentMode, ignoreValue);

                    row.Cells.Add(cell);

                }


                MainGridView.Rows.Add(row);
            }
            MainGridView.RowHeadersWidth = 60;
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

            double median = HelperMethods.Median(array);
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

        ////Remove all outliers that are > 1 + Percentile or < 1 - percentile from the average
        //public static List<double[]> RemoveOutliers(List<double[]> unconditionedData, double standardDeviationMultiple)
        //{
        //    //Firstly remove all NaN, Infinity, null variables
        //    List<double[]> conditionedData = ConditionData(unconditionedData);
        //    List<double[]> returnData = new List<double[]>();

        //    //Split into two seperate arrays
        //    List<double> array1 = new List<double>();
        //    List<double> array2 = new List<double>();

        //    for(int i = 0; i < unconditionedData.Count; i++)
        //    {
        //        array1.Add(unconditionedData[i][0]);
        //        array2.Add(unconditionedData[i][1]);
        //    }

        //    if (array1.Count == 0 || array1.Count == 0) return conditionedData;

        //    double median1 = HelperMethods.Median(array1);
        //    double median2 = HelperMethods.Median(array2);

        //    double average1 = array1.Average();
        //    double average2 = array2.Average();

        //    double sumOfSquaresOfDifferences1 = array1.Select(val => (val - average1) * (val - average1)).Sum();
        //    double sumOfSquaresOfDifferences2 = array2.Select(val => (val - average2) * (val - average2)).Sum();

        //    double standardDeviation1 = Math.Sqrt(sumOfSquaresOfDifferences1 / array1.Count);
        //    double standardDeviation2 = Math.Sqrt(sumOfSquaresOfDifferences2 / array2.Count);

        //    //Remove any value > or < standardDeviationMultiple*standardDeviation1 from the median
        //    double upperLimit1 = (standardDeviationMultiple * standardDeviation1) + median1;
        //    double lowerLimit1 = median1 - (standardDeviationMultiple * -standardDeviation1);

        //    double upperLimit2 = (standardDeviationMultiple * standardDeviation2) + median2;
        //    double lowerLimit2 = median2 - (standardDeviationMultiple * -standardDeviation2);

        //    foreach (double[] value in unconditionedData)
        //    {

        //        if ((value[0] <= upperLimit1 && value[0] >= lowerLimit1) && (value[1] <= upperLimit2 && value[1] >= lowerLimit2))
        //        {
        //            returnData.Add(value);
        //        }
        //    }

        //    return returnData;
        //}

        //Todo this should go in a Custom Datagrid
        DataGridViewTuningCell.CellFormat currentMode = DataGridViewTuningCell.CellFormat.Average;

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
            public double colourRange;
            public double colourOffset;

            public double min;
            public double max;
            public int count;
            public readonly CellFormat cellFormat = CellFormat.Uninitialized;
            public double[] data = new double[0];
            public double[] secondaryData = new double[0];
            public double range;
            public double offset;
            public double ignoreValue;
            public bool cellCurrentlyIgnored = false;

            public DataGridViewTuningCell()
            {
            }

            public DataGridViewTuningCell(double[] secondaryDataInput, double[] doubleArray, double range, double offset, CellFormat format = CellFormat.Average, double ignoreValue = Double.MinValue)
            {
                this.data = doubleArray;
                this.secondaryData = secondaryDataInput;
                this.range = range;
                this.offset = offset;
                this.count = doubleArray.Length;
                this.ignoreValue = ignoreValue;
                SetFormat(format, range, offset);

            }

            public enum CellFormat
            {
                Uninitialized = -1,
                Average = 0,
                StandardDeviation = 1,
                Minimum = 2,
                Maximum = 3,
                Count = 4,
            }

            public void SetFormat(CellFormat newFormat, double _colourRange, double _colourOffset)
            {
                this.colourRange = _colourRange;
                this.colourOffset = _colourOffset;

                if (newFormat == cellFormat) return;

                this.Value = "";
                if (data.Length == 0) return;

                average = data.Average();
                min = data.Min();
                max = data.Max();

                double sumOfSquaresOfDifferences = data.Select(val => (val - average) * (val - average)).Sum();
                this.standardDeviation = Math.Sqrt(sumOfSquaresOfDifferences / data.Length);

                double dataVal = 0.0;

                if (newFormat == CellFormat.Average) dataVal = average;
                else if (newFormat == CellFormat.Minimum) dataVal = min;
                else if (newFormat == CellFormat.Maximum) dataVal = max;
                else if (newFormat == CellFormat.StandardDeviation) dataVal = standardDeviation;
                else if (newFormat == CellFormat.Count) dataVal = count;

                if (HelperMethods.IsValidDouble(dataVal) && dataVal >= this.ignoreValue)
                {
                    this.Value = String.Format("{0:0.00}", dataVal);

                    double scalar = 360.0 / colourRange;
                    double value = scalar * (dataVal - colourOffset);

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
            currentMode = DataGridViewTuningCell.CellFormat.Minimum;
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
                    cell.SetFormat(DataGridViewTuningCell.CellFormat.Minimum, max - min, min);
                }
            }
        }

        private void maxButton_Click(object sender, EventArgs e)
        {
            double min = double.MaxValue;
            double max = double.MinValue;
            currentMode = DataGridViewTuningCell.CellFormat.Maximum;
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
                    cell.SetFormat(DataGridViewTuningCell.CellFormat.Maximum, max - min, min);
                }
            }
        }

        private void countButton_Click(object sender, EventArgs e)
        {
            double min = double.MaxValue;
            double max = double.MinValue;
            currentMode = DataGridViewTuningCell.CellFormat.Count;
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
                    cell.SetFormat(DataGridViewTuningCell.CellFormat.Count, max - min, min);
                }
            }
        }

        private void SDButton_Click(object sender, EventArgs e)
        {
            //Update the range for the colour calculation
            double min = double.MaxValue;
            double max = double.MinValue;
            currentMode = DataGridViewTuningCell.CellFormat.StandardDeviation;
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
                    cell.SetFormat(DataGridViewTuningCell.CellFormat.StandardDeviation, max - min, min);
                }
            }
        }

        private void averageButton_Click(object sender, EventArgs e)
        {
            double min = double.MaxValue;
            double max = double.MinValue;
            currentMode = DataGridViewTuningCell.CellFormat.Average;
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
                    cell.SetFormat(DataGridViewTuningCell.CellFormat.Average, max - min, min);
                }
            }
        }

        private void checkPulseButton_Click(object sender, EventArgs e)
        {
            int yIndex = 0;
            int xIndex = 0;
            foreach (DataGridViewRow row in MainGridView.Rows)
            {
                foreach (DataGridViewTuningCell cell in row.Cells)
                {
                    if (cell.Selected)
                    {
                        yIndex = cell.RowIndex;
                        xIndex = cell.ColumnIndex;
                        break;
                    }
                }
            }

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

        private void UComboBox_DragLeave(object sender, EventArgs e)
        {

        }

        private void UAxisMultiplierTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
