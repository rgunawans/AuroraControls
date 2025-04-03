using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
 
namespace AuroraControls
{
    public class ATextBox : TextBox
    {
        #region Member Variables
        Color focusColor = Color.White;
        Color unfocusColor = Color.White;
        Color focusTextColor = Color.Black;
        Color unfocusTextColor = Color.Black;
        bool selectAllOnFocus = false;
        bool isMandatory = false;
        Label warningLabel;
        #endregion
 
        #region Properties
        public bool Mandatory
        {
            get { return isMandatory; }
            set { isMandatory = value; }
        }
        #endregion
 
        #region Constructor
        public ATextBox()
        {
            this.BackColor = focusColor;
            this.Leave += Textbox_Leave;
            this.Enter += Textbox_Enter;
            this.GotFocus += Textbox_Enter; // Consolidated event handler
        }
        #endregion
 
        private void Textbox_Enter(object sender, EventArgs e)
        {
            this.BackColor = focusColor;
            this.ForeColor = focusTextColor;
            if (selectAllOnFocus) this.SelectAll();
        }
 
        private void Textbox_Leave(object sender, EventArgs e)
        {
            this.BackColor = unfocusColor;
            this.ForeColor = unfocusTextColor;
 
            if (Mandatory && string.IsNullOrEmpty(this.Text))
            {
                this.Focus();
                ShowWarning("This textbox cannot be blank!");
            }
            else
            {
                HideWarning();
            }
        }
 
        private void ShowWarning(string message)
        {
            if (warningLabel == null)
            {
                warningLabel = new Label
                {
                    ForeColor = Color.Red,
                    Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    Top = this.Top + (this.Height + 2),
                    Left = this.Left,
                    AutoSize = true
                };
                this.Parent.Controls.Add(warningLabel);
            }
            warningLabel.Text = message;
            warningLabel.Visible = true;
        }
 
        private void HideWarning()
        {
            if (warningLabel != null)
            {
                warningLabel.Visible = false;
            }
        }
    }
}
