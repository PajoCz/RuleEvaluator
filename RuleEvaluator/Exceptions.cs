using System;

namespace RuleEvaluator
{
    /// <summary>
    /// Base exception for RuleEvaluator errors.
    /// </summary>
    public class RuleEvaluatorException : Exception
    {
        public RuleEvaluatorException(string message) : base(message) { }
        public RuleEvaluatorException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Thrown when no matching rule is found.
    /// </summary>
    public class RuleNotFoundException : RuleEvaluatorException
    {
        public string? RuleSetKey { get; }
        public object[]? Parameters { get; }

        public RuleNotFoundException(string? ruleSetKey, object[]? parameters)
            : base($"No matching rule found in ruleset '{ruleSetKey}' for parameters [{string.Join(", ", parameters ?? Array.Empty<object>())}]")
        {
            RuleSetKey = ruleSetKey;
            Parameters = parameters;
        }
    }

    /// <summary>
    /// Thrown when a cell validation module returns null (unknown validation result).
    /// </summary>
    public class MatcherException : RuleEvaluatorException
    {
        public MatcherException(string message) : base(message) { }
    }

    /// <summary>
    /// Thrown when input parameter count doesn't match expected input cell count.
    /// </summary>
    public class InputParameterCountMismatchException : RuleEvaluatorException
    {
        public int ExpectedCount { get; }
        public int ActualCount { get; }

        public InputParameterCountMismatchException(string? ruleItemsName, int expectedCount, int actualCount)
            : base(string.IsNullOrEmpty(ruleItemsName)
                ? $"Input data with {actualCount} parameters but expected is {expectedCount} input parameters"
                : $"RuleEvaluator '{ruleItemsName}' finding with {actualCount} parameters as input data but expected is {expectedCount} input parameters")
        {
            ExpectedCount = expectedCount;
            ActualCount = actualCount;
        }
    }

    /// <summary>
    /// Thrown when interval string format is invalid.
    /// </summary>
    public class InvalidIntervalFormatException : RuleEvaluatorException
    {
        public string IntervalString { get; }

        public InvalidIntervalFormatException(string intervalString)
            : base($"Invalid interval format: '{intervalString}'")
        {
            IntervalString = intervalString;
        }
    }

    /// <summary>
    /// Thrown when a database or infrastructure error occurs during rule loading.
    /// </summary>
    public class RuleLoadException : RuleEvaluatorException
    {
        public RuleLoadException(string message) : base(message) { }
        public RuleLoadException(string message, Exception innerException) : base(message, innerException) { }
    }
}
