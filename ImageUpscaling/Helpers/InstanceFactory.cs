using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ImageUpscaling.Helpers
{
    public static class InstanceFactory
    {
        public static ICollection<T> GetInstance<T>()
        {
            var type = typeof(T);
            var types = GetImplimentationTypes(type);
            var str = type.Name.Remove(0, 4);
            return types.Select(i => (T)Activator.CreateInstance(i)).ToList();
        }

        private static ICollection<Type> GetImplimentationTypes(Type type)
        {
            return Assembly.GetAssembly(type).GetTypes()
                .Where(i => type.IsAssignableFrom(i) && i != type).ToList();
        }
    }
}
