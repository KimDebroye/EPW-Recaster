using System.Drawing;

namespace EPW_Recaster
{
    internal class RtMessage
    {
        public string Message { get; set; } = null;

        public Color Color { get; set; } = new Color(); // Default color.

        public bool CustomColor { get; set; } = false;
        public bool Bold { get; set; } = false;

        public RtMessage(string message, Color color, bool bold = false, int indent = 0)
        {
            Message = new string(' ', indent) + message; // Indent (if applicable) and set message.
            Color = color;
            CustomColor = true;
            Bold = bold;
        }

        public RtMessage(string message, string color = "", bool bold = false, int indent = 0)
        {
            Message = new string(' ', indent) + message; // Indent (if applicable) and set message.

            if (!string.IsNullOrEmpty(color))
            {
                try
                {
                    Color = Color.FromName(color);
                }
                catch
                {
                    // Ignore given color.
                    CustomColor = false;
                }
            }
            else
            {
                CustomColor = false;
            }

            Bold = bold;
        }
    }
}