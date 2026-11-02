using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace AuroraControls
{
    /// <summary>
    /// Custom Label control supporting web-like hover effects, URL navigation, and clipboard copy.
    /// </summary>
    [ToolboxBitmap(typeof(Label))]
    [DefaultProperty("Text")]
    [DefaultEvent("Click")]
    public class ALabelBox : Label
    {
        private Color _hoverForeColor = Color.Empty;
        private Color _hoverBackColor = Color.Empty;
        private Color _normalForeColor;
        private Color _normalBackColor;
        private Font _normalFont;
        private Font _hoverFont;
        private bool _isHovered = false;

        private bool _hoverUnderline = false;
        private Cursor _hoverCursor = Cursors.Hand;

        private bool _isLink = false;
        private string _url = string.Empty;
        private bool _autoOpenUrl = true;

        private bool _enableCopyOnClick = false;
        private bool _enableContextMenu = true;
        private string _copyText = string.Empty;
        private bool _showFeedbackOnCopy = true;
        private string _copyFeedbackMessage = "Copied to clipboard!";
        private ToolTip _copyToolTip;

        private ContextMenuStrip _contextMenu;
        private ToolStripMenuItem _copyMenuItem;

        /// <summary>
        /// Occurs when the URL link is clicked.
        /// </summary>
        [Category("Action")]
        [Description("Occurs when the label link is clicked.")]
        public event EventHandler<string> LinkClicked;

        /// <summary>
        /// Occurs when text is copied from the label to clipboard.
        /// </summary>
        [Category("Action")]
        [Description("Occurs when the label text is copied to the clipboard.")]
        public event EventHandler<string> LabelCopied;

        /// <summary>
        /// Initializes a new instance of the <see cref="ALabelBox"/> class.
        /// </summary>
        public ALabelBox()
        {
            _normalForeColor = this.ForeColor;
            _normalBackColor = this.BackColor;
            _normalFont = this.Font;

            SetupContextMenu();
        }

        private void SetupContextMenu()
        {
            _contextMenu = new ContextMenuStrip();
            _copyMenuItem = new ToolStripMenuItem("Copy", null, (s, e) => CopyToClipboard());
            _contextMenu.Items.Add(_copyMenuItem);
        }

        #region Extended Properties - Hover

        /// <summary>
        /// Gets or sets the text color when the mouse hovers over the label.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties - Hover")]
        [Description("Color of the text when mouse hovers over the control.")]
        [DisplayName("Hover ForeColor")]
        public Color HoverForeColor
        {
            get => _hoverForeColor;
            set
            {
                _hoverForeColor = value;
                if (_isHovered) Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the background color when the mouse hovers over the label.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties - Hover")]
        [Description("Background color when mouse hovers over the control.")]
        [DisplayName("Hover BackColor")]
        public Color HoverBackColor
        {
            get => _hoverBackColor;
            set
            {
                _hoverBackColor = value;
                if (_isHovered) Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether text is underlined when hovered.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties - Hover")]
        [Description("Underlines text on hover (web link style).")]
        [DisplayName("Hover Underline")]
        [DefaultValue(false)]
        public bool HoverUnderline
        {
            get => _hoverUnderline;
            set
            {
                _hoverUnderline = value;
                UpdateHoverFont();
            }
        }

        /// <summary>
        /// Gets or sets the cursor displayed when mouse hovers over the label.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties - Hover")]
        [Description("Cursor displayed when mouse hovers over the control.")]
        [DisplayName("Hover Cursor")]
        public Cursor HoverCursor
        {
            get => _hoverCursor;
            set => _hoverCursor = value;
        }

        #endregion

        #region Extended Properties - URL & Link

        /// <summary>
        /// Gets or sets a value indicating whether this label functions as a clickable hyperlink.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties - Link")]
        [Description("Enables hyperlink behavior.")]
        [DisplayName("Is Link")]
        [DefaultValue(false)]
        public bool IsLink
        {
            get => _isLink;
            set
            {
                _isLink = value;
                if (_isLink && _hoverForeColor == Color.Empty)
                {
                    _hoverForeColor = Color.RoyalBlue;
                    _hoverUnderline = true;
                    UpdateHoverFont();
                }
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the destination URL when clicked.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties - Link")]
        [Description("Target URL opened when label is clicked.")]
        [DisplayName("URL")]
        [DefaultValue("")]
        public string Url
        {
            get => _url;
            set => _url = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to automatically launch the default browser when clicked.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties - Link")]
        [Description("Automatically open the URL in the system default web browser.")]
        [DisplayName("Auto Open URL")]
        [DefaultValue(true)]
        public bool AutoOpenUrl
        {
            get => _autoOpenUrl;
            set => _autoOpenUrl = value;
        }

        #endregion

        #region Extended Properties - Copy

        /// <summary>
        /// Gets or sets a value indicating whether left-clicking the label copies text to the clipboard.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties - Copy")]
        [Description("Enables copying text to clipboard when left clicked.")]
        [DisplayName("Enable Copy On Click")]
        [DefaultValue(false)]
        public bool EnableCopyOnClick
        {
            get => _enableCopyOnClick;
            set => _enableCopyOnClick = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether right-click context menu provides a Copy option.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties - Copy")]
        [Description("Shows right-click context menu with Copy command.")]
        [DisplayName("Enable Context Menu")]
        [DefaultValue(true)]
        public bool EnableContextMenu
        {
            get => _enableContextMenu;
            set => _enableContextMenu = value;
        }

        /// <summary>
        /// Gets or sets custom text to be copied to the clipboard. If empty, uses the label Text.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties - Copy")]
        [Description("Custom text to copy. If empty, control Text is copied.")]
        [DisplayName("Copy Text")]
        [DefaultValue("")]
        public string CopyText
        {
            get => _copyText;
            set => _copyText = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether a brief feedback tooltip is shown upon copying.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties - Copy")]
        [Description("Shows brief tooltip feedback when text is copied.")]
        [DisplayName("Show Feedback On Copy")]
        [DefaultValue(true)]
        public bool ShowFeedbackOnCopy
        {
            get => _showFeedbackOnCopy;
            set => _showFeedbackOnCopy = value;
        }

        /// <summary>
        /// Gets or sets the message displayed when text is copied.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties - Copy")]
        [Description("Feedback message displayed after copying.")]
        [DisplayName("Copy Feedback Message")]
        [DefaultValue("Copied to clipboard!")]
        public string CopyFeedbackMessage
        {
            get => _copyFeedbackMessage;
            set => _copyFeedbackMessage = value;
        }

        #endregion

        private void UpdateHoverFont()
        {
            _hoverFont?.Dispose();
            if (_hoverUnderline && this.Font != null)
            {
                _hoverFont = new Font(this.Font, this.Font.Style | FontStyle.Underline);
            }
            else
            {
                _hoverFont = null;
            }
        }

        /// <inheritdoc />
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (!_isHovered)
            {
                _normalFont = this.Font;
            }
            UpdateHoverFont();
        }

        /// <inheritdoc />
        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            if (!_isHovered)
            {
                _normalForeColor = this.ForeColor;
            }
        }

        /// <inheritdoc />
        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            if (!_isHovered)
            {
                _normalBackColor = this.BackColor;
            }
        }

        /// <inheritdoc />
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isHovered = true;

            if (_hoverForeColor != Color.Empty)
            {
                base.ForeColor = _hoverForeColor;
            }

            if (_hoverBackColor != Color.Empty)
            {
                base.BackColor = _hoverBackColor;
            }

            if (_hoverUnderline && _hoverFont != null)
            {
                base.Font = _hoverFont;
            }

            if (_isLink || _enableCopyOnClick || _hoverForeColor != Color.Empty || _hoverUnderline)
            {
                this.Cursor = _hoverCursor;
            }
        }

        /// <inheritdoc />
        protected override void OnMouseLeave(EventArgs e)
        {
            _isHovered = false;

            if (_hoverForeColor != Color.Empty)
            {
                base.ForeColor = _normalForeColor;
            }

            if (_hoverBackColor != Color.Empty)
            {
                base.BackColor = _normalBackColor;
            }

            if (_hoverUnderline && _normalFont != null)
            {
                base.Font = _normalFont;
            }

            this.Cursor = Cursors.Default;
            base.OnMouseLeave(e);
        }

        /// <inheritdoc />
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Right && _enableContextMenu)
            {
                _contextMenu.Show(this, e.Location);
            }
        }

        /// <inheritdoc />
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            if (_isLink || !string.IsNullOrWhiteSpace(_url))
            {
                ExecuteLink();
            }

            if (_enableCopyOnClick)
            {
                CopyToClipboard();
            }
        }

        /// <summary>
        /// Executes URL opening action and triggers <see cref="LinkClicked"/> event.
        /// </summary>
        public void ExecuteLink()
        {
            string targetUrl = !string.IsNullOrWhiteSpace(_url) ? _url : this.Text;
            LinkClicked?.Invoke(this, targetUrl);

            if (_autoOpenUrl && !string.IsNullOrWhiteSpace(targetUrl))
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = targetUrl,
                        UseShellExecute = true
                    });
                }
                catch
                {
                    try
                    {
                        Process.Start(targetUrl);
                    }
                    catch
                    {
                        // Handle unresolvable URLs gracefully
                    }
                }
            }
        }

        /// <summary>
        /// Copies current text (or <see cref="CopyText"/>) to the clipboard.
        /// </summary>
        public void CopyToClipboard()
        {
            string textToCopy = !string.IsNullOrEmpty(_copyText) ? _copyText : this.Text;
            if (string.IsNullOrEmpty(textToCopy)) return;

            try
            {
                if (System.Threading.Thread.CurrentThread.GetApartmentState() == System.Threading.ApartmentState.STA)
                {
                    Clipboard.SetText(textToCopy);
                }
                else
                {
                    var thread = new System.Threading.Thread(() =>
                    {
                        try
                        {
                            Clipboard.SetText(textToCopy);
                        }
                        catch
                        {
                        }
                    });
                    thread.SetApartmentState(System.Threading.ApartmentState.STA);
                    thread.Start();
                    thread.Join(500);
                }
            }
            catch
            {
                // Clipboard lock protection
            }

            LabelCopied?.Invoke(this, textToCopy);

            if (_showFeedbackOnCopy)
            {
                ShowFeedback();
            }
        }

        private void ShowFeedback()
        {
            if (_copyToolTip == null)
            {
                _copyToolTip = new ToolTip
                {
                    IsBalloon = true,
                    ToolTipTitle = "Notice"
                };
            }

            _copyToolTip.Show(_copyFeedbackMessage, this, this.Width / 2, -30, 1500);
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _hoverFont?.Dispose();
                _copyToolTip?.Dispose();
                _contextMenu?.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// Alias for <see cref="ALabelBox"/>.
    /// </summary>
    [ToolboxBitmap(typeof(Label))]
    public class ALabel : ALabelBox
    {
    }
}
