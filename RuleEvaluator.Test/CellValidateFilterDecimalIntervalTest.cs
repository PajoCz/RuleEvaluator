using NUnit.Framework;

namespace RuleEvaluator.Test
{
    [TestFixture]
    public class CellValidateFilterDecimalIntervalTest
    {
        [TestCase(-20, ExpectedResult = false)]
        [TestCase(-10.001, ExpectedResult = false)]
        [TestCase(-10, ExpectedResult = false)]
        [TestCase(0, ExpectedResult = true)]
        [TestCase(5, ExpectedResult = false)]
        [TestCase(5.001, ExpectedResult = false)]
        [TestCase(10, ExpectedResult = false)]
        public bool Validate_NothingIncluding(decimal p_Value)
        {
            return new CellValidateFilterDecimalInterval(-10m, false, 5m, false).Validate(p_Value);
        }

        [TestCase(-20, ExpectedResult = false)]
        [TestCase(-10.001, ExpectedResult = false)]
        [TestCase(-10, ExpectedResult = true)]
        [TestCase(0, ExpectedResult = true)]
        [TestCase(5, ExpectedResult = false)]
        [TestCase(5.001, ExpectedResult = false)]
        [TestCase(10, ExpectedResult = false)]
        public bool Validate_FromIncluding(decimal p_Value)
        {
            return new CellValidateFilterDecimalInterval(-10m, true, 5m, false).Validate(p_Value);
        }

        [TestCase(-20, ExpectedResult = false)]
        [TestCase(-10.001, ExpectedResult = false)]
        [TestCase(-10, ExpectedResult = false)]
        [TestCase(0, ExpectedResult = true)]
        [TestCase(5, ExpectedResult = true)]
        [TestCase(5.001, ExpectedResult = false)]
        [TestCase(10, ExpectedResult = false)]
        public bool Validate_ToIncluding(decimal p_Value)
        {
            return new CellValidateFilterDecimalInterval(-10m, false, 5m, true).Validate(p_Value);
        }

        [TestCase(-20, ExpectedResult = false)]
        [TestCase(-10.001, ExpectedResult = false)]
        [TestCase(-10, ExpectedResult = true)]
        [TestCase(0, ExpectedResult = true)]
        [TestCase(5, ExpectedResult = true)]
        [TestCase(5.001, ExpectedResult = false)]
        [TestCase(10, ExpectedResult = false)]
        public bool Validate_BothIncluding(decimal p_Value)
        {
            return new CellValidateFilterDecimalInterval(-10m, true, 5m, true).Validate(p_Value);
        }

        [TestCase("INTERVAL(10,2000)", 10, false, 2000, false)]
        [TestCase("INTERVAL<10,2000)", 10, true, 2000, false)]
        [TestCase("INTERVAL<10,2000>", 10, true, 2000, true)]
        [TestCase("INTERVAL(10,2000>", 10, false, 2000, true)]
        [TestCase(" INTERVAL ( 10, 2 000 ) ", 10, false, 2000, false)]
        [TestCase(" IN TER VAL ( 10, 2 000 ) ", 10, false, 2000, false)]
        [TestCase("Interval<10,15)", 10, true, 15, false)]
        public void CreateFromString_Correct(string p_Text, decimal p_From, bool p_FromIncluding, decimal p_To, bool p_ToIncluding)
        {
            var res = CellValidateFilterDecimalInterval.CreateFromString(p_Text);
            Assert.IsNotNull(res, "Text is not in syntax of Interval");
            Assert.AreEqual(p_From, res.From);
            Assert.AreEqual(p_FromIncluding, res.FromClosedIncluding);
            Assert.AreEqual(p_To, res.To);
            Assert.AreEqual(p_ToIncluding, res.ToClosedIncluding);
        }

        [TestCase("INTERVALY(10,2000)")]
        [TestCase("INTERVAL(10,2000]")]
        [TestCase("INTERVAL(10,2000,50)")]
        [TestCase("INTERVAL(A,B)")]
        public void CreateFromString_IncorrectReturnNull(string p_Text)
        {
            Assert.IsNull(CellValidateFilterDecimalInterval.CreateFromString(p_Text));
        }
    }
}
