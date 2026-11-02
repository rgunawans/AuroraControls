using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AuroraControls
{
    /// <summary>
    /// Specifies the text casing style for <see cref="ATextBox"/>.
    /// </summary>
    public enum TextCasing
    {
        /// <summary>Normal text as entered.</summary>
        Normal,
        /// <summary>Capitalize the first letter of each sentence.</summary>
        SentenceCase,
        /// <summary>Convert all characters to lowercase.</summary>
        LowerCase,
        /// <summary>Convert all characters to uppercase.</summary>
        UpperCase,
        /// <summary>Capitalize the first letter of each word.</summary>
        CapitalizeEachWord
    }

    /// <summary>
    /// Custom TextBox with focus colors, select-all behavior, mandatory state, text casing, cue banner placeholder, and debounced search.
    /// </summary>
    [ToolboxBitmap(typeof(TextBox))]
    public class ATextBox : TextBox
    {
        private const int ECM_FIRST = 0x1500;
        private const int EM_SETCUEBANNER = ECM_FIRST + 1;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        private Color _focusColor = Color.White;
        private Color _unfocusColor = Color.White;
        private Color _focusTextColor = Color.Black;
        private Color _unfocusTextColor = Color.Black;
        private Color _mandatoryColor = Color.MistyRose;
        private bool _selectAllOnFocus = false;
        private bool _isMandatory = false;
        private TextCasing _textCasing = TextCasing.Normal;
        private bool _isFormatting = false;
        private string _placeholderText = string.Empty;
        private int _debounceDelay = 0;
        private Timer _debounceTimer;

        /// <summary>
        /// Occurs when text change has paused for the duration specified by <see cref="DebounceDelay"/>.
        /// </summary>
        [Category("Extended Events")]
        [Description("Occurs when text editing pauses for the duration specified by DebounceDelay.")]
        public event EventHandler DebouncedTextChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ATextBox"/> class.
        /// </summary>
        public ATextBox()
        {
            UpdateColors();
        }

        /// <summary>
        /// Gets or sets the placeholder / cue banner text displayed when the control is empty.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Placeholder text shown when textbox is empty.")]
        [DisplayName("Placeholder Text")]
        [DefaultValue("")]
#if NETCOREAPP || NET8_0_OR_GREATER
        public new string PlaceholderText
#else
        public string PlaceholderText
#endif
        {
            get => _placeholderText;
            set
            {
                _placeholderText = value ?? string.Empty;
#if NETCOREAPP || NET8_0_OR_GREATER
                base.PlaceholderText = _placeholderText;
#endif
                UpdateCueBanner();
            }
        }

        /// <summary>
        /// Gets or sets the debounce delay in milliseconds before triggering <see cref="DebouncedTextChanged"/>.
        /// Set to 0 to disable debouncing.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Delay in milliseconds to debounce text changes. 0 disables debouncing.")]
        [DisplayName("Debounce Delay (ms)")]
        [DefaultValue(0)]
        public int DebounceDelay
        {
            get => _debounceDelay;
            set
            {
                _debounceDelay = Math.Max(0, value);
                if (_debounceDelay == 0 && _debounceTimer != null)
                {
                    _debounceTimer.Stop();
                    _debounceTimer.Dispose();
                    _debounceTimer = null;
                }
            }
        }

        /// <summary>
        /// Forces any pending debounced change event to fire immediately.
        /// </summary>
        public void FlushDebounce()
        {
            if (_debounceTimer != null && _debounceTimer.Enabled)
            {
                _debounceTimer.Stop();
                OnDebouncedTextChanged();
            }
        }

        private void UpdateCueBanner()
        {
            if (this.IsHandleCreated)
            {
                SendMessage(this.Handle, EM_SETCUEBANNER, (IntPtr)1, _placeholderText);
            }
        }

        /// <inheritdoc />
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateCueBanner();
        }

        /// <summary>
        /// Triggers the <see cref="DebouncedTextChanged"/> event.
        /// </summary>
        protected virtual void OnDebouncedTextChanged()
        {
            DebouncedTextChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Gets or sets the text casing rule applied to the control.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Specifies the casing formatting applied to the text.")]
        [DisplayName("Text Casing")]
        [DefaultValue(TextCasing.Normal)]
        public TextCasing TextCasing
        {
            get => _textCasing;
            set
            {
                _textCasing = value;
                if (_textCasing == TextCasing.UpperCase)
                {
                    this.CharacterCasing = CharacterCasing.Upper;
                }
                else if (_textCasing == TextCasing.LowerCase)
                {
                    this.CharacterCasing = CharacterCasing.Lower;
                }
                else
                {
                    this.CharacterCasing = CharacterCasing.Normal;
                }
                ApplyCasing();
            }
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
            set
            {
                _isMandatory = value;
                UpdateColors();
            }
        }

        /// <summary>
        /// Gets or sets the background color when the textbox is mandatory and left empty.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Color when mandatory field is empty")]
        [DisplayName("Mandatory Color")]
        [DefaultValue(typeof(Color), "MistyRose")]
        public Color MandatoryColor
        {
            get => _mandatoryColor;
            set
            {
                _mandatoryColor = value;
                if (!this.Focused) UpdateColors();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this field satisfies its mandatory requirement.
        /// </summary>
        [Browsable(false)]
        public bool IsValid => !_isMandatory || !string.IsNullOrWhiteSpace(this.Text);

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
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            UpdateColors();
            if (_selectAllOnFocus)
            {
                if (this.IsHandleCreated)
                {
                    this.BeginInvoke((Action)this.SelectAll);
                }
                else
                {
                    this.SelectAll();
                }
            }
        }

        /// <inheritdoc />
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            ApplyCasing();
            UpdateColors();
        }

        /// <inheritdoc />
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            ApplyCasing();
            if (!this.Focused)
            {
                UpdateColors();
            }

            if (_debounceDelay > 0)
            {
                if (_debounceTimer == null)
                {
                    _debounceTimer = new Timer();
                    _debounceTimer.Tick += (s, args) =>
                    {
                        _debounceTimer.Stop();
                        OnDebouncedTextChanged();
                    };
                }
                _debounceTimer.Interval = _debounceDelay;
                _debounceTimer.Stop();
                _debounceTimer.Start();
            }
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _debounceTimer?.Dispose();
                _debounceTimer = null;
            }
            base.Dispose(disposing);
        }

        private void ApplyCasing()
        {
            if (_isFormatting || _textCasing == TextCasing.Normal || string.IsNullOrEmpty(this.Text))
            {
                return;
            }

            string formatted = FormatCasing(this.Text, _textCasing);
            if (formatted != this.Text)
            {
                _isFormatting = true;
                int selStart = this.SelectionStart;
                int selLength = this.SelectionLength;
                this.Text = formatted;
                this.SelectionStart = Math.Min(selStart, this.Text.Length);
                this.SelectionLength = Math.Min(selLength, Math.Max(0, this.Text.Length - this.SelectionStart));
                _isFormatting = false;
            }
        }

        private static string FormatCasing(string input, TextCasing casing)
        {
            if (string.IsNullOrEmpty(input)) return input;

            switch (casing)
            {
                case TextCasing.LowerCase:
                    return input.ToLower();

                case TextCasing.UpperCase:
                    return input.ToUpper();

                case TextCasing.CapitalizeEachWord:
                    return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());

                case TextCasing.SentenceCase:
                    char[] chars = input.ToLower().ToCharArray();
                    bool newSentence = true;
                    for (int i = 0; i < chars.Length; i++)
                    {
                        if (char.IsLetter(chars[i]))
                        {
                            if (newSentence)
                            {
                                chars[i] = char.ToUpper(chars[i]);
                                newSentence = false;
                            }
                        }
                        else if (chars[i] == '.' || chars[i] == '!' || chars[i] == '?' || chars[i] == '\n' || chars[i] == '\r')
                        {
                            newSentence = true;
                        }
                    }
                    return new string(chars);

                default:
                    return input;
            }
        }

        private void UpdateColors()
        {
            if (!this.Focused && _isMandatory && string.IsNullOrWhiteSpace(this.Text))
            {
                this.BackColor = _mandatoryColor;
                this.ForeColor = _unfocusTextColor;
            }
            else
            {
                this.BackColor = this.Focused ? _focusColor : _unfocusColor;
                this.ForeColor = this.Focused ? _focusTextColor : _unfocusTextColor;
            }
        }
    }
}
