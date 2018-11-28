using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AuroraControls
{
   public class aCheckBox : CheckBox
    {
        #region Member Variables
        Color checkColor = Color.White;
        Color uncheckColor = SystemColors.Control;
        Color checkTextColor = Color.Black;
        Color uncheckTextColor = Color.Black;
        #endregion

        #region Constructor
        public aCheckBox()
        {
            this.CheckStateChanged += ACheckBox_CheckStateChanged;
           

        }

        private void ACheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.Checked )
            {
                this.ForeColor = checkTextColor;
                this.BackColor = checkColor;
                
            }
            else
            {
                this.ForeColor = uncheckTextColor;
                this.BackColor = uncheckColor;
            }
        }

       
        #endregion


        #region User Defined Properties
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set color when checked")]
        [DisplayName("Checked Color")]
        public Color CheckColor
        {
            get
            {
                return this.checkColor;
            }
            set
            {
                this.checkColor = value;

            }
        }
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set color when unchecked")]
        [DisplayName("unChecked Color")]
        public Color unCheckColor
        {
            get
            {
                return this.uncheckColor;
            }
            set
            {
                this.uncheckColor = value;

            }
        }
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set text color when checked")]
        [DisplayName("Checked Text Color")]
        public Color CheckTextColor
        {
            get
            {
                return this.checkTextColor;
            }
            set
            {
                this.checkTextColor = value;

            }
        }
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set text color when unchecked")]
        [DisplayName("unChecked Text Color")]
        public Color UncheckTextColor
        {
            get
            {
                return this.uncheckTextColor;
            }
            set
            {
                this.uncheckTextColor = value;

            }
        }
        #endregion

    }
}
