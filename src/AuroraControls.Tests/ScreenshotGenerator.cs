using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using AuroraControls;
using DemoProject;
using Xunit;

namespace AuroraControls.Tests
{
    public class ScreenshotGenerator
    {
        private string GetImagesDir()
        {
            // Find repo root (directory containing .git or README.md)
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            while (!string.IsNullOrEmpty(dir) && !File.Exists(Path.Combine(dir, "README.md")))
            {
                dir = Directory.GetParent(dir)?.FullName;
            }
            string imagesDir = Path.Combine(dir ?? ".", "docs", "images");
            Directory.CreateDirectory(imagesDir);
            return imagesDir;
        }

        [Fact]
        public void GenerateScreenshots()
        {
            string outDir = GetImagesDir();

            // Run STA thread for WinForms capture
            var thread = new Thread(() =>
            {
                CaptureDemoForm(Path.Combine(outDir, "demo_project.png"));
                CaptureButtons(Path.Combine(outDir, "component_abutton.png"));
                CaptureCheckBoxes(Path.Combine(outDir, "component_acheckbox.png"));
                CaptureTextBoxes(Path.Combine(outDir, "component_atextbox.png"));
                CaptureTabControl(Path.Combine(outDir, "component_atabcontrol.png"));
                CaptureMessageBox(Path.Combine(outDir, "component_amessagebox.png"));
                CaptureLabelTimer(Path.Combine(outDir, "component_alabeltimer.png"));
                CaptureLabelBox(Path.Combine(outDir, "component_alabelbox.png"));
                CaptureBarcode(Path.Combine(outDir, "component_abarcode.png"));
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            Assert.True(File.Exists(Path.Combine(outDir, "demo_project.png")));
        }

        private void CaptureDemoForm(string filePath)
        {
            using var form = new FormDemo();
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(100, 100);
            form.Show();
            Application.DoEvents();

            using var bmp = new Bitmap(form.Width, form.Height);
            form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
            bmp.Save(filePath, ImageFormat.Png);
            form.Close();
        }

        private void CaptureButtons(string filePath)
        {
            using var form = new Form
            {
                ClientSize = new Size(560, 220),
                BackColor = Color.FromArgb(248, 249, 250),
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual
            };

            var title = new Label
            {
                Text = "AButton Component Showcase",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(20, 15),
                AutoSize = true
            };
            form.Controls.Add(title);

            var btnNormal = new AButton
            {
                Text = "Standard Button",
                Size = new Size(150, 42),
                Location = new Point(25, 60),
                FocusedColor = Color.FromArgb(0, 120, 215),
                FocusedTextColor = Color.White
            };
            form.Controls.Add(btnNormal);

            var btnBadge = new AButton
            {
                Text = "Notifications",
                Size = new Size(150, 42),
                Location = new Point(195, 60),
                BadgeText = "5",
                BadgeBackColor = Color.Crimson,
                FocusedColor = Color.FromArgb(50, 60, 70),
                FocusedTextColor = Color.White
            };
            form.Controls.Add(btnBadge);

            var btnProgress = new AButton
            {
                Text = "Downloading...",
                Size = new Size(160, 42),
                Location = new Point(365, 60),
                ProgressValue = 65,
                ProgressColor = Color.FromArgb(40, 167, 69),
                FocusedColor = Color.FromArgb(52, 58, 64),
                FocusedTextColor = Color.White
            };
            form.Controls.Add(btnProgress);

            var btnLoading = new AButton
            {
                Text = "Processing...",
                Size = new Size(150, 42),
                Location = new Point(25, 130),
                IsLoading = true,
                FocusedColor = Color.FromArgb(94, 44, 165),
                FocusedTextColor = Color.White
            };
            form.Controls.Add(btnLoading);

            var btnSuccess = new AButton
            {
                Text = "✓ Saved Success",
                Size = new Size(150, 42),
                Location = new Point(195, 130),
                FocusedColor = Color.FromArgb(40, 167, 69),
                FocusedTextColor = Color.White
            };
            form.Controls.Add(btnSuccess);

            var btnOutline = new AButton
            {
                Text = "Danger / Delete",
                Size = new Size(160, 42),
                Location = new Point(365, 130),
                FocusedColor = Color.FromArgb(220, 53, 69),
                FocusedTextColor = Color.White
            };
            form.Controls.Add(btnOutline);

            form.Show();
            Application.DoEvents();

            using var bmp = new Bitmap(form.Width, form.Height);
            form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
            bmp.Save(filePath, ImageFormat.Png);
            form.Close();
        }

        private void CaptureCheckBoxes(string filePath)
        {
            using var form = new Form
            {
                ClientSize = new Size(540, 180),
                BackColor = Color.FromArgb(248, 249, 250),
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual
            };

            var title = new Label
            {
                Text = "ACheckBox Component (Standard & Switch Style)",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(20, 15),
                AutoSize = true
            };
            form.Controls.Add(title);

            var chk1 = new ACheckBox
            {
                Text = "Standard Check (Checked)",
                Location = new Point(30, 60),
                Size = new Size(220, 30),
                Checked = true,
                CheckColor = Color.FromArgb(220, 245, 230),
                CheckTextColor = Color.FromArgb(0, 100, 50)
            };
            form.Controls.Add(chk1);

            var chk2 = new ACheckBox
            {
                Text = "Standard Check (Unchecked)",
                Location = new Point(280, 60),
                Size = new Size(220, 30),
                Checked = false,
                UnCheckColor = Color.FromArgb(245, 245, 245),
                UnCheckTextColor = Color.FromArgb(80, 80, 80)
            };
            form.Controls.Add(chk2);

            var chkSwitchOn = new ACheckBox
            {
                Text = "Switch Mode (Active)",
                Location = new Point(30, 110),
                Size = new Size(220, 32),
                Style = CheckBoxStyle.Switch,
                Checked = true,
                SwitchOnColor = Color.FromArgb(0, 150, 136)
            };
            form.Controls.Add(chkSwitchOn);

            var chkSwitchOff = new ACheckBox
            {
                Text = "Switch Mode (Inactive)",
                Location = new Point(280, 110),
                Size = new Size(220, 32),
                Style = CheckBoxStyle.Switch,
                Checked = false,
                SwitchOffColor = Color.LightSlateGray
            };
            form.Controls.Add(chkSwitchOff);

            form.Show();
            Application.DoEvents();

            using var bmp = new Bitmap(form.Width, form.Height);
            form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
            bmp.Save(filePath, ImageFormat.Png);
            form.Close();
        }

        private void CaptureTextBoxes(string filePath)
        {
            using var form = new Form
            {
                ClientSize = new Size(560, 220),
                BackColor = Color.FromArgb(248, 249, 250),
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual
            };

            var title = new Label
            {
                Text = "ATextBox Component (Placeholder, Debounce, Mandatory)",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(20, 15),
                AutoSize = true
            };
            form.Controls.Add(title);

            var lbl1 = new Label { Text = "Placeholder Text:", Location = new Point(25, 55), AutoSize = true, Font = new Font("Segoe UI", 9f) };
            var txt1 = new ATextBox
            {
                Location = new Point(25, 75),
                Size = new Size(240, 30),
                PlaceholderText = "Search items, tags, codes...",
                Font = new Font("Segoe UI", 10f)
            };
            form.Controls.AddRange(new Control[] { lbl1, txt1 });

            var lbl2 = new Label { Text = "Mandatory / Required Field (Invalid state):", Location = new Point(290, 55), AutoSize = true, Font = new Font("Segoe UI", 9f) };
            var txt2 = new ATextBox
            {
                Location = new Point(290, 75),
                Size = new Size(240, 30),
                Mandatory = true,
                MandatoryColor = Color.FromArgb(255, 230, 230),
                Text = "",
                Font = new Font("Segoe UI", 10f)
            };
            form.Controls.AddRange(new Control[] { lbl2, txt2 });

            var lbl3 = new Label { Text = "Sentence Casing + Auto Select On Focus:", Location = new Point(25, 125), AutoSize = true, Font = new Font("Segoe UI", 9f) };
            var txt3 = new ATextBox
            {
                Location = new Point(25, 145),
                Size = new Size(240, 30),
                TextCasing = TextCasing.CapitalizeEachWord,
                Text = "john doe enterprise",
                Font = new Font("Segoe UI", 10f)
            };
            form.Controls.AddRange(new Control[] { lbl3, txt3 });

            var lbl4 = new Label { Text = "Debounce Search (400ms delay):", Location = new Point(290, 125), AutoSize = true, Font = new Font("Segoe UI", 9f) };
            var txt4 = new ATextBox
            {
                Location = new Point(290, 145),
                Size = new Size(240, 30),
                DebounceDelay = 400,
                Text = "aurora winforms",
                Font = new Font("Segoe UI", 10f)
            };
            form.Controls.AddRange(new Control[] { lbl4, txt4 });

            form.Show();
            Application.DoEvents();

            using var bmp = new Bitmap(form.Width, form.Height);
            form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
            bmp.Save(filePath, ImageFormat.Png);
            form.Close();
        }

        private void CaptureTabControl(string filePath)
        {
            using var form = new Form
            {
                ClientSize = new Size(620, 250),
                BackColor = Color.FromArgb(248, 249, 250),
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual
            };

            var title = new Label
            {
                Text = "ATabControl Component (Modern Flat Design)",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(20, 15),
                AutoSize = true
            };
            form.Controls.Add(title);

            var tab = new ATabControl
            {
                Location = new Point(20, 50),
                Size = new Size(580, 180),
                HeaderFont = new Font("Segoe UI", 10f, FontStyle.Regular),
                ItemSize = new Size(130, 36)
            };

            var page1 = new TabPage("Dashboard") { BackColor = Color.White, Padding = new Padding(12) };
            page1.Controls.Add(new Label { Text = "Dashboard Overview & Statistics with flat tabs", Location = new Point(15, 20), AutoSize = true, Font = new Font("Segoe UI", 10f) });
            page1.Controls.Add(new AButton { Text = "Primary Action", Location = new Point(15, 60), Size = new Size(130, 34), FocusedColor = Color.FromArgb(0, 120, 215), FocusedTextColor = Color.White });

            var page2 = new TabPage("Analytics");
            var page3 = new TabPage("Preferences");

            tab.TabPages.Add(page1);
            tab.TabPages.Add(page2);
            tab.TabPages.Add(page3);

            form.Controls.Add(tab);

            form.Show();
            Application.DoEvents();

            using var bmp = new Bitmap(form.Width, form.Height);
            form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
            bmp.Save(filePath, ImageFormat.Png);
            form.Close();
        }

        private void CaptureMessageBox(string filePath)
        {
            // Create a preview card representing AMessageBox variants
            using var form = new Form
            {
                ClientSize = new Size(680, 310),
                BackColor = Color.FromArgb(245, 247, 250),
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual
            };

            var title = new Label
            {
                Text = "AMessageBox Component (Modern Dialogs & Modals)",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(20, 15),
                AutoSize = true
            };
            form.Controls.Add(title);

            // Mock Card 1: Success Dialog Light
            var card1 = new Panel
            {
                Location = new Point(20, 50),
                Size = new Size(310, 230),
                BackColor = Color.White
            };
            var bar1 = new Panel { Dock = DockStyle.Top, Height = 4, BackColor = Color.FromArgb(40, 167, 69) };
            card1.Controls.Add(bar1);

            var lblT1 = new Label { Text = "✓ Transaksi Berhasil", Font = new Font("Segoe UI", 11f, FontStyle.Bold), Location = new Point(15, 18), AutoSize = true, ForeColor = Color.FromArgb(33, 37, 41) };
            var lblM1 = new Label { Text = "Data transaksi baru telah berhasil disimpan ke database tanpa kendala.", Location = new Point(15, 48), Size = new Size(280, 50), ForeColor = Color.FromArgb(80, 80, 80), Font = new Font("Segoe UI", 9.5f) };
            var btnOk1 = new AButton { Text = "OK", Location = new Point(190, 175), Size = new Size(100, 34), FocusedColor = Color.FromArgb(40, 167, 69), FocusedTextColor = Color.White };
            card1.Controls.AddRange(new Control[] { lblT1, lblM1, btnOk1 });
            form.Controls.Add(card1);

            // Mock Card 2: Dark Theme Confirmation / Warning
            var card2 = new Panel
            {
                Location = new Point(350, 50),
                Size = new Size(310, 230),
                BackColor = Color.FromArgb(30, 32, 38)
            };
            var bar2 = new Panel { Dock = DockStyle.Top, Height = 4, BackColor = Color.FromArgb(235, 145, 0) };
            card2.Controls.Add(bar2);

            var lblT2 = new Label { Text = "⚠ Konfirmasi Hapus", Font = new Font("Segoe UI", 11f, FontStyle.Bold), Location = new Point(15, 18), AutoSize = true, ForeColor = Color.White };
            var lblM2 = new Label { Text = "Apakah Anda yakin ingin menghapus data ini? Aksi ini tidak dapat dibatalkan.", Location = new Point(15, 48), Size = new Size(280, 50), ForeColor = Color.FromArgb(200, 200, 200), Font = new Font("Segoe UI", 9.5f) };
            var btnNo = new AButton { Text = "Batal", Location = new Point(80, 175), Size = new Size(95, 34), FocusedColor = Color.FromArgb(60, 64, 75), FocusedTextColor = Color.White };
            var btnYes = new AButton { Text = "Hapus", Location = new Point(190, 175), Size = new Size(100, 34), FocusedColor = Color.FromArgb(220, 53, 69), FocusedTextColor = Color.White };
            card2.Controls.AddRange(new Control[] { lblT2, lblM2, btnNo, btnYes });
            form.Controls.Add(card2);

            form.Show();
            Application.DoEvents();

            using var bmp = new Bitmap(form.Width, form.Height);
            form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
            bmp.Save(filePath, ImageFormat.Png);
            form.Close();
        }

        private void CaptureLabelTimer(string filePath)
        {
            using var form = new Form
            {
                ClientSize = new Size(540, 160),
                BackColor = Color.FromArgb(248, 249, 250),
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual
            };

            var title = new Label
            {
                Text = "ALabelTimer Component (Live Clocks & Formatting)",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(20, 15),
                AutoSize = true
            };
            form.Controls.Add(title);

            var lbl1 = new ALabelTimer
            {
                FormatText = "HH:mm:ss",
                Location = new Point(25, 60),
                Size = new Size(220, 40),
                Font = new Font("Segoe UI", 18f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 120, 215)
            };
            form.Controls.Add(lbl1);

            var lbl2 = new ALabelTimer
            {
                FormatText = "dddd, dd MMMM yyyy",
                Location = new Point(25, 105),
                Size = new Size(350, 30),
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                ForeColor = Color.FromArgb(80, 80, 80)
            };
            form.Controls.Add(lbl2);

            form.Show();
            Application.DoEvents();

            using var bmp = new Bitmap(form.Width, form.Height);
            form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
            bmp.Save(filePath, ImageFormat.Png);
            form.Close();
        }

        private void CaptureLabelBox(string filePath)
        {
            using var form = new Form
            {
                ClientSize = new Size(500, 190),
                BackColor = Color.White,
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual,
                Location = new Point(100, 100)
            };

            var title = new Label
            {
                Text = "ALabelBox Component (Web Hover, URL & Copy)",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(20, 15),
                AutoSize = true
            };
            form.Controls.Add(title);

            var lbl1 = new ALabelBox
            {
                Text = "Interactive Web Hover (Underline & Color)",
                Location = new Point(25, 55),
                AutoSize = true,
                Font = new Font("Segoe UI", 10f, FontStyle.Regular),
                ForeColor = Color.DarkSlateGray,
                HoverForeColor = Color.Crimson,
                HoverUnderline = true
            };
            form.Controls.Add(lbl1);

            var lbl2 = new ALabelBox
            {
                Text = "https://github.com/rgunawans/AuroraControls (Click to open)",
                Location = new Point(25, 90),
                AutoSize = true,
                Font = new Font("Segoe UI", 10f, FontStyle.Underline),
                ForeColor = Color.RoyalBlue,
                HoverForeColor = Color.Navy,
                IsLink = true,
                Url = "https://github.com/rgunawans/AuroraControls"
            };
            form.Controls.Add(lbl2);

            var lbl3 = new ALabelBox
            {
                Text = "INV-2026-AURORA (Click to Copy to Clipboard)",
                Location = new Point(25, 125),
                AutoSize = true,
                Font = new Font("Segoe UI", 10f, FontStyle.Regular),
                ForeColor = Color.SeaGreen,
                EnableCopyOnClick = true
            };
            form.Controls.Add(lbl3);

            form.Show();
            Application.DoEvents();

            using var bmp = new Bitmap(form.Width, form.Height);
            form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
            bmp.Save(filePath, ImageFormat.Png);
            form.Close();
        }

        private void CaptureBarcode(string filePath)
        {
            using var form = new Form
            {
                ClientSize = new Size(580, 240),
                BackColor = Color.White,
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual,
                Location = new Point(100, 100)
            };

            var title = new Label
            {
                Text = "ABarcode Component (1D & 2D Barcode, Color, Logo)",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(20, 15),
                AutoSize = true
            };
            form.Controls.Add(title);

            // Sample Aurora logo
            var logo = new Bitmap(36, 36);
            using (var g = Graphics.FromImage(logo))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.DodgerBlue);
                using (var f = new Font("Segoe UI", 16f, FontStyle.Bold))
                using (var b = new SolidBrush(Color.White))
                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    g.DrawString("A", f, b, new RectangleF(0, 0, 36, 36), sf);
                }
            }

            // QR Code with Logo & Custom ForeColor
            var qr = new ABarcode
            {
                Location = new Point(25, 50),
                Size = new Size(160, 160),
                BarcodeType = BarcodeType.QRCode,
                Value = "https://github.com/rgunawans/AuroraControls",
                ForeColor = Color.MidnightBlue,
                BackColor = Color.FromArgb(248, 249, 250),
                Logo = logo,
                LogoShape = BarcodeLogoShape.RoundedRectangle,
                LogoSizePercent = 25
            };
            form.Controls.Add(qr);

            // Code 128
            var code128 = new ABarcode
            {
                Location = new Point(205, 50),
                Size = new Size(350, 75),
                BarcodeType = BarcodeType.Code128,
                Value = "AURORA-128-DEMO",
                ForeColor = Color.Black,
                BackColor = Color.FromArgb(248, 249, 250),
                ShowText = true
            };
            form.Controls.Add(code128);

            // Code 39
            var code39 = new ABarcode
            {
                Location = new Point(205, 135),
                Size = new Size(350, 75),
                BarcodeType = BarcodeType.Code39,
                Value = "CODE39-OK",
                ForeColor = Color.DarkSlateGray,
                BackColor = Color.FromArgb(248, 249, 250),
                ShowText = true
            };
            form.Controls.Add(code39);

            form.Show();
            Application.DoEvents();

            using var bmp = new Bitmap(form.Width, form.Height);
            form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
            bmp.Save(filePath, ImageFormat.Png);
            form.Close();
        }
    }
}
