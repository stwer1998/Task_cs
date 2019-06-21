using System;
using System.Windows.Forms;
using System.Drawing;
using MyGame;
using System.Collections.Generic;

namespace Asteroids
{
	/// <summary>
	/// Главный класс игры Asteroids
	/// </summary>
	static class Game
	{
		internal static BufferedGraphicsContext _context;
		internal static BufferedGraphics buf;
		internal static BufferedGraphics bufb;
		/// <summary>
		/// Ширина главного окна
		/// </summary>
		internal static int width;
		/// <summary>
		/// Высота главного окна
		/// </summary>
		internal static int height;
		/// <summary>
		/// Кол-во выстрелов
		/// </summary>
		internal static int counthit;
		/// <summary>
		/// Игровой счёт
		/// </summary>
		internal static int score;

		internal static AsteroidsField asteroidnoePole;
		internal static IGameObject staticheskiyFon;
		internal static IGameObject animirovanniyFon;
		/// <summary>
		/// Корабль игрока
		/// </summary>
		internal static StarShip playerShip;
		/// <summary>
		/// Снаряды
		/// </summary>
		internal static List<Bullet> shells = new List<Bullet>();
		/// <summary>
		/// Взрыв
		/// </summary>
		internal static Collapse explosion;
		internal static Form gameForm;
		/// <summary>
		/// Кнопка паузы
		/// </summary>
		internal static PauseButton pauseButton;

		internal static bool pfl = false;
		internal static bool gofl = false;
		internal static bool ftfl = true;
		/// <summary>
		/// Текущая координат X курсора мыши
		/// </summary>
		internal static int currentCordinateX;
		/// <summary>
		/// Текущая координат Y курсора мыши
		/// </summary>
		internal static int currentCordinateY;
		// Изображения
		internal static Image[] ci;//даже не придумал название потомучто не понятно что это
		internal static Image[] bi;
		internal static Image[] ssi;
		internal static Image[] esi;
		internal static Image[] ai;
		internal static Image[] di;
		internal static Image pbi;
		internal static Image pbih;
		/// <summary>
		/// Таймер
		/// </summary>
		private static Timer timer = new Timer { Interval = 5 };

		internal delegate void GameEventHandler(string message);
		internal static event GameEventHandler _event;

		static Game()
		{

		}
		/// <summary>
		/// Инициализация игры
		/// </summary>
		/// <param name="form"></param>
		public static bool Init(Form form)
		{
			//Загружаем картинки
			try
			{
				ci = new Image[]
				{
					new Bitmap("images\\collapse1.png"),
					new Bitmap("images\\collapse2.png"),
					new Bitmap("images\\collapse3.png"),
				};
				bi = new Image[]
				{
					new Bitmap("images\\bulletL.png"),
					new Bitmap("images\\bulletR.png"),
				};
				pbi = new Bitmap("images\\pause.png");
				pbih = new Bitmap("images\\pause_hover.png");
				ssi = new Image[]
				{
					new Bitmap("images\\ship0R.png"),
					new Bitmap("images\\ship1R.png"),
					new Bitmap("images\\ship2R.png"),
					new Bitmap("images\\ship3R.png"),
				};
				esi = new Image[]
				{
					new Bitmap("images\\ship0L.png"),
					new Bitmap("images\\ship1L.png"),
					new Bitmap("images\\ship2L.png"),
					new Bitmap("images\\ship3L.png"),
				};
				ai = new Image[]
				{
					new Bitmap("images\\a1.png"),
					new Bitmap("images\\a2.png"),
					new Bitmap("images\\a3.png"),
					new Bitmap("images\\a4.png"),
					new Bitmap("images\\aid.png"),
				};
				di = new Image[]
				{
					new Bitmap("images\\star0.jpg"),
					new Bitmap("images\\star.jpg"),
					new Bitmap("images\\star1.jpg"),
					new Bitmap("images\\star2.jpg")
				};
			}
			catch
			{
				MessageBox.Show("Не удалось загрузить один или нсколько файлов изображений.", "Ошибка ввода/вывода");
				return false;
			}

			_event?.Invoke("Старт игры");
			gameForm = form;
			// Графическое устройство для вывода графики
			Graphics g;

			// Предоставляет доступ к главному буферу графического контекста для текущего приложения
			_context = BufferedGraphicsManager.Current;
			g = form.CreateGraphics();

			// Создаем объект (поверхность рисования) и связываем его с формой
			// Запоминаем размеры формы
			gameForm.Text = "Asteroids";
			width = form.ClientSize.Width;
			height = form.ClientSize.Height;
			//Cursor.Hide();

			// Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
			buf = _context.Allocate(g, new Rectangle(0, 0, width, height));
			bufb = _context.Allocate(g, new Rectangle(0, 0, width, height));
			asteroidnoePole = new AsteroidsField();
			staticheskiyFon = new Background();
			animirovanniyFon = new BackgroundAnimation();
			playerShip = new StarShip(new Point(50, Game.height / 2), 5, new Size(90, 30));
			explosion = new Collapse();
			pauseButton = new PauseButton(new Point(Game.width / 2 - 20, 10), new Point(0, 0), new Size(80, 80));

			timer.Start();
			timer.Tick += (s, e) => {
				//Рисуем все объекты
				buf.Graphics.Clear(Color.Black);
				staticheskiyFon.Draw();
				animirovanniyFon.Draw();

				foreach (var item in shells)
				{
					item.Draw();
				}

				playerShip.Draw();
				explosion.Draw();
				asteroidnoePole.Draw();

				decimal statistic = score != 0 && counthit != 0 ? (decimal)score / counthit : 0;
				buf.Graphics
					.DrawString($"Энергия: {playerShip.energy}     Сбито вражеских кораблей: {score}     Сделано выстрелов: {counthit}     Результативность: {(statistic * 100):F2}%",
					new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular), Brushes.White, 0, 0);
				pauseButton.Draw();
				buf.Render();

				// Обновляем все объекты
				
					for (int i = 0; i < shells.Count; i++)
					{
						shells[i]?.Update();
						if (shells[i].Pos.X >= Game.width || shells[i].Pos.X < -shells[i].Size.Width)
						{
							shells.Remove(shells[i]);
							i = shells.Count;
						}
					}
				asteroidnoePole.Update();
				animirovanniyFon.Update();
				playerShip.Update();
				explosion.Update();

				if (gofl)
					ToFinish();
				if (ftfl) { pfl = true; ToPause(); ftfl = false; ToPause(); }
			};

           

			form.Resize += (sender, e) => {
				_event?.Invoke("Попытка изменить размер окна");
				Form form1 = (Form)sender;
				if (form1.WindowState == FormWindowState.Minimized)
				{
					timer.Stop();
				}
				if (form1.WindowState == FormWindowState.Maximized)
				{
					timer.Start();
				}
				form1.Width = width;
				form1.Height = height;
			};
			form.KeyDown += (s, e) => { playerShip.ChangeShip(); };
			form.MouseMove += (s, e) => {
				currentCordinateX = e.X;
				currentCordinateY = e.Y;
			};
			form.MouseClick += (s, e) => {
				if (!gofl && !pfl)
				{
					if (shells.Count < 5)
					{
						counthit++;
						shells.Add(new Bullet(new Point(playerShip.Pos.X + playerShip.Size.Width + 1, playerShip.Pos.Y + playerShip.Size.Height - 5), playerShip.Dir, new Size(32, 8)));
						Sounds.PlayShootSound();
					}
				}
			};
			form.MouseClick += Form_MouseClick_First;
			form.FormClosing += (s, e) => Sounds.AmbientMusicOff();
			form.LocationChanged += (sender, e) =>
			{
				form.SetDesktopLocation(0, 0);
			};
			return true;
		}

		/// <summary>
		/// Остановка игры
		/// </summary>
		public static void ToFinish()
		{
			_event?.Invoke("Конец игры");
			timer.Stop();
			buf.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif,
			60, FontStyle.Underline), Brushes.White, width / 2 - 180, height / 2 - 90);
			buf.Render();
			Sounds.AmbientMusicOff();
			gameForm.MouseClick += Form_MouseClick_First;
		}

		/// <summary>
		/// Пауза
		/// </summary>
		public static void ToPause()
		{
			if (pfl)
			{
				_event?.Invoke("Игра снята с паузы");
				timer.Start();
			}
			else
			{
				_event?.Invoke("Игра поставлена на паузу");
				timer.Stop();
				int fontSize = 15;

				string str = "Asteroids";
				buf.Graphics.DrawString(str, new Font(FontFamily.GenericSansSerif,
				fontSize * 2, FontStyle.Bold), Brushes.White, width / 2 - str.Length / 2 * (fontSize - 7) * 3, height / 2 - 140);

				str = "Цель игры пролететь как можно дальше через астероидное поле";
				buf.Graphics.DrawString(str, new Font(FontFamily.GenericSansSerif,
				fontSize, FontStyle.Regular), Brushes.White, width / 2 - (str.Length * (fontSize - 5)) / 2, height / 2 - 70);

				str = "Управляйте кораблём с помощью мыши(клик - запуск ракеты)";
				buf.Graphics.DrawString(str, new Font(FontFamily.GenericSansSerif,
				fontSize, FontStyle.Regular), Brushes.White, width / 2 - (str.Length * (fontSize - 5)) / 2, height / 2 - 30);

				str = "Уворачивайтесь от столкновений и сбивайте астероиды";
				buf.Graphics.DrawString(str, new Font(FontFamily.GenericSansSerif,
				fontSize, FontStyle.Regular), Brushes.White, width / 2 - (str.Length * (fontSize - 5)) / 2, height / 2 + 10);

				str = "Картинку корабля можно поменять нажав Enter на клавиатуре";
				buf.Graphics.DrawString(str, new Font(FontFamily.GenericSansSerif,
				fontSize, FontStyle.Bold), Brushes.White, width / 2 - (str.Length * (fontSize - 5)) / 2, height / 2 + 120);

				buf.Render();
			}
			pfl = !(pfl);

		}

		private static void Form_MouseClick_First(object sender, EventArgs e)
		{
			Sounds.AmbientMusicOn();
			gameForm.MouseClick -= Form_MouseClick_First;
			counthit = 0;
			score = 0;
			gofl = false;
			pfl = true;
			playerShip.Pos = new Point(50, Game.height / 2);
			playerShip.energy = 100;
			Cursor.Position = playerShip.Pos;
			asteroidnoePole.Clear();
			timer.Start();
			ToPause();
		}		
	}
}