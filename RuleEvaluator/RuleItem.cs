using System;
using System.Collections.Generic;

namespace RuleEvaluator
{
    /// <summary>
    /// One rule item composed of a list of cells, with ValidateInput method.
    /// </summary>
    public class RuleItem
    {
        public readonly List<ICell> Cells;

        private List<ICell>? _cellsOnlyInputCached;
        private List<ICell>? _cellsOnlyOutputCached;

        public RuleItem(ICellFactory p_Factory, params object[] p_Cells)
        {
            Cells = new List<ICell>(p_Cells.Length);
            foreach (var cell in p_Cells)
            {
                Cells.Add(cell is ICell c ? c : p_Factory.CreateCell(cell));
            }
        }

        public List<ICell> CellsOnlyInput
        {
            get
            {
                _cellsOnlyInputCached ??= Cells.FindAll(c => c.InputOutputType == CellInputOutputType.Input);
                return _cellsOnlyInputCached;
            }
        }

        public List<ICell> CellsOnlyOutput
        {
            get
            {
                _cellsOnlyOutputCached ??= Cells.FindAll(c => c.InputOutputType == CellInputOutputType.Output);
                return _cellsOnlyOutputCached;
            }
        }

        public bool ValidateInput(string? p_RuleItemsName, params object[] p_Data)
        {
            if (p_Data?.Length != CellsOnlyInput.Count)
                throw new InputParameterCountMismatchException(p_RuleItemsName, CellsOnlyInput.Count, p_Data?.Length ?? 0);

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

        public ICell? PrimaryKey => Cells.Find(c => c.InputOutputType == CellInputOutputType.PrimaryKey);
    }
}