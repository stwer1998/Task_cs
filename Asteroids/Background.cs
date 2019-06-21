using System;
using System.Drawing;

namespace Asteroids
{
    /// <summary>
    /// Класс описывает объект статического заднего фона
    /// </summary>
    class Star : BaseObject, IGameObject
    {
        public Star(Point _pos, Point _dir, Size _size)
             : base(_pos, _dir, _size)
        {
        }

        public override void Draw()
        {
            Game.buf.Graphics.DrawEllipse(Pens.DimGray, this.Pos.X, this.Pos.Y,
                     this.Size.Width, this.Size.Height);
        }
    }
    /// <summary>
    /// Статический задний фон
    /// </summary>
    class Background : IGameObject
    {
        private readonly Star[] backgroundStars = new Star[200];
        
        public Background()
        {
            Random rnd = new Random();
            for (int i = 0; i < backgroundStars.Length; i++)
            {
                int x = rnd.Next(0, Game.w);
                int y = rnd.Next(0, Game.h);
                this.backgroundStars[i] = new Star(new Point(x,y), new Point(0, 0), new Size(1,1));
            }
        }

        public void Draw()
        {
            foreach (var star in this.backgroundStars)
            {
                star.Draw();
            }
        }
        public void Update()
        {
        }
    }
}
