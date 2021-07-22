using System;
using System.Collections.Generic;

namespace EPW_Recaster
{
    [Serializable]
    internal class Condition
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

    /// <summary>
    /// Used for storing/loading/working with different condition list entries > containing conditions.
    /// </summary>
    [Serializable]
    internal class ConditionListEntry : List<Condition> { }

    /// <summary>
    /// Used for storing/loading/working with different condition lists > containing condition list entries > containing conditions.
    /// </summary>
    [Serializable]
    internal class ConditionList : List<ConditionListEntry> { }
}