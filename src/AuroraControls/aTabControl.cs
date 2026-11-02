using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace AuroraControls
{
    /// <summary>
    /// Custom TabControl with customizable header font, tab colors, and container background color.
    /// </summary>
    [ToolboxBitmap(typeof(TabControl))]
    public class ATabControl : TabControl
    {
        private Font _headerFont;
        private Color _activeTabColor = Color.White;
        private Color _activeTextColor = Color.FromArgb(0, 120, 215);
        private Color _inactiveTabColor = Color.FromArgb(240, 240, 240);
        private Color _inactiveTextColor = Color.FromArgb(100, 100, 100);
        private Color _hoverTabColor = Color.FromArgb(230, 230, 230);
        private Color _hoverTextColor = Color.FromArgb(40, 40, 40);
        private Color _containerColor = Color.White;
        private Color _borderColor = Color.FromArgb(218, 220, 224);
        private Color _indicatorColor = Color.FromArgb(0, 120, 215);
        private bool _showIndicator = true;
        private int _indicatorThickness = 3;
        private bool _autoSyncPageBackColor = true;
        private int _hoveredIndex = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="ATabControl"/> class.
        /// </summary>
        public ATabControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            DrawMode = TabDrawMode.OwnerDrawFixed;
            ItemSize = new Size(120, 36);
            SizeMode = TabSizeMode.Fixed;
            UpdateTabPagesColor();
        }

        /// <summary>
        /// Gets or sets the font used for the tab headers.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Font used for drawing tab header text.")]
        public Font HeaderFont
        {
            get => _headerFont ?? Font;
            set
            {
                _headerFont = value;
                if (value != null && ItemSize.Height < value.Height + 14)
                {
                    ItemSize = new Size(ItemSize.Width, value.Height + 14);
                }
                Invalidate();
            }
        }

        private bool ShouldSerializeHeaderFont() => _headerFont != null;
        private void ResetHeaderFont() => _headerFont = null;

        /// <summary>
        /// Gets or sets the background color of the active tab.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Background color of the currently selected tab header.")]
        [DefaultValue(typeof(Color), "White")]
        public Color ActiveTabColor
        {
            get => _activeTabColor;
            set { _activeTabColor = value; Invalidate(); }
        }

        /// <summary>
        /// Gets or sets the text color of the active tab.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Text color of the currently selected tab header.")]
        public Color ActiveTextColor
        {
            get => _activeTextColor;
            set { _activeTextColor = value; Invalidate(); }
        }

        private bool ShouldSerializeActiveTextColor() => _activeTextColor != Color.FromArgb(0, 120, 215);
        private void ResetActiveTextColor() => _activeTextColor = Color.FromArgb(0, 120, 215);

        /// <summary>
        /// Gets or sets the background color of inactive tabs.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Background color of inactive tab headers.")]
        public Color InactiveTabColor
        {
            get => _inactiveTabColor;
            set { _inactiveTabColor = value; Invalidate(); }
        }

        private bool ShouldSerializeInactiveTabColor() => _inactiveTabColor != Color.FromArgb(240, 240, 240);
        private void ResetInactiveTabColor() => _inactiveTabColor = Color.FromArgb(240, 240, 240);

        /// <summary>
        /// Gets or sets the text color of inactive tabs.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Text color of inactive tab headers.")]
        public Color InactiveTextColor
        {
            get => _inactiveTextColor;
            set { _inactiveTextColor = value; Invalidate(); }
        }

        private bool ShouldSerializeInactiveTextColor() => _inactiveTextColor != Color.FromArgb(100, 100, 100);
        private void ResetInactiveTextColor() => _inactiveTextColor = Color.FromArgb(100, 100, 100);

        /// <summary>
        /// Gets or sets the background color when hovering over an unselected tab.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Background color of tab header when mouse hovers over it.")]
        public Color HoverTabColor
        {
            get => _hoverTabColor;
            set { _hoverTabColor = value; Invalidate(); }
        }

        private bool ShouldSerializeHoverTabColor() => _hoverTabColor != Color.FromArgb(230, 230, 230);
        private void ResetHoverTabColor() => _hoverTabColor = Color.FromArgb(230, 230, 230);

        /// <summary>
        /// Gets or sets the text color when hovering over an unselected tab.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Text color of tab header when mouse hovers over it.")]
        public Color HoverTextColor
        {
            get => _hoverTextColor;
            set { _hoverTextColor = value; Invalidate(); }
        }

        private bool ShouldSerializeHoverTextColor() => _hoverTextColor != Color.FromArgb(40, 40, 40);
        private void ResetHoverTextColor() => _hoverTextColor = Color.FromArgb(40, 40, 40);

        /// <summary>
        /// Gets or sets the background color of the tab container.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Background color of the container area behind tabs and inside tab pages.")]
        [DefaultValue(typeof(Color), "White")]
        public Color ContainerColor
        {
            get => _containerColor;
            set
            {
                _containerColor = value;
                UpdateTabPagesColor();
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the border color of tabs and container edges.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Border line color for tabs and container edges.")]
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        private bool ShouldSerializeBorderColor() => _borderColor != Color.FromArgb(218, 220, 224);
        private void ResetBorderColor() => _borderColor = Color.FromArgb(218, 220, 224);

        /// <summary>
        /// Gets or sets the color of the active tab selection indicator bar.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Color of the accent indicator bar on the active tab.")]
        public Color IndicatorColor
        {
            get => _indicatorColor;
            set { _indicatorColor = value; Invalidate(); }
        }

        private bool ShouldSerializeIndicatorColor() => _indicatorColor != Color.FromArgb(0, 120, 215);
        private void ResetIndicatorColor() => _indicatorColor = Color.FromArgb(0, 120, 215);

        /// <summary>
        /// Gets or sets whether to display an indicator bar on the active tab.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Whether to show an accent line indicator on the selected tab.")]
        [DefaultValue(true)]
        public bool ShowIndicator
        {
            get => _showIndicator;
            set { _showIndicator = value; Invalidate(); }
        }

        /// <summary>
        /// Gets or sets the thickness of the indicator bar in pixels.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("Thickness of the selection indicator bar in pixels.")]
        [DefaultValue(3)]
        public int IndicatorThickness
        {
            get => _indicatorThickness;
            set { _indicatorThickness = Math.Max(1, value); Invalidate(); }
        }

        /// <summary>
        /// Gets or sets whether to automatically set TabPage BackColor to ContainerColor.
        /// </summary>
        [Browsable(true)]
        [Category("Extended Properties")]
        [Description("When true, automatically synchronizes TabPage BackColor with ContainerColor.")]
        [DefaultValue(true)]
        public bool AutoSyncPageBackColor
        {
            get => _autoSyncPageBackColor;
            set
            {
                _autoSyncPageBackColor = value;
                if (value) UpdateTabPagesColor();
            }
        }

        private void UpdateTabPagesColor()
        {
            if (!_autoSyncPageBackColor) return;
            foreach (TabPage page in TabPages)
            {
                if (page.BackColor != _containerColor)
                {
                    page.BackColor = _containerColor;
                }
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (e.Control is TabPage page && _autoSyncPageBackColor)
            {
                page.BackColor = _containerColor;
            }
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            UpdateTabPagesColor();
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int newHover = -1;
            for (int i = 0; i < TabCount; i++)
            {
                if (GetTabRect(i).Contains(e.Location))
                {
                    newHover = i;
                    break;
                }
            }

            if (newHover != _hoveredIndex)
            {
                _hoveredIndex = newHover;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_hoveredIndex != -1)
            {
                _hoveredIndex = -1;
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            for (int i = 0; i < TabCount; i++)
            {
                if (GetTabRect(i).Contains(e.Location))
                {
                    if (SelectedIndex != i)
                    {
                        SelectedIndex = i;
                    }
                    break;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // 1. Paint container background
            using (SolidBrush bgBrush = new SolidBrush(_containerColor))
            {
                g.FillRectangle(bgBrush, ClientRectangle);
            }

            // 2. Paint tab headers area background strip
            Rectangle stripRect = GetHeaderStripRectangle();
            using (SolidBrush stripBrush = new SolidBrush(_inactiveTabColor))
            {
                g.FillRectangle(stripBrush, stripRect);
            }

            // 3. Paint each tab header
            Font drawFont = HeaderFont;
            for (int i = 0; i < TabCount; i++)
            {
                Rectangle tabRect = GetTabRect(i);
                bool isSelected = (i == SelectedIndex);
                bool isHovered = (i == _hoveredIndex && !isSelected);

                Color tabBg = isSelected ? _activeTabColor : (isHovered ? _hoverTabColor : _inactiveTabColor);
                Color textClr = isSelected ? _activeTextColor : (isHovered ? _hoverTextColor : _inactiveTextColor);

                using (SolidBrush tabBrush = new SolidBrush(tabBg))
                {
                    g.FillRectangle(tabBrush, tabRect);
                }

                // Tab Border
                using (Pen borderPen = new Pen(_borderColor))
                {
                    g.DrawRectangle(borderPen, tabRect);
                }

                // Active Tab Indicator
                if (isSelected && _showIndicator)
                {
                    Rectangle indicatorRect = GetIndicatorRectangle(tabRect);
                    using (SolidBrush indBrush = new SolidBrush(_indicatorColor))
                    {
                        g.FillRectangle(indBrush, indicatorRect);
                    }
                }

                // Draw Icon if ImageList available
                int iconWidth = 0;
                TabPage page = TabPages[i];
                Image img = GetTabImage(page);
                if (img != null)
                {
                    int imgY = tabRect.Y + (tabRect.Height - img.Height) / 2;
                    int imgX = tabRect.X + 8;
                    g.DrawImage(img, imgX, imgY, img.Width, img.Height);
                    iconWidth = img.Width + 6;
                }

                // Draw Text
                Rectangle textRect = new Rectangle(
                    tabRect.X + iconWidth + 4,
                    tabRect.Y,
                    tabRect.Width - iconWidth - 8,
                    tabRect.Height
                );

                using (SolidBrush textBrush = new SolidBrush(textClr))
                using (StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center,
                    Trimming = StringTrimming.EllipsisCharacter,
                    FormatFlags = StringFormatFlags.NoWrap
                })
                {
                    g.DrawString(page.Text, drawFont, textBrush, textRect, sf);
                }
            }

            // 4. Paint border around container/pages
            using (Pen borderPen = new Pen(_borderColor))
            {
                Rectangle displayRect = DisplayRectangle;
                displayRect.Inflate(1, 1);
                g.DrawRectangle(borderPen, displayRect);
            }
        }

        private Rectangle GetHeaderStripRectangle()
        {
            if (TabCount == 0) return new Rectangle(0, 0, Width, ItemSize.Height);

            Rectangle firstTab = GetTabRect(0);
            switch (Alignment)
            {
                case TabAlignment.Bottom:
                    return new Rectangle(0, Height - ItemSize.Height, Width, ItemSize.Height);
                case TabAlignment.Left:
                    return new Rectangle(0, 0, ItemSize.Height, Height);
                case TabAlignment.Right:
                    return new Rectangle(Width - ItemSize.Height, 0, ItemSize.Height, Height);
                case TabAlignment.Top:
                default:
                    return new Rectangle(0, 0, Width, firstTab.Height > 0 ? firstTab.Height + 2 : ItemSize.Height);
            }
        }

        private Rectangle GetIndicatorRectangle(Rectangle tabRect)
        {
            switch (Alignment)
            {
                case TabAlignment.Bottom:
                    return new Rectangle(tabRect.X, tabRect.Y, tabRect.Width, _indicatorThickness);
                case TabAlignment.Left:
                    return new Rectangle(tabRect.Right - _indicatorThickness, tabRect.Y, _indicatorThickness, tabRect.Height);
                case TabAlignment.Right:
                    return new Rectangle(tabRect.X, tabRect.Y, _indicatorThickness, tabRect.Height);
                case TabAlignment.Top:
                default:
                    return new Rectangle(tabRect.X, tabRect.Bottom - _indicatorThickness, tabRect.Width, _indicatorThickness);
            }
        }

        private Image GetTabImage(TabPage page)
        {
            if (ImageList == null) return null;
            if (page.ImageIndex >= 0 && page.ImageIndex < ImageList.Images.Count)
            {
                return ImageList.Images[page.ImageIndex];
            }
            if (!string.IsNullOrEmpty(page.ImageKey) && ImageList.Images.ContainsKey(page.ImageKey))
            {
                return ImageList.Images[page.ImageKey];
            }
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _headerFont?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
