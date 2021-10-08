using MetroFramework;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

namespace EPW_Recaster
{
    public partial class MainGui : MetroForm
    {
        #region Variables | Properties.

        #region Condition List Container.

        private List<ConditionList> ConditionLists { get; set; } = new List<ConditionList>();
        private int CurrentConditionListIndex { get; set; } = 0;

        private int MaxConditionLists { get; set; } = 5;

        #endregion Condition List Container.

        #region Main Gui.

        private double CaptureRegionHeightClipping { get; set; } = 0.75;

        #endregion Main Gui.

        #region Info Gui.

        private InfoGui InfoGui { get; set; }

        #endregion Info Gui.

        #region Task Tokens.

        internal CancellationTokenSource Ocr_CancellationTokenSource { get; set; } = null;

        internal DateTime Ocr_StartTime { get; set; } = new DateTime();
        internal CancellationToken Ocr_Token { get; set; } = new CancellationToken();

        #endregion Task Tokens.

        #region GUI Progress Reporting.

        private IProgress<RtMessage> GuiInfoRichTxtProgress { get; set; } = null;
        private IProgress<string> GuiInfoTxtProgress { get; set; } = null;

        #endregion GUI Progress Reporting.

        #region Custom text colors.

        // ( Preview reference: http://www.flounder.com/csharp_color_table.htm )
        private Color DefaultColor { get; set; } = new Color();

        private Color GreenLightColor { get; set; } = Color.ForestGreen;
        private Color OrangeLightColor { get; set; } = Color.Tomato;
        private Color RedLightColor { get; set; } = Color.Firebrick;
        private Color BlueStatColor { get; set; } = Color.RoyalBlue;
        private Color ComboStatColor { get; set; } = Color.Goldenrod;

        #endregion Custom text colors.

        #endregion Variables | Properties.

        #region Methods.

        #region Progress Handling Methods.

        internal void AddMsg(string msg = "")
        {
            GuiInfoTxtProgress.Report(msg); // [DEVNOTE] Could add a class containing a text string and text color.

            Ocr_Token.ThrowIfCancellationRequested();
        }

        internal void AddMsg(RtMessage msg)
        {
            GuiInfoRichTxtProgress.Report(msg);

            Ocr_Token.ThrowIfCancellationRequested();
        }

        private void InitializeProgressHandling()
        {
            // Set up GUI Info Text progress handling.
            // [DEVNOTE] 'Modifiers' property on child form's control should be set to (f.e.) 'Internal' in order to get access.
            GuiInfoTxtProgress = new Progress<string>(value =>
            {
                if (!String.IsNullOrEmpty(value))
                {
                    // If not first appended text,
                    // add a leading newline.
                    if (!String.IsNullOrEmpty(InfoGui.rTxtBoxInfo.Text))
                    {
                        InfoGui.rTxtBoxInfo.AppendText(Environment.NewLine);
                    }
                    else
                    {
                        // Text box is currently empty.
                        // Trim any leading newlines in given text message.
                        value = value.TrimStart('\r', '\n');
                    }

                    // Append given text.
                    InfoGui.rTxtBoxInfo.AppendText(value);
                }
                else
                {
                    // If given text is empty,
                    // clear rich text box contents.
                    InfoGui.rTxtBoxInfo.Text = "";
                }

                // Auto scroll to bottom.
                if (InfoGui.chkbxAutoScrollBottom.Checked)
                {
                    InfoGui.rTxtBoxInfo.SelectionStart = InfoGui.rTxtBoxInfo.TextLength;
                    InfoGui.rTxtBoxInfo.ScrollToCaret();
                }

                // ~ InfoGui.rTxtBoxInfo.Refresh();
                InfoGui.rTxtBoxInfo.Invalidate();
                InfoGui.rTxtBoxInfo.Update();
            });

            GuiInfoRichTxtProgress = new Progress<RtMessage>(value =>
            {
                if (!String.IsNullOrEmpty(value.Message))
                {
                    // If not first appended text,
                    // add a leading newline.
                    if (!String.IsNullOrEmpty(InfoGui.rTxtBoxInfo.Text))
                    {
                        InfoGui.rTxtBoxInfo.AppendText(Environment.NewLine);
                    }
                    else
                    {
                        // Text box is currently empty.
                        // Trim any leading newlines in given text message.
                        value.Message = value.Message.TrimStart('\r', '\n');
                    }

                    // Change text color if needed.
                    Color origTextColor = InfoGui.rTxtBoxInfo.SelectionColor;
                    if (value.CustomColor)
                    {
                        InfoGui.rTxtBoxInfo.SelectionStart = InfoGui.rTxtBoxInfo.TextLength;
                        InfoGui.rTxtBoxInfo.SelectionLength = 0;

                        InfoGui.rTxtBoxInfo.SelectionColor = value.Color;
                    }

                    // Change text font if needed.
                    Font origTextFont = InfoGui.rTxtBoxInfo.SelectionFont;
                    if (value.Bold)
                    {
                        InfoGui.rTxtBoxInfo.SelectionStart = InfoGui.rTxtBoxInfo.TextLength;
                        InfoGui.rTxtBoxInfo.SelectionLength = 0;

                        InfoGui.rTxtBoxInfo.SelectionFont = new Font(InfoGui.rTxtBoxInfo.Font, FontStyle.Bold);
                    }

                    // Append given text.
                    InfoGui.rTxtBoxInfo.AppendText(value.Message);

                    // Restore original text color if needed.
                    if (value.CustomColor)
                    {
                        InfoGui.rTxtBoxInfo.SelectionColor = origTextColor;
                    }

                    // Restore original text font if needed.
                    if (value.Bold)
                    {
                        InfoGui.rTxtBoxInfo.SelectionFont = origTextFont;
                    }
                }
                else
                {
                    // If given text is empty,
                    // clear rich text box contents.
                    InfoGui.rTxtBoxInfo.Text = "";
                }

                // Auto scroll to bottom.
                if (InfoGui.chkbxAutoScrollBottom.Checked)
                {
                    InfoGui.rTxtBoxInfo.SelectionStart = InfoGui.rTxtBoxInfo.TextLength;
                    InfoGui.rTxtBoxInfo.ScrollToCaret();
                }

                // ~ InfoGui.rTxtBoxInfo.Refresh();
                InfoGui.rTxtBoxInfo.Invalidate();
                InfoGui.rTxtBoxInfo.Update();
            });
        }

        #endregion Progress Handling Methods.

        #region Constructors | Initialization Methods.

        public MainGui()
        {
            InitializeComponent();

            #region Quick Dev Test(s).

            bool devTest = false;
            //bool devTest = true;
            if (devTest)
            {
                Properties.Settings.Default.Reset();
                Properties.Settings.Default.Save();
            }

            #endregion Quick Dev Test(s).

            try
            {
                // =========
                // Info Gui.
                // =========
                InfoGui = new InfoGui(this);
                InitializeInfoGuiStyle(); // [DEVNOTE] Best to place before showing.
                InfoGui.Show();

                // =========
                // Main Gui.
                // =========

                InitializeProgressHandling();

                InitializeMainGuiStyle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex, "Exception caught");
                this.Close();
            }
        }

        private void CheckAdminPrivileges()
        {
            // =============================================
            // AT FIRST LAUNCH ONLY ( [DEVNOTE] Kind of... )
            // =============================================
            if (!Directory.Exists(Tesseract.Ocr.AssemblyCodeBaseDirectory + @"\Logged"))
            {
                // Check if executable is not being ran as an administrator.
                if (
                    !new WindowsPrincipal(WindowsIdentity.GetCurrent())
                    .IsInRole(WindowsBuiltInRole.Administrator)
                 )
                {
                    // Show a first launch warning to user.
                    MetroMessageBox.Show(this,
                        "It seems you are not running this program as an administrator.\r\n" +
                        "Some low level actions may not work when running this program.\r\n" +
                        //"in user mode ( f.e. sending click events programmatically ).\r\n" +
                        "[ In order to run this program as an administrator: ]\r\n" +
                        "  • Right-click '" + System.Diagnostics.Process.GetCurrentProcess().ProcessName + "(.exe)' > Choose 'Properties'.\r\n" +
                        "  • 'Compatibility' tab > Check '☑ Run this program as an admin'.\r\n" +
                        "  • Confirm by clicking 'OK' && relaunch.",
                        "( Beep ... At First Launch Notification Only )", // Stop | Warning | Notification
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // Now create that directory not to bug the user again,
                // unless that directory is removed.
                Directory.CreateDirectory(Tesseract.Ocr.AssemblyCodeBaseDirectory + @"\Logged");

                // Close the application.
                // Up to the user whether it's relaunched in admin mode or not.
                //Application.Exit(); [DEVNOTE] Commented out (for user-friendliness).
            }
        }

        internal void SetInfoGuiSizeAndPosition()
        {
            if (InfoGui.FormsChained)
            {
                if (this.WindowState != FormWindowState.Minimized)
                {
                    InfoGui.Location = new Point(
                                    this.Location.X + this.Size.Width + 5,
                                    this.Location.Y);

                    InfoGui.Size = new Size(300, this.Size.Height);
                }
            }
            else
            {
                if (Properties.Settings.Default.InfoWindowLocation.X != 0)
                {
                    InfoGui.Location = Properties.Settings.Default.InfoWindowLocation;
                }

                if (Properties.Settings.Default.InfoWindowSize.Width != 0)
                {
                    InfoGui.Size = Properties.Settings.Default.InfoWindowSize;
                }
            }
        }

        private void InitializeInfoGuiStyle()
        {
            // Size & Position.
            // ----------------
            InfoGui.StartPosition = FormStartPosition.Manual;
            InfoGui.CheckFormsChainState(toggle: false);

            // Theme | Style | Colors.
            // -----------------------

            InfoGui.Theme = metroStyleManager.Theme;
            InfoGui.Style = metroStyleManager.Style;

            InfoGui.Opacity = this.Opacity;
            InfoGui.TopMost = this.TopMost;

            // Display logo.
            InfoGui.pbLogo.BackColor = Color.Transparent;
            InfoGui.pbLogo.Location = new Point(InfoGui.pbLogo.Location.X, InfoGui.pbLogo.Location.Y - 5);
            InfoGui.pbLogo.Image = Image.FromFile(Tesseract.Ocr.AssemblyCodeBaseDirectory + @"\Media\Images\Logo.png");

            // Custom.
            InfoGui.btnChainForms.Theme = metroStyleManager.Theme;

            InfoGui.btnLogFolder.Theme = metroStyleManager.Theme;

            InfoGui.chkbxAutoScrollBottom.Theme = metroStyleManager.Theme;
            InfoGui.chkbxAutoScrollBottom.Style = metroStyleManager.Style;

            InfoGui.rTxtBoxInfo.BackColor = this.BackColor;
            InfoGui.rTxtBoxInfo.ForeColor = MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme);

            InfoGui.btnOcr.Theme = metroStyleManager.Theme;

            InfoGui.lblMaxRolls.Theme = metroStyleManager.Theme;
            InfoGui.numMaxRolls.BackColor = this.BackColor;
            InfoGui.numMaxRolls.ForeColor = MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme);

            InfoGui.chkbxPreviewCapture.Theme = metroStyleManager.Theme;
            InfoGui.chkbxPreviewCapture.Style = metroStyleManager.Style;

            // Set stored Max Rolls.
            if (Properties.Settings.Default.MaxRolls > 0 & Properties.Settings.Default.MaxRolls <= InfoGui.numMaxRolls.Maximum)
            {
                InfoGui.numMaxRolls.Value = Properties.Settings.Default.MaxRolls;
            }
        }

        private void InitializeMainGuiStyle()
        {
            // Condition list related.
            // -----------------------

            // Set up context menu('s).
            cmExportImport.Theme = metroStyleManager.Theme;
            cmExportImport.Style = metroStyleManager.Style;

            // Set current condition list index,
            // check appropriate radio button &
            // add descriptive tooltips.
            if (Properties.Settings.Default.CurrentConditionListIndex >= 0 & Properties.Settings.Default.CurrentConditionListIndex <= MaxConditionLists)
            {
                CurrentConditionListIndex = Properties.Settings.Default.CurrentConditionListIndex;

                foreach (RadioButton rBtn in this.Controls.OfType<RadioButton>())
                {
                    if (rBtn.Text.Contains((CurrentConditionListIndex + 1).ToString())) // [DEVNOTE] Needs a translation from an index to human readable number.
                    {
                        rBtn.Checked = true;
                        //break; // [DEVNOTE] Commented (out).
                        //  Reason:  Code loop needs to add descriptive tooltips for all radio buttons (on this form).
                    }

                    toolTip.SetToolTip(rBtn, "\r\n" +
                        "ℹ\r\n" +
                        "• ( Left-Click ) Switch to Condition List " + rBtn.Text + ".\r\n" +
                        "• ( Right-Click ) Export | Import | Clear Condition List " + rBtn.Text + ".\r\n ");
                }
            }

            // Size.
            // -----
            if (Properties.Settings.Default.MainWindowSize.Width != 0)
            {
                this.Size = Properties.Settings.Default.MainWindowSize;
            }

            // Location.
            // ---------
            if (Properties.Settings.Default.MainWindowLocation.X != 0)
            {
                this.Location = Properties.Settings.Default.MainWindowLocation;
            }
            else
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }

            // Theme | Style | Colors.
            // -----------------------

            // Default.
            this.Theme = metroStyleManager.Theme;
            this.Style = metroStyleManager.Style;

            // Overwrite if needed by cfg file.
            this.SetThemeAndStyle();

            captureRegion.BackColor = Color.FromArgb(15, GreenLightColor); // [DEVNOTE] Alpha = between 0 and 255.

            lblCaptureRegion.BackColor = Color.FromArgb(15, GreenLightColor);
            lblCaptureRegion.ForeColor = MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme); //this.BackColor;//

            toolTip.Theme = MetroThemeStyle.Dark;
            toolTip.ShowAlways = true;

            #region Current list.

            // [DEVNOTE] Could add this in a method, though only using it once.
            Color currThemeTextColor = MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme);
            double brightnessFactor = 0.45;
            lblCurrentList.ForeColor = Color.FromArgb(currThemeTextColor.A,
                                                        (int)(currThemeTextColor.R * brightnessFactor), (int)(currThemeTextColor.G * brightnessFactor), (int)(currThemeTextColor.B * brightnessFactor));

            rbList1.Theme = Theme;
            rbList1.Style = Style;

            rbList2.Theme = Theme;
            rbList2.Style = Style;

            rbList3.Theme = Theme;
            rbList3.Style = Style;

            rbList4.Theme = Theme;
            rbList4.Style = Style;

            rbList5.Theme = Theme;
            rbList5.Style = Style;

            #endregion Current list.

            #region Main condition.

            gbConditions.BackColor = this.BackColor;

            lblConditionsNote.UseCustomBackColor = true;
            lblConditionsNote.BackColor = this.BackColor;
            lblConditionsNote.UseCustomForeColor = true;
            lblConditionsNote.ForeColor = RedLightColor; //MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme);

            numAmount.BackColor = this.BackColor;
            numAmount.ForeColor = MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme);

            lblAmount.Theme = metroStyleManager.Theme;

            cbTerms.BackColor = this.BackColor;
            cbTerms.ForeColor = MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme);

            PopulateTermsComboBox();

            btnAddCondition.Theme = metroStyleManager.Theme;
            btnAddCondition.BackColor = this.BackColor;
            btnAddCondition.UseCustomForeColor = true;
            btnAddCondition.ForeColor = GreenLightColor;

            btnAddCondition.Enabled = false; // Disable at start, will only be enabled when a stat to be added is selected.

            // Data Grid (added in v3).
            dgConditions.AutoGenerateColumns = false;

            // Styling.
            dgConditions.BackgroundColor = this.BackColor;
            dgConditions.ForeColor = MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme);

            dgConditions.DefaultCellStyle.BackColor = this.BackColor;
            dgConditions.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgConditions.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            dgConditions.DefaultCellStyle.Padding = new Padding(0, 3, 0, 3);

            // Column setup.
            dgConditions.Columns.Add("Prefix", "Prefix");
            dgConditions.Columns["Prefix"].Width = 8;
            dgConditions.Columns["Prefix"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            dgConditions.Columns.Add("Stats", "Preferred Stat(s)");
            dgConditions.Columns["Stats"].Width = 231;
            dgConditions.Columns["Stats"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            dgConditions.Columns.Add("Remove", "Remove");
            dgConditions.Columns["Remove"].Width = 14;
            dgConditions.Columns["Remove"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            dgConditions.Columns["Remove"].DefaultCellStyle.ForeColor = RedLightColor;

            // Quick Resize Condition Grid.
            dgConditions.Width =
                dgConditions.Columns.Cast<DataGridViewColumn>().Sum(x => x.Width)
                + (dgConditions.RowHeadersVisible ? dgConditions.RowHeadersWidth : 0) + 20; // +20 as a spacer for scrollbar

            PopulateConditionList();

            #endregion Main condition.

            #region SubCondition.

            numSubAmount.BackColor = this.BackColor;
            numSubAmount.ForeColor = MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme);

            lblSubAmount.Theme = metroStyleManager.Theme;

            cbSubTerms.BackColor = this.BackColor;
            cbSubTerms.ForeColor = MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme);

            PopulateSubTermsComboBox();

            #endregion SubCondition.

            #region SubSubCondition.

            numSubSubAmount.BackColor = this.BackColor;
            numSubSubAmount.ForeColor = MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme);

            lblSubSubAmount.Theme = metroStyleManager.Theme;

            cbSubSubTerms.BackColor = this.BackColor;
            cbSubSubTerms.ForeColor = MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme);

            PopulateSubSubTermsComboBox();

            #endregion SubSubCondition.

            #region SubSubSubCondition.

            numSubSubSubAmount.BackColor = this.BackColor;
            numSubSubSubAmount.ForeColor = MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme);

            lblSubSubSubAmount.Theme = metroStyleManager.Theme;

            cbSubSubSubTerms.BackColor = this.BackColor;
            cbSubSubSubTerms.ForeColor = MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme);

            PopulateSubSubSubTermsComboBox();

            #endregion SubSubSubCondition.

            #region Any Amount.

            chkbxAnyAmount.Theme = metroStyleManager.Theme;
            chkbxAnyAmount.Style = metroStyleManager.Style;

            #endregion Any Amount.

            toolTip.SetToolTip(numSubAmount, "\r\nℹ\r\nIgnore white stats.\r\n( f.e. 'Phys. Res.' maximum = 4 )\r\n ");

            // [DEVNOTE] Added mouse hover event based workaround in order to show tooltip longer than default 5 seconds.
            //toolTip.SetToolTip(chkbxAnyAmount, "\r\nℹ\r\nWhen checked:\r\n" +
            //    "✅ Accept any amount\r\nof each of the selected stats\r\n( to be detected at least once ).\r\n" +
            //    "❌ Won't accept if a stat is detected\r\nothers than the ones listed\r\nor when a listed stat is missing.\r\n "
            //    );
        }

        /// <summary>
        /// Populates the Condition List View.
        /// To be used at initialization phase,
        /// since the AddCondition and RemoveCondition methods
        /// will keep it updated afterwards.
        /// </summary>
        private void PopulateConditionList(ConditionList importedList = null)
        {
            ConditionList conditionListEntries = null;

            // If an imported list has been passed as a parameter.
            if (importedList != null)
            {
                conditionListEntries = importedList;
            }
            else
            {
                // (Try) Load from saved settings. Can be an empty list.
                conditionListEntries = LoadConditionList();
            }

            // Populate the condition list.
            dgConditions.Rows.Clear();

            if (conditionListEntries.Count > 0)
            {
                foreach (ConditionListEntry conditionListEntry in conditionListEntries)
                {
                    AddConditionListEntry(conditionListEntry);
                    // AddConditionListEntry will handle saving.
                }
            }
            else
            {
                // ===============================================
                // Store conditions only if an empty imported list
                // has been passed as a parameter.
                // ===============================================
                if (conditionListEntries != null)
                {
                    StoreConditionList();
                }
            }

            // Check labels.
            if (conditionListEntries.Count <= 1)
            {
                gbConditions.Text = "Equipment must have :";
            }
            else
            {
                gbConditions.Text = "Equipment must have (either) :";
            }
            if (conditionListEntries.Count == 0)
            {
                lblConditionsNote.Visible = true;
            }
            else
            {
                lblConditionsNote.Visible = false;
            }
        }

        /// /// <summary>
        /// I know, I know... repetitive code ... refactoring ... a little lethargic atm.
        /// </summary>
        private void PopulateSubSubSubTermsComboBox()
        {
            // Data.
            SortedDictionary<string, string> cbSubSubSubTermsSource = new SortedDictionary<string, string>();

            // Load cfg file containing terms.
            string[] terms = File.ReadAllLines(Tesseract.Ocr.AssemblyCodeBaseDirectory + @"\Config\Stats.cfg");

            // Add a blank dummy.
            cbSubSubSubTermsSource.Add("( optional additional stat )", "");

            // Add stats.
            foreach (string term in terms)
            {
                if (!term.Contains('#')) // Ignore custom comments.
                {
                    // Split by pipe character '|'.
                    string[] splitTerm = term.Split('|');

                    if (splitTerm.Count() == 2)
                    {
                        if (!splitTerm[0].Trim().Contains("*")) // Do not add unique stats as sub conditions.
                        {
                            cbSubSubSubTermsSource.Add(splitTerm[0].Trim(), splitTerm[1].Trim());
                        }
                    }
                }
            }

            // Sort by Long Term.
            cbSubSubSubTermsSource.OrderBy(key => key.Key);

            cbSubSubSubTerms.DataSource = new BindingSource(cbSubSubSubTermsSource, null);
            cbSubSubSubTerms.DisplayMember = "Key";
            cbSubSubSubTerms.ValueMember = "Value";

            cbSubSubSubTerms.SelectedIndex = 0;
        }

        private void PopulateSubSubTermsComboBox()
        {
            // Data.
            SortedDictionary<string, string> cbSubSubTermsSource = new SortedDictionary<string, string>();

            // Load cfg file containing terms.
            string[] terms = File.ReadAllLines(Tesseract.Ocr.AssemblyCodeBaseDirectory + @"\Config\Stats.cfg");

            // Add a blank dummy.
            cbSubSubTermsSource.Add("( optional additional stat )", "");

            // Add stats.
            foreach (string term in terms)
            {
                if (!term.Contains('#')) // Ignore custom comments.
                {
                    // Split by pipe character '|'.
                    string[] splitTerm = term.Split('|');

                    if (splitTerm.Count() == 2)
                    {
                        if (!splitTerm[0].Trim().Contains("*")) // Do not add unique stats as sub conditions.
                        {
                            cbSubSubTermsSource.Add(splitTerm[0].Trim(), splitTerm[1].Trim());
                        }
                    }
                }
            }

            // Sort by Long Term.
            cbSubSubTermsSource.OrderBy(key => key.Key);

            cbSubSubTerms.DataSource = new BindingSource(cbSubSubTermsSource, null);
            cbSubSubTerms.DisplayMember = "Key";
            cbSubSubTerms.ValueMember = "Value";

            cbSubSubTerms.SelectedIndex = 0;
        }

        private void PopulateSubTermsComboBox()
        {
            // Data.
            SortedDictionary<string, string> cbSubTermsSource = new SortedDictionary<string, string>();

            // Load cfg file containing terms.
            string[] terms = File.ReadAllLines(Tesseract.Ocr.AssemblyCodeBaseDirectory + @"\Config\Stats.cfg");

            // Add a blank dummy.
            cbSubTermsSource.Add("( optional additional stat )", "");

            // Add stats.
            foreach (string term in terms)
            {
                if (!term.Contains('#')) // Ignore custom comments.
                {
                    // Split by pipe character '|'.
                    string[] splitTerm = term.Split('|');

                    if (splitTerm.Count() == 2)
                    {
                        if (!splitTerm[0].Trim().Contains("*")) // Do not add unique stats as sub conditions.
                        {
                            cbSubTermsSource.Add(splitTerm[0].Trim(), splitTerm[1].Trim());
                        }
                    }
                }
            }

            // Sort by Long Term.
            cbSubTermsSource.OrderBy(key => key.Key);

            cbSubTerms.DataSource = new BindingSource(cbSubTermsSource, null);
            cbSubTerms.DisplayMember = "Key";
            cbSubTerms.ValueMember = "Value";

            cbSubTerms.SelectedIndex = 0;
        }

        private void PopulateTermsComboBox()
        {
            // Styling.
            cbTerms.BackColor = this.BackColor;

            // Data.
            SortedDictionary<string, string> cbTermsSource = new SortedDictionary<string, string>();

            // Load cfg file containing terms.
            string[] terms = File.ReadAllLines(Tesseract.Ocr.AssemblyCodeBaseDirectory + @"\Config\Stats.cfg");

            // Add a blank dummy.
            cbTermsSource.Add("", "");

            // Add stats.
            foreach (string term in terms)
            {
                if (!term.Contains('#')) // Ignore custom comments.
                {
                    // Split by pipe character '|'.
                    string[] splitTerm = term.Split('|');

                    if (splitTerm.Count() == 2)
                    {
                        cbTermsSource.Add(splitTerm[0].Trim(), splitTerm[1].Trim());
                    }
                }
            }

            // Sort by Long Term.
            cbTermsSource.OrderBy(key => key.Key);

            cbTerms.DataSource = new BindingSource(cbTermsSource, null);
            cbTerms.DisplayMember = "Key";
            cbTerms.ValueMember = "Value";

            cbTerms.SelectedIndex = 0;
        }

        private void SetThemeAndStyle()
        {
            if (File.Exists(Tesseract.Ocr.AssemblyCodeBaseDirectory + @"\Config\ThemeColorStyle.cfg"))
            {
                // Load cfg file containing theme & style settings.
                string[] themeColorStyle = File.ReadAllLines(Tesseract.Ocr.AssemblyCodeBaseDirectory + @"\Config\ThemeColorStyle.cfg");
                foreach (string line in themeColorStyle)
                {
                    if (!line.Contains('#')) // Ignore custom comments.
                    {
                        // Split by pipe character '|'.
                        string[] split = line.Split('|');

                        if (split.Count() == 2)
                        {
                            // Theme.
                            if (split[0].ToLower().Contains("Theme".ToLower()))
                            {
                                if (String.IsNullOrEmpty(split[1].Trim()))
                                    split[1] = "Default";

                                if (Enum.TryParse(split[1].Trim(), out MetroThemeStyle theme))
                                {
                                    this.Theme = theme;
                                    this.metroStyleManager.Theme = theme;
                                }
                            }

                            // Color Style.
                            if (split[0].ToLower().Contains("Style".ToLower()))
                            {
                                if (String.IsNullOrEmpty(split[1].Trim()))
                                    split[1] = "Default";

                                if (Enum.TryParse(split[1].Trim(), out MetroColorStyle style))
                                {
                                    this.Style = style;
                                    this.metroStyleManager.Style = style;
                                }
                            }

                            // Opacity.
                            if (split[0].ToLower().Contains("Opacity".ToLower()))
                            {
                                if (String.IsNullOrEmpty(split[1].Trim()))
                                    split[1] = "85";

                                if (Double.TryParse(split[1].Replace("%", "").Trim(), out double opacity))
                                {
                                    if (opacity >= 15 & opacity <= 100)
                                    {
                                        opacity = opacity / 100; // Convert to double (f.e. 85(%) => 0.85).

                                        this.Opacity = opacity;
                                        InfoGui.Opacity = opacity;
                                    }
                                }
                            }

                            // Custom Text Colors.
                            if (split[0].ToLower().Contains("Green".ToLower()))
                            {
                                if (String.IsNullOrEmpty(split[1].Trim()))
                                    split[1] = "ForestGreen";

                                try
                                {
                                    this.GreenLightColor = Color.FromName(split[1].Trim());
                                }
                                catch
                                {
                                    this.GreenLightColor = Color.ForestGreen;
                                }
                            }
                            if (split[0].ToLower().Contains("Orange".ToLower()))
                            {
                                if (String.IsNullOrEmpty(split[1].Trim()))
                                    split[1] = "Tomato";

                                try
                                {
                                    this.OrangeLightColor = Color.FromName(split[1].Trim());
                                }
                                catch
                                {
                                    this.OrangeLightColor = Color.Tomato;
                                }
                            }
                            if (split[0].ToLower().Contains("Red".ToLower()))
                            {
                                if (String.IsNullOrEmpty(split[1].Trim()))
                                    split[1] = "Firebrick";

                                try
                                {
                                    this.RedLightColor = Color.FromName(split[1].Trim());
                                }
                                catch
                                {
                                    this.RedLightColor = Color.Firebrick;
                                }
                            }
                            if (split[0].ToLower().Contains("Blue Stat".ToLower()))
                            {
                                if (String.IsNullOrEmpty(split[1].Trim()))
                                    split[1] = "RoyalBlue";

                                try
                                {
                                    this.BlueStatColor = Color.FromName(split[1].Trim());
                                }
                                catch
                                {
                                    this.BlueStatColor = Color.RoyalBlue;
                                }
                            }
                            if (split[0].ToLower().Contains("Combo Stat".ToLower()))
                            {
                                if (String.IsNullOrEmpty(split[1].Trim()))
                                    split[1] = "Goldenrod";

                                try
                                {
                                    this.ComboStatColor = Color.FromName(split[1].Trim());
                                }
                                catch
                                {
                                    this.ComboStatColor = Color.Goldenrod;
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion Constructors | Initialization Methods.

        #region General Methods.

        internal void SetCaptureRegion()
        {
            // Recalculate the capture region.
            if (!InfoGui.PreviewCapture)
            {
                // Non-Preview Capture : Right Half Capture only.
                captureRegion.Location = new Point(
                    seeThroughRegion.Location.X + (int)Math.Round((double)(seeThroughRegion.Width / 2)),
                    seeThroughRegion.Location.Y
                );

                captureRegion.Width = (int)Math.Round((double)(seeThroughRegion.Width / 2));
                captureRegion.Height = (int)Math.Round((double)(seeThroughRegion.Height * CaptureRegionHeightClipping));
            }
            else
            {
                // Preview Capture : Full Width Capture.
                captureRegion.Location = new Point(
                       seeThroughRegion.Location.X,
                       seeThroughRegion.Location.Y
                   );

                captureRegion.Width = (int)Math.Round((double)(seeThroughRegion.Width));
                captureRegion.Height = (int)Math.Round((double)(seeThroughRegion.Height * CaptureRegionHeightClipping));
            }

            // Position label.
            lblCaptureRegion.Location = new Point(
                captureRegion.Location.X + 3,
                captureRegion.Location.Y + captureRegion.Height - lblCaptureRegion.Height - 3
            );
        }

        /// <summary>
        /// Used for context menu importing.
        /// </summary>
        private ConditionList ConditionListFromBase64String(string base64string)
        {
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(base64string)))
            {
                BinaryFormatter bf = new BinaryFormatter();
                return (ConditionList)bf.Deserialize(ms);
            }
        }

        /// <summary>
        /// Used for context menu exporting.
        /// </summary>
        private string ConditionListToBase64String(ConditionList currConditionList)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, currConditionList);
                ms.Position = 0;
                byte[] buffer = new byte[(int)ms.Length];
                ms.Read(buffer, 0, buffer.Length);
                return Convert.ToBase64String(buffer);
            }
        }

        private List<ConditionList> ConditionListsFromBase64String(string base64string)
        {
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(base64string)))
            {
                BinaryFormatter bf = new BinaryFormatter();
                return (List<ConditionList>)bf.Deserialize(ms);
            }
        }

        private string ConditionListsToBase64String(List<ConditionList> conditionLists)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, conditionLists);
                ms.Position = 0;
                byte[] buffer = new byte[(int)ms.Length];
                ms.Read(buffer, 0, buffer.Length);
                return Convert.ToBase64String(buffer);
            }
        }

        private ConditionList LoadConditionList()
        {
            // [DEVNOTE] Personal coding safe guard. Won't show to user.
            if ((CurrentConditionListIndex + 1) > MaxConditionLists)
            {
                MessageBox.Show(
                    "[DEVNOTE] Can't load condition list with index " + CurrentConditionListIndex + "." +
                    "\r\nExceeding maximum of " + MaxConditionLists + " lists (-1 for max. index) allowed." +
                    "\r\nReturning an empty condition list."
                    );
                new ConditionList();
            }

            try
            {
                if (!String.IsNullOrEmpty(Properties.Settings.Default.ConditionLists))
                {
                    // Add to ConditionLists container not to lose other container lists, then return requested condition list.
                    ConditionLists = ConditionListsFromBase64String(Properties.Settings.Default.ConditionLists);

                    return ConditionLists[CurrentConditionListIndex];
                }
                else
                {
                    // At first start, add default (empty) condition lists.
                    // ====================================================

                    // Add up to x condition lists to start with.
                    for (int i = 0; i < MaxConditionLists; i++)
                    {
                        ConditionLists.Add(new ConditionList());
                    }

                    return new ConditionList();
                }
            }
            catch
            {
                // Ignore older settings or any other strange exception I may not have played chess with (just in case, don't think it would actually hit).
                return new ConditionList();
            }
        }

        private void StoreConditionList()
        {
            // [DEVNOTE] Personal coding safe guard. Won't show to user. At least hmmm ... shouldn't.
            if ((CurrentConditionListIndex + 1) > MaxConditionLists)
            {
                MessageBox.Show(
                    "[DEVNOTE] Can't store condition list with index " + CurrentConditionListIndex + "." +
                    "\r\nExceeding maximum of " + MaxConditionLists + " lists (-1 for max. index) allowed."
                    );
                return;
            }

            // A. Add to (temp) condition list for storage. Can be empty.
            ConditionList currConditionList = new ConditionList();
            foreach (DataGridViewRow row in dgConditions.Rows)
            {
                currConditionList.Add((ConditionListEntry)row.Tag);
            }

            // Update condition lists container at current condition list index.
            ConditionLists[CurrentConditionListIndex] = currConditionList;

            // B. Store all condition lists (shown + not shown).
            Properties.Settings.Default.ConditionLists = ConditionListsToBase64String(ConditionLists);
            Properties.Settings.Default.Save();
        }

        private void DrawGroupBox(GroupBox box, Graphics g, Color textColor, Color borderColor, bool dotted = false)
        {
            if (box != null)
            {
                Brush textBrush = new SolidBrush(textColor);
                Brush borderBrush = new SolidBrush(borderColor);
                Pen borderPen = new Pen(borderBrush);
                if (dotted)
                {
                    borderPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                }
                SizeF strSize = g.MeasureString(box.Text, box.Font);
                Rectangle rect = new Rectangle(box.ClientRectangle.X,
                                               box.ClientRectangle.Y + (int)(strSize.Height / 2),
                                               box.ClientRectangle.Width - 1,
                                               box.ClientRectangle.Height - (int)(strSize.Height / 2) - 1);

                // Clear text and border.
                g.Clear(this.BackColor);

                // Text.
                g.DrawString(box.Text, box.Font, textBrush, box.Padding.Left, 0);

                // Border.
                // Left.
                g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
                // Right.
                g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                // Bottom.
                g.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                // Top1.
                g.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));
                // Top2.
                g.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));
            }
        }

        private void AddConditionListEntry(ConditionListEntry conditionListEntry)
        {
            // ====================
            // A. Preparation work.
            // ====================

            // Descriptive part.
            string longTermEntry = "";

            for (int i = 0; i < conditionListEntry.Count(); i++)
            {
                string currentStatTerm = "";

                // =====================
                // a. Fixed Amount Mode.
                // =====================
                if (conditionListEntry[i].Amount > 0)
                {
                    if (i == 0)
                    {
                        currentStatTerm += "[  fixed amount  |  any other stat allowed  ]\r\n";

                        currentStatTerm += "  • min. ";
                    }
                    else
                    {
                        currentStatTerm += "\r\n  & min. ";
                    }

                    currentStatTerm += conditionListEntry[i].Amount.ToString() + " x ";
                }

                // ===================
                // b. Any Amount Mode.
                // ===================
                if (conditionListEntry[i].Amount == 0)
                {
                    if (i == 0)
                    {
                        currentStatTerm += "[  any combination  |  no other stat allowed  ]\r\n";

                        currentStatTerm += "  • min. 1 x ";
                    }
                    else
                    {
                        currentStatTerm += "\r\n  & min. 1 x ";
                    }
                }

                // ===================================================
                // c. Add full stat string (applicable to both modes).
                // ===================================================
                currentStatTerm += conditionListEntry[i].LongTerm;

                // =========================================
                // d. Clean up 'min.' part if not necessary.
                // =========================================

                // Unique stats (can have only 1, so no 'min.').
                if (
                    currentStatTerm.ToLower().Contains("Purify".ToLower()) ||
                    currentStatTerm.ToLower().Contains("Frenzy".ToLower()) ||
                    currentStatTerm.ToLower().Contains("Square".ToLower()) ||
                    currentStatTerm.ToLower().Contains("Shatter".ToLower()) ||
                    currentStatTerm.ToLower().Contains("Blackhole".ToLower())
                    )
                {
                    currentStatTerm = currentStatTerm.Replace(" min. ", "");
                }

                // Already maxed stats (can't have more, so no 'min.').
                if (
                    currentStatTerm.ToLower().Contains("Atk".ToLower()) ||
                    currentStatTerm.ToLower().Contains("Def.".ToLower())
                    )
                {
                    if (conditionListEntry[i].Amount == 5)
                    {
                        currentStatTerm = currentStatTerm.Replace(" min. ", "");
                    }
                }
                else
                {
                    if (conditionListEntry[i].Amount == 4)
                    {
                        currentStatTerm = currentStatTerm.Replace(" min. ", "");
                    }
                }

                // ======================================================
                // e. Append current stat term string to long term entry.
                // ======================================================
                longTermEntry += currentStatTerm;
            }

            // ==========================================================================
            // ( Additionally, )
            // Also edit the title if needed (will only apply to fixed amount entries,
            // not the combo-ed ones, which are already limited to max only given stats).
            // ==========================================================================
            if (conditionListEntry.Count == 1)
            {
                if (
                    conditionListEntry[0].Amount == 5 &
                    (conditionListEntry[0].LongTerm.ToLower().Contains("Atk".ToLower()) || conditionListEntry[0].LongTerm.ToLower().Contains("Def.".ToLower()))
                    )
                {
                    longTermEntry = longTermEntry.Replace("any other stat allowed", "no other stat possible");
                }
            }
            else
            {
                // [DEVNOTE] f.e. 4 x Channelling => Can't know in advance if it's for a weapon or for an armor piece, hence max at 5.
                if (conditionListEntry.Sum(condition => condition.Amount) == 5)
                {
                    longTermEntry = longTermEntry.Replace("any other stat allowed", "no other stat possible");
                }
            }

            // ====================
            // B. Add to data grid.
            // ====================
            string[] rowContents = new string[] { "", longTermEntry, "✖" }; // [DEVNOTE] amountEntry|prefix deprecated during development. [TODO] Remove first unused 0px width column some time.

            int addedRowIndex = dgConditions.Rows.Add(rowContents);

            // Attach the full condition entry to the added row.
            dgConditions.Rows[addedRowIndex].Tag = conditionListEntry;

            // Color cells based on amount mode.
            if (conditionListEntry[0].Amount > 0)
            {
                //dgConditions.Rows[addedRowIndex].Cells[dgConditions.Columns["Prefix"].Index].Style.ForeColor = BlueStatColor;
                dgConditions.Rows[addedRowIndex].Cells[dgConditions.Columns["Stats"].Index].Style.ForeColor = BlueStatColor;
            }
            if (conditionListEntry[0].Amount == 0)
            {
                //dgConditions.Rows[addedRowIndex].Cells[dgConditions.Columns["Prefix"].Index].Style.ForeColor = ComboStatColor;
                dgConditions.Rows[addedRowIndex].Cells[dgConditions.Columns["Stats"].Index].Style.ForeColor = ComboStatColor;
            }

            // ====================
            // C. Store conditions.
            // ====================
            StoreConditionList();

            // ====================
            // D. Check/Set labels.
            // ====================
            if (dgConditions.Rows.Count <= 1)
            {
                gbConditions.Text = "Equipment must have :";
            }
            else
            {
                gbConditions.Text = "Equipment must have (either) :";
            }
            if (dgConditions.Rows.Count == 0)
            {
                lblConditionsNote.Visible = true;
            }
            else
            {
                lblConditionsNote.Visible = false;
            }
        }

        private void RemoveConditionListEntry(int rowIndex)
        {
            dgConditions.Rows.RemoveAt(rowIndex);

            StoreConditionList();

            // Check labels.
            if (dgConditions.Rows.Count <= 1)
            {
                gbConditions.Text = "Equipment must have :";
            }
            else
            {
                gbConditions.Text = "Equipment must have (either) :";
            }
            if (dgConditions.Rows.Count == 0)
            {
                lblConditionsNote.Visible = true;
            }
            else
            {
                lblConditionsNote.Visible = false;
            }
        }

        #endregion General Methods.

        #endregion Methods.

        #region Events.

        private void MainGui_Shown(object sender, EventArgs e)
        {
            CheckAdminPrivileges();
        }

        private void btnAddCondition_Click(object sender, EventArgs e)
        {
            // Unfocus.
            lblAmount.Focus();

            #region A. Main Condition.

            // Return if blank dummy is being added.
            if (cbTerms.SelectedIndex == 0)
                return;

            // Get selected KVP from combo box.
            KeyValuePair<string, string> selectedMainConditionEntry
                = (KeyValuePair<string, string>)cbTerms.SelectedItem;

            // Create a main condition.
            Condition mainCondition = new Condition((int)numAmount.Value, selectedMainConditionEntry.Key.Replace("*", "").Trim(), selectedMainConditionEntry.Value);

            #endregion A. Main Condition.

            #region B. (Optional) Additional Condition.

            Condition additionalCondition = null;
            if (cbSubTerms.SelectedIndex != 0)
            {
                // Get selected KVP from combo box.
                KeyValuePair<string, string> selectedAdditionalConditionEntry
                = (KeyValuePair<string, string>)cbSubTerms.SelectedItem;

                // Create an additional condition.
                additionalCondition = new Condition((int)numSubAmount.Value, selectedAdditionalConditionEntry.Key.Replace("*", "").Trim(), selectedAdditionalConditionEntry.Value);
            }

            #endregion B. (Optional) Additional Condition.

            #region C. (Optional) Second Additional Condition.

            Condition additionalCondition2 = null;
            if (cbSubSubTerms.SelectedIndex != 0)
            {
                // Get selected KVP from combo box.
                KeyValuePair<string, string> selectedAdditionalConditionEntry2
                = (KeyValuePair<string, string>)cbSubSubTerms.SelectedItem;

                // Create a second additional condition.
                additionalCondition2 = new Condition((int)numSubSubAmount.Value, selectedAdditionalConditionEntry2.Key.Replace("*", "").Trim(), selectedAdditionalConditionEntry2.Value);
            }

            #endregion C. (Optional) Second Additional Condition.

            #region D. (Optional) Third Additional Condition.

            Condition additionalCondition3 = null;
            if (cbSubSubSubTerms.SelectedIndex != 0)
            {
                // Get selected KVP from combo box.
                KeyValuePair<string, string> selectedAdditionalConditionEntry3
                = (KeyValuePair<string, string>)cbSubSubSubTerms.SelectedItem;

                // Create a third additional condition.
                additionalCondition3 = new Condition((int)numSubSubSubAmount.Value, selectedAdditionalConditionEntry3.Key.Replace("*", "").Trim(), selectedAdditionalConditionEntry3.Value);
            }

            #endregion D. (Optional) Third Additional Condition.

            #region E. Set up list condition entry.

            ConditionListEntry conditionListEntry = new ConditionListEntry();

            conditionListEntry.Add(mainCondition);

            if (additionalCondition != null)
            {
                conditionListEntry.Add(additionalCondition);
            }

            if (additionalCondition2 != null)
            {
                conditionListEntry.Add(additionalCondition2);
            }

            if (additionalCondition3 != null)
            {
                conditionListEntry.Add(additionalCondition3);
            }

            #endregion E. Set up list condition entry.

            #region F. Combo mode option.

            if (chkbxAnyAmount.Checked)
            {
                foreach (Condition condition in conditionListEntry)
                {
                    // Set to 0 ( unique value | can't ever be selected by user ).
                    condition.Amount = 0;
                }
            }

            #endregion F. Combo mode option.

            #region G. User entry checks.

            // [DEVNOTE] The order of these checks is important (for certain steps).

            // ========================================================================
            // 1. Always set amount to 1x for unique stats Purify Spell, God of Frenzy,
            //    Square Formation, Soul Shatter, Spirit Blackhole, ...
            //    ( except when in combo mode )
            // ========================================================================
            // Only main condition can have a unique stat (option not available for additional stat).

            #region User entry check 1.

            // Leave at zero amount value when in combo mode.
            if (!chkbxAnyAmount.Checked)
            {
                if (
                        conditionListEntry[0].LongTerm.ToLower().Contains("Purify".ToLower()) ||
                        conditionListEntry[0].LongTerm.ToLower().Contains("Frenzy".ToLower()) ||
                        conditionListEntry[0].LongTerm.ToLower().Contains("Square".ToLower()) ||
                        conditionListEntry[0].LongTerm.ToLower().Contains("Shatter".ToLower()) ||
                        conditionListEntry[0].LongTerm.ToLower().Contains("Blackhole".ToLower())
                        )
                {
                    conditionListEntry[0].Amount = 1;
                }
            }

            #endregion User entry check 1.

            // ===========================================
            // 2. If stats are equal, merge into one stat.
            // ===========================================

            #region User entry check 2.

            ConditionListEntry mergedConditionListEntry = new ConditionListEntry();

            if (conditionListEntry.Count > 1)
            {
                for (int i = 0; i < conditionListEntry.Count; i++)
                {
                    // Prepare a temp condition. To be check if updated and/or added later on.
                    Condition tmpCondition = new Condition(conditionListEntry[i].Amount, conditionListEntry[i].LongTerm, conditionListEntry[i].ShortTerm);

                    for (int ii = 0; ii < conditionListEntry.Count; ii++)
                    {
                        // Logic-wise, only compare conditions after current looped condition i.
                        // Any amount of an equal condition before current condition in list, would already have been merged.
                        if (ii > i)
                        {
                            if (tmpCondition.LongTerm == conditionListEntry[ii].LongTerm)
                            {
                                // Update temp condition amount.
                                tmpCondition.Amount += conditionListEntry[ii].Amount;
                            }
                        }
                    }

                    // Check whether to add temp condition (if not already in merged condition list).
                    bool isUniqueCondition = true;

                    foreach (Condition condition in mergedConditionListEntry)
                    {
                        if (tmpCondition.LongTerm == condition.LongTerm)
                        {
                            isUniqueCondition = false;
                            break;
                        }
                    }

                    if (isUniqueCondition)
                        mergedConditionListEntry.Add(tmpCondition);
                }

                conditionListEntry = mergedConditionListEntry;
            }

            #endregion User entry check 2.

            // ==============================================
            // 3. Check individual and total amount of stats.
            // ==============================================

            #region User entry check 3.

            int totalStatAmount = 0;

            // 3-1. Individual stat amount (main & additional if applicable).
            // --------------------------------------------------------------
            for (int i = 0; i < conditionListEntry.Count; i++)
            {
                // [DEVNOTE] Afaik, only Attack and Defense Levels can have at max 5 equal stats.
                if (
                    conditionListEntry[i].ShortTerm.ToLower().Contains("Atk".ToLower()) ||
                    conditionListEntry[i].ShortTerm.ToLower().Contains("Def.".ToLower())
                    )
                {
                    if (conditionListEntry[i].Amount > 5)
                    {
                        conditionListEntry[i].Amount = 5;
                    }
                }
                else
                {
                    if (conditionListEntry[i].Amount > 4)
                    {
                        conditionListEntry[i].Amount = 4;
                    }
                }

                totalStatAmount += conditionListEntry[i].Amount;
            }

            // 3-2. Total stat amount (combined amount of main & additional).
            // --------------------------------------------------------------
            if (conditionListEntry.Count > 1)
            {
                // [DEVNOTE] No check in advance on type of equipment (max. 4 for armor or max. 5 = weapon ),
                //           hence defaulting to 5 and hoping on a little goodwill/thought from the user.
                if (totalStatAmount > 5)
                {
                    // Show warning to user.
                    MetroMessageBox.Show(this,
                                "The combined amount of preferred stats (" + totalStatAmount.ToString() + ") exceeds the maximum amount of (blue) stats." + Environment.NewLine +
                                "( weapon = max. 5 stats | armor = max. 4 stats )" + Environment.NewLine +
                                "Please check and correct the amount of each stat before adding it to the list.",
                                "", // Warning
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // Return (don't add).
                    return;
                }
            }

            #endregion User entry check 3.

            // ==========================================================
            // 4. Check if a similar condition hasn't already been added.
            // ==========================================================

            #region User entry check 4.

            // 4-1. ( Temporarily ) Order current to be entered condition list.
            //      ( Alphabetically by Short Term. )
            //      ( To be matched in next step. )
            // ----------------------------------------------------------------
            List<Condition> orderedConditionListEntry = conditionListEntry.OrderBy(o => o.ShortTerm).ToList();

            bool uniqueEntry = true;

            // 4-2. Check ordered condition list entry against
            //      each ordered condition list entry in data grid rows.
            // ---------------------------------------------------------
            foreach (DataGridViewRow row in dgConditions.Rows)
            {
                // Only if amount of contained conditions are equal.
                if (((ConditionListEntry)row.Tag).Count == orderedConditionListEntry.Count)
                {
                    // Alphabetically order the current data grid row condition list as well.
                    List<Condition> currDataGridRowConditionList = ((ConditionListEntry)row.Tag).OrderBy(o => o.ShortTerm).ToList();

                    // In case both entry and current row condition(s) match, a similar entry is found => do not consider it a unique entry.
                    bool entryAndRowConditionListMatching = true;
                    for (int i = 0; i < orderedConditionListEntry.Count; i++)
                    {
                        if (
                            orderedConditionListEntry[i].Amount != currDataGridRowConditionList[i].Amount ||
                            orderedConditionListEntry[i].ShortTerm != currDataGridRowConditionList[i].ShortTerm
                          )
                        {
                            entryAndRowConditionListMatching = false;
                        }
                    }

                    // Break data grid row condition loop if a similar entry has been found.
                    if (entryAndRowConditionListMatching)
                    {
                        uniqueEntry = false;
                        break;
                    }
                }
            }

            // If not a unique entry, notify user and don't add.
            if (!uniqueEntry)
            {
                MetroMessageBox.Show(this,
                                "A similar condition has already been added.",
                                "", // Note
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            #endregion User entry check 4.

            // =======================================================
            // 5. [DEVNOTE] [POSSIBLE TODO] Additional possible check:
            // =======================================================
            //              f.e. - already in list:   [ (minimum) 2 x Int & (minimum) 2 x Rdmt ]
            //                   - user wants to add: [ (minimum) 1 x Int & (minimum) 2 x Rdmt ] => would make earlier entry unnecessary.
            // Hoping on a little goodwill/thought from the user, leaving unimplemented for now,
            // since not harmful to have both (would only be additional clutter).

            #region User entry check 5 (unimplemented).

            // Not implemented.

            #endregion User entry check 5 (unimplemented).

            // ==============================================
            // 6. If the above boiled down to 1 stat only and
            //    the user has checked any amount of stats,
            //    max out the boiled down stat.
            //    [DEVNOTE] Since, using the condition logic:
            //    f.e.     (Combo) min. 1x Int ( and only int, no other stat allowed )
            //    translates to => min. 4x Int
            // ==============================================

            #region User entry check 6.

            if (
                conditionListEntry.Count == 1 &
                chkbxAnyAmount.Checked
                )
            {
                // [DEVNOTE] Afaik, only Attack and Defense Levels can have at max 5 equal stats.
                if (
                    conditionListEntry[0].ShortTerm.ToLower().Contains("Atk".ToLower()) ||
                    conditionListEntry[0].ShortTerm.ToLower().Contains("Def.".ToLower())
                    )
                {
                    conditionListEntry[0].Amount = 5;
                }
                else
                {
                    conditionListEntry[0].Amount = 4;
                }
            }

            #endregion User entry check 6.

            #endregion G. User entry checks.

            #region H. Add condition list entry.

            AddConditionListEntry(conditionListEntry);

            #endregion H. Add condition list entry.

            #region I. Reset UI controls to default.

            // Reset condition entry.
            numAmount.Value = 1;
            cbTerms.SelectedIndex = 0;

            // Reset first additional condition entry.
            numSubAmount.Value = 1;
            cbSubTerms.SelectedIndex = 0;

            // Reset second additional condition entry.
            numSubSubAmount.Value = 1;
            cbSubSubTerms.SelectedIndex = 0;

            // Reset third additional condition entry.
            numSubSubSubAmount.Value = 1;
            cbSubSubSubTerms.SelectedIndex = 0;

            // Uncheck & hide any amount checkbox.
            chkbxAnyAmount.Checked = false; // [DEVNOTE] Will trigger the combo box resizings.
            chkbxAnyAmount.Visible = false;

            #endregion I. Reset UI controls to default.
        }

        private void cbSubSubSubTerms_DropDownClosed(object sender, EventArgs e)
        {
            lblAmount.Focus();
        }

        private void cbSubSubSubTerms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSubSubTerms.SelectedIndex != 0)
            {
                if (cbSubSubSubTerms.SelectedIndex == 0)
                {
                    btnAddCondition.Height = 75;

                    numSubSubSubAmount.Value = 1;
                }
                else
                {
                    btnAddCondition.Height = 102;
                }
            }
        }

        private void cbSubSubTerms_DropDownClosed(object sender, EventArgs e)
        {
            lblAmount.Focus();
        }

        private void cbSubSubTerms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSubTerms.SelectedIndex != 0)
            {
                if (cbSubSubTerms.SelectedIndex == 0)
                {
                    btnAddCondition.Height = 48;

                    numSubSubSubAmount.Visible = false;
                    lblSubSubSubAmount.Visible = false;
                    cbSubSubSubTerms.Visible = false;

                    try // [DEVNOTE] Avoid initialization phase exceptions.
                    {
                        cbSubSubSubTerms.SelectedIndex = 0;
                        numSubSubAmount.Value = 1;
                        numSubSubSubAmount.Value = 1;
                    }
                    catch { }

                    chkbxAnyAmount.Location = new Point(chkbxAnyAmount.Location.X, numSubSubAmount.Location.Y + 26);
                }
                else
                {
                    btnAddCondition.Height = 75;

                    numSubSubSubAmount.Visible = true;
                    lblSubSubSubAmount.Visible = true;
                    cbSubSubSubTerms.Visible = true;

                    chkbxAnyAmount.Location = new Point(chkbxAnyAmount.Location.X, numSubSubSubAmount.Location.Y + 26);
                }
            }
        }

        private void cbSubTerms_DropDownClosed(object sender, EventArgs e)
        {
            lblSubAmount.Focus();
        }

        private void cbSubTerms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSubTerms.SelectedIndex == 0)
            {
                btnAddCondition.Height = 21;

                numSubSubAmount.Visible = false;
                numSubSubSubAmount.Visible = false;
                lblSubSubAmount.Visible = false;
                lblSubSubSubAmount.Visible = false;
                cbSubSubTerms.Visible = false;
                cbSubSubSubTerms.Visible = false;

                try // [DEVNOTE] Avoid initialization phase exceptions.
                {
                    cbSubSubTerms.SelectedIndex = 0;
                    cbSubSubSubTerms.SelectedIndex = 0;
                    numSubAmount.Value = 1;
                    numSubSubAmount.Value = 1;
                    numSubSubSubAmount.Value = 1;
                }
                catch { }

                chkbxAnyAmount.Visible = false;
                chkbxAnyAmount.Checked = false; // [DEVNOTE] Will trigger the combo box resizings.

                chkbxAnyAmount.Location = new Point(chkbxAnyAmount.Location.X, numSubAmount.Location.Y + 26);
            }
            else
            {
                btnAddCondition.Height = 48;

                numSubSubAmount.Visible = true;
                lblSubSubAmount.Visible = true;
                cbSubSubTerms.Visible = true;

                chkbxAnyAmount.Visible = true;

                chkbxAnyAmount.Location = new Point(chkbxAnyAmount.Location.X, numSubSubAmount.Location.Y + 26);
            }
        }

        private void cbTerms_DropDownClosed(object sender, EventArgs e)
        {
            lblAmount.Focus();
        }

        private void cbTerms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTerms.SelectedIndex == 0)
            {
                numSubAmount.Visible = false;
                lblSubAmount.Visible = false;
                cbSubTerms.Visible = false;

                try // [DEVNOTE] Avoid initialization phase exceptions.
                {
                    cbSubTerms.SelectedIndex = 0;
                    cbSubSubTerms.SelectedIndex = 0;
                    cbSubSubSubTerms.SelectedIndex = 0;
                    numAmount.Value = 1;
                    numSubAmount.Value = 1;
                    numSubSubAmount.Value = 1;
                    numSubSubSubAmount.Value = 1;
                }
                catch { }

                btnAddCondition.Enabled = false;
            }
            else
            {
                numSubAmount.Visible = true;
                lblSubAmount.Visible = true;
                cbSubTerms.Visible = true;

                btnAddCondition.Enabled = true;
            }
        }

        private void chkbxAnyAmount_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbxAnyAmount.Checked)
            {
                chkbxAnyAmount.Text = chkbxAnyAmount.Text.Replace(" ?", "");

                cbTerms.Location = new Point(cbTerms.Location.X - 51, cbTerms.Location.Y);
                cbSubTerms.Location = new Point(cbSubTerms.Location.X - 51, cbSubTerms.Location.Y);
                cbSubSubTerms.Location = new Point(cbSubSubTerms.Location.X - 51, cbSubSubTerms.Location.Y);
                cbSubSubSubTerms.Location = new Point(cbSubSubSubTerms.Location.X - 51, cbSubSubSubTerms.Location.Y);

                cbTerms.Size = new Size(cbTerms.Width + 51, cbTerms.Height);
                cbSubTerms.Size = new Size(cbSubTerms.Width + 51, cbSubTerms.Height);
                cbSubSubTerms.Size = new Size(cbSubSubTerms.Width + 51, cbSubSubTerms.Height);
                cbSubSubSubTerms.Size = new Size(cbSubSubSubTerms.Width + 51, cbSubSubSubTerms.Height);

                cbTerms.Refresh();
                cbSubTerms.Refresh();
                cbSubSubTerms.Refresh();
                cbSubSubSubTerms.Refresh();
            }
            else
            {
                chkbxAnyAmount.Text = chkbxAnyAmount.Text + " ?";

                cbTerms.Location = new Point(cbTerms.Location.X + 51, cbTerms.Location.Y);
                cbSubTerms.Location = new Point(cbSubTerms.Location.X + 51, cbSubTerms.Location.Y);
                cbSubSubTerms.Location = new Point(cbSubSubTerms.Location.X + 51, cbSubSubTerms.Location.Y);
                cbSubSubSubTerms.Location = new Point(cbSubSubSubTerms.Location.X + 51, cbSubSubSubTerms.Location.Y);

                cbTerms.Size = new Size(cbTerms.Width - 51, cbTerms.Height);
                cbSubTerms.Size = new Size(cbSubTerms.Width - 51, cbSubTerms.Height);
                cbSubSubTerms.Size = new Size(cbSubSubTerms.Width - 51, cbSubSubTerms.Height);
                cbSubSubSubTerms.Size = new Size(cbSubSubSubTerms.Width - 51, cbSubSubSubTerms.Height);

                cbTerms.Refresh();
                cbSubTerms.Refresh();
                cbSubSubTerms.Refresh();
                cbSubSubSubTerms.Refresh();
            }
        }

        private void dgConditions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Remove entry (if applicable).
            // =============================

            // Clicked row index number.
            int clickedRowIndex = Convert.ToInt32(e.RowIndex.ToString());

            // Clicked column index number.
            int clickedColIndex = Convert.ToInt32(e.ColumnIndex.ToString());

            // Only delete the clicked row when clicked in the remove column.
            if (clickedColIndex == dgConditions.Columns["Remove"].Index)
            {
                RemoveConditionListEntry(clickedRowIndex);
            }
        }

        private void dgConditions_SelectionChanged(object sender, EventArgs e)
        {
            dgConditions.ClearSelection();
        }

        private void gbDotted_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;

            /*Color color = MetroFramework.Drawing.MetroPaint.GetStyleColor(MetroColorStyle.Orange);
            DrawGroupBox(box, e.Graphics, textColor: color, borderColor: color);*/

            DrawGroupBox(box, e.Graphics,
                textColor: MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme),
                borderColor: MetroFramework.Drawing.MetroPaint.GetStyleColor(this.Style),
                dotted: true
                );
        }

        private void MainGui_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Directory.Exists(Tesseract.Ocr.TempCacheDirectory))
                Directory.Delete(Tesseract.Ocr.TempCacheDirectory, recursive: true); // Delete Tesseract's temp cache folder & its contents.

            if (Ocr_CancellationTokenSource != null)
                Ocr_CancellationTokenSource.Cancel();
        }

        private void MainGui_LocationChanged(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.Manual;

            // Recalculate InfoGui size & position (will only apply when forms are chained together).
            this.SetInfoGuiSizeAndPosition();

            if (this.WindowState != FormWindowState.Minimized)
            {
                // Store the new main window location.
                // The info window location is stored in the InfoGui LocationChanged method.
                Properties.Settings.Default.MainWindowLocation = this.Location;
                Properties.Settings.Default.Save();
            }
        }

        private void MainGui_Resize(object sender, EventArgs e)
        {
            this.SetInfoGuiSizeAndPosition();
        }

        private void MainGui_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                this.SetCaptureRegion();

                // Store the new main window size.
                // The info window size is stored in the InfoGui SizeChanged method.
                Properties.Settings.Default.MainWindowSize = this.Size;
                Properties.Settings.Default.Save();

                // (Force) Set info form top-most.
                InfoGui.TopMost = true;
            }
        }

        // Wire all radio button click events into this.
        // [DEVNOTE] Added to click in order only to only trigger when clicked, not at f.e. form load.
        private void AllRadioButtons_Click(Object sender, EventArgs e)
        {
            MouseEventArgs meArgs = e as MouseEventArgs; // C# TryCast

            if (meArgs != null)
            {
                RadioButton clickedRb = (RadioButton)sender;

                // ====================
                // Left click behavior.
                // ====================
                if (meArgs.Button == MouseButtons.Left)
                {
                    // Check if the raiser of the event is a checked Radio Button.
                    if (clickedRb.Checked)
                    {
                        // Get text value from checked control & (try) parse it to int.
                        if (Int32.TryParse(clickedRb.Text, out int selectedConditionListIndex))
                        {
                            // [DEVNOTE] [IMPORTANT] If f.e. radio button value = 1, then index will be 0 ( = 1 - 1 )
                            CurrentConditionListIndex = --selectedConditionListIndex;
                        }
                        else
                        {
                            CurrentConditionListIndex = 0; // Set to first condition list.
                        }

                        // Save the current selected index.
                        Properties.Settings.Default.CurrentConditionListIndex = CurrentConditionListIndex;
                        Properties.Settings.Default.Save();

                        // Reload conditions into data grid.
                        // This will use the updated CurrentConditionListIndex.
                        PopulateConditionList();
                    }
                }

                // =====================
                // Right click behavior.
                // =====================
                // [DEVNOTE] Moved to MouseUp.
                //  Reason:  Right-click won't raise a click event.
                // ---------------------
            }
        }

        // Wire all radio button (right) click events into this.
        private void AllRadioButtons_MouseUp(object sender, MouseEventArgs e)
        {
            // ====================================
            // Right click behavior (radio button).
            // ====================================
            if (e.Button == MouseButtons.Right)
            {
                RadioButton rClickedRb = (RadioButton)sender;
                int clickedConditionListIndex = Int32.Parse(rClickedRb.Text) - 1;

                // Update the toolstrip items.
                cmExportImport.Items[0].Text = "⮝ Copy / Export [ Condition List " + rClickedRb.Text + " ]";
                cmExportImport.Items[0].Tag = clickedConditionListIndex; // Tag condition list index to be exported.

                cmExportImport.Items[1].Text = "⮟ Paste / Import [ Condition List " + rClickedRb.Text + " ]";
                cmExportImport.Items[1].Tag = clickedConditionListIndex; // Tag condition list index to be imported.

                cmExportImport.Items[3].Text = "❌ Clear [ Condition List " + rClickedRb.Text + " ]";
                cmExportImport.Items[3].Tag = clickedConditionListIndex; // Tag condition list index to be cleared of all entries.

                if (ConditionLists[clickedConditionListIndex].Count == 0)
                {
                    // Disable export if to be exported condition list is empty.
                    cmExportImport.Items[0].Visible = false;

                    // Disable clear.
                    cmExportImport.Items[2].Visible = false;
                    cmExportImport.Items[3].Visible = false;
                }
                else
                {
                    // (Re)Enable export if to be exported condition list isn't empty.
                    cmExportImport.Items[0].Visible = true;

                    // (Re)Enable clear.
                    cmExportImport.Items[2].Visible = true;
                    cmExportImport.Items[3].Visible = true;
                }

                // Show the context menu.
                cmExportImport.Show(rClickedRb, rClickedRb.Width - 22, rClickedRb.Height + 5); // e.Location.X, e.Location.Y
            }
        }

        // Allowing right clicking the data grid as well to show the context menu in order to save/load current condition
        // by tricking it as a radio button sender.
        private void DataGrid_MouseUp(object sender, MouseEventArgs e)
        {
            // =================================
            // Right click behavior (data grid).
            // =================================
            if (e.Button == MouseButtons.Right)
            {
                RadioButton fakeRadioButton = this.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked); // Using LINQ.

                AllRadioButtons_MouseUp(
                    sender: fakeRadioButton,
                    e: new MouseEventArgs(
                        MouseButtons.Right, 1,
                        e.Location.X, //e.Location.X - 165,
                        e.Location.Y, //e.Location.Y + 50,
                        0)
                    );

                //MessageBox.Show(fakeRadioButton.Location.X.ToString() + " " + dgConditions.Location.X.ToString() + " | " + (fakeRadioButton.Location.X - dgConditions.Location.X).ToString());
                //int var1 = PointToScreen(fakeRadioButton.Location).X;
                //int var2 = PointToScreen(e.Location).X;
                //MessageBox.Show(var1.ToString() + " - " + var2 + " = " + (var1 - var2).ToString());
            }

            // Additionally, clear selection to remove dotted selection borders.
            dgConditions.ClearSelection();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // The export toolstrip tag contains right clicked condition list index.
            int rightClickedConditionListIndex = (int)cmExportImport.Items[0].Tag;

            // Copy requested condition list to clipboard as a base64 string.
            Clipboard.SetText(ConditionListToBase64String(ConditionLists[rightClickedConditionListIndex]));

            // Notify user.
            MetroMessageBox.Show(this,
                        "[ Condition List " + (rightClickedConditionListIndex + 1) + " ] has been copied/exported to the clipboard.\n\n" +
                        "Paste anywhere to share and/or ( use the copied text to )\nimport as/overwrite a condition list.",
                        "", // Note
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // In case the user cancels the input process,
            // provide a way to return to current list.
            RadioButton selectedBtnBeforeImport = null;
            selectedBtnBeforeImport = this.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked); // Using LINQ.
            // [DEVNOTE] Set the above condition list index as current list index.
            //           = Handled by forced click event further on.

            // The import toolstrip tag contains right clicked condition list index.
            int rightClickedConditionListIndex = (int)cmExportImport.Items[1].Tag;

            // Move to the condition list that needs updating
            // by triggering a radio button click corresponding to that list.
            foreach (RadioButton rBtn in this.Controls.OfType<RadioButton>())
            {
                if (rBtn.Text.Contains((rightClickedConditionListIndex + 1).ToString())) // [DEVNOTE] Needs translation to human readable string.
                {
                    // Check the right clicked radio button.
                    // [DEVNOTE] Didn't think this was necessary with the forced click below,
                    //           though apparently it is.
                    rBtn.Checked = true;

                    // Programmatically (left-)click the radio button corresponding to the list to be updated,
                    // triggering a focus on that list.
                    AllRadioButtons_Click(rBtn, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));

                    break;
                }
            }

            // Avoid showing the input box behind current form being topmost.
            bool topMostBackup = this.TopMost;
            this.TopMost = false;

            // Set the default 'DefaultResponse' text.
            string defaultResponse = "(  paste here using  ⌨ 【 CTRL 】+【 V 】  or  🖰  Right-Click > Paste  )";

            string clipboardText = null;

            // Check clipboard to see if it contains text.
            if (Clipboard.ContainsText(TextDataFormat.Text))
            {
                clipboardText = Clipboard.GetText(TextDataFormat.Text).Trim();

                // Check if the current clipboard text happens to be a valid condition list.
                // If so, prefill the DefaultResponse textbox.
                ConditionList clipboardListCondition = null;

                try
                {
                    clipboardListCondition = ConditionListFromBase64String(clipboardText.Trim());
                }
                catch { }

                // If clipboard text corresponds to a valid condition list.
                if (clipboardListCondition != null)
                {
                    clipboardText = clipboardText.Trim();
                }
                //else
                //{
                //    clipboardText = null;
                //}
            }

            // Request user response.
            // Show input box at least once.
            string userResponse = defaultResponse;

            while (userResponse == defaultResponse)
            {
                // [DEVNOTE] Ahum ... "loaning" input box from VB.NET's default libraries ...
                userResponse = Microsoft.VisualBasic.Interaction.InputBox(
                    Prompt: "Paste the copied/clipboard text down below\r\n" +
                            " in order to import/overwrite [ Condition List " + (rightClickedConditionListIndex + 1).ToString() + " ] .",
                    Title: "Import As [ Condition List " + (rightClickedConditionListIndex + 1).ToString() + " ]",
                    DefaultResponse: string.IsNullOrEmpty(clipboardText) ? defaultResponse : clipboardText
                    ).Trim();
            }

            // Restore form topmost setting.
            this.TopMost = topMostBackup;

            // Validate user response.
            if (string.IsNullOrEmpty(userResponse))
            {
                // ===============
                // User cancelled.
                // ===============

                // Cancel/restore code at end of function.
            }
            else
            {
                ConditionList importedListCondition = null;

                try
                {
                    importedListCondition = ConditionListFromBase64String(userResponse.Trim());
                }
                catch { }

                // If valid user response.
                if (importedListCondition != null)
                {
                    // ========================
                    // Valid condition entered.
                    // ========================

                    string overwriteWith = "";

                    if (importedListCondition.Count == 0)
                    {
                        overwriteWith = "with an empty condition list.";
                    }
                    else
                    {
                        if (importedListCondition.Count == 1)
                            overwriteWith = "with an imported condition list containing\r\n< one > condition entry.";
                        else
                            overwriteWith = "with an imported condition list containing\r\na total of < " + importedListCondition.Count.ToString() + " > condition entries.";
                    }

                    // Show overwrite note to user and get response.
                    DialogResult userConfirmationResponse =
                        MetroMessageBox.Show(this,
                                "You are about to overwrite [ Condition List " + (rightClickedConditionListIndex + 1) + " ]\r\n" +
                                overwriteWith + "\r\n" +
                                "\r\n" +
                                "Continue ?",
                                "", // Stop | Warning
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);

                    // ===============
                    // User confirmed.
                    // ===============

                    if (userConfirmationResponse == DialogResult.OK)
                    {
                        // Now that the condition list to be updated
                        // a) has focus,
                        // b) is valid and
                        // c) the user confirmed to overwrite the current one:
                        //    Update the current condition list with the imported one.
                        PopulateConditionList(importedListCondition);

                        // Exit this method ( in order not to run undo code ).
                        return;
                    } // ( else will reach undo code. )
                }
                else
                {
                    // ==========================
                    // Invalid condition entered.
                    // ==========================

                    // Show warning to user.
                    MetroMessageBox.Show(this,
                                "Whoops ... the provided text does not correspond to a valid condition list.",
                                "", // Error
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // ==========================
            // User cancelled.
            // ( or )
            // Invalid condition entered.
            // ==========================

            // When this point is reached:
            // ---------------------------
            // UNDO CHANGES/RESTORE.
            // ---------------------
            // Cancel the import and restore where needed.
            selectedBtnBeforeImport.Checked = true;

            // Programmatically (left-)click the (original) radio button,
            // triggering a focus on/restoring/showing its list.
            AllRadioButtons_Click(selectedBtnBeforeImport, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
        }

        private void clearListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // In case the user cancels the input process,
            // provide a way to return to current list.
            RadioButton selectedBtnBeforeClear = null;
            selectedBtnBeforeClear = this.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked); // Using LINQ.
            // [DEVNOTE] Set the above condition list index as current list index.
            //           = Handled by forced click event further on.

            // The import toolstrip tag contains right clicked condition list index.
            int rightClickedConditionListIndex = (int)cmExportImport.Items[1].Tag;

            // Move to the condition list that needs updating
            // by triggering a radio button click corresponding to that list.
            foreach (RadioButton rBtn in this.Controls.OfType<RadioButton>())
            {
                if (rBtn.Text.Contains((rightClickedConditionListIndex + 1).ToString())) // [DEVNOTE] Needs translation to human readable string.
                {
                    // Check the right clicked radio button.
                    // [DEVNOTE] Didn't think this was necessary with the forced click below,
                    //           though apparently it is.
                    rBtn.Checked = true;

                    // Programmatically (left-)click the radio button corresponding to the list to be updated,
                    // triggering a focus on that list.
                    AllRadioButtons_Click(rBtn, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));

                    break;
                }
            }

            // Show deletion note to user and get response.
            DialogResult userConfirmationResponse =
                MetroMessageBox.Show(this,
                        "You are about to clear all entries of [ Condition List " + (rightClickedConditionListIndex + 1) + " ] .\r\n" +
                        "\r\n" +
                        "Continue ?",
                        "", // Stop | Warning
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);

            // If user confirmed, clear all entries.
            if (userConfirmationResponse == DialogResult.OK)
            {
                // Now that the condition list to be updated
                // a) has focus and
                // b) the user confirmed to clear the current one:
                //    Update the current condition list with an empty one.
                PopulateConditionList(new ConditionList());
            }
            else
            {
                // ===============
                // User cancelled.
                // ===============

                // ---------------------
                // UNDO CHANGES/RESTORE.
                // ---------------------
                // Cancel the import and restore where needed.
                selectedBtnBeforeClear.Checked = true;

                // Programmatically (left-)click the (original) radio button,
                // triggering a focus on/restoring/showing its list.
                AllRadioButtons_Click(selectedBtnBeforeClear, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
            }
        }

        private void numAmount_Enter(object sender, EventArgs e)
        {
            lblAmount.Focus();
        }

        private void numAmount_ValueChanged(object sender, EventArgs e)
        {
            lblAmount.Focus();
        }

        private void numSubAmount_Enter(object sender, EventArgs e)
        {
            lblSubAmount.Focus();
        }

        private void numSubAmount_ValueChanged(object sender, EventArgs e)
        {
            lblSubAmount.Focus();
        }

        private void numSubSubAmount_Enter(object sender, EventArgs e)
        {
            lblSubAmount.Focus();
        }

        private void numSubSubAmount_ValueChanged(object sender, EventArgs e)
        {
            lblSubAmount.Focus();
        }

        private void numSubSubSubAmount_Enter(object sender, EventArgs e)
        {
            lblSubAmount.Focus();
        }

        private void numSubSubSubAmount_ValueChanged(object sender, EventArgs e)
        {
            lblSubAmount.Focus();
        }

        private void chkbxAnyAmount_MouseHover(object sender, EventArgs e)
        {
            // [DEVNOTE]
            // Mouse Hover Event based workaround in order to show tooltip longer than default 5 seconds.
            // Can (apparently) go up to 32,767 milliseconds.
            // Reference: https://stackoverflow.com/a/8225836
            toolTip.Show("\r\nℹ\r\nWhen checked:\r\n" +
                "✅ Accept any amount\r\nof each of the selected stats\r\n( to be detected at least once ).\r\n" +
                "❌ Won't accept if a stat is detected\r\nothers than the ones listed\r\nor when a listed stat is missing.\r\n ",
                chkbxAnyAmount,
                duration: 30000
                );
        }

        private void seeThroughRegion_Resize(object sender, EventArgs e)
        {
            int currWidth = seeThroughRegion.Width;
            int currHeight = seeThroughRegion.Height;

            // 'Retain the old attribute'.
            int positionRetainX = (int)Math.Round(0.21 * currWidth);
            int positionRetainY = (int)Math.Round(0.879 * currHeight);

            btnRetain.Location = new Point(
                seeThroughRegion.Location.X + (int)positionRetainX - (int)Math.Round(0.50 * btnRetain.Width),
                seeThroughRegion.Location.Y + (int)positionRetainY - (int)Math.Round(0.50 * btnRetain.Height));

            // 'Use the new attribute'.
            int positionNewX = (int)Math.Round(0.81 * currWidth);
            int positionNewY = (int)Math.Round(0.879 * currHeight);

            btnNew.Location = new Point(
                seeThroughRegion.Location.X + (int)positionNewX - (int)Math.Round(0.50 * btnNew.Width),
                seeThroughRegion.Location.Y + (int)positionNewY - (int)Math.Round(0.50 * btnNew.Height));

            // 'Reproduce'.
            int positionReproduceX = (int)Math.Round(0.52 * currWidth);
            int positionReproduceY = (int)Math.Round(0.92 * currHeight);

            btnReproduce.Location = new Point(
                seeThroughRegion.Location.X + (int)positionReproduceX - (int)Math.Round(0.50 * btnReproduce.Width),
                seeThroughRegion.Location.Y + (int)positionReproduceY - (int)Math.Round(0.50 * btnReproduce.Height));
        }

        #region Drag and drop rows reorder related.

        // Based on (reference): https://www.inforbiro.com/blog/c-datagridview-drag-and-drop-rows-reorder.

        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;

        private void dgConditions_DragDrop(object sender, DragEventArgs e)
        {
            // The mouse locations are relative to the screen, so these must be
            // converted to client coordinates.
            Point clientPoint = dgConditions.PointToClient(new Point(e.X, e.Y));

            // Get the row index of the item the mouse is below.
            rowIndexOfItemUnderMouseToDrop = dgConditions.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            // If the drag operation was a move above a valid row to be inserted, remove and insert the row (else do nothing).
            if (e.Effect == DragDropEffects.Move & rowIndexOfItemUnderMouseToDrop != -1)
            {
                DataGridViewRow rowToMove = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
                dgConditions.Rows.RemoveAt(rowIndexFromMouseDown);
                dgConditions.Rows.Insert(rowIndexOfItemUnderMouseToDrop, rowToMove);

                // Additionally: store reordered condition list.
                StoreConditionList();
            }
        }

        private void dgConditions_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void dgConditions_MouseDown(object sender, MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            rowIndexFromMouseDown = dgConditions.HitTest(e.X, e.Y).RowIndex;

            if (rowIndexFromMouseDown != -1)
            {
                // Remember the point where the mouse down occurred.
                // The DragSize indicates the size that the mouse can move
                // before a drag event should be started.
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                dragBoxFromMouseDown = new Rectangle(
                          new Point(
                            e.X - (dragSize.Width / 2),
                            e.Y - (dragSize.Height / 2)),
                      dragSize);
            }
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void dgConditions_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                try
                {
                    // If the mouse moves outside the rectangle, start the drag.
                    if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                    {
                        // Proceed with the drag and drop, passing in the list item.
                        DragDropEffects dropEffect = dgConditions.DoDragDrop(
                              dgConditions.Rows[rowIndexFromMouseDown],
                              DragDropEffects.Move);
                    }
                }
                catch { }
            }
        }

        #endregion Drag and drop rows reorder related.

        #endregion Events.
    }
}