using System.Media;
using System.Windows.Forms;
using WMPLib;

namespace Asteroids
{
    /// <summary>
    /// Игровые звуки
    /// </summary>
    static class Sounds
    {
        /// <summary>
        /// Объект для фоновой музыки
        /// </summary>
        private static SoundPlayer bgMusic;
        private readonly static string shootSound;
        private readonly static string enemyShootSound;
        private readonly static string boomSound;
        private readonly static string aidSound;
        private static int counter;
        private const int maxCountSounds = 15;
        /// <summary>
        /// Массив объектов для воспроизведения звука
        /// </summary>
        private static WindowsMediaPlayer[] playArray;

        static Sounds()
        {           
            shootSound = @"sounds\shoot.wav";
            boomSound = @"sounds\boom.wav";
            aidSound = @"sounds\aid.wav";
            enemyShootSound = @"sounds\enemyshoot.wav";
            playArray = new WindowsMediaPlayer[maxCountSounds];
            for (int i = 0; i < playArray.Length; i++)
            {
                playArray[i] = new WindowsMediaPlayer();
            }
        }
        /// <summary>
        /// Включает фоновую музыку
        /// </summary>
        public static void AmbientMusicOn()
        {
            try
            {
                bgMusic = new SoundPlayer(@"sounds\music.wav");
                bgMusic.PlayLooping();
            }
            catch
            {
                MessageBox.Show("Не удалось загрузить файл с музыкой", "Ошибка ввода/вывода");
            }
        }
        /// <summary>
        /// Выключает фоновую музыку
        /// </summary>
        public static void AmbientMusicOff()
        {
            bgMusic.Stop();
        }

        public static void PlayShootSound()
        {
            PlaySound(shootSound);
        }

        public static void PlayBoomSound()
        {
            PlaySound(boomSound);
        }

        public static void PlayAidSound()
        {
            PlaySound(aidSound);
        }

        public static void PlayEnemyShootSound()
        {
            PlaySound(enemyShootSound);
        }

        /// <summary>
        /// Проигрывает звук
        /// </summary>
        /// <param name="soundPath">путь к звуковому файлу</param>
        private static void PlaySound(string soundPath)
        {
            counter = counter < (maxCountSounds - 1) ? ++counter : 0;
            try
            {
                playArray[counter].URL = soundPath;
                //playArray[counter].controls.play();
            }
            catch
            {

            }
        }
    }
}
