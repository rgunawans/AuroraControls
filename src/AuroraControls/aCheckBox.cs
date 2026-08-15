using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AuroraControls
{
    /// <summary>
    /// Custom CheckBox with enhanced checked/unchecked colors.
    /// </summary>
    [ToolboxBitmap(typeof(CheckBox))]
    public class ACheckBox : CheckBox
    {
        private Color _checkColor = Color.White;
        private Color _unCheckColor = Color.White;
        private Color _checkTextColor = Color.Black;
        private Color _unCheckTextColor = Color.Black;

        public ACheckBox()
        {
            UpdateColors();
        }

        protected override void OnCheckStateChanged(EventArgs e)
        {
            base.OnCheckStateChanged(e);
            UpdateColors();
        }

        /// <summary>
        /// Gets or sets the background color when the checkbox is checked.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set color when checked")]
        [DisplayName("Checked Color")]
        [DefaultValue(typeof(Color), "White")]
        public Color CheckColor
        {
            get => _checkColor;
            set
            {
                _checkColor = value;
                if (this.Checked) UpdateColors();
            }
        }

        /// <summary>
        /// Gets or sets the background color when the checkbox is unchecked.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set color when unchecked")]
        [DisplayName("Unchecked Color")]
        [DefaultValue(typeof(Color), "White")]
        public Color UnCheckColor
        {
            get => _unCheckColor;
            set
            {
                _unCheckColor = value;
                if (!this.Checked) UpdateColors();
            }
        }

        /// <summary>
        /// Gets or sets the text color when the checkbox is checked.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set text color when checked")]
        [DisplayName("Checked Text Color")]
        [DefaultValue(typeof(Color), "Black")]
        public Color CheckTextColor
        {
            get => _checkTextColor;
            set
            {
                _checkTextColor = value;
                if (this.Checked) UpdateColors();
            }
        }

        /// <summary>
        /// Gets or sets the text color when the checkbox is unchecked.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set text color when unchecked")]
        [DisplayName("Unchecked Text Color")]
        [DefaultValue(typeof(Color), "Black")]
        public Color UnCheckTextColor
        {
            get => _unCheckTextColor;
            set
            {
                _unCheckTextColor = value;
                if (!this.Checked) UpdateColors();
            }
        }

        private void UpdateColors()
        {
            this.ForeColor = this.Checked ? _checkTextColor : _unCheckTextColor;
            this.BackColor = this.Checked ? _checkColor : _unCheckColor;
        }
    }
}
