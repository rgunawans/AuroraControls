using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using System.Drawing;

namespace AuroraControls
{
    /// <summary>
    /// Custom Label that automatically updates with the current date and time.
    /// </summary>
    [ToolboxBitmap(typeof(Label))]
    public class ALabelTimer : Label
    {
        private Timer _timer = new Timer();
        private string _formatText = "dd-MMM-yyyy HH:mm:ss";
        private CultureInfo _culture = new CultureInfo("en-US", false);

        /// <summary>
        /// Initializes a new instance of the <see cref="ALabelTimer"/> class.
        /// </summary>
        public ALabelTimer()
        {
            _timer.Interval = 1000;
            _timer.Tick += Timer_Tick;
        }

        /// <inheritdoc />
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (!this.DesignMode && LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                UpdateLabel();
                _timer.Start();
            }
        }

        /// <inheritdoc />
        protected override void OnHandleDestroyed(EventArgs e)
        {
            _timer.Stop();
            base.OnHandleDestroyed(e);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            this.Text = DateTime.Now.ToString(_formatText, _culture);
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_timer != null)
                {
                    _timer.Tick -= Timer_Tick;
                    _timer.Dispose();
                    _timer = null;
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets or sets the date and time format string.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Format Text")]
        [DisplayName("Format Text")]
        [DefaultValue("dd-MMM-yyyy HH:mm:ss")]
        public string FormatText
        {
            get => _formatText;
            set
            {
                _formatText = value;
                UpdateLabel();
            }
        }

        /// <summary>
        /// Gets or sets the culture info used for formatting the date and time.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Culture Info")]
        [DisplayName("Culture")]
        public CultureInfo Culture
        {
            get => _culture;
            set
            {
                _culture = value;
                UpdateLabel();
            }
        }
    }
}
