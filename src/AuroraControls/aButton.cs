using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
 
namespace AuroraControls
{
    public class AButton : Button
    {
        #region Member Variables
        public Color focusColor = Color.White;
        public Color unfocusColor = Color.Gray; // Default value
        public Color focusTextColor = Color.Black;
        public Color unfocusTextColor = Color.DarkGray; // Default value
        #endregion
 
        public AButton()
        {
            this.MouseEnter += AButton_MouseEnterLeave;
            this.MouseLeave += AButton_MouseEnterLeave;
            this.Enter += AButton_MouseEnterLeave;
            this.Leave += AButton_MouseEnterLeave;
        }
 
        private void AButton_MouseEnterLeave(object sender, EventArgs e)
        {
            if (e is EventArgs)
            {
                if (this.Focused )
                {
                    this.BackColor = focusColor;
                    this.ForeColor = focusTextColor;
                }
                else
                {
                    this.BackColor = unfocusColor;
                    this.ForeColor = unfocusTextColor;
                }
            }
        }
 
        #region User Defined Properties
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set color when focused")]
        [DisplayName("Focused Color")]
        public Color FocusedColor
        {
            get { return focusColor; }
            set { focusColor = value; }
        }
 
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set color when lost focus or leave the control")]
        [DisplayName("Unfocused Color")]
        public Color UnfocusedColor
        {
            get { return unfocusColor; }
            set { unfocusColor = value; }
        }
 
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set text color when focused")]
        [DisplayName("Focused Text Color")]
        public Color FocusedTextColor
        {
            get { return focusTextColor; }
            set { focusTextColor = value; }
        }
 
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set text color when lost focus or leave the control")]
        [DisplayName("Unfocused Text Color")]
        public Color UnfocusedTextColor
        {
            get { return unfocusTextColor; }
            set { unfocusTextColor = value; }
        }
        #endregion
    }
}
