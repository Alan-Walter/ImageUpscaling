using System;
using System.Collections.Generic;

namespace ImageScaling
{
    /// <summary>
    /// Интерфейс фабрики масштабирования изображений
    /// </summary>
    public interface IScalingFactory<T> where T : IScaleImage
    {
        /// <summary>
        /// Получение объектов алгоритмов масштабирования
        /// </summary>
        /// <returns>Список объектов</returns>
        ICollection<T> GetScaleObjects();

        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="t">Тип класса</param>
        /// <returns>Объект класса</returns>
        T GetScaleObject(Type t);

        /// <summary>
        /// Получить типы имплементации для типа
        /// </summary>
        /// <returns>Коллекция типов</returns>
        ICollection<Type> GetImplTypes();
    }
}