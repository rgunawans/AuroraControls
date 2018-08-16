using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace AuroraControls
{
    public class aLabelTimer:Label
    {
        Timer waktu = new Timer();

        #region Member Variables
        String formatText = "dd-mm-yyyy HH:mm:ss";
      
        #endregion

        #region Constructor
        public aLabelTimer()
        {
            waktu.Interval = 1000;
            waktu.Enabled = true;

            waktu.Tick += Waktu_Tick;
        }

        private void Waktu_Tick(object sender, EventArgs e)
        {
            this.Text = System.DateTime.Now.ToString(formatText);
        }
        #endregion

        #region User Defined Properties
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Format Text")]
        [DisplayName("Format Text")]
        public String FormatText
        {
            get
            {
                return this.formatText;
            }
            set
            {
                this.formatText = value;

            }
        }

        #endregion
    }
}
