using System;
using NUnit.Framework;

namespace RuleEvaluator.Test
{
    [TestFixture]
    public class CellValidateDecimalTest
    {
        [Test]
        public void Validate_CorrectTypes_NotThrownException()
        {
            var validator = new CellValidateDecimal();
            var filter = new CellValidateDecimalFilter();
            var valueIsOk = validator.Validate(filter, 10m);
        }


        [Test]
        public void Validate_CellFilterNull_ThrowsException()
        {
            var validator = new CellValidateDecimal();
            Assert.Throws<ArgumentNullException>(() => validator.Validate(null, 10m));
        }

        [Test]
        public void Validate_DataNull_ThrowsException()
        {
            var validator = new CellValidateDecimal();
            var filter = new CellValidateDecimalFilter();
            Assert.Throws<ArgumentNullException>(() => validator.Validate(filter, null));
        }

        [Test]
        public void Validate_FilterNotTypeOfCellValidateDecimalFilter_ThrowsException()
        {
            var validator = new CellValidateDecimal();
            var filter = new object();
            Assert.Throws<ArgumentException>(() => validator.Validate(filter, 10m));
        }

        [Test]
        public void Validate_ValueNotTypeOfDecimal_ThrowsException()
        {
            var validator = new CellValidateDecimal();
            var filter = new CellValidateDecimalFilter();
            Assert.Throws<ArgumentException>(() => validator.Validate(filter, "text"));
        }

        /// <summary>
        /// TODO: Is integer correct input? Implement any conversion in Validate method and accept integer inputs?
        /// </summary>
        [Test]        
        public void Validate_ValueNotTypeOfDecimal_SetInteger_ThrowsException()
        {
            var validator = new CellValidateDecimal();
            var filter = new CellValidateDecimalFilter();
            Assert.Throws<ArgumentException>(() => validator.Validate(filter, 10));
        }

        /// <summary>
        /// TODO: Is integer correct input? Implement any conversion in Validate method and accept integer inputs?
        /// </summary>
        [Test]
        public void Validate_ValueNotTypeOfDecimal_SetDouble_ThrowsException()
        {
            var validator = new CellValidateDecimal();
            var filter = new CellValidateDecimalFilter();
            Assert.Throws<ArgumentException>(() => validator.Validate(filter, 10d));
        }
    }
}
