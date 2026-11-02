using System;
using System.Drawing;
using System.Windows.Forms;
using AuroraControls;

namespace DemoProject
{
    public partial class FormDemo : Form
    {
        private Label lblStatus;
        private AButton btnToggleStyle;
        private AButton btnIncProgress;
        private ATabControl aTabControl1;
        private bool _isDarkTab = false;
        private Timer _loadingTimer;

        public FormDemo()
        {
            InitializeComponent();
            SetupNewFeaturesDemo();
        }

        private void SetupNewFeaturesDemo()
        {
            this.ClientSize = new Size(800, 530);

            // Status label for feedback
            lblStatus = new Label
            {
                Location = new Point(12, 235),
                Size = new Size(760, 35),
                ForeColor = Color.DarkSlateGray,
                Text = "Status: Siap (Ketik pada TextBox untuk uji debounce)"
            };
            this.Controls.Add(lblStatus);

            // ATextBox 1: Cue banner placeholder & Debounce search demo
            aTextBox1.PlaceholderText = "Ketik di sini (Debounce Search 400ms)...";
            aTextBox1.DebounceDelay = 400;
            aTextBox1.DebouncedTextChanged += (s, e) =>
            {
                lblStatus.Text = $"[Debounce Search] Hasil: \"{aTextBox1.Text}\" ({DateTime.Now:HH:mm:ss})";
            };

            // ATextBox 2: Placeholder & Sentence casing
            aTextBox2.PlaceholderText = "Field Wajib & Title Case...";
            aTextBox2.TextCasing = TextCasing.CapitalizeEachWord;

            // AButton: Badge, Progress bar, & Async Loading state demo
            aButton1.Text = "Proses Data (Async)";
            aButton1.Width = 150;
            aButton1.BadgeText = "NEW";
            aButton1.BadgeBackColor = Color.Crimson;
            aButton1.ProgressValue = 40;
            aButton1.Click += (s, e) =>
            {
                aButton1.IsLoading = true;
                lblStatus.Text = "Status: Operasi async sedang berjalan...";

                _loadingTimer?.Dispose();
                _loadingTimer = new Timer { Interval = 2000 };
                _loadingTimer.Tick += (ts, te) =>
                {
                    _loadingTimer.Stop();
                    aButton1.IsLoading = false;
                    lblStatus.Text = "Status: Operasi async selesai!";
                };
                _loadingTimer.Start();
            };

            // Button to increment progress
            btnIncProgress = new AButton
            {
                Location = new Point(170, 183),
                Size = new Size(110, 36),
                Text = "+10% Progress"
            };
            btnIncProgress.Click += (s, e) =>
            {
                aButton1.ProgressValue = (aButton1.ProgressValue + 10) % 110;
                lblStatus.Text = $"Status: Progress = {aButton1.ProgressValue}%";
            };
            this.Controls.Add(btnIncProgress);

            // ACheckBox: Modern Toggle Switch demo
            aCheckBox1.Text = "Mode Switch";
            aCheckBox1.Style = CheckBoxStyle.Switch;
            aCheckBox1.SwitchOnColor = Color.FromArgb(0, 150, 136);

            // Button to toggle checkbox style
            btnToggleStyle = new AButton
            {
                Location = new Point(140, 22),
                Size = new Size(110, 26),
                Text = "Ganti Gaya Check"
            };
            btnToggleStyle.Click += (s, e) =>
            {
                aCheckBox1.Style = aCheckBox1.Style == CheckBoxStyle.Standard
                    ? CheckBoxStyle.Switch
                    : CheckBoxStyle.Standard;
                lblStatus.Text = $"Status: CheckBox Style = {aCheckBox1.Style}";
            };
            this.Controls.Add(btnToggleStyle);

            // ATabControl Demo
            aTabControl1 = new ATabControl
            {
                Location = new Point(12, 275),
                Size = new Size(760, 240),
                HeaderFont = new Font("Segoe UI", 10f, FontStyle.Regular),
                ItemSize = new Size(140, 36)
            };

            var page1 = new TabPage("Dashboard") { Padding = new Padding(12) };
            page1.Controls.Add(new Label { Text = "Halaman Dashboard: Kontainer mengikuti ContainerColor.", Location = new Point(15, 15), AutoSize = true });

            var page2 = new TabPage("Pengaturan") { Padding = new Padding(12) };
            page2.Controls.Add(new Label { Text = "Halaman Pengaturan: Font header & warna tab dapat disesuaikan.", Location = new Point(15, 15), AutoSize = true });

            // AMessageBox Showcase Tab
            var pageMsgBox = new TabPage("AMessageBox Demo") { Padding = new Padding(12) };
            SetupMessageBoxDemoTab(pageMsgBox);

            aTabControl1.TabPages.Add(pageMsgBox);
            aTabControl1.TabPages.Add(page1);
            aTabControl1.TabPages.Add(page2);

            this.Controls.Add(aTabControl1);

            // Button to toggle Header Font Size
            var btnToggleTabFont = new AButton
            {
                Location = new Point(290, 183),
                Size = new Size(140, 36),
                Text = "Ubah Font Tab"
            };
            btnToggleTabFont.Click += (s, e) =>
            {
                float newSize = aTabControl1.HeaderFont.Size == 10f ? 12f : 10f;
                aTabControl1.HeaderFont = new Font("Segoe UI", newSize, FontStyle.Bold);
                lblStatus.Text = $"Tab Header Font diubah: {newSize}pt Bold";
            };
            this.Controls.Add(btnToggleTabFont);

            // Button to toggle Dark/Light Theme on ATabControl
            var btnToggleTabTheme = new AButton
            {
                Location = new Point(440, 183),
                Size = new Size(150, 36),
                Text = "Tema Tab (Dark/Light)"
            };
            btnToggleTabTheme.Click += (s, e) =>
            {
                _isDarkTab = !_isDarkTab;
                if (_isDarkTab)
                {
                    aTabControl1.ContainerColor = Color.FromArgb(32, 33, 36);
                    aTabControl1.ActiveTabColor = Color.FromArgb(45, 46, 50);
                    aTabControl1.ActiveTextColor = Color.DeepSkyBlue;
                    aTabControl1.InactiveTabColor = Color.FromArgb(24, 25, 27);
                    aTabControl1.InactiveTextColor = Color.LightGray;
                    aTabControl1.HoverTabColor = Color.FromArgb(60, 64, 67);
                    aTabControl1.HoverTextColor = Color.White;
                    aTabControl1.BorderColor = Color.FromArgb(70, 70, 75);
                    aTabControl1.IndicatorColor = Color.DeepSkyBlue;
                    foreach (TabPage p in aTabControl1.TabPages)
                        foreach (Control ctl in p.Controls) ctl.ForeColor = Color.White;
                }
                else
                {
                    aTabControl1.ContainerColor = Color.White;
                    aTabControl1.ActiveTabColor = Color.White;
                    aTabControl1.ActiveTextColor = Color.FromArgb(0, 120, 215);
                    aTabControl1.InactiveTabColor = Color.FromArgb(240, 240, 240);
                    aTabControl1.InactiveTextColor = Color.FromArgb(100, 100, 100);
                    aTabControl1.HoverTabColor = Color.FromArgb(230, 230, 230);
                    aTabControl1.HoverTextColor = Color.FromArgb(40, 40, 40);
                    aTabControl1.BorderColor = Color.FromArgb(218, 220, 224);
                    aTabControl1.IndicatorColor = Color.FromArgb(0, 120, 215);
                    foreach (TabPage p in aTabControl1.TabPages)
                        foreach (Control ctl in p.Controls) ctl.ForeColor = Color.Black;
                }
                lblStatus.Text = $"Tema Tab: {(_isDarkTab ? "Dark Mode" : "Light Mode")}";
            };
            this.Controls.Add(btnToggleTabTheme);
        }

        private void SetupMessageBoxDemoTab(TabPage tab)
        {
            var lblInfo = new Label
            {
                Text = "Klik tombol berikut untuk mendemokan varian AMessageBox:",
                Location = new Point(15, 12),
                AutoSize = true,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            tab.Controls.Add(lblInfo);

            int btnW = 160;
            int btnH = 34;

            // 1. Info Box (Sesuai Desain Gambar)
            var btn1 = new AButton { Text = "1. Info (Desain Ungu)", Location = new Point(15, 42), Size = new Size(btnW, btnH), FocusedColor = Color.FromArgb(94, 44, 165), FocusedTextColor = Color.White };
            btn1.Click += (s, e) =>
            {
                var res = AMessageBox.Show(this, "Message text", "Info box title", MessageBoxButtons.OK, AMessageBoxIcon.Info);
                lblStatus.Text = $"[AMessageBox] Info Result: {res}";
            };
            tab.Controls.Add(btn1);

            // 2. Success Box
            var btn2 = new AButton { Text = "2. Success Emerald", Location = new Point(190, 42), Size = new Size(btnW, btnH), FocusedColor = Color.FromArgb(40, 167, 69), FocusedTextColor = Color.White };
            btn2.Click += (s, e) =>
            {
                var res = AMessageBox.Show(this, "Data transaksi baru telah berhasil disimpan ke database.", "Transaksi Berhasil", MessageBoxButtons.OK, AMessageBoxIcon.Success, Color.FromArgb(40, 167, 69));
                lblStatus.Text = $"[AMessageBox] Success Result: {res}";
            };
            tab.Controls.Add(btn2);

            // 3. Warning (Yes / No)
            var btn3 = new AButton { Text = "3. Warning (Yes/No)", Location = new Point(365, 42), Size = new Size(btnW, btnH), FocusedColor = Color.FromArgb(235, 145, 0), FocusedTextColor = Color.White };
            btn3.Click += (s, e) =>
            {
                var res = AMessageBox.Show(this, "Apakah Anda yakin ingin menghapus data yang dipilih secara permanen?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, AMessageBoxIcon.Warning);
                lblStatus.Text = $"[AMessageBox] Warning Result: {res}";
            };
            tab.Controls.Add(btn3);

            // 4. Error + Collapsible Details
            var btn4 = new AButton { Text = "4. Error + Log Details", Location = new Point(540, 42), Size = new Size(btnW, btnH), FocusedColor = Color.FromArgb(220, 53, 69), FocusedTextColor = Color.White };
            btn4.Click += (s, e) =>
            {
                var res = AMessageBox.Show(this, new AMessageBoxOptions
                {
                    Title = "Koneksi Timeout",
                    Message = "Gagal menghubungi server database Aurora. Silakan periksa jaringan Anda.",
                    Icon = AMessageBoxIcon.Error,
                    DetailsText = "System.TimeoutException: The operation has timed out.\r\n   at AuroraControls.Demo.Connect()\r\n   at AuroraControls.Demo.ExecuteQuery()",
                    Buttons = MessageBoxButtons.RetryCancel,
                    CustomButton1Text = "Coba Lagi",
                    CustomButton2Text = "Batal"
                });
                lblStatus.Text = $"[AMessageBox] Error Result: {res}";
            };
            tab.Controls.Add(btn4);

            // 5. Input Prompt Dialog
            var btn5 = new AButton { Text = "5. Input Prompt", Location = new Point(15, 88), Size = new Size(btnW, btnH), FocusedColor = Color.FromArgb(23, 162, 184), FocusedTextColor = Color.White };
            btn5.Click += (s, e) =>
            {
                string input = AMessageBox.ShowInput(this, "Masukkan nama folder proyek baru:", "Buat Folder", defaultValue: "Proyek_Baru");
                lblStatus.Text = input != null ? $"[AMessageBox] Input diterima: \"{input}\"" : "[AMessageBox] Input dibatalkan.";
            };
            tab.Controls.Add(btn5);

            // 6. Floating Toast Popup
            var btn6 = new AButton { Text = "6. Floating Toast (3s)", Location = new Point(190, 88), Size = new Size(btnW, btnH), FocusedColor = Color.FromArgb(108, 117, 125), FocusedTextColor = Color.White };
            btn6.Click += (s, e) =>
            {
                AMessageBox.ShowToast("Laporan bulanan selesai di-generate.", "Notifikasi Sistem", AMessageBoxIcon.Success, 3);
                lblStatus.Text = "[AMessageBox] Floating Toast ditampilkan di pojok kanan bawah.";
            };
            tab.Controls.Add(btn6);

            // 7. Dark Mode Theme
            var btn7 = new AButton { Text = "7. Dark Mode Theme", Location = new Point(365, 88), Size = new Size(btnW, btnH), FocusedColor = Color.FromArgb(50, 50, 60), FocusedTextColor = Color.White };
            btn7.Click += (s, e) =>
            {
                var res = AMessageBox.Show(this, new AMessageBoxOptions
                {
                    Title = "Dark Mode Popup",
                    Message = "Ini adalah tampilan tema gelap elegan dengan border & button yang disesuaikan.",
                    Theme = AMessageBoxTheme.Dark,
                    Icon = AMessageBoxIcon.Info,
                    Buttons = MessageBoxButtons.OKCancel,
                    CustomButton1Text = "Mengerti",
                    CustomButton2Text = "Tutup"
                });
                lblStatus.Text = $"[AMessageBox] Dark Mode Result: {res}";
            };
            tab.Controls.Add(btn7);

            // 8. Auto-Close Countdown Ticker
            var btn8 = new AButton { Text = "8. Countdown (5s)", Location = new Point(540, 88), Size = new Size(btnW, btnH), FocusedColor = Color.FromArgb(138, 43, 226), FocusedTextColor = Color.White };
            btn8.Click += (s, e) =>
            {
                var res = AMessageBox.Show(this, new AMessageBoxOptions
                {
                    Title = "Auto-Close Dialog",
                    Message = "Dialog ini akan otomatis tertutup sendiri jika tidak ada tindakan dalam 5 detik.",
                    AutoCloseSeconds = 5,
                    Icon = AMessageBoxIcon.Info
                });
                lblStatus.Text = $"[AMessageBox] Countdown Dialog Result: {res}";
            };
            tab.Controls.Add(btn8);
        }

        private void setcolor()
        {
            aTextBox1.Mandatory = chMandatory.Checked;
            aTextBox2.Mandatory = chMandatory.Checked;
            aTextBox1.SelectAllOnFocus = chSelectAll.Checked;
            aTextBox2.SelectAllOnFocus = chSelectAll.Checked;
        }

        private void btnFocusFore_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pctFocusFore.BackColor = colorDialog1.Color;
                aTextBox1.FocusedColor = colorDialog1.Color;
                aTextBox2.FocusedColor = colorDialog1.Color;
                aButton1.FocusedColor = colorDialog1.Color;
                aCheckBox1.CheckColor = colorDialog1.Color;
                setcolor();
            }
        }

        private void btnFocusText_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pctFocusText.BackColor = colorDialog1.Color;
                aTextBox1.FocusedTextColor = colorDialog1.Color;
                aTextBox2.FocusedTextColor = colorDialog1.Color;
                aButton1.FocusedTextColor = colorDialog1.Color;
                aCheckBox1.CheckTextColor = colorDialog1.Color;
                setcolor();
            }
        }

        private void btnUnFocusFore_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pctUnFocusFore.BackColor = colorDialog1.Color;
                aTextBox1.UnfocusedColor = colorDialog1.Color;
                aTextBox2.UnfocusedColor = colorDialog1.Color;
                aButton1.UnfocusedColor = colorDialog1.Color;
                aCheckBox1.UnCheckColor = colorDialog1.Color;
                setcolor();
            }
        }

        private void btnUnFocusText_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pctUnFocusText.BackColor = colorDialog1.Color;
                aTextBox1.UnfocusedTextColor = colorDialog1.Color;
                aTextBox2.UnfocusedTextColor = colorDialog1.Color;
                aButton1.UnfocusedTextColor = colorDialog1.Color;
                aCheckBox1.UnCheckTextColor = colorDialog1.Color;
                setcolor();
            }
        }

        private void chSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            setcolor();
        }

        private void ChMandatory_CheckedChanged(object sender, EventArgs e)
        {
            setcolor();
        }

        private void FormDemo_Load(object sender, EventArgs e)
        {
            setcolor();
        }
    }
}
