using System.Drawing;

namespace EPW_Recaster
{
    internal class RtMessage
    {
        public string Message { get; set; } = null;

        public Color Color { get; set; } = new Color();

        public RtMessage(string message, Color color)
        {
            Message = message;
            Color = color;
        }
        public RtMessage(string message, string color)
        {
            Message = message;
            try
            {
                Color = Color.FromName(color);
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        public RtMessage(string message)
        {
            Message = message;
        }
    }
}