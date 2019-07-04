using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace HumanErrorProject.Ui.ModelBinders
{
    public class GenericBinderProvider<T> : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (context.Metadata.ModelType == typeof(IList<T>))
            {
                return new BinderTypeModelBinder(typeof(ListGenericBinder<T>));
            }
            if (context.Metadata.ModelType == typeof(T))
            {
                return new BinderTypeModelBinder(typeof(GenericBinder<T>));
            }
            return null;
        }
    }
}
