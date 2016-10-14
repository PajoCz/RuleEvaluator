using System;
using System.Collections.Generic;

namespace RuleEvaluator
{
    public class RuleItem
    {
        public readonly List<Cell> Cells;

        private List<Cell> _CellsOnlyInputCached;

        private List<Cell> _CellsOnlyOutputCached;

        public RuleItem(params object[] p_Cells)
        {
            Cells = new List<Cell>(p_Cells.Length);
            CellValidateModuleDetector detector = new CellValidateModuleDetector();
            foreach (var cell in p_Cells)
            {
                Cells.Add(cell is Cell ? cell as Cell : new Cell(cell, detector.DetectByFilterData(cell)));
            }
        }

        public List<Cell> CellsOnlyInput
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

        public List<Cell> CellsOnlyOutput
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

        public bool ValidateInput(params object[] p_Data)
        {
            if (p_Data == null) throw new ArgumentNullException(nameof(p_Data));
            if (p_Data.Length != CellsOnlyInput.Count) throw new ArgumentOutOfRangeException(nameof(p_Data));

            for (var i = 0; i < CellsOnlyInput.Count; i++)
            {
                if (!CellsOnlyInput[i].Validate(p_Data[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public Cell Output(int p_OutputIndex)
        {
            return CellsOnlyOutput[p_OutputIndex];
        }
    }
}