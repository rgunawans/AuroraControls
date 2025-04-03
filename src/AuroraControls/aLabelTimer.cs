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
    public class ALabelTimer : Label
    {
        private Timer timer = new Timer();
 
        #region Member Variables
        private string formatText = "dd-MMM-yyyy HH:mm:ss";
        private CultureInfo culture = new CultureInfo("en-US", false);
        #endregion
 
        #region Constructor
        public ALabelTimer()
        {
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        #endregion
 
        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateLabel();
        }
 
        private void UpdateLabel()
        {
            this.Text = DateTime.Now.ToString(formatText, culture);
        }
 
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                timer.Tick -= Timer_Tick; // Unsubscribe from the event
                timer.Dispose(); // Dispose of the timer
            }
            base.Dispose(disposing);
        }
 
        #region User Defined Properties
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Format Text")]
        [DisplayName("Format Text")]
        public string FormatText
        {
            get => formatText;
            set => formatText = value;
        }
 
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Culture Info")]
        [DisplayName("Culture")]
        public CultureInfo Culture
        {
            get => culture;
            set => culture = value;
        }
        #endregion
    }
}
