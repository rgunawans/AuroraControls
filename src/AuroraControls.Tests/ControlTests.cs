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
        public void ALabelTimer_FormatAndCulture_UpdateTextImmediately()
        {
            using var lbl = new ALabelTimer();
            lbl.FormatText = "yyyy";
            Assert.Equal(DateTime.Now.Year.ToString(), lbl.Text);

            lbl.Culture = new CultureInfo("fr-FR");
            lbl.FormatText = "MMMM";
            Assert.False(string.IsNullOrEmpty(lbl.Text));
        }
    }
}
