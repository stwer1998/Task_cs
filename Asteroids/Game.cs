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
		internal static int w;
		/// <summary>
		/// Высота главного окна
		/// </summary>
		internal static int h;
		/// <summary>
		/// Кол-во выстрелов
		/// </summary>
		internal static int hit = 0;
		/// <summary>
		/// Игровой счёт
		/// </summary>
		internal static int schet = 0;

		internal static AsteroidsField asteroidnoePole;
		internal static IGameObject staticheskiyFon;
		internal static IGameObject animirovanniyFon;
		/// <summary>
		/// Корабль игрока
		/// </summary>
		internal static StarShip abc;
		/// <summary>
		/// Снаряды
		/// </summary>
		internal static List<Bullet> b = new List<Bullet>();
		/// <summary>
		/// Взрыв
		/// </summary>
		internal static Collapse bah;
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
		internal static int mX;
		/// <summary>
		/// Текущая координат Y курсора мыши
		/// </summary>
		internal static int mY;
		// Изображения
		internal static Image[] ci;
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
		private static Timer t = new Timer { Interval = 5 };

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
			w = form.ClientSize.Width;
			h = form.ClientSize.Height;
			//Cursor.Hide();

			// Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
			buf = _context.Allocate(g, new Rectangle(0, 0, w, h));
			bufb = _context.Allocate(g, new Rectangle(0, 0, w, h));
			asteroidnoePole = new AsteroidsField();
			staticheskiyFon = new Background();
			animirovanniyFon = new BackgroundAnimation();
			abc = new StarShip(new Point(50, Game.h / 2), 5, new Size(90, 30));
			bah = new Collapse();
			pauseButton = new PauseButton(new Point(Game.w / 2 - 20, 10), new Point(0, 0), new Size(80, 80));

			t.Start();
			t.Tick += (s, e) => {
				//Рисуем все объекты
				buf.Graphics.Clear(Color.Black);
				staticheskiyFon.Draw();
				animirovanniyFon.Draw();

				foreach (var item in b)
				{
					item.Draw();
				}

				abc.Draw();
				bah.Draw();
				asteroidnoePole.Draw();

				decimal statistic = schet != 0 && hit != 0 ? (decimal)schet / hit : 0;
				buf.Graphics
					.DrawString($"Энергия: {abc.energy}     Сбито вражеских кораблей: {schet}     Сделано выстрелов: {hit}     Результативность: {(statistic * 100):F2}%",
					new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular), Brushes.White, 0, 0);
				pauseButton.Draw();
				buf.Render();

				// Обновляем все объекты
				if (b.Count > 0)
					for (int i = 0; i < b.Count; i++)
					{
						b[i]?.Update();
						if (b[i].Pos.X >= Game.w || b[i].Pos.X < -b[i].Size.Width)
						{
							b.Remove(b[i]);
							i = b.Count;
						}
					}
				asteroidnoePole.Update();
				animirovanniyFon.Update();
				abc.Update();
				bah.Update();

				if (gofl)
					Finish();
				if (ftfl) { pfl = true; Pause(); ftfl = false; Pause(); }
			};

			form.Resize += (sender, e) => {
				_event?.Invoke("Попытка изменить размер окна");
				Form form1 = (Form)sender;
				if (form1.WindowState == FormWindowState.Minimized)
				{
					t.Stop();
				}
				if (form1.WindowState == FormWindowState.Maximized)
				{
					t.Start();
				}
				form1.Width = w;
				form1.Height = h;
			};
			form.KeyDown += (s, e) => { abc.ChangeShip(); };
			form.MouseMove += (s, e) => {
				mX = e.X;
				mY = e.Y;
			};
			form.MouseClick += (s, e) => {
				if (!gofl && !pfl)
				{
					if (b.Count < 5)
					{
						hit++;
						b.Add(new Bullet(new Point(abc.Pos.X + abc.Size.Width + 1, abc.Pos.Y + abc.Size.Height - 5), abc.Dir, new Size(32, 8)));
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
		public static void Finish()
		{
			_event?.Invoke("Конец игры");
			t.Stop();
			buf.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif,
			60, FontStyle.Underline), Brushes.White, w / 2 - 180, h / 2 - 90);
			buf.Render();
			Sounds.AmbientMusicOff();
			gameForm.MouseClick += Form_MouseClick_First;
		}

		/// <summary>
		/// Пауза
		/// </summary>
		public static void Pause()
		{
			if (pfl)
			{
				_event?.Invoke("Игра снята с паузы");
				t.Start();
			}
			else
			{
				_event?.Invoke("Игра поставлена на паузу");
				t.Stop();
				int fontSize = 15;

				string str = "Asteroids";
				buf.Graphics.DrawString(str, new Font(FontFamily.GenericSansSerif,
				fontSize * 2, FontStyle.Bold), Brushes.White, w / 2 - str.Length / 2 * (fontSize - 7) * 3, h / 2 - 140);

				str = "Цель игры пролететь как можно дальше через астероидное поле";
				buf.Graphics.DrawString(str, new Font(FontFamily.GenericSansSerif,
				fontSize, FontStyle.Regular), Brushes.White, w / 2 - (str.Length * (fontSize - 5)) / 2, h / 2 - 70);

				str = "Управляйте кораблём с помощью мыши(клик - запуск ракеты)";
				buf.Graphics.DrawString(str, new Font(FontFamily.GenericSansSerif,
				fontSize, FontStyle.Regular), Brushes.White, w / 2 - (str.Length * (fontSize - 5)) / 2, h / 2 - 30);

				str = "Уворачивайтесь от столкновений и сбивайте астероиды";
				buf.Graphics.DrawString(str, new Font(FontFamily.GenericSansSerif,
				fontSize, FontStyle.Regular), Brushes.White, w / 2 - (str.Length * (fontSize - 5)) / 2, h / 2 + 10);

				str = "Картинку корабля можно поменять нажав Enter на клавиатуре";
				buf.Graphics.DrawString(str, new Font(FontFamily.GenericSansSerif,
				fontSize, FontStyle.Bold), Brushes.White, w / 2 - (str.Length * (fontSize - 5)) / 2, h / 2 + 120);

				buf.Render();
			}
			pfl = !(pfl);

		}

		private static void Form_MouseClick_First(object sender, EventArgs e)
		{
			Sounds.AmbientMusicOn();
			gameForm.MouseClick -= Form_MouseClick_First;
			hit = 0;
			schet = 0;
			gofl = false;
			pfl = true;
			abc.Pos = new Point(50, Game.h / 2);
			abc.energy = 100;
			Cursor.Position = abc.Pos;
			asteroidnoePole.Clear();
			t.Start();
			Pause();
		}		
	}
}