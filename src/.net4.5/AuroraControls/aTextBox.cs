﻿using System;
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
        Color unfocusColor = SystemColors.Control;
        Color focusTextColor = Color.Black;
        Color unfocusTextColor = Color.Black;
        Boolean selectAllOnFocus = false;

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


        #region User Defined Properties
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
