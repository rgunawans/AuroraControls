using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
 
namespace AuroraControls
{
    public class ACheckBox : CheckBox
    {
        #region Member Variables
        public Color checkColor = Color.White;
        public Color unCheckColor = Color.White;
        public Color checkTextColor = Color.Black;
        public Color unCheckTextColor = Color.Black;
        #endregion
 
        #region Constructor
        public ACheckBox()
        {
            this.CheckStateChanged += ACheckBox_CheckStateChanged;
            // Initialize the colors based on the initial checked state
            ACheckBox_CheckStateChanged(this, EventArgs.Empty);
        }
        #endregion
 
        private void ACheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            this.ForeColor = this.Checked ? checkTextColor : unCheckTextColor;
            this.BackColor = this.Checked ? checkColor : unCheckColor;
        }
 
        #region User Defined Properties
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set color when checked")]
        [DisplayName("Checked Color")]
        public Color CheckColor
        {
            get => checkColor;
            set
            {
                checkColor = value;
                Invalidate(); // Redraw the control
            }
        }
 
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set color when unchecked")]
        [DisplayName("Unchecked Color")]
        public Color UnCheckColor
        {
            get => unCheckColor;
            set
            {
                unCheckColor = value;
                Invalidate(); // Redraw the control
            }
        }
 
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set text color when checked")]
        [DisplayName("Checked Text Color")]
        public Color CheckTextColor
        {
            get => checkTextColor;
            set
            {
                checkTextColor = value;
                Invalidate(); // Redraw the control
            }
        }
 
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set text color when unchecked")]
        [DisplayName("Unchecked Text Color")]
        public Color UnCheckTextColor
        {
            get => unCheckTextColor;
            set
            {
                unCheckTextColor = value;
                Invalidate(); // Redraw the control
            }
        }
        #endregion
    }
}
