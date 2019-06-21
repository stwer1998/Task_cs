using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Asteroids
{
    /// <summary>
    /// Описывает поле астероидов
    /// </summary>
    class AsteroidsField : IGameObject
    {
        /// <summary>
        /// Список объектов поля астероидов
        /// </summary>
        static List<BaseObject> FieldOfAsteroids;
        /// <summary>
        /// Максимальное кол-во объектов на экране
        /// </summary>
        const int maxCount = 30;
        /// <summary>
        /// Минимальная скорость движения объектов
        /// </summary>
        const int minSpeed = -1;
        /// <summary>
        /// Направление движения
        /// </summary>
        private Point direction = new Point(minSpeed, 0);

        Random rnd = new Random();

        public AsteroidsField()
        {
            FieldOfAsteroids = new List<BaseObject>(maxCount);
            FieldOfAsteroids.Add(CreateNewFieldObject());

            Asteroid.ShipCollision += (sender, e) => FieldOfAsteroids.Remove((BaseObject)sender);
            Asteroid.BulletCollision += (sender, e) => ((BaseObject)sender).Pos.X = -100;
        }
        /// <summary>
        /// Создаёт новый объект
        /// </summary>
        /// <returns></returns>
        public Asteroid CreateNewFieldObject()
        {
            int randomPosition = rnd.Next(50, Game.h - 100);
            int randomSize = rnd.Next(15, 100);
            this.direction.X = minSpeed - rnd.Next(100) % 3;
            int randomObject = rnd.Next(100);
            if (randomObject % 51 == 0) 
                //Создаётся аптечка
            {
                return new Asteroid(new Point(Game.w, randomPosition),
                this.direction, new Size(33, 23),
                Game.ai.Length - 1, true);
            }
            else if (randomObject % 10 == 0)
            {
                //Создаётся вражеский корабль
                return new EnemyShip(new Point(Game.w, randomPosition),
                new Point (this.direction.X - 2, 0), new Size(110, 35),
                rnd.Next(100) % (Game.esi.Length - 1));
            }
            else
                //Создаётся астероид
                return new Asteroid(new Point(Game.w, randomPosition),
                this.direction, new Size(randomSize, randomSize),
                rnd.Next(100) % (Game.ai.Length - 1), false);
        }

        public void Draw()
        {
            if (FieldOfAsteroids.Count > 0)
                foreach (var _asteroid in FieldOfAsteroids)
                {
                    _asteroid.Draw();
                }
        }

        public void Update()
        {
            if (FieldOfAsteroids.Count() < maxCount && rnd.Next(100) % 15 == 0)
            {
                FieldOfAsteroids.Add(CreateNewFieldObject());
            }

            if (FieldOfAsteroids.Count > 0)
                for (int i = 0; i < FieldOfAsteroids.Count(); i++)
                {
                    FieldOfAsteroids[i].Update();
                    if (FieldOfAsteroids[i].Pos.X < 0 - FieldOfAsteroids[i].Size.Width)
                        FieldOfAsteroids.Remove(FieldOfAsteroids[i]);
                    else
                        FieldOfAsteroids[i].Rect = new Rectangle(FieldOfAsteroids[i].Pos, FieldOfAsteroids[i].Size);
                }
        }

        public void Clear()
        {
            FieldOfAsteroids.Clear();
        }
    }
}

