namespace DataAnalyser
{
    partial class PCMSimulator
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.calculatedFuelMassErrorTextBox = new System.Windows.Forms.TextBox();
            this.calculatedInjectorPulseErrorTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.calculateAirmassButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(0, 116);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(358, 13);
            this.label9.TabIndex = 38;
            this.label9.Text = "Error of Calculated Fuel Mass via SD vs Calculated Fuel Mass via Inj Pulse";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(0, 69);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(137, 13);
            this.label8.TabIndex = 37;
            this.label8.Text = "Error of Calculated Inj Pulse";
            // 
            // calculatedFuelMassErrorTextBox
            // 
            this.calculatedFuelMassErrorTextBox.Location = new System.Drawing.Point(3, 139);
            this.calculatedFuelMassErrorTextBox.Name = "calculatedFuelMassErrorTextBox";
            this.calculatedFuelMassErrorTextBox.Size = new System.Drawing.Size(100, 20);
            this.calculatedFuelMassErrorTextBox.TabIndex = 36;
            // 
            // calculatedInjectorPulseErrorTextBox
            // 
            this.calculatedInjectorPulseErrorTextBox.Location = new System.Drawing.Point(3, 93);
            this.calculatedInjectorPulseErrorTextBox.Name = "calculatedInjectorPulseErrorTextBox";
            this.calculatedInjectorPulseErrorTextBox.Size = new System.Drawing.Size(100, 20);
            this.calculatedInjectorPulseErrorTextBox.TabIndex = 35;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(173, 13);
            this.label3.TabIndex = 34;
            this.label3.Text = "Calculate Airmass, Fuelmass, Inj ms";
            // 
            // calculateAirmassButton
            // 
            this.calculateAirmassButton.Location = new System.Drawing.Point(3, 31);
            this.calculateAirmassButton.Name = "calculateAirmassButton";
            this.calculateAirmassButton.Size = new System.Drawing.Size(170, 23);
            this.calculateAirmassButton.TabIndex = 33;
            this.calculateAirmassButton.Text = "Calculate and Update CSV";
            this.calculateAirmassButton.UseVisualStyleBackColor = true;
            this.calculateAirmassButton.Click += new System.EventHandler(this.calculateAirmassButton_Click_1);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(3, 233);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(170, 23);
            this.cancelButton.TabIndex = 39;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(3, 204);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(357, 23);
            this.progressBar.TabIndex = 40;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 188);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 41;
            this.label1.Text = "Progress:";
            // 
            // PCMSimulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.calculatedFuelMassErrorTextBox);
            this.Controls.Add(this.calculatedInjectorPulseErrorTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.calculateAirmassButton);
            this.Name = "PCMSimulator";
            this.Size = new System.Drawing.Size(1033, 631);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox calculatedFuelMassErrorTextBox;
        private System.Windows.Forms.TextBox calculatedInjectorPulseErrorTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button calculateAirmassButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label label1;
    }
}
