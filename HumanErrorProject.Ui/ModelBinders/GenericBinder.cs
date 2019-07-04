using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HumanErrorProject.Ui.ModelBinders
{
    public class GenericBinder<T> : IModelBinder
    {
        protected GenericEntityBinderFactory<T> Factory;
        public GenericBinder(GenericEntityBinderFactory<T> factory)
        {
            Factory = factory;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));
            var modelName = bindingContext.ModelName;
            var content = Factory.Create(modelName, bindingContext.ValueProvider);
            bindingContext.Result = ModelBindingResult.Success(content);
            return Task.CompletedTask;
        }
    }
}
