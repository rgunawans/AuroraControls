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
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pctFocusText = new System.Windows.Forms.PictureBox();
            this.pctFocusFore = new System.Windows.Forms.PictureBox();
            this.btnFocusText = new AuroraControls.AButton();
            this.btnFocusFore = new AuroraControls.AButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pctUnFocusText = new System.Windows.Forms.PictureBox();
            this.btnUnFocusText = new AuroraControls.AButton();
            this.pctUnFocusFore = new System.Windows.Forms.PictureBox();
            this.btnUnFocusFore = new AuroraControls.AButton();
            this.chSelectAll = new System.Windows.Forms.CheckBox();
            this.chMandatory = new System.Windows.Forms.CheckBox();
            this.aTextBox2 = new AuroraControls.ATextBox();
            this.aTextBox1 = new AuroraControls.ATextBox();
            this.aLabelTimer1 = new AuroraControls.ALabelTimer();
            this.aCheckBox1 = new AuroraControls.ACheckBox();
            this.aButton1 = new AuroraControls.AButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctFocusText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctFocusFore)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctUnFocusText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctUnFocusFore)).BeginInit();
            this.SuspendLayout();
            // 
            // colorDialog1
            // 
            this.colorDialog1.AnyColor = true;
            this.colorDialog1.FullOpen = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pctFocusText);
            this.groupBox1.Controls.Add(this.pctFocusFore);
            this.groupBox1.Controls.Add(this.btnFocusText);
            this.groupBox1.Controls.Add(this.btnFocusFore);
            this.groupBox1.Location = new System.Drawing.Point(374, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(182, 139);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Focus / Checked";
            // 
            // pctFocusText
            // 
            this.pctFocusText.BackColor = System.Drawing.Color.Black;
            this.pctFocusText.Location = new System.Drawing.Point(106, 81);
            this.pctFocusText.Name = "pctFocusText";
            this.pctFocusText.Size = new System.Drawing.Size(43, 36);
            this.pctFocusText.TabIndex = 4;
            this.pctFocusText.TabStop = false;
            // 
            // pctFocusFore
            // 
            this.pctFocusFore.BackColor = System.Drawing.Color.White;
            this.pctFocusFore.Location = new System.Drawing.Point(106, 30);
            this.pctFocusFore.Name = "pctFocusFore";
            this.pctFocusFore.Size = new System.Drawing.Size(43, 36);
            this.pctFocusFore.TabIndex = 3;
            this.pctFocusFore.TabStop = false;
            // 
            // btnFocusText
            // 
            this.btnFocusText.FocusedColor = System.Drawing.SystemColors.Control;
            this.btnFocusText.FocusedTextColor = System.Drawing.Color.Black;
            this.btnFocusText.Location = new System.Drawing.Point(15, 81);
            this.btnFocusText.Name = "btnFocusText";
            this.btnFocusText.Size = new System.Drawing.Size(84, 36);
            this.btnFocusText.TabIndex = 2;
            this.btnFocusText.Text = "Text Color";
            this.btnFocusText.UnfocusedColor = System.Drawing.SystemColors.Control;
            this.btnFocusText.UnfocusedTextColor = System.Drawing.Color.Black;
            this.btnFocusText.UseVisualStyleBackColor = true;
            this.btnFocusText.Click += new System.EventHandler(this.btnFocusText_Click);
            // 
            // btnFocusFore
            // 
            this.btnFocusFore.FocusedColor = System.Drawing.SystemColors.Control;
            this.btnFocusFore.FocusedTextColor = System.Drawing.Color.Black;
            this.btnFocusFore.Location = new System.Drawing.Point(15, 30);
            this.btnFocusFore.Name = "btnFocusFore";
            this.btnFocusFore.Size = new System.Drawing.Size(84, 36);
            this.btnFocusFore.TabIndex = 1;
            this.btnFocusFore.Text = "Fore Color";
            this.btnFocusFore.UnfocusedColor = System.Drawing.SystemColors.Control;
            this.btnFocusFore.UnfocusedTextColor = System.Drawing.Color.Black;
            this.btnFocusFore.UseVisualStyleBackColor = true;
            this.btnFocusFore.Click += new System.EventHandler(this.btnFocusFore_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pctUnFocusText);
            this.groupBox2.Controls.Add(this.btnUnFocusText);
            this.groupBox2.Controls.Add(this.pctUnFocusFore);
            this.groupBox2.Controls.Add(this.btnUnFocusFore);
            this.groupBox2.Location = new System.Drawing.Point(582, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(178, 139);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "UnFocus / UnChecked";
            // 
            // pctUnFocusText
            // 
            this.pctUnFocusText.BackColor = System.Drawing.Color.Black;
            this.pctUnFocusText.Location = new System.Drawing.Point(115, 81);
            this.pctUnFocusText.Name = "pctUnFocusText";
            this.pctUnFocusText.Size = new System.Drawing.Size(43, 36);
            this.pctUnFocusText.TabIndex = 6;
            this.pctUnFocusText.TabStop = false;
            // 
            // btnUnFocusText
            // 
            this.btnUnFocusText.FocusedColor = System.Drawing.SystemColors.Control;
            this.btnUnFocusText.FocusedTextColor = System.Drawing.Color.Black;
            this.btnUnFocusText.Location = new System.Drawing.Point(25, 81);
            this.btnUnFocusText.Name = "btnUnFocusText";
            this.btnUnFocusText.Size = new System.Drawing.Size(84, 36);
            this.btnUnFocusText.TabIndex = 8;
            this.btnUnFocusText.Text = "Text Color";
            this.btnUnFocusText.UnfocusedColor = System.Drawing.SystemColors.Control;
            this.btnUnFocusText.UnfocusedTextColor = System.Drawing.Color.Black;
            this.btnUnFocusText.UseVisualStyleBackColor = true;
            this.btnUnFocusText.Click += new System.EventHandler(this.btnUnFocusText_Click);
            // 
            // pctUnFocusFore
            // 
            this.pctUnFocusFore.BackColor = System.Drawing.Color.White;
            this.pctUnFocusFore.Location = new System.Drawing.Point(115, 30);
            this.pctUnFocusFore.Name = "pctUnFocusFore";
            this.pctUnFocusFore.Size = new System.Drawing.Size(43, 36);
            this.pctUnFocusFore.TabIndex = 5;
            this.pctUnFocusFore.TabStop = false;
            // 
            // btnUnFocusFore
            // 
            this.btnUnFocusFore.FocusedColor = System.Drawing.SystemColors.Control;
            this.btnUnFocusFore.FocusedTextColor = System.Drawing.Color.Black;
            this.btnUnFocusFore.Location = new System.Drawing.Point(25, 30);
            this.btnUnFocusFore.Name = "btnUnFocusFore";
            this.btnUnFocusFore.Size = new System.Drawing.Size(84, 36);
            this.btnUnFocusFore.TabIndex = 7;
            this.btnUnFocusFore.Text = "Fore Color";
            this.btnUnFocusFore.UnfocusedColor = System.Drawing.SystemColors.Control;
            this.btnUnFocusFore.UnfocusedTextColor = System.Drawing.Color.Black;
            this.btnUnFocusFore.UseVisualStyleBackColor = true;
            this.btnUnFocusFore.Click += new System.EventHandler(this.btnUnFocusFore_Click);
            // 
            // chSelectAll
            // 
            this.chSelectAll.AutoSize = true;
            this.chSelectAll.Location = new System.Drawing.Point(374, 158);
            this.chSelectAll.Name = "chSelectAll";
            this.chSelectAll.Size = new System.Drawing.Size(145, 20);
            this.chSelectAll.TabIndex = 7;
            this.chSelectAll.Text = "Select All On Focus";
            this.chSelectAll.UseVisualStyleBackColor = true;
            this.chSelectAll.CheckedChanged += new System.EventHandler(this.chSelectAll_CheckedChanged);
            // 
            // chMandatory
            // 
            this.chMandatory.AutoSize = true;
            this.chMandatory.Location = new System.Drawing.Point(587, 158);
            this.chMandatory.Name = "chMandatory";
            this.chMandatory.Size = new System.Drawing.Size(93, 20);
            this.chMandatory.TabIndex = 8;
            this.chMandatory.Text = "Mandatory";
            this.chMandatory.UseVisualStyleBackColor = true;
            this.chMandatory.CheckedChanged += new System.EventHandler(this.ChMandatory_CheckedChanged);
            // 
            // aTextBox2
            // 
            this.aTextBox2.BackColor = System.Drawing.Color.White;
            this.aTextBox2.ForeColor = System.Drawing.Color.Black;
            this.aTextBox2.Location = new System.Drawing.Point(12, 129);
            this.aTextBox2.Mandatory = false;
            this.aTextBox2.Name = "aTextBox2";
            this.aTextBox2.Size = new System.Drawing.Size(314, 22);
            this.aTextBox2.TabIndex = 3;
            // 
            // aTextBox1
            // 
            this.aTextBox1.BackColor = System.Drawing.Color.White;
            this.aTextBox1.ForeColor = System.Drawing.Color.Black;
            this.aTextBox1.Location = new System.Drawing.Point(12, 72);
            this.aTextBox1.Mandatory = false;
            this.aTextBox1.Name = "aTextBox1";
            this.aTextBox1.Size = new System.Drawing.Size(314, 22);
            this.aTextBox1.TabIndex = 2;
            // 
            // aLabelTimer1
            // 
            this.aLabelTimer1.AutoSize = true;
            this.aLabelTimer1.Culture = new System.Globalization.CultureInfo("en-US");
            this.aLabelTimer1.FormatText = "dd-MMM-yyyy HH:mm:ss";
            this.aLabelTimer1.Location = new System.Drawing.Point(13, 52);
            this.aLabelTimer1.Name = "aLabelTimer1";
            this.aLabelTimer1.Size = new System.Drawing.Size(129, 16);
            this.aLabelTimer1.TabIndex = 1;
            this.aLabelTimer1.Text = "03-Apr-2025 21:35:03";
            // 
            // aCheckBox1
            // 
            this.aCheckBox1.AutoSize = true;
            this.aCheckBox1.BackColor = System.Drawing.SystemColors.Control;
            this.aCheckBox1.CheckColor = System.Drawing.Color.LimeGreen;
            this.aCheckBox1.CheckTextColor = System.Drawing.Color.DarkRed;
            this.aCheckBox1.ForeColor = System.Drawing.Color.Black;
            this.aCheckBox1.Location = new System.Drawing.Point(12, 28);
            this.aCheckBox1.Name = "aCheckBox1";
            this.aCheckBox1.Size = new System.Drawing.Size(105, 20);
            this.aCheckBox1.TabIndex = 0;
            this.aCheckBox1.Text = "aCheckBox1";
            this.aCheckBox1.UnCheckColor = System.Drawing.SystemColors.Control;
            this.aCheckBox1.UnCheckTextColor = System.Drawing.Color.Black;
            this.aCheckBox1.UseVisualStyleBackColor = false;
            // 
            // aButton1
            // 
            this.aButton1.FocusedColor = System.Drawing.SystemColors.Control;
            this.aButton1.FocusedTextColor = System.Drawing.Color.Black;
            this.aButton1.Location = new System.Drawing.Point(12, 183);
            this.aButton1.Name = "aButton1";
            this.aButton1.Size = new System.Drawing.Size(84, 36);
            this.aButton1.TabIndex = 4;
            this.aButton1.Text = "aButton1";
            this.aButton1.UnfocusedColor = System.Drawing.SystemColors.Control;
            this.aButton1.UnfocusedTextColor = System.Drawing.Color.Black;
            this.aButton1.UseVisualStyleBackColor = true;
            // 
            // FormDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.chMandatory);
            this.Controls.Add(this.chSelectAll);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.aTextBox2);
            this.Controls.Add(this.aTextBox1);
            this.Controls.Add(this.aLabelTimer1);
            this.Controls.Add(this.aCheckBox1);
            this.Controls.Add(this.aButton1);
            this.Name = "FormDemo";
            this.Text = "Demo App";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pctFocusText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctFocusFore)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pctUnFocusText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctUnFocusFore)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AuroraControls.AButton aButton1;
        private AuroraControls.ACheckBox aCheckBox1;
        private AuroraControls.ALabelTimer aLabelTimer1;
        private AuroraControls.ATextBox aTextBox1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private AuroraControls.ATextBox aTextBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pctFocusText;
        private System.Windows.Forms.PictureBox pctFocusFore;
        private AuroraControls.AButton btnFocusText;
        private AuroraControls.AButton btnFocusFore;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pctUnFocusText;
        private AuroraControls.AButton btnUnFocusText;
        private System.Windows.Forms.PictureBox pctUnFocusFore;
        private AuroraControls.AButton btnUnFocusFore;
        private System.Windows.Forms.CheckBox chSelectAll;
        private System.Windows.Forms.CheckBox chMandatory;
    }
}

