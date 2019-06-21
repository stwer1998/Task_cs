using Asteroids;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace MyGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new Logger("Log.txt");
            Form form = new Form();
            form.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            form.MaximizeBox = false;

            if (Game.Init(form))
            {
                form.Show();
                Application.Run(form);

                logger.Log("Стоп");
            }
            else
                logger.Log("Выполнение прервано из-за ошибки. Недоступны ресурсы приложения.");
        }
    }
}
