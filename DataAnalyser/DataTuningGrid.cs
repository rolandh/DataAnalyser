using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataAnalyser
{
    public class DataTuningGrid : DataGridView
    {
        //Todo this should go in a Custom Datagrid
        public CellFormat currentMode = CellFormat.Average;
        public double colourRange;
        public double colourOffset;

        public DataTuningGrid()
        {

        }

        public void SetFormat(CellFormat newFormat, Boolean forceRedraw = false)
        {
            if(!forceRedraw && currentMode == newFormat) return;

            if (this.Rows == null) return;
            if (this.Columns == null) return;

            if (newFormat == CellFormat.Uninitialized) newFormat = CellFormat.Average;

            double min = double.MaxValue;
            double max = double.MinValue;

            currentMode = newFormat;

            //Calculate the range for redrawing the colour by finding the min, max and offset
            foreach (DataGridViewRow row in this.Rows)
            {

                switch (currentMode)
                {
                    case CellFormat.Average:
                        foreach (DataGridViewTuningCell cell in row.Cells)
                        {
                            cell.SetFormat(currentMode);
                            if (!cell.cellCurrentlyIgnored)
                            {
                                if (cell.data.Length > 0)
                                {
                                    if (cell.average < min) min = cell.average;
                                    if (cell.average > max) max = cell.average;
                                }
                            }
                        }
                        break;
                    case CellFormat.Minimum:
                        foreach (DataGridViewTuningCell cell in row.Cells)
                        {
                            cell.SetFormat(currentMode);
                            if (!cell.cellCurrentlyIgnored)
                            {
                                if (cell.data.Length > 0)
                                {
                                    if (cell.min < min) min = cell.min;
                                    if (cell.min > max) max = cell.min;
                                }
                            }
                        }
                        break;
                    case CellFormat.Maximum:
                        foreach (DataGridViewTuningCell cell in row.Cells)
                        {
                            cell.SetFormat(currentMode);
                            if (!cell.cellCurrentlyIgnored)
                            {
                                if (cell.data.Length > 0)
                                {
                                    if (cell.max < min) min = cell.max;
                                    if (cell.max > max) max = cell.max;
                                }
                            }
                        }
                        break;
                    case CellFormat.StandardDeviation:
                        foreach (DataGridViewTuningCell cell in row.Cells)
                        {
                            cell.SetFormat(currentMode);
                            if (!cell.cellCurrentlyIgnored)
                            {
                                if (cell.data.Length > 0)
                                {
                                    if (cell.standardDeviation < min) min = cell.standardDeviation;
                                    if (cell.standardDeviation > max) max = cell.standardDeviation;
                                }
                            }
                        }
                        break;
                    case CellFormat.Count:
                        foreach (DataGridViewTuningCell cell in row.Cells)
                        {
                            cell.SetFormat(currentMode);
                            if (!cell.cellCurrentlyIgnored)
                            {
                                if (cell.data.Length > 0)
                                {
                                    if (cell.count < min) min = cell.count;
                                    if (cell.count > max) max = cell.count;
                                }
                            }
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            double range = max - min;
            foreach (DataGridViewRow row in this.Rows)
            {
                foreach (DataGridViewTuningCell cell in row.Cells)
                {
                    cell.SetColour(range, min);
                }
            }
        }

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }
    }
    public class DataGridViewTuningColumn : DataGridViewTextBoxColumn
    {
        public DataGridViewTuningColumn()
        {
            this.CellTemplate = new DataGridViewTuningCell();
        }
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

    public class DataGridViewTuningCell : DataGridViewTextBoxCell
    {
        public double dataValue = Double.NaN;
        public CellFormat cellFormat = CellFormat.Uninitialized;
        public double[] data = new double[0];
        public double[] secondaryData = new double[0];
        public double ignoreValue;
        public bool cellCurrentlyIgnored = false;
        public double min = Double.NaN;
        public double max = Double.NaN;
        public double average = Double.NaN;
        public double standardDeviation = Double.NaN;
        public double count;

        public DataGridViewTuningCell()
        {
        }

        public DataGridViewTuningCell(double[] secondaryDataInput, double[] doubleArray, double range, double offset, CellFormat format = CellFormat.Average, double ignoreValue = Double.MinValue)
        {
            this.data = doubleArray;
            this.secondaryData = secondaryDataInput;
            this.ignoreValue = ignoreValue;
            if (data.Length > 0)
            {
                this.average = data.Average();
                this.min = data.Min();
                this.max = data.Max();
            }
            this.count = data.Length;
            double sumOfSquaresOfDifferences = data.Select(val => (val - average) * (val - average)).Sum();
            standardDeviation = Math.Sqrt(sumOfSquaresOfDifferences / data.Length);

            SetFormat(format);
            SetColour(range, offset);

        }

        public void SetColour(double colourRange, double colourOffset)
        {
            if (HelperMethods.IsValidDouble(dataValue) && !cellCurrentlyIgnored)
            {
                double scalar = 360.0 / colourRange;
                double value = scalar * (dataValue - colourOffset);

                double H = (0.4 * (360.0 - value)) - 10.0;
                double S = 0.9;
                double B = 0.92;

                this.Style.BackColor = HelperMethods.HSVtoRGB(H, S, B);
            } else
            {
                this.Style.BackColor = Color.White;
            }
        }


        public void SetFormat(CellFormat newFormat)
        {

            if (newFormat == cellFormat) return;
            cellFormat = newFormat;

            this.Value = "";
            if (data.Length == 0) return;

            if (newFormat == CellFormat.Uninitialized) newFormat = CellFormat.Average;
            if (newFormat == CellFormat.Average) dataValue = average;
            else if (newFormat == CellFormat.Minimum) dataValue = min;
            else if (newFormat == CellFormat.Maximum) dataValue = max;
            else if (newFormat == CellFormat.StandardDeviation) dataValue = standardDeviation;
            else if (newFormat == CellFormat.Count) dataValue = count;
            else throw new NotImplementedException();

            if (HelperMethods.IsValidDouble(dataValue) && dataValue >= this.ignoreValue)
            {
                this.Value = String.Format("{0:0.00}", dataValue);

                cellCurrentlyIgnored = false;
            }
            else
            {
                cellCurrentlyIgnored = true;
            }

        }

   

    }
}
