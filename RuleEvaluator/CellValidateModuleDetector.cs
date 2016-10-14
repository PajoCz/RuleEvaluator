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

            var cellValidateFilterAttribute = p_Data.GetType().GetCustomAttribute<CellValidateFilterAttribute>();
            if (cellValidateFilterAttribute != null)
            {   //detected by filter attribute [CellValidateFilter(ValidateModule=...)]
                return cellValidateFilterAttribute.GetValidateModuleInstance();
            }
            //default
            return new CellValidateModuleRegex();
        }
    }
}
