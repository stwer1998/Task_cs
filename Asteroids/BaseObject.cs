using System.Drawing;

namespace Asteroids
{
    /// <summary>
    /// Базовый объект
    /// </summary>
    class BaseObject: IGameObject, ICollision
    {
        public Point Pos;
        public Point Dir;
        public Size Size;
        public Rectangle Rect { get; set; }

        protected BaseObject()
        {
                
        }        

        protected BaseObject(Point _pos, Point _dir, Size _size)
        {
            this.Pos = _pos;
            this.Dir = _dir;
            this.Size = _size;
        }
        /// <summary>
        /// Выводит объект на экран
        /// </summary>
        public virtual void Draw() { }
        /// <summary>
        /// Выполняет просчёт изменений в оъектах
        /// </summary>
        public virtual void Update() { }
        /// <summary>
        /// Проверяет объекты на пересечение
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Collision(ICollision obj) => obj.Rect.IntersectsWith(this.Rect);
        /// <summary>
        /// Возвращает свою копию
        /// </summary>
        /// <returns></returns>
        public BaseObject Clone()
        {
            return new BaseObject()
            {
                Pos = this.Pos,
                Dir = this.Dir,
                Size = this.Size,
            };
        }
    }
}