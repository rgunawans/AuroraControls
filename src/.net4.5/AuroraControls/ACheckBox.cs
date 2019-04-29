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
   public class ACheckBox : CheckBox
    {
        #region Member Variables
        Color checkColor = Color.White;
        Color unCheckColor = Color.White;
        Color checkTextColor = Color.Black;
        Color unCheckTextColor = Color.Black;
        #endregion

        #region Constructor
        public ACheckBox()
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
                this.ForeColor = unCheckTextColor;
                this.BackColor = unCheckColor;
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
        public Color UnCheckColor
        {
            get
            {
                return this.unCheckColor;
            }
            set
            {
                this.unCheckColor = value;

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
        public Color UnCheckTextColor
        {
            get
            {
                return this.unCheckTextColor;
            }
            set
            {
                this.unCheckTextColor = value;

            }
        }
        #endregion

    }
}
