using NUnit.Framework;

namespace RuleEvaluator.Test
{
    [TestFixture]
    public class CellValidateModuleDetectorTest
    {
        [Test]
        public void DetectByFilterData_InputString_ReturnsRegex()
        {
            var detector = new CellValidateModuleDetector();
            Assert.IsInstanceOf<CellValidateModuleRegex>(detector.DetectByFilterData("Text"));
        }

        [Test]
        public void DetectByFilterData_InputStringRegex_ReturnsRegex()
        {
            var detector = new CellValidateModuleDetector();
            Assert.IsInstanceOf<CellValidateModuleRegex>(detector.DetectByFilterData("Text[0-9]"));
        }

        [Test]
        public void DetectByFilterData_InputStringRegex_ReturnsDecimalRange()
        {
            var detector = new CellValidateModuleDetector();
            Assert.IsInstanceOf<CellValidateModuleDecimalInterval>(detector.DetectByFilterData(new CellValidateFilterDecimalInterval(1, true, 10, true)));
        }
    }
}
