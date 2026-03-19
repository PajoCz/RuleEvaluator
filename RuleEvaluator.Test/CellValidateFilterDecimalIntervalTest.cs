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

        [TestCase("INTERVAL(10;2000)", 10, false, 2000, false)]
        [TestCase("INTERVAL<10;2000)", 10, true, 2000, false)]
        [TestCase("INTERVAL<10;2000>", 10, true, 2000, true)]
        [TestCase("INTERVAL(10;2000>", 10, false, 2000, true)]
        [TestCase(" INTERVAL ( 10; 2 000 ) ", 10, false, 2000, false)]
        [TestCase(" IN TER VAL ( 10; 2 000 ) ", 10, false, 2000, false)]
        [TestCase("Interval<10;15)", 10, true, 15, false)]
        [TestCase("Interval<10.123;15,123)", 10.123, true, 15.123, false)]
        [TestCase("Interval<10,123456789;15.123456789)", 10.123456789, true, 15.123456789, false)]
        public void CreateFromString_Correct(string p_Text, decimal p_From, bool p_FromIncluding, decimal p_To, bool p_ToIncluding)
        {
            var res = CellValidateFilterDecimalInterval.CreateFromString(p_Text);
            Assert.That(res, Is.Not.Null, "Text is not in syntax of Interval");
            Assert.That(res!.From, Is.EqualTo(p_From));
            Assert.That(res.FromClosedIncluding, Is.EqualTo(p_FromIncluding));
            Assert.That(res.To, Is.EqualTo(p_To));
            Assert.That(res.ToClosedIncluding, Is.EqualTo(p_ToIncluding));
        }

        [TestCase("INTERVALY(10;2000)")]
        [TestCase("INTERVAL(10;2000]")]
        [TestCase("INTERVAL(A;B)")]
        public void CreateFromString_IncorrectReturnNull(string p_Text)
        {
            Assert.That(CellValidateFilterDecimalInterval.CreateFromString(p_Text), Is.Null);
        }
    }
}
