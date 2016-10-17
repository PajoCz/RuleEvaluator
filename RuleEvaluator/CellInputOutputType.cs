namespace RuleEvaluator
{
    public enum CellInputOutputType
    {
        /// <summary>
        /// Cell is used for searching by Cell.Validate method
        /// </summary>
        Input,
        /// <summary>
        /// Cell is not used for searching. Contains output data in searched (validated) RuleItem
        /// </summary>
        Output
    }
}