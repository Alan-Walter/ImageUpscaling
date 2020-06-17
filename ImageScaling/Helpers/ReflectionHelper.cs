using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ImageScaling.Helpers
{
    /// <summary>
    /// Хелпер рефлексии
    /// </summary>
    internal static class ReflectionHelper
    {
        /// <summary>
        /// Получение коллекции объектов определенного типа
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ICollection<T> GetInstance<T>()
        {
            var type = typeof(T);
            var types = GetImplimentationTypes(type);
            return types.Select(i => (T)Activator.CreateInstance(i)).ToList();
        }

        /// <summary>
        /// Получение коллекции типов имплементации без абстрактных классов и интерфейсов
        /// </summary>
        /// <param name="type">Тип, чьих наследников нужно найти</param>
        /// <returns>Коллекция с типами наследников</returns>
        public static ICollection<Type> GetImplimentationTypes(Type type)
        {
            return Assembly.GetAssembly(type).GetTypes()
                .Where(i => type.IsAssignableFrom(i) && i != type && !i.IsAbstract && !i.IsInterface).ToList();
        }

        /// <summary>
        /// Установить приватному полю указанное значение val через рефлексию
        /// https://stackoverflow.com/questions/1565734/is-it-possible-to-set-private-property-via-reflection
        /// </summary>
        /// <param name="obj">Объект, поле которого необходимо изменить</param>
        /// <param name="fieldName">Имя поля в формате строки</param>
        /// <param name="val">Значение, которое необходимо установить</param>
        public static void SetPrivateFieldValue(this object obj, string fieldName, object val)
        {
            ExceptionHelper.ThrowIfNull(obj, "obj");
            ExceptionHelper.ThrowIfNull(val, "val");

            Type type = obj.GetType();
            FieldInfo fieldInfo = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            ExceptionHelper.ThrowIfNull(fieldInfo, $"field {fieldName} not found!");
            fieldInfo.SetValue(obj, val);
        }
    }
}