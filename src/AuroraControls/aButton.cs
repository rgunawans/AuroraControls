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

        public AButton()
        {
            UpdateColors();
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

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isHoveredOrFocused = true;
            UpdateColors();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (!this.Focused)
            {
                _isHoveredOrFocused = false;
                UpdateColors();
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            _isHoveredOrFocused = true;
            UpdateColors();
        }

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
