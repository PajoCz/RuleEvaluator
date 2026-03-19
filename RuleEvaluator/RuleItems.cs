using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RuleEvaluator
{
    public class RuleItems
    {
        public readonly string? Name;
        private readonly ICellFactory _cellFactory;
        private readonly IRuleItemsCall? _ruleItemsCall;
        private readonly List<RuleItem> _data;
        private bool _enableDiagnostics;

        public RuleItems(ICellFactory p_CellFactory)
        {
            _cellFactory = p_CellFactory;
            _data = new List<RuleItem>();
        }

        public RuleItems(ICellFactory p_CellFactory, IRuleItemsCall? p_RuleItemsCall, string? p_Name)
        {
            _cellFactory = p_CellFactory;
            _ruleItemsCall = p_RuleItemsCall;
            Name = p_Name;
            _data = new List<RuleItem>();
        }

        /// <summary>
        /// Enable or disable diagnostics collection for Find/FindAll operations.
        /// </summary>
        public bool EnableDiagnostics
        {
            get => _enableDiagnostics;
            set => _enableDiagnostics = value;
        }

        /// <summary>
        /// Gets the number of rule items.
        /// </summary>
        public int Count => _data.Count;

        public void AddRuleItem(params object[] p_Cells)
        {
            _data.Add(new RuleItem(_cellFactory, p_Cells));
        }

        public List<RuleItem> FindAll(params object[] p_FindParams)
        {
            var result = _data.FindAll(i => i.ValidateInput(Name, p_FindParams));
            _ruleItemsCall?.FindCalled(new RuleItemsCallFindCalled()
            {
                Method = RuleItemsCallFindCalledMethod.FindAll,
                Name = Name,
                Inputs = p_FindParams?.ToList(),
                Outputs = result.Select(r => r.CellsOnlyOutput.Select(c => c.FilterValue).ToList()).ToList()
            });
            return result;
        }

        public RuleItem? Find(params object[] p_FindParams)
        {
            var result = _data.Find(i => i.ValidateInput(Name, p_FindParams));
            _ruleItemsCall?.FindCalled(new RuleItemsCallFindCalled()
            {
                Method = RuleItemsCallFindCalledMethod.Find,
                Name = Name,
                Inputs = p_FindParams?.ToList(),
                Outputs = result != null ? new List<List<object>>() { result.CellsOnlyOutput.Select(c => c.FilterValue).ToList() } : null
            });
            return result;
        }

        /// <summary>
        /// Modern API: Find first matching rule and return a result object with optional diagnostics.
        /// </summary>
        public FindResult FindWithDiagnostics(params object[] findParams)
        {
            var sw = _enableDiagnostics ? Stopwatch.StartNew() : null;
            int evaluated = 0;
            RuleItem? match = null;

            foreach (var item in _data)
            {
                evaluated++;
                if (item.ValidateInput(Name, findParams))
                {
                    match = item;
                    break;
                }
            }

            sw?.Stop();

            _ruleItemsCall?.FindCalled(new RuleItemsCallFindCalled()
            {
                Method = RuleItemsCallFindCalledMethod.Find,
                Name = Name,
                Inputs = findParams?.ToList(),
                Outputs = match != null ? new List<List<object>>() { match.CellsOnlyOutput.Select(c => c.FilterValue).ToList() } : null
            });

            var diagnostics = _enableDiagnostics ? new EvaluationDiagnostics
            {
                RuleSetName = Name,
                Inputs = findParams ?? Array.Empty<object>(),
                RulesEvaluated = evaluated,
                MatchCount = match != null ? 1 : 0,
                NoMatchReason = match == null ? "No rule matched the input parameters" : null,
                Elapsed = sw?.Elapsed ?? TimeSpan.Zero
            } : null;

            return new FindResult(match, diagnostics);
        }

        /// <summary>
        /// Modern API: Find all matching rules and return a result object with optional diagnostics.
        /// </summary>
        public FindAllResult FindAllWithDiagnostics(params object[] findParams)
        {
            var sw = _enableDiagnostics ? Stopwatch.StartNew() : null;
            var matches = _data.FindAll(i => i.ValidateInput(Name, findParams));
            sw?.Stop();

            _ruleItemsCall?.FindCalled(new RuleItemsCallFindCalled()
            {
                Method = RuleItemsCallFindCalledMethod.FindAll,
                Name = Name,
                Inputs = findParams?.ToList(),
                Outputs = matches.Select(r => r.CellsOnlyOutput.Select(c => c.FilterValue).ToList()).ToList()
            });

            var diagnostics = _enableDiagnostics ? new EvaluationDiagnostics
            {
                RuleSetName = Name,
                Inputs = findParams ?? Array.Empty<object>(),
                RulesEvaluated = _data.Count,
                MatchCount = matches.Count,
                NoMatchReason = matches.Count == 0 ? "No rule matched the input parameters" : null,
                Elapsed = sw?.Elapsed ?? TimeSpan.Zero
            } : null;

            return new FindAllResult(matches, diagnostics);
        }

        public List<RuleItem> GetAll()
        {
            _ruleItemsCall?.FindCalled(new RuleItemsCallFindCalled()
            {
                Method = RuleItemsCallFindCalledMethod.GetAll,
                Name = Name,
                Inputs = null,
                Outputs = _data.Select(r => r.CellsOnlyOutput.Select(c => c.FilterValue).ToList()).ToList()
            });
            return _data;
        }
    }
}