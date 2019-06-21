using System;
using System.Diagnostics;
using System.IO;

namespace Asteroids
{
    /// <summary>
    /// Класс логгер
    /// </summary>
    class Logger
    {
        private readonly string logFilePath;

        public Logger(string _logFileName)
        {
            this.logFilePath = _logFileName;

            if (File.Exists(this.logFilePath))
                File.Delete(this.logFilePath);

            Asteroid.ShipCollision += (s, e) => Log($"Столкновение корабля с другим объектом (Energy = {Game.abc.energy})");
            Asteroid.BulletCollision += (s, e) => Log($"Поподание снаряда (Score = {Game.schet})");
            Game._event += (msg) => Log(msg);
        }
        /// <summary>
        /// Записывает в лог сообщение
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message)
        {
            PrintLog(message);
            FileLog(message);
        }

        private void PrintLog(string message) => Debug.WriteLine($"{DateTime.Now} => {message}");

        private void FileLog(string message)
        {
            using (StreamWriter sw = new StreamWriter(logFilePath, true, System.Text.Encoding.Default))
            {
                sw.WriteLine($"{DateTime.Now} => {message}");
            }
        }
    }
}
