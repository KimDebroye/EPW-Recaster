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
        // For logging purposes.
        Bitmap img;

        // To be used by Tesseract (OCR).
        Bitmap upscaledImg;
        string upscaledImgPath = Tesseract.Ocr.TempCacheDirectory + @"\tmp.png";

        int CaptureRegionWidth { get; set; } = 0;
        int CaptureRegionHeight { get; set; } = 0;

        Point CaptureRegionUpperLeftPoint { get; set; } = new Point();

        Point RetainClickPoint { get; set; } = new Point();

        Point NewClickPoint { get; set; } = new Point();

        Point ReproduceClickPoint { get; set; } = new Point();

        int AwaitIngameReproduceButtonAvailable { get; set; } = 1750; // Time it takes for the in-game reproduce button to become available again.
        int AwaitIngameStatsRolled { get; set; } = 1250; // Time it takes for the in-game stats to be rolled.
        int AwaitAcceptRejectAction { get; set; } = 1750; // Time to wait before accepting/rejecting a roll.

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

            if (lvConditions.InvokeRequired)
            {
                lvConditions.Invoke(new MethodInvoker(delegate
                {
                    foreach (ListViewItem lvi in lvConditions.Items)
                    {
                        conditions.Add((Condition)lvi.Tag);
                    }
                }));
            }
            else
            {
                foreach (ListViewItem lvi in lvConditions.Items)
                {
                    conditions.Add((Condition)lvi.Tag);
                }
            }

            // Get subconditions for this preview or batch roll.
            List<Condition> subConditions = new List<Condition>();

            if (lvSubConditions.InvokeRequired)
            {
                lvSubConditions.Invoke(new MethodInvoker(delegate
                {
                    foreach (ListViewItem lvi in lvSubConditions.Items)
                    {
                        subConditions.Add((Condition)lvi.Tag);
                    }
                }));
            }
            else
            {
                foreach (ListViewItem lvi in lvSubConditions.Items)
                {
                    subConditions.Add((Condition)lvi.Tag);
                }
            }

            #endregion

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
                #endregion

                // Display in info box.
                AddMsg();
                #endregion

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
                                    AddMsg("[ Detected Blue Weapon Stats ]");
                                else
                                    AddMsg("[ Detected Blue Gear Stats ]");

                                foreach (Stat blueStat in blueStats)
                                {
                                    //AddMsg(new RtMessage(blueStats[i], "Blue"));
                                    AddMsg("⮩ " + blueStat.FormattedStat);

                                    if (!uniqueStatWarningShown)
                                    {
                                        // Check if the stat contains 'Purify'...
                                        // If it does, give the user a warning about missing stat(s).
                                        if (blueStat.Identifier.Contains("Purify"))
                                        {
                                            DialogResult userChoice = MessageBox.Show(
                                                "A 'Purify' stat was detected.\r\n" +
                                                "\r\n" +
                                                "This will result in incorrect judging and handling of conditions " +
                                                "whenever this stat is rolled, since the in-game window needs scrolling " +
                                                "for all stats to be readable (falling out of scope for " + Application.ProductName + ").\r\n" +
                                                "\r\n" +
                                                "In short:\r\n" +
                                                "Some rolls could miss out on accepting a possibly valid roll.\r\n" +
                                                "\r\n" +
                                                "Continue rolling?",
                                                "IMPORTANT WARNING",
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

                                validEntry = true;

                                // Increase roll counter if the current capture was a valid one.
                                nrRolls++;
                            }
                            else
                            {
                                AddMsg("[ Equipment Not Identifiable ]");
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
                        AddMsg("No new valid roll information detected (yet).");
                    }
                }
                else
                {
                    //AddMsg("No text found in region.");
                    AddMsg("No new valid roll information detected (yet).");
                }
                #endregion

                // ======================
                // == JUDGE CONDITIONS ==
                // ======================

                #region Judge conditions.
                // Check history first, stop if needed.
                // [DEVNOTE] Only check when history is filled entirely.
                if (!InfoGui.PreviewCapture & capturedTextHistory.Count == capturedTextHistoryCapacity && capturedTextHistory.Distinct().Count() == 1)
                {
                    AddMsg(); // Clear info box first.

                    AddMsg("It doesn't seem necessary to roll any further." + Environment.NewLine +
                        "The roll process has been halted automatically.");
                    AddMsg("Either:" + Environment.NewLine +
                        "- the in-game number of Perfect Elements" + Environment.NewLine +
                        "  has been depleted" + Environment.NewLine +
                        "- the conditions have already been" + Environment.NewLine +
                        "  met and accepted" + Environment.NewLine +
                        "- the game client disconnected" + Environment.NewLine +
                        "- the tool wasn't able to read rolled stats correctly" + Environment.NewLine +
                        "  > check capture region boundaries" + Environment.NewLine +
                        "  > check if tool is overlapping" + Environment.NewLine +
                        "    game client to be rolled");
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

                //int totalHits = 0;
                bool conditionMet = false;
                bool subConditionMet = false;

                if (validEntry)
                {
                    // ----------------------------------
                    // MATCH AGAINST REQUIRED CONDITIONS.
                    // ----------------------------------

                    #region Required condition(s) matching.
                    AddMsg("=========================");
                    AddMsg("[  Required Conditions  ]");

                    Condition matchedCondition = null;

                    foreach (Condition currCondition in conditions)
                    {
                        int hits = currEquipment.MatchStat(currCondition.ShortTerm);
                        //totalHits += hits;

                        AddMsg(
                            "# '" + currCondition.LongTerm + "' detected: " + hits + " / " + currCondition.Amount + "."
                            );

                        if (!conditionMet) // Only when at least one condition hasn't already been met.
                        {
                            conditionMet = (hits >= Convert.ToInt32(currCondition.Amount)); // Check if current condition has been met.

                            // If current condition is met, store it for additional checks in subconditions (f.e. 5 x amount check).
                            if (conditionMet)
                                matchedCondition = currCondition;

                            /*if (conditionMet)
                                break;*/ // [DEVNOTE] Don't break for informative display purposes.
                        }
                    }
                    #endregion

                    // ----------------------------------
                    // MATCH AGAINST OPTIONAL CONDITIONS.
                    // ----------------------------------

                    #region Additional condition(s) matching.
                    // Only when at least one subcondition has been set
                    // & one of required conditions is met.
                    if (subConditions.Count > 0)
                    {
                        AddMsg("=========================");
                        AddMsg("[ Additional Conditions ]");

                        if (conditionMet)
                        {
                            // DO NOT CHECK WHEN MATCHED REQUIRED CONDITION HAS AN ALREADY MAXED AMOUNT OF STATS.
                            if (matchedCondition.Amount != 5)
                            {
                                // For each set subcondition, check whether it matches.
                                foreach (Condition currSubCondition in subConditions)
                                {
                                    int subHits = currEquipment.MatchStat(currSubCondition.ShortTerm);
                                    //totalHits += hits;

                                    AddMsg(
                                        "# '" + currSubCondition.LongTerm + "' detected: " + subHits + " / " + currSubCondition.Amount + "."
                                        );

                                    if (!subConditionMet) // Only when at least one subcondition hasn't already been met.
                                    {
                                        subConditionMet = (subHits >= Convert.ToInt32(currSubCondition.Amount)); // Check if current subcondition has been met.

                                        /*if(subConditionMet)
                                            break;*/ // [DEVNOTE] Don't break for informative display purposes.
                                    }
                                }
                            }
                            else
                            {
                                subConditionMet = true;
                                AddMsg("( Required condition already using 5 stats (max). Skipped. )");
                            }
                        }
                        else
                        {
                            AddMsg("( Skipped. )");
                        }
                    }
                    else
                    {
                        // If no subconditions were set, check as true.
                        subConditionMet = true;
                    }
                    #endregion

                    // ---------------------
                    // LOG RESULTS/DECISION.
                    // ---------------------

                    //AddMsg("=====" + Environment.NewLine + "Total hits: " + totalHits + "."); // [DEVNOTE] Per roll only, not overall. Hidden to avoid confusion.
                    AddMsg("=========================");
                    AddMsg("• Conditions met: " + ((conditionMet & subConditionMet) ? "yes." : "no."));
                    if (!InfoGui.PreviewCapture)
                    {
                        if (conditionMet & subConditionMet)
                        {
                            AddMsg("  => Accepting new attributes.");
                        }
                        else
                        {
                            AddMsg("  => Retaining old attributes.");
                        }
                    }
                }

                if (!InfoGui.PreviewCapture)
                {
                    AddMsg("• Number of rolls: " + nrRolls.ToString() + " / " + maxNrRolls.ToString() + ".");
                }
                #endregion

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

                    if (conditionMet & subConditionMet)
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
                #endregion
            }

            #endregion
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

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        // Mouse actions.
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

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
            if (InfoGui.InvokeRequired)
            {
                InfoGui.BeginInvoke(new MethodInvoker(delegate
                {
                    InfoGui.Focus();
                }));
            }
            else
            {
                InfoGui.Focus();
            }
        }

        //public void MoveMouse(uint posX, uint posY)
        //{
        //    this.Cursor = new Cursor(Cursor.Current.Handle);
        //    Cursor.Position = new Point((int)posX, (int)posY);

        //    Thread.Sleep(50);
        //}

        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);

        public void MoveMouse(uint posX, uint posY)
        {
            SetCursorPos((int)posX, (int)posY);
            Thread.Sleep(50);
        }
    }
}
