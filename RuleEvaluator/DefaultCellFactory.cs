using System;

namespace RuleEvaluator
{
    /// <summary>
    /// Default factory for creating Cell instances without requiring a DI container.
    /// Uses the built-in matcher pipeline: decimal interval → regex (chain of responsibility).
    /// </summary>
    public sealed class DefaultCellFactory : ICellFactory
    {
        private readonly ICellValidateModule _validateModule;

        /// <summary>
        /// Creates a factory with the default matcher pipeline (decimal interval → regex).
        /// </summary>
        public DefaultCellFactory()
        {
            var regexModule = new CellValidateModuleRegex();
            _validateModule = new CellValidateModuleDecimalInterval(regexModule);
        }

        /// <summary>
        /// Creates a factory with a custom matcher pipeline.
        /// </summary>
        public DefaultCellFactory(ICellValidateModule validateModule)
        {
            _validateModule = validateModule ?? throw new ArgumentNullException(nameof(validateModule));
        }

        public ICell CreateCell(object p_FilterValue, CellInputOutputType p_CellInputOutputTypeType = CellInputOutputType.Input)
        {
            return new Cell(_validateModule, p_FilterValue, p_CellInputOutputTypeType);
        }
    }
}
