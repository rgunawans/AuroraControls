using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AuroraControls
{
    /// <summary>
    /// Icon types for <see cref="AMessageBox"/>.
    /// </summary>
    public enum AMessageBoxIcon
    {
        None,
        Info,
        Success,
        Warning,
        Error,
        Question
    }

    /// <summary>
    /// Theme mode for <see cref="AMessageBox"/>.
    /// </summary>
    public enum AMessageBoxTheme
    {
        Light,
        Dark
    }

    /// <summary>
    /// Configuration options for custom <see cref="AMessageBox"/> dialogs.
    /// </summary>
    public class AMessageBoxOptions
    {
        public string Title { get; set; } = "Notification";
        public string Message { get; set; } = string.Empty;
        public string DetailsText { get; set; }
        public MessageBoxButtons Buttons { get; set; } = MessageBoxButtons.OK;
        public AMessageBoxIcon Icon { get; set; } = AMessageBoxIcon.Info;
        public AMessageBoxTheme Theme { get; set; } = AMessageBoxTheme.Light;
        public Color? AccentColor { get; set; }
        public Color CardBackColor { get; set; } = Color.Empty;
        public Color TitleColor { get; set; } = Color.Empty;
        public Color MessageColor { get; set; } = Color.Empty;
        public Color BorderColor { get; set; } = Color.Empty;
        public Font TitleFont { get; set; }
        public Font MessageFont { get; set; }
        public int AutoCloseSeconds { get; set; } = 0;
        public bool ShowCloseButton { get; set; } = true;
        public int CornerRadius { get; set; } = 12;
        public string CustomButton1Text { get; set; }
        public string CustomButton2Text { get; set; }
        public string CustomButton3Text { get; set; }

        // Input prompt mode
        public bool ShowInput { get; set; } = false;
        public string DefaultInputText { get; set; } = string.Empty;
        public string InputPlaceholder { get; set; } = string.Empty;
        public string InputResult { get; set; } = string.Empty;
    }

    /// <summary>
    /// Modern customizable card-style message box replacement for WinForms.
    /// </summary>
    public static class AMessageBox
    {
        public static Color DefaultAccentColor { get; set; } = Color.FromArgb(94, 44, 165);

        public static DialogResult Show(string message) =>
            Show(null, message, "Notification", MessageBoxButtons.OK, AMessageBoxIcon.Info, null);

        public static DialogResult Show(string message, string title) =>
            Show(null, message, title, MessageBoxButtons.OK, AMessageBoxIcon.Info, null);

        public static DialogResult Show(string message, string title, MessageBoxButtons buttons) =>
            Show(null, message, title, buttons, AMessageBoxIcon.Info, null);

        public static DialogResult Show(string message, string title, MessageBoxButtons buttons, AMessageBoxIcon icon) =>
            Show(null, message, title, buttons, icon, null);

        public static DialogResult Show(string message, string title, MessageBoxButtons buttons, AMessageBoxIcon icon, Color accentColor) =>
            Show(null, message, title, buttons, icon, (Color?)accentColor);

        public static DialogResult Show(IWin32Window owner, string message, string title = "Notification",
            MessageBoxButtons buttons = MessageBoxButtons.OK, AMessageBoxIcon icon = AMessageBoxIcon.Info, Color? accentColor = null)
        {
            var options = new AMessageBoxOptions
            {
                Title = title,
                Message = message,
                Buttons = buttons,
                Icon = icon,
                AccentColor = accentColor
            };
            return Show(owner, options);
        }

        public static DialogResult Show(AMessageBoxOptions options) => Show(null, options);

        public static DialogResult Show(IWin32Window owner, AMessageBoxOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            using (var form = new AMessageBoxForm(options))
            {
                if (owner != null)
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    var res = form.ShowDialog(owner);
                    options.InputResult = form.InputResult;
                    return res;
                }
                form.StartPosition = FormStartPosition.CenterScreen;
                var result = form.ShowDialog();
                options.InputResult = form.InputResult;
                return result;
            }
        }

        public static string ShowInput(string message, string title = "Input", string defaultValue = "", string placeholder = "")
        {
            return ShowInput(null, message, title, defaultValue, placeholder);
        }

        public static string ShowInput(IWin32Window owner, string message, string title = "Input", string defaultValue = "", string placeholder = "")
        {
            var options = new AMessageBoxOptions
            {
                Title = title,
                Message = message,
                Buttons = MessageBoxButtons.OKCancel,
                Icon = AMessageBoxIcon.Question,
                ShowInput = true,
                DefaultInputText = defaultValue,
                InputPlaceholder = placeholder
            };

            DialogResult res = Show(owner, options);
            return res == DialogResult.OK ? options.InputResult : null;
        }

        public static void ShowToast(string message, string title = "Notification", AMessageBoxIcon icon = AMessageBoxIcon.Info, int durationSeconds = 3)
        {
            var options = new AMessageBoxOptions
            {
                Title = title,
                Message = message,
                Icon = icon,
                Buttons = MessageBoxButtons.OK,
                AutoCloseSeconds = durationSeconds,
                ShowCloseButton = true
            };

            var form = new AMessageBoxForm(options, isToast: true);
            form.Show();
        }
    }

    /// <summary>
    /// Internal dialog form for rendering modern card-style message box.
    /// </summary>
    internal sealed class AMessageBoxForm : Form
    {
        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private readonly AMessageBoxOptions _options;
        private readonly Color _accent;
        private readonly Color _cardBg;
        private readonly Color _borderCol;
        private readonly Color _textCol;
        private readonly Color _titleCol;
        private readonly bool _isToast;

        private readonly Timer _autoCloseTimer;
        private int _remainingSeconds;

        private Rectangle _closeButtonRect;
        private bool _isCloseHovered = false;

        private TextBox _inputBox;
        private Panel _detailsPanel;
        private TextBox _detailsTextBox;
        private Button _detailsToggleButton;
        private ModernCardButton _primaryBtn;
        private string _originalPrimaryBtnText;
        private bool _isDetailsExpanded = false;

        public string InputResult => _inputBox != null ? _inputBox.Text : string.Empty;

        public AMessageBoxForm(AMessageBoxOptions options, bool isToast = false)
        {
            _options = options;
            _isToast = isToast;
            _accent = options.AccentColor ?? GetDefaultIconColor(options.Icon);

            bool isDark = options.Theme == AMessageBoxTheme.Dark;
            _cardBg = !options.CardBackColor.IsEmpty ? options.CardBackColor : (isDark ? Color.FromArgb(28, 28, 34) : Color.White);
            _borderCol = !options.BorderColor.IsEmpty ? options.BorderColor : (isDark ? Color.FromArgb(50, 50, 60) : Color.FromArgb(220, 220, 230));
            _textCol = !options.MessageColor.IsEmpty ? options.MessageColor : (isDark ? Color.FromArgb(215, 215, 225) : Color.FromArgb(60, 60, 65));
            _titleCol = !options.TitleColor.IsEmpty ? options.TitleColor : _accent;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            BackColor = _cardBg;
            Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            KeyPreview = true;

            BuildLayout();

            if (options.AutoCloseSeconds > 0)
            {
                _remainingSeconds = options.AutoCloseSeconds;
                UpdatePrimaryButtonCountdown();

                _autoCloseTimer = new Timer { Interval = 1000 };
                _autoCloseTimer.Tick += (s, e) =>
                {
                    _remainingSeconds--;
                    UpdatePrimaryButtonCountdown();
                    if (_remainingSeconds <= 0)
                    {
                        _autoCloseTimer.Stop();
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                };
                _autoCloseTimer.Start();
            }

            if (_isToast)
            {
                PositionAsToast();
            }
        }

        private void PositionAsToast()
        {
            StartPosition = FormStartPosition.Manual;
            var wa = Screen.PrimaryScreen.WorkingArea;
            Location = new Point(wa.Right - Width - 20, wa.Bottom - Height - 20);
            TopMost = true;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        private static Color GetDefaultIconColor(AMessageBoxIcon icon)
        {
            switch (icon)
            {
                case AMessageBoxIcon.Success:
                    return Color.FromArgb(40, 167, 69);
                case AMessageBoxIcon.Warning:
                    return Color.FromArgb(235, 145, 0);
                case AMessageBoxIcon.Error:
                    return Color.FromArgb(220, 53, 69);
                case AMessageBoxIcon.Question:
                    return Color.FromArgb(23, 162, 184);
                case AMessageBoxIcon.Info:
                default:
                    return AMessageBox.DefaultAccentColor;
            }
        }

        private void BuildLayout()
        {
            const int minWidth = 420;
            const int maxWidth = 640;
            const int paddingLeft = 78;
            const int paddingRight = 36;
            const int contentWidth = minWidth - paddingLeft - paddingRight;

            Font titleFont = _options.TitleFont ?? new Font("Segoe UI", 12.5f, FontStyle.Bold);
            Font msgFont = _options.MessageFont ?? new Font("Segoe UI", 9.5f, FontStyle.Regular);

            Size titleSize = TextRenderer.MeasureText(_options.Title, titleFont, new Size(contentWidth, int.MaxValue), TextFormatFlags.WordBreak);
            Size msgSize = TextRenderer.MeasureText(_options.Message, msgFont, new Size(contentWidth, int.MaxValue), TextFormatFlags.WordBreak);

            int calculatedWidth = Math.Max(minWidth, Math.Min(maxWidth, paddingLeft + Math.Max(titleSize.Width, msgSize.Width) + paddingRight));
            int contentHeight = 24 + titleSize.Height + 8 + msgSize.Height + 16;

            if (_options.ShowInput)
            {
                contentHeight += 44;
            }

            if (!string.IsNullOrEmpty(_options.DetailsText))
            {
                contentHeight += 26;
            }

            int footerHeight = _isToast ? 16 : 62;
            int totalHeight = contentHeight + footerHeight;

            ClientSize = new Size(calculatedWidth, totalHeight);
            _closeButtonRect = new Rectangle(calculatedWidth - 36, 12, 24, 24);

            int currentY = 24 + titleSize.Height + 8 + msgSize.Height + 8;

            // Optional Input text box
            if (_options.ShowInput)
            {
                _inputBox = new TextBox
                {
                    Location = new Point(paddingLeft, currentY),
                    Size = new Size(calculatedWidth - paddingLeft - paddingRight, 28),
                    Font = new Font("Segoe UI", 10f),
                    Text = _options.DefaultInputText,
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = _options.Theme == AMessageBoxTheme.Dark ? Color.FromArgb(40, 40, 48) : Color.White,
                    ForeColor = _textCol
                };
                Controls.Add(_inputBox);
                currentY += 38;
            }

            // Optional Collapsible Details
            if (!string.IsNullOrEmpty(_options.DetailsText))
            {
                _detailsToggleButton = new Button
                {
                    Text = "▼ Show Details",
                    Location = new Point(paddingLeft, currentY),
                    Size = new Size(110, 22),
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = _accent,
                    BackColor = Color.Transparent,
                    Font = new Font("Segoe UI", 8.25f),
                    Cursor = Cursors.Hand
                };
                _detailsToggleButton.FlatAppearance.BorderSize = 0;
                _detailsToggleButton.Click += (s, e) => ToggleDetails();
                Controls.Add(_detailsToggleButton);

                _detailsPanel = new Panel
                {
                    Location = new Point(paddingLeft, currentY + 26),
                    Size = new Size(calculatedWidth - paddingLeft - paddingRight, 100),
                    Visible = false
                };

                _detailsTextBox = new TextBox
                {
                    Multiline = true,
                    ReadOnly = true,
                    ScrollBars = ScrollBars.Vertical,
                    Dock = DockStyle.Fill,
                    Text = _options.DetailsText,
                    Font = new Font("Consolas", 8.5f),
                    BackColor = _options.Theme == AMessageBoxTheme.Dark ? Color.FromArgb(35, 35, 42) : Color.FromArgb(248, 248, 250),
                    ForeColor = _textCol
                };
                _detailsPanel.Controls.Add(_detailsTextBox);
                Controls.Add(_detailsPanel);
            }

            if (!_isToast)
            {
                CreateButtons(calculatedWidth, totalHeight - footerHeight);
            }
        }

        private void ToggleDetails()
        {
            _isDetailsExpanded = !_isDetailsExpanded;
            _detailsPanel.Visible = _isDetailsExpanded;
            _detailsToggleButton.Text = _isDetailsExpanded ? "▲ Hide Details" : "▼ Show Details";

            int delta = _isDetailsExpanded ? 110 : -110;
            Height += delta;
        }

        private void CreateButtons(int formWidth, int footerTop)
        {
            var buttonPanel = new Panel
            {
                Location = new Point(14, footerTop + 8),
                Size = new Size(formWidth - 28, 46),
                BackColor = Color.Transparent,
                Dock = DockStyle.Bottom
            };

            int btnWidth = 92;
            int btnHeight = 34;
            int spacing = 10;
            int rightX = buttonPanel.Width - btnWidth;

            ModernCardButton AddBtn(string text, DialogResult result, bool isPrimary)
            {
                var btn = new ModernCardButton
                {
                    Text = text,
                    DialogResult = result,
                    Size = new Size(btnWidth, btnHeight),
                    Location = new Point(rightX, 6),
                    IsPrimary = isPrimary,
                    AccentColor = _accent,
                    IsDarkTheme = _options.Theme == AMessageBoxTheme.Dark,
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
                };
                btn.Click += (s, e) =>
                {
                    DialogResult = result;
                    Close();
                };
                buttonPanel.Controls.Add(btn);
                rightX -= (btnWidth + spacing);
                return btn;
            }

            switch (_options.Buttons)
            {
                case MessageBoxButtons.OK:
                    _primaryBtn = AddBtn(_options.CustomButton1Text ?? "OK", DialogResult.OK, true);
                    break;
                case MessageBoxButtons.OKCancel:
                    AddBtn(_options.CustomButton2Text ?? "Cancel", DialogResult.Cancel, false);
                    _primaryBtn = AddBtn(_options.CustomButton1Text ?? "OK", DialogResult.OK, true);
                    break;
                case MessageBoxButtons.YesNo:
                    AddBtn(_options.CustomButton2Text ?? "No", DialogResult.No, false);
                    _primaryBtn = AddBtn(_options.CustomButton1Text ?? "Yes", DialogResult.Yes, true);
                    break;
                case MessageBoxButtons.YesNoCancel:
                    AddBtn(_options.CustomButton3Text ?? "Cancel", DialogResult.Cancel, false);
                    AddBtn(_options.CustomButton2Text ?? "No", DialogResult.No, false);
                    _primaryBtn = AddBtn(_options.CustomButton1Text ?? "Yes", DialogResult.Yes, true);
                    break;
                case MessageBoxButtons.RetryCancel:
                    AddBtn(_options.CustomButton2Text ?? "Cancel", DialogResult.Cancel, false);
                    _primaryBtn = AddBtn(_options.CustomButton1Text ?? "Retry", DialogResult.Retry, true);
                    break;
                default:
                    _primaryBtn = AddBtn(_options.CustomButton1Text ?? "OK", DialogResult.OK, true);
                    break;
            }

            if (_primaryBtn != null)
            {
                _originalPrimaryBtnText = _primaryBtn.Text;
            }

            Controls.Add(buttonPanel);
        }

        private void UpdatePrimaryButtonCountdown()
        {
            if (_primaryBtn != null && _remainingSeconds > 0)
            {
                _primaryBtn.Text = $"{_originalPrimaryBtnText} ({_remainingSeconds}s)";
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
            else if (e.KeyCode == Keys.Enter && !_options.ShowInput)
            {
                DialogResult = _primaryBtn != null ? _primaryBtn.DialogResult : DialogResult.OK;
                Close();
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                try
                {
                    Clipboard.SetText($"{_options.Title}\r\n{_options.Message}");
                }
                catch { }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                if (_options.ShowCloseButton && _closeButtonRect.Contains(e.Location))
                {
                    DialogResult = DialogResult.Cancel;
                    Close();
                    return;
                }

                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_options.ShowCloseButton)
            {
                bool hovered = _closeButtonRect.Contains(e.Location);
                if (hovered != _isCloseHovered)
                {
                    _isCloseHovered = hovered;
                    Invalidate(_closeButtonRect);
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_isCloseHovered)
            {
                _isCloseHovered = false;
                Invalidate(_closeButtonRect);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var bounds = new Rectangle(0, 0, Width - 1, Height - 1);

            // 1. Draw outer rounded border & fill
            using (var path = GetRoundedRectangle(bounds, _options.CornerRadius))
            {
                using (var brush = new SolidBrush(_cardBg))
                {
                    g.FillPath(brush, path);
                }
                using (var pen = new Pen(_borderCol, 1.2f))
                {
                    g.DrawPath(pen, path);
                }
            }

            // 2. Draw Left Accent Stripe
            int accentWidth = 6;
            using (var accentBrush = new SolidBrush(_accent))
            {
                using (var accentPath = new GraphicsPath())
                {
                    accentPath.AddArc(0, 0, _options.CornerRadius * 2, _options.CornerRadius * 2, 180, 90);
                    accentPath.AddLine(_options.CornerRadius, 0, accentWidth, 0);
                    accentPath.AddLine(accentWidth, Height - _options.CornerRadius, accentWidth, Height);
                    accentPath.AddLine(accentWidth, Height, _options.CornerRadius, Height);
                    accentPath.AddArc(0, Height - _options.CornerRadius * 2, _options.CornerRadius * 2, _options.CornerRadius * 2, 90, 90);
                    accentPath.CloseFigure();
                    g.FillPath(accentBrush, accentPath);
                }
            }

            // 3. Draw Icon badge
            DrawIconBadge(g, 26, 20, 38, _options.Icon, _accent);

            // 4. Draw Title
            Font titleFont = _options.TitleFont ?? new Font("Segoe UI", 12.5f, FontStyle.Bold);
            using (var titleBrush = new SolidBrush(_titleCol))
            {
                g.DrawString(_options.Title, titleFont, titleBrush, new PointF(76, 18));
            }

            // 5. Draw Message
            Font msgFont = _options.MessageFont ?? new Font("Segoe UI", 9.5f, FontStyle.Regular);
            int contentWidth = Width - 78 - 36;
            var msgRect = new Rectangle(76, 48, contentWidth, Height - (_isToast ? 20 : 62) - 48);
            using (var msgBrush = new SolidBrush(_textCol))
            {
                using (var sf = new StringFormat { Trimming = StringTrimming.EllipsisWord, FormatFlags = StringFormatFlags.LineLimit })
                {
                    g.DrawString(_options.Message, msgFont, msgBrush, msgRect, sf);
                }
            }

            // 6. Draw Divider Line (if not toast)
            if (!_isToast)
            {
                int dividerY = Height - 56;
                Color divCol = _options.Theme == AMessageBoxTheme.Dark ? Color.FromArgb(48, 48, 56) : Color.FromArgb(238, 238, 242);
                using (var divPen = new Pen(divCol, 1f))
                {
                    g.DrawLine(divPen, 18, dividerY, Width - 18, dividerY);
                }
            }

            // 7. Draw Close Button 'x'
            if (_options.ShowCloseButton)
            {
                DrawCloseButton(g, _closeButtonRect, _isCloseHovered);
            }
        }

        private void DrawIconBadge(Graphics g, int x, int y, int size, AMessageBoxIcon icon, Color accent)
        {
            var rect = new Rectangle(x, y, size, size);

            using (var brush = new SolidBrush(accent))
            {
                g.FillEllipse(brush, rect);
            }

            using (var pen = new Pen(Color.White, 2.4f) { StartCap = LineCap.Round, EndCap = LineCap.Round })
            {
                int cx = x + size / 2;
                int cy = y + size / 2;

                switch (icon)
                {
                    case AMessageBoxIcon.Success:
                        g.DrawLines(pen, new[]
                        {
                            new Point(cx - 7, cy),
                            new Point(cx - 2, cy + 5),
                            new Point(cx + 7, cy - 5)
                        });
                        break;

                    case AMessageBoxIcon.Warning:
                        g.DrawLine(pen, cx, cy - 7, cx, cy + 1);
                        using (var dotBrush = new SolidBrush(Color.White))
                        {
                            g.FillEllipse(dotBrush, cx - 1.5f, cy + 4.5f, 3f, 3f);
                        }
                        break;

                    case AMessageBoxIcon.Error:
                        g.DrawLine(pen, cx - 5, cy - 5, cx + 5, cy + 5);
                        g.DrawLine(pen, cx + 5, cy - 5, cx - 5, cy + 5);
                        break;

                    case AMessageBoxIcon.Question:
                        using (var qFont = new Font("Segoe UI", 12f, FontStyle.Bold))
                        using (var qBrush = new SolidBrush(Color.White))
                        {
                            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                            g.DrawString("?", qFont, qBrush, new RectangleF(x, y, size, size), sf);
                        }
                        break;

                    case AMessageBoxIcon.Info:
                    default:
                        using (var dotBrush = new SolidBrush(Color.White))
                        {
                            g.FillEllipse(dotBrush, cx - 1.8f, cy - 8f, 3.6f, 3.6f);
                        }
                        g.DrawLine(pen, cx, cy - 2, cx, cy + 7);
                        break;
                }
            }
        }

        private void DrawCloseButton(Graphics g, Rectangle rect, bool hovered)
        {
            if (hovered)
            {
                Color hCol = _options.Theme == AMessageBoxTheme.Dark ? Color.FromArgb(50, 45, 65) : Color.FromArgb(235, 230, 245);
                using (var hoverBrush = new SolidBrush(hCol))
                {
                    g.FillEllipse(hoverBrush, rect);
                }
            }

            int cx = rect.X + rect.Width / 2;
            int cy = rect.Y + rect.Height / 2;
            int r = 4;

            Color xColor = hovered ? _accent : (_options.Theme == AMessageBoxTheme.Dark ? Color.FromArgb(170, 170, 185) : Color.FromArgb(140, 140, 155));
            using (var xPen = new Pen(xColor, 1.8f) { StartCap = LineCap.Round, EndCap = LineCap.Round })
            {
                g.DrawLine(xPen, cx - r, cy - r, cx + r, cy + r);
                g.DrawLine(xPen, cx + r, cy - r, cx - r, cy + r);
            }
        }

        private static GraphicsPath GetRoundedRectangle(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int diameter = radius * 2;
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _autoCloseTimer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// Lightweight modern rounded action button for <see cref="AMessageBoxForm"/>.
    /// </summary>
    internal sealed class ModernCardButton : Button
    {
        private bool _isHovered;
        private bool _isPressed;

        public bool IsPrimary { get; set; } = true;
        public bool IsDarkTheme { get; set; } = false;
        public Color AccentColor { get; set; } = Color.FromArgb(94, 44, 165);
        public int CornerRadius { get; set; } = 8;

        public ModernCardButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Cursor = Cursors.Hand;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isHovered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isHovered = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _isPressed = true;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _isPressed = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            Color backColor;
            Color textColor;

            if (IsPrimary)
            {
                if (_isPressed)
                    backColor = ControlPaint.Dark(AccentColor, 0.15f);
                else if (_isHovered)
                    backColor = ControlPaint.Light(AccentColor, 0.12f);
                else
                    backColor = AccentColor;

                textColor = Color.White;
            }
            else
            {
                if (IsDarkTheme)
                {
                    if (_isPressed)
                        backColor = Color.FromArgb(45, 45, 55);
                    else if (_isHovered)
                        backColor = Color.FromArgb(55, 55, 68);
                    else
                        backColor = Color.FromArgb(38, 38, 48);

                    textColor = Color.FromArgb(200, 200, 215);
                }
                else
                {
                    if (_isPressed)
                        backColor = Color.FromArgb(220, 220, 225);
                    else if (_isHovered)
                        backColor = Color.FromArgb(238, 238, 242);
                    else
                        backColor = Color.FromArgb(246, 246, 249);

                    textColor = Color.FromArgb(70, 70, 75);
                }
            }

            using (var path = GetRoundedRectangle(rect, CornerRadius))
            {
                using (var brush = new SolidBrush(backColor))
                {
                    g.FillPath(brush, path);
                }

                if (!IsPrimary)
                {
                    Color borderC = IsDarkTheme ? Color.FromArgb(70, 70, 85) : Color.FromArgb(215, 215, 222);
                    using (var borderPen = new Pen(borderC, 1f))
                    {
                        g.DrawPath(borderPen, path);
                    }
                }
            }

            TextRenderer.DrawText(g, Text, Font, rect, textColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private static GraphicsPath GetRoundedRectangle(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int diameter = radius * 2;
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
