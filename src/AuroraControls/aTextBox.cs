using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AuroraControls
{
    /// <summary>
    /// Custom TextBox with focus colors, select-all behavior, and mandatory state.
    /// </summary>
    [ToolboxBitmap(typeof(TextBox))]
    public class ATextBox : TextBox
    {
        private Color _focusColor = Color.White;
        private Color _unfocusColor = Color.White;
        private Color _focusTextColor = Color.Black;
        private Color _unfocusTextColor = Color.Black;
        private bool _selectAllOnFocus = false;
        private bool _isMandatory = false;

        public ATextBox()
        {
            UpdateColors();
        }

        /// <summary>
        /// Gets or sets a value indicating whether this field is required to have a value.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set to true if this field is required")]
        [DisplayName("Mandatory")]
        [DefaultValue(false)]
        public bool Mandatory
        {
            get => _isMandatory;
            set => _isMandatory = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether all text is selected when the control receives focus.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set to true to select all text when focused")]
        [DisplayName("Select All On Focus")]
        [DefaultValue(false)]
        public bool SelectAllOnFocus
        {
            get => _selectAllOnFocus;
            set => _selectAllOnFocus = value;
        }

        /// <summary>
        /// Gets or sets the background color when the textbox is focused.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set color when focused")]
        [DisplayName("Focused Color")]
        [DefaultValue(typeof(Color), "White")]
        public Color FocusedColor
        {
            get => _focusColor;
            set
            {
                _focusColor = value;
                if (this.Focused) UpdateColors();
            }
        }

        /// <summary>
        /// Gets or sets the background color when the textbox is not focused.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set color when lost focus or leave the control")]
        [DisplayName("Unfocused Color")]
        [DefaultValue(typeof(Color), "White")]
        public Color UnfocusedColor
        {
            get => _unfocusColor;
            set
            {
                _unfocusColor = value;
                if (!this.Focused) UpdateColors();
            }
        }

        /// <summary>
        /// Gets or sets the text color when the textbox is focused.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set text color when focused")]
        [DisplayName("Focused Text Color")]
        [DefaultValue(typeof(Color), "Black")]
        public Color FocusedTextColor
        {
            get => _focusTextColor;
            set
            {
                _focusTextColor = value;
                if (this.Focused) UpdateColors();
            }
        }

        /// <summary>
        /// Gets or sets the text color when the textbox is not focused.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Set text color when lost focus or leave the control")]
        [DisplayName("Unfocused Text Color")]
        [DefaultValue(typeof(Color), "Black")]
        public Color UnfocusedTextColor
        {
            get => _unfocusTextColor;
            set
            {
                _unfocusTextColor = value;
                if (!this.Focused) UpdateColors();
            }
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            UpdateColors();
            if (_selectAllOnFocus)
            {
                this.SelectAll();
            }
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            UpdateColors();
        }

        private void UpdateColors()
        {
            this.BackColor = this.Focused ? _focusColor : _unfocusColor;
            this.ForeColor = this.Focused ? _focusTextColor : _unfocusTextColor;
        }
    }
}
