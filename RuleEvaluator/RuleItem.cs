using System;
using System.Collections.Generic;
using Castle.Windsor;

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

        public RuleItem(IWindsorContainer p_Container, params object[] p_Cells)
        {
            Cells = new List<ICell>(p_Cells.Length);
            foreach (var cell in p_Cells)
            {
                Cells.Add(cell is ICell ? cell as ICell : new CellFactory(p_Container).CreateCell(cell));
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

        public ICell Output(int p_OutputIndex)
        {
            return CellsOnlyOutput[p_OutputIndex];
        }
    }
}