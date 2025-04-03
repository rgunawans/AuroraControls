using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoProject
{
    public partial class FormDemo : Form
    {
       
        public FormDemo()
        {
            InitializeComponent();
        }

      private void setcolor()
        {
           
            aTextBox1.Mandatory = chMandatory.Checked;
            aTextBox2.Mandatory = chMandatory.Checked;

        }
        private void btnFocusFore_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pctFocusFore.BackColor = colorDialog1.Color;
                aTextBox1.focusColor = colorDialog1.Color;
                aTextBox2.focusColor = colorDialog1.Color;
                aButton1.FocusedColor = colorDialog1.Color;
                aCheckBox1.CheckColor = colorDialog1.Color;
                setcolor();
            }

        }

        private void btnFocusText_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pctFocusText.BackColor = colorDialog1.Color;
                aTextBox1.focusTextColor = colorDialog1.Color;
                aTextBox2.focusTextColor = colorDialog1.Color;
                aButton1.FocusedTextColor = colorDialog1.Color;
                aCheckBox1.CheckTextColor = colorDialog1.Color;
                setcolor();
            }
        }

        private void btnUnFocusFore_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pctUnFocusFore.BackColor = colorDialog1.Color;
                aTextBox1.unfocusColor = colorDialog1.Color;
                aTextBox2.unfocusColor = colorDialog1.Color;
                aButton1.UnfocusedColor = colorDialog1.Color;
                aCheckBox1.UnCheckColor = colorDialog1.Color;
                setcolor();
            }
        }

        private void btnUnFocusText_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pctUnFocusText.BackColor = colorDialog1.Color;
                aTextBox1.unfocusTextColor = colorDialog1.Color;
                aTextBox2.unfocusTextColor = colorDialog1.Color;
                aButton1.UnfocusedTextColor = colorDialog1.Color;
                aCheckBox1.UnCheckTextColor = colorDialog1.Color;
                setcolor();
            }
        }
        
        private void chSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            setcolor();
        }

        private void ChMandatory_CheckedChanged(object sender, EventArgs e)
        {
            setcolor();
        }

        private void ATextBox1_Leave(object sender, EventArgs e)
        {

        }
    }
}
