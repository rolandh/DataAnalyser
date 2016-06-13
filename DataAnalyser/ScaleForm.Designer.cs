namespace DataAnalyser
{
    partial class ScaleForm
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
            this.Start = new System.Windows.Forms.Label();
            this.End = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.axisTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.startMaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.endMaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.stepMaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.SuspendLayout();
            // 
            // Start
            // 
            this.Start.AutoSize = true;
            this.Start.Location = new System.Drawing.Point(12, 14);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(29, 13);
            this.Start.TabIndex = 1;
            this.Start.Text = "Start";
            // 
            // End
            // 
            this.End.AutoSize = true;
            this.End.Location = new System.Drawing.Point(12, 62);
            this.End.Name = "End";
            this.End.Size = new System.Drawing.Size(26, 13);
            this.End.TabIndex = 3;
            this.End.Text = "End";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Step";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 165);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Generate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // axisTextBox
            // 
            this.axisTextBox.Location = new System.Drawing.Point(115, 33);
            this.axisTextBox.Name = "axisTextBox";
            this.axisTextBox.Size = new System.Drawing.Size(687, 20);
            this.axisTextBox.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(112, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Axis";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(115, 165);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Accept";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // startMaskedTextBox
            // 
            this.startMaskedTextBox.Location = new System.Drawing.Point(16, 30);
            this.startMaskedTextBox.Name = "startMaskedTextBox";
            this.startMaskedTextBox.Size = new System.Drawing.Size(75, 20);
            this.startMaskedTextBox.TabIndex = 10;
            this.startMaskedTextBox.Text = "0.0";
            // 
            // endMaskedTextBox
            // 
            this.endMaskedTextBox.Location = new System.Drawing.Point(16, 78);
            this.endMaskedTextBox.Name = "endMaskedTextBox";
            this.endMaskedTextBox.Size = new System.Drawing.Size(75, 20);
            this.endMaskedTextBox.TabIndex = 11;
            this.endMaskedTextBox.Text = "10";
            // 
            // stepMaskedTextBox
            // 
            this.stepMaskedTextBox.Location = new System.Drawing.Point(16, 127);
            this.stepMaskedTextBox.Name = "stepMaskedTextBox";
            this.stepMaskedTextBox.Size = new System.Drawing.Size(75, 20);
            this.stepMaskedTextBox.TabIndex = 12;
            this.stepMaskedTextBox.Text = "1.0";
            // 
            // ScaleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 308);
            this.Controls.Add(this.stepMaskedTextBox);
            this.Controls.Add(this.endMaskedTextBox);
            this.Controls.Add(this.startMaskedTextBox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.axisTextBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.End);
            this.Controls.Add(this.Start);
            this.Name = "ScaleForm";
            this.Text = "Edit Scale";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label Start;
        private System.Windows.Forms.Label End;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox axisTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.MaskedTextBox startMaskedTextBox;
        private System.Windows.Forms.MaskedTextBox endMaskedTextBox;
        private System.Windows.Forms.MaskedTextBox stepMaskedTextBox;
    }
}