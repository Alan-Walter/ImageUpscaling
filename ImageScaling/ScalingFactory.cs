using ImageScaling.Helpers;

using System;
using System.Collections.Generic;

namespace ImageScaling
{
    /// <summary>
    /// Generic фабрика получения объектов алгоритмов масштабирования
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ScalingFactory<T> : IScalingFactory<T> where T : IScaleImage
    {
        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="type">Тип класса</param>
        /// <returns>Объект класса</returns>
        public T GetScaleObject(Type type)
        {
            if (!typeof(T).IsAssignableFrom(type))
            {
                throw new ArgumentException("Invalid type");
            }

            var instance = Activator.CreateInstance(type);
            return (T)instance;
        }

        /// <summary>
        /// Получение объектов алгоритмов масштабирования
        /// </summary>
        /// <returns>Объекты алгоритмов масштабирования</returns>
        public ICollection<T> GetScaleObjects()
        {
            return ReflectionHelper.GetInstance<T>();
        }

        /// <summary>
        /// Получить типы имплементации для типа
        /// </summary>
        /// <returns>Коллекция типов</returns>
        public ICollection<Type> GetImplTypes()
        {
            return ReflectionHelper.GetImplimentationTypes(typeof(T));
        }
    }
}