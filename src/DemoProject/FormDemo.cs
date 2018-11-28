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
            aCheckBox1.CheckColor = pctFocusFore.BackColor;
            aCheckBox1.CheckTextColor = pctFocusText.BackColor;

            aCheckBox1.UnCheckColor = pctUnFocusFore.BackColor;
            aCheckBox1.UnCheckTextColor = pctUnFocusText.BackColor;

            aTextBox1.FocusedColor = pctFocusFore.BackColor;
            aTextBox1.FocusedTextColor = pctFocusText.BackColor;

            aTextBox1.unFocusedColor = pctUnFocusFore.BackColor;
            aTextBox1.unFocusedTextColor = pctUnFocusText.BackColor;

            aTextBox2.FocusedColor = pctFocusFore.BackColor;
            aTextBox2.FocusedTextColor = pctFocusText.BackColor;

            aTextBox2.unFocusedColor = pctUnFocusFore.BackColor;
            aTextBox2.unFocusedTextColor = pctUnFocusText.BackColor;

            aButton1.FocusedColor = pctFocusFore.BackColor;
            aButton1.FocusedTextColor = pctFocusText.BackColor;

            aButton1.unFocusedColor = pctUnFocusFore.BackColor;
            aButton1.unFocusedTextColor = pctUnFocusText.BackColor;
        }
        private void btnFocusFore_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pctFocusFore.BackColor = colorDialog1.Color;
                setcolor();
            }

        }

        private void btnFocusText_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pctFocusText.BackColor = colorDialog1.Color; setcolor();
            }
        }

        private void btnUnFocusFore_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pctUnFocusFore.BackColor = colorDialog1.Color; setcolor();
            }
        }

        private void btnUnFocusText_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pctUnFocusText.BackColor = colorDialog1.Color; setcolor();
            }
        }
    }
}
