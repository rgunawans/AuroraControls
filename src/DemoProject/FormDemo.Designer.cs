namespace DemoProject
{
    partial class FormDemo
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
            this.aButton1 = new AuroraControls.aButton();
            this.aCheckBox1 = new AuroraControls.aCheckBox();
            this.aLabelTimer1 = new AuroraControls.aLabelTimer();
            this.aTextBox1 = new AuroraControls.aTextBox();
            this.SuspendLayout();
            // 
            // aButton1
            // 
            this.aButton1.FocusedColor = System.Drawing.Color.White;
            this.aButton1.FocusedTextColor = System.Drawing.Color.Black;
            this.aButton1.Location = new System.Drawing.Point(12, 164);
            this.aButton1.Name = "aButton1";
            this.aButton1.Size = new System.Drawing.Size(75, 23);
            this.aButton1.TabIndex = 0;
            this.aButton1.Text = "aButton1";
            this.aButton1.unFocusedColor = System.Drawing.SystemColors.Control;
            this.aButton1.unFocusedTextColor = System.Drawing.Color.Black;
            this.aButton1.UseVisualStyleBackColor = true;
            // 
            // aCheckBox1
            // 
            this.aCheckBox1.AutoSize = true;
            this.aCheckBox1.BackColor = System.Drawing.SystemColors.Control;
            this.aCheckBox1.CheckColor = System.Drawing.Color.LimeGreen;
            this.aCheckBox1.CheckTextColor = System.Drawing.Color.DarkRed;
            this.aCheckBox1.ForeColor = System.Drawing.Color.Black;
            this.aCheckBox1.Location = new System.Drawing.Point(13, 73);
            this.aCheckBox1.Name = "aCheckBox1";
            this.aCheckBox1.Size = new System.Drawing.Size(108, 21);
            this.aCheckBox1.TabIndex = 1;
            this.aCheckBox1.Text = "aCheckBox1";
            this.aCheckBox1.unCheckColor = System.Drawing.SystemColors.Control;
            this.aCheckBox1.UncheckTextColor = System.Drawing.Color.Black;
            this.aCheckBox1.UseVisualStyleBackColor = false;
            // 
            // aLabelTimer1
            // 
            this.aLabelTimer1.AutoSize = true;
            this.aLabelTimer1.CultureInfo = new System.Globalization.CultureInfo("en-US");
            this.aLabelTimer1.FormatText = "dd-MMM-yyyy HH:mm:ss";
            this.aLabelTimer1.Location = new System.Drawing.Point(13, 101);
            this.aLabelTimer1.Name = "aLabelTimer1";
            this.aLabelTimer1.Size = new System.Drawing.Size(151, 17);
            this.aLabelTimer1.TabIndex = 2;
            this.aLabelTimer1.Text = "28-Nov-2018 17:10:42";
            // 
            // aTextBox1
            // 
            this.aTextBox1.BackColor = System.Drawing.Color.White;
            this.aTextBox1.FocusedColor = System.Drawing.Color.White;
            this.aTextBox1.FocusedTextColor = System.Drawing.Color.Black;
            this.aTextBox1.Location = new System.Drawing.Point(16, 136);
            this.aTextBox1.Name = "aTextBox1";
            this.aTextBox1.Size = new System.Drawing.Size(314, 22);
            this.aTextBox1.TabIndex = 3;
            this.aTextBox1.unFocusedColor = System.Drawing.SystemColors.Control;
            this.aTextBox1.unFocusedTextColor = System.Drawing.Color.Black;
            // 
            // FormDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.aTextBox1);
            this.Controls.Add(this.aLabelTimer1);
            this.Controls.Add(this.aCheckBox1);
            this.Controls.Add(this.aButton1);
            this.Name = "FormDemo";
            this.Text = "Demo App";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AuroraControls.aButton aButton1;
        private AuroraControls.aCheckBox aCheckBox1;
        private AuroraControls.aLabelTimer aLabelTimer1;
        private AuroraControls.aTextBox aTextBox1;
    }
}

