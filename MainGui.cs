using Extensions;
using MetroFramework;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

namespace EPW_Recaster
{
    public partial class MainGui : MetroForm
    {
        #region Main Gui.

        private double CaptureRegionHeightClipping { get; set; } = 0.75;

        #endregion

        #region Info Gui.

        InfoGui InfoGui { get; set; }

        #endregion

        #region Task Tokens.

        internal CancellationTokenSource Ocr_CancellationTokenSource { get; set; } = null;

        internal CancellationToken Ocr_Token { get; set; } = new CancellationToken();

        internal DateTime Ocr_StartTime { get; set; } = new DateTime();
        #endregion

        #region GUI Progress Reporting.
        private IProgress<string> GuiInfoTxtProgress { get; set; } = null;
        private IProgress<RtMessage> GuiInfoRichTxtProgress { get; set; } = null;
        #endregion

        #region Progress Handling Methods.
        private void InitializeProgressHandling()
        {
            // Set up GUI Info Text progress handling.
            // [DEVNOTE] 'Modifiers' property on child form's control should be set to (f.e.) 'Internal' in order to get access.
            GuiInfoTxtProgress = new Progress<string>(value =>
            {
                if (!String.IsNullOrEmpty(value))
                {
                    // If not first appended text,
                    // add a newline.
                    if (!String.IsNullOrEmpty(InfoGui.rTxtBoxInfo.Text))
                        InfoGui.rTxtBoxInfo.AppendText(Environment.NewLine);

                    // Append given text.
                    InfoGui.rTxtBoxInfo.AppendText(value);
                }
                else
                {
                    // If given text is empty,
                    // clear rich text box contents.
                    InfoGui.rTxtBoxInfo.Text = "";
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
                    // add a newline.
                    if (!String.IsNullOrEmpty(InfoGui.rTxtBoxInfo.Text))
                        InfoGui.rTxtBoxInfo.AppendText(Environment.NewLine);

                    // Append given text.
                    Color origTextColor = InfoGui.rTxtBoxInfo.SelectionColor;

                    InfoGui.rTxtBoxInfo.SelectionStart = InfoGui.rTxtBoxInfo.TextLength;
                    InfoGui.rTxtBoxInfo.SelectionLength = 0;

                    InfoGui.rTxtBoxInfo.SelectionColor = value.Color;
                    InfoGui.rTxtBoxInfo.AppendText(value.Message);
                    InfoGui.rTxtBoxInfo.SelectionColor = origTextColor;
                }
                else
                {
                    // If given text is empty,
                    // clear rich text box contents.
                    InfoGui.rTxtBoxInfo.Text = "";
                }

                // ~ InfoGui.rTxtBoxInfo.Refresh();
                InfoGui.rTxtBoxInfo.Invalidate();
                InfoGui.rTxtBoxInfo.Update();
            });
        }

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
        #endregion

        public MainGui()
        {
            try
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
                #endregion

                InfoGui = new InfoGui(this);

                InitializeMainGuiStyle();
                InitializeInfoGuiStyle(); // [DEVNOTE] Best to place before showing. 

                InfoGui.Show();

                InitializeProgressHandling();
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex, "Exception caught");
                this.Close();
            }
        }

        private void InitializeMainGuiStyle()
        {
            // Size.
            // -----
            if(Properties.Settings.Default.MainWindowSize.Width != 0)
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

            // Programmatorical default.
            this.Theme = metroStyleManager.Theme;
            this.Style = metroStyleManager.Style;

            // Overwrite if needed by cfg file.
            this.SetThemeAndStyle();

            captureRegion.BackColor = Color.FromArgb(15, Color.DarkGreen); // [DEVNOTE] Alpha = between 0 and 255.

            lblCaptureRegion.BackColor = Color.FromArgb(15, Color.DarkGreen);
            lblCaptureRegion.ForeColor = MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme); //this.BackColor;//

            toolTip.Theme = MetroThemeStyle.Dark;

            #region Conditions.

            gbConditions.BackColor = this.BackColor;

            lvConditions.BackColor = this.BackColor;
            lvConditions.ForeColor = MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme);

            lblConditionsNote.UseCustomBackColor = true;
            lblConditionsNote.BackColor = this.BackColor;
            lblConditionsNote.UseCustomForeColor = true;
            lblConditionsNote.ForeColor = Color.Red; //MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme);

            numAmount.BackColor = this.BackColor;
            numAmount.ForeColor = MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme);

            lblAmount.Theme = metroStyleManager.Theme;

            cbTerms.BackColor = this.BackColor;
            cbTerms.ForeColor = MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme);

            PopulateTermsComboBox();

            btnAddCondition.Theme = metroStyleManager.Theme;
            btnAddCondition.BackColor = this.BackColor;
            btnAddCondition.UseCustomForeColor = true;
            btnAddCondition.ForeColor = Color.Green;

            // Double buffering.
            lvConditions
            .GetType()
            .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
            .SetValue(lvConditions, true, null);

            lvConditions.FullRowSelect = true;
            lvConditions.View = View.Details;

            lvConditions.Columns.Add("Dummy"); // [DEVNOTE] Quick hack in order to be able to right align 2nd (first visible) column.
            lvConditions.Columns.Add("Amount");
            lvConditions.Columns.Add("Preferred Stat");
            lvConditions.Columns.Add("Dummy"); // [DEVNOTE] Quick hack in order to be able to color last column.
            lvConditions.Columns.Add("Delete");

            ListViewExtender extender = new ListViewExtender(lvConditions);
            // Extend with a button column.
            ListViewButtonColumn buttonAction = new ListViewButtonColumn(lvConditions.Columns.Count - 1);
            buttonAction.Click += lvConditions_OnButtonRemoveClick;
            buttonAction.FixedWidth = true;

            lvConditions.Columns[0].Width = 0; // [DEVNOTE] Quick hack in order to be able to right align 2nd (first visible) column.
            lvConditions.Columns[1].Width = 35;
            lvConditions.Columns[1].TextAlign = HorizontalAlignment.Right;
            lvConditions.Columns[2].Width = 169;
            lvConditions.Columns[2].TextAlign = HorizontalAlignment.Left;
            lvConditions.Columns[3].Width = 1; // [DEVNOTE] Quick hack in order to be able to color last column. Needs to be 1 in order to work.
            lvConditions.Columns[4].Width = 25;
            lvConditions.Columns[4].TextAlign = HorizontalAlignment.Right;

            extender.AddColumn(buttonAction);

            PopulateConditionList();

            #endregion

            toolTip.SetToolTip(numAmount, "\r\nℹ\r\nIgnore white stats.\r\n( f.e. 'Phys. Res.' maximum = 4 )\r\n ");

            #region Subconditions.

            gbSubConditions.BackColor = this.BackColor;

            lvSubConditions.BackColor = this.BackColor;
            lvSubConditions.ForeColor = MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme);

            lblSubConditionsNote.UseCustomBackColor = true;
            lblSubConditionsNote.BackColor = this.BackColor;
            lblSubConditionsNote.UseCustomForeColor = true;
            lblSubConditionsNote.ForeColor = MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme);

            numSubAmount.BackColor = this.BackColor;
            numSubAmount.ForeColor = MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme);

            lblSubAmount.Theme = metroStyleManager.Theme;

            cbSubTerms.BackColor = this.BackColor;
            cbSubTerms.ForeColor = MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme);

            PopulateSubTermsComboBox();

            btnAddSubCondition.Theme = metroStyleManager.Theme;
            btnAddSubCondition.BackColor = this.BackColor;
            btnAddSubCondition.UseCustomForeColor = true;
            btnAddSubCondition.ForeColor = Color.Green;

            // Double buffering.
            lvSubConditions
            .GetType()
            .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
            .SetValue(lvSubConditions, true, null);

            lvSubConditions.FullRowSelect = true;
            lvSubConditions.View = View.Details;

            lvSubConditions.Columns.Add("Dummy"); // [DEVNOTE] Quick hack in order to be able to right align 2nd (first visible) column.
            lvSubConditions.Columns.Add("Amount");
            lvSubConditions.Columns.Add("Preferred Additional stat");
            lvSubConditions.Columns.Add("Dummy"); // [DEVNOTE] Quick hack in order to be able to color last column.
            lvSubConditions.Columns.Add("Delete");

            ListViewExtender extenderSub = new ListViewExtender(lvSubConditions);
            // Extend with a button column.
            ListViewButtonColumn subButtonAction = new ListViewButtonColumn(lvSubConditions.Columns.Count - 1);
            subButtonAction.Click += lvSubConditions_OnButtonRemoveClick;
            subButtonAction.FixedWidth = true;

            lvSubConditions.Columns[0].Width = 0; // [DEVNOTE] Quick hack in order to be able to right align 2nd (first visible) column.
            lvSubConditions.Columns[1].Width = 35;
            lvSubConditions.Columns[1].TextAlign = HorizontalAlignment.Right;
            lvSubConditions.Columns[2].Width = 169;
            lvSubConditions.Columns[2].TextAlign = HorizontalAlignment.Left;
            lvSubConditions.Columns[3].Width = 1; // [DEVNOTE] Quick hack in order to be able to color last column. Needs to be 1 in order to work.
            lvSubConditions.Columns[4].Width = 25;
            lvSubConditions.Columns[4].TextAlign = HorizontalAlignment.Right;

            extenderSub.AddColumn(subButtonAction);

            PopulateSubConditionList();

            #endregion

            toolTip.SetToolTip(numSubAmount, "\r\nℹ\r\nIgnore white stats.\r\n( f.e. 'Phys. Res.' maximum = 4 )\r\n ");

            toolTip.SetToolTip(btnAddSubCondition, "\r\nℹ\r\nAdded additional\r\nrequired conditions\r\nwill only be ignored\r\nif a required condition\r\nwas matched\r\nusing 5 x stats.\r\n ");
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
                        }
                    }
                } 
            }
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

        private void PopulateSubTermsComboBox()
        {
            // Styling.
            cbSubTerms.BackColor = this.BackColor;

            // Data.
            SortedDictionary<string, string> cbSubTermsSource = new SortedDictionary<string, string>();

            // Load cfg file containing terms.
            string[] terms = File.ReadAllLines(Tesseract.Ocr.AssemblyCodeBaseDirectory + @"\Config\Stats.cfg");

            // Add a blank dummy.
            cbSubTermsSource.Add("", "");

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

        /// <summary>
        /// Populates the Condition List View.
        /// To be used at initialization phase,
        /// since the AddCondition and RemoveCondition methods
        /// will keep it updated afterwards.
        /// </summary>
        private void PopulateConditionList()
        {
            // (Try) Load from saved settings. Can be an empty list.
            List<Condition> conditions = LoadConditions();

            // Populate the list view.
            lvConditions.Items.Clear();

            if (conditions.Count > 0)
            {
                foreach (Condition condition in conditions)
                {
                    AddCondition(condition);
                }
            }

            // Check labels.
            if (conditions.Count <= 1)
            {
                gbConditions.Text = "(Required) Must have at least:";
            }
            else
            {
                gbConditions.Text = "(Required) Must have at least (either):";
            }
            if (conditions.Count == 0)
            {
                lblConditionsNote.Visible = true;
            }
            else
            {
                lblConditionsNote.Visible = false;
            }
        }
        
        /// <summary>
         /// Populates the Condition List View.
         /// To be used at initialization phase,
         /// since the AddCondition and RemoveCondition methods
         /// will keep it updated afterwards.
         /// </summary>
        private void PopulateSubConditionList()
        {
            // (Try) Load from saved settings. Can be an empty list.
            List<Condition> subConditions = LoadSubConditions();

            // Populate the list view.
            lvSubConditions.Items.Clear();

            if (subConditions.Count > 0)
            {
                foreach (Condition subCondition in subConditions)
                {
                    AddSubCondition(subCondition);
                }
            }

            // Check labels.
            if (subConditions.Count <= 1)
            {
                gbSubConditions.Text = "(Additional) AND must also have at least:";
            }
            else
            {
                gbSubConditions.Text = "(Additional) AND must also have at least (either):";
            }
            if (subConditions.Count == 0)
            {
                lblSubConditionsNote.Visible = true;
            }
            else
            {
                lblSubConditionsNote.Visible = false;
            }
        }

        private void AddCondition(Condition condition)
        {
            // A. User input error corrections.
            
            // a. Always set amount to 1x for unique stats Purify Spell, God of Frenzy, Square Formation, Soul Shatter, Spirit Blackhole, ...
            if (
                condition.LongTerm.Contains("Purify") ||
                condition.LongTerm.Contains("Frenzy") ||
                condition.LongTerm.Contains("Square") ||
                condition.LongTerm.Contains("Shatter") ||
                condition.LongTerm.Contains("Blackhole")
               )
            {
                condition.Amount = 1;
            }

            // b. Since version 2, this tool ignores white stats.
            //    If 'Phys. Res.' amount > 4, force to 4.
            if (
                condition.LongTerm.Contains("Phys. Res.") &
                condition.Amount > 4
               )
            {
                condition.Amount = 4;
            }

            // B. Add to list view.
            ListViewItem listViewItem = new ListViewItem("");
            listViewItem.SubItems.Add(condition.Amount.ToString() + "   x");
            listViewItem.SubItems.Add(condition.LongTerm);
            listViewItem.SubItems.Add("");
            listViewItem.SubItems.Add("✖"); // Remove button.

            listViewItem.SubItems[listViewItem.SubItems.Count - 2].ForeColor = Color.Red;
            listViewItem.UseItemStyleForSubItems = false;

            listViewItem.Tag = condition;

            lvConditions.Items.Add(listViewItem);

            StoreConditions();

            // C. Check labels.
            if (lvConditions.Items.Count <= 1)
            {
                gbConditions.Text = "(Required) Must have at least:";
            }
            else
            {
                gbConditions.Text = "(Required) Must have at least (either):";
            }
            if (lvConditions.Items.Count == 0)
            {
                lblConditionsNote.Visible = true;
            }
            else
            {
                lblConditionsNote.Visible = false;
            }
        }

        private void btnAddCondition_Click(object sender, EventArgs e)
        {
            // Unfocus.
            lblAmount.Focus();

            // Get selected KVP from combo box.
            KeyValuePair<string, string> selectedEntry
                = (KeyValuePair<string, string>)cbTerms.SelectedItem;

            // Return if blank dummy is being added.
            if (String.IsNullOrEmpty(selectedEntry.Key))
                return;

            // Create a new condition.
            Condition newCondition = new Condition((int)numAmount.Value, selectedEntry.Key.Replace("*","").Trim(), selectedEntry.Value);

            // Check if a similar condition hasn't already been added.
            Condition similarCondition = null;
            foreach(ListViewItem lvi in lvConditions.Items)
            {
                if(newCondition.LongTerm == ((Condition)lvi.Tag).LongTerm)
                {
                    similarCondition = (Condition)lvi.Tag;
                }
            }

            if (similarCondition == null)
            {
                #region Additional logic check before adding (compare with other list).
                // [DEVNOTE] [IMPORTANT] Additional logic check.
                // Check if a similar subcondition hasn't already been added in optional condition list.
                // Reason: makes no sense to f.e. require 2 x channelling & add optional required stat 1 x channelling.
                foreach (ListViewItem lvi in lvSubConditions.Items)
                {
                    if (newCondition.LongTerm == ((Condition)lvi.Tag).LongTerm)
                    {
                        similarCondition = (Condition)lvi.Tag;
                    }
                }

                if (similarCondition == null)
                {
                    AddCondition(newCondition);
                }
                else
                {
                    MetroMessageBox.Show(this, "\n\n" +
                                "'" + newCondition.Amount.ToString() + " x " + newCondition.LongTerm + "' can not be added to the required condition list.\n" +
                                "A similar '" + newCondition.LongTerm + "' condition has already been added in the optional condition list.\n" +
                                "Remove '" + similarCondition.Amount.ToString() + " x " + similarCondition.LongTerm + "' first or add another condition.",
                                "Note",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                #endregion
            }
            else
            {
                if (similarCondition.Amount != newCondition.Amount)
                {
                    MetroMessageBox.Show(this, "\n\n" +
                                "'" + newCondition.Amount.ToString() + " x " + newCondition.LongTerm + "' can not be added to the condition list.\n" +
                                "A similar '" + newCondition.LongTerm + "' condition has already been added.\n" +
                                "Remove '" + similarCondition.Amount.ToString() + " x " + similarCondition.LongTerm + "' first or add another condition.",
                                "Note",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning); 
                }
                else
                {
                    // Do nothing. | Don't add.
                }
            }
        }

        private void AddSubCondition(Condition subCondition)
        {
            // A. Always set amount to 1x for unique stats Purify Spell, God of Frenzy, Square Formation, Soul Shatter, Spirit Blackhole, ...
            if (
                subCondition.LongTerm.Contains("Purify") ||
                subCondition.LongTerm.Contains("Frenzy") ||
                subCondition.LongTerm.Contains("Square") ||
                subCondition.LongTerm.Contains("Shatter") ||
                subCondition.LongTerm.Contains("Blackhole")
               )
            {
                subCondition.Amount = 1;
            }

            // B. Add to list view.
            ListViewItem listViewItem = new ListViewItem("");
            listViewItem.SubItems.Add(subCondition.Amount.ToString() + "   x");
            listViewItem.SubItems.Add(subCondition.LongTerm);
            listViewItem.SubItems.Add("");
            listViewItem.SubItems.Add("✖"); // Remove button.

            listViewItem.SubItems[listViewItem.SubItems.Count - 2].ForeColor = Color.Red;
            listViewItem.UseItemStyleForSubItems = false;

            listViewItem.Tag = subCondition;

            lvSubConditions.Items.Add(listViewItem);

            StoreSubConditions();

            // C. Check labels.
            if (lvSubConditions.Items.Count <= 1)
            {
                gbSubConditions.Text = "(Additional) AND must also have at least:";
            }
            else
            {
                gbSubConditions.Text = "(Additional) AND must also have at least (either):";
            }
            if (lvSubConditions.Items.Count == 0)
            {
                lblSubConditionsNote.Visible = true;
            }
            else
            {
                lblSubConditionsNote.Visible = false;
            }
        }

        private void btnAddSubCondition_Click(object sender, EventArgs e)
        {
            // Unfocus.
            lblSubAmount.Focus();

            // Get selected KVP from combo box.
            KeyValuePair<string, string> selectedEntry
                = (KeyValuePair<string, string>)cbSubTerms.SelectedItem;

            // Return if blank dummy is being added.
            if (String.IsNullOrEmpty(selectedEntry.Key))
                return;

            // Create a new condition.
            Condition newCondition = new Condition((int)numSubAmount.Value, selectedEntry.Key.Replace("*", "").Trim(), selectedEntry.Value);

            // Check if a similar subcondition hasn't already been added in subcondition list.
            Condition similarCondition = null;
            foreach (ListViewItem lvi in lvSubConditions.Items)
            {
                if (newCondition.LongTerm == ((Condition)lvi.Tag).LongTerm)
                {
                    similarCondition = (Condition)lvi.Tag;
                }
            }

            if (similarCondition == null)
            {
                #region Additional logic check before adding (compare with other list).
                // [DEVNOTE] [IMPORTANT] Additional logic check.
                // Check if a similar subcondition hasn't already been added in main condition list.
                // Reason: makes no sense to f.e. require 2 x channelling & add optional required stat 1 x channelling.
                foreach (ListViewItem lvi in lvConditions.Items)
                {
                    if (newCondition.LongTerm == ((Condition)lvi.Tag).LongTerm)
                    {
                        similarCondition = (Condition)lvi.Tag;
                    }
                }

                if (similarCondition == null)
                {
                    AddSubCondition(newCondition); 
                }
                else
                {
                    MetroMessageBox.Show(this, "\n\n" +
                                "'" + newCondition.Amount.ToString() + " x " + newCondition.LongTerm + "' can not be added to the optional condition list.\n" +
                                "A similar '" + newCondition.LongTerm + "' condition has already been added in the required condition list.\n" +
                                "Remove '" + similarCondition.Amount.ToString() + " x " + similarCondition.LongTerm + "' first or add another condition.",
                                "Note",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                #endregion
            }
            else
            {
                if (similarCondition.Amount != newCondition.Amount)
                {
                    MetroMessageBox.Show(this, "\n\n" +
                                "'" + newCondition.Amount.ToString() + " x " + newCondition.LongTerm + "' can not be added to the optional condition list.\n" +
                                "A similar '" + newCondition.LongTerm + "' condition has already been added.\n" +
                                "Remove '" + similarCondition.Amount.ToString() + " x " + similarCondition.LongTerm + "' first or add another condition.",
                                "Note",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    // Do nothing. | Don't add.
                }
            }
        }

        private void RemoveCondition(ListViewItem conditionItem)
        {
            lvConditions.Items.Remove(conditionItem);

            StoreConditions();

            // Check labels.
            if (lvConditions.Items.Count <= 1)
            {
                gbConditions.Text = "(Required) Must have at least:";
            }
            else
            {
                gbConditions.Text = "(Required) Must have at least (either):";
            }
            if (lvConditions.Items.Count == 0)
            {
                lblConditionsNote.Visible = true;
            }
            else
            {
                lblConditionsNote.Visible = false;
            }
        }

        private void lvConditions_OnButtonRemoveClick(object sender, ListViewColumnMouseEventArgs e)
        {
            RemoveCondition(e.Item);
        }

        private void RemoveSubCondition(ListViewItem conditionItem)
        {
            lvSubConditions.Items.Remove(conditionItem);

            StoreSubConditions();

            // Check labels.
            if (lvSubConditions.Items.Count <= 1)
            {
                gbSubConditions.Text = "(Additional) AND must also have at least:";
            }
            else
            {
                gbSubConditions.Text = "(Additional) AND must also have at least (either):";
            }
            if (lvSubConditions.Items.Count == 0)
            {
                lblSubConditionsNote.Visible = true;
            }
            else
            {
                lblSubConditionsNote.Visible = false;
            }
        }

        private void lvSubConditions_OnButtonRemoveClick(object sender, ListViewColumnMouseEventArgs e)
        {
            RemoveSubCondition(e.Item);
        }

        private string ConditionListToBase64String(List<Condition> conditions)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, conditions);
                ms.Position = 0;
                byte[] buffer = new byte[(int)ms.Length];
                ms.Read(buffer, 0, buffer.Length);
                return Convert.ToBase64String(buffer);
            }
        }

        private List<Condition> ConditionListFromBase64String(string base64string)
        {
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(base64string)))
            {
                BinaryFormatter bf = new BinaryFormatter();
                return (List<Condition>)bf.Deserialize(ms);
            }
        }

        private void StoreConditions()
        {
            // A. Add to (temp) conditions container for storage. Can be empty.
            List<Condition> conditions = new List<Condition>();
            foreach (ListViewItem lvi in lvConditions.Items)
            {
                conditions.Add((Condition)lvi.Tag);
            }

            // B. Store.
            Properties.Settings.Default.Conditions = ConditionListToBase64String(conditions);
            Properties.Settings.Default.Save();
        }

        private List<Condition> LoadConditions()
        {
            if (!String.IsNullOrEmpty(Properties.Settings.Default.Conditions))
            {
                return ConditionListFromBase64String(Properties.Settings.Default.Conditions);
            }
            else
            {
                return new List<Condition>();
            }
        }

        private void StoreSubConditions()
        {
            // A. Add to (temp) subconditions container for storage. Can be empty.
            List<Condition> subConditions = new List<Condition>();
            foreach (ListViewItem lvi in lvSubConditions.Items)
            {
                subConditions.Add((Condition)lvi.Tag);
            }

            // B. Store.
            Properties.Settings.Default.SubConditions = ConditionListToBase64String(subConditions);
            Properties.Settings.Default.Save();
        }

        private List<Condition> LoadSubConditions()
        {
            if (!String.IsNullOrEmpty(Properties.Settings.Default.SubConditions))
            {
                return ConditionListFromBase64String(Properties.Settings.Default.SubConditions);
            }
            else
            {
                return new List<Condition>();
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

            // Custom.
            InfoGui.btnChainForms.Theme = metroStyleManager.Theme;

            InfoGui.btnLogFolder.Theme = metroStyleManager.Theme;

            InfoGui.rTxtBoxInfo.BackColor = this.BackColor;
            InfoGui.rTxtBoxInfo.ForeColor = MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme);
            
            InfoGui.btnOcr.Theme = metroStyleManager.Theme;

            InfoGui.lblMaxRolls.Theme = metroStyleManager.Theme;
            InfoGui.numMaxRolls.BackColor = this.BackColor;
            InfoGui.numMaxRolls.ForeColor = MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme);

            InfoGui.chkbxPreviewCapture.Theme = metroStyleManager.Theme;
            InfoGui.chkbxPreviewCapture.Style = metroStyleManager.Style;

            // Set stored Max Rolls.
            if(Properties.Settings.Default.MaxRolls > 0 & Properties.Settings.Default.MaxRolls <= 999)
            {
                InfoGui.numMaxRolls.Value = Properties.Settings.Default.MaxRolls;
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
            }
        }

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

        private void MainGui_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Directory.Exists(Tesseract.Ocr.TempCacheDirectory))
                Directory.Delete(Tesseract.Ocr.TempCacheDirectory, recursive: true); // Delete Tesseract's temp cache folder & its contents.

            if (Ocr_CancellationTokenSource != null)
                Ocr_CancellationTokenSource.Cancel();
        }

        private void lvConditions_Enter(object sender, EventArgs e)
        {
            lblAmount.Focus();
        }

        private void lvSubConditions_Enter(object sender, EventArgs e)
        {
            lblSubAmount.Focus();
        }

        private void numAmount_Enter(object sender, EventArgs e)
        {
            lblAmount.Focus();
        }

        private void numSubAmount_Enter(object sender, EventArgs e)
        {
            lblSubAmount.Focus();
        }

        private void numAmount_ValueChanged(object sender, EventArgs e)
        {
            lblAmount.Focus();
        }

        private void numSubAmount_ValueChanged(object sender, EventArgs e)
        {
            lblSubAmount.Focus();
        }

        private void cbTerms_DropDownClosed(object sender, EventArgs e)
        {
            lblAmount.Focus();
        }

        private void cbSubTerms_DropDownClosed(object sender, EventArgs e)
        {
            lblSubAmount.Focus();
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

        private void gbConditions_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;

            /*Color color = MetroFramework.Drawing.MetroPaint.GetStyleColor(MetroColorStyle.Orange);
            DrawGroupBox(box, e.Graphics, textColor: color, borderColor: color);*/

            DrawGroupBox(box, e.Graphics,
                textColor: MetroFramework.Drawing.MetroPaint.ForeColor.Label.Normal(this.Theme),
                borderColor: MetroFramework.Drawing.MetroPaint.GetStyleColor(this.Style)
                );
        }

        private void gbSubConditions_Paint(object sender, PaintEventArgs e)
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
    }
}
