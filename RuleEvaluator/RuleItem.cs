using System;
using System.Collections.Generic;

namespace RuleEvaluator
{
    /// <summary>
    /// One rule item with composition of List<ICell> with ValidateInput method
    /// </summary>
    public class RuleItem
    {
        public readonly List<ICell> Cells;

        private List<ICell> _CellsOnlyInputCached;

        private List<ICell> _CellsOnlyOutputCached;

        public RuleItem(ICellFactory p_Factory, params object[] p_Cells)
        {
            Cells = new List<ICell>(p_Cells.Length);
            foreach (var cell in p_Cells)
            {
                Cells.Add(cell is ICell ? cell as ICell : p_Factory.CreateCell(cell));
            }
        }

        public List<ICell> CellsOnlyInput
        {
            get
            {
                if (_CellsOnlyInputCached == null)
                {
                    _CellsOnlyInputCached = Cells.FindAll(c => c.InputOutputType == CellInputOutputType.Input);
                }
                return _CellsOnlyInputCached;
            }
        }

        public List<ICell> CellsOnlyOutput
        {
            get
            {
                if (_CellsOnlyOutputCached == null)
                {
                    _CellsOnlyOutputCached = Cells.FindAll(c => c.InputOutputType == CellInputOutputType.Output);
                }
                return _CellsOnlyOutputCached;
            }
        }

        public bool ValidateInput(string p_RuleItemsName, params object[] p_Data)
        {
            if (p_Data?.Length != CellsOnlyInput.Count)
                throw new ArgumentOutOfRangeException(
                    string.IsNullOrEmpty(p_RuleItemsName)
                        ? $"Input data with {p_Data?.Length} parameters but expected is {CellsOnlyInput.Count} input parameters"
                        : $"RuleEvaluator '{p_RuleItemsName}' finding with {p_Data?.Length} parameters as input data but expected is {CellsOnlyInput.Count} input parameters"
                    , nameof(p_Data));

            for (var i = 0; i < CellsOnlyInput.Count; i++)
            {
                if (!CellsOnlyInput[i].Validate(p_Data[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public ICell Output(int p_OutputIndex)
        {
            return CellsOnlyOutput[p_OutputIndex];
        }

        public ICell PrimaryKey => Cells.Find(c => c.InputOutputType == CellInputOutputType.PrimaryKey);
    }
}