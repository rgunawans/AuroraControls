using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;

namespace AuroraControls
{
    /// <summary>
    /// Supported Barcode Types in ABarcode.
    /// </summary>
    public enum BarcodeType
    {
        Code128,
        Code39,
        EAN13,
        QRCode
    }

    /// <summary>
    /// QR Code Error Correction Levels.
    /// </summary>
    public enum QRCodeErrorCorrection
    {
        Low,       // ~7% recovery
        Medium,    // ~15% recovery
        Quartile,  // ~25% recovery
        High       // ~30% recovery
    }

    /// <summary>
    /// Logo shape rendered in center of 2D Barcode.
    /// </summary>
    public enum BarcodeLogoShape
    {
        Rectangle,
        RoundedRectangle,
        Circle
    }

    /// <summary>
    /// Custom Barcode control supporting 1D (Code 128, Code 39, EAN-13) and 2D (QR Code) formats,
    /// custom fore/back colors, and center logo embedding with high error correction.
    /// </summary>
    [ToolboxBitmap(typeof(PictureBox))]
    [DefaultProperty("Value")]
    [DefaultEvent("Click")]
    public class ABarcode : Control
    {
        #region Private Fields

        private BarcodeType _barcodeType = BarcodeType.QRCode;
        private string _value = "AuroraControls";
        private QRCodeErrorCorrection _errorCorrection = QRCodeErrorCorrection.Medium;

        private Color _foreColor = Color.Black;
        private Color _backColor = Color.White;

        // 1D Barcode Text
        private bool _showText = true;
        private Font _textFont;
        private StringAlignment _textAlignment = StringAlignment.Center;

        // Logo
        private Image _logo = null;
        private int _logoSizePercent = 20; // 5% to 35% of barcode size
        private BarcodeLogoShape _logoShape = BarcodeLogoShape.RoundedRectangle;
        private Color _logoBackgroundColor = Color.White;
        private Color _logoBorderColor = Color.LightGray;
        private int _logoBorderWidth = 2;
        private int _logoPadding = 3;

        // Layout
        private int _quietZone = 10;
        private bool _keepAspectRatio = true;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ABarcode"/> class.
        /// </summary>
        public ABarcode()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            Size = new Size(200, 200);
            BackColor = Color.White;
            ForeColor = Color.Black;
            _textFont = new Font("Segoe UI", 9f, FontStyle.Regular);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the type of barcode to generate.
        /// </summary>
        [Category("Barcode")]
        [DefaultValue(BarcodeType.QRCode)]
        [Description("The symbology/type of barcode to generate.")]
        public BarcodeType BarcodeType
        {
            get => _barcodeType;
            set
            {
                if (_barcodeType != value)
                {
                    _barcodeType = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the string value/payload encoded in the barcode.
        /// </summary>
        [Category("Barcode")]
        [DefaultValue("AuroraControls")]
        [Description("The string data or text payload to encode.")]
        public string Value
        {
            get => _value;
            set
            {
                string val = value ?? string.Empty;
                if (_value != val)
                {
                    _value = val;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the error correction level for QR Code.
        /// When a Logo is used, Quartile or High is strongly recommended.
        /// </summary>
        [Category("Barcode")]
        [DefaultValue(QRCodeErrorCorrection.Medium)]
        [Description("Error correction capability for QR Code (Low: ~7%, Medium: ~15%, Quartile: ~25%, High: ~30%).")]
        public QRCodeErrorCorrection ErrorCorrection
        {
            get => _errorCorrection;
            set
            {
                if (_errorCorrection != value)
                {
                    _errorCorrection = value;
                    if (_barcodeType == BarcodeType.QRCode)
                    {
                        Invalidate();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the color of the barcode bars/modules.
        /// </summary>
        [Category("Barcode Appearance")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Foreground color of the barcode bars or modules.")]
        public new Color ForeColor
        {
            get => _foreColor;
            set
            {
                if (_foreColor != value)
                {
                    _foreColor = value;
                    base.ForeColor = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the background color of the barcode.
        /// </summary>
        [Category("Barcode Appearance")]
        [DefaultValue(typeof(Color), "White")]
        [Description("Background color behind the barcode.")]
        public new Color BackColor
        {
            get => _backColor;
            set
            {
                if (_backColor != value)
                {
                    _backColor = value;
                    base.BackColor = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether human-readable text is displayed beneath 1D barcodes.
        /// </summary>
        [Category("Barcode 1D Text")]
        [DefaultValue(true)]
        [Description("Displays human-readable text below 1D barcodes.")]
        public bool ShowText
        {
            get => _showText;
            set
            {
                if (_showText != value)
                {
                    _showText = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the font used for human-readable text on 1D barcodes.
        /// </summary>
        [Category("Barcode 1D Text")]
        [Description("Font for human-readable text below 1D barcodes.")]
        public Font TextFont
        {
            get => _textFont;
            set
            {
                _textFont = value ?? new Font("Segoe UI", 9f);
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the alignment of human-readable text on 1D barcodes.
        /// </summary>
        [Category("Barcode 1D Text")]
        [DefaultValue(StringAlignment.Center)]
        [Description("Alignment of text below 1D barcodes.")]
        public StringAlignment TextAlignment
        {
            get => _textAlignment;
            set
            {
                if (_textAlignment != value)
                {
                    _textAlignment = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the logo image placed in the center of the barcode (QR Code).
        /// </summary>
        [Category("Barcode Logo")]
        [DefaultValue(null)]
        [Description("Logo image placed in the center of 2D QR Code.")]
        public Image Logo
        {
            get => _logo;
            set
            {
                if (_logo != value)
                {
                    _logo = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the logo size as a percentage of the barcode dimension (5% to 35%).
        /// </summary>
        [Category("Barcode Logo")]
        [DefaultValue(20)]
        [Description("Logo size percentage relative to barcode dimension (5 to 35).")]
        public int LogoSizePercent
        {
            get => _logoSizePercent;
            set
            {
                int clamped = Math.Max(5, Math.Min(35, value));
                if (_logoSizePercent != clamped)
                {
                    _logoSizePercent = clamped;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the shape of the logo container.
        /// </summary>
        [Category("Barcode Logo")]
        [DefaultValue(BarcodeLogoShape.RoundedRectangle)]
        [Description("The clipping / background shape of the logo container.")]
        public BarcodeLogoShape LogoShape
        {
            get => _logoShape;
            set
            {
                if (_logoShape != value)
                {
                    _logoShape = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the background color of the logo area.
        /// </summary>
        [Category("Barcode Logo")]
        [DefaultValue(typeof(Color), "White")]
        [Description("Background color behind the center logo.")]
        public Color LogoBackgroundColor
        {
            get => _logoBackgroundColor;
            set
            {
                if (_logoBackgroundColor != value)
                {
                    _logoBackgroundColor = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the border color around the logo container.
        /// </summary>
        [Category("Barcode Logo")]
        [DefaultValue(typeof(Color), "LightGray")]
        [Description("Border color around the center logo container.")]
        public Color LogoBorderColor
        {
            get => _logoBorderColor;
            set
            {
                if (_logoBorderColor != value)
                {
                    _logoBorderColor = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the border width around the logo container.
        /// </summary>
        [Category("Barcode Logo")]
        [DefaultValue(2)]
        [Description("Border width in pixels around the center logo container.")]
        public int LogoBorderWidth
        {
            get => _logoBorderWidth;
            set
            {
                int val = Math.Max(0, value);
                if (_logoBorderWidth != val)
                {
                    _logoBorderWidth = val;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the padding around the logo image within its container.
        /// </summary>
        [Category("Barcode Logo")]
        [DefaultValue(3)]
        [Description("Inner padding around the logo in pixels.")]
        public int LogoPadding
        {
            get => _logoPadding;
            set
            {
                int val = Math.Max(0, value);
                if (_logoPadding != val)
                {
                    _logoPadding = val;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the quiet zone (margin) in pixels around the barcode.
        /// </summary>
        [Category("Barcode Appearance")]
        [DefaultValue(10)]
        [Description("Margin (quiet zone) around the barcode in pixels.")]
        public int QuietZone
        {
            get => _quietZone;
            set
            {
                int val = Math.Max(0, value);
                if (_quietZone != val)
                {
                    _quietZone = val;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether square aspect ratio is maintained.
        /// </summary>
        [Category("Barcode Appearance")]
        [DefaultValue(true)]
        [Description("When true, maintains a 1:1 aspect ratio for QR Code or centers 1D barcodes.")]
        public bool KeepAspectRatio
        {
            get => _keepAspectRatio;
            set
            {
                if (_keepAspectRatio != value)
                {
                    _keepAspectRatio = value;
                    Invalidate();
                }
            }
        }

        #endregion

        #region Painting

        /// <inheritdoc />
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            RenderBarcode(g, ClientRectangle);
        }

        private void RenderBarcode(Graphics g, Rectangle destRect)
        {
            using (var backBrush = new SolidBrush(_backColor))
            {
                g.FillRectangle(backBrush, destRect);
            }

            if (string.IsNullOrEmpty(_value))
            {
                DrawPlaceholder(g, destRect, "No Value Specified");
                return;
            }

            try
            {
                if (_barcodeType == BarcodeType.QRCode)
                {
                    RenderQRCode(g, destRect);
                }
                else
                {
                    Render1DBarcode(g, destRect);
                }
            }
            catch (Exception ex)
            {
                DrawPlaceholder(g, destRect, "Encoding Error: " + ex.Message);
            }
        }

        private void DrawPlaceholder(Graphics g, Rectangle destRect, string text)
        {
            using (var pen = new Pen(Color.FromArgb(180, Color.LightGray), 1f) { DashStyle = DashStyle.Dash })
            {
                Rectangle r = destRect;
                r.Width -= 1;
                r.Height -= 1;
                g.DrawRectangle(pen, r);
            }
            using (var font = new Font(Font.FontFamily, 8.5f, FontStyle.Italic))
            using (var brush = new SolidBrush(Color.Gray))
            using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                g.DrawString(text, font, brush, destRect, sf);
            }
        }

        private void RenderQRCode(Graphics g, Rectangle destRect)
        {
            // If a logo is present, ensure at least Quartile or High error correction
            QRCodeErrorCorrection effEc = _errorCorrection;
            if (_logo != null && effEc < QRCodeErrorCorrection.Quartile)
            {
                effEc = QRCodeErrorCorrection.High;
            }

            bool[,] matrix = QrCodeGenerator.Generate(_value, effEc);
            if (matrix == null)
            {
                DrawPlaceholder(g, destRect, "QR Code Data Too Large");
                return;
            }

            int moduleCount = matrix.GetLength(0);
            int availableWidth = Math.Max(1, destRect.Width - (_quietZone * 2));
            int availableHeight = Math.Max(1, destRect.Height - (_quietZone * 2));
            int qrSide = _keepAspectRatio ? Math.Min(availableWidth, availableHeight) : Math.Min(availableWidth, availableHeight);

            float moduleSize = (float)qrSide / moduleCount;
            float startX = destRect.X + _quietZone + ((availableWidth - qrSide) / 2f);
            float startY = destRect.Y + _quietZone + ((availableHeight - qrSide) / 2f);

            // Draw QR modules
            using (var moduleBrush = new SolidBrush(_foreColor))
            {
                for (int r = 0; r < moduleCount; r++)
                {
                    for (int c = 0; c < moduleCount; c++)
                    {
                        if (matrix[r, c])
                        {
                            g.FillRectangle(moduleBrush,
                                (float)Math.Round(startX + (c * moduleSize)),
                                (float)Math.Round(startY + (r * moduleSize)),
                                (float)Math.Ceiling(moduleSize),
                                (float)Math.Ceiling(moduleSize));
                        }
                    }
                }
            }

            // Draw center logo if configured
            if (_logo != null)
            {
                DrawQrLogo(g, startX, startY, qrSide);
            }
        }

        private void DrawQrLogo(Graphics g, float startX, float startY, float qrSide)
        {
            float logoSide = qrSide * (_logoSizePercent / 100f);
            float logoX = startX + ((qrSide - logoSide) / 2f);
            float logoY = startY + ((qrSide - logoSide) / 2f);
            var logoRect = new RectangleF(logoX, logoY, logoSide, logoSide);

            // Logo Background and Shape
            using (GraphicsPath path = CreateShapePath(logoRect, _logoShape))
            {
                using (var bgBrush = new SolidBrush(_logoBackgroundColor))
                {
                    g.FillPath(bgBrush, path);
                }

                // Draw image clipped inside shape
                RectangleF imgRect = new RectangleF(
                    logoRect.X + _logoPadding,
                    logoRect.Y + _logoPadding,
                    Math.Max(1, logoRect.Width - (_logoPadding * 2)),
                    Math.Max(1, logoRect.Height - (_logoPadding * 2)));

                var state = g.Save();
                g.SetClip(path);
                g.DrawImage(_logo, imgRect);
                g.Restore(state);

                // Draw logo border
                if (_logoBorderWidth > 0 && _logoBorderColor != Color.Transparent)
                {
                    using (var borderPen = new Pen(_logoBorderColor, _logoBorderWidth))
                    {
                        borderPen.Alignment = PenAlignment.Inset;
                        g.DrawPath(borderPen, path);
                    }
                }
            }
        }

        private GraphicsPath CreateShapePath(RectangleF rect, BarcodeLogoShape shape)
        {
            var path = new GraphicsPath();
            if (shape == BarcodeLogoShape.Circle)
            {
                path.AddEllipse(rect);
            }
            else if (shape == BarcodeLogoShape.RoundedRectangle)
            {
                float radius = Math.Min(rect.Width, rect.Height) * 0.25f;
                float d = radius * 2f;
                path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                path.CloseFigure();
            }
            else
            {
                path.AddRectangle(rect);
            }
            return path;
        }

        private void Render1DBarcode(Graphics g, Rectangle destRect)
        {
            bool[] pattern = null;
            string displayPayload = _value;

            switch (_barcodeType)
            {
                case BarcodeType.Code128:
                    pattern = Barcode1DEncoder.EncodeCode128(_value);
                    break;
                case BarcodeType.Code39:
                    pattern = Barcode1DEncoder.EncodeCode39(_value);
                    break;
                case BarcodeType.EAN13:
                    pattern = Barcode1DEncoder.EncodeEAN13(_value, out displayPayload);
                    break;
            }

            if (pattern == null || pattern.Length == 0)
            {
                DrawPlaceholder(g, destRect, "Invalid 1D Barcode Data");
                return;
            }

            int textHeight = 0;
            if (_showText && !string.IsNullOrEmpty(displayPayload))
            {
                textHeight = (int)Math.Ceiling(g.MeasureString(displayPayload, _textFont).Height) + 4;
            }

            int availableWidth = Math.Max(1, destRect.Width - (_quietZone * 2));
            int availableHeight = Math.Max(1, destRect.Height - (_quietZone * 2) - textHeight);

            float barWidth = (float)availableWidth / pattern.Length;
            if (_keepAspectRatio)
            {
                // In keep aspect ratio, narrow the bar width to an integer pixel or neat float
                barWidth = Math.Max(1f, (float)Math.Floor(barWidth));
            }

            float totalBarcodeWidth = barWidth * pattern.Length;
            float startX = destRect.X + _quietZone + Math.Max(0, (availableWidth - totalBarcodeWidth) / 2f);
            float startY = destRect.Y + _quietZone;

            // Draw bars
            using (var barBrush = new SolidBrush(_foreColor))
            {
                for (int i = 0; i < pattern.Length; i++)
                {
                    if (pattern[i])
                    {
                        g.FillRectangle(barBrush,
                            (float)Math.Round(startX + (i * barWidth)),
                            startY,
                            (float)Math.Ceiling(barWidth),
                            availableHeight);
                    }
                }
            }

            // Draw human readable text below barcode
            if (_showText && !string.IsNullOrEmpty(displayPayload))
            {
                var textRect = new RectangleF(
                    destRect.X + _quietZone,
                    startY + availableHeight + 2,
                    availableWidth,
                    textHeight);

                using (var textBrush = new SolidBrush(_foreColor))
                using (var sf = new StringFormat
                {
                    Alignment = _textAlignment,
                    LineAlignment = StringAlignment.Center
                })
                {
                    g.DrawString(displayPayload, _textFont, textBrush, textRect, sf);
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Generates a <see cref="Bitmap"/> of the barcode at the specified dimensions.
        /// </summary>
        /// <param name="width">Output image width.</param>
        /// <param name="height">Output image height.</param>
        /// <returns>A new <see cref="Bitmap"/> rendering the barcode.</returns>
        public Bitmap GenerateImage(int width, int height)
        {
            if (width <= 0) width = Math.Max(100, Width);
            if (height <= 0) height = Math.Max(100, Height);

            var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                RenderBarcode(g, new Rectangle(0, 0, width, height));
            }
            return bmp;
        }

        /// <summary>
        /// Saves the barcode image directly to a file.
        /// </summary>
        /// <param name="filePath">Target file path.</param>
        /// <param name="format">Image format (PNG, JPEG, BMP, etc.).</param>
        public void SaveImage(string filePath, ImageFormat format = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (format == null) format = ImageFormat.Png;

            using (Bitmap bmp = GenerateImage(Width, Height))
            {
                bmp.Save(filePath, format);
            }
        }

        /// <summary>
        /// Copies the generated barcode image to the clipboard.
        /// </summary>
        public void CopyToClipboard()
        {
            using (Bitmap bmp = GenerateImage(Width, Height))
            {
                try
                {
                    if (System.Threading.Thread.CurrentThread.GetApartmentState() == System.Threading.ApartmentState.STA)
                    {
                        Clipboard.SetImage(bmp);
                    }
                    else
                    {
                        var thread = new System.Threading.Thread(() =>
                        {
                            try
                            {
                                Clipboard.SetImage(bmp);
                            }
                            catch { }
                        });
                        thread.SetApartmentState(System.Threading.ApartmentState.STA);
                        thread.Start();
                        thread.Join(1000);
                    }
                }
                catch { }
            }
        }

        #endregion

        #region Internal Barcode 1D & QR Code Generators

        /// <summary>
        /// 1D Barcode Encoders for Code 128, Code 39, and EAN-13.
        /// </summary>
        internal static class Barcode1DEncoder
        {
            #region Code 128

            private static readonly string[] Code128Patterns = new string[]
            {
                "212222", "222122", "222221", "121223", "121322", "131222", "122213", "122312", "132212", "221213",
                "221312", "231212", "112232", "122132", "122231", "113222", "123122", "123221", "223211", "221132",
                "221231", "213212", "223112", "312131", "311222", "321122", "321221", "312212", "322112", "322211",
                "212123", "212321", "232121", "111323", "131123", "131321", "112313", "132113", "132311", "211313",
                "231113", "231311", "112133", "112331", "132131", "113123", "113321", "133121", "313121", "211331",
                "231131", "213113", "213311", "213131", "311123", "311321", "331121", "312113", "312311", "332111",
                "314111", "221411", "431111", "111224", "111422", "121124", "121421", "141122", "141221", "112214",
                "112412", "122114", "122411", "142112", "142211", "241211", "221114", "413111", "241112", "134111",
                "111242", "121142", "121241", "114212", "124112", "124211", "411212", "421112", "421211", "212141",
                "214121", "412121", "111143", "111341", "131141", "114113", "114311", "411113", "411311", "113141",
                "114131", "311141", "411131", "211412", "211214", "211232", "2331112"
            };

            public static bool[] EncodeCode128(string text)
            {
                if (string.IsNullOrEmpty(text)) return null;

                var indices = new List<int>();
                // Use Code Set B by default (Start Code B = 104)
                indices.Add(104);

                for (int i = 0; i < text.Length; i++)
                {
                    char c = text[i];
                    int code = c - 32;
                    if (code < 0 || code > 95) code = 0; // fallback to space
                    indices.Add(code);
                }

                // Checksum calculation (mod 103)
                int checksum = indices[0];
                for (int i = 1; i < indices.Count; i++)
                {
                    checksum += indices[i] * i;
                }
                indices.Add(checksum % 103);
                indices.Add(106); // Stop code

                var bits = new List<bool>();
                foreach (int idx in indices)
                {
                    string pattern = Code128Patterns[idx];
                    bool isBar = true;
                    foreach (char ch in pattern)
                    {
                        int width = ch - '0';
                        for (int w = 0; w < width; w++)
                        {
                            bits.Add(isBar);
                        }
                        isBar = !isBar;
                    }
                }

                return bits.ToArray();
            }

            #endregion

            #region Code 39

            private static readonly Dictionary<char, string> Code39Patterns = new Dictionary<char, string>
            {
                {'0', "000110100"}, {'1', "100100001"}, {'2', "001100001"}, {'3', "101100000"},
                {'4', "000110001"}, {'5', "100110000"}, {'6', "001110000"}, {'7', "000100101"},
                {'8', "100100100"}, {'9', "001100100"}, {'A', "100001001"}, {'B', "001001001"},
                {'C', "101001000"}, {'D', "000011001"}, {'E', "100011000"}, {'F', "001011000"},
                {'G', "000001101"}, {'H', "100001100"}, {'I', "001001100"}, {'J', "000011100"},
                {'K', "100000011"}, {'L', "001000011"}, {'M', "101000010"}, {'N', "000010011"},
                {'O', "100010010"}, {'P', "001010010"}, {'Q', "000000111"}, {'R', "100000110"},
                {'S', "001000110"}, {'T', "000010110"}, {'U', "110000001"}, {'V', "011000001"},
                {'W', "111000000"}, {'X', "010010001"}, {'Y', "110010000"}, {'Z', "011010000"},
                {'-', "010000101"}, {'.', "110000100"}, {' ', "011000100"}, {'$', "010101000"},
                {'/', "010100010"}, {'+', "010001010"}, {'%', "000101010"}, {'*', "010010100"}
            };

            public static bool[] EncodeCode39(string text)
            {
                if (string.IsNullOrEmpty(text)) return null;

                string upper = "*" + text.ToUpperInvariant() + "*";
                var bits = new List<bool>();

                for (int i = 0; i < upper.Length; i++)
                {
                    char c = upper[i];
                    if (!Code39Patterns.TryGetValue(c, out string pattern))
                    {
                        pattern = Code39Patterns[' ']; // fallback
                    }

                    for (int p = 0; p < 9; p++)
                    {
                        bool isBar = (p % 2 == 0);
                        int width = pattern[p] == '1' ? 3 : 1;
                        for (int w = 0; w < width; w++)
                        {
                            bits.Add(isBar);
                        }
                    }

                    // Inter-character gap (1 narrow space)
                    bits.Add(false);
                }

                return bits.ToArray();
            }

            #endregion

            #region EAN-13

            private static readonly string[] EAN_L = new string[]
            {
                "0001101", "0011001", "0010011", "0111101", "0100011",
                "0110001", "0101111", "0111011", "0110111", "0001011"
            };

            private static readonly string[] EAN_G = new string[]
            {
                "0100111", "0110011", "0011011", "0100001", "0011101",
                "0111001", "0000101", "0010001", "0001001", "0010111"
            };

            private static readonly string[] EAN_R = new string[]
            {
                "1110010", "1100110", "1101100", "1000010", "1011100",
                "1001110", "1010000", "1000100", "1001000", "1110100"
            };

            private static readonly string[] EAN_Parity = new string[]
            {
                "LLLLLL", "LLGLGG", "LLGGLG", "LLGGGL", "LGLLGG",
                "LGGLLG", "LGGGLL", "LGLGLG", "LGLGGL", "LGGLGL"
            };

            public static bool[] EncodeEAN13(string text, out string formatted)
            {
                formatted = string.Empty;
                if (string.IsNullOrEmpty(text)) return null;

                // Extract only digits
                var sb = new StringBuilder();
                foreach (char c in text)
                {
                    if (char.IsDigit(c)) sb.Append(c);
                }

                if (sb.Length < 12)
                {
                    // Pad with leading zeros up to 12 digits
                    sb.Insert(0, new string('0', 12 - sb.Length));
                }
                else if (sb.Length > 13)
                {
                    sb.Length = 12;
                }

                string digits12 = sb.ToString().Substring(0, 12);
                int checksum = CalculateEANChecksum(digits12);
                string fullEAN = digits12 + checksum.ToString();
                formatted = fullEAN;

                var bits = new List<bool>();

                // Quiet zone and Start guard "101"
                bits.Add(true); bits.Add(false); bits.Add(true);

                int firstDigit = fullEAN[0] - '0';
                string parity = EAN_Parity[firstDigit];

                // Left 6 digits
                for (int i = 0; i < 6; i++)
                {
                    int digit = fullEAN[i + 1] - '0';
                    bool useG = parity[i] == 'G';
                    string pattern = useG ? EAN_G[digit] : EAN_L[digit];
                    foreach (char bit in pattern)
                    {
                        bits.Add(bit == '1');
                    }
                }

                // Center guard "01010"
                bits.Add(false); bits.Add(true); bits.Add(false); bits.Add(true); bits.Add(false);

                // Right 6 digits
                for (int i = 6; i < 12; i++)
                {
                    int digit = fullEAN[i + 1] - '0';
                    string pattern = EAN_R[digit];
                    foreach (char bit in pattern)
                    {
                        bits.Add(bit == '1');
                    }
                }

                // End guard "101"
                bits.Add(true); bits.Add(false); bits.Add(true);

                return bits.ToArray();
            }

            private static int CalculateEANChecksum(string digits12)
            {
                int sum = 0;
                for (int i = 0; i < 12; i++)
                {
                    int val = digits12[i] - '0';
                    sum += (i % 2 == 0) ? val : (val * 3);
                }
                int mod = sum % 10;
                return (mod == 0) ? 0 : (10 - mod);
            }

            #endregion
        }

        /// <summary>
        /// Pure C# Standard QR Code Generator (ISO/IEC 18004 compatible).
        /// Supports Versions 1 to 40, Byte Mode, and Error Correction Levels (L, M, Q, H).
        /// </summary>
        internal static class QrCodeGenerator
        {
            public static bool[,] Generate(string text, QRCodeErrorCorrection ecc)
            {
                byte[] data = Encoding.UTF8.GetBytes(text ?? string.Empty);
                int eccIndex = (int)ecc; // 0=L, 1=M, 2=Q, 3=H

                // Select minimum version that fits data
                int version = SelectVersion(data.Length, eccIndex);
                if (version < 1) return null;

                int size = 17 + (4 * version);
                bool[,] modules = new bool[size, size];
                bool[,] isFunction = new bool[size, size];

                // 1. Draw Function Patterns (Finder, Timing, Alignment)
                DrawFunctionPatterns(modules, isFunction, version, size);

                // 2. Prepare Data and Error Correction Codewords
                byte[] codewords = PrepareCodewords(data, version, eccIndex);

                // 3. Place Data Modules in Matrix
                PlaceCodewords(modules, isFunction, codewords, size);

                // 4. Evaluate Masks and Apply Best Mask
                int bestMask = EvaluateBestMask(modules, isFunction, size);
                ApplyMask(modules, isFunction, bestMask, size);

                // 5. Draw Format Information (Mask + ECC)
                DrawFormatInfo(modules, eccIndex, bestMask, size);

                // 6. Draw Version Information (for Version >= 7)
                if (version >= 7)
                {
                    DrawVersionInfo(modules, version, size);
                }

                return modules;
            }

            private static int SelectVersion(int dataLen, int eccIndex)
            {
                for (int v = 1; v <= 40; v++)
                {
                    int totalDataCapacity = GetDataCapacity(v, eccIndex);
                    // Byte mode overhead: 4 bits mode + (v <= 9 ? 8 : 16) bits char count
                    int overheadBits = 4 + (v <= 9 ? 8 : 16);
                    int requiredBytes = (overheadBits + (dataLen * 8) + 7) / 8;
                    if (requiredBytes <= totalDataCapacity)
                    {
                        return v;
                    }
                }
                return -1; // Overflow
            }

            private static void DrawFunctionPatterns(bool[,] modules, bool[,] isFunction, int version, int size)
            {
                // Finder Patterns
                DrawFinder(modules, isFunction, 0, 0);
                DrawFinder(modules, isFunction, size - 7, 0);
                DrawFinder(modules, isFunction, 0, size - 7);

                // Timing Patterns
                for (int i = 8; i < size - 8; i++)
                {
                    bool bit = (i % 2 == 0);
                    modules[6, i] = bit;
                    isFunction[6, i] = true;
                    modules[i, 6] = bit;
                    isFunction[i, 6] = true;
                }

                // Dark Module
                modules[(4 * version) + 9, 8] = true;
                isFunction[(4 * version) + 9, 8] = true;

                // Alignment Patterns (Version >= 2)
                int[] alignCoords = GetAlignmentPatternPositions(version);
                for (int r = 0; r < alignCoords.Length; r++)
                {
                    for (int c = 0; c < alignCoords.Length; c++)
                    {
                        int row = alignCoords[r];
                        int col = alignCoords[c];
                        // Skip if overlapping finder patterns
                        if ((row == 6 && col == 6) ||
                            (row == 6 && col == size - 7) ||
                            (row == size - 7 && col == 6))
                            continue;

                        DrawAlignment(modules, isFunction, row - 2, col - 2);
                    }
                }

                // Reserve Format Information Areas
                for (int i = 0; i <= 8; i++)
                {
                    isFunction[8, i] = true;
                    isFunction[i, 8] = true;
                }
                for (int i = 0; i < 8; i++)
                {
                    isFunction[8, size - 1 - i] = true;
                    isFunction[size - 1 - i, 8] = true;
                }

                // Reserve Version Information Areas (Version >= 7)
                if (version >= 7)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            isFunction[size - 11 + j, i] = true;
                            isFunction[i, size - 11 + j] = true;
                        }
                    }
                }
            }

            private static void DrawFinder(bool[,] modules, bool[,] isFunction, int x, int y)
            {
                for (int dy = -1; dy <= 7; dy++)
                {
                    for (int dx = -1; dx <= 7; dx++)
                    {
                        int rx = x + dx;
                        int ry = y + dy;
                        if (rx >= 0 && rx < modules.GetLength(0) && ry >= 0 && ry < modules.GetLength(0))
                        {
                            isFunction[ry, rx] = true;
                            if (dx >= 0 && dx <= 6 && dy >= 0 && dy <= 6)
                            {
                                modules[ry, rx] = (dx == 0 || dx == 6 || dy == 0 || dy == 6 ||
                                                   (dx >= 2 && dx <= 4 && dy >= 2 && dy <= 4));
                            }
                            else
                            {
                                modules[ry, rx] = false; // separator
                            }
                        }
                    }
                }
            }

            private static void DrawAlignment(bool[,] modules, bool[,] isFunction, int x, int y)
            {
                for (int dy = 0; dy < 5; dy++)
                {
                    for (int dx = 0; dx < 5; dx++)
                    {
                        int rx = x + dx;
                        int ry = y + dy;
                        isFunction[ry, rx] = true;
                        modules[ry, rx] = (dx == 0 || dx == 4 || dy == 0 || dy == 4 || (dx == 2 && dy == 2));
                    }
                }
            }

            private static byte[] PrepareCodewords(byte[] data, int version, int eccIndex)
            {
                int totalDataCodewords = GetDataCapacity(version, eccIndex);
                var bb = new BitBuffer();

                // Byte mode indicator: 0100
                bb.AppendBits(0x4, 4);

                // Character count indicator
                int countBits = (version <= 9) ? 8 : 16;
                bb.AppendBits(data.Length, countBits);

                // Data bits
                for (int i = 0; i < data.Length; i++)
                {
                    bb.AppendBits(data[i], 8);
                }

                // Terminator (up to 4 bits of 0)
                int terminatorLen = Math.Min(4, (totalDataCodewords * 8) - bb.BitLength);
                if (terminatorLen > 0)
                {
                    bb.AppendBits(0, terminatorLen);
                }

                // Pad to byte boundary
                if (bb.BitLength % 8 != 0)
                {
                    bb.AppendBits(0, 8 - (bb.BitLength % 8));
                }

                // Pad bytes 0xEC and 0x11
                int padCount = totalDataCodewords - (bb.BitLength / 8);
                byte[] padBytes = { 0xEC, 0x11 };
                for (int i = 0; i < padCount; i++)
                {
                    bb.AppendBits(padBytes[i % 2], 8);
                }

                byte[] dataBytes = bb.ToByteArray();

                // Generate Reed-Solomon Error Correction Blocks
                return InterleaveBlocks(dataBytes, version, eccIndex);
            }

            private static byte[] InterleaveBlocks(byte[] data, int version, int eccIndex)
            {
                int numBlocks = GetNumBlocks(version, eccIndex);
                int totalEccCodewords = GetTotalEccCodewords(version, eccIndex);
                int eccPerBlock = totalEccCodewords / numBlocks;

                int numShortBlocks = numBlocks - (data.Length % numBlocks);
                int shortBlockLen = data.Length / numBlocks;

                byte[][] dataBlocks = new byte[numBlocks][];
                byte[][] eccBlocks = new byte[numBlocks][];

                int dataOffset = 0;
                for (int b = 0; b < numBlocks; b++)
                {
                    int blockLen = (b < numShortBlocks) ? shortBlockLen : (shortBlockLen + 1);
                    dataBlocks[b] = new byte[blockLen];
                    Array.Copy(data, dataOffset, dataBlocks[b], 0, blockLen);
                    dataOffset += blockLen;

                    eccBlocks[b] = ReedSolomonCompute(dataBlocks[b], eccPerBlock);
                }

                // Interleave data and ecc
                byte[] interleaved = new byte[data.Length + totalEccCodewords];
                int idx = 0;

                int maxDataLen = shortBlockLen + 1;
                for (int i = 0; i < maxDataLen; i++)
                {
                    for (int b = 0; b < numBlocks; b++)
                    {
                        if (i < dataBlocks[b].Length)
                        {
                            interleaved[idx++] = dataBlocks[b][i];
                        }
                    }
                }

                for (int i = 0; i < eccPerBlock; i++)
                {
                    for (int b = 0; b < numBlocks; b++)
                    {
                        interleaved[idx++] = eccBlocks[b][i];
                    }
                }

                return interleaved;
            }

            private static byte[] ReedSolomonCompute(byte[] data, int eccCount)
            {
                byte[] generator = ReedSolomonGenerator(eccCount);
                byte[] result = new byte[eccCount];

                for (int i = 0; i < data.Length; i++)
                {
                    byte factor = (byte)(data[i] ^ result[0]);
                    Array.Copy(result, 1, result, 0, eccCount - 1);
                    result[eccCount - 1] = 0;

                    for (int j = 0; j < eccCount; j++)
                    {
                        result[j] ^= GF_Multiply(generator[j], factor);
                    }
                }
                return result;
            }

            private static byte[] ReedSolomonGenerator(int degree)
            {
                byte[] poly = new byte[degree];
                poly[degree - 1] = 1;
                int root = 1;

                for (int i = 0; i < degree; i++)
                {
                    for (int j = 0; j < degree; j++)
                    {
                        poly[j] = GF_Multiply(poly[j], (byte)root);
                        if (j + 1 < degree)
                        {
                            poly[j] ^= poly[j + 1];
                        }
                    }
                    root = GF_Multiply((byte)root, 2);
                }
                return poly;
            }

            private static byte GF_Multiply(byte a, byte b)
            {
                int res = 0;
                int x = a, y = b;
                while (y > 0)
                {
                    if ((y & 1) != 0) res ^= x;
                    x <<= 1;
                    if ((x & 0x100) != 0) x ^= 0x11D; // standard QR GF(256) polynomial
                    y >>= 1;
                }
                return (byte)res;
            }

            private static void PlaceCodewords(bool[,] modules, bool[,] isFunction, byte[] codewords, int size)
            {
                int bitIndex = 0;
                int totalBits = codewords.Length * 8;

                int right = size - 1;
                bool upwards = true;

                while (right > 0)
                {
                    if (right == 6) right--; // Skip vertical timing column

                    for (int vert = 0; vert < size; vert++)
                    {
                        int y = upwards ? (size - 1 - vert) : vert;
                        for (int colOffset = 0; colOffset < 2; colOffset++)
                        {
                            int x = right - colOffset;
                            if (!isFunction[y, x])
                            {
                                bool bit = false;
                                if (bitIndex < totalBits)
                                {
                                    int byteIdx = bitIndex / 8;
                                    int bitPos = 7 - (bitIndex % 8);
                                    bit = ((codewords[byteIdx] >> bitPos) & 1) != 0;
                                    bitIndex++;
                                }
                                modules[y, x] = bit;
                            }
                        }
                    }
                    right -= 2;
                    upwards = !upwards;
                }
            }

            private static bool MaskCondition(int mask, int x, int y)
            {
                switch (mask)
                {
                    case 0: return (x + y) % 2 == 0;
                    case 1: return y % 2 == 0;
                    case 2: return x % 3 == 0;
                    case 3: return (x + y) % 3 == 0;
                    case 4: return ((y / 2) + (x / 3)) % 2 == 0;
                    case 5: return ((x * y) % 2) + ((x * y) % 3) == 0;
                    case 6: return (((x * y) % 2) + ((x * y) % 3)) % 2 == 0;
                    case 7: return (((x + y) % 2) + ((x * y) % 3)) % 2 == 0;
                    default: return false;
                }
            }

            private static void ApplyMask(bool[,] modules, bool[,] isFunction, int mask, int size)
            {
                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        if (!isFunction[y, x] && MaskCondition(mask, x, y))
                        {
                            modules[y, x] = !modules[y, x];
                        }
                    }
                }
            }

            private static int EvaluateBestMask(bool[,] modules, bool[,] isFunction, int size)
            {
                int bestMask = 0;
                int minPenalty = int.MaxValue;

                for (int m = 0; m < 8; m++)
                {
                    ApplyMask(modules, isFunction, m, size);
                    int penalty = ComputePenalty(modules, size);
                    ApplyMask(modules, isFunction, m, size); // unmask

                    if (penalty < minPenalty)
                    {
                        minPenalty = penalty;
                        bestMask = m;
                    }
                }
                return bestMask;
            }

            private static int ComputePenalty(bool[,] modules, int size)
            {
                int penalty = 0;

                // Rule 1: 5 or more consecutive same color modules
                for (int r = 0; r < size; r++)
                {
                    int run = 0;
                    bool last = false;
                    for (int c = 0; c < size; c++)
                    {
                        if (c == 0 || modules[r, c] != last)
                        {
                            last = modules[r, c];
                            run = 1;
                        }
                        else
                        {
                            run++;
                            if (run == 5) penalty += 3;
                            else if (run > 5) penalty++;
                        }
                    }
                }

                // Rule 2: 2x2 blocks of same color
                for (int r = 0; r < size - 1; r++)
                {
                    for (int c = 0; c < size - 1; c++)
                    {
                        bool val = modules[r, c];
                        if (val == modules[r, c + 1] && val == modules[r + 1, c] && val == modules[r + 1, c + 1])
                        {
                            penalty += 3;
                        }
                    }
                }

                return penalty;
            }

            private static void DrawFormatInfo(bool[,] modules, int eccIndex, int mask, int size)
            {
                // Format bits: ECC (2 bits) + Mask (3 bits) = 5 bits -> BCH code (15, 5)
                // ECC bits mapping: L=1, M=0, Q=3, H=2
                int[] eccBits = { 1, 0, 3, 2 };
                int data = (eccBits[eccIndex] << 3) | mask;
                int rem = data << 10;
                for (int i = 4; i >= 0; i--)
                {
                    if (((rem >> (i + 10)) & 1) != 0)
                    {
                        rem ^= 0x537 << i;
                    }
                }
                int format = ((data << 10) | rem) ^ 0x5412; // Mask with 0x5412

                // Top-left placement
                for (int i = 0; i < 6; i++) modules[8, i] = ((format >> i) & 1) != 0;
                modules[8, 7] = ((format >> 6) & 1) != 0;
                modules[8, 8] = ((format >> 7) & 1) != 0;
                modules[7, 8] = ((format >> 8) & 1) != 0;
                for (int i = 9; i < 15; i++) modules[14 - i, 8] = ((format >> i) & 1) != 0;

                // Split placement (bottom-left and top-right)
                for (int i = 0; i < 7; i++) modules[size - 1 - i, 8] = ((format >> i) & 1) != 0;
                for (int i = 7; i < 15; i++) modules[8, size - 15 + i] = ((format >> i) & 1) != 0;
            }

            private static void DrawVersionInfo(bool[,] modules, int version, int size)
            {
                int rem = version << 12;
                for (int i = 5; i >= 0; i--)
                {
                    if (((rem >> (i + 12)) & 1) != 0)
                    {
                        rem ^= 0x1F25 << i;
                    }
                }
                int versionBits = (version << 12) | rem;

                for (int i = 0; i < 18; i++)
                {
                    bool bit = ((versionBits >> i) & 1) != 0;
                    int a = size - 11 + (i % 3);
                    int b = i / 3;
                    modules[a, b] = bit;
                    modules[b, a] = bit;
                }
            }

            private static int GetDataCapacity(int version, int eccIndex)
            {
                // Total codewords for version minus ECC codewords
                int totalCodewords = GetTotalCodewords(version);
                int eccCodewords = GetTotalEccCodewords(version, eccIndex);
                return totalCodewords - eccCodewords;
            }

            private static int GetTotalCodewords(int version)
            {
                int[] total = {
                    0, 26, 44, 70, 100, 134, 172, 196, 242, 292, 346,
                    404, 466, 532, 581, 655, 733, 815, 901, 991, 1085,
                    1156, 1258, 1364, 1474, 1588, 1706, 1828, 1921, 2051, 2185,
                    2323, 2465, 2611, 2761, 2876, 3034, 3196, 3362, 3532, 3706
                };
                return total[version];
            }

            private static int GetTotalEccCodewords(int version, int eccIndex)
            {
                // [version, ecc (L=0, M=1, Q=2, H=3)]
                int[,] eccTable = {
                    {0,0,0,0}, {7,10,13,17}, {10,16,22,28}, {15,26,36,44}, {20,36,52,64},
                    {26,48,72,88}, {36,64,96,112}, {40,72,108,130}, {48,88,132,156}, {60,110,160,192},
                    {72,130,192,224}, {80,150,224,264}, {96,176,260,308}, {104,198,288,352}, {120,216,320,384},
                    {132,240,360,432}, {144,280,408,480}, {168,308,448,532}, {180,338,504,588}, {196,364,546,650},
                    {224,416,600,700}, {224,442,644,750}, {252,476,690,816}, {270,504,750,900}, {300,560,810,960},
                    {312,588,870,1050}, {336,644,952,1110}, {360,700,1020,1200}, {390,728,1050,1260}, {420,784,1140,1350},
                    {450,812,1200,1440}, {480,868,1290,1530}, {510,924,1350,1620}, {540,980,1440,1710}, {570,1036,1530,1800},
                    {570,1064,1590,1890}, {600,1120,1680,1980}, {630,1204,1770,2100}, {660,1260,1860,2220}, {720,1316,1950,2310},
                    {750,1372,2040,2430}
                };
                return eccTable[version, eccIndex];
            }

            private static int GetNumBlocks(int version, int eccIndex)
            {
                int[,] blockTable = {
                    {0,0,0,0}, {1,1,1,1}, {1,1,1,1}, {1,1,2,2}, {1,2,2,4},
                    {1,2,4,4}, {2,4,4,4}, {2,4,6,5}, {2,4,6,6}, {2,5,8,8},
                    {4,5,8,8}, {4,5,8,11}, {4,8,10,11}, {4,9,12,16}, {4,9,16,16},
                    {6,10,12,18}, {6,10,17,16}, {6,11,16,19}, {6,13,18,21}, {7,14,21,25},
                    {8,16,20,25}, {8,17,23,25}, {9,17,23,34}, {9,18,25,30}, {10,20,27,32},
                    {12,21,29,35}, {12,23,34,37}, {12,25,34,40}, {13,26,35,42}, {14,28,38,45},
                    {15,29,40,48}, {16,31,43,51}, {17,33,45,54}, {18,35,48,57}, {19,37,51,60},
                    {19,38,53,63}, {20,40,56,66}, {21,43,59,70}, {22,45,62,74}, {24,47,65,77},
                    {25,49,68,81}
                };
                return blockTable[version, eccIndex];
            }

            private static int[] GetAlignmentPatternPositions(int version)
            {
                if (version <= 1) return new int[0];
                int count = (version / 7) + 2;
                int[] coords = new int[count];
                coords[0] = 6;
                int size = 17 + (4 * version);
                coords[count - 1] = size - 7;

                if (count > 2)
                {
                    int step = (size - 13 + count - 2) / (count - 1);
                    step = (step + 1) / 2 * 2; // round up to even
                    for (int i = count - 2; i > 0; i--)
                    {
                        coords[i] = coords[i + 1] - step;
                    }
                }
                return coords;
            }

            private class BitBuffer
            {
                private readonly List<byte> _bytes = new List<byte>();
                public int BitLength { get; private set; } = 0;

                public void AppendBits(int value, int count)
                {
                    for (int i = count - 1; i >= 0; i--)
                    {
                        int bit = (value >> i) & 1;
                        if (BitLength % 8 == 0)
                        {
                            _bytes.Add(0);
                        }
                        if (bit != 0)
                        {
                            _bytes[_bytes.Count - 1] |= (byte)(1 << (7 - (BitLength % 8)));
                        }
                        BitLength++;
                    }
                }

                public byte[] ToByteArray()
                {
                    return _bytes.ToArray();
                }
            }
        }

        #endregion
    }
}
