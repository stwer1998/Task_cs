using System.Drawing;

namespace Asteroids
{
    /// <summary>
    /// Снаряд
    /// </summary>
    class Bullet : BaseObject, IGameObject, ICollision
    {
        private int bulletTick = 0;
        private const int bulletTickMaxValue = 7;
        private readonly int imageIndex;

        public Bullet(Point _pos, Point _dir, Size _size, int _imageIndex = 0)
            : base(_pos, _dir, _size)
        {
            imageIndex = _imageIndex;
            Asteroid.BulletCollision += (sender, _bullet) =>
            {
                if (this == _bullet)
                {
                    _bullet.Pos.X = Game.w;
                }
            };

            StarShip.BulletCollision += (sender, _bullet) =>
            {
                if (this == _bullet)
                {
                    _bullet.Pos.X = -this.Size.Width;
                }
            };
        }

        public override void Draw()
        {
            if (this.Pos.X < Game.w && this.Pos.X > 0 - this.Size.Width)
                Game.buf.Graphics.DrawImage(Game.bi[imageIndex], this.Pos.X, this.Pos.Y, this.Size.Width, this.Size.Height);
        }

        public override void Update()
        {
            if (this.Pos.X < Game.w)
            {
                this.Pos.X = this.Pos.X + this.Dir.X;
                this.Rect = new Rectangle(this.Pos, this.Size);
                if (++this.bulletTick > bulletTickMaxValue)
                {
                    this.Dir.X += this.Dir.X > 0 ? 1 : -1;
                    bulletTick = 0;
                }
            }
        }
    }
}
