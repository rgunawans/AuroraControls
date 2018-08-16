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
       

        #region Constructor
        public aLabelTimer()
        {
            waktu.Interval = 1000;
            waktu.Enabled = true;

            waktu.Tick += Waktu_Tick;
        }

        private void Waktu_Tick(object sender, EventArgs e)
        {
            this.Text = System.DateTime.Now.ToString();
        }
        #endregion

        //#region User Defined Properties
        //[Browsable(true)]
        //[Category("Extended Properties")]
        //[Description("Format Text")]
        //[DisplayName("Format Text")]
        //public IFormatProvider FormatText
        //{
        //    get
        //    {
        //        return FormatText;
        //    }
        //    set
        //    {
        //        this.Text.ToString( FormatText);

        //    }
        //}
      
        //#endregion
    }
}
