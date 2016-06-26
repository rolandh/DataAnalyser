using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FordPCMEditor;
using System.Diagnostics;
using System.Threading;
using JR.Utils.GUI.Forms;

namespace DataAnalyser
{
    public partial class PCMSimulator : UserControl
    {
        public FordPCMHelper fordPCMHelper;
        public string[] csvHeaders;
        public double[][] csvData;
        CancellationTokenSource cts;

        public PCMSimulator()
        {
            InitializeComponent();
        }

        static public double GetRSquared(double[][] array1, int index1, int index2, double scale1, double scale2)
        {
            double R = 0;

            try
            {
                // sum(xy)
                int invalidEntries = 0;
                double sumXY = 0;
                for (int c = 0; c <= array1.Length - 1; c++)
                {
                    double number1 = (array1[c][index1] * scale1);
                    double number2 = (array1[c][index2] * scale2);

                    if (!HelperMethods.IsValidDouble(number1) || !HelperMethods.IsValidDouble(number2))
                    {
                        invalidEntries++;
                        continue;
                    }

                    sumXY = sumXY + (array1[c][index1] * scale1) * (array1[c][index2] * scale2);
                }

                // sum(x)
                double sumX = 0;
                for (int c = 0; c <= array1.Length - 1; c++)
                {
                    sumX = sumX + (array1[c][index1] * scale1);
                }

                // sum(y)
                double sumY = 0;
                for (int c = 0; c <= array1.Length - 1; c++)
                {
                    sumY = sumY + (array1[c][index2] * scale2);
                }

                // sum(x^2)
                double sumXX = 0;
                for (int c = 0; c <= array1.Length - 1; c++)
                {
                    sumXX = sumXX + (array1[c][index1] * scale1) * (array1[c][index1] * scale1);
                }

                // sum(y^2)
                double sumYY = 0;
                for (int c = 0; c <= array1.Length - 1; c++)
                {
                    sumYY = sumYY + (array1[c][index2] * scale2) * (array1[c][index2] * scale2);
                }

                // n
                int n = array1.Length - invalidEntries;

                R = (n * sumXY - sumX * sumY) / (Math.Pow((n * sumXX - Math.Pow(sumX, 2)), 0.5) * Math.Pow((n * sumYY - Math.Pow(sumY, 2)), 0.5));
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return R * R;
        }

        private async void calculateAirmassButton_Click_1(object sender, EventArgs e)
        {
            //this.cancelButton.Enabled = true;
            //this.calculateAirmassButton.Enabled = false;

            var progressIndicator = new Progress<int>(ReportProgress);
            cts = new CancellationTokenSource();
            try
            {
                 await EmulatePCM(progressIndicator, cts.Token);


                //EmulatePCMSync();
            }
            catch (OperationCanceledException)
            {
                this.progressBar.Value = 0;
            }
            catch (Exception ex)
            {
                var currentStack = new System.Diagnostics.StackTrace(true);
                string stackTrace = currentStack.ToString();

                FlexibleMessageBox.Show("Failed to process file due to: " + Environment.NewLine + stackTrace,
                                     "Error",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Information,
                                     MessageBoxDefaultButton.Button2);
            }
            finally
            {
                this.cancelButton.Enabled = false;
                this.calculateAirmassButton.Enabled = true;
            }

            this.progressBar.Value = 0;

            return;

        }


        void ReportProgress(int value)
        {
            //Update the UI to reflect the progress value that is passed back.
            this.progressBar.Value = value;
        }

        //public void EmulatePCMSync()
        //{

        //    double[] returnArray = new double[2];
        //    returnArray[0] = double.NaN;
        //    returnArray[1] = double.NaN;
        //    if (fordPCMHelper == null)
        //    {
        //        MessageBox.Show("You must load a .CSV file first!");
        //        return;
        //    }

        //    if (!fordPCMHelper.fileLoaded)
        //    {
        //        MessageBox.Show("You must load a .HPT file first!");
        //    return;
        //    }

        //    int indexOfPulse, indexOfCalculatedLoad, indexOfMAP, indexOfTPS, indexOfCamAngle, indexOfRPM, indexOfMapPerAirmass, indexOfMapPerZeroAirmass, indexOfAirMass, indexOfFuelMassViaSD, indexOfFuelMassViaInjMs, indexOfCalculatedInjectorPW, indexOfCalculatedAFR, indexOfCalculatedInjectorPWTrimmed, indexOfFuelTrim;


        //    indexOfCamAngle = Array.FindIndex(csvHeaders, x => (x.IndexOf("cam angle", StringComparison.OrdinalIgnoreCase) >= 0));
        //    indexOfRPM = Array.FindIndex(csvHeaders, x => (x.IndexOf("rpm", StringComparison.OrdinalIgnoreCase) >= 0));
        //    indexOfTPS = Array.FindIndex(csvHeaders, x => (x.IndexOf("ETC throttle", StringComparison.OrdinalIgnoreCase) >= 0));
        //    indexOfMAP = Array.FindIndex(csvHeaders, x => (x.IndexOf("MANIFOLD ABSOLUTE PRESSURE", StringComparison.OrdinalIgnoreCase) >= 0));
        //    indexOfPulse = Array.FindIndex(csvHeaders, x => (x.IndexOf("FUEL PULSEWIDTH", StringComparison.OrdinalIgnoreCase) >= 0));
        //    indexOfFuelTrim = Array.FindIndex(csvHeaders, x => (x.IndexOf("FUEL TRIM", StringComparison.OrdinalIgnoreCase) >= 0));

        //    if (indexOfMAP == -1)
        //    {
        //        MessageBox.Show("Couldn't find MAP entry in csv data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    if (indexOfCamAngle == -1)
        //    {
        //        MessageBox.Show("Couldn't find cam angle entry in csv data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    if (indexOfRPM == -1)
        //    {
        //        MessageBox.Show("Couldn't find rpm entry in csv data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    if (indexOfTPS == -1)
        //    {
        //        MessageBox.Show("Couldn't find TPS entry in csv data, we will assume commanded lambda is always 1.0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        //        return;
        //    }

        //    indexOfMapPerAirmass = Array.IndexOf(csvHeaders, "Map per Airmass");
        //    indexOfMapPerZeroAirmass = Array.IndexOf(csvHeaders, "Map per Zero Airmass");
        //    indexOfAirMass = Array.IndexOf(csvHeaders, "Calculated Airmass");
        //    indexOfFuelMassViaSD = Array.IndexOf(csvHeaders, "Calculated Fuel Mass via SD");
        //    indexOfFuelMassViaInjMs = Array.IndexOf(csvHeaders, "Calculated Fuel Mass via Pulsewidth");
        //    indexOfCalculatedInjectorPW = Array.IndexOf(csvHeaders, "Calculated Injector Pulsewidth");
        //    indexOfCalculatedInjectorPWTrimmed = Array.IndexOf(csvHeaders, "Calculated Injector Pulsewidth With Fuel Trim");
        //    indexOfCalculatedAFR = Array.IndexOf(csvHeaders, "Calculated Commanded AFR");
        //    indexOfCalculatedLoad = Array.IndexOf(csvHeaders, "Calculated Load");

        //    int arrayResizeAmount = 0;
        //    if (indexOfMapPerAirmass == -1)
        //    {
        //        indexOfMapPerAirmass = csvHeaders.Length;
        //        arrayResizeAmount++;
        //    }

        //    if (indexOfMapPerZeroAirmass == -1)
        //    {
        //        indexOfMapPerZeroAirmass = csvHeaders.Length + arrayResizeAmount;
        //        arrayResizeAmount++;
        //    }
        //    if (indexOfAirMass == -1)
        //    {
        //        indexOfAirMass = csvHeaders.Length + arrayResizeAmount;
        //        arrayResizeAmount++;
        //    }
        //    if (indexOfFuelMassViaSD == -1)
        //    {
        //        indexOfFuelMassViaSD = csvHeaders.Length + arrayResizeAmount;
        //        arrayResizeAmount++;
        //    }
        //    if (indexOfFuelMassViaInjMs == -1)
        //    {
        //        indexOfFuelMassViaInjMs = csvHeaders.Length + arrayResizeAmount;
        //        arrayResizeAmount++;
        //    }
        //    if (indexOfCalculatedInjectorPW == -1)
        //    {
        //        indexOfCalculatedInjectorPW = csvHeaders.Length + arrayResizeAmount;
        //        arrayResizeAmount++;
        //    }
        //    if (indexOfCalculatedInjectorPWTrimmed == -1)
        //    {
        //        indexOfCalculatedInjectorPWTrimmed = csvHeaders.Length + arrayResizeAmount;
        //        arrayResizeAmount++;
        //    }
        //    if (indexOfCalculatedAFR == -1)
        //    {
        //        indexOfCalculatedAFR = csvHeaders.Length + arrayResizeAmount;
        //        arrayResizeAmount++;
        //    }
        //    if (indexOfCalculatedLoad == -1)
        //    {
        //        indexOfCalculatedLoad = csvHeaders.Length + arrayResizeAmount;
        //        arrayResizeAmount++;
        //    }


        //    //Create new arrays with 3 extra values,
        //    string[] newCsvHeaders = new string[csvHeaders.Length + arrayResizeAmount];
        //    double[][] newCsvData = new double[csvData.Length][];


        ////Copy the previous data
        //    Array.Copy(csvHeaders, newCsvHeaders, csvHeaders.Length);
        //    csvHeaders.CopyTo(newCsvHeaders, 0);

        //    for (int i = 0; i < csvData.Length; i++)
        //    {
        //        newCsvData[i] = new double[csvData[0].Length + arrayResizeAmount];
        //        for (int j = 0; j < csvData[0].Length; j++)
        //        {
        //            newCsvData[i][j] = csvData[i][j];
        //        }
        //    }

        //    //Add the new headers
        //    newCsvHeaders[indexOfMapPerAirmass] = "Map per Airmass";
        //    newCsvHeaders[indexOfMapPerZeroAirmass] = "Map per Zero Airmass";
        //    newCsvHeaders[indexOfAirMass] = "Calculated Airmass";
        //    newCsvHeaders[indexOfFuelMassViaSD] = "Calculated Fuel Mass via SD";
        //    newCsvHeaders[indexOfFuelMassViaInjMs] = "Calculated Fuel Mass via Pulsewidth";
        //    newCsvHeaders[indexOfCalculatedInjectorPW] = "Calculated Injector Pulsewidth";
        //    newCsvHeaders[indexOfCalculatedInjectorPWTrimmed] = "Calculated Injector Pulsewidth With Fuel Trim";
        //    newCsvHeaders[indexOfCalculatedAFR] = "Calculated Commanded AFR";
        //    newCsvHeaders[indexOfCalculatedLoad] = "Calculated Load";

            
        //    //get stoich
        //    double stoichAFR;
        //    if (!fordPCMHelper.TryGetDouble(2300, out stoichAFR))
        //    {
        //        MessageBox.Show("Couldn't read Stoich AFR value (2300), we will assume 14.64", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        //        stoichAFR = 14.64;
        //    }

        //    Boolean calculatePulseWidth = true;

        //    //get low slope
        //    double lowSlope;
        //    if (!fordPCMHelper.TryGetDouble(12010, out lowSlope))
        //    {
        //        MessageBox.Show("Couldn't read Injector Low Slope (12010), we will not be able to calculate pulse width!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        //        calculatePulseWidth = false;
        //    }
        //    lowSlope /= 3600.0;

        //    //get high slope
        //    double highSlope;
        //    if (!fordPCMHelper.TryGetDouble(12011, out highSlope))
        //    {
        //        MessageBox.Show("Couldn't read Injector High Slope (12010), we will not be able to calculate pulse width!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        //        calculatePulseWidth = false;
        //    }
        //    highSlope /= 3600.0;

        //    //get offset
        //    double breakpoint;
        //    if (!fordPCMHelper.TryGetDouble(12012, out breakpoint))
        //    {
        //        MessageBox.Show("Couldn't read Injector breakpoint (12012), we will not be able to calculate pulse width!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        //        calculatePulseWidth = false;
        //    }

        //    //get offset
        //    double offset;
        //    if (!fordPCMHelper.TryGetDouble(32050, out offset))
        //    {
        //        MessageBox.Show("Couldn't read Injector offset at 12.0V (32050), we will not be able to calculate pulse width!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        //        //calculatePulseWidth = false;
        //        offset = 0.00146499997936189;
        //    }


        //    double displacement;
        //    if (!fordPCMHelper.TryGetDouble(50000, out displacement))
        //    {
        //        MessageBox.Show("Couldn't read engine displacement (50000), we will assume 4.0L (0.00172 lb)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        //        displacement = 0.00172;
        //    }

        //    Boolean lambdaErrorIssued = false;

        //    //int samplesProcessed = 0;
        //    int totalCount = csvData.Length;
        //    //totalCount = 1200;
        //    //for (int i = 0; i < csvData.GetLength(0); i++)
        //    for (int i = 0; i < totalCount; i++)
        //    //Parallel.For(0, csvData.GetLength(0), i =>
        //    //Parallel.For(0, totalCount, i =>
        //    {

        //        double camAngle = csvData[i][indexOfCamAngle];
        //        double rpm = csvData[i][indexOfRPM];
        //        double map = csvData[i][indexOfMAP];
        //        double csvPulseWidthSeconds = csvData[i][indexOfPulse];
        //        double fuelTrim = csvData[i][indexOfFuelTrim];
        //        double TPS = -1.0;
        //        double commandedLambda = 1.0;

        //        //Get the commanded lambda value
        //        if (indexOfTPS != -1)
        //        {
        //            TPS = csvData[i][indexOfTPS];
        //            TPS *= 1.25;
        //            if (!fordPCMHelper.TryGetTableValue(rpm, TPS, 50151, out commandedLambda))
        //            {
        //                if (!lambdaErrorIssued) MessageBox.Show(String.Format("Failed to get commanded lambda for {0}rpm {1}TPS%, we will use 1.0 from now on and no longer report anymore errors of this type", rpm, TPS));
        //                lambdaErrorIssued = true;
        //                commandedLambda = 1.0;
        //            }

        //        }
        //        newCsvData[i][indexOfCalculatedAFR] = commandedLambda;

        //        //interpolate the map per airmass values
        //        double mapPerAirmass;
        //        double mapPerZeroAirmass;
        //        if (!fordPCMHelper.TryGetTableValue(rpm, camAngle, 50051, out mapPerZeroAirmass))
        //        {
        //            MessageBox.Show(String.Format("Failed to get Map per Zero Airmass for {0}rpm {1} Cam Angle, this is a fatal error, giving up, sorry bro", rpm, camAngle));
        //            return;
        //        }
        //        if (!fordPCMHelper.TryGetTableValue(rpm, camAngle, 50055, out mapPerAirmass))
        //        {
        //            MessageBox.Show(String.Format("Failed to get Map per Airmass for {0}rpm {1} Cam Angle, this is a fatal error, giving up, sorry bro", rpm, camAngle));
        //            return;
        //        }

        //        newCsvData[i][ indexOfMapPerAirmass] = mapPerAirmass;
        //        newCsvData[i][ indexOfMapPerZeroAirmass] = mapPerZeroAirmass;

        //        //Calculate the airmass
        //        double airMassLbs = (map - mapPerZeroAirmass) / mapPerAirmass;
        //        newCsvData[i][indexOfAirMass] = airMassLbs;

        //        //Calculate the load
        //        newCsvData[i][indexOfCalculatedLoad] = airMassLbs / displacement;

        //        //Calculate the fuelmass via the SD maps
        //        double fuelMassViaSD = airMassLbs / (stoichAFR * commandedLambda);

        //        newCsvData[i][indexOfFuelMassViaSD] = fuelMassViaSD;

        //        //Calculate the pulse width
        //        double calculatedPulseWidthSec;
        //        //When below the breakpoint

        //        if (fuelMassViaSD <= breakpoint)
        //        {
        //            calculatedPulseWidthSec = (fuelMassViaSD / lowSlope) + offset;
        //        }
        //        else
        //        {
        //            //fuelmass=high*(ms-(breakpoint*((1/low)-(1/high))+offset))
        //            //ms=(breakpoint*((1/low)-(1/high))+(fuelmass/highslope)+offset
        //            calculatedPulseWidthSec = (breakpoint * ((1 / lowSlope) - (1 / highSlope))) + (fuelMassViaSD / highSlope) + offset;
        //        }

        //        double calculatedPulseWithMs = (calculatedPulseWidthSec - offset) * 1000.0;
        //        //double calculatedPulseWithMs = (calculatedPulseWidthSec) * 1000.0;

        //        //This is the low cut off
        //        if (calculatedPulseWithMs < 0.2) calculatedPulseWithMs = 0.2;

        //        double csvPulseWidthMs = csvPulseWidthSeconds * 1000.0;

        //        newCsvData[i][indexOfCalculatedInjectorPW] = calculatedPulseWithMs;
        //        double trimmedInjPulse = calculatedPulseWithMs * fuelTrim;

        //        newCsvData[i][indexOfCalculatedInjectorPWTrimmed] = trimmedInjPulse;

        //        //Calculate the fuel mass via a reverse calc of the injector pulse width

        //        double breakpointMs = ((breakpoint / lowSlope) + offset) * 1000.0;

        //        double fuelMassViaInjectorMs;
        //        if(csvPulseWidthMs < breakpointMs)
        //        {
        //            //fuelMassViaInjectorMs = lowSlope * (csvPulseWidthSeconds - offset);
        //            fuelMassViaInjectorMs = lowSlope * (csvPulseWidthSeconds);
        //        }
        //        else
        //        {
        //            //fuelMassViaInjectorMs = highSlope * (csvPulseWidthSeconds - (breakpoint * ((1 / lowSlope) - (1 / highSlope)) + offset));
        //            fuelMassViaInjectorMs = highSlope * (csvPulseWidthSeconds - (breakpoint * ((1 / lowSlope) - (1 / highSlope))));
        //        }


        //        newCsvData[i][indexOfFuelMassViaInjMs] = fuelMassViaInjectorMs;

                

        //        newCsvData[i][indexOfPulse] = newCsvData[i][indexOfPulse] * 1000.0;
                    

        //    }
        //    //});

        //    //Calculate the R² value

        //    double errorOfCalculatedInjPulse = GetRSquared(newCsvData, indexOfCalculatedInjectorPW, indexOfPulse, 1.0, 1000.0);
        //    double errorOfCalculatedInjPulseTrimmed = GetRSquared(newCsvData, indexOfCalculatedInjectorPWTrimmed, indexOfPulse, 1.0, 1000.0);
        //    double errorOfCalculatedFuelMassPulse = GetRSquared(newCsvData, indexOfFuelMassViaSD, indexOfFuelMassViaInjMs, 1.0, 1.0);

        //    csvHeaders = newCsvHeaders;
        //    csvData = newCsvData;

        //    HelperMethods.WriteCSV(@"C:\temp\test.csv", csvHeaders, csvData);

        //    returnArray[0] = errorOfCalculatedInjPulse;
        //    returnArray[1] = errorOfCalculatedInjPulseTrimmed;
        //    returnArray[2] = errorOfCalculatedFuelMassPulse;

        //    if (HelperMethods.IsValidDouble(returnArray[0])) calculatedInjectorPulseErrorTextBox.Text = String.Format("{0:0.###}", returnArray[0]);

        //    if (HelperMethods.IsValidDouble(returnArray[1])) calculatedInjectorTrimmedPulseErrorTextBox.Text = String.Format("{0:0.###}", returnArray[1]);
        //    if (HelperMethods.IsValidDouble(returnArray[2])) calculatedFuelMassErrorTextBox.Text = String.Format("{0:0.###}", returnArray[2]);

        //    return;
        //}


        async Task EmulatePCM(IProgress<int> progress, CancellationToken cts)
        {
            Boolean failed = false;
            double[] results = await Task.Run<double[]>(() =>
            {
                double[] returnArray = new double[2];
                returnArray[0] = double.NaN;
                returnArray[1] = double.NaN;

                if (fordPCMHelper == null)
                {
                    MessageBox.Show("You must load a .CSV file first!");
                    return returnArray;
                }

                if (!fordPCMHelper.fileLoaded)
                {
                    MessageBox.Show("You must load a .HPT file first!");
                    return returnArray;
                }

                int indexOfPulse, indexOfCalculatedLoad, indexOfMAP, indexOfTPS, indexOfCamAngle, indexOfRPM, indexOfMapPerAirmass, indexOfMapPerZeroAirmass, indexOfAirMass, indexOfFuelMassViaSD, indexOfFuelMassViaInjMs, indexOfCalculatedInjectorPW, indexOfCalculatedAFR, indexOfCalculatedInjectorPWTrimmed, indexOfFuelTrim;


                indexOfCamAngle = Array.FindIndex(csvHeaders, x => (x.IndexOf("cam angle", StringComparison.OrdinalIgnoreCase) >= 0));
                indexOfRPM = Array.FindIndex(csvHeaders, x => (x.IndexOf("rpm", StringComparison.OrdinalIgnoreCase) >= 0));
                indexOfTPS = Array.FindIndex(csvHeaders, x => (x.IndexOf("ETC throttle", StringComparison.OrdinalIgnoreCase) >= 0));
                indexOfMAP = Array.FindIndex(csvHeaders, x => (x.IndexOf("MANIFOLD ABSOLUTE PRESSURE", StringComparison.OrdinalIgnoreCase) >= 0));
                indexOfPulse = Array.FindIndex(csvHeaders, x => (x.IndexOf("FUEL PULSEWIDTH", StringComparison.OrdinalIgnoreCase) >= 0));
                indexOfFuelTrim = Array.FindIndex(csvHeaders, x => (x.IndexOf("FUEL TRIM", StringComparison.OrdinalIgnoreCase) >= 0));

                if (indexOfMAP == -1)
                {
                    MessageBox.Show("Couldn't find MAP entry in csv data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return returnArray;
                }

                if (indexOfCamAngle == -1)
                {
                    MessageBox.Show("Couldn't find cam angle entry in csv data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return returnArray;
                }

                if (indexOfRPM == -1)
                {
                    MessageBox.Show("Couldn't find rpm entry in csv data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return returnArray;
                }

                if (indexOfTPS == -1)
                {
                    MessageBox.Show("Couldn't find TPS entry in csv data, we will assume commanded lambda is always 1.0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return returnArray;
                }

                indexOfMapPerAirmass = Array.IndexOf(csvHeaders, "Map per Airmass");
                indexOfMapPerZeroAirmass = Array.IndexOf(csvHeaders, "Map per Zero Airmass");
                indexOfAirMass = Array.IndexOf(csvHeaders, "Calculated Airmass");
                indexOfFuelMassViaSD = Array.IndexOf(csvHeaders, "Calculated Fuel Mass via SD");
                indexOfFuelMassViaInjMs = Array.IndexOf(csvHeaders, "Calculated Fuel Mass via Pulsewidth");
                indexOfCalculatedInjectorPW = Array.IndexOf(csvHeaders, "Calculated Injector Pulsewidth");
                indexOfCalculatedInjectorPWTrimmed = Array.IndexOf(csvHeaders, "Calculated Injector Pulsewidth With Fuel Trim");
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
                if (indexOfFuelMassViaSD == -1)
                {
                    indexOfFuelMassViaSD = csvHeaders.Length + arrayResizeAmount;
                    arrayResizeAmount++;
                }
                if (indexOfFuelMassViaInjMs == -1)
                {
                    indexOfFuelMassViaInjMs = csvHeaders.Length + arrayResizeAmount;
                    arrayResizeAmount++;
                }
                if (indexOfCalculatedInjectorPW == -1)
                {
                    indexOfCalculatedInjectorPW = csvHeaders.Length + arrayResizeAmount;
                    arrayResizeAmount++;
                }
                if (indexOfCalculatedInjectorPWTrimmed == -1)
                {
                    indexOfCalculatedInjectorPWTrimmed = csvHeaders.Length + arrayResizeAmount;
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
                double[][] newCsvData = new double[csvData.Length][];


                //Copy the previous data
                Array.Copy(csvHeaders, newCsvHeaders, csvHeaders.Length);
                csvHeaders.CopyTo(newCsvHeaders, 0);

                for (int i = 0; i < csvData.Length; i++)
                {
                    newCsvData[i] = new double[csvData[0].Length + arrayResizeAmount];
                    for (int j = 0; j < csvData[0].Length; j++)
                    {
                        newCsvData[i][j] = csvData[i][j];
                    }
                }

                //Add the new headers
                newCsvHeaders[indexOfMapPerAirmass] = "Map per Airmass";
                newCsvHeaders[indexOfMapPerZeroAirmass] = "Map per Zero Airmass";
                newCsvHeaders[indexOfAirMass] = "Calculated Airmass";
                newCsvHeaders[indexOfFuelMassViaSD] = "Calculated Fuel Mass via SD";
                newCsvHeaders[indexOfFuelMassViaInjMs] = "Calculated Fuel Mass via Pulsewidth";
                newCsvHeaders[indexOfCalculatedInjectorPW] = "Calculated Injector Pulsewidth";
                newCsvHeaders[indexOfCalculatedInjectorPWTrimmed] = "Calculated Injector Pulsewidth With Fuel Trim";
                newCsvHeaders[indexOfCalculatedAFR] = "Calculated Commanded AFR";
                newCsvHeaders[indexOfCalculatedLoad] = "Calculated Load";


                //get stoich
                double stoichAFR;
                if (!fordPCMHelper.TryGetDouble(2300, out stoichAFR))
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

                int samplesProcessed = 0;
                int totalCount = csvData.Length;
                Parallel.For(0, totalCount, i =>
                {
                    if (failed) return;

                    double camAngle = csvData[i][indexOfCamAngle];
                    double rpm = csvData[i][indexOfRPM];
                    double map = csvData[i][indexOfMAP];
                    double csvPulseWidthSeconds = csvData[i][indexOfPulse];
                    double fuelTrim = csvData[i][indexOfFuelTrim];
                    double TPS = -1.0;
                    double commandedLambda = 1.0;

                    //Get the commanded lambda value
                    if (indexOfTPS != -1)
                    {
                        TPS = csvData[i][indexOfTPS];
                        TPS *= 1.25;
                        if (!fordPCMHelper.TryGetTableValue(rpm, TPS, 50151, out commandedLambda))
                        {
                            if (!lambdaErrorIssued) MessageBox.Show(String.Format("Failed to get commanded lambda for {0}rpm {1}TPS%, we will use 1.0 from now on and no longer report anymore errors of this type", rpm, TPS));
                            lambdaErrorIssued = true;
                            commandedLambda = 1.0;
                        }

                    }
                    newCsvData[i][indexOfCalculatedAFR] = commandedLambda;

                    //interpolate the map per airmass values
                    double mapPerAirmass;
                    double mapPerZeroAirmass;
                    if (!fordPCMHelper.TryGetTableValue(rpm, camAngle, 50051, out mapPerZeroAirmass))
                    {
                        MessageBox.Show(String.Format("Failed to get Map per Zero Airmass for {0}rpm {1} Cam Angle, this is a fatal error, giving up, sorry bro", rpm, camAngle));
                        failed = true;
                        return;
                    }
                    if (!fordPCMHelper.TryGetTableValue(rpm, camAngle, 50055, out mapPerAirmass))
                    {
                        MessageBox.Show(String.Format("Failed to get Map per Airmass for {0}rpm {1} Cam Angle, this is a fatal error, giving up, sorry bro", rpm, camAngle));
                        failed = true;
                        return;
                    }

                    newCsvData[i][indexOfMapPerAirmass] = mapPerAirmass;
                    newCsvData[i][indexOfMapPerZeroAirmass] = mapPerZeroAirmass;

                    //Calculate the airmass
                    double airMassLbs = (map - mapPerZeroAirmass) / mapPerAirmass;
                    newCsvData[i][indexOfAirMass] = airMassLbs;

                    //Calculate the load
                    newCsvData[i][indexOfCalculatedLoad] = airMassLbs / displacement;

                    //Calculate the fuelmass via the SD maps
                    double fuelMassViaSD = airMassLbs / (stoichAFR * commandedLambda);

                    newCsvData[i][indexOfFuelMassViaSD] = fuelMassViaSD;

                    //Calculate the pulse width
                    double calculatedPulseWidthSec;
                    //When below the breakpoint

                    if (fuelMassViaSD <= breakpoint)
                    {
                        calculatedPulseWidthSec = (fuelMassViaSD / lowSlope) + offset;
                    }
                    else
                    {
                        //fuelmass=high*(ms-(breakpoint*((1/low)-(1/high))+offset))
                        //ms=(breakpoint*((1/low)-(1/high))+(fuelmass/highslope)+offset
                        calculatedPulseWidthSec = (breakpoint * ((1 / lowSlope) - (1 / highSlope))) + (fuelMassViaSD / highSlope) + offset;
                    }

                    double calculatedPulseWithMs = (calculatedPulseWidthSec - offset) * 1000.0;

                    //This is the low cut off
                    if (calculatedPulseWithMs < 0.2) calculatedPulseWithMs = 0.2;

                    double csvPulseWidthMs = csvPulseWidthSeconds * 1000.0;

                    newCsvData[i][indexOfCalculatedInjectorPW] = calculatedPulseWithMs;
                    double trimmedInjPulse = calculatedPulseWithMs * fuelTrim;

                    newCsvData[i][indexOfCalculatedInjectorPWTrimmed] = trimmedInjPulse;

                    //Calculate the fuel mass via a reverse calc of the injector pulse width

                    double breakpointMs = ((breakpoint / lowSlope) + offset) * 1000.0;

                    double fuelMassViaInjectorMs;
                    if (csvPulseWidthMs < breakpointMs)
                    {
                        fuelMassViaInjectorMs = lowSlope * (csvPulseWidthSeconds);
                    }
                    else
                    {
                        fuelMassViaInjectorMs = highSlope * (csvPulseWidthSeconds - (breakpoint * ((1 / lowSlope) - (1 / highSlope))));
                    }


                    newCsvData[i][indexOfFuelMassViaInjMs] = fuelMassViaInjectorMs;

                    newCsvData[i][indexOfPulse] = newCsvData[i][indexOfPulse] * 1000.0;

                    samplesProcessed++;
                    if (samplesProcessed % 120 == 0)
                    {
                        progress.Report((samplesProcessed * 100 / totalCount));
                    }
                });

                if (failed) return null;

                //Calculate the R² value

                double errorOfCalculatedInjPulse = GetRSquared(newCsvData, indexOfCalculatedInjectorPW, indexOfPulse, 1.0, 1000.0);
                double errorOfCalculatedInjPulseTrimmed = GetRSquared(newCsvData, indexOfCalculatedInjectorPWTrimmed, indexOfPulse, 1.0, 1000.0);
                double errorOfCalculatedFuelMassPulse = GetRSquared(newCsvData, indexOfFuelMassViaSD, indexOfFuelMassViaInjMs, 1.0, 1.0);

                csvHeaders = newCsvHeaders;
                csvData = newCsvData;

                HelperMethods.WriteCSV(@"C:\temp\test.csv", csvHeaders, csvData);

                returnArray[0] = errorOfCalculatedInjPulse;
                returnArray[1] = errorOfCalculatedInjPulseTrimmed;
                returnArray[2] = errorOfCalculatedFuelMassPulse;
                return returnArray;
            });

            if (failed) return;

            if (HelperMethods.IsValidDouble(results[0])) calculatedInjectorPulseErrorTextBox.Text = String.Format("{0:0.###}", results[0]);

            if (HelperMethods.IsValidDouble(results[1])) calculatedInjectorTrimmedPulseErrorTextBox.Text = String.Format("{0:0.###}", results[1]);
            if (HelperMethods.IsValidDouble(results[2])) calculatedFuelMassErrorTextBox.Text = String.Format("{0:0.###}", results[2]);
        }

        private void TaskIsRunning()
        {
            // Update UI to reflect background task.

        }

        private void TaskIsComplete()
        {
            // Reset UI.
            this.progressBar.Value = 0;
            this.calculateAirmassButton.Enabled = true;
            this.cancelButton.Enabled = false;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            cts.Cancel();
        }
    }

}
