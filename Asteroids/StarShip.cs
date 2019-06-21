using System;
using System.Drawing;

namespace Asteroids
{
    /// <summary>
    /// Космический корабль 
    /// </summary>
    class StarShip : BaseObject, IGameObject
    {
        public int Speed;
        public int imageIndex;
        public int energy = 100;
        const int mouseSpeedCorrection = 1;

        public delegate void BulletHitHandler(object sender, Bullet _bullet);
        /// <summary>
        /// Событие столкновения снаряда с кораблём
        /// </summary>
        public static event BulletHitHandler BulletCollision;

        Random rnd = new Random();

        public StarShip(Point _pos, int _speed, Size _size)
             : base(_pos, new Point(_speed,0), _size)
        {
            this.Pos = _pos;

            int randomValue = rnd.Next(100)%4;
            this.imageIndex = 3;

            this.Size = _size;

            this.Speed = _speed;

            Rect = new Rectangle(Pos, Size);

            Asteroid.ShipCollision += Damage;
        }
        /// <summary>
        /// Обработчик столкновений коробля с другими объектами
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="aim"></param>
        private void Damage(object sender, bool aim)
        {
            if ((sender is Asteroid) && ((Asteroid)sender).aid)
            {
                energy = energy + 25 <= 200 ? energy + 25 : 200;
            }
            else
            {
                energy -= 25;
            }
            if (energy <= 0)
                Game.gofl = true;
        }

        public override void Draw()
        {
            Game.buf.Graphics.DrawImage(Game.ssi[this.imageIndex], this.Pos.X, this.Pos.Y, this.Size.Width, this.Size.Height);
        }

        public override void Update()
        {
            this.Rect = new Rectangle(this.Pos, this.Size);
            if (Pos.Y > Game.mY) MouseMoveUp((Pos.Y - Game.mY) > 20 ? 2 : 0);
            else
                if (Pos.Y < Game.mY) MouseMoveDown((Game.mY - Pos.Y) > 20 ? 2 : 0);
            if (Pos.X > Game.mX) MouseMoveLeft((Pos.X - Game.mX) > 20 ? 2 : 0);
            else
                if (Pos.X < Game.mX) MouseMoveRight((Game.mX - Pos.X) > 20 ? 2 : 0);
            CollisionTest();
        }

        private void CollisionTest()
        {
            foreach (var item in Game.b)
            {
                if (this.Collision(item))
                {
                    Damage(this, false);
                    BulletCollision?.Invoke(this, item);
                }
            }
        }

        /// <summary>
        /// Выбор(рандомно) нового спрайта для коробля
        /// </summary>
        public void ChangeShip()
        {
            int newShip = this.imageIndex;
            while (newShip == this.imageIndex)
            {
                newShip = rnd.Next(100)%4;
            }
            this.imageIndex = newShip;
        }
        /// <summary>
        /// Движение вверх
        /// </summary>
        public void MoveUp()
        {
            this.Pos.Y -= (this.Pos.Y - this.Speed) > 20 ? this.Speed : 0;
        }
        /// <summary>
        /// Движение вниз
        /// </summary>
        public void MoveDown()
        {
            this.Pos.Y += (this.Pos.Y + this.Speed + this.Size.Height) < Game.h - 50 ? this.Speed : 0;
        }
        /// <summary>
        /// Движение влево
        /// </summary>
        public void MoveLeft()
        {
            this.Pos.X -= (this.Pos.X - this.Speed) > 20 ? this.Speed : 0;
        }
        /// <summary>
        /// Движение вправо
        /// </summary>
        public void MoveRight()
        {
            this.Pos.X += (this.Pos.X + this.Speed + this.Size.Width) < Game.w - 50 ? this.Speed : 0;
        }
        /// <summary>
        /// Движение вверх
        /// </summary>
        /// <param name="incr"></param>
        public void MouseMoveUp(int incr = 0)
        {
            this.Pos.Y -= (this.Pos.Y - mouseSpeedCorrection) > 20 ? mouseSpeedCorrection + incr : 0;
        }
        /// <summary>
        /// Движение вниз
        /// </summary>
        /// <param name="incr"></param>
        public void MouseMoveDown(int incr = 0)
        {
            this.Pos.Y += (this.Pos.Y + mouseSpeedCorrection + this.Size.Height) < Game.h - 50 ? mouseSpeedCorrection + incr : 0;
        }
        /// <summary>
        /// Движение влево
        /// </summary>
        /// <param name="incr"></param>
        public void MouseMoveLeft(int incr = 0)
        {
            this.Pos.X -= (this.Pos.X - mouseSpeedCorrection) > 20 ? mouseSpeedCorrection + incr : 0;
        }
        /// <summary>
        /// Движение вправо
        /// </summary>
        /// <param name="incr"></param>
        public void MouseMoveRight(int incr = 0)
        {
            this.Pos.X += (this.Pos.X + mouseSpeedCorrection + this.Size.Width) < Game.w - 50 ? mouseSpeedCorrection + incr : 0;
        }
    }
}