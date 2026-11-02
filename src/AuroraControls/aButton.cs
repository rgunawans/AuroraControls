using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AuroraControls
{
    /// <summary>
    /// Custom Button with focus and hover color enhancements.
    /// </summary>
    [ToolboxBitmap(typeof(Button))]
    public class AButton : Button
    {
        private Color _focusedColor = Color.White;
        private Color _unfocusedColor = Color.Gray;
        private Color _focusedTextColor = Color.Black;
        private Color _unfocusedTextColor = Color.DarkGray;
        private bool _isHoveredOrFocused = false;

        private bool _isLoading = false;
        private string _loadingText = "Loading...";
        private string _originalText = string.Empty;
        private Timer _spinnerTimer;
        private int _spinnerAngle = 0;

        private string _badgeText = string.Empty;
        private Color _badgeBackColor = Color.Red;
        private Color _badgeTextColor = Color.White;

        private int _progressValue = 0;
        private Color _progressColor = Color.FromArgb(80, 0, 120, 215);

        /// <summary>
        /// Initializes a new instance of the <see cref="AButton"/> class.
        /// </summary>
        public AButton()
        {
            UpdateColors();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the button is in an asynchronous loading state.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Enables animated loading state and suppresses user click.")]
        [DisplayName("Is Loading")]
        [DefaultValue(false)]
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading == value) return;
                _isLoading = value;
                if (_isLoading)
                {
                    _originalText = this.Text;
                    StartSpinner();
                }
                else
                {
                    StopSpinner();
                    if (!string.IsNullOrEmpty(_originalText))
                    {
                        this.Text = _originalText;
                    }
                }
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the text displayed while <see cref="IsLoading"/> is active.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Text displayed during loading state.")]
        [DisplayName("Loading Text")]
        [DefaultValue("Loading...")]
        public string LoadingText
        {
            get => _loadingText;
            set
            {
                _loadingText = value;
                if (_isLoading) this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the badge notification text shown on the top-right corner.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Badge notification text (e.g. '5', 'NEW') displayed at corner.")]
        [DisplayName("Badge Text")]
        [DefaultValue("")]
        public string BadgeText
        {
            get => _badgeText;
            set
            {
                _badgeText = value ?? string.Empty;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the background color of the badge.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Background color for the corner badge.")]
        [DisplayName("Badge BackColor")]
        [DefaultValue(typeof(Color), "Red")]
        public Color BadgeBackColor
        {
            get => _badgeBackColor;
            set
            {
                _badgeBackColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the text color of the badge.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Text color for the corner badge.")]
        [DisplayName("Badge TextColor")]
        [DefaultValue(typeof(Color), "White")]
        public Color BadgeTextColor
        {
            get => _badgeTextColor;
            set
            {
                _badgeTextColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the progress completion percentage (0 - 100) rendered behind or over the button.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Progress percentage (0-100) rendered as a progress indicator.")]
        [DisplayName("Progress Value")]
        [DefaultValue(0)]
        public int ProgressValue
        {
            get => _progressValue;
            set
            {
                _progressValue = Math.Max(0, Math.Min(100, value));
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the color of the progress bar overlay.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Color of the progress bar indicator.")]
        [DisplayName("Progress Color")]
        public Color ProgressColor
        {
            get => _progressColor;
            set
            {
                _progressColor = value;
                this.Invalidate();
            }
        }

        private void StartSpinner()
        {
            if (_spinnerTimer == null)
            {
                _spinnerTimer = new Timer { Interval = 40 };
                _spinnerTimer.Tick += (s, e) =>
                {
                    _spinnerAngle = (_spinnerAngle + 30) % 360;
                    this.Invalidate();
                };
            }
            _spinnerTimer.Start();
        }

        private void StopSpinner()
        {
            _spinnerTimer?.Stop();
            _spinnerTimer?.Dispose();
            _spinnerTimer = null;
        }

        /// <inheritdoc />
        protected override void OnClick(EventArgs e)
        {
            if (_isLoading) return;
            base.OnClick(e);
        }

        /// <inheritdoc />
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            Graphics g = pevent.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Draw progress bar overlay if > 0
            if (_progressValue > 0)
            {
                int progressWidth = (int)(this.Width * (_progressValue / 100.0f));
                using (var progressBrush = new SolidBrush(_progressColor))
                {
                    g.FillRectangle(progressBrush, 0, this.Height - 4, progressWidth, 4);
                }
            }

            // Draw loading overlay
            if (_isLoading)
            {
                using (var overlayBrush = new SolidBrush(Color.FromArgb(180, this.BackColor)))
                {
                    g.FillRectangle(overlayBrush, this.ClientRectangle);
                }

                // Draw spinner
                int spinnerSize = Math.Min(16, this.Height - 8);
                int spinnerX = 8;
                int spinnerY = (this.Height - spinnerSize) / 2;

                using (var spinnerPen = new Pen(this.ForeColor, 2f))
                {
                    g.DrawArc(spinnerPen, spinnerX, spinnerY, spinnerSize, spinnerSize, _spinnerAngle, 270);
                }

                // Draw loading text
                string textToDraw = string.IsNullOrEmpty(_loadingText) ? "..." : _loadingText;
                var textRect = new Rectangle(spinnerX + spinnerSize + 4, 0, this.Width - spinnerSize - 16, this.Height);
                TextRenderer.DrawText(g, textToDraw, this.Font, textRect, this.ForeColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
            }

            // Draw badge
            if (!string.IsNullOrEmpty(_badgeText))
            {
                using (Font badgeFont = new Font(this.Font.FontFamily, Math.Max(7f, this.Font.Size - 3f), FontStyle.Bold))
                {
                    SizeF badgeSize = g.MeasureString(_badgeText, badgeFont);
                    int bw = Math.Max((int)badgeSize.Width + 6, 16);
                    int bh = Math.Max((int)badgeSize.Height + 2, 16);
                    int bx = this.Width - bw - 4;
                    int by = 3;

                    var badgeRect = new Rectangle(bx, by, bw, bh);
                    using (var badgeBrush = new SolidBrush(_badgeBackColor))
                    {
                        g.FillEllipse(badgeBrush, badgeRect);
                    }
                    using (var textBrush = new SolidBrush(_badgeTextColor))
                    using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    {
                        g.DrawString(_badgeText, badgeFont, textBrush, badgeRect, sf);
                    }
                }
            }
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopSpinner();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets or sets the background color when the button is focused or hovered.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set color when focused")]
        [DisplayName("Focused Color")]
        [DefaultValue(typeof(Color), "White")]
        public Color FocusedColor
        {
            get => _focusedColor;
            set
            {
                _focusedColor = value;
                if (_isHoveredOrFocused) UpdateColors();
            }
        }

        /// <summary>
        /// Gets or sets the background color when the button is not focused or hovered.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set color when lost focus or leave the control")]
        [DisplayName("Unfocused Color")]
        [DefaultValue(typeof(Color), "Gray")]
        public Color UnfocusedColor
        {
            get => _unfocusedColor;
            set
            {
                _unfocusedColor = value;
                if (!_isHoveredOrFocused) UpdateColors();
            }
        }

        /// <summary>
        /// Obsolete. Use <see cref="UnfocusedColor"/> instead.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use UnfocusedColor instead.")]
        public Color unFocusedColor
        {
            get => UnfocusedColor;
            set => UnfocusedColor = value;
        }

        /// <summary>
        /// Gets or sets the text color when the button is focused or hovered.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set text color when focused")]
        [DisplayName("Focused Text Color")]
        [DefaultValue(typeof(Color), "Black")]
        public Color FocusedTextColor
        {
            get => _focusedTextColor;
            set
            {
                _focusedTextColor = value;
                if (_isHoveredOrFocused) UpdateColors();
            }
        }

        /// <summary>
        /// Gets or sets the text color when the button is not focused or hovered.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set text color when lost focus or leave the control")]
        [DisplayName("Unfocused Text Color")]
        [DefaultValue(typeof(Color), "DarkGray")]
        public Color UnfocusedTextColor
        {
            get => _unfocusedTextColor;
            set
            {
                _unfocusedTextColor = value;
                if (!_isHoveredOrFocused) UpdateColors();
            }
        }

        /// <summary>
        /// Obsolete. Use <see cref="UnfocusedTextColor"/> instead.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use UnfocusedTextColor instead.")]
        public Color unFocusedTextColor
        {
            get => UnfocusedTextColor;
            set => UnfocusedTextColor = value;
        }

        /// <inheritdoc />
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isHoveredOrFocused = true;
            UpdateColors();
        }

        /// <inheritdoc />
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (!this.Focused)
            {
                _isHoveredOrFocused = false;
                UpdateColors();
            }
        }

        /// <inheritdoc />
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            _isHoveredOrFocused = true;
            UpdateColors();
        }

        /// <inheritdoc />
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            var rect = new Rectangle(Point.Empty, this.Size);
            if (!rect.Contains(this.PointToClient(Cursor.Position)))
            {
                _isHoveredOrFocused = false;
                UpdateColors();
            }
        }

        private void UpdateColors()
        {
            this.BackColor = _isHoveredOrFocused ? _focusedColor : _unfocusedColor;
            this.ForeColor = _isHoveredOrFocused ? _focusedTextColor : _unfocusedTextColor;
        }
    }
}
