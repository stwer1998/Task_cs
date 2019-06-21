namespace Asteroids
{
    interface IGameObject
    {
        /// <summary>
        /// Выводит все объекты на экран
        /// </summary>
        void Draw();
        /// <summary>
        /// Выполняет просчёт изменений в оъектах
        /// </summary>
        void Update();
    }
}
