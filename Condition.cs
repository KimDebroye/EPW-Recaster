using System;

namespace EPW_Recaster
{
    public partial class MainGui
    {
        [Serializable]
        public class Condition
        {
            public int Amount { get; set; } = 1;
            public string LongTerm { get; set; } = null;
            public string ShortTerm { get; set; } = null;

            public Condition(int amount = 1, string longTerm = "", string shortTerm = "")
            {
                Amount = amount;
                LongTerm = longTerm;
                ShortTerm = shortTerm;
            }
        }

    }
}