using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Tesseract
{
    public static class Ocr
    {
        private static string AssemblyCodeBase { get; set; } = new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath;
        public static string AssemblyCodeBaseDirectory { get; set; } = Path.GetDirectoryName(AssemblyCodeBase);

        public static string VersionPath { get; set; } = @"Tesseract\5 (Alpha)\";

        public static string Executable { get; set; } =
            Path.Combine(
                AssemblyCodeBaseDirectory,
                VersionPath + @"tesseract.exe"
            );

        public static string Version
        {
            get
            {
                return VersionPath.Split('\\')[1].Trim(); // f.e. 5 (Alpha)
            }
        }

        public static string TempCacheDirectory { get; set; } = AssemblyCodeBaseDirectory +
            @"\" + VersionPath.Split('\\')[0].Trim() + // f.e. \Tesseract
            @"\(TempCache)";

        public static string Language { get; set; } = "eng";

        // Reference:
        // https://tesseract-ocr.github.io/tessdoc/ImproveQuality.html#page-segmentation-method
        public static short PageSegMode { get; set; } = 3; // Fully automatic page segmentation, but no OSD. (Default)

        public static string GetText(string imgPath, string outputPath)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "\"" + Executable + "\"",
                    // [DEVNOTE] ChangeExtension: Tesseract does not want .txt or any extension to be added to outputPath. Removes if needed.
                    Arguments = "\"" + imgPath + "\" \"" + Path.ChangeExtension(outputPath, null) + "\" -l " + Language + " --psm " + PageSegMode,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            //System.Windows.Forms.Clipboard.SetText(process.StartInfo.FileName + " " + process.StartInfo.Arguments);

            process.Start();
            process.WaitForExit();

            string capturedText = File.ReadAllText(outputPath + ".txt", Encoding.UTF8);

            // Remove blank lines.
            capturedText = Regex.Replace(capturedText, @"^\s*$\n|\r", string.Empty, RegexOptions.Multiline).TrimEnd();

            // [DEVNOTE] Overwrite the file written by Tesseract with cleaned contents.
            File.WriteAllText(outputPath + ".txt", capturedText, Encoding.UTF8);

            return capturedText;
        }
    }
}