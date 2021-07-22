using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace EPW_Recaster
{
    public partial class MainGui
    {
        #region Variables | Properties.

        // For logging purposes.
        private Bitmap img;

        // To be used by Tesseract (OCR).
        private Bitmap upscaledImg;

        private string upscaledImgPath = Tesseract.Ocr.TempCacheDirectory + @"\tmp.png";

        private int CaptureRegionWidth { get; set; } = 0;
        private int CaptureRegionHeight { get; set; } = 0;

        private Point CaptureRegionUpperLeftPoint { get; set; } = new Point();

        private Point RetainClickPoint { get; set; } = new Point();

        private Point NewClickPoint { get; set; } = new Point();

        private Point ReproduceClickPoint { get; set; } = new Point();

        private int AwaitIngameReproduceButtonAvailable { get; set; } = 1750; // Time it takes for the in-game reproduce button to become available again.
        private int AwaitIngameStatsRolled { get; set; } = 1250; // Time it takes for the in-game stats to be rolled.
        private int AwaitAcceptRejectAction { get; set; } = 1750; // Time to wait before accepting/rejecting a roll.

        #endregion Variables | Properties.

        #region Methods.

        private System.Drawing.Bitmap CaptureRegion()
        {
            Bitmap tmpBmp = new Bitmap(CaptureRegionWidth, (int)Math.Round(CaptureRegionHeight * CaptureRegionHeightClipping));
            Graphics g;
            g = Graphics.FromImage(tmpBmp);
            g.CopyFromScreen(CaptureRegionUpperLeftPoint, new Point(0, 0), new Size(CaptureRegionWidth, (int)Math.Round(CaptureRegionHeight * CaptureRegionHeightClipping)));
            return (System.Drawing.Bitmap)tmpBmp;
        }

        internal void PrepareOcrLogic()
        {
            // Clear info text.
            AddMsg();

            // Set default color (in case needed in code later on).
            DefaultColor = InfoGui.rTxtBoxInfo.SelectionColor;

            if (!Directory.Exists(Tesseract.Ocr.AssemblyCodeBaseDirectory + @"\Logged"))
            {
                Directory.CreateDirectory(Tesseract.Ocr.AssemblyCodeBaseDirectory + @"\Logged");
            }

            if (!Directory.Exists(Tesseract.Ocr.TempCacheDirectory))
            {
                Directory.CreateDirectory(Tesseract.Ocr.TempCacheDirectory);
            }

            // Set the capture region data before minimizing form.
            Size size = seeThroughRegion.ClientSize;

            if (!InfoGui.PreviewCapture)
            {
                // Non-Preview Capture : Right Half Capture only.
                CaptureRegionWidth = size.Width / 2;
                CaptureRegionHeight = size.Height;
                CaptureRegionUpperLeftPoint = seeThroughRegion.PointToScreen(new Point(size.Width / 2, 0));
            }
            else
            {
                // Preview Capture : Full Width Capture.
                CaptureRegionWidth = size.Width;
                CaptureRegionHeight = size.Height;
                CaptureRegionUpperLeftPoint = seeThroughRegion.PointToScreen(new Point(0, 0));
            }

            RetainClickPoint = btnRetain.PointToScreen(
                new Point(
                    (int)Math.Round(0.50F * btnRetain.Width),
                    (int)Math.Round(0.50F * btnRetain.Height)
                    )
                );

            NewClickPoint = btnNew.PointToScreen(
                new Point(
                    (int)Math.Round(0.50F * btnNew.Width),
                    (int)Math.Round(0.50F * btnNew.Height)
                    )
                );

            ReproduceClickPoint = btnReproduce.PointToScreen(
                new Point(
                    (int)Math.Round(0.50F * btnReproduce.Width),
                    (int)Math.Round(0.50F * btnReproduce.Height)
                    )
                );
            // ---

            this.WindowState = FormWindowState.Minimized;
            InfoGui.WindowState = FormWindowState.Normal;

            SetParamsFromCfg();
        }

        private void SetParamsFromCfg()
        {
            if (File.Exists(Tesseract.Ocr.AssemblyCodeBaseDirectory + @"\Config\Params.cfg"))
            {
                // Load cfg file containing specific parameters.
                string[] parameters = File.ReadAllLines(Tesseract.Ocr.AssemblyCodeBaseDirectory + @"\Config\Params.cfg");
                foreach (string line in parameters)
                {
                    if (!line.Contains('#')) // Ignore custom comments.
                    {
                        // Split by pipe character '|'.
                        string[] split = line.Split('|');

                        if (split.Count() == 2)
                        {
                            // Await In-Game Reproduce Button Available.
                            if (split[0].ToLower().Contains("Await In-Game Reproduce".ToLower()))
                            {
                                if (Int32.TryParse(Regex.Match(split[1].Trim(), @"\d+").Value, out int cfgValue))
                                {
                                    this.AwaitIngameReproduceButtonAvailable = cfgValue;
                                }
                            }

                            // Await In-Game Stats Rolled.
                            if (split[0].ToLower().Contains("Await In-Game Stats".ToLower()))
                            {
                                if (Int32.TryParse(Regex.Match(split[1].Trim(), @"\d+").Value, out int cfgValue))
                                {
                                    this.AwaitIngameStatsRolled = cfgValue;
                                }
                            }

                            // Await Accept/Reject Action Time.
                            if (split[0].ToLower().Contains("Await Accept/Reject".ToLower()))
                            {
                                if (Int32.TryParse(Regex.Match(split[1].Trim(), @"\d+").Value, out int cfgValue))
                                {
                                    this.AwaitAcceptRejectAction = cfgValue;
                                }
                            }
                        }
                    }
                }
            }
        }

        internal void DoOcrLogic()
        {
            // ===========
            // == SETUP ==
            // ===========

            #region Initial roll setup.

            AddMsg(); // An empty msg clears (rich) text box.

            bool keepRunning = true;

            int nrRolls = 0;
            int maxNrRolls = (short)InfoGui.numMaxRolls.Value;

            bool uniqueStatWarningShown = false;

            // History of captured text in order to stop after x times same result.
            int capturedTextHistoryCapacity = 3; // Store/Compare X times.
            Queue<string> capturedTextHistory = new Queue<string>(capturedTextHistoryCapacity);

            //Thread.Sleep(500);

            // [DEVNOTE] Define here so a new folder won't be created each minute (at start only).
            string logPathWithoutExtension = Tesseract.Ocr.AssemblyCodeBaseDirectory + @"\Logged\" + DateTime.Now.ToString("yyyy-dd-MM") + " (Started " + DateTime.Now.ToString("HH-mm") + ")";

            // Get conditions for this preview or batch roll.
            List<Condition> conditions = new List<Condition>();

            // Get conditions for this preview or batch roll.
            List<ConditionListEntry> conditionListEntries = new List<ConditionListEntry>();

            if (dgConditions.InvokeRequired)
            {
                dgConditions.Invoke(new MethodInvoker(delegate
                {
                    foreach (DataGridViewRow row in dgConditions.Rows)
                    {
                        conditionListEntries.Add((ConditionListEntry)row.Tag);
                    }
                }));
            }
            else
            {
                foreach (DataGridViewRow row in dgConditions.Rows)
                {
                    conditionListEntries.Add((ConditionListEntry)row.Tag);
                }
            }

            #endregion Initial roll setup.

            // ===============
            // == ROLL LOOP ==
            // ===============

            #region Main roll loop.

            while (keepRunning)
            {
                // ========================
                // == CURRENT ROLL SETUP ==
                // ========================

                #region Current roll setup.

                Ocr_Token.ThrowIfCancellationRequested();

                img = CaptureRegion();
                upscaledImg = (System.Drawing.Bitmap)ResizeImage(img, scaleFactor: 1.5);

                string imgPath;
                string outputPath;

                Equipment currEquipment = new Equipment();

                bool validEntry = false;

                if (!InfoGui.PreviewCapture)
                {
                    string currTimeStamp = DateTime.Now.ToString("yyyy-dd-MM") + " (" + DateTime.Now.ToString("HH-mm-ss") + ")";

                    imgPath = logPathWithoutExtension + @"\" + currTimeStamp + ".png";
                    outputPath = logPathWithoutExtension + @"\" + currTimeStamp; // [DEVNOTE] Tesseract does not want .txt or any extension to be added to outputPath.
                }
                else
                {
                    // Overwrite logPathWithoutExtension.
                    logPathWithoutExtension = Tesseract.Ocr.AssemblyCodeBaseDirectory + @"\Logged";

                    imgPath = logPathWithoutExtension + @"\" + "Preview Capture.png";
                    outputPath = logPathWithoutExtension + @"\" + "Preview Capture"; // [DEVNOTE] Tesseract does not want .txt or any extension to be added to outputPath.
                }

                if (!Directory.Exists(logPathWithoutExtension))
                {
                    Directory.CreateDirectory(logPathWithoutExtension);
                }

                img.Save(imgPath, System.Drawing.Imaging.ImageFormat.Png);
                upscaledImg.Save(upscaledImgPath, System.Drawing.Imaging.ImageFormat.Png);

                string rawCapturedText = Tesseract.Ocr.GetText(
                    /*imgPath: imgPath,*/
                    imgPath: upscaledImgPath,
                    outputPath: outputPath
                    );

                currEquipment.OcrText = rawCapturedText;

                #region Queue handling.

                // Add most recent capture.
                capturedTextHistory.Enqueue(currEquipment.OcrText);

                // Remove oldest captured text if history extends predefined capacity.
                if (capturedTextHistory.Count > capturedTextHistoryCapacity)
                {
                    capturedTextHistory.Dequeue();
                }

                #endregion Queue handling.

                #region (Temporarily) Cache the config short terms.

                List<string[]> cfgTerms = new List<string[]>();

                // Load cfg file containing terms and temp cache/store all terms found.
                foreach (string term in File.ReadAllLines(Tesseract.Ocr.AssemblyCodeBaseDirectory + @"\Config\Stats.cfg"))
                {
                    if (!term.Contains('#')) // Ignore custom comments.
                    {
                        // Split by pipe character '|'.
                        string[] splitTerm = term.Split('|');

                        if (splitTerm.Count() == 2)
                        {
                            cfgTerms.Add(new string[] { splitTerm[0].Trim(), splitTerm[1].Trim() }); // splitTerm[0] = long term | splitTerm[1] = short term
                        }
                    }
                }

                #endregion (Temporarily) Cache the config short terms.

                // Display in info box.
                AddMsg();

                #endregion Current roll setup.

                // ===================
                // == VALIDATE ROLL ==
                // ===================

                #region Validate roll.

                if (!String.IsNullOrEmpty(currEquipment.OcrText))
                {
                    if (currEquipment.OcrText.Count() >= 20) // Consider as containing stats when char count > 20.
                    {
                        try
                        {
                            //AddMsg(currEquipment.OcrText);

                            List<Stat> blueStats = currEquipment.BlueStats;

                            if (blueStats.Count() >= 4)
                            {
                                if (currEquipment.IsWeapon)
                                    AddMsg(new RtMessage("[ Detected Blue Weapon Stats ]", bold: true));
                                else
                                    AddMsg(new RtMessage("[ Detected Blue Gear Stats ]", bold: true));

                                foreach (Stat blueStat in blueStats)
                                {
                                    AddMsg(new RtMessage("⮩ " + blueStat.FormattedStat, color: BlueStatColor, indent: 3));

                                    if (!uniqueStatWarningShown)
                                    {
                                        // Check if the stat contains 'Purify'...
                                        // If it does, give the user a warning about missing stat(s).
                                        if (blueStat.Identifier.Contains("Purify"))
                                        {
                                            DialogResult userChoice = MessageBox.Show(
                                                "A 'Purify' stat was detected.\r\n" +
                                                "\r\n" +
                                                "Without any actual game file alterations (configs.pck),\r\n" +
                                                "this will result in incorrect judging and handling of conditions " +
                                                "whenever this stat is rolled, since the in-game window needs scrolling " +
                                                "for all stats to be readable (falling out of scope for " + Application.ProductName + ").\r\n" +
                                                "\r\n" +
                                                "======\r\n" +
                                                "In short :\r\n" +
                                                "======\r\n" +
                                                "If long descriptive stat(s) aren't patched,\r\nsome rolls could miss out on accepting a possibly valid roll.\r\n" +
                                                "\r\n" +
                                                "Continue rolling ?",
                                                "[ IMPORTANT WARNING ]",
                                                MessageBoxButtons.OKCancel,
                                                MessageBoxIcon.Warning
                                                );

                                            uniqueStatWarningShown = true; // Only show once.

                                            if (userChoice == DialogResult.Cancel)
                                            {
                                                Ocr_CancellationTokenSource.Cancel();
                                            }
                                        }
                                    }
                                }

                                AddMsg("=========================");

                                validEntry = true;

                                // Increase roll counter if the current capture was a valid one.
                                nrRolls++;
                            }
                            else
                            {
                                AddMsg(new RtMessage("[ Equipment Not Identifiable ]", color: RedLightColor, bold: true));
                                AddMsg(
                                    "  => This roll will not be judged/handled."
                                    );
                            }
                        }
                        catch/*(Exception e)*/
                        {
                            //MessageBox.Show(e.ToString());
                            AddMsg(currEquipment.OcrText);
                        }
                    }
                    else
                    {
                        AddMsg("No valid roll information detected (yet).");
                    }
                }
                else
                {
                    //AddMsg("No text found in region.");
                    AddMsg("No valid roll information detected (yet).");
                }

                #endregion Validate roll.

                // ======================
                // == JUDGE CONDITIONS ==
                // ======================

                #region Judge conditions.

                // Check history first, stop if needed.
                // [DEVNOTE] Only check when history is filled entirely.
                if (!InfoGui.PreviewCapture & capturedTextHistory.Count == capturedTextHistoryCapacity && capturedTextHistory.Distinct().Count() == 1)
                {
                    AddMsg(); // Clear info box first.

                    AddMsg(new RtMessage("The roll process has been halted.", bold: true));
                    AddMsg("( It doesn't seem necessary to roll any further. )" + Environment.NewLine);
                    AddMsg("Either:" + Environment.NewLine +
                        "- the in-game number of Perfect Elements" + Environment.NewLine +
                        "  has been depleted" + Environment.NewLine +
                        "- the conditions have already been" + Environment.NewLine +
                        "  met and accepted" + Environment.NewLine +
                        "- the game client disconnected" + Environment.NewLine +
                        "- the tool wasn't able to read rolled stats correctly" + Environment.NewLine +
                        "  > check capture region boundaries" + Environment.NewLine +
                        "  > check if tool is overlapping" + Environment.NewLine +
                        "    the game client");
                    AddMsg("=========================");

                    string doneSound = Tesseract.Ocr.AssemblyCodeBaseDirectory + @"\Media\Sounds\Done.wav";
                    if (File.Exists(doneSound)) // !InfoGui.PreviewCapture already checked above.
                    {
                        System.Media.SoundPlayer player = new System.Media.SoundPlayer(doneSound);
                        player.Play();
                    }

                    //return; // [DEVNOTE] Needs to reach #Rolls display. Instead:
                    keepRunning = false;
                }

                // ~ else continue checking ...

                bool conditionMet = false;

                if (validEntry)
                {
                    #region Match Condition(s).

                    for (int i = 0; i < conditionListEntries.Count(); i++)
                    {
                        //AddMsg("=========================");
                        AddMsg(new RtMessage("[ Checking Condition List Entry " + (i + 1) + " ]", bold: true));

                        conditionMet = true;

                        // =====================
                        // A. Fixed Amount Mode.
                        // =====================

                        #region Fixed Amount Mode.

                        foreach (Condition currCondition in conditionListEntries[i])
                        {
                            if (currCondition.Amount > 0)
                            {
                                int hits = currEquipment.MatchStat(currCondition.ShortTerm);

                                AddMsg(new RtMessage(
                                    message:
                                    (hits >= Convert.ToInt32(currCondition.Amount) ? "✓" : "❌") + " '" + currCondition.LongTerm + "': " + hits + " / " + currCondition.Amount,
                                    color:
                                    hits >= Convert.ToInt32(currCondition.Amount) ? GreenLightColor : OrangeLightColor,
                                    indent: 3
                                ));

                                if (conditionMet) // Only when at least one condition has already been met.
                                {
                                    conditionMet = (hits >= Convert.ToInt32(currCondition.Amount)); // Check if current condition has been met.
                                }
                            }
                        }

                        #endregion Fixed Amount Mode.

                        // ==============
                        // B. Combo Mode.
                        // ==============

                        #region Combo Mode.

                        string allowedComboStats = "";

                        foreach (Condition currCondition in conditionListEntries[i])
                        {
                            if (currCondition.Amount == 0)
                            {
                                int hits = currEquipment.MatchStat(currCondition.ShortTerm);

                                AddMsg(new RtMessage(
                                    message:
                                    (hits > 0 ? "✓" : "❌") + " '" + currCondition.LongTerm + "': " + hits,
                                    color:
                                    hits > 0 ? GreenLightColor : OrangeLightColor,
                                    indent: 3
                                ));

                                if (conditionMet) // Only when at least one condition has already been met.
                                {
                                    conditionMet = (hits > 0); // Check if current condition has been met.

                                    // Check if the current condition has been met.
                                    // [DEVNOTE] conditionMet check not necessary here.
                                    // Would break if set to false above and wouldn't enter the next step anyway.
                                    allowedComboStats += currCondition.ShortTerm + " | ";
                                }
                            }
                        }

                        // -----------------------------------------------
                        // Combo mode can not have any other stat combined
                        // other than the ones in the condition entry.
                        // -----------------------------------------------

                        // Check current equipment stats against each config short term.
                        // [DEVNOTE] If allowedComboStats isn't an empty string at this point,
                        //           current condition list entry is a combo.
                        //           Additionally, check if current condition has (still) been met.
                        if (!string.IsNullOrEmpty(allowedComboStats) & conditionMet)
                        {
                            foreach (string[] currConfigTerm in cfgTerms)
                            {
                                // If not an allowed stat.
                                if (!allowedComboStats.ToLower().Contains(currConfigTerm[1].ToLower()))
                                {
                                    int hits = currEquipment.MatchStat(currConfigTerm[1]);

                                    // If a non allowed stat has been found in current equipment.
                                    if (hits > 0)
                                    {
                                        conditionMet = false;
                                    }
                                }

                                // Break the foreach loop if a non allowed stat was found
                                // and notify.
                                if (!conditionMet)
                                {
                                    AddMsg(new RtMessage(
                                        message:
                                        "❌ Non allowed combo stat detected:",
                                        color:
                                        OrangeLightColor,
                                        indent: 3
                                    ));
                                    AddMsg(new RtMessage(
                                         message:
                                         "'" + currConfigTerm[0] + "'",
                                         color:
                                         OrangeLightColor,
                                         indent: 7
                                     ));

                                    break;
                                }
                            }
                        }

                        #endregion Combo Mode.

                        if (conditionMet)
                        {
                            break;
                        }
                    }

                    // ---------------------
                    // LOG RESULTS/DECISION.
                    // ---------------------

                    AddMsg("=========================");
                    if (conditionListEntries.Count() > 1)
                    {
                        AddMsg(new RtMessage(
                            message:
                            "• " + (conditionMet ? "Condition met." : "No condition met."),
                            color:
                            conditionMet ? GreenLightColor : OrangeLightColor
                            ));
                    }
                    else
                    {
                        AddMsg(new RtMessage(
                            message:
                            "• " + (conditionMet ? "Condition met." : "Condition hasn't been met."),
                            color:
                            conditionMet ? GreenLightColor : OrangeLightColor
                            ));
                    }
                    if (!InfoGui.PreviewCapture)
                    {
                        if (conditionMet)
                        {
                            AddMsg(new RtMessage("=> Accepting new attributes.", color: GreenLightColor, indent: 3));
                        }
                        else
                        {
                            AddMsg(new RtMessage("=> Retaining old attributes.", indent: 3));
                        }
                    }

                    #endregion Match Condition(s).
                }

                if (!InfoGui.PreviewCapture)
                {
                    AddMsg("• Number of rolls: " + nrRolls.ToString() + " / " + maxNrRolls.ToString() + ".");
                }

                #endregion Judge conditions.

                // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

                // [DEVNOTE] Invoke due to threading (else not in sync).
                // OVERWRITE TESSERACT RESULT FILE.
                InfoGui.rTxtBoxInfo.Invoke((MethodInvoker)(() =>
                    File.WriteAllText(outputPath + ".txt",
                        "[ Raw OCR ]" + Environment.NewLine +
                        rawCapturedText + Environment.NewLine +
                        "=========================" + Environment.NewLine +
                        "[ Custom Optimized OCR ]" + Environment.NewLine +
                        currEquipment.OcrText.TrimEnd() + Environment.NewLine +
                        "=========================" + Environment.NewLine +
                        InfoGui.rTxtBoxInfo.Text,
                        System.Text.Encoding.UTF8
                        )
                ));

                // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

                // =============
                // == ACTIONS ==
                // =============

                #region Perform actions based on matching decision.

                if (!InfoGui.PreviewCapture & keepRunning) // [DEVNOTE] keepRunning could have been set to false in history check.
                {
                    // Wait for a bit before rejecting or accepting current roll.
                    Thread.Sleep(AwaitAcceptRejectAction);

                    Ocr_Token.ThrowIfCancellationRequested();

                    if (conditionMet)
                    {
                        // Accept the new attributes.
                        MoveMouse((uint)NewClickPoint.X, (uint)NewClickPoint.Y);
                        DoLeftMouseClick((uint)NewClickPoint.X, (uint)NewClickPoint.Y);

                        string successSound = Tesseract.Ocr.AssemblyCodeBaseDirectory + @"\Media\Sounds\Success.wav";
                        if (File.Exists(successSound)) // !InfoGui.PreviewCapture already checked above.
                        {
                            System.Media.SoundPlayer player = new System.Media.SoundPlayer(successSound);
                            player.Play();
                        }

                        keepRunning = false;
                    }
                    else
                    {
                        // Keep the old attributes.
                        MoveMouse((uint)RetainClickPoint.X, (uint)RetainClickPoint.Y);
                        DoLeftMouseClick((uint)RetainClickPoint.X, (uint)RetainClickPoint.Y);

                        // Reproduce?
                        if (nrRolls == maxNrRolls) // Check if not at allowed rolls.
                        {
                            AddMsg(Environment.NewLine + Environment.NewLine + "Max number of rolls reached. Halted.");

                            string doneSound = Tesseract.Ocr.AssemblyCodeBaseDirectory + @"\Media\Sounds\Done.wav";
                            if (File.Exists(doneSound)) // !InfoGui.PreviewCapture already checked above.
                            {
                                System.Media.SoundPlayer player = new System.Media.SoundPlayer(doneSound);
                                player.Play();
                            }

                            keepRunning = false;
                        }
                        else
                        {
                            MoveMouse((uint)ReproduceClickPoint.X, (uint)ReproduceClickPoint.Y);
                            // Additional wait until in-game reproduce button becomes available again.
                            Thread.Sleep(AwaitIngameReproduceButtonAvailable); // 1500 = measured | 1500+ = safer value.
                            DoLeftMouseClick((uint)ReproduceClickPoint.X, (uint)ReproduceClickPoint.Y);
                            // Additional wait until ingame stats have been rolled.
                            Thread.Sleep(AwaitIngameStatsRolled);
                        }
                    }
                }
                else // if PreviewCapture = true;
                {
                    keepRunning = false;
                }

                #endregion Perform actions based on matching decision.
            }

            #endregion Main roll loop.
        }

        private System.Drawing.Image ResizeImage(System.Drawing.Image imgToResize, double scaleFactor = 2)
        {
            //Get the image current width.
            int sourceWidth = imgToResize.Width;
            //Get the image current height.
            int sourceHeight = imgToResize.Height;

            // Recalculate width & height based on given scale factor.
            int destWidth = (int)(sourceWidth * scaleFactor);
            int destHeight = (int)(sourceHeight * scaleFactor);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Draw image with new width and height.
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (System.Drawing.Image)b;
        }

        public void DoLeftMouseClick(uint posX, uint posY)
        {
            // Call the imported function with the cursor's current position.
            //uint X = (uint)Cursor.Position.X;
            //uint Y = (uint)Cursor.Position.Y;

            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, posX, posY, 0, 0); // Current app will lose focus.
            Thread.Sleep(50);

            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, posX, posY, 0, 0); // Actual click.
            Thread.Sleep(50);

            // Refocus InfoGui (in order for Esc key capture to work without having to do a keyboard hook).
            //if (InfoGui.InvokeRequired)
            //{
            //    InfoGui.BeginInvoke(new MethodInvoker(delegate
            //    {
            //        InfoGui.Focus();
            //    }));
            //}
            //else
            //{
            //    InfoGui.Focus();
            //}
        }

        //public void MoveMouse(uint posX, uint posY)
        //{
        //    this.Cursor = new Cursor(Cursor.Current.Handle);
        //    Cursor.Position = new Point((int)posX, (int)posY);

        //    Thread.Sleep(50);
        //}

        #region Low Level Methods.

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        // Mouse actions.
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;

        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);

        public void MoveMouse(uint posX, uint posY)
        {
            SetCursorPos((int)posX, (int)posY);
            Thread.Sleep(50);
        }

        #endregion Low Level Methods.

        #endregion Methods.
    }
}