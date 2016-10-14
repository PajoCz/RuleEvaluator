using System;

namespace RuleEvaluator
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CellValidateFilterAttribute : Attribute
    {
        public Type ValidateModule { get; set; }
    }
}
