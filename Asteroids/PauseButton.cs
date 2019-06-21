using System.Drawing;

namespace Asteroids
{
    /// <summary>
    /// Кнопка паузы
    /// </summary>
    class PauseButton : BaseObject
    {
        /// <summary>
        /// Наведён ли указатель мыши на кнопку
        /// </summary>
        private bool hover;

        public PauseButton(Point _pos, Point _dir, Size _size)
            : base(_pos, _dir, _size)
        {
            Rect = new Rectangle(_pos, _size);
            Game.gameForm.MouseClick += (s, e) =>
            {
                if (!Game.gofl)
                {
                    var mouseLocation = e.Location;
                    if (this.Rect.IntersectsWith(new Rectangle(e.Location, new Size(1, 1))))
                        Game.Pause();
                }
            };
            Game.gameForm.MouseMove += (s, e) =>
            {
                if (e.X >= this.Pos.X 
                 && e.X <= this.Pos.X + this.Size.Height 
                 && e.Y >= this.Pos.Y
                 && e.Y <= this.Pos.Y + this.Size.Height)
                    hover = true;
                else
                    hover = false;
            };
        }

        public override void Draw()
        {
            if(!hover)
                Game.buf.Graphics.DrawImage(Game.pbi, this.Pos.X + 5, this.Pos.Y + 5, this.Size.Width - 10, this.Size.Height - 10);
            else
                Game.buf.Graphics.DrawImage(Game.pbih, this.Pos.X, this.Pos.Y, this.Size.Width, this.Size.Height);
        }
    }
}
