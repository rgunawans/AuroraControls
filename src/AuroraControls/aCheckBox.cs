using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AuroraControls
{
    /// <summary>
    /// Specifies the visual style of the <see cref="ACheckBox"/>.
    /// </summary>
    public enum CheckBoxStyle
    {
        /// <summary>Standard Windows checkbox.</summary>
        Standard,
        /// <summary>Modern toggle switch style.</summary>
        Switch
    }

    /// <summary>
    /// Custom CheckBox with enhanced checked/unchecked colors and optional modern Toggle Switch style.
    /// </summary>
    [ToolboxBitmap(typeof(CheckBox))]
    public class ACheckBox : CheckBox
    {
        private Color _checkColor = Color.White;
        private Color _unCheckColor = Color.White;
        private Color _checkTextColor = Color.Black;
        private Color _unCheckTextColor = Color.Black;
        private CheckBoxStyle _style = CheckBoxStyle.Standard;
        private Color _switchOnColor = Color.FromArgb(0, 120, 215);
        private Color _switchOffColor = Color.FromArgb(200, 200, 200);
        private Color _switchThumbColor = Color.White;

        /// <summary>
        /// Initializes a new instance of the <see cref="ACheckBox"/> class.
        /// </summary>
        public ACheckBox()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateColors();
        }

        /// <summary>
        /// Gets or sets the visual style of the control (Standard CheckBox or Toggle Switch).
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Visual style of the control.")]
        [DisplayName("Style")]
        [DefaultValue(CheckBoxStyle.Standard)]
        public CheckBoxStyle Style
        {
            get => _style;
            set
            {
                _style = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the track color of the switch when checked.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Color of the toggle track when active/checked.")]
        [DisplayName("Switch On Color")]
        public Color SwitchOnColor
        {
            get => _switchOnColor;
            set
            {
                _switchOnColor = value;
                if (this.Checked && _style == CheckBoxStyle.Switch) this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the track color of the switch when unchecked.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Color of the toggle track when inactive/unchecked.")]
        [DisplayName("Switch Off Color")]
        public Color SwitchOffColor
        {
            get => _switchOffColor;
            set
            {
                _switchOffColor = value;
                if (!this.Checked && _style == CheckBoxStyle.Switch) this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the color of the toggle thumb/knob.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Color of the switch circular thumb.")]
        [DisplayName("Switch Thumb Color")]
        public Color SwitchThumbColor
        {
            get => _switchThumbColor;
            set
            {
                _switchThumbColor = value;
                if (_style == CheckBoxStyle.Switch) this.Invalidate();
            }
        }

        /// <inheritdoc />
        protected override void OnPaint(PaintEventArgs pevent)
        {
            if (_style == CheckBoxStyle.Standard)
            {
                base.OnPaint(pevent);
                return;
            }

            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(this.BackColor);

            int switchWidth = 38;
            int switchHeight = 20;
            int switchY = Math.Max(0, (this.Height - switchHeight) / 2);
            int switchX = 2;

            // Draw track
            Color trackColor = this.Checked ? _switchOnColor : _switchOffColor;
            using (GraphicsPath trackPath = GetRoundedRectangle(new Rectangle(switchX, switchY, switchWidth, switchHeight), switchHeight / 2))
            using (SolidBrush trackBrush = new SolidBrush(trackColor))
            {
                g.FillPath(trackBrush, trackPath);
            }

            // Draw thumb knob
            int thumbDiameter = switchHeight - 4;
            int thumbX = this.Checked ? (switchX + switchWidth - thumbDiameter - 2) : (switchX + 2);
            int thumbY = switchY + 2;

            using (SolidBrush thumbBrush = new SolidBrush(_switchThumbColor))
            {
                g.FillEllipse(thumbBrush, thumbX, thumbY, thumbDiameter, thumbDiameter);
            }

            // Draw text
            if (!string.IsNullOrEmpty(this.Text))
            {
                int textX = switchX + switchWidth + 8;
                Rectangle textRect = new Rectangle(textX, 0, Math.Max(0, this.Width - textX), this.Height);
                TextRenderer.DrawText(g, this.Text, this.Font, textRect, this.ForeColor,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }
        }

        private static GraphicsPath GetRoundedRectangle(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        /// <inheritdoc />
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
