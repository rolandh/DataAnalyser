using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JR.Utils.GUI.Forms;

namespace DataAnalyser
{
    public static class HelperMethods
    {


        private static double Mean(double[][] values, int index, int start, int end)
        {
            double s = 0;

            for (int i = start; i < end; i++)
            {
                s += values[i][index];
            }

            return s / (end - start);
        }


        private static double RootMeanSquare(double[][] values, int index)
        {
            double s = 0;
            int i;
            for (i = 0; i < values.Length; i++)
            {
                s += values[i][index] * values[i][index];
            }
            return Math.Sqrt(s / values.Length);
        }


        /// <summary>
        /// Partitions the given list around a pivot element such that all elements on left of pivot are <= pivot
        /// and the ones at thr right are > pivot. This method can be used for sorting, N-order statistics such as
        /// as median finding algorithms.
        /// Pivot is selected ranodmly if random number generator is supplied else its selected as last element in the list.
        /// Reference: Introduction to Algorithms 3rd Edition, Corman et al, pp 171
        /// </summary>
        private static int Partition<T>(this IList<T> list, int start, int end, Random rnd = null) where T : IComparable<T>
        {
            if (rnd != null)
                list.Swap(end, rnd.Next(start, end));

            var pivot = list[end];
            var lastLow = start - 1;
            for (var i = start; i < end; i++)
            {
                if (list[i].CompareTo(pivot) <= 0)
                    list.Swap(i, ++lastLow);
            }
            list.Swap(end, ++lastLow);
            return lastLow;
        }

        /// <summary>
        /// Returns Nth smallest element from the list. Here n starts from 0 so that n=0 returns minimum, n=1 returns 2nd smallest element etc.
        /// Note: specified list would be mutated in the process.
        /// Reference: Introduction to Algorithms 3rd Edition, Corman et al, pp 216
        /// </summary>
        public static T NthOrderStatistic<T>(this IList<T> list, int n, Random rnd = null) where T : IComparable<T>
        {
            return NthOrderStatistic(list, n, 0, list.Count - 1, rnd);
        }
        private static T NthOrderStatistic<T>(this IList<T> list, int n, int start, int end, Random rnd) where T : IComparable<T>
        {
            while (true)
            {
                var pivotIndex = list.Partition(start, end, rnd);
                if (pivotIndex == n)
                    return list[pivotIndex];

                if (n < pivotIndex)
                    end = pivotIndex - 1;
                else
                    start = pivotIndex + 1;
            }
        }

        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            if (i == j)   //This check is not required but Partition function may make many calls so its for perf reason
                return;
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        /// <summary>
        /// Note: specified list would be mutated in the process.
        /// </summary>
        public static T Median<T>(this IList<T> list) where T : IComparable<T>
        {
            return list.NthOrderStatistic((list.Count - 1) / 2);
        }

        public static double Median<T>(this IEnumerable<T> sequence, Func<T, double> getValue)
        {
            var list = sequence.Select(getValue).ToList();
            var mid = (list.Count - 1) / 2;
            return list.NthOrderStatistic(mid);
        }



        public static bool IsValidDouble(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value)) return false;
            return true;
        }

        public static bool WriteCSV(string filename, string [] csvHeaders, double[][] csvData)
        {
            // Get the file's text.
            try
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    string header = "";
                    for (int column = 0; column < csvHeaders.Length; column++)
                    {
                        header += csvHeaders[column].ToString();
                        if (column != csvHeaders.Length - 1) header += ",";
                    }
                    writer.WriteLine(header);

                    for (int row = 0; row < csvData.Length; row++)
                    {
                        string rowString = "";
                        for (int column = 0; column < csvData[0].Length; column++)
                        {
                            if(IsValidDouble(csvData[row][column])) rowString += csvData[row][column].ToString();
                            if (column != csvHeaders.Length - 1) rowString += ",";
                        }
                        writer.WriteLine(rowString);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message, "Error");
                return false;
            }
        }

        // Load a CSV file into an array of rows and columns.
        // Assume there may be blank lines but every line has
        public static bool LoadCsv(string filename, out string[] csvHeaders, out double[][] csvData)
        {
            // Get the file's text.
            csvHeaders = new string[0];
            csvData = new double[0][];
            try
            {
                string whole_file = System.IO.File.ReadAllText(filename);

                // Split into lines.
                whole_file = whole_file.Replace('\n', '\r');
                string[] lines = whole_file.Split(new char[] { '\r' },
                    StringSplitOptions.RemoveEmptyEntries);

                //Check if empty
                if (lines.Length < 2)
                {
                    MessageBox.Show("CSV file requires at least one entry", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (string.IsNullOrEmpty(lines[0]))
                {
                    MessageBox.Show("CSV file missing header", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                int headerRow = 0;
                int firstDataRow = 1;
                if (lines[0].Contains("HP Tuners CSV Log File"))
                {
                    //We have a HPL file
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].Equals("[Channel Data]"))
                        {
                            firstDataRow = i + 1;
                        }
                        if (lines[i].Equals("[Channel Information]"))
                        {
                            headerRow = i + 2;
                        }
                        if (firstDataRow != 1 && headerRow != 0) break;

                        if (i == lines.Length - 2)
                        {
                            MessageBox.Show("Could not find [Channel Data] Or [Channel Information] within HPT CSV log file, giving up.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }

                }

                // See how many rows and columns there are.
                int num_rows = lines.Length - firstDataRow - 1;
                int num_cols = lines[headerRow].Split(',').Length;

                // Allocate the data array.
                csvHeaders = new string[num_cols];
                csvData = new double[num_rows-1][];

                // Load the array.

                string[] headerLine = lines[headerRow].Split(',');
                if (headerLine.Length < 3)
                {
                    MessageBox.Show("Not enough headers in CSV file! Need at least three, possibly wrong file format? I'm giving up, sorry bro.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                csvHeaders = headerLine;

                for (int r = firstDataRow; r < (num_rows); r++)
                {
                    string[] line_r = lines[r].Split(',');

                    csvData[r - 1] = new double[num_cols];
                    for (int c = 0; c < num_cols; c++)
                    {
                        double result;
                        if (String.IsNullOrEmpty(line_r[c])) continue;
                        if (Double.TryParse(line_r[c], out result))
                        {
                            csvData[r - 1][c] = result;
                        }
                        else
                        {
                            csvData[r - 1][c] = double.NaN;
                        }
                    }
                }

                return true;


            }
            catch (Exception e)
            {
                var currentStack = new System.Diagnostics.StackTrace(true);
                string stackTrace = currentStack.ToString();

                FlexibleMessageBox.Show("Failed to open file. Stacktrace: " + Environment.NewLine + stackTrace,
                                     "Error",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Information,
                                     MessageBoxDefaultButton.Button2);
                return false;
            }
        }
    }
}
