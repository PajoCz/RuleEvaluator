using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using RuleEvaluator.Repository.Contract;
using RuleEvaluator.Repository.Database;

namespace RuleEvaluator.Test
{
    [TestFixture]
    public class ModernApiTest
    {
        private static ICellFactory CreateFactory() => new DefaultCellFactory();

        #region DefaultCellFactory Tests

        [Test]
        public void DefaultCellFactory_CanCreateWithoutDI()
        {
            var factory = new DefaultCellFactory();
            var cell = factory.CreateCell("test");
            Assert.That(cell, Is.Not.Null);
            Assert.That(cell.FilterValue, Is.EqualTo("test"));
        }

        [Test]
        public void DefaultCellFactory_CustomMatcher_Works()
        {
            var customMatcher = new CellValidateModuleRegex();
            var factory = new DefaultCellFactory(customMatcher);
            var cell = factory.CreateCell("test.*");
            Assert.That(cell.Validate("testing"), Is.True);
        }

        [Test]
        public void DefaultCellFactory_NullMatcher_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new DefaultCellFactory(null!));
        }

        #endregion

        #region Diagnostics Tests

        [Test]
        public void FindWithDiagnostics_ReturnsMatch()
        {
            var cf = CreateFactory();
            var items = new RuleItems(cf);
            items.EnableDiagnostics = true;
            items.AddRuleItem(".*", cf.CreateCell("Result", CellInputOutputType.Output));

            var result = items.FindWithDiagnostics("anything");
            Assert.That(result.HasMatch, Is.True);
            Assert.That(result.Match!.Output(0).FilterValue, Is.EqualTo("Result"));
            Assert.That(result.Diagnostics, Is.Not.Null);
            Assert.That(result.Diagnostics!.RulesEvaluated, Is.EqualTo(1));
            Assert.That(result.Diagnostics.MatchCount, Is.EqualTo(1));
            Assert.That(result.Diagnostics.HasMatch, Is.True);
        }

        [Test]
        public void FindWithDiagnostics_NoMatch_ReturnsNullWithReason()
        {
            var cf = CreateFactory();
            var items = new RuleItems(cf);
            items.EnableDiagnostics = true;
            items.AddRuleItem("Exact", cf.CreateCell("Result", CellInputOutputType.Output));

            var result = items.FindWithDiagnostics("NoMatch");
            Assert.That(result.HasMatch, Is.False);
            Assert.That(result.Match, Is.Null);
            Assert.That(result.Diagnostics!.NoMatchReason, Is.Not.Null);
            Assert.That(result.Diagnostics.MatchCount, Is.EqualTo(0));
        }

        [Test]
        public void FindWithDiagnostics_DiagnosticsDisabled_NoDiagnostics()
        {
            var cf = CreateFactory();
            var items = new RuleItems(cf);
            items.EnableDiagnostics = false;
            items.AddRuleItem(".*", cf.CreateCell("Result", CellInputOutputType.Output));

            var result = items.FindWithDiagnostics("anything");
            Assert.That(result.HasMatch, Is.True);
            Assert.That(result.Diagnostics, Is.Null);
        }

        [Test]
        public void FindAllWithDiagnostics_ReturnsAllMatches()
        {
            var cf = CreateFactory();
            var items = new RuleItems(cf);
            items.EnableDiagnostics = true;
            items.AddRuleItem(".*", cf.CreateCell("Result1", CellInputOutputType.Output));
            items.AddRuleItem(".*", cf.CreateCell("Result2", CellInputOutputType.Output));
            items.AddRuleItem("NoMatch", cf.CreateCell("Result3", CellInputOutputType.Output));

            var result = items.FindAllWithDiagnostics("anything");
            Assert.That(result.HasMatch, Is.True);
            Assert.That(result.Matches.Count, Is.EqualTo(2));
            Assert.That(result.Diagnostics!.MatchCount, Is.EqualTo(2));
            Assert.That(result.Diagnostics.RulesEvaluated, Is.EqualTo(3));
        }

        #endregion

        #region Error Model Tests

        [Test]
        public void MatcherException_ThrownForUnknownValidateResult()
        {
            var factory = new DefaultCellFactory(new AlwaysNullMatcher());
            var cell = factory.CreateCell("pattern");
            Assert.Throws<MatcherException>(() => cell.Validate("value"));
        }

        [Test]
        public void InputParameterCountMismatchException_ContainsDetails()
        {
            var cf = CreateFactory();
            var ri = new RuleItem(cf, "a", "b");
            var ex = Assert.Throws<InputParameterCountMismatchException>(() => ri.ValidateInput("TestRule", "a"));
            Assert.That(ex!.ExpectedCount, Is.EqualTo(2));
            Assert.That(ex.ActualCount, Is.EqualTo(1));
        }

        [Test]
        public void RuleNotFoundException_ContainsDetails()
        {
            var ex = new RuleNotFoundException("TestKey", new object[] { "a", "b" });
            Assert.That(ex.RuleSetKey, Is.EqualTo("TestKey"));
            Assert.That(ex.Parameters, Is.Not.Null);
            Assert.That(ex.Message, Does.Contain("TestKey"));
        }

        [Test]
        public void InvalidIntervalFormatException_ContainsIntervalString()
        {
            var ex = new InvalidIntervalFormatException("INVALID");
            Assert.That(ex.IntervalString, Is.EqualTo("INVALID"));
            Assert.That(ex.Message, Does.Contain("INVALID"));
        }

        private class AlwaysNullMatcher : ICellValidateModule
        {
            public bool? Validate(object p_CellFilter, object p_ValueDataForValidating) => null;
        }

        #endregion

        #region Thread Safety Tests

        [Test]
        public void ConcurrentFindOperations_AreThreadSafe()
        {
            var cf = CreateFactory();
            var items = new RuleItems(cf);
            for (int i = 0; i < 100; i++)
            {
                items.AddRuleItem($"pattern{i}", cf.CreateCell($"output{i}", CellInputOutputType.Output));
            }

            var exceptions = new List<Exception>();
            var tasks = new Task[20];

            for (int t = 0; t < tasks.Length; t++)
            {
                int taskId = t;
                tasks[t] = Task.Run(() =>
                {
                    try
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            var idx = (taskId * 50 + i) % 100;
                            var result = items.Find($"pattern{idx}");
                            if (result == null)
                                throw new Exception($"Expected to find pattern{idx}");
                        }
                    }
                    catch (Exception ex)
                    {
                        lock (exceptions) { exceptions.Add(ex); }
                    }
                });
            }

            Task.WaitAll(tasks);
            Assert.That(exceptions, Is.Empty, string.Join("; ", exceptions.Select(e => e.Message)));
        }

        [Test]
        public void ConcurrentFindAll_AreThreadSafe()
        {
            var cf = CreateFactory();
            var items = new RuleItems(cf);
            items.AddRuleItem(".*", cf.CreateCell("output1", CellInputOutputType.Output));
            items.AddRuleItem(".*", cf.CreateCell("output2", CellInputOutputType.Output));

            var tasks = Enumerable.Range(0, 10).Select(_ => Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    var results = items.FindAll("anything");
                    Assert.That(results.Count, Is.EqualTo(2));
                }
            })).ToArray();

            Task.WaitAll(tasks);
        }

        #endregion

        #region Interval Parser Tests

        [Test]
        [TestCase("INTERVAL(10;2000)", 10, false, 2000, false)]
        [TestCase("INTERVAL<10;2000)", 10, true, 2000, false)]
        [TestCase("INTERVAL<10;2000>", 10, true, 2000, true)]
        [TestCase("INTERVAL(10;2000>", 10, false, 2000, true)]
        [TestCase(" INTERVAL ( 10; 2 000 ) ", 10, false, 2000, false)]
        [TestCase(" IN TER VAL ( 10; 2 000 ) ", 10, false, 2000, false)]
        [TestCase("Interval<10;15)", 10, true, 15, false)]
        [TestCase("Interval<10.123;15,123)", 10.123, true, 15.123, false)]
        [TestCase("Interval<10,123456789;15.123456789)", 10.123456789, true, 15.123456789, false)]
        public void IntervalParser_ParsesCorrectFormats(string text, decimal from, bool fromIncl, decimal to, bool toIncl)
        {
            var result = CellValidateFilterDecimalInterval.CreateFromString(text);
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.From, Is.EqualTo(from));
            Assert.That(result.FromClosedIncluding, Is.EqualTo(fromIncl));
            Assert.That(result.To, Is.EqualTo(to));
            Assert.That(result.ToClosedIncluding, Is.EqualTo(toIncl));
        }

        [Test]
        [TestCase("INTERVALY(10;2000)")]
        [TestCase("INTERVAL(10;2000]")]
        [TestCase("INTERVAL(A;B)")]
        [TestCase("")]
        [TestCase("random text")]
        public void IntervalParser_InvalidFormat_ReturnsNull(string text)
        {
            Assert.That(CellValidateFilterDecimalInterval.CreateFromString(text), Is.Null);
        }

        #endregion

        #region Cache Tests

        [Test]
        public void CacheWrapperEmpty_AlwaysCallsCallback()
        {
            var cache = new CacheWrapperEmpty();
            int callCount = 0;
            var result1 = cache.GetItem("key1", () => { callCount++; return "value"; }, TimeSpan.FromMinutes(1));
            var result2 = cache.GetItem("key1", () => { callCount++; return "value"; }, TimeSpan.FromMinutes(1));
            Assert.That(callCount, Is.EqualTo(2));
            Assert.That(result1, Is.EqualTo("value"));
        }

        [Test]
        public void CacheWrapperMemory_CachesItems()
        {
            var cache = new CacheWrapperMemory();
            int callCount = 0;
            var result1 = cache.GetItem("test_cache_key", () => { callCount++; return "cached_value"; }, TimeSpan.FromMinutes(1));
            var result2 = cache.GetItem("test_cache_key", () => { callCount++; return "new_value"; }, TimeSpan.FromMinutes(1));
            Assert.That(callCount, Is.EqualTo(1));
            Assert.That(result1, Is.EqualTo("cached_value"));
            Assert.That(result2, Is.EqualTo("cached_value"));
            cache.ClearAll();
        }

        [Test]
        public void CacheWrapperMemory_ClearAll_RemovesCachedItems()
        {
            var cache = new CacheWrapperMemory();
            cache.GetItem("clear_test_key", () => "value1", TimeSpan.FromMinutes(1));
            cache.ClearAll();
            int callCount = 0;
            cache.GetItem("clear_test_key", () => { callCount++; return "value2"; }, TimeSpan.FromMinutes(1));
            Assert.That(callCount, Is.EqualTo(1));
            cache.ClearAll();
        }

        #endregion

        #region Repository Contract Tests

        [Test]
        public void RuleNotFoundException_IsRuleEvaluatorException()
        {
            var ex = new RuleNotFoundException("TestKey", new object[] { "param1" });
            Assert.That(ex, Is.InstanceOf<RuleEvaluatorException>());
        }

        #endregion

        #region Compatibility Layer Tests

        [Test]
        public void LegacyApi_RuleItems_FindAndFindAll_StillWork()
        {
            var cf = CreateFactory();
            RuleItems items = new RuleItems(cf);
            items.AddRuleItem(".*", ".*", cf.CreateCell("Output1", CellInputOutputType.Output));
            items.AddRuleItem(".*", ".*", cf.CreateCell("Output2", CellInputOutputType.Output));

            var found = items.Find("a", "b");
            Assert.That(found, Is.Not.Null);
            Assert.That(found!.Output(0).FilterValue, Is.EqualTo("Output1"));

            var all = items.FindAll("a", "b");
            Assert.That(all.Count, Is.EqualTo(2));
        }

        [Test]
        public void LegacyApi_RuleItemsWithCallHook_StillWorks()
        {
            var cf = CreateFactory();
            var callHook = new TestRuleItemsCall();
            RuleItems items = new RuleItems(cf, callHook, "TestRuleSet");
            items.AddRuleItem(".*", cf.CreateCell("Output", CellInputOutputType.Output));

            items.Find("anything");

            Assert.That(callHook.LastCalled, Is.Not.Null);
            Assert.That(callHook.LastCalled!.Method, Is.EqualTo(RuleItemsCallFindCalledMethod.Find));
            Assert.That(callHook.LastCalled.Name, Is.EqualTo("TestRuleSet"));
        }

        [Test]
        public void LegacyApi_GetAll_StillWorks()
        {
            var cf = CreateFactory();
            RuleItems items = new RuleItems(cf);
            items.AddRuleItem("a", cf.CreateCell("Out1", CellInputOutputType.Output));
            items.AddRuleItem("b", cf.CreateCell("Out2", CellInputOutputType.Output));

            var all = items.GetAll();
            Assert.That(all.Count, Is.EqualTo(2));
        }

        private class TestRuleItemsCall : IRuleItemsCall
        {
            public RuleItemsCallFindCalled? LastCalled { get; private set; }
            public void FindCalled(RuleItemsCallFindCalled p_CalledInfo)
            {
                LastCalled = p_CalledInfo;
            }
        }

        #endregion
    }
}
