using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Asteroids
{
    class EnemyShip : Asteroid
    {
        Random rnd = new Random();

        public EnemyShip(Point _pos, Point _speed, Size _size, int _imageIndex)
             : base(_pos, _speed, _size, _imageIndex)
        {

        }

        public override void Draw()
        {
            Game.buf.Graphics.DrawImage(Game.esi[this.imageIndex], this.Pos.X, this.Pos.Y, this.Size.Width, this.Size.Height);
        }

        public override void Update()
        {
            this.Pos.X = this.Pos.X + this.Dir.X;
            CollisionTest();
            if (rnd.Next(1000) % 250 == 0)
                Fire();
        }
        private void Fire()
        {
            Game.b.Add(new Bullet(new Point(this.Pos.X - 35 , this.Pos.Y + this.Size.Height/2), new Point(this.Dir.X, 0), new Size(32, 8), 1));
            Sounds.PlayEnemyShootSound();
        }
    }
}
