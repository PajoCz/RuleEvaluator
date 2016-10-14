using System;

namespace RuleEvaluator
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CellValidateFilterAttribute : Attribute
    {
        public Type ValidateModule { get; set; }

        public ICellValidateModule GetValidateModuleInstance()
        {
            var validationModuleInstance = Activator.CreateInstance(ValidateModule);
            if (!(validationModuleInstance is ICellValidateModule)) throw new Exception($"ValidateModule of type {ValidateModule} does not implement interface {typeof(ICellValidateModule)}");
            return (ICellValidateModule)validationModuleInstance;
        }
    }
}
