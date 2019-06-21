using Asteroids;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MyGame
{
    /// <summary>
    /// Класс показывающий картинку взрыва
    /// </summary>
    class CollapsePicture : BaseObject
    {
        public int frame;
        private readonly int imageIndex;
        private Random r = new Random();

        public CollapsePicture(Point _pos, Point _dir, Size _size)
             : base(_pos, _dir, _size)
        {
            this.Size.Width = _size.Height * 3;
            this.Size.Height = _size.Height * 3;
            this.Pos.X = _pos.X + _size.Width / 2 - this.Size.Width / 2;
            this.Pos.Y = _pos.Y + _size.Height / 2 - this.Size.Height / 2;
            this.frame = 0;
            this.imageIndex = r.Next(100) % Game.ci.Length;
        }

        public override void Draw()
        {
            Game.buf.Graphics.DrawImage
                (
                Game.ci[imageIndex], 
                this.Pos.X, 
                this.Pos.Y, 
                this.Size.Width, 
                this.Size.Height
                );
        }

        public override void Update()
        {
            this.frame++;
            this.Pos.X += this.Dir.X;
        }
    }
    /// <summary>
    /// Взрывы
    /// </summary>
    class Collapse : IGameObject
    {
        List<CollapsePicture> collapseSprites = new List<CollapsePicture>();

        public Collapse()
        {
            Asteroid.BulletCollision += Boom;
            StarShip.BulletCollision += Boom;
            Asteroid.ShipCollision += (s, aim) =>
            {
                if (!aim)
                {
                    this.collapseSprites
                    .Add(new CollapsePicture(new Point(Game.abc.Pos.X + Game.abc.Size.Width / 2, Game.abc.Pos.Y),
                        new Point(0, 0),
                        new Size(40, 40)));
                    Sounds.PlayBoomSound();
                }
                else
                    Sounds.PlayAidSound();
            };

        }

        public void Boom (object s, Bullet _bullet)
        {
            if (s is EnemyShip)
                Game.schet++;
            BaseObject obj = (BaseObject)s;
            this.collapseSprites
            .Add(new CollapsePicture(new Point(_bullet.Pos.X, obj.Pos.Y), new Point(0,0), new Size (obj.Size.Width, obj.Size.Width)));
            Sounds.PlayBoomSound();
        }

        public void Draw()
        {
            for (int i = 0; i < collapseSprites.Count(); i++)
            {
                this.collapseSprites[i].Draw();
                if (this.collapseSprites[i].frame > 20)
                    this.collapseSprites.Remove(this.collapseSprites[i]);
            }
        }

        public void Update()
        {

            foreach (var item in this.collapseSprites)
            {
                item.Update();
            }
        }
    }
}
