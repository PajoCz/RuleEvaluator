using System.Text.RegularExpressions;

namespace RuleEvaluator
{
    /// <summary>
    /// Deserialized object of interval string syntax : INTERVAL[\(<]\d*,\d*\)>)
    /// </summary>
    public class CellValidateFilterDecimalInterval
    {
        public CellValidateFilterDecimalInterval(decimal p_From, bool p_FromClosedIncluding, decimal p_To, bool p_ToClosedIncluding)
        {
            From = p_From;
            FromClosedIncluding = p_FromClosedIncluding;
            To = p_To;
            ToClosedIncluding = p_ToClosedIncluding;
        }

        public CellValidateFilterDecimalInterval()
        {
        }

        public decimal From { get; set; }
        /// <summary>
        /// If begin of interval is Closed (including From number) otherwise start of interval is Opened (excluding From number)
        /// </summary>
        public bool FromClosedIncluding { get; set; }
        public decimal To { get; set; }
        /// <summary>
        /// If end of interval is Closed (including To number) otherwise end of interval is Opened (excluding To number)
        /// </summary>
        public bool ToClosedIncluding { get; set; }

        public bool Validate(decimal p_Value)
        {
            var result = true;
            if (FromClosedIncluding && p_Value < From) result = false;
            if (!FromClosedIncluding && p_Value <= From) result = false;
            if (ToClosedIncluding && p_Value > To) result = false;
            if (!ToClosedIncluding && p_Value >= To) result = false;
            return result;
        }

        public static CellValidateFilterDecimalInterval CreateFromString(string p_Data)
        {
            Regex regex = new Regex(@"INTERVAL(?<FromOpenedClosed>[\(<])(?<FromNumber>\d*),(?<ToNumber>\d*)(?<ToOpenedClosed>[\)>])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var match = regex.Match(p_Data.Replace(" ",""));
            if (match.Success)
            {
                decimal from = decimal.Parse(match.Groups["FromNumber"].Value);
                bool fromIncluding = match.Groups["FromOpenedClosed"].Value == "<";
                decimal to = decimal.Parse(match.Groups["ToNumber"].Value);
                bool toIncluding = match.Groups["ToOpenedClosed"].Value == ">";
                return new CellValidateFilterDecimalInterval(from, fromIncluding, to, toIncluding);
            }
            return null;
        }

    }
}