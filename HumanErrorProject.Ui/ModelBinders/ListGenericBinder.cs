using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HumanErrorProject.Ui.ModelBinders
{
    public class ListGenericBinder<T> : IModelBinder
    {
        protected GenericEntityBinderFactory<T> Factory;

        public ListGenericBinder(GenericEntityBinderFactory<T> factory)
        {
            Factory = factory;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));
            var modelName = bindingContext.ModelName;
            var index = 0;

            var contents = new List<T>();

            while (bindingContext.ValueProvider.GetValue($"{modelName}[{index}].Name") != ValueProviderResult.None)
            {
                var content = Factory.Create($"{modelName}[{index}]", bindingContext.ValueProvider);
                contents.Add(content);
                ++index;
            }
            bindingContext.Result = ModelBindingResult.Success(
                contents);
            return Task.CompletedTask;
        }
    }
}
