using System;
using System.Drawing;
using System.Globalization;
using AuroraControls;
using Xunit;

namespace AuroraControls.Tests
{
    public class ControlTests
    {
        [Fact]
        public void AButton_DefaultAndCustomColors_ApplyCorrectly()
        {
            using var btn = new AButton();
            Assert.Equal(Color.Gray, btn.BackColor);
            Assert.Equal(Color.DarkGray, btn.ForeColor);

            btn.UnfocusedColor = Color.Blue;
            btn.UnfocusedTextColor = Color.Yellow;
            Assert.Equal(Color.Blue, btn.BackColor);
            Assert.Equal(Color.Yellow, btn.ForeColor);
        }

        [Fact]
        public void ACheckBox_CheckStateChange_UpdatesColors()
        {
            using var chk = new ACheckBox();
            chk.CheckColor = Color.LightGreen;
            chk.UnCheckColor = Color.LightCoral;

            chk.Checked = false;
            Assert.Equal(Color.LightCoral, chk.BackColor);

            chk.Checked = true;
            Assert.Equal(Color.LightGreen, chk.BackColor);
        }

        [Fact]
        public void ATextBox_MandatoryAndValidity_BehaveAsExpected()
        {
            using var txt = new ATextBox();
            Assert.True(txt.IsValid);

            txt.Mandatory = true;
            txt.Text = string.Empty;
            Assert.False(txt.IsValid);
            Assert.Equal(txt.MandatoryColor, txt.BackColor);

            txt.Text = "Valid Input";
            Assert.True(txt.IsValid);
            Assert.Equal(txt.UnfocusedColor, txt.BackColor);
        }

        [Fact]
        public void ATextBox_TextCasing_FormatsCorrectly()
        {
            using var txt = new ATextBox();

            txt.TextCasing = TextCasing.UpperCase;
            txt.Text = "hello world";
            Assert.Equal("HELLO WORLD", txt.Text);

            txt.TextCasing = TextCasing.LowerCase;
            txt.Text = "HELLO WORLD";
            Assert.Equal("hello world", txt.Text);

            txt.TextCasing = TextCasing.CapitalizeEachWord;
            txt.Text = "hello world";
            Assert.Equal("Hello World", txt.Text);

            txt.TextCasing = TextCasing.SentenceCase;
            txt.Text = "hello world. this is a test! awesome?";
            Assert.Equal("Hello world. This is a test! Awesome?", txt.Text);
        }

        [Fact]
        public void ALabelTimer_FormatAndCulture_UpdateTextImmediately()
        {
            using var lbl = new ALabelTimer();
            lbl.FormatText = "yyyy";
            Assert.Equal(DateTime.Now.Year.ToString(), lbl.Text);

            lbl.Culture = new CultureInfo("fr-FR");
            lbl.FormatText = "MMMM";
            Assert.False(string.IsNullOrEmpty(lbl.Text));
        }

        [Fact]
        public void ATextBox_DebounceAndPlaceholder_PropertiesFunction()
        {
            using var txt = new ATextBox();
            txt.PlaceholderText = "Search...";
            Assert.Equal("Search...", txt.PlaceholderText);

            txt.DebounceDelay = 100;
            Assert.Equal(100, txt.DebounceDelay);

            bool debouncedFired = false;
            txt.DebouncedTextChanged += (s, e) => debouncedFired = true;

            txt.Text = "Query";
            txt.FlushDebounce();
            Assert.True(debouncedFired);
        }

        [Fact]
        public void AButton_BadgeLoadingAndProgress_PropertiesFunction()
        {
            using var btn = new AButton();
            btn.BadgeText = "99+";
            btn.BadgeBackColor = Color.Crimson;
            Assert.Equal("99+", btn.BadgeText);
            Assert.Equal(Color.Crimson, btn.BadgeBackColor);

            btn.ProgressValue = 50;
            Assert.Equal(50, btn.ProgressValue);

            btn.Text = "Submit";
            btn.IsLoading = true;
            Assert.True(btn.IsLoading);
            btn.IsLoading = false;
            Assert.False(btn.IsLoading);
        }

        [Fact]
        public void ACheckBox_StyleSwitch_PropertiesFunction()
        {
            using var chk = new ACheckBox();
            Assert.Equal(CheckBoxStyle.Standard, chk.Style);

            chk.Style = CheckBoxStyle.Switch;
            Assert.Equal(CheckBoxStyle.Switch, chk.Style);

            chk.SwitchOnColor = Color.SeaGreen;
            chk.SwitchOffColor = Color.LightSlateGray;
            Assert.Equal(Color.SeaGreen, chk.SwitchOnColor);
            Assert.Equal(Color.LightSlateGray, chk.SwitchOffColor);
        }

        [Fact]
        public void AMessageBoxOptions_DefaultValues_AreCorrect()
        {
            var options = new AMessageBoxOptions();
            Assert.Equal("Notification", options.Title);
            Assert.Equal(string.Empty, options.Message);
            Assert.Equal(System.Windows.Forms.MessageBoxButtons.OK, options.Buttons);
            Assert.Equal(AMessageBoxIcon.Info, options.Icon);
            Assert.Equal(Color.Empty, options.CardBackColor);
            Assert.True(options.ShowCloseButton);
            Assert.Equal(12, options.CornerRadius);
            Assert.Equal(AMessageBoxTheme.Light, options.Theme);
            Assert.False(options.ShowInput);
        }

        [Fact]
        public void AMessageBoxOptions_DarkModeAndInput_ConfiguresProperly()
        {
            var options = new AMessageBoxOptions
            {
                Theme = AMessageBoxTheme.Dark,
                ShowInput = true,
                DefaultInputText = "Initial Value",
                DetailsText = "Exception at System.Void()..."
            };

            Assert.Equal(AMessageBoxTheme.Dark, options.Theme);
            Assert.True(options.ShowInput);
            Assert.Equal("Initial Value", options.DefaultInputText);
            Assert.Equal("Exception at System.Void()...", options.DetailsText);
        }

        [Fact]
        public void AMessageBox_DefaultAccentColor_IsPurple()
        {
            Assert.Equal(Color.FromArgb(94, 44, 165), AMessageBox.DefaultAccentColor);
            AMessageBox.DefaultAccentColor = Color.Navy;
            Assert.Equal(Color.Navy, AMessageBox.DefaultAccentColor);
            AMessageBox.DefaultAccentColor = Color.FromArgb(94, 44, 165);
        }

        [Fact]
        public void ATabControl_PropertiesAndTabSync_WorkCorrectly()
        {
            using var tab = new ATabControl();
            using var page = new System.Windows.Forms.TabPage("Test Page");
            tab.TabPages.Add(page);

            Assert.Equal(Color.White, tab.ContainerColor);
            Assert.Equal(Color.White, page.BackColor);

            tab.ContainerColor = Color.FromArgb(30, 30, 30);
            Assert.Equal(Color.FromArgb(30, 30, 30), page.BackColor);

            using var font = new Font("Segoe UI", 12f, FontStyle.Bold);
            tab.HeaderFont = font;
            Assert.Equal(font, tab.HeaderFont);
            Assert.True(tab.ItemSize.Height >= font.Height);

            tab.ActiveTabColor = Color.Black;
            tab.ActiveTextColor = Color.Yellow;
            Assert.Equal(Color.Black, tab.ActiveTabColor);
            Assert.Equal(Color.Yellow, tab.ActiveTextColor);
        }

        [Fact]
        public void ALabelBox_HoverPropertiesAndStyle_WorkCorrectly()
        {
            using var lbl = new ALabelBox();
            lbl.Text = "https://github.com";
            lbl.HoverForeColor = Color.Blue;
            lbl.HoverBackColor = Color.LightYellow;
            lbl.HoverUnderline = true;

            Assert.Equal(Color.Blue, lbl.HoverForeColor);
            Assert.Equal(Color.LightYellow, lbl.HoverBackColor);
            Assert.True(lbl.HoverUnderline);
            Assert.Equal(Cursors.Hand, lbl.HoverCursor);
        }

        [Fact]
        public void ALabelBox_LinkPropertiesAndExecution_WorkCorrectly()
        {
            using var lbl = new ALabelBox();
            lbl.Url = "https://github.com/rgunawans/AuroraControls";
            lbl.AutoOpenUrl = false; // Prevent opening real browser in test
            lbl.IsLink = true;

            string openedUrl = null;
            lbl.LinkClicked += (s, url) => openedUrl = url;

            lbl.ExecuteLink();
            Assert.Equal("https://github.com/rgunawans/AuroraControls", openedUrl);
        }

        [Fact]
        public void ALabelBox_CopyTextAndEvents_WorkCorrectly()
        {
            using var lbl = new ALabelBox();
            lbl.Text = "Aurora Controls";
            lbl.CopyText = "Custom Copy String";
            lbl.ShowFeedbackOnCopy = false;

            string copiedText = null;
            lbl.LabelCopied += (s, text) => copiedText = text;

            lbl.CopyToClipboard();
            Assert.Equal("Custom Copy String", copiedText);
        }

        [Fact]
        public void ABarcode_QRCodeWithLogoAndColors_GeneratesValidBitmap()
        {
            using var barcode = new ABarcode();
            barcode.BarcodeType = BarcodeType.QRCode;
            barcode.Value = "https://github.com/rgunawans/AuroraControls";
            barcode.ForeColor = Color.Navy;
            barcode.BackColor = Color.WhiteSmoke;
            barcode.ErrorCorrection = QRCodeErrorCorrection.High;

            // Create a small test logo
            using var logo = new Bitmap(32, 32);
            using (var g = Graphics.FromImage(logo))
            {
                g.Clear(Color.Crimson);
            }
            barcode.Logo = logo;
            barcode.LogoShape = BarcodeLogoShape.RoundedRectangle;
            barcode.LogoSizePercent = 25;

            using var bmp = barcode.GenerateImage(200, 200);
            Assert.NotNull(bmp);
            Assert.Equal(200, bmp.Width);
            Assert.Equal(200, bmp.Height);
        }

        [Theory]
        [InlineData(BarcodeType.Code128, "AURORA-12345")]
        [InlineData(BarcodeType.Code39, "AURORA99")]
        [InlineData(BarcodeType.EAN13, "4006381333931")]
        public void ABarcode_1DTypes_GenerateValidBitmap(BarcodeType type, string value)
        {
            using var barcode = new ABarcode();
            barcode.BarcodeType = type;
            barcode.Value = value;
            barcode.ForeColor = Color.DarkSlateBlue;
            barcode.BackColor = Color.White;
            barcode.ShowText = true;

            using var bmp = barcode.GenerateImage(300, 120);
            Assert.NotNull(bmp);
            Assert.Equal(300, bmp.Width);
            Assert.Equal(120, bmp.Height);
        }
    }
}
