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
            this.tabPage1 = new System.Windows.Forms.TabPage();
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
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.LoadButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SecondaryGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainGridView)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 40);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1544, 614);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ChangeMainYAxisButton);
            this.tabPage1.Controls.Add(this.ChangeMainXAxisButton);
            this.tabPage1.Controls.Add(this.UComboBox);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.zAxisComboBox);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.yAxisComboBox);
            this.tabPage1.Controls.Add(this.xAxisComboBox);
            this.tabPage1.Controls.Add(this.labelYAxis);
            this.tabPage1.Controls.Add(this.labelXAxis);
            this.tabPage1.Controls.Add(this.checkPulseButton);
            this.tabPage1.Controls.Add(this.averageButton);
            this.tabPage1.Controls.Add(this.SDButton);
            this.tabPage1.Controls.Add(this.countButton);
            this.tabPage1.Controls.Add(this.maxButton);
            this.tabPage1.Controls.Add(this.minButton);
            this.tabPage1.Controls.Add(this.SecondaryGridView);
            this.tabPage1.Controls.Add(this.MainGridView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1536, 588);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "3D Plot View";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ChangeMainYAxisButton
            // 
            this.ChangeMainYAxisButton.Location = new System.Drawing.Point(108, 109);
            this.ChangeMainYAxisButton.Name = "ChangeMainYAxisButton";
            this.ChangeMainYAxisButton.Size = new System.Drawing.Size(96, 23);
            this.ChangeMainYAxisButton.TabIndex = 22;
            this.ChangeMainYAxisButton.Text = "Change Y Axis";
            this.ChangeMainYAxisButton.UseVisualStyleBackColor = true;
            this.ChangeMainYAxisButton.Click += new System.EventHandler(this.ChangeMainYAxisButton_Click);
            // 
            // ChangeMainXAxisButton
            // 
            this.ChangeMainXAxisButton.Location = new System.Drawing.Point(6, 109);
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
            this.UComboBox.Location = new System.Drawing.Point(876, 111);
            this.UComboBox.Name = "UComboBox";
            this.UComboBox.Size = new System.Drawing.Size(280, 21);
            this.UComboBox.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(873, 95);
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
            this.button1.Location = new System.Drawing.Point(876, 50);
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
            this.checkPulseButton.Location = new System.Drawing.Point(1162, 109);
            this.checkPulseButton.Name = "checkPulseButton";
            this.checkPulseButton.Size = new System.Drawing.Size(152, 23);
            this.checkPulseButton.TabIndex = 8;
            this.checkPulseButton.Text = "View Cell vs U";
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
            this.SecondaryGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SecondaryGridView.Location = new System.Drawing.Point(876, 138);
            this.SecondaryGridView.Name = "SecondaryGridView";
            this.SecondaryGridView.Size = new System.Drawing.Size(654, 444);
            this.SecondaryGridView.TabIndex = 2;
            this.SecondaryGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.SecondaryGridView_ColumnHeaderMouseClick);
            this.SecondaryGridView.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.SecondaryGridView_RowHeaderMouseClick);
            // 
            // MainGridView
            // 
            this.MainGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MainGridView.Location = new System.Drawing.Point(5, 138);
            this.MainGridView.Name = "MainGridView";
            this.MainGridView.Size = new System.Drawing.Size(865, 444);
            this.MainGridView.TabIndex = 1;
            this.MainGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MainGridView_ColumnHeaderMouseClick);
            this.MainGridView.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MainGridView_RowHeaderMouseClick);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1536, 588);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Recalc SD Maps (future)";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(12, 11);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(75, 23);
            this.LoadButton.TabIndex = 1;
            this.LoadButton.Text = "Load Data";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(6, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(122, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Load HPT File";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
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
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SecondaryGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainGridView)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.TabPage tabPage1;
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
        private System.Windows.Forms.Button button2;
    }
}

