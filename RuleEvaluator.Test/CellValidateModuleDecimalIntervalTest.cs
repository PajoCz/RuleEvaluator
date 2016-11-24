using System;
using NUnit.Framework;

namespace RuleEvaluator.Test
{
    [TestFixture]
    public class CellValidateModuleDecimalIntervalTest
    {
        [Test]
        public void Validate_CorrectTypes_NotThrowsException()
        {
            var validator = new CellValidateModuleDecimalInterval();
            var filter = new CellValidateFilterDecimalInterval();
            var valueIsOk = validator.Validate(filter, 10m);
        }


        [Test]
        public void Validate_CellFilterNull_ThrowsException()
        {
            var validator = new CellValidateModuleDecimalInterval();
            Assert.Throws<ArgumentNullException>(() => validator.Validate(null, 10m));
        }

        [Test]
        public void Validate_DataNull_ThrowsException()
        {
            var validator = new CellValidateModuleDecimalInterval();
            var filter = new CellValidateFilterDecimalInterval();
            Assert.Throws<ArgumentNullException>(() => validator.Validate(filter, null));
        }

        [Test]
        public void Validate_ValueNotTypeOfDecimal_SetString_ThrowsException()
        {
            var validator = new CellValidateModuleDecimalInterval();
            var filter = new CellValidateFilterDecimalInterval();
            Assert.Throws<FormatException>(() => validator.Validate(filter, "text"));
        }

        [Test]
        public void Validate_ValueNotTypeOfDecimal_SetString_INtervalConvertToDecimalAndAllIsOk()
        {
            var validator = new CellValidateModuleDecimalInterval();
            var filter = new CellValidateFilterDecimalInterval(10, true, 20, true);
            Assert.IsTrue(validator.Validate(filter, "10"));
        }

        [Test]
        public void Validate_ValueNotTypeOfDecimal_SetInteger_IntervalConvertToDecimalAndAllIsOk()
        {
            var validator = new CellValidateModuleDecimalInterval();
            var filter = new CellValidateFilterDecimalInterval(10, true, 20, true);
            Assert.IsTrue(validator.Validate(filter, 10));
        }

        [Test]
        public void Validate_ValueNotTypeOfDecimal_SetDouble_IntervalConvertToDecimalAndAllIsOk()
        {
            var validator = new CellValidateModuleDecimalInterval();
            var filter = new CellValidateFilterDecimalInterval(10, true, 20, true);
            Assert.IsTrue(validator.Validate(filter, 10d));
        }

        [Test]
        public void Validate_ValueNotTypeOfDecimal_SetFloat_IntervalConvertToDecimalAndAllIsOk()
        {
            var validator = new CellValidateModuleDecimalInterval();
            var filter = new CellValidateFilterDecimalInterval(10, true, 20, true);
            Assert.IsTrue(validator.Validate(filter, 10f));
        }
    }
}
