using System.Drawing;

namespace Asteroids
{
    /// <summary>
    /// Интерфейс объектов поддерживающих проверку на столкновения
    /// </summary>
    interface ICollision
    {
        /// <summary>
        /// Метод проверяющий на столкновения
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool Collision(ICollision obj);
        /// <summary>
        /// Прямоугольник включающий в себя весь объект
        /// </summary>
        Rectangle Rect { get; set; }
    }
}
