using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Asteroids
{
    /// <summary>
    /// Анимированный задний фон (летящая звёздная пыль)
    /// </summary>
    class BackgroundAnimation : IGameObject
    {
        static List<StarDust> Dust;
        const int maxCount = 100;
        Random rnd = new Random();

        public BackgroundAnimation()
        {
            Dust = new List<StarDust>(maxCount);
            Dust.Add(new StarDust());
        }

        public void Draw()
        {
            if (Dust.Count > 0)
                foreach (var star in Dust)
                {
                    star.Draw();
                }
        }

        public void Update()
        {
            if (Dust.Count() < maxCount && rnd.Next(100) % 5 == 0)
            {
                 Dust.Add(new StarDust());
            }

            if (Dust.Count > 0)
                for (int i = 0; i < Dust.Count; i++)
                {
                    Dust[i].Update();
                }
        }
        /// <summary>
        /// Класс описывающий объект для анимированного заднего фона
        /// </summary>
        class StarDust : BaseObject, IGameObject
        {
            public int Speed;
            public int size;
            const int maxValue = 5;
            public int imageIndex;
            
            Random rnd = new Random();

            public StarDust()
            {
                Pos.X = Game.w;
                Pos.Y = rnd.Next(0, Game.h);

                int randomValue = (rnd.Next(100) % maxValue) + 1;
                size = randomValue;
                Speed = randomValue;
                imageIndex = randomValue % 4;
            }

            public override void Draw()
            {
                Game.buf.Graphics.DrawImage(Game.di[this.imageIndex], this.Pos.X, this.Pos.Y, this.size, this.size);
            }

            public override void Update()
            {
                this.Pos.X = this.Pos.X - this.Speed;
                if (this.Pos.X < 0)
                    Dust.Remove(this);
            }

        }
    }
}
