using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace EPW_Recaster
{
    internal class Stat
    {
        public string Identifier { get; set; } = "";

        public string Separator { get; set; } = "";

        public string Value { get; set; } = "";

        public string FormattedStat
        {
            get
            {
                return Identifier + " " + Separator + " " + Value;
            }
        }

        public Stat(string identifier)
        {
            Identifier = identifier;
        }

        public Stat(string identifier, string value, string separator = "", bool isWeaponStat = false)
        {
            Identifier = identifier;
            Separator = separator;
            Value = value;
        }
    }

    internal class Equipment
    {
        private string ocrText;
        public string OcrText
        {
            get
            {
                return ocrText;
            }
            set
            {
                // Initial OCR text cleaning.
                value = value.
                    Replace("A ", "").
                    Replace("t +", "+").
                    Replace("vy ", "").
                    Replace("yy ", "").
                    Replace("\r\n+", " +").Replace("\r+", "\n+").Replace("\n+", " +").
                    Replace("\r\nseconds", " seconds").Replace("\rseconds", "\nseconds").Replace("\nseconds", " seconds").
                    Replace(")", "").
                    Replace("Old:", "").
                    Replace("Old", "").
                    Replace("New:", "").
                    Replace("New", "");

                string cleanedUpText = "";

                foreach (string line in value.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                {
                    string currentLine = line;

                    // Filter out starting words containing only one character (OCR related logical 'issue').
                    if (currentLine.Split(' ').Count() > 0)
                    {
                        if (currentLine.Split(' ').First().Count() == 1)
                        {
                            currentLine = currentLine.Replace(currentLine.Split(' ').First() + " ", "");
                        }
                    }

                    cleanedUpText += currentLine + Environment.NewLine;
                }

                // Remove lines that only contain whitespace.
                ocrText = Regex.Replace(cleanedUpText, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline); ;
            }
        }

        public bool IsWeapon
        {
            get
            {
                return !ocrText.ToLower().Contains("Metal Resistance".ToLower());
            }
        }

        public List<Stat> Stats
        {
            get
            {
                List<Stat> stats = new List<Stat>();

                foreach (string line in OcrText.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                {
                    // Format into Stat | Separator | Value .
                    string[] split = line.Split(new char[] { ':', '+', '-' }, StringSplitOptions.RemoveEmptyEntries);

                    if (split.Count() > 0)
                    {
                        if (!String.IsNullOrEmpty(split.First().Trim()))
                        {
                            stats.Add(
                                new Stat(
                                    identifier: split.First().Trim(),
                                    separator: ( /*if*/ (split.Count() > 1) ? line.Replace(split.First().Trim(), "").Replace(split.Last().Trim(), "").Trim() : /*else*/ "").Replace(":", "").Replace(" ", ""),
                                    value: ((split.Count() > 1) ? split.Last().Trim() : "")
                                    )
                            );

                            // [DEVNOTE] May be too destructive.
                            //           (in f.e. Preview Mode when a tiny bit of in-game left scroll UI elements are captured, data is scambled up)
                            /* 
                            // -------
                            // CLEANUP
                            // -------
                            // Remove stats not having some sort of a value.
                            // [DEVNOTE] Won't remove f.e. Spirit Blackhole entirely, since it has a value filled with part of descriptive string.
                            if (String.IsNullOrEmpty(stats.Last().Value))
                            {
                                stats.RemoveAt(stats.Count - 1);
                            }

                            // Get unique stats from Stats config file.
                            string[] configStats = File.ReadAllLines(Tesseract.Ocr.AssemblyCodeBaseDirectory + @"\Config\Stats.cfg");

                            // Add stats.
                            foreach (string cfgLine in configStats)
                            {
                                if (!cfgLine.Contains('#')) // Ignore custom comments.
                                {
                                    // Split by pipe character '|'.
                                    string[] splitTerm = cfgLine.Split('|');

                                    if (splitTerm.Count() == 2)
                                    {
                                        if (splitTerm.First().Contains("*"))
                                        {
                                            if (stats.Count() > 0)
                                            {
                                                if (stats.Last().Identifier.ToLower().Contains(splitTerm.First().Replace("*", "").Trim().ToLower()))
                                                {
                                                    stats.Last().Identifier = splitTerm.First().Replace("*", "(Unique)").Trim();
                                                    stats.Last().Separator = "";
                                                    stats.Last().Value = "";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            */
                        }
                    }
                }

                return stats;
            }
        }

        public List<Stat> BlueStats
        {
            get
            {
                List<Stat> blueStats = Stats;

                // Fetch & return blue stats based on type.
                if (IsWeapon)
                {
                    // (Try to) remove last entry/entries (being ... Physical and/or Magic Attack) | (!) Could miss out due to being out of visible in-game area (f.e. Purify).
                    blueStats.RemoveAll(stat => stat.Identifier.Contains(" Attack "));

                    return blueStats;
                }
                else
                {
                    // Take the first 4 entries.
                    return blueStats.Take(4).ToList<Stat>();
                }
            }
        }

        public List<Stat> WhiteStats
        {
            get
            {
                List<Stat> allStats = Stats;

                List<Stat> blueStats = BlueStats;

                // Count the blue stats.
                // Since blue stats should always be on top,
                // those items will be removed from all stats,
                // keeping only the non blue stats.
                if (blueStats.Count() > 0)
                {
                    for(int i=0; i < blueStats.Count(); i++)
                    {
                        allStats.RemoveAt(i);
                    }
                }
                
                return allStats;
            }
        }

        public int MatchStat(string term = "", bool forceOriginalOcrTextMatch = false)
        {
            if (!String.IsNullOrEmpty(OcrText))
            {
                string equipmentStats = "";

                if (!forceOriginalOcrTextMatch)
                {
                    try
                    {
                        // Try get blue stats only.
                        foreach (Stat blueStat in BlueStats)
                        {
                            equipmentStats += blueStat.FormattedStat + Environment.NewLine;
                        }
                    }
                    catch
                    {
                        // If getting blue stats, get full Ocr text.
                        equipmentStats = OcrText;
                    }
                }
                else
                {
                    equipmentStats = OcrText;
                }

                int hits = equipmentStats.ToLowerInvariant().Split(new string[] { term.ToLowerInvariant() }, StringSplitOptions.None).Count() - 1;
                return hits;
            }
            else
                return 0;
        }

        public Equipment(string ocrText = "")
        {
            OcrText = ocrText;
        }
    }
}