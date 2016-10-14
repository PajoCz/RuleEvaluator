using NUnit.Framework;

namespace RuleEvaluator.Test
{
    [TestFixture]
    public class CellValidateRegexTest
    {        
        [TestCase("1[1-9]", 11, ExpectedResult = true)]
        [TestCase("1[1-9]", 11d, ExpectedResult = true)]
        [TestCase("1[1-9]", "11", ExpectedResult = true)]
        [TestCase("1[1-9]", 15, ExpectedResult = true)]
        [TestCase("1[1-9]", "15", ExpectedResult = true)]
        [TestCase("1[1-9]", 19, ExpectedResult = true)]
        [TestCase("1[1-9]", "19", ExpectedResult = true)]
        [TestCase("1[1-9]", "", ExpectedResult = false)]
        [TestCase("1[1-9]", 20, ExpectedResult = false)]
        [TestCase("1[1-9]", "20", ExpectedResult = false)]
        [TestCase("1[1-9]", 111, ExpectedResult = false)]
        [TestCase("1[1-9]", "111", ExpectedResult = false)]
        public bool Validate_ValueDataNotNull(object p_CellFilter, object p_ValueDataForValidating)
        {
            CellValidateRegex validate = new CellValidateRegex();
            return validate.Validate(p_CellFilter, p_ValueDataForValidating);
        }

        [TestCase("1[1-9]", null, ExpectedResult = false)]
        [TestCase(".*", null, ExpectedResult = true)]
        public bool Validate_ValueDataNull(object p_CellFilter, object p_ValueDataForValidating)
        {
            CellValidateRegex validate = new CellValidateRegex();
            return validate.Validate(p_CellFilter, p_ValueDataForValidating);
        }

    }
}
