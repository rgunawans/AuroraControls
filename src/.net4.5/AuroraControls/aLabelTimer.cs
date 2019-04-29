using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace AuroraControls
{
    public class ALabelTimer:Label
    {
        Timer waktu = new Timer();

        #region Member Variables
        String formatText = "dd-MMM-yyyy HH:mm:ss";
        CultureInfo ci = new CultureInfo("EN-US",false);
        #endregion

        #region Constructor
        public ALabelTimer()
        {
            waktu.Interval = 1000;
            waktu.Enabled = true;

            waktu.Tick += Waktu_Tick;
        }

        private void Waktu_Tick(object sender, EventArgs e)
        {
            this.Text = System.DateTime.Now.ToString(formatText,ci);
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


        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Culture Info")]
        [DisplayName("Culture Info")]

        public CultureInfo CultureInfo
        {
            get
            {
                return this.ci;
            }
            set
            {
                this.ci = value;

            }
        }

        #endregion
    }
}
