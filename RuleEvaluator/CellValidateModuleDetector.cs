using System;
using System.Reflection;

namespace RuleEvaluator
{
    public class CellValidateModuleDetector
    {
        public ICellValidateModule DetectByFilterData(object p_Data)
        {
            //todo: null p_Data
            if (p_Data == null) throw new ArgumentNullException(nameof(p_Data));

            //todo: ChainOfResponsibility design pattern to choose right CellValidateModule. This pattern return right type a instancing (lifestyle) is work for another class

            var cellValidateFilterAttribute = p_Data.GetType().GetCustomAttribute<CellValidateFilterAttribute>();
            if (cellValidateFilterAttribute != null)
            {   //detected by filter attribute [CellValidateFilter(ValidateModule=...)]
                return cellValidateFilterAttribute.GetValidateModuleInstance();
            }

            var filterDecimal = CellValidateFilterDecimalInterval.CreateFromString(p_Data.ToString());
            if (filterDecimal != null)
            {
                return new CellValidateModuleDecimalInterval();
            }

            //default
            return new CellValidateModuleRegex();
        }
    }
}
