namespace DataAnalyser
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.PlotTab = new System.Windows.Forms.TabPage();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.usd = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.xsd = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.ysd = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.zsd = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.XAxisMultiplierTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ZAxisMultiplierTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.UAxisMultiplierTextBox = new System.Windows.Forms.TextBox();
            this.YAxisMultiplierTextBox = new System.Windows.Forms.TextBox();
            this.ChangeMainYAxisButton = new System.Windows.Forms.Button();
            this.ChangeMainXAxisButton = new System.Windows.Forms.Button();
            this.UComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.zAxisComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.yAxisComboBox = new System.Windows.Forms.ComboBox();
            this.xAxisComboBox = new System.Windows.Forms.ComboBox();
            this.labelYAxis = new System.Windows.Forms.Label();
            this.labelXAxis = new System.Windows.Forms.Label();
            this.checkPulseButton = new System.Windows.Forms.Button();
            this.averageButton = new System.Windows.Forms.Button();
            this.SDButton = new System.Windows.Forms.Button();
            this.countButton = new System.Windows.Forms.Button();
            this.maxButton = new System.Windows.Forms.Button();
            this.minButton = new System.Windows.Forms.Button();
            this.SecondaryGridView = new System.Windows.Forms.DataGridView();
            this.MainGridView = new System.Windows.Forms.DataGridView();
            this.CalculateTab = new System.Windows.Forms.TabPage();
            this.hptTab = new System.Windows.Forms.TabPage();
            this.fordPCMHelper = new FordPCMEditor.FordPCMHelper();
            this.LoadButton = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.ignoreCellTextBox = new System.Windows.Forms.TextBox();
            this.pcmSimulator1 = new DataAnalyser.PCMSimulator();
            this.tabControl1.SuspendLayout();
            this.PlotTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SecondaryGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainGridView)).BeginInit();
            this.CalculateTab.SuspendLayout();
            this.hptTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.PlotTab);
            this.tabControl1.Controls.Add(this.CalculateTab);
            this.tabControl1.Controls.Add(this.hptTab);
            this.tabControl1.Location = new System.Drawing.Point(12, 40);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1553, 619);
            this.tabControl1.TabIndex = 0;
            // 
            // PlotTab
            // 
            this.PlotTab.Controls.Add(this.label15);
            this.PlotTab.Controls.Add(this.ignoreCellTextBox);
            this.PlotTab.Controls.Add(this.label13);
            this.PlotTab.Controls.Add(this.label14);
            this.PlotTab.Controls.Add(this.usd);
            this.PlotTab.Controls.Add(this.label11);
            this.PlotTab.Controls.Add(this.label12);
            this.PlotTab.Controls.Add(this.xsd);
            this.PlotTab.Controls.Add(this.label9);
            this.PlotTab.Controls.Add(this.label10);
            this.PlotTab.Controls.Add(this.ysd);
            this.PlotTab.Controls.Add(this.label8);
            this.PlotTab.Controls.Add(this.label3);
            this.PlotTab.Controls.Add(this.zsd);
            this.PlotTab.Controls.Add(this.label7);
            this.PlotTab.Controls.Add(this.label6);
            this.PlotTab.Controls.Add(this.XAxisMultiplierTextBox);
            this.PlotTab.Controls.Add(this.label5);
            this.PlotTab.Controls.Add(this.ZAxisMultiplierTextBox);
            this.PlotTab.Controls.Add(this.label4);
            this.PlotTab.Controls.Add(this.UAxisMultiplierTextBox);
            this.PlotTab.Controls.Add(this.YAxisMultiplierTextBox);
            this.PlotTab.Controls.Add(this.ChangeMainYAxisButton);
            this.PlotTab.Controls.Add(this.ChangeMainXAxisButton);
            this.PlotTab.Controls.Add(this.UComboBox);
            this.PlotTab.Controls.Add(this.label2);
            this.PlotTab.Controls.Add(this.zAxisComboBox);
            this.PlotTab.Controls.Add(this.label1);
            this.PlotTab.Controls.Add(this.button1);
            this.PlotTab.Controls.Add(this.yAxisComboBox);
            this.PlotTab.Controls.Add(this.xAxisComboBox);
            this.PlotTab.Controls.Add(this.labelYAxis);
            this.PlotTab.Controls.Add(this.labelXAxis);
            this.PlotTab.Controls.Add(this.checkPulseButton);
            this.PlotTab.Controls.Add(this.averageButton);
            this.PlotTab.Controls.Add(this.SDButton);
            this.PlotTab.Controls.Add(this.countButton);
            this.PlotTab.Controls.Add(this.maxButton);
            this.PlotTab.Controls.Add(this.minButton);
            this.PlotTab.Controls.Add(this.SecondaryGridView);
            this.PlotTab.Controls.Add(this.MainGridView);
            this.PlotTab.Location = new System.Drawing.Point(4, 22);
            this.PlotTab.Name = "PlotTab";
            this.PlotTab.Padding = new System.Windows.Forms.Padding(3);
            this.PlotTab.Size = new System.Drawing.Size(1545, 593);
            this.PlotTab.TabIndex = 0;
            this.PlotTab.Text = "3D Plot View";
            this.PlotTab.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(1032, 110);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(103, 13);
            this.label13.TabIndex = 42;
            this.label13.Text = "Standard Deviations";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(880, 110);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(102, 13);
            this.label14.TabIndex = 41;
            this.label14.Text = "Ignore Data Beyond";
            // 
            // usd
            // 
            this.usd.Location = new System.Drawing.Point(988, 106);
            this.usd.Name = "usd";
            this.usd.Size = new System.Drawing.Size(38, 20);
            this.usd.TabIndex = 40;
            this.usd.Text = "3.14";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(158, 112);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(103, 13);
            this.label11.TabIndex = 39;
            this.label11.Text = "Standard Deviations";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 112);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(102, 13);
            this.label12.TabIndex = 38;
            this.label12.Text = "Ignore Data Beyond";
            // 
            // xsd
            // 
            this.xsd.Location = new System.Drawing.Point(114, 108);
            this.xsd.Name = "xsd";
            this.xsd.Size = new System.Drawing.Size(38, 20);
            this.xsd.TabIndex = 37;
            this.xsd.Text = "3.14";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(422, 112);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(103, 13);
            this.label9.TabIndex = 36;
            this.label9.Text = "Standard Deviations";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(270, 112);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 13);
            this.label10.TabIndex = 35;
            this.label10.Text = "Ignore Data Beyond";
            // 
            // ysd
            // 
            this.ysd.Location = new System.Drawing.Point(378, 108);
            this.ysd.Name = "ysd";
            this.ysd.Size = new System.Drawing.Size(38, 20);
            this.ysd.TabIndex = 34;
            this.ysd.Text = "3.14";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(715, 111);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(103, 13);
            this.label8.TabIndex = 33;
            this.label8.Text = "Standard Deviations";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(563, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 13);
            this.label3.TabIndex = 32;
            this.label3.Text = "Ignore Data Beyond";
            // 
            // zsd
            // 
            this.zsd.Location = new System.Drawing.Point(671, 107);
            this.zsd.Name = "zsd";
            this.zsd.Size = new System.Drawing.Size(38, 20);
            this.zsd.TabIndex = 31;
            this.zsd.Text = "3.14";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(903, 85);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "U Axis Multiplier";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 81);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 13);
            this.label6.TabIndex = 29;
            this.label6.Text = "X Axis Multiplier";
            // 
            // XAxisMultiplierTextBox
            // 
            this.XAxisMultiplierTextBox.Location = new System.Drawing.Point(114, 78);
            this.XAxisMultiplierTextBox.Name = "XAxisMultiplierTextBox";
            this.XAxisMultiplierTextBox.Size = new System.Drawing.Size(100, 20);
            this.XAxisMultiplierTextBox.TabIndex = 28;
            this.XAxisMultiplierTextBox.Text = "1.0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(587, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "Z Axis Multiplier";
            // 
            // ZAxisMultiplierTextBox
            // 
            this.ZAxisMultiplierTextBox.Location = new System.Drawing.Point(673, 82);
            this.ZAxisMultiplierTextBox.Name = "ZAxisMultiplierTextBox";
            this.ZAxisMultiplierTextBox.Size = new System.Drawing.Size(100, 20);
            this.ZAxisMultiplierTextBox.TabIndex = 26;
            this.ZAxisMultiplierTextBox.Text = "1.0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(292, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "Y Axis Multiplier";
            // 
            // UAxisMultiplierTextBox
            // 
            this.UAxisMultiplierTextBox.Location = new System.Drawing.Point(989, 82);
            this.UAxisMultiplierTextBox.Name = "UAxisMultiplierTextBox";
            this.UAxisMultiplierTextBox.Size = new System.Drawing.Size(100, 20);
            this.UAxisMultiplierTextBox.TabIndex = 24;
            this.UAxisMultiplierTextBox.Text = "1.0";
            // 
            // YAxisMultiplierTextBox
            // 
            this.YAxisMultiplierTextBox.Location = new System.Drawing.Point(378, 78);
            this.YAxisMultiplierTextBox.Name = "YAxisMultiplierTextBox";
            this.YAxisMultiplierTextBox.Size = new System.Drawing.Size(100, 20);
            this.YAxisMultiplierTextBox.TabIndex = 23;
            this.YAxisMultiplierTextBox.Text = "1.0";
            // 
            // ChangeMainYAxisButton
            // 
            this.ChangeMainYAxisButton.Location = new System.Drawing.Point(105, 146);
            this.ChangeMainYAxisButton.Name = "ChangeMainYAxisButton";
            this.ChangeMainYAxisButton.Size = new System.Drawing.Size(96, 23);
            this.ChangeMainYAxisButton.TabIndex = 22;
            this.ChangeMainYAxisButton.Text = "Change Y Axis";
            this.ChangeMainYAxisButton.UseVisualStyleBackColor = true;
            this.ChangeMainYAxisButton.Click += new System.EventHandler(this.ChangeMainYAxisButton_Click);
            // 
            // ChangeMainXAxisButton
            // 
            this.ChangeMainXAxisButton.Location = new System.Drawing.Point(3, 146);
            this.ChangeMainXAxisButton.Name = "ChangeMainXAxisButton";
            this.ChangeMainXAxisButton.Size = new System.Drawing.Size(96, 23);
            this.ChangeMainXAxisButton.TabIndex = 20;
            this.ChangeMainXAxisButton.Text = "Change X Axis";
            this.ChangeMainXAxisButton.UseVisualStyleBackColor = true;
            this.ChangeMainXAxisButton.Click += new System.EventHandler(this.ChangeMainAxisButton_Click);
            // 
            // UComboBox
            // 
            this.UComboBox.FormattingEnabled = true;
            this.UComboBox.Location = new System.Drawing.Point(885, 52);
            this.UComboBox.Name = "UComboBox";
            this.UComboBox.Size = new System.Drawing.Size(280, 21);
            this.UComboBox.TabIndex = 19;
            this.UComboBox.DragLeave += new System.EventHandler(this.UComboBox_DragLeave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(882, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "U Value";
            // 
            // zAxisComboBox
            // 
            this.zAxisComboBox.FormattingEnabled = true;
            this.zAxisComboBox.Location = new System.Drawing.Point(590, 52);
            this.zAxisComboBox.Name = "zAxisComboBox";
            this.zAxisComboBox.Size = new System.Drawing.Size(280, 21);
            this.zAxisComboBox.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(587, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Z Value";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(795, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "Display";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // yAxisComboBox
            // 
            this.yAxisComboBox.FormattingEnabled = true;
            this.yAxisComboBox.Location = new System.Drawing.Point(295, 52);
            this.yAxisComboBox.Name = "yAxisComboBox";
            this.yAxisComboBox.Size = new System.Drawing.Size(288, 21);
            this.yAxisComboBox.TabIndex = 14;
            // 
            // xAxisComboBox
            // 
            this.xAxisComboBox.FormattingEnabled = true;
            this.xAxisComboBox.Items.AddRange(new object[] {
            "rpm"});
            this.xAxisComboBox.Location = new System.Drawing.Point(9, 52);
            this.xAxisComboBox.Name = "xAxisComboBox";
            this.xAxisComboBox.Size = new System.Drawing.Size(280, 21);
            this.xAxisComboBox.TabIndex = 13;
            // 
            // labelYAxis
            // 
            this.labelYAxis.AutoSize = true;
            this.labelYAxis.Location = new System.Drawing.Point(292, 36);
            this.labelYAxis.Name = "labelYAxis";
            this.labelYAxis.Size = new System.Drawing.Size(36, 13);
            this.labelYAxis.TabIndex = 12;
            this.labelYAxis.Text = "Y Axis";
            // 
            // labelXAxis
            // 
            this.labelXAxis.AutoSize = true;
            this.labelXAxis.Location = new System.Drawing.Point(6, 36);
            this.labelXAxis.Name = "labelXAxis";
            this.labelXAxis.Size = new System.Drawing.Size(36, 13);
            this.labelXAxis.TabIndex = 10;
            this.labelXAxis.Text = "X Axis";
            // 
            // checkPulseButton
            // 
            this.checkPulseButton.Location = new System.Drawing.Point(885, 146);
            this.checkPulseButton.Name = "checkPulseButton";
            this.checkPulseButton.Size = new System.Drawing.Size(262, 23);
            this.checkPulseButton.TabIndex = 8;
            this.checkPulseButton.Text = "View Selected Cell vs U";
            this.checkPulseButton.UseVisualStyleBackColor = true;
            this.checkPulseButton.Click += new System.EventHandler(this.checkPulseButton_Click);
            // 
            // averageButton
            // 
            this.averageButton.Location = new System.Drawing.Point(5, 6);
            this.averageButton.Name = "averageButton";
            this.averageButton.Size = new System.Drawing.Size(75, 23);
            this.averageButton.TabIndex = 7;
            this.averageButton.Text = "Average";
            this.averageButton.UseVisualStyleBackColor = true;
            this.averageButton.Click += new System.EventHandler(this.averageButton_Click);
            // 
            // SDButton
            // 
            this.SDButton.Location = new System.Drawing.Point(329, 6);
            this.SDButton.Name = "SDButton";
            this.SDButton.Size = new System.Drawing.Size(96, 23);
            this.SDButton.TabIndex = 6;
            this.SDButton.Text = "Standard Dev";
            this.SDButton.UseVisualStyleBackColor = true;
            this.SDButton.Click += new System.EventHandler(this.SDButton_Click);
            // 
            // countButton
            // 
            this.countButton.Location = new System.Drawing.Point(248, 6);
            this.countButton.Name = "countButton";
            this.countButton.Size = new System.Drawing.Size(75, 23);
            this.countButton.TabIndex = 5;
            this.countButton.Text = "Count";
            this.countButton.UseVisualStyleBackColor = true;
            this.countButton.Click += new System.EventHandler(this.countButton_Click);
            // 
            // maxButton
            // 
            this.maxButton.Location = new System.Drawing.Point(167, 6);
            this.maxButton.Name = "maxButton";
            this.maxButton.Size = new System.Drawing.Size(75, 23);
            this.maxButton.TabIndex = 4;
            this.maxButton.Text = "Max";
            this.maxButton.UseVisualStyleBackColor = true;
            this.maxButton.Click += new System.EventHandler(this.maxButton_Click);
            // 
            // minButton
            // 
            this.minButton.Location = new System.Drawing.Point(86, 6);
            this.minButton.Name = "minButton";
            this.minButton.Size = new System.Drawing.Size(75, 23);
            this.minButton.TabIndex = 3;
            this.minButton.Text = "Min";
            this.minButton.UseVisualStyleBackColor = true;
            this.minButton.Click += new System.EventHandler(this.minButton_Click);
            // 
            // SecondaryGridView
            // 
            this.SecondaryGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SecondaryGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SecondaryGridView.Location = new System.Drawing.Point(885, 175);
            this.SecondaryGridView.Name = "SecondaryGridView";
            this.SecondaryGridView.Size = new System.Drawing.Size(654, 412);
            this.SecondaryGridView.TabIndex = 2;
            this.SecondaryGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.SecondaryGridView_ColumnHeaderMouseClick);
            this.SecondaryGridView.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.SecondaryGridView_RowHeaderMouseClick);
            // 
            // MainGridView
            // 
            this.MainGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.MainGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MainGridView.Location = new System.Drawing.Point(5, 175);
            this.MainGridView.Name = "MainGridView";
            this.MainGridView.Size = new System.Drawing.Size(865, 412);
            this.MainGridView.TabIndex = 1;
            this.MainGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MainGridView_ColumnHeaderMouseClick);
            this.MainGridView.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MainGridView_RowHeaderMouseClick);
            // 
            // CalculateTab
            // 
            this.CalculateTab.Controls.Add(this.pcmSimulator1);
            this.CalculateTab.Location = new System.Drawing.Point(4, 22);
            this.CalculateTab.Name = "CalculateTab";
            this.CalculateTab.Size = new System.Drawing.Size(1545, 593);
            this.CalculateTab.TabIndex = 2;
            this.CalculateTab.Text = "Calculate Variables";
            this.CalculateTab.UseVisualStyleBackColor = true;
            // 
            // hptTab
            // 
            this.hptTab.Controls.Add(this.fordPCMHelper);
            this.hptTab.Location = new System.Drawing.Point(4, 22);
            this.hptTab.Name = "hptTab";
            this.hptTab.Padding = new System.Windows.Forms.Padding(3);
            this.hptTab.Size = new System.Drawing.Size(1545, 593);
            this.hptTab.TabIndex = 1;
            this.hptTab.Text = "HPT File Viewer";
            this.hptTab.UseVisualStyleBackColor = true;
            // 
            // fordPCMHelper
            // 
            this.fordPCMHelper.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fordPCMHelper.Location = new System.Drawing.Point(6, 0);
            this.fordPCMHelper.Name = "fordPCMHelper";
            this.fordPCMHelper.Size = new System.Drawing.Size(1543, 587);
            this.fordPCMHelper.TabIndex = 0;
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(12, 11);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(106, 23);
            this.LoadButton.TabIndex = 1;
            this.LoadButton.Text = "Load CSV Data";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(665, 11);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(77, 13);
            this.label15.TabIndex = 44;
            this.label15.Text = "Ignore Cells  < ";
            // 
            // ignoreCellTextBox
            // 
            this.ignoreCellTextBox.Location = new System.Drawing.Point(748, 7);
            this.ignoreCellTextBox.Name = "ignoreCellTextBox";
            this.ignoreCellTextBox.Size = new System.Drawing.Size(38, 20);
            this.ignoreCellTextBox.TabIndex = 43;
            this.ignoreCellTextBox.Text = "50";
            // 
            // pcmSimulator1
            // 
            this.pcmSimulator1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pcmSimulator1.Location = new System.Drawing.Point(3, 3);
            this.pcmSimulator1.Name = "pcmSimulator1";
            this.pcmSimulator1.Size = new System.Drawing.Size(1537, 578);
            this.pcmSimulator1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1568, 680);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainForm";
            this.Text = "DataAnalyser";
            this.tabControl1.ResumeLayout(false);
            this.PlotTab.ResumeLayout(false);
            this.PlotTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SecondaryGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainGridView)).EndInit();
            this.CalculateTab.ResumeLayout(false);
            this.hptTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.TabPage PlotTab;
        private System.Windows.Forms.ComboBox yAxisComboBox;
        private System.Windows.Forms.ComboBox xAxisComboBox;
        private System.Windows.Forms.Label labelYAxis;
        private System.Windows.Forms.Label labelXAxis;
        private System.Windows.Forms.Button checkPulseButton;
        private System.Windows.Forms.Button averageButton;
        private System.Windows.Forms.Button SDButton;
        private System.Windows.Forms.Button countButton;
        private System.Windows.Forms.Button maxButton;
        private System.Windows.Forms.Button minButton;
        private System.Windows.Forms.DataGridView SecondaryGridView;
        private System.Windows.Forms.DataGridView MainGridView;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox zAxisComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox UComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ChangeMainXAxisButton;
        private System.Windows.Forms.Button ChangeMainYAxisButton;
        private System.Windows.Forms.TabPage hptTab;
        private System.Windows.Forms.TabPage CalculateTab;
        private System.Windows.Forms.TextBox YAxisMultiplierTextBox;
        private System.Windows.Forms.TextBox UAxisMultiplierTextBox;
        private FordPCMEditor.FordPCMHelper fordPCMHelper;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox ZAxisMultiplierTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox XAxisMultiplierTextBox;
        private PCMSimulator pcmSimulator1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox usd;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox xsd;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox zsd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox ysd;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox ignoreCellTextBox;
    }
}

