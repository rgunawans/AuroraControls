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
        Boolean selectAllOnFocus = false;
        Boolean isMandatory = false;
        #endregion

        #region Constructor
        public ATextBox()
        {

            this.BackColor = focusColor;

            this.Leave += Textbox_Leave;
            this.Enter += Textbox_Enter;

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
            if (this.Mandatory && this.Text.Length == 0)
            {
                this.Focus();
                Label warntext = new Label
                {
                    ForeColor = Color.Red,
                    Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    Top = this.Top + (this.Height + 2),
                    Left = this.Left,
                    Text = "This textbox cannot blank !",
                    Visible = true,
                    Name = "warningText",
                    AutoSize = true
                };
                this.Parent.Controls.Add(warntext);
                //  warntext.Show();
            }
            else
            {
                Label warn = this.Parent.Controls.Find("warningText", true).FirstOrDefault() as Label;
                if (warn != null)
                {

                    warn.Dispose();
                }
            }

        }

        #region User Defined Properties

        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("This textbox cannot blank")]
        [DisplayName("Is Mandatory")]
        public Boolean Mandatory
        {
            get
            {
                return this.isMandatory;
            }
            set
            {
                this.isMandatory = value;

            }
        }

        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Select all on focus")]
        [DisplayName("Select All On Focus")]
        public Boolean SelectAllOnFocus
        {
            get
            {
                return this.selectAllOnFocus;
            }
            set
            {
                this.selectAllOnFocus = value;

            }
        }

        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set color when focused")]
        [DisplayName("Focused Color")]
        public Color FocusedColor
        {
            get
            {
                return this.focusColor;
            }
            set
            {
                this.focusColor = value;

            }
        }
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set color when lost focus or leave the control")]
        [DisplayName("unFocused Color")]
        public Color unFocusedColor
        {
            get
            {
                return this.unfocusColor;
            }
            set
            {
                this.unfocusColor = value;

            }
        }
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set text color when focused")]
        [DisplayName("Focused Text Color")]
        public Color FocusedTextColor
        {
            get
            {
                return this.focusTextColor;
            }
            set
            {
                this.focusTextColor = value;

            }
        }
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set text color when lost focus or leave the control")]
        [DisplayName("unFocused Text Color")]
        public Color unFocusedTextColor
        {
            get
            {
                return this.unfocusTextColor;
            }
            set
            {
                this.unfocusTextColor = value;

            }
        }
        #endregion
    }
}
