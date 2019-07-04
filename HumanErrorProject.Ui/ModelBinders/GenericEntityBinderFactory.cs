using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace HumanErrorProject.Ui.ModelBinders
{
    public abstract class GenericEntityBinderFactory<TEntity>
    {
        public abstract TEntity Create(string model, IValueProvider valueProvider);

        public T First<T>(string name, IValueProvider valueProvider)
        {
            var result = valueProvider.GetValue(name);
            if (result == ValueProviderResult.None)
                throw new ArgumentNullException($"Couldn't find {name} to bind too.");
            var value = result.FirstValue;
            if (typeof(T) == typeof(string))
                return (T)(object)(value);
            return JsonConvert.DeserializeObject<T>(value);
        }

        public T FirstOrDefault<T>(string name, IValueProvider valueProvider)
        {
            var result = valueProvider.GetValue(name);
            if (result == ValueProviderResult.None)
                return default(T);
            var value = result.FirstValue;
            if (typeof(T) == typeof(string))
                return (T)(object)(value);
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
