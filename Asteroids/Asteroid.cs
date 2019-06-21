using System;
using System.Drawing;

namespace Asteroids
{
    /// <summary>
    /// Класс астероид
    /// </summary>
    internal class Asteroid : BaseObject, IGameObject, ICollision
    {
        public delegate void ShipHitHandler(object sender, bool aid);
        public delegate void BulletHitHandler(object sender, Bullet _bullet);
        /// <summary>
        /// Событие столкновения коробля с другим объектом
        /// </summary>
        public static event ShipHitHandler ShipCollision;
        /// <summary>
        /// Событие столкновения снаряда с другим объектом
        /// </summary>
        public static event BulletHitHandler BulletCollision;
        /// <summary>
        /// является ли объект аптечкой
        /// </summary>
        public bool aid = false;
        /// <summary>
        /// Индекс изображения в массиве изображений
        /// </summary>
        protected int imageIndex;

        public Asteroid(Point _pos, Point _dir, Size _size, int _imageIndex = 0, bool _aid = false)
            : base(_pos, _dir, _size)
        {
            this.aid = _aid;
            this.imageIndex = _imageIndex;
        }

        public override void Draw()
        {
            Game.buf.Graphics.DrawImage(Game.ai[this.imageIndex], this.Pos.X, this.Pos.Y, this.Size.Width, this.Size.Height);
        }

        public override void Update()
        {
            this.Pos.X = this.Pos.X + this.Dir.X;
            CollisionTest();
        }
        /// <summary>
        /// Проверка на столкновение со снарядом или кораблём игрока
        /// </summary>
        public void CollisionTest()
        {
            foreach (var item in Game.b)
            {
                if (this.Collision(item))
                {
                    BulletCollision?.Invoke(this, item);
                }
            }
            if (this.Collision(Game.abc))
                ShipCollision?.Invoke(this, aid);
        }
    }
}
