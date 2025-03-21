﻿using System;
using NUnit.Framework;

namespace RuleEvaluator.Test
{
    [TestFixture]
    public class CellValidateModuleRegexTest
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
        [TestCase("Text.*", "Text\nNewLine", ExpectedResult = true)]
        [TestCase("Text.*", "Text\r\nNewLine", ExpectedResult = true)]
        [TestCase("Text\\nNew.*", "Text\nNewLine", ExpectedResult = true)]
        [TestCase("Text\\nOld.*", "Text\nNewLine", ExpectedResult = false)]
        public bool? Validate_ValueDataNotNull(object p_CellFilter, object p_ValueDataForValidating)
        {
            CellValidateModuleRegex validateModule = new CellValidateModuleRegex();
            return validateModule.Validate(p_CellFilter, p_ValueDataForValidating);
        }

        [TestCase("1[1-9]", null, ExpectedResult = false)]
        [TestCase(".*", null, ExpectedResult = true)]
        public bool? Validate_ValueDataNull(object p_CellFilter, object p_ValueDataForValidating)
        {
            CellValidateModuleRegex validateModule = new CellValidateModuleRegex();
            return validateModule.Validate(p_CellFilter, p_ValueDataForValidating);
        }

        [TestCase(null, "AnyValue")]
        public void Validate_NullCellFilter_ThrowsException(object p_CellFilter, object p_ValueDataForValidating)
        {
            CellValidateModuleRegex validateModule = new CellValidateModuleRegex();
            Assert.Throws<ArgumentNullException>(() => validateModule.Validate(p_CellFilter, p_ValueDataForValidating));
        }
    }
}
