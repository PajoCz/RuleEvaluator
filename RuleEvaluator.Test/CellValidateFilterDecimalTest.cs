using NUnit.Framework;

namespace RuleEvaluator.Test
{
    [TestFixture]
    public class CellValidateFilterDecimalTest
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
            return new CellValidateFilterDecimal(-10m, false, 5m, false).Validate(p_Value);
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
            return new CellValidateFilterDecimal(-10m, true, 5m, false).Validate(p_Value);
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
            return new CellValidateFilterDecimal(-10m, false, 5m, true).Validate(p_Value);
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
            return new CellValidateFilterDecimal(-10m, true, 5m, true).Validate(p_Value);
        }
    }
}
