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
    }
}
