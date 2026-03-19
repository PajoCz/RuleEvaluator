using System;
using System.Collections.Generic;

namespace RuleEvaluator
{
    /// <summary>
    /// Provides detailed diagnostics about a rule evaluation operation.
    /// </summary>
    public sealed class EvaluationDiagnostics
    {
        /// <summary>Name of the ruleset being evaluated.</summary>
        public string? RuleSetName { get; init; }

        /// <summary>Input parameters used for evaluation.</summary>
        public IReadOnlyList<object> Inputs { get; init; } = Array.Empty<object>();

        /// <summary>Total number of rules evaluated.</summary>
        public int RulesEvaluated { get; init; }

        /// <summary>Number of rules that matched.</summary>
        public int MatchCount { get; init; }

        /// <summary>Whether a match was found.</summary>
        public bool HasMatch => MatchCount > 0;

        /// <summary>The reason no match was found, if applicable.</summary>
        public string? NoMatchReason { get; init; }

        /// <summary>Time taken for evaluation.</summary>
        public TimeSpan Elapsed { get; init; }
    }

    /// <summary>
    /// Result of a Find operation, containing the matched rule (if any) and diagnostics.
    /// </summary>
    public sealed class FindResult
    {
        /// <summary>The matched rule item, or null if no match was found.</summary>
        public RuleItem? Match { get; }

        /// <summary>Whether a match was found.</summary>
        public bool HasMatch => Match != null;

        /// <summary>Diagnostics for this evaluation.</summary>
        public EvaluationDiagnostics? Diagnostics { get; }

        public FindResult(RuleItem? match, EvaluationDiagnostics? diagnostics = null)
        {
            Match = match;
            Diagnostics = diagnostics;
        }
    }

    /// <summary>
    /// Result of a FindAll operation, containing all matched rules and diagnostics.
    /// </summary>
    public sealed class FindAllResult
    {
        /// <summary>All matched rule items.</summary>
        public IReadOnlyList<RuleItem> Matches { get; }

        /// <summary>Whether any match was found.</summary>
        public bool HasMatch => Matches.Count > 0;

        /// <summary>Diagnostics for this evaluation.</summary>
        public EvaluationDiagnostics? Diagnostics { get; }

        public FindAllResult(IReadOnlyList<RuleItem> matches, EvaluationDiagnostics? diagnostics = null)
        {
            Matches = matches;
            Diagnostics = diagnostics;
        }
    }
}
