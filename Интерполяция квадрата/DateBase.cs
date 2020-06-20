using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using info.lundin.math;

namespace Интерполяция_квадрата
{
	public partial class Graph2D : Form
	{
		MyColor[] c;
		Curcle curcle;
		public Thread thread;
		public List<int> NumbersGrig = new List<int>();
		public List<Thread> ParallelFuncThread = new List<Thread>();
		public int IndexGrid = 0;
		public int IndexCoordinat = 0;
		public double[,] GridCoordinats;
		public double[,] pixsel; // пиксельная панель декртовая система координат
		public double[,] pixsel2 = new double[401, 361]; // пиксельная панель 
		public double MinOriginal = 0;
		public double MaxOriginal = 0;
		public double Min = 0;
		public double Max = 0;
		public double MaxZnatLine = 0;
		public double ParamFunction = 0;
		public Point Start;
		public Point End;
		public PointF StartCoordinat;
		public PointF EndCoordinat;
		public PointF StartGridPolyar;
		public PointF EndGridPolyar;
		public List<Bitmap> bitmaps = new List<Bitmap>(); // все кадры
		public Bitmap bmFunction; // график функции
		public Bitmap bmFunctionCadr; // график функции
		public Bitmap bmGrid; // сетка
		public Bitmap bmCoordinat; // система координат
		public Graphics gr; // объект для рисования на PictutreBox
		public int PointX = 0;
		public int PointY = 0;
		public int k = 1; // Счётчик кадров
		public bool F = false;
		int PicResize = 0;
		public int SizeGrid = 2;   //Размер сетки
		public double SizeAxis = 2; //размер координатной оси
		public double ZFunctionFull = 0;
		GetFunction getFunction; // Создание функции 
		public Thread ParallelFunction;
		public Stopwatch timer;
		static readonly object locker = new object();

		public int Time = 0;
		public int TimeParallel = 0;

		public int TimeDifference = 0;

		private string ZFunctionDecartStatic { get; set; }   //Считанная функция 
		private string ZFunctionPolyarStatic { get; set; }
		private string ZFunctionDecartDinamic { get; set; }
		private string ZFunctionPolyarDinamic { get; set; }

		public double Zfunction = 0;

		public Graph2D()
		{
			c = new MyColor[7];

			for (int i = 0; i < c.Length; i++)
			{
				c[i] = new MyColor();
			}

			int NumberGrid = 2;
			do
			{
				if ((480 % NumberGrid == 0) && (NumberGrid % 2 == 0))
				{
					NumbersGrig.Add(NumberGrid);
				}
				NumberGrid++;
			}
			while (NumberGrid <= 200);

			//Цветовая палитра
			#region
			//ТЕСТОВЫЕ ЦВЕТА - Тёплый цвет перетекание из красного в жёлтый
			//c[0].SetColor(255, 0, 0); // красный
			//c[1].SetColor(161, 38, 34); // красный
			//c[2].SetColor(235, 135, 44);// оранжевый
			//c[3].SetColor(255, 182, 78);// жёлтый
			//c[4].SetColor(255, 217, 131); // зелёный
			//c[5].SetColor(251, 234, 167); // голубой
			//c[6].SetColor(255, 254, 211); // синий
			//							  //c[6].SetColor(128, 0, 255); //фиолетовый


			////c[0].SetColor(234, 96, 68); // красный
			////c[1].SetColor(255, 130, 92);// оранжевый
			////c[2].SetColor(235, 173, 96);// жёлтый
			////c[3].SetColor(31, 176, 121); // зелёный
			////c[4].SetColor(87, 194, 184); // голубой
			////c[5].SetColor(103, 170, 199); // синий
			////c[6].SetColor(135, 146, 192); //фиолетовый

			// Основные 
			//c[0].SetColor(0, 0, 55); // темно синий
			//c[1].SetColor(0, 0, 255);// синий
			//c[2].SetColor(0, 164, 170);// голубой
			//c[3].SetColor(0, 255, 0); // зелёный
			//c[4].SetColor(255, 255, 0);// жёлтый
			//c[5].SetColor(255, 128, 0); // оранжевый
			//c[6].SetColor(164, 0, 0); //красный
			#endregion


			c[0].SetColor(0, 24, 100); // темно синий
			c[1].SetColor(0, 36, 151);// синий
			c[2].SetColor(0, 255, 255);// голубой
			c[3].SetColor(0, 255, 0); // зелёный
			c[4].SetColor(255, 255, 1);// жёлтый
			c[5].SetColor(219, 90, 0); // оранжевый
			c[6].SetColor(205, 11, 1); //красный



			InitializeComponent();
			Get();
			pictureBox5.Image = global::Интерполяция_квадрата.Properties.Resources.icons8_меню_2_48;
			GenericCoordinats(); //Генерация системы координат

			DisplayGridStartForm(); // Отрисовка сетки 

			getFunction = new GetFunction(ZFunctionDecartStatic, ZFunctionPolyarStatic,
				ZFunctionDecartDinamic, ZFunctionPolyarDinamic);

			button1.Enabled = false;
			ParallelBtn.Enabled = false;
		}

		// Построить график функции
		async private void button1_Click( object sender, EventArgs e )
		{

			timer = new Stopwatch();
			timer.Start();
			await Task.Run(() =>
			{
				this.Invoke(new Action(() =>
				{

					BlockUiElements();
				}));

				if (checkBox1.Checked == true)
				{
					try
					{

						// c сеткой
						if (checkBox3.Checked == true)
						{
							if (checkBox5.Checked == true)  // с линиями уровня
							{
								DisplayFunction(); //Перерисовка функции


								gr = Graphics.FromImage(bmFunction);

								//Отрисовка сетки 
								for (int x = (480 / SizeGrid); x < pictureBox4.Width; x += (480 / SizeGrid))
								{
									Start.X = 0;
									Start.Y = x;

									End.X = pictureBox1.Width - 1;
									End.Y = x;

									curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
									curcle.LineDisplay();

									Start.X = x;
									Start.Y = 0;

									End.X = x;
									End.Y = pictureBox1.Height - 1;

									curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
									curcle.LineDisplay();
								}
								if (checkBox7.Checked == true)
								{
									MarchingSquares(); // Ставим линии уровня
								}
								else if (checkBox7.Checked == false)
								{
									MarchingSquaresOriginal(); // Ставим линии уровня
								}

							}
							else if (checkBox5.Checked == false)// без линий уровня
							{

								DisplayFunction(); //Перерисовка функции

								gr = Graphics.FromImage(bmFunction);

								//Отрисовка сетки 
								for (int x = (480 / SizeGrid); x < pictureBox4.Width; x += (480 / SizeGrid))
								{
									Start.X = 0;
									Start.Y = x;

									End.X = pictureBox1.Width - 1;
									End.Y = x;

									curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
									curcle.LineDisplay();

									Start.X = x;
									Start.Y = 0;

									End.X = x;
									End.Y = pictureBox1.Height - 1;

									curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
									curcle.LineDisplay();
								}
							}



							this.Invoke(new Action(() =>
							{
								pictureBox4.Image = (Image)(bmFunction);
							}));

						}
						else // без сетки
						{
							if (checkBox5.Checked == true)  // с линиями уровня
							{
								DisplayFunction(); //Перерисовка функции

								if (checkBox7.Checked == true)
								{
									MarchingSquares(); // Ставим линии уровня
								}
								else if (checkBox7.Checked == false)
								{
									MarchingSquaresOriginal(); // Ставим линии уровня
								}

								this.Invoke(new Action(() =>
								{
									pictureBox4.Image = (Image)(bmFunction);
								}));
							}
							else if (checkBox5.Checked == false)// без линий уровня
							{
								DisplayFunction(); //Перерисовка функции

								this.Invoke(new Action(() =>
								{
									pictureBox4.Image = (Image)(bmFunction);
								}));
							}


						}
					}
					catch
					{

						this.Invoke(new Action(() =>
						{

							button2.Enabled = false;


							bmFunction = null; // Удаляем нашу функцию
							pictureBox4.Image = null;
							checkBox2.Checked = false;

							if (checkBox3.Checked == true)
							{
								DisplayGridStartForm(); // Отрисовка сетки при декартовой систему координат
							}
							else if (checkBox4.Checked == true)
							{
								bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								gr = Graphics.FromImage(bmGrid);
								pictureBox4.Image = bmGrid;
							}
						}));
					}
				}       //декартова
				else if (checkBox2.Checked == true)
				{
					try
					{

						// c сеткой
						if (checkBox3.Checked == true)
						{
							if (checkBox5.Checked == true)  // с линиями уровня
							{
								DisplayFunctionPolyar(); //Перерисовка функции

								gr = Graphics.FromImage(bmFunction);

								//сетка для полярной системы координ
								DisplayGridPolyar();

								if (checkBox7.Checked == true)
								{
									MarchingSquares(); // Ставим линии уровня
								}
								else if (checkBox7.Checked == false)
								{
									MarchingSquaresOriginal(); // Ставим линии уровня
								}

								this.Invoke(new Action(() =>
								{

									pictureBox4.Image = (Image)(bmFunction);
								}));
							}
							else if (checkBox5.Checked == false)// без линий уровня
							{
								DisplayFunctionPolyar(); //Перерисовка функции

								gr = Graphics.FromImage(bmFunction);

								//сетка для полярной системы координ
								DisplayGridPolyar();



								this.Invoke(new Action(() =>
								{

									pictureBox4.Image = (Image)(bmFunction);
								}));
							}

						}
						else //без сетки
						{


							if (checkBox5.Checked == true)  // с линиями уровня
							{
								DisplayFunctionPolyar();//Перерисовка функции

								gr = Graphics.FromImage(bmFunction);


								if (checkBox7.Checked == true)
								{
									MarchingSquares(); // Ставим линии уровня
								}
								else if (checkBox7.Checked == false)
								{
									MarchingSquaresOriginal(); // Ставим линии уровня
								}

								this.Invoke(new Action(() =>
								{

									pictureBox4.Image = (Image)(bmFunction);
								}));
							}
							else if (checkBox5.Checked == false)// без линий уровня
							{
								DisplayFunctionPolyar();//Перерисовка функции
								this.Invoke(new Action(() =>
								{
									pictureBox4.Image = (Image)(bmFunction);
								}));

							}
						}
					}
					catch
					{
						this.Invoke(new Action(() =>
						{



							button2.Enabled = false;

							bmFunction = null; // Удаляем нашу функцию
							checkBox1.Checked = false;
							pictureBox4.Image = null;

							if (checkBox3.Checked == true) // с сеткой
							{
								BlockUiElements();

								//pictureBox4.BorderStyle = BorderStyle.None;

								bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								gr = Graphics.FromImage(bmGrid);

								DisplayGridPolyar();  // Рисуем сетку полярной системы координат

								pictureBox4.Image = bmGrid;
								OpenUiElements();

							}
							else if (checkBox4.Checked == true)
							{
								bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								gr = Graphics.FromImage(bmGrid);
								pictureBox4.Image = bmGrid;
							}
						}));
					}
				}

				this.Invoke(new Action(() =>
				{
					OpenUiElements();
				}));
			});
			timer.Stop();
			Time = Convert.ToInt32(timer.ElapsedMilliseconds);
			if (TimeParallel != 0)
			{
				if (Time > TimeParallel)
				{
					TimeDifference = Time - TimeParallel;
				}
				else
				{
					TimeDifference = TimeParallel - Time;
				}
			}
			label9.Text = Time.ToString() + " ms";
			label11.Text = TimeDifference.ToString() + " ms";
		}


		//Расчитать график функции	декартова+	 
		public void DisplayFunction()
		{
			bmFunction = new Bitmap(pictureBox4.Width, pictureBox4.Height);
			gr = Graphics.FromImage(bmFunction);

			gr.SmoothingMode = SmoothingMode.AntiAlias;
			F = true;
			try
			{
				GetFunctionZnath(); //просчитываем значения в узлах сетки и проводим интерполяцию данных 

				for (int i = 0; i < 481; i++)
				{
					for (int j = 0; j < 481; j++)
						//экранные координаты
						bmFunction.SetPixel(j, i, getColor(pixsel[i, j]));
				}

			}
			catch (Exception)
			{

				//thread.Abort();
				this.Invoke(new Action(() =>
				{
					MessageBox.Show("Введите пожалуйста корректно функцию или проверьте не присутствует ли деление на 0", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);

					if (thread != null)
						thread.Abort();

					if (F)
					{

						bmFunction = null; // Удаляем нашу функцию
						pictureBox4.Image = null;
						//checkBox2.Checked = false;

						if (checkBox3.Checked == true)
						{
							DisplayGridStartForm(); // Отрисовка сетки при декартовой систему координат
						}
						else if (checkBox4.Checked == true)
						{
							bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
							gr = Graphics.FromImage(bmGrid);
							pictureBox4.Image = bmGrid;
						}

						//pictureBox4.Image = null;
						OpenUiElements();
						return;


						//pictureBox4.Image = null;
						//OpenUiElements();
						//return;
					}
					else
					{


						button2.Enabled = false;
						//thread.Abort();
						bitmaps.Clear();
						OpenUiElements();
						StopThreadBtn.Enabled = false;
						label80.Text = "";
						progressBar1.Value = 0;
						F = true;
						k = 0;
					}


				}));
			}


		}
		//Расчитать график	 
		public void DisplayFunctionPolyar()
		{
			bmFunction = new Bitmap(pictureBox4.Width, pictureBox4.Height);
			gr = Graphics.FromImage(bmFunction);
			int XX = 0;
			int YY = 0;
			double Distans = 0;
			int radius = ((pictureBox4.Height - 1) / 2);
			gr.SmoothingMode = SmoothingMode.AntiAlias;
			F = true;

			try
			{
				//	// Просчитать узлы
				GetFunctionZnathPolyar();

				for (int i = 0; i < 481; i++)
				{

					for (int j = 0; j < 481; j++)
					{
						XX = (radius * -1) + j;
						YY = radius - i;
						Distans = Math.Sqrt(XX * XX + YY * YY);
						if (Distans <= radius)
							bmFunction.SetPixel(j, i, getColor(pixsel[i, j]));

					}



				}
			}
			//MessageBox.Show("Введите пожалуйста корректно функцию или проверьте не присутствует ли деление на 0", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
			catch (Exception)
			{
				//thread.Abort();

				this.Invoke(new Action(() =>
				{
					MessageBox.Show("Введите пожалуйста корректно функцию или проверьте не присутствует ли деление на 0", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);

					if (thread != null)
						thread.Abort();
					if (F)
					{

						button2.Enabled = false;

						bmFunction = null; // Удаляем нашу функцию
						checkBox1.Checked = false;
						pictureBox4.Image = null;

						if (checkBox3.Checked == true) // с сеткой
						{
							BlockUiElements();

							//pictureBox4.BorderStyle = BorderStyle.None;

							bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
							gr = Graphics.FromImage(bmGrid);

							DisplayGridPolyar();  // Рисуем сетку полярной системы координат

							pictureBox4.Image = bmGrid;
							OpenUiElements();

						}
						else if (checkBox4.Checked == true)
						{
							bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
							gr = Graphics.FromImage(bmGrid);
							pictureBox4.Image = bmGrid;
						}


						//pictureBox4.Image = null;
						OpenUiElements();
						return;

					}
					else
					{


						button2.Enabled = false;
						//thread.Abort();
						bitmaps.Clear();
						OpenUiElements();
						StopThreadBtn.Enabled = false;
						label80.Text = "";
						progressBar1.Value = 0;
						F = true;
						k = 0;
					}


				}));
			}

			//break;
		}

		SettingCadr settingCadr;


		double T = 0;
		double ShagT = 0;
		double ColKadr = 0;
		int CollichCadrov = 0;
		double ValueColProgressBar = 0;
		double ShagBrogressBar = 0;

		// Открываем поток для расчётов	кадров функции
	 public void Threading()
		{
			//await Task.Run(() =>
			//{
				try
				{

					if (settingCadr.Flag == 1)
					{
						this.Invoke(new Action(() =>
						{

							bitmaps.Clear();
							OpenUiElements();
							StopThreadBtn.Enabled = false;
							label80.Text = "";
							progressBar1.Value = 0;
							k = 0;

							thread.Abort();
						}));
					}


				}
				catch (Exception)
				{
					this.Invoke(new Action(() =>
					{

						StopThreadBtn.Enabled = false;
						OpenUiElements();
						thread.Abort();
					}));

				}

				this.Invoke(new Action(() =>
				{
					BlockUiElements();
				}));
				try
				{

					for (double h = T; h < ColKadr; h += ShagT)    // 100 кадров
					{
						bmFunctionCadr = new Bitmap(pictureBox4.Width, pictureBox4.Height);
						gr = Graphics.FromImage(bmFunctionCadr);
						ParamFunction = h;
						int XX = 0;
						int YY = 0;
						double Distans = 0;
						int radius = ((pictureBox4.Height - 1) / 2);

						if (checkBox1.Checked == true)
						{
							if (ParallelCardr)
							{

								DisplayFunctionParallel();
								//foreach (Thread parallel in ParallelFuncThread)
								//	parallel.Abort();
							}

							else
							{

								GetFunctionZnath(); //просчитываем значения в узлах сетки и проводим интерполяцию данных
							}

							for (int i = 0; i < 481; i++)
							{
								for (int j = 0; j < 481; j++)
								{
									//экранные координаты
									bmFunctionCadr.SetPixel(j, i, getColor(pixsel[i, j]));

								}
							}
						}
						else if (checkBox2.Checked == true)
						{

							if (ParallelCardr)
							{
								GetFunctionZnathPolyarParallel();
								//foreach (Thread parallel in ParallelFuncThread)
								//	parallel.Abort();

							}
							else
							{

								GetFunctionZnathPolyar();
							}

							for (int i = 0; i < 481; i++)
							{
								for (int j = 0; j < 481; j++)
								{
									XX = (radius * -1) + j;
									YY = radius - i;
									Distans = Math.Sqrt(XX * XX + YY * YY);
									if (Distans <= radius)
										bmFunctionCadr.SetPixel(j, i, getColor(pixsel[i, j]));

								}
							}

						}

						if (checkBox3.Checked == true)
						{
							if (checkBox1.Checked == true)
							{
								if (checkBox5.Checked == true)
								{
									DisplayCadrInGrig();

									if (checkBox7.Checked == true)
									{
										MarchingSquares(); // Ставим линии уровня
									}
									else if (checkBox7.Checked == false)
									{
										MarchingSquaresOriginal(); // Ставим линии уровня
									}
									bitmaps.Add(bmFunctionCadr); // Добавляем кадры в список картинок с учётом сетки
								}
								else
								{
									DisplayCadrInGrig();
									bitmaps.Add(bmFunctionCadr); // Добавляем кадры в список картинок с учётом сетки
								}

							}
							else if (checkBox2.Checked == true)
							{
								if (checkBox5.Checked == true)
								{
									DisplayCadrInGrigPolyar();

									if (checkBox7.Checked == true)
									{
										MarchingSquares(); // Ставим линии уровня
									}
									else if (checkBox7.Checked == false)
									{
										MarchingSquaresOriginal(); // Ставим линии уровня
									}
									bitmaps.Add(bmFunctionCadr); // Добавляем кадры в список картинок с учётом сетки
								}
								else
									bitmaps.Add(bmFunctionCadr); // Добавляем кадры в список картинок с учётом сетки

							}
						}
						else if (checkBox4.Checked == true)
						{
							if (checkBox5.Checked == true)
							{


								if (checkBox7.Checked == true)
								{
									MarchingSquares(); // Ставим линии уровня
								}
								else if (checkBox7.Checked == false)
								{
									MarchingSquaresOriginal(); // Ставим линии уровня
								}

								bitmaps.Add(bmFunctionCadr); // Добавляем кадры в список картинок без сетки
							}
							else
								bitmaps.Add(bmFunctionCadr); // Добавляем кадры в список картинок без сетки

						}

						if (h >= ValueColProgressBar)
						{

							this.Invoke(new Action(() => { label80.Text = "Идёт обработка кадров " + (k).ToString() + "%"; }));

							this.Invoke(new Action(() => { progressBar1.Value = k; }));
							ValueColProgressBar += ShagBrogressBar;
							k++;
						}


					}
				}

				catch
				{
					this.Invoke(new Action(() =>
					{
						MessageBox.Show("Неверно установлена функция для обработки кадров или произошла внезапная остановка процесса обработки кадров!", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
						bitmaps.Clear();
						OpenUiElements();
						StopThreadBtn.Enabled = false;
						label80.Text = "";
						progressBar1.Value = 0;
						k = 0;

						thread.Abort();
					}));
				}

				this.Invoke(new Action(() =>
				{
					progressBar1.Value = 100;
					label80.Text = "Обработка кадров завершена";
					OpenUiElements();
					F = true;
					button2.Enabled = true;
					StopThreadBtn.Enabled = false;
					ParallelCardr = false;

				}));
			//});
		}

		private Color getColor( double i )
		{

			double len = (Max / 6);// длина одного участка
			Color color = Color.AntiqueWhite;
			int n = (int)(i / len); // номер учатка в котором мы находимся
			double k = (i - (len * n)) / len; // в какой позиции  мы находимся
			int r = 0, g = 0, b = 0;
			if (n > 5) n = 5;

			r = (int)(c[n].r + (c[n + 1].r - c[n].r) * k);
			g = (int)(c[n].g + (c[n + 1].g - c[n].g) * k);
			b = (int)(c[n].b + (c[n + 1].b - c[n].b) * k);

			color = Color.FromArgb(255, r, g, b);


			return color;
		}

		//Составление легенды
		private Color getColorPanel2( int i )
		{
			double len = (panel2.Width / 6);// длина одного участка
			int n = (int)(i / len); // номер учатка в котором мы находимся
			double k = (i - (len * n)) / len; // в какой позиции  мы находимся
			int r, g, b;
			if (n > 5) n = 5;
			r = (int)(c[n].r + (c[n + 1].r - c[n].r) * k);
			g = (int)(c[n].g + (c[n + 1].g - c[n].g) * k);
			b = (int)(c[n].b + (c[n + 1].b - c[n].b) * k);
			return Color.FromArgb(255, r, g, b);
		}

		// Билинейная Интерполяция - нахождение промежуточных значений квадрата
		private void PointSquart( int LeftTopY, int LeftTopX, int RightTopY,
			int RightTopX, int LeftBottomY, int LeftBottomX, int RightBottomY, int RightBottomX )
		{                   //0
			for (int i = LeftTopY + 1; i < LeftBottomY; i++)
			{   //левая грань квадрата         //y2 -y                            //y2 - y1
				pixsel[i, LeftTopX] = (((Convert.ToDouble(LeftTopY) - Convert.ToDouble(i)) / (Convert.ToDouble(LeftTopY) - Convert.ToDouble(LeftBottomY))) *

					pixsel[LeftBottomY, LeftBottomX]) + (((Convert.ToDouble(i) - Convert.ToDouble(LeftBottomY)) / (Convert.ToDouble(LeftTopY) - Convert.ToDouble(LeftBottomY))) * pixsel[LeftTopY, LeftTopX]);
				//правая грань квадрата
				//y2 -y                           //y2 - y1
				pixsel[i, RightTopX] = (((Convert.ToDouble(RightTopY) - Convert.ToDouble(i)) / (Convert.ToDouble(RightTopY) - Convert.ToDouble(RightBottomY))) *

					pixsel[RightBottomY, RightBottomX]) + (((Convert.ToDouble(i) - Convert.ToDouble(RightBottomY)) / (Convert.ToDouble(RightTopY) - Convert.ToDouble(RightBottomY))) * pixsel[RightTopY, RightTopX]);
			}

			for (int i = LeftTopX + 1; i < RightTopX; i++)
			{  //R2(Верхняя грань квадрата)                //x2-x               //x2-x1                       
				pixsel[LeftTopY, i] = (((Convert.ToDouble(RightTopX) - Convert.ToDouble(i)) / (Convert.ToDouble(RightTopX) - Convert.ToDouble(LeftTopX))) *

					pixsel[LeftTopY, LeftTopX]) + (((Convert.ToDouble(i) - Convert.ToDouble(LeftTopX)) / (Convert.ToDouble(RightTopX) - Convert.ToDouble(LeftTopX))) * pixsel[RightTopY, RightTopX]);
				//R1(Нижняя грань квадрата)
				//x2-x                    //x2-x1                  
				pixsel[LeftBottomY, i] = (((Convert.ToDouble(RightBottomX) - Convert.ToDouble(i)) / (Convert.ToDouble(RightBottomX) - Convert.ToDouble(LeftBottomX))) *

					pixsel[LeftBottomY, LeftBottomX]) + (((Convert.ToDouble(i) - Convert.ToDouble(LeftBottomX)) / (Convert.ToDouble(RightBottomX) - Convert.ToDouble(LeftBottomX))) * pixsel[RightBottomY, RightBottomX]);
				//P(Все значения от верхней грани до нижней)
				for (int j = LeftTopY + 1; j < LeftBottomY; j++)
				{       //f(R2)                     //y2-y                          //y2 - y1
					pixsel[j, i] = (((Convert.ToDouble(LeftTopY) - Convert.ToDouble(j)) / (Convert.ToDouble(LeftTopY) - Convert.ToDouble(LeftBottomY))) *

						pixsel[LeftBottomY, i]) + (((Convert.ToDouble(j) - Convert.ToDouble(LeftBottomY)) / (Convert.ToDouble(LeftTopY) - Convert.ToDouble(LeftBottomY))) * pixsel[LeftTopY, i]);
				}
			}
		}

		//расчитываем функцию в каждом пикселе декартова систеима координат  
		public void GetFunctionZnath()
		{
			pixsel = new double[481, 481];
			//16
			GridCoordinats = new double[SizeGrid + 1, SizeGrid + 1];

			double Shag = SizeAxis / (SizeGrid / 2);
			double ShagX = 0;
			double ShagY = 0;
			//заполняем значениями сетку
			for (int i = 0; i < SizeGrid + 1; i++)
			{
				for (int j = 0; j < SizeGrid + 1; j++)
				{                //8
					double x = (SizeAxis * -1) + ShagX;
					//8
					double y = SizeAxis - ShagY;
					double f = Function(x, y);

					GridCoordinats[i, j] = f;
					ShagX += Shag;
				}
				ShagX = 0;
				ShagY += Shag;

			}

			//1 шаг ищём максимальное и минимальное значение в узлах исходной сетки 

			MinOriginal = GridCoordinats[0, 0];
			MaxOriginal = GridCoordinats[0, 0];

			for (int i = 0; i < SizeGrid + 1; i++)
			{
				for (int j = 0; j < SizeGrid + 1; j++)
				{
					if (GridCoordinats[i, j] < MinOriginal)
						MinOriginal = GridCoordinats[i, j];

					if (GridCoordinats[i, j] > MaxOriginal)
						MaxOriginal = GridCoordinats[i, j];

					//if (GridCoordinats[i, j]  MaxOriginal)
					//	MaxOriginal = GridCoordinats[i, j];
				}
			}

			//Минимальное  значение функции умноженное на -1
			MaxZnatLine = MinOriginal * -1;

			ShagX = 0;
			ShagY = 0;

			//Устанавливает значения в узлы сетки 
			for (int i = 0; i < SizeGrid + 1; i++)
			{
				for (int j = 0; j < SizeGrid + 1; j++)
				{
					double x = (SizeAxis * -1) + ShagX;
					double y = SizeAxis - ShagY;

					if (MinOriginal >= 0)
					{
						GridCoordinats[i, j] = Function(x, y);
						ShagX += Shag;
					}
					else
					{

						GridCoordinats[i, j] = Function(x, y) + MaxZnatLine;
						ShagX += Shag;
					}


				}
				ShagX = 0;
				ShagY += Shag;

			}

			Min = GridCoordinats[0, 0];
			Max = GridCoordinats[0, 0];

			//Находим масимальное значение в узлах изменённой сетки 
			for (int i = 0; i < SizeGrid + 1; i++)
			{
				for (int j = 0; j < SizeGrid + 1; j++)
				{
					if (GridCoordinats[i, j] < Min)
						Min = GridCoordinats[i, j];

					if (GridCoordinats[i, j] > Max)
						Max = GridCoordinats[i, j];
				}
			}

			PointX = 0;
			PointY = 0;
			//переносим значения в узлах сетки на пиксельную сетку
			for (int i = 0; i < 481; i += 480 / SizeGrid)
			{
				for (int j = 0; j < 481; j += 480 / SizeGrid)
				{
					pixsel[i, j] = GridCoordinats[PointY, PointX];
					PointX++;
				}
				PointY++;
				PointX = 0;
			}

			for (int i = 0; i <= 480 - (480 / SizeGrid); i += 480 / SizeGrid)
			{
				for (int j = 0; j <= 480 - (480 / SizeGrid); j += 480 / SizeGrid)
				{
					//координаты углов квадрата
					int LeftTopY = i;
					int LeftTopX = j;
					int RightTopY = i;
					int RightTopX = j + (480 / SizeGrid);
					int LeftBottomY = i + (480 / SizeGrid);
					int LeftBottomX = j;
					int RightBottomY = i + (480 / SizeGrid);
					int RightBottomX = j + (480 / SizeGrid);

					PointSquart(LeftTopY, LeftTopX, RightTopY, RightTopX,
						LeftBottomY, LeftBottomX, RightBottomY, RightBottomX);

				}

			}

		}

		//расчитываем функцию в каждом пикселе для полярной системы	
		public void GetFunctionZnathPolyar()
		{
			pixsel = new double[481, 481];
			//16
			GridCoordinats = new double[SizeGrid + 1, SizeGrid + 1];

			double Shag = SizeAxis / (SizeGrid / 2);
			double ShagX = 0;
			double ShagY = 0;
			//заполняем значениями сетку
			for (int i = 0; i < SizeGrid + 1; i++)
			{
				for (int j = 0; j < SizeGrid + 1; j++)
				{                //8
					double x = (SizeAxis * -1) + ShagX;
					//8
					double y = SizeAxis - ShagY;
					double f = ConvertToPolyar(x, y);

					GridCoordinats[i, j] = f;
					ShagX += Shag;
				}
				ShagX = 0;
				ShagY += Shag;

			}

			//1 шаг ищём максимальное и минимальное значение в узлах исходной сетки 

			MinOriginal = GridCoordinats[0, 0];
			MaxOriginal = GridCoordinats[0, 0];

			for (int i = 0; i < SizeGrid + 1; i++)
			{
				for (int j = 0; j < SizeGrid + 1; j++)
				{
					if (GridCoordinats[i, j] < MinOriginal)
						MinOriginal = GridCoordinats[i, j];

					if (GridCoordinats[i, j] > MaxOriginal)
						MaxOriginal = GridCoordinats[i, j];
				}
			}

			//Минимальное  значение функции умноженное на -1
			MaxZnatLine = MinOriginal * -1;

			ShagX = 0;
			ShagY = 0;

			//Устанавливает значения в узлы сетки 
			for (int i = 0; i < SizeGrid + 1; i++)
			{
				for (int j = 0; j < SizeGrid + 1; j++)
				{
					double x = (SizeAxis * -1) + ShagX;
					double y = SizeAxis - ShagY;

					if (MinOriginal >= 0)
					{
						GridCoordinats[i, j] = ConvertToPolyar(x, y);
						ShagX += Shag;
					}
					else
					{

						GridCoordinats[i, j] = ConvertToPolyar(x, y) + MaxZnatLine;
						ShagX += Shag;
					}


				}
				ShagX = 0;
				ShagY += Shag;

			}

			Min = GridCoordinats[0, 0];
			Max = GridCoordinats[0, 0];

			//Находим масимальное значение в узлах изменённой сетки 
			for (int i = 0; i < SizeGrid + 1; i++)
			{
				for (int j = 0; j < SizeGrid + 1; j++)
				{
					if (GridCoordinats[i, j] < Min)
						Min = GridCoordinats[i, j];

					if (GridCoordinats[i, j] > Max)
						Max = GridCoordinats[i, j];
				}
			}

			PointX = 0;
			PointY = 0;
			//переносим значения в узлах сетки на пиксельную сетку
			for (int i = 0; i < 481; i += 480 / SizeGrid)
			{
				for (int j = 0; j < 481; j += 480 / SizeGrid)
				{
					pixsel[i, j] = GridCoordinats[PointY, PointX];
					PointX++;
				}
				PointY++;
				PointX = 0;
			}

			for (int i = 0; i <= 480 - (480 / SizeGrid); i += 480 / SizeGrid)
			{
				for (int j = 0; j <= 480 - (480 / SizeGrid); j += 480 / SizeGrid)
				{
					//координаты углов квадрата
					int LeftTopY = i;
					int LeftTopX = j;
					int RightTopY = i;
					int RightTopX = j + (480 / SizeGrid);
					int LeftBottomY = i + (480 / SizeGrid);
					int LeftBottomX = j;
					int RightBottomY = i + (480 / SizeGrid);
					int RightBottomX = j + (480 / SizeGrid);

					PointSquart(LeftTopY, LeftTopX, RightTopY, RightTopX,
						LeftBottomY, LeftBottomX, RightBottomY, RightBottomX);

				}

			}
		}


		public double zFunc = 0;

		//Вводим функцию для  декартовой!	
		public double Function( double x, double y )
		{
			zFunc = 0;

			//try
			//{
			if (F)  // Обычное построение функции
			{
				#region
				//X^2+Y^2+Sin(4*X)+Sin(4*Y)
				//Math.Sin(Math.Pow(x * y, 2)) + Math.Cos(Math.Pow(x, 4) * Math.Pow(x * y, 2));		  !!!Важно	1
				//(Math.Sin(Math.Pow(x * y, 5)) + Math.Cos(Math.Pow(x, 5) * Math.Pow(x * y, 2))) * 6 Важно 2
				//5 * x * (Math.Sin(Math.Pow(x * y, 5))) - 10 * y + Math.Cos(Math.Pow(x, 5)) * 28   !!
				//Math.Sin(Math.Pow(x * y, 2)) * Math.Cos(Math.Pow(x, 4) * Math.Pow(x * y, 2))
				//log(x^2+y^2)^3
				// Math.Sqrt(x*x + y*y) + 3 * Math.Cos(Math.Sqrt(x*x + y*y)) + 5 * Math.Cos(x * y)
				//x * Math.Sin(y) + y * Math.Sin(x)
				// Math.Sqrt(x * x + y * y) + 3 * Math.Cos(Math.Sqrt(x * x + y * y)) + 5 * Math.Cos(x * y)!
				#endregion
				//20 + Math.pow(x[0],2) + Math.pow(x[1], 2) - 10*(Math.cos(2*Math.PI * x[0]) + Math.cos(2*Math.PI * x[1]))
				//Функция Химмельблау - f(x,y)  = (x^2+y-11)^2 + (x+y^2-7)^2
				//Функция Растригина - 	 20 + Math.Pow(x, 2) + Math.Pow(y, 2) - 10 * (Math.Cos(2 * Math.PI * x) + Math.Cos(2 * Math.PI * y))

				//Function At = new Function("At(b,h) = (sin(b^2+h)-11)^2 + sin((b+h^2-7)^2) * pi");
				//Expression el1 = new Expression("At(2,4)", At);

				//this.Invoke(new Action(() =>
				//{
				//	textBox1.Text = Convert.ToString(el1.calculate());

				//}));
				//GetFunction getFunction = new GetFunction();

				zFunc = ZnathFunctionDecartStatic(x, y);

				//zFunc = (Math.Pow(x, 2) + Math.Pow(y, 2)) * Math.Sin(x * y);
			}
			else // Построение функции( кадрами)
			{
				//Введите функцию графику двух переменных для вычисления кадров	 (ParamFunction - это  наше T)
				//zFunc = 20 + Math.Pow(x, 2) + Math.Pow(y, 2) - 10 * (Math.Cos(2 * Math.PI * x) + Math.Cos(2 * Math.PI * y)) * Math.Sin(ParamFunction);
				zFunc = ZnathFunctionDecartDinamic(x, y, ParamFunction);

			}
			return zFunc;
		}

		//Вводим функцию для полярной!
		public double ConvertToPolyar( double x, double y )
		{
			double R = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
			double Fi = Math.Atan2(y, x);
			zFunc = 0;
			#region
			//double F = R * Math.Cos(Fi);

			//double F =( Math.Sin(Fi) * (1 + (Math.Tan(Fi /2)))) / 4;
			//double F = (Math.Sin(Fi * R) + Math.Cos(Fi)) * Math.Cos(R); // 

			//double F = (Math.Sqrt(R + Math.Cos(Fi)) + Math.Cos(R)) + Math.Exp(Math.Cos(Fi)); // 
			//Math.Cos(R) * Math.Cos(Fi * 3.0)!
			//Math.Sin(R * Math.Sin(R)) - Math.Cos(Fi * Math.Cos(Fi))
			#endregion

			//try
			//{
			if (F)// Обычное построение функции
			{

				//zFunc = Math.Cos(R) * Math.Cos(Fi * 3.0);
				zFunc = ZnathfunctionPolyarStatic(Fi, R);
			}
			else // Построение функции (кадрами)
			{
				// Задайте функцию для вычисления кадров (ParamFunction - это наше T)
				//zFunc = ((R + Math.Cos(Fi))) + Math.Cos(R) + Math.Cos(Fi) * Math.Sin(R * ParamFunction);
				zFunc = ZnathfunctionPolyarDinamic(Fi, R, ParamFunction);

			}

			return zFunc;
		}
		private void panel2_Paint( object sender, PaintEventArgs e )
		{
			Point X1;
			Point Y1;
			Curcle curcle1;
			for (int i = 0; i <= panel2.Width; i++)
			{
				X1 = new Point(i, 0);
				Y1 = new Point(i, panel2.Height);

				//Ставим деления на координатную сетку
				curcle1 = new Curcle(X1, Y1, getColorPanel2(panel2.Width - i), panel2.CreateGraphics());
				curcle1.LineDisplay();
			}
		}

		private void panel2_Resize( object sender, EventArgs e )
		{
			panel2.Invalidate();
		}

		Point moveStart;
		private void panel3_MouseDown( object sender, MouseEventArgs e )
		{
			if (e.Button == MouseButtons.Left)
			{
				moveStart = new Point(e.X, e.Y);
			}

		}

		private void panel3_MouseMove( object sender, MouseEventArgs e )
		{
			if ((e.Button & MouseButtons.Left) != 0)
			{

				Point deltaPos = new Point(e.X - moveStart.X, e.Y - moveStart.Y);

				this.Location = new Point(this.Location.X + deltaPos.X,
				this.Location.Y + deltaPos.Y);
			}
		}

		private void pictureBox2_Click( object sender, EventArgs e )
		{
			if (thread != null)
				thread.Abort();

			this.Close();
		}


		//Будет проходить по всем битмапам и выводить изображения  
		async private void button2_Click( object sender, EventArgs e )
		{
			BlockUiElements();
			button2.Enabled = false;
			progressBar1.Value = 0;
			label80.Text = "";
			foreach (Bitmap bit in bitmaps)
			{
				pictureBox4.Image = bit;
				await Task.Delay(60);
			}
			button2.Enabled = true;
			OpenUiElements();
		}
		public bool ParallelCardr = false;
		private void StartTreatment_Click( object sender, EventArgs e )
		{
			k = 1;
			F = false;
			StopThreadBtn.Enabled = true;
			button2.Enabled = false;

			bitmaps.Clear(); // Очистка кадров


			settingCadr = new SettingCadr();

			settingCadr.ShowDialog();

			settingCadr.textBoxT.Text = settingCadr.textBoxT.Text.Replace(".", ",");
			settingCadr.textBoxShagT.Text = settingCadr.textBoxShagT.Text.Replace(".", ",");
			ParallelCardr = settingCadr.checkBoxParallel.Checked;
			settingCadr.Focus();
			//try
			//{

			T = Convert.ToDouble(settingCadr.textBoxT.Text);
			ShagT = Convert.ToDouble(settingCadr.textBoxShagT.Text);
			CollichCadrov = Convert.ToInt32(settingCadr.textBox1.Text);



			ColKadr = ((ShagT * CollichCadrov) + T) - ShagT;

			ValueColProgressBar = ColKadr / 100;
			ShagBrogressBar = ColKadr / 100;
			//Проверка
			//(() => GetFunctionLeftTopResizePolyar()))
			thread = new Thread(() => Threading());
			thread.Start();


		}

		public void BlockUiElements()
		{
			checkBox1.Enabled = false;
			checkBox2.Enabled = false;
			checkBox3.Enabled = false;
			checkBox4.Enabled = false;

			checkBox5.Enabled = false;
			checkBox6.Enabled = false;
			checkBox7.Enabled = false;
			checkBox8.Enabled = false;


			SizeAxisMinusBn.Enabled = false;
			SizeAxisPlusBn.Enabled = false;
			SizeGrigMinusBn.Enabled = false;
			SizeGrigPlusBn.Enabled = false;
			button1.Enabled = false;
			ParallelBtn.Enabled = false;
			GetFunctonS.Enabled = false;

			StartTreatment.Enabled = false;
		}
		public void OpenStopCadr()
		{
			StopThreadBtn.Enabled = false;
		}
		public void OpenUiElements()
		{
			checkBox1.Enabled = true;
			checkBox2.Enabled = true;
			checkBox3.Enabled = true;
			checkBox4.Enabled = true;

			checkBox5.Enabled = true;
			checkBox6.Enabled = true;
			checkBox7.Enabled = true;
			checkBox8.Enabled = true;

			SizeAxisMinusBn.Enabled = true;
			SizeAxisPlusBn.Enabled = true;
			SizeGrigMinusBn.Enabled = true;
			SizeGrigPlusBn.Enabled = true;
			button1.Enabled = true;
			GetFunctonS.Enabled = true;
			ParallelBtn.Enabled = true;
			StartTreatment.Enabled = true;

		}

		Pen pn;
		Rectangle rect;

		#region
		//private void Spherise()
		//{
		//	for (int i = 0; i < vertices.Count; i++)
		//	{
		//		float radius = this.radius;
		//		float longitude = 0;
		//		float latitude = 0;

		//		float sphereRadius = 32;

		//		Color color = vertices[i].Color;

		//		ToPolar(vertices[i].Position - centre, out radius, out longitude, out latitude);
		//		Vector3 position = ToCartesian(sphereRadius, longitude, latitude) + centre;

		//		Vector3 normal = vertices[i].Position - centre;
		//		normal.Normalize();

		//		const float lerpAmount = 0.6f;
		//		Vector3 lerp = (position - vertices[i].Position) * lerpAmount + vertices[i].Position;
		//		vertices[i] = new VertexPositionColorNormal(lerp, color, normal);
		//	}
		//}

		//private void ToPolar( Vector3 cart, out float radius, out float longitude, out float latitude )
		//{
		//	radius = (float)Math.Sqrt((double)(cart.X * cart.X + cart.Y * cart.Y + cart.Z * cart.Z));
		//	longitude = (float)Math.Acos(cart.X / Math.Sqrt(cart.X * cart.X + cart.Y * cart.Y)) * (cart.Y < 0 ? -1 : 1);
		//	latitude = (float)Math.Acos(cart.Z / radius) * (cart.Z < 0 ? -1 : 1);
		//}

		//private Vector3 ToCartesian( float radius, float longitude, float latitude )
		//{
		//	float x = radius * (float)(Math.Sin(latitude) * Math.Cos(longitude));
		//	float y = radius * (float)(Math.Sin(latitude) * Math.Sin(longitude));
		//	float z = radius * (float)Math.Cos(latitude);

		//	return new Vector3(x, y, z);
		//}
		#endregion

		// Интрополяция информационной панели 
		private void panel4_Paint( object sender, PaintEventArgs e )
		{

		}

		public class MyColor
		{
			public int r, g, b;
			public static int max;

			public void SetColor( int R, int G, int B )
			{
				r = R;
				g = G;
				b = B;
			}
		}

		//Обработчик для декартовой системы координат  
		private void checkBox1_CheckedChanged( object sender, EventArgs e )
		{
			if (checkBox1.Checked == true)
			{
				button2.Enabled = false;
				pictureBox4.BorderStyle = BorderStyle.FixedSingle;

				bmFunction = null; // Удаляем нашу функцию
				pictureBox4.Image = null;
				checkBox2.Checked = false;

				if (checkBox3.Checked == true)
				{
					DisplayGridStartForm(); // Отрисовка сетки при декартовой систему координат
				}
				else if (checkBox4.Checked == true)
				{
					bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
					gr = Graphics.FromImage(bmGrid);
					pictureBox4.Image = bmGrid;
				}
			}

			else
			{

				if (checkBox2.Checked == true)
					checkBox1.Checked = false;
				else
					checkBox1.Checked = true;

			}
			label9.Text = "";
			label11.Text = "";
			label8.Text = "";
			button1.Enabled = false;
			ParallelBtn.Enabled = false;

		}

		// Обработчик для полярной системы координат  
		private void checkBox2_CheckedChanged( object sender, EventArgs e )
		{

			if (checkBox2.Checked == true) // полярная
			{
				button2.Enabled = false;
				pictureBox4.BorderStyle = BorderStyle.None;
				bmFunction = null; // Удаляем нашу функцию
				checkBox1.Checked = false;
				pictureBox4.Image = null;

				if (checkBox3.Checked == true) // с сеткой
				{
					BlockUiElements();

					bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
					gr = Graphics.FromImage(bmGrid);

					DisplayGridPolyar();  // Рисуем сетку полярной системы координат

					pictureBox4.Image = bmGrid;
					OpenUiElements();

				}
				else if (checkBox4.Checked == true)
				{
					bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
					gr = Graphics.FromImage(bmGrid);
					pictureBox4.Image = bmGrid;
				}

			}
			else
			{
				if (checkBox1.Checked == true)
					checkBox2.Checked = false;
				else
					checkBox2.Checked = true;
			}
			button1.Enabled = false;
			ParallelBtn.Enabled = false;
			label9.Text = "";
			label11.Text = "";
			label8.Text = "";

		}


		//Показать сетку
		async private void checkBox3_CheckedChanged( object sender, EventArgs e )
		{
			await Task.Run(() =>
			{
				if (checkBox3.Checked == true)//Показать сетку
				{
					this.Invoke(new Action(() => { checkBox4.Checked = false; }));
					this.Invoke(new Action(() => { BlockUiElements(); }));

					if ((checkBox1.Checked == true))    //декартова
					{

						if (checkBox5.Checked == true)  //С сеткой
						{

							if (bmFunction != null)
							{
								gr = Graphics.FromImage(bmFunction);

								DisplayFunction();

								for (int x = (480 / SizeGrid); x < pictureBox4.Width; x += (480 / SizeGrid))
								{
									Start.X = 0;
									Start.Y = x;

									End.X = pictureBox4.Width - 1;
									End.Y = x;

									curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
									curcle.LineDisplay();

									Start.X = x;
									Start.Y = 0;

									End.X = x;
									End.Y = pictureBox4.Height - 1;

									curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
									curcle.LineDisplay();


								}
								if (checkBox7.Checked == true)
								{
									MarchingSquares(); // Ставим линии уровня
								}
								else if (checkBox7.Checked == false)
								{
									MarchingSquaresOriginal(); // Ставим линии уровня
								}

								this.Invoke(new Action(() => { pictureBox4.Image = bmFunction; }));
							}
							else
							{
								bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								gr = Graphics.FromImage(bmGrid);

								for (int x = (480 / SizeGrid); x < pictureBox4.Width; x += (480 / SizeGrid))
								{
									Start.X = 0;
									Start.Y = x;

									End.X = pictureBox4.Width - 1;
									End.Y = x;

									curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
									curcle.LineDisplay();

									Start.X = x;
									Start.Y = 0;

									End.X = x;
									End.Y = pictureBox4.Height - 1;

									curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
									curcle.LineDisplay();

								}
								this.Invoke(new Action(() => { pictureBox4.Image = bmGrid; }));
							}


						}  // с линиями
						else     // без линий
						{
							if (bmFunction != null)
							{
								gr = Graphics.FromImage(bmFunction);
								DisplayFunction();

								// Рисуем сетку полярной системы координат
								for (int x = (480 / SizeGrid); x < pictureBox4.Width; x += (480 / SizeGrid))
								{
									Start.X = 0;
									Start.Y = x;

									End.X = pictureBox4.Width - 1;
									End.Y = x;

									curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
									curcle.LineDisplay();

									Start.X = x;
									Start.Y = 0;

									End.X = x;
									End.Y = pictureBox4.Height - 1;

									curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
									curcle.LineDisplay();


								}
								//GenericCoordinats();
								this.Invoke(new Action(() => { pictureBox4.Image = bmFunction; }));
							}
							else
							{
								bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								gr = Graphics.FromImage(bmGrid);

								for (int x = (480 / SizeGrid); x < pictureBox4.Width; x += (480 / SizeGrid))
								{
									Start.X = 0;
									Start.Y = x;

									End.X = pictureBox4.Width - 1;
									End.Y = x;

									curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
									curcle.LineDisplay();

									Start.X = x;
									Start.Y = 0;

									End.X = x;
									End.Y = pictureBox4.Height - 1;

									curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
									curcle.LineDisplay();


								}
								this.Invoke(new Action(() => { pictureBox4.Image = bmGrid; }));
							}
						}
						this.Invoke(new Action(() => { OpenUiElements(); }));

					} //декартова 
					else if ((checkBox2.Checked == true))
					{

						this.Invoke(new Action(() => { checkBox1.Checked = false; }));
						this.Invoke(new Action(() => { BlockUiElements(); }));
						//pictureBox4.BorderStyle = BorderStyle.None;
						if (bmFunction != null)
						{

							gr = Graphics.FromImage(bmFunction);
							DisplayGridPolyar();
							// Рисуем сетку полярной системы координат


							this.Invoke(new Action(() => { pictureBox4.Image = bmFunction; }));

						}
						else
						{
							bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
							gr = Graphics.FromImage(bmGrid);
							DisplayGridPolyar(); ;
							// Рисуем сетку полярной системы координат
							this.Invoke(new Action(() => { pictureBox4.Image = bmGrid; }));


						}

						this.Invoke(new Action(() => { OpenUiElements(); }));

					}  // полярная

				}
				else
				{
					this.Invoke(new Action(() => { OpenUiElements(); }));
					if (checkBox4.Checked == true)
					{
						this.Invoke(new Action(() => { checkBox3.Checked = false; }));


					}
					else
					{
						this.Invoke(new Action(() => { checkBox3.Checked = true; }));

					}

				}
			});

		}

		//Скрыть сетку		  
		async private void checkBox4_CheckedChanged( object sender, EventArgs e )
		{

			await Task.Run(() =>
			{

				if (checkBox4.Checked == true)
				{
					this.Invoke(new Action(() =>
					{
						checkBox3.Checked = false;
						BlockUiElements();

					}));

					if ((checkBox1.Checked == true))      // декартова
					{
						this.Invoke(new Action(() =>
						{
							BlockUiElements();

						}));

						if (bmFunction != null)
						{
							this.Invoke(new Action(() =>
							{

								BlockUiElements();
								//gr = Graphics.FromImage(bmFunction);
							}));
							if (checkBox5.Checked == true)
							{
								DisplayFunction();
								if (checkBox7.Checked == true)
								{
									MarchingSquares(); // Ставим линии уровня
								}
								else if (checkBox7.Checked == false)
								{
									MarchingSquaresOriginal(); // Ставим линии уровня
								}
								this.Invoke(new Action(() => { pictureBox4.Image = bmFunction; }));
							}    //ставим изолинии
							else
							{
								DisplayFunction();
								this.Invoke(new Action(() => { pictureBox4.Image = bmFunction; }));
							}

						}
						else
						{
							this.Invoke(new Action(() => { pictureBox4.Image = null; }));
							//this.Invoke(new Action(() =>
							//{
							//	OpenUiElements();
							//}));
						}


					}
					else if (checkBox2.Checked == true)
					{
						this.Invoke(new Action(() =>
						{
							BlockUiElements();
						}));

						if (bmFunction != null)
						{
							//this.Invoke(new Action(() => { BlockUiElements(); }));
							this.Invoke(new Action(() =>
						{
							BlockUiElements();

						}));
							if (checkBox5.Checked == true) //ставим изолинии
							{
								DisplayFunctionPolyar();
								if (checkBox7.Checked == true)
								{
									MarchingSquares(); // Ставим линии уровня
								}
								else if (checkBox7.Checked == false)
								{
									MarchingSquaresOriginal(); // Ставим линии уровня
								}

								this.Invoke(new Action(() => { pictureBox4.Image = bmFunction; }));
							}   //изолинии 
							else
							{
								DisplayFunctionPolyar();
								this.Invoke(new Action(() => { pictureBox4.Image = bmFunction; }));

							}

						}
						else
						{
							this.Invoke(new Action(() =>
							{
								BlockUiElements();

							}));
							this.Invoke(new Action(() => { pictureBox4.Image = null; }));
							//this.Invoke(new Action(() =>
							//{
							//	OpenUiElements();
							//}));
							this.Invoke(new Action(() =>
						{
							OpenUiElements();

						}));

						}

					}   // полярная

					this.Invoke(new Action(() =>
				{
					OpenUiElements();

				}));

				}
				else
				{
					this.Invoke(new Action(() =>
					{
						OpenUiElements();

					}));

					if (checkBox3.Checked == true)
					{
						this.Invoke(new Action(() => { checkBox4.Checked = false; }));
						//this.Invoke(new Action(() => { OpenUiElements(); }));

					}
					else
					{
						this.Invoke(new Action(() => { checkBox4.Checked = true; }));
						//this.Invoke(new Action(() => { OpenUiElements(); }));
					}
				}

			});
		}


		//Уменьшить размер сетки
		async private void SizeGrigMinusBn_Click( object sender, EventArgs e )
		{
			await Task.Run(() =>
			{
				if (IndexGrid == 0)
				{

					SizeGrid = NumbersGrig[0];
					IndexGrid = 0;


				}
				else
				{
					//this.Invoke(new Action(() =>
					//{
					IndexGrid--;
					SizeGrid = NumbersGrig[IndexGrid];
					//}));

				}
				if (checkBox1.Checked == true) //декартова
				{
					try
					{
						this.Invoke(new Action(() =>
						{
							BlockUiElements();
							//pictureBox4.Image = null;
							//Пересчитываем функцию
						}));
						//показать сетку
						if (checkBox3.Checked == true)
						{


							if (bmFunction != null)
							{

								DisplayFunction();
								gr = Graphics.FromImage(bmFunction);

							}
							else
							{
								//this.Invoke(new Action(() =>
								//{
								bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								gr = Graphics.FromImage(bmGrid);
								//}));


							}
							//Строим сетку
							for (int x = (480 / SizeGrid); x < 481; x += (480 / SizeGrid))
							{
								Start.X = 0;
								Start.Y = x;

								End.X = pictureBox4.Width - 1;
								End.Y = x;

								curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
								curcle.LineDisplay();

								Start.X = x;
								Start.Y = 0;

								End.X = x;
								End.Y = pictureBox4.Height - 1;

								curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
								curcle.LineDisplay();
							}
							if (bmFunction != null)
							{
								if (checkBox5.Checked == true)
								{
									if (checkBox7.Checked == true)
									{
										MarchingSquares(); // Ставим линии уровня
									}
									else if (checkBox7.Checked == false)
									{
										MarchingSquaresOriginal(); // Ставим линии уровня
									}
									this.Invoke(new Action(() =>
									{
										pictureBox4.Image = null;
										pictureBox4.Image = bmFunction;
									}));
								}
								else
								{

									this.Invoke(new Action(() =>
									{
										pictureBox4.Image = null;
										pictureBox4.Image = bmFunction;
									}));
								}

							}
							else
							{
								this.Invoke(new Action(() =>
								{
									pictureBox4.Image = null;
									pictureBox4.Image = bmGrid;
								}));

							}



						}
						else
						{
							//this.Invoke(new Action(() =>
							//{

							//	BlockUiElements();
							//}));

							if (bmFunction != null)
							{



								//Пересчитываем функцию
								DisplayFunction();

								if (checkBox5.Checked == true)
								{
									gr = Graphics.FromImage(bmFunction);

									if (checkBox7.Checked == true)
									{
										MarchingSquares(); // Ставим линии уровня
									}
									else if (checkBox7.Checked == false)
									{
										MarchingSquaresOriginal(); // Ставим линии уровня
									}

									this.Invoke(new Action(() =>
									{
										pictureBox4.Image = null;
										pictureBox4.Image = bmFunction;
									}));
								}
								else
								{

									this.Invoke(new Action(() =>
									{

										pictureBox4.Image = null;
										pictureBox4.Image = bmFunction;
										//OpenUiElements();
									}));
								}

							}

							this.Invoke(new Action(() =>
							{

								pictureBox4.Image = null;
								pictureBox4.Image = bmFunction;
								//OpenUiElements();
							}));
						}
					}
					catch
					{
						this.Invoke(new Action(() =>
						{

							button2.Enabled = false;


							bmFunction = null; // Удаляем нашу функцию
							pictureBox4.Image = null;
							checkBox2.Checked = false;

							if (checkBox3.Checked == true)
							{
								DisplayGridStartForm(); // Отрисовка сетки при декартовой систему координат
							}
							else if (checkBox4.Checked == true)
							{
								bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								gr = Graphics.FromImage(bmGrid);
								pictureBox4.Image = bmGrid;
							}
						}));
					}

					this.Invoke(new Action(() =>
					{
						OpenUiElements();
					}));
				}
				else //полярная 
				{
					try
					{

						this.Invoke(new Action(() =>
						{
							BlockUiElements();
						}));

						if (bmFunction != null)
						{


							//GenericCoordinats(); // Ставим ось координат

							//Пересчитываем функцию
							DisplayFunctionPolyar();    // пересчитали функцию в битмап Function



							if (checkBox3.Checked == true)
							{
								gr = Graphics.FromImage(bmFunction);
								DisplayGridPolyar();

								if (checkBox5.Checked == true)
								{
									if (checkBox7.Checked == true)
									{
										MarchingSquares(); // Ставим линии уровня
									}
									else if (checkBox7.Checked == false)
									{
										MarchingSquaresOriginal(); // Ставим линии уровня
									}
									this.Invoke(new Action(() =>
									{
										pictureBox4.Image = null;
										pictureBox4.Image = bmFunction;
									}));
								}
								else
								{

									this.Invoke(new Action(() =>
									{
										pictureBox4.Image = null;
										pictureBox4.Image = bmFunction;
									}));
								}

							}
							else
							{
								if (checkBox5.Checked == true)
								{
									gr = Graphics.FromImage(bmFunction);

									if (checkBox7.Checked == true)
									{
										MarchingSquares(); // Ставим линии уровня
									}
									else if (checkBox7.Checked == false)
									{
										MarchingSquaresOriginal(); // Ставим линии уровня
									}
									this.Invoke(new Action(() =>
									{
										pictureBox4.Image = null;
										pictureBox4.Image = bmFunction;
									}));
								}
								else
								{

									this.Invoke(new Action(() =>
									{
										pictureBox4.Image = null;
										pictureBox4.Image = bmFunction;
									}));
								}


							}


						}
						else
						{
							//this.Invoke(new Action(() =>
							//{
							//	BlockUiElements();
							//}));

							if (checkBox3.Checked == true)
							{
								bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								gr = Graphics.FromImage(bmGrid);

								DisplayGridPolyar();  // Рисуем сетку полярной системы координат
								this.Invoke(new Action(() =>
							{
								pictureBox4.Image = bmGrid;
							}));


							}


						}


					}
					catch
					{
						this.Invoke(new Action(() =>
						{



							button2.Enabled = false;

							bmFunction = null; // Удаляем нашу функцию
							checkBox1.Checked = false;
							pictureBox4.Image = null;

							if (checkBox3.Checked == true) // с сеткой
							{
								BlockUiElements();

								//pictureBox4.BorderStyle = BorderStyle.None;

								bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								gr = Graphics.FromImage(bmGrid);

								DisplayGridPolyar();  // Рисуем сетку полярной системы координат

								pictureBox4.Image = bmGrid;
								OpenUiElements();

							}
							else if (checkBox4.Checked == true)
							{
								bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								gr = Graphics.FromImage(bmGrid);
								pictureBox4.Image = bmGrid;
							}
						}));
					}
					this.Invoke(new Action(() =>
					{
						OpenUiElements();
					}));
				}
				//this.Invoke(new Action(() =>
				//{
				//	OpenUiElements();
				//}));

			});



		}

		//Увеличить размер сетки
		async private void SizeGrigPlusBn_Click( object sender, EventArgs e )
		{
			await Task.Run(() =>
			{
				if (IndexGrid == NumbersGrig.Count - 1)
				{
					SizeGrid = NumbersGrig[NumbersGrig.Count - 1];
					IndexGrid = NumbersGrig.Count - 1;
				}
				else
				{
					IndexGrid++;
					SizeGrid = NumbersGrig[IndexGrid];
				}
				if (checkBox1.Checked == true)
				{
					try
					{
						this.Invoke(new Action(() =>
						{
							BlockUiElements();

						}));

						if (checkBox3.Checked == true)
						{


							if (bmFunction != null)
							{


								//Пересчитываем функцию
								DisplayFunction();

								gr = Graphics.FromImage(bmFunction);
							}
							else
							{
								bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								gr = Graphics.FromImage(bmGrid);
							}
							//Строим сетку
							for (int x = (480 / SizeGrid); x < 481; x += (480 / SizeGrid))
							{
								Start.X = 0;
								Start.Y = x;

								End.X = pictureBox4.Width - 1;
								End.Y = x;

								curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
								curcle.LineDisplay();

								Start.X = x;
								Start.Y = 0;

								End.X = x;
								End.Y = pictureBox4.Height - 1;

								curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
								curcle.LineDisplay();
							}
							if (bmFunction != null)
							{
								if (checkBox5.Checked == true)
								{
									if (checkBox7.Checked == true)
									{
										MarchingSquares(); // Ставим линии уровня
									}
									else if (checkBox7.Checked == false)
									{
										MarchingSquaresOriginal(); // Ставим линии уровня
									}
									this.Invoke(new Action(() =>
									{
										pictureBox4.Image = null;
										pictureBox4.Image = bmFunction;
									}));
								}
								else
								{

									this.Invoke(new Action(() =>
									{
										pictureBox4.Image = null;
										pictureBox4.Image = bmFunction;
									}));
								}

							}
							else
							{
								this.Invoke(new Action(() =>
								{
									pictureBox4.Image = null;
									pictureBox4.Image = bmGrid;
								}));

							}




						}
						else
						{
							//this.Invoke(new Action(() =>
							//{
							//	BlockUiElements();
							//}));

							if (bmFunction != null)
							{


								//Пересчитываем функцию
								DisplayFunction();

								if (checkBox5.Checked == true)
								{
									if (checkBox7.Checked == true)
									{
										MarchingSquares(); // Ставим линии уровня
									}
									else if (checkBox7.Checked == false)
									{
										MarchingSquaresOriginal(); // Ставим линии уровня
									}
									this.Invoke(new Action(() =>
									{
										pictureBox4.Image = bmFunction;
									}));

								}
								else
								{


									this.Invoke(new Action(() =>
									{
										pictureBox4.Image = bmFunction;
									}));
								}



							}
						}
					}
					catch
					{
						this.Invoke(new Action(() =>
						{

							button2.Enabled = false;


							bmFunction = null; // Удаляем нашу функцию
							pictureBox4.Image = null;
							checkBox2.Checked = false;

							if (checkBox3.Checked == true)
							{
								DisplayGridStartForm(); // Отрисовка сетки при декартовой систему координат
							}
							else if (checkBox4.Checked == true)
							{
								bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								gr = Graphics.FromImage(bmGrid);
								pictureBox4.Image = bmGrid;
							}
						}));
					}
					this.Invoke(new Action(() =>
					{
						OpenUiElements();
					}));

				}
				else
				{
					try
					{

						this.Invoke(new Action(() =>
						{
							BlockUiElements();
						}));

						if (bmFunction != null)
						{

							//GenericCoordinats(); // Ставим ось координат

							//Пересчитываем функцию
							DisplayFunctionPolyar();    // пересчитали функцию в битмап Function

							if (checkBox3.Checked == true)
							{
								gr = Graphics.FromImage(bmFunction);
								//сетка
								DisplayGridPolyar();

								if (checkBox5.Checked == true)
								{
									if (checkBox7.Checked == true)
									{
										MarchingSquares(); // Ставим линии уровня
									}
									else if (checkBox7.Checked == false)
									{
										MarchingSquaresOriginal(); // Ставим линии уровня
									}
									this.Invoke(new Action(() =>
									{
										pictureBox4.Image = null;
										pictureBox4.Image = bmFunction;
									}));
								}
								else
								{

									this.Invoke(new Action(() =>
									{
										pictureBox4.Image = null;
										pictureBox4.Image = bmFunction;
									}));
								}

							}
							else
							{
								gr = Graphics.FromImage(bmFunction);
								if (checkBox5.Checked == true)
								{
									if (checkBox7.Checked == true)
									{
										MarchingSquares(); // Ставим линии уровня
									}
									else if (checkBox7.Checked == false)
									{
										MarchingSquaresOriginal(); // Ставим линии уровня
									}
									this.Invoke(new Action(() =>
									{
										pictureBox4.Image = null;
										pictureBox4.Image = bmFunction;
									}));
								}
								else
								{

									this.Invoke(new Action(() =>
									{
										pictureBox4.Image = null;
										pictureBox4.Image = bmFunction;
									}));
								}


							}


						}
						else
						{
							//this.Invoke(new Action(() =>
							//{
							//	BlockUiElements();
							//}));

							if (checkBox3.Checked == true)
							{
								bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								gr = Graphics.FromImage(bmGrid);

								DisplayGridPolyar();  // Рисуем сетку полярной системы координат
								this.Invoke(new Action(() =>
							{
								pictureBox4.Image = bmGrid;
							}));


								//GenericCoordinats();

							}


						}

					}
					catch
					{
						this.Invoke(new Action(() =>
						{



							button2.Enabled = false;

							bmFunction = null; // Удаляем нашу функцию
							checkBox1.Checked = false;
							pictureBox4.Image = null;

							if (checkBox3.Checked == true) // с сеткой
							{
								BlockUiElements();

								//pictureBox4.BorderStyle = BorderStyle.None;

								bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								gr = Graphics.FromImage(bmGrid);

								DisplayGridPolyar();  // Рисуем сетку полярной системы координат

								pictureBox4.Image = bmGrid;
								OpenUiElements();

							}
							else if (checkBox4.Checked == true)
							{
								bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								gr = Graphics.FromImage(bmGrid);
								pictureBox4.Image = bmGrid;
							}
						}));
					}
					this.Invoke(new Action(() =>
					{
						OpenUiElements();
					}));
				}
			});

		}

		//Отрисовка сетки при запуски приложения (декартова система координат)	
		public void DisplayGridStartForm()
		{
			bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
			gr = Graphics.FromImage(bmGrid);

			for (int x = (480 / SizeGrid); x < 481; x += (480 / SizeGrid))
			{
				Start.X = 0;
				Start.Y = x;

				End.X = pictureBox4.Width - 1;
				End.Y = x;
				//горизонталь
				curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
				curcle.LineDisplay();

				Start.X = x;
				Start.Y = 0;

				End.X = x;
				End.Y = pictureBox4.Height - 1;
				//вертикаль
				curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
				curcle.LineDisplay();
			}
			pictureBox4.Image = bmGrid;
		}

		//Отрисовка сетки (Полярная система координат)	 
		public void DisplayGridPolyar()
		{

			int Radius = ((pictureBox4.Height - 1) / 2);
			int shag = Radius / (int)SizeAxis * 2;
			pn = new Pen(Color.FromArgb(32, 34, 37), 1);
			try
			{

				for (int j = 1; j <= Radius * 2 + 1; j += shag)
				{
					rect = new Rectangle(((pictureBox4.Height / 2) - (j / 2)), ((pictureBox4.Width / 2) - (j / 2)), j, j);

					for (int l = 0; l <= 360; l += 1)
						gr.DrawArc(pn, rect, l, l + 1);

				}
			}
			catch
			{

			}

			//Рисуем центральную ось 
			Start.X = 0;
			Start.Y = pictureBox4.Height / 2;

			End.X = pictureBox4.Width;
			End.Y = pictureBox4.Height / 2;

			curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
			curcle.LineDisplay();

			Start.X = pictureBox4.Width / 2;
			Start.Y = 0;

			End.X = pictureBox4.Width / 2;
			End.Y = pictureBox4.Height;

			curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
			curcle.LineDisplay();

			// Сетка для полярной оси координат
			//double fi = (45 * Math.PI) / 180;
			if (SizeGrid >= 2)
			{
				float fiShag = (90 / ((float)(SizeGrid) / 2f));

				for (double i = fiShag; i <= 180; i += fiShag)
				{

					double alfa1 = ((360 - i) * Math.PI) / 180;
					StartGridPolyar.X = Radius - (float)(Radius * Math.Cos(alfa1));
					StartGridPolyar.Y = Radius - (float)(Radius * Math.Sin(alfa1));
					double alfa2 = ((360 - (180 + i)) * Math.PI) / 180;
					EndGridPolyar.X = Radius - (float)(Radius * Math.Cos(alfa2));
					EndGridPolyar.Y = Radius - (float)(Radius * Math.Sin(alfa2));

					curcle = new Curcle(StartGridPolyar, EndGridPolyar, Color.FromArgb(32, 34, 37), gr);
					curcle.LineDisplayF();
				}
			}

		}

		//Расчёт кадров с учётом сетки 	для декартов 
		public void DisplayCadrInGrig()
		{
			gr = Graphics.FromImage(bmFunctionCadr);
			try
			{

				//Строим сетку
				for (int x = (480 / SizeGrid); x < 481; x += (480 / SizeGrid))
				{
					Start.X = 0;
					Start.Y = x;

					End.X = pictureBox4.Width - 1;
					End.Y = x;

					curcle = new Curcle(Start, End, Color.FromArgb(62, 62, 66), gr);
					curcle.LineDisplay();

					Start.X = x;
					Start.Y = 0;

					End.X = x;
					End.Y = pictureBox4.Height - 1;

					curcle = new Curcle(Start, End, Color.FromArgb(62, 62, 66), gr);
					curcle.LineDisplay();
				}
			}
			catch
			{

			}
		}

		//Острисовка сетки полярной системы
		public void DisplayCadrInGrigPolyar()
		{
			gr = Graphics.FromImage(bmFunctionCadr);
			int Radius = ((pictureBox4.Height - 1) / 2);
			int shag = Radius / (int)SizeAxis * 2;
			pn = new Pen(Color.FromArgb(62, 62, 66), 1);
			try
			{

				for (int j = 1; j <= Radius * 2 + 1; j += shag)
				{
					rect = new Rectangle(((pictureBox4.Height / 2) - (j / 2)), ((pictureBox4.Width / 2) - (j / 2)), j, j);

					for (int l = 0; l <= 360; l += 1)
						gr.DrawArc(pn, rect, l, l + 1);

				}

			}
			catch { }
			//Рисуем центральную ось 
			Start.X = 0;
			Start.Y = pictureBox4.Height / 2;

			End.X = pictureBox4.Width;
			End.Y = pictureBox4.Height / 2;

			curcle = new Curcle(Start, End, Color.FromArgb(62, 62, 66), gr);
			curcle.LineDisplay();

			Start.X = pictureBox4.Width / 2;
			Start.Y = 0;

			End.X = pictureBox4.Width / 2;
			End.Y = pictureBox4.Height;

			curcle = new Curcle(Start, End, Color.FromArgb(62, 62, 66), gr);
			curcle.LineDisplay();

			// Сетка для полярной оси координат
			//double fi = (45 * Math.PI) / 180;
			if (SizeGrid > 2)
			{
				float fiShag = (90 / ((float)(SizeGrid) / 2f));

				for (double i = fiShag; i <= 180; i += fiShag)
				{

					double alfa1 = ((360 - i) * Math.PI) / 180;
					StartGridPolyar.X = Radius - (float)(Radius * Math.Cos(alfa1));
					StartGridPolyar.Y = Radius - (float)(Radius * Math.Sin(alfa1));
					double alfa2 = ((360 - (180 + i)) * Math.PI) / 180;
					EndGridPolyar.X = Radius - (float)(Radius * Math.Cos(alfa2));
					EndGridPolyar.Y = Radius - (float)(Radius * Math.Sin(alfa2));

					curcle = new Curcle(StartGridPolyar, EndGridPolyar, Color.FromArgb(62, 62, 66), gr);
					curcle.LineDisplayF();
				}
			}

		}


		// Генирация Осей координат	 
		public void GenericCoordinats()
		{
			pictureBox1.Controls.Clear();
			int PointYLabel = 0;
			int ShagPointYLabel = (480 / (((int)SizeAxis * 2)));
			for (int i = 0; i < (SizeAxis * 2) + 1; i++)
			{

				Label label = new Label();
				label.AutoSize = true;
				label.ForeColor = System.Drawing.Color.FromArgb(54, 57, 63);
				label.BackColor = System.Drawing.Color.FromArgb(187, 188, 188);
				label.Location = new System.Drawing.Point(3, PointYLabel);
				label.Name = i.ToString();
				label.Size = new System.Drawing.Size(7, 7);
				label.TabIndex = 103;
				label.Text = (SizeAxis - i).ToString();
				pictureBox1.Controls.Add(label);
				PointYLabel += ShagPointYLabel;
			}

			bmCoordinat = new Bitmap(pictureBox1.Height, pictureBox1.Width);
			gr = Graphics.FromImage(bmCoordinat);
			try
			{

				for (float x = 5.501f; x < 480 + (480 / (SizeAxis * 2)); x += (480 / ((float)SizeAxis * 2)))
				{
					StartCoordinat.X = 25;
					StartCoordinat.Y = x;

					EndCoordinat.X = pictureBox4.Width - 1;
					EndCoordinat.Y = x;

					curcle = new Curcle(StartCoordinat, EndCoordinat, Color.FromArgb(32, 34, 37), gr);
					curcle.LineDisplayF();

				}
			}
			catch { }

			pictureBox1.Image = bmCoordinat;

		}

		// Увеличение маштаба оси координат
		async private void SizeAxisPlusBn_Click( object sender, EventArgs e )
		{
			await Task.Run(() =>
			{
				if (IndexCoordinat == 8)
				{
					IndexCoordinat = 8;
					//GenericCoordinats();//Перерисовка оси координта
					//pictureBox4.Image = null;
					////Пересчитываем функцию
					//DisplayFunction();
					//pictureBox4.Image = bmFunction;
					SizeAxis = NumbersGrig[IndexCoordinat];
					this.Invoke(new Action(() =>
					{
						GenericCoordinats();
					}));

				}
				else
				{
					IndexCoordinat++;
					SizeAxis = NumbersGrig[IndexCoordinat];



					if (checkBox1.Checked == true)   // декатова
					{
						try
						{
							this.Invoke(new Action(() =>
							{
								BlockUiElements();
							}));

							//Пересчитываем функцию
							if (bmFunction != null)
							{


								this.Invoke(new Action(() =>
								{
									GenericCoordinats();//Перерисовка оси координат 
								}));


								DisplayFunction(); //Пересчитываем функцию

								if (checkBox3.Checked == true)
								{
									if (checkBox5.Checked == true)
									{
										gr = Graphics.FromImage(bmFunction);
										for (int x = (480 / SizeGrid); x < 481; x += (480 / SizeGrid))
										{
											Start.X = 0;
											Start.Y = x;

											End.X = pictureBox4.Width - 1;
											End.Y = x;

											curcle = new Curcle(Start, End, Color.FromArgb(62, 62, 66), gr);
											curcle.LineDisplay();

											Start.X = x;
											Start.Y = 0;

											End.X = x;
											End.Y = pictureBox4.Height - 1;

											curcle = new Curcle(Start, End, Color.FromArgb(62, 62, 66), gr);
											curcle.LineDisplay();
										}
										if (checkBox7.Checked == true)
										{
											MarchingSquares(); // Ставим линии уровня
										}
										else if (checkBox7.Checked == false)
										{
											MarchingSquaresOriginal(); // Ставим линии уровня
										}
										this.Invoke(new Action(() =>
										{
											pictureBox4.Image = null;

											pictureBox4.Image = bmFunction;
										}));
									}
									else
									{

										gr = Graphics.FromImage(bmFunction);

										for (int x = (480 / SizeGrid); x < 481; x += (480 / SizeGrid))
										{
											Start.X = 0;
											Start.Y = x;

											End.X = pictureBox4.Width - 1;
											End.Y = x;

											curcle = new Curcle(Start, End, Color.FromArgb(62, 62, 66), gr);
											curcle.LineDisplay();

											Start.X = x;
											Start.Y = 0;

											End.X = x;
											End.Y = pictureBox4.Height - 1;

											curcle = new Curcle(Start, End, Color.FromArgb(62, 62, 66), gr);
											curcle.LineDisplay();
										}
										this.Invoke(new Action(() =>
										{
											pictureBox4.Image = null;

											pictureBox4.Image = bmFunction;
										}));
									}


								}
								else
								{
									if (checkBox5.Checked == true)
									{
										gr = Graphics.FromImage(bmFunction);

										if (checkBox7.Checked == true)
										{
											MarchingSquares(); // Ставим линии уровня
										}
										else if (checkBox7.Checked == false)
										{
											MarchingSquaresOriginal(); // Ставим линии уровня
										}
										this.Invoke(new Action(() =>
										{
											pictureBox4.Image = null;
											pictureBox4.Image = bmFunction;

										}));

									}
									else
									{

										this.Invoke(new Action(() =>
										{
											pictureBox4.Image = null;
											pictureBox4.Image = bmFunction;

										}));
									}


								}


							}
							else
							{
								this.Invoke(new Action(() =>
								{
									//BlockUiElements();
									GenericCoordinats();//Перерисовка оси координат
														//OpenUiElements();

								}));

							}
						}
						catch
						{
							this.Invoke(new Action(() =>
							{

								button2.Enabled = false;


								bmFunction = null; // Удаляем нашу функцию
								pictureBox4.Image = null;
								checkBox2.Checked = false;

								if (checkBox3.Checked == true)
								{
									DisplayGridStartForm(); // Отрисовка сетки при декартовой систему координат
								}
								else if (checkBox4.Checked == true)
								{
									bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
									gr = Graphics.FromImage(bmGrid);
									pictureBox4.Image = bmGrid;
								}
							}));
						}

						this.Invoke(new Action(() =>
						{
							OpenUiElements();

						}));
					}
					else //полярная
					{
						try
						{
							this.Invoke(new Action(() =>
							{
								BlockUiElements();

								//GenericCoordinats(); // Ставим ось координат

							}));

							if (bmFunction != null)
							{
								this.Invoke(new Action(() =>
								{
									//BlockUiElements();

									GenericCoordinats(); // Ставим ось координат

								}));


								//Пересчитываем функцию
								DisplayFunctionPolyar();    // пересчитали функцию в битмап Function

								if (checkBox3.Checked == true)
								{
									if (checkBox5.Checked == true)
									{
										gr = Graphics.FromImage(bmFunction);
										//сетка
										DisplayGridPolyar();

										if (checkBox7.Checked == true)
										{
											MarchingSquares(); // Ставим линии уровня
										}
										else if (checkBox7.Checked == false)
										{
											MarchingSquaresOriginal(); // Ставим линии уровня
										}

										this.Invoke(new Action(() =>
										{
											pictureBox4.Image = null;
											pictureBox4.Image = bmFunction;

										}));
									}
									else
									{

										gr = Graphics.FromImage(bmFunction);
										//сетка
										DisplayGridPolyar();
										this.Invoke(new Action(() =>
										{
											pictureBox4.Image = null;
											pictureBox4.Image = bmFunction;

										}));
									}

								}
								else if (checkBox4.Checked == true)
								{
									//bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
									//gr = Graphics.FromImage(bmGrid);
									if (checkBox5.Checked == true)
									{
										gr = Graphics.FromImage(bmFunction);

										if (checkBox7.Checked == true)
										{
											MarchingSquares(); // Ставим линии уровня
										}
										else if (checkBox7.Checked == false)
										{
											MarchingSquaresOriginal(); // Ставим линии уровня
										}

										this.Invoke(new Action(() =>
										{

											GenericCoordinats();
											pictureBox4.Image = bmFunction;
											//OpenUiElements();

										}));
									}
									else
									{

										this.Invoke(new Action(() =>
										{

											GenericCoordinats();
											pictureBox4.Image = bmFunction;
											//OpenUiElements();

										}));
									}
								}

							}
							else
							{
								//this.Invoke(new Action(() =>
								//{
								//	BlockUiElements();

								//}));

								if (checkBox3.Checked == true)    // с сеткой
								{
									bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
									gr = Graphics.FromImage(bmGrid);

									DisplayGridPolyar();  // Рисуем сетку полярной системы координат
									this.Invoke(new Action(() =>
								{
									pictureBox4.Image = bmGrid;

									GenericCoordinats();

								}));


								}
								else if (checkBox4.Checked == true)
								{
									this.Invoke(new Action(() =>
									{
										pictureBox4.Image = null;
										GenericCoordinats();

									}));


								}


							}
						}
						catch
						{
							this.Invoke(new Action(() =>
							{



								button2.Enabled = false;

								bmFunction = null; // Удаляем нашу функцию
								checkBox1.Checked = false;
								pictureBox4.Image = null;

								if (checkBox3.Checked == true) // с сеткой
								{
									BlockUiElements();

									//pictureBox4.BorderStyle = BorderStyle.None;

									bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
									gr = Graphics.FromImage(bmGrid);

									DisplayGridPolyar();  // Рисуем сетку полярной системы координат

									pictureBox4.Image = bmGrid;
									OpenUiElements();

								}
								else if (checkBox4.Checked == true)
								{
									bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
									gr = Graphics.FromImage(bmGrid);
									pictureBox4.Image = bmGrid;
								}
							}));
						}

						this.Invoke(new Action(() =>
						{
							OpenUiElements();

						}));
					}
				}
			});
		}

		//Уменьшение маштаба оси координат
		async private void SizeAxisMinusBn_Click( object sender, EventArgs e )
		{
			await Task.Run(() =>
			{
				if (IndexCoordinat == 0)
				{
					IndexCoordinat = 0;
					SizeAxis = NumbersGrig[IndexCoordinat];
				}
				else
				{
					IndexCoordinat--;
					SizeAxis = NumbersGrig[IndexCoordinat];

					if (checkBox1.Checked == true)  // декартова
					{
						try
						{
							this.Invoke(new Action(() =>
							{
								BlockUiElements();
								//GenericCoordinats(); // Ставим ось координат

							}));

							if (bmFunction != null)
							{
								this.Invoke(new Action(() =>
								{
									//BlockUiElements();
									GenericCoordinats(); // Ставим ось координат

								}));


								//Пересчитываем функцию
								DisplayFunction();    //битмап Function

								if (checkBox3.Checked == true)
								{
									if (checkBox5.Checked == true)
									{
										gr = Graphics.FromImage(bmFunction);
										for (int x = (480 / SizeGrid); x < 481; x += (480 / SizeGrid))
										{
											Start.X = 0;
											Start.Y = x;

											End.X = pictureBox4.Width - 1;
											End.Y = x;

											curcle = new Curcle(Start, End, Color.FromArgb(62, 62, 66), gr);
											curcle.LineDisplay();

											Start.X = x;
											Start.Y = 0;

											End.X = x;
											End.Y = pictureBox4.Height - 1;

											curcle = new Curcle(Start, End, Color.FromArgb(62, 62, 66), gr);
											curcle.LineDisplay();
										}
										if (checkBox7.Checked == true)
										{
											MarchingSquares(); // Ставим линии уровня
										}
										else if (checkBox7.Checked == false)
										{
											MarchingSquaresOriginal(); // Ставим линии уровня
										}
										this.Invoke(new Action(() =>
										{
											pictureBox4.Image = null;
											pictureBox4.Image = bmFunction;

										}));
									}
									else
									{

										gr = Graphics.FromImage(bmFunction);

										for (int x = (480 / SizeGrid); x < 481; x += (480 / SizeGrid))
										{
											Start.X = 0;
											Start.Y = x;

											End.X = pictureBox4.Width - 1;
											End.Y = x;

											curcle = new Curcle(Start, End, Color.FromArgb(62, 62, 66), gr);
											curcle.LineDisplay();

											Start.X = x;
											Start.Y = 0;

											End.X = x;
											End.Y = pictureBox4.Height - 1;

											curcle = new Curcle(Start, End, Color.FromArgb(62, 62, 66), gr);
											curcle.LineDisplay();
										}
										this.Invoke(new Action(() =>
										{
											pictureBox4.Image = null;
											pictureBox4.Image = bmFunction;

										}));
									}



								}
								else
								{
									if (checkBox5.Checked == true)
									{

										if (checkBox7.Checked == true)
										{
											MarchingSquares(); // Ставим линии уровня
										}
										else if (checkBox7.Checked == false)
										{
											MarchingSquaresOriginal(); // Ставим линии уровня
										}
										this.Invoke(new Action(() =>
										{
											pictureBox4.Image = null;
											pictureBox4.Image = bmFunction;

										}));
									}
									else
									{
										this.Invoke(new Action(() =>
										{
											pictureBox4.Image = null;
											pictureBox4.Image = bmFunction;

										}));
									}


								}


							}
							else
							{
								this.Invoke(new Action(() =>
								{
									//BlockUiElements();
									GenericCoordinats();//Перерисовка оси координат
														//OpenUiElements();

								}));

							}
						}
						catch
						{
							this.Invoke(new Action(() =>
							{

								button2.Enabled = false;


								bmFunction = null; // Удаляем нашу функцию
								pictureBox4.Image = null;
								checkBox2.Checked = false;

								if (checkBox3.Checked == true)
								{
									DisplayGridStartForm(); // Отрисовка сетки при декартовой систему координат
								}
								else if (checkBox4.Checked == true)
								{
									bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
									gr = Graphics.FromImage(bmGrid);
									pictureBox4.Image = bmGrid;
								}
							}));
						}

						this.Invoke(new Action(() =>
						{
							OpenUiElements();

						}));
					}
					else if (checkBox2.Checked == true) // полярная система 
					{
						try
						{
							this.Invoke(new Action(() =>
							{
								BlockUiElements();

								//GenericCoordinats(); // Ставим ось координат

							}));
							if (bmFunction != null)
							{
								this.Invoke(new Action(() =>
								{
									//BlockUiElements();

									GenericCoordinats(); // Ставим ось координат

								}));


								//Пересчитываем функцию
								DisplayFunctionPolyar();    // пересчитали функцию в битмап Function

								if (checkBox3.Checked == true)
								{
									if (checkBox5.Checked == true)
									{
										gr = Graphics.FromImage(bmFunction);

										//сетка
										DisplayGridPolyar();
										if (checkBox7.Checked == true)
										{
											MarchingSquares(); // Ставим линии уровня
										}
										else if (checkBox7.Checked == false)
										{
											MarchingSquaresOriginal(); // Ставим линии уровня
										}
										this.Invoke(new Action(() =>
										{
											pictureBox4.Image = null;
											pictureBox4.Image = bmFunction;

										}));
									}
									else
									{
										gr = Graphics.FromImage(bmFunction);
										//сетка
										DisplayGridPolyar();
										this.Invoke(new Action(() =>
										{
											pictureBox4.Image = null;
											pictureBox4.Image = bmFunction;

										}));
									}


								}
								else if (checkBox4.Checked == true)
								{
									if (checkBox5.Checked == true)
									{
										gr = Graphics.FromImage(bmFunction);
										if (checkBox7.Checked == true)
										{
											MarchingSquares(); // Ставим линии уровня
										}
										else if (checkBox7.Checked == false)
										{
											MarchingSquaresOriginal(); // Ставим линии уровня
										}
										this.Invoke(new Action(() =>
										{
											pictureBox4.Image = null;
											pictureBox4.Image = bmFunction;

										}));

									}
									else
									{

										this.Invoke(new Action(() =>
										{
											pictureBox4.Image = null;
											pictureBox4.Image = bmFunction;

										}));
									}


								}


							}
							else
							{
								//this.Invoke(new Action(() =>
								//{
								//	BlockUiElements();

								//}));

								if (checkBox3.Checked == true)
								{
									bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
									gr = Graphics.FromImage(bmGrid);

									DisplayGridPolyar();  // Рисуем сетку полярной системы координат

									this.Invoke(new Action(() =>
								{
									GenericCoordinats();
									pictureBox4.Image = bmGrid;
								}));


								}
								else if (checkBox4.Checked == true)
								{
									this.Invoke(new Action(() =>
									{
										pictureBox4.Image = null;
										GenericCoordinats();
										//OpenUiElements();

									}));
								}
								//this.Invoke(new Action(() =>
								//{

								//	OpenUiElements();

								//}));

							}
						}
						catch
						{
							this.Invoke(new Action(() =>
							{



								button2.Enabled = false;

								bmFunction = null; // Удаляем нашу функцию
								checkBox1.Checked = false;
								pictureBox4.Image = null;

								if (checkBox3.Checked == true) // с сеткой
								{
									BlockUiElements();

									//pictureBox4.BorderStyle = BorderStyle.None;

									bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
									gr = Graphics.FromImage(bmGrid);

									DisplayGridPolyar();  // Рисуем сетку полярной системы координат

									pictureBox4.Image = bmGrid;
									OpenUiElements();

								}
								else if (checkBox4.Checked == true)
								{
									bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
									gr = Graphics.FromImage(bmGrid);
									pictureBox4.Image = bmGrid;
								}
							}));
						}

						this.Invoke(new Action(() =>
						{
							OpenUiElements();

						}));
					}
				}
			});
		}

		async public void Get()
		{
			for (double i = 0.0; i <= 0.98; i += 0.1)
			{
				this.Opacity = i;
				await Task.Delay(50);
			}
			this.Opacity = 0.99;
		}

		public void StopThreadBtn_Click( object sender, EventArgs e )
		{
			//MessageBox.Show("Произошло прерывание обработки кадров", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
			button2.Enabled = false;
			thread.Abort();
			bitmaps.Clear();
			OpenUiElements();
			StopThreadBtn.Enabled = false;
			label80.Text = "";
			progressBar1.Value = 0;
			F = true;
			k = 0;
		}


		async private void pictureBox5_Click( object sender, EventArgs e )
		{
			if (PicResize == 0)
			{
				PicResize = 1;

				pictureBox5.Enabled = false;
				pictureBox2.Image = null;
				for (int i = 544; i <= 870; i += 32)
				{
					ClientSize = new Size(i, 536);
					await Task.Delay(1);
				}
				pictureBox5.Enabled = true;
				pictureBox2.Location = new System.Drawing.Point(838, 5);
				pictureBox2.Image = global::Интерполяция_квадрата.Properties.Resources.Gnome_Window_Close_32__1_;
				pictureBox5.Image = global::Интерполяция_квадрата.Properties.Resources.monotone_arrow_left_small;

			}
			else
			{
				PicResize = 0;

				pictureBox5.Enabled = false;
				pictureBox2.Image = null;
				for (int i = 871; i >= 520; i -= 30)
				{

					ClientSize = new Size(i, 536);
					await Task.Delay(1);
				}
				pictureBox5.Enabled = true;
				pictureBox2.Location = new System.Drawing.Point(519, 5);
				pictureBox2.Image = global::Интерполяция_квадрата.Properties.Resources.Gnome_Window_Close_32__1_;
				pictureBox5.Image = global::Интерполяция_квадрата.Properties.Resources.icons8_меню_2_48;
			}

		}

		private void panel4_Paint_2( object sender, PaintEventArgs e )
		{

		}

		//551; 562
		PointF One = new PointF();
		PointF Two = new PointF();
		Curcle CurcleLine;
		public bool[,] Mask = new bool[2, 2];

		public void MarchingSquaresOriginal()
		{
			double radius = ((pictureBox4.Height - 1) / 2);
			double XX = 0;
			double YY = 0;
			double Distans = 0;

			double MarshalLen = Max / 14;
			for (double k = 0; k <= Max; k += MarshalLen)
			{

				for (int i = 0; i <= 480 - (480 / SizeGrid); i += 480 / SizeGrid)
				{
					for (int j = 0; j <= 480 - (480 / SizeGrid); j += 480 / SizeGrid)
					{
						//координаты углов квадрата
						int LeftTopY = i;
						int LeftTopX = j;
						int RightTopY = i;
						int RightTopX = j + (480 / SizeGrid);
						int LeftBottomY = i + (480 / SizeGrid);
						int LeftBottomX = j;
						int RightBottomY = i + (480 / SizeGrid);
						int RightBottomX = j + (480 / SizeGrid);

						if (checkBox2.Checked == true)
						{
							XX = (radius * -1) + j;
							YY = radius - i;
							Distans = Math.Sqrt(XX * XX + YY * YY);
							if (Distans <= radius)
							{
								MarchigSquaresGraphicsOriginal(LeftTopY, LeftTopX, RightTopY,
					RightTopX, LeftBottomY, LeftBottomX, RightBottomY, RightBottomX, k);
							}

						}
						else if(checkBox1.Checked == true)
						{
							MarchigSquaresGraphicsOriginal(LeftTopY, LeftTopX, RightTopY,
				   RightTopX, LeftBottomY, LeftBottomX, RightBottomY, RightBottomX, k);
						}




					}

				}
			}
		}

		//Без интерполяции
		public void MarchigSquaresGraphicsOriginal( int LeftTopY, int LeftTopX, int RightTopY,
			int RightTopX, int LeftBottomY, int LeftBottomX, int RightBottomY, int RightBottomX, double MarshalLen )
		{


			double Number = MarshalLen;

			double K1 = (pixsel[LeftTopY, LeftTopX]);
			double K2 = (pixsel[RightTopY, RightTopX]);
			double K3 = (pixsel[RightBottomY, RightBottomX]);
			double K4 = (pixsel[LeftBottomY, LeftBottomX]);

			if (K1 > Number) Mask[0, 0] = true;
			else Mask[0, 0] = false;

			if (K2 > Number) Mask[0, 1] = true;
			else Mask[0, 1] = false;

			if (K3 > Number) Mask[1, 1] = true;
			else Mask[1, 1] = false;

			if (K4 > Number) Mask[1, 0] = true;
			else Mask[1, 0] = false;



			if ((Mask[0, 0] == false) && (Mask[0, 1] == false) && (Mask[1, 1] == false) && (Mask[1, 0] == true))    //1
			{
				One.X = (float)LeftBottomX;
				One.Y = (LeftTopY + LeftBottomY) / 2;

				Two.Y = (float)LeftBottomY;
				Two.X = (LeftBottomX + RightBottomX) / 2;
				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}
			else if ((Mask[0, 0] == false) && (Mask[0, 1] == false) && (Mask[1, 1] == true) && (Mask[1, 0] == false))//2
			{
				One.X = (float)RightBottomX;
				One.Y = (RightBottomY + RightTopY) / 2;

				Two.Y = (float)LeftBottomY;
				Two.X = (LeftBottomX + RightBottomX) / 2;
				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}
			else if ((Mask[0, 0] == false) && (Mask[0, 1] == false) && (Mask[1, 1] == true) && (Mask[1, 0] == true)) //3
			{
				One.X = (float)LeftBottomX;
				One.Y = (LeftTopY + LeftBottomY) / 2;

				Two.X = (float)RightBottomX;
				Two.Y = (RightBottomY + RightTopY) / 2;
				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}
			else if ((Mask[0, 0] == false) && (Mask[0, 1] == true) && (Mask[1, 1] == false) && (Mask[1, 0] == false)) //4
			{
				One.Y = (float)RightTopY;
				One.X = (LeftTopX + RightTopX) / 2;

				Two.X = (float)RightBottomX;
				Two.Y = (RightBottomY + RightTopY) / 2;
				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}
			else if ((Mask[0, 0] == false) && (Mask[0, 1] == true) && (Mask[1, 1] == false) && (Mask[1, 0] == true)) //5
			{
				One.Y = (float)LeftTopY;
				One.X = (LeftTopX + RightTopX) / 2;
				Two.X = (float)LeftBottomX;
				Two.Y = (LeftTopY + LeftBottomY) / 2;

				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();

				One.X = (float)RightBottomX;
				One.Y = (RightBottomY + RightTopY) / 2;

				Two.Y = (float)LeftBottomY;
				Two.X = (LeftBottomX + RightBottomX) / 2;

				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}
			else if ((Mask[0, 0] == false) && (Mask[0, 1] == true) && (Mask[1, 1] == true) && (Mask[1, 0] == false)) //6
			{
				One.Y = (float)LeftTopY;
				One.X = (LeftTopX + RightTopX) / 2;

				Two.Y = (float)LeftBottomY;
				Two.X = (LeftBottomX + RightBottomX) / 2;

				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}
			else if ((Mask[0, 0] == false) && (Mask[0, 1] == true) && (Mask[1, 1] == true) && (Mask[1, 0] == true)) //7
			{
				One.Y = (float)LeftTopY;
				One.X = (LeftTopX + RightTopX) / 2;

				Two.X = (float)LeftBottomX;
				Two.Y = (LeftTopY + LeftBottomY) / 2;

				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}
			else if ((Mask[0, 0] == true) && (Mask[0, 1] == false) && (Mask[1, 1] == false) && (Mask[1, 0] == false)) //8
			{
				One.Y = (float)LeftTopY;
				One.X = (LeftTopX + RightTopX) / 2;

				Two.X = (float)LeftBottomX;
				Two.Y = (LeftTopY + LeftBottomY) / 2;

				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}
			else if ((Mask[0, 0] == true) && (Mask[0, 1] == false) && (Mask[1, 1] == false) && (Mask[1, 0] == true)) //9
			{
				One.Y = (float)LeftTopY;
				One.X = (LeftTopX + RightTopX) / 2;


				Two.Y = (float)LeftBottomY;
				Two.X = (LeftBottomX + RightBottomX) / 2;
				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}
			else if ((Mask[0, 0] == true) && (Mask[0, 1] == false) && (Mask[1, 1] == true) && (Mask[1, 0] == false)) //10
			{
				One.Y = (float)LeftTopY;
				One.X = (LeftTopX + RightTopX) / 2;
				Two.X = (float)RightBottomX;
				Two.Y = (RightBottomY + RightTopY) / 2;
				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();

				One.X = (float)LeftBottomX;
				One.Y = (LeftTopY + LeftBottomY) / 2;

				Two.Y = (float)LeftBottomY;
				Two.X = (LeftBottomX + RightBottomX) / 2;

				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}
			else if ((Mask[0, 0] == true) && (Mask[0, 1] == false) && (Mask[1, 1] == true) && (Mask[1, 0] == true)) //11
			{
				One.Y = (float)LeftTopY;
				One.X = (LeftTopX + RightTopX) / 2;

				Two.X = (float)RightBottomX;
				Two.Y = (RightBottomY + RightTopY) / 2;

				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}
			else if ((Mask[0, 0] == true) && (Mask[0, 1] == true) && (Mask[1, 1] == false) && (Mask[1, 0] == false)) //12
			{

				One.X = (float)LeftBottomX;
				One.Y = (LeftTopY + LeftBottomY) / 2;

				Two.X = (float)RightBottomX;
				Two.Y = (RightBottomY + RightTopY) / 2;
				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}
			else if ((Mask[0, 0] == true) && (Mask[0, 1] == true) && (Mask[1, 1] == false) && (Mask[1, 0] == true)) //13
			{
				One.X = (float)RightBottomX;
				One.Y = (RightBottomY + RightTopY) / 2;

				Two.Y = (float)RightBottomY;
				Two.X = (LeftBottomX + RightBottomX) / 2;
				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}
			else if ((Mask[0, 0] == true) && (Mask[0, 1] == true) && (Mask[1, 1] == true) && (Mask[1, 0] == false)) //14
			{
				One.X = (float)LeftBottomX;
				One.Y = (LeftTopY + LeftBottomY) / 2;

				Two.Y = (float)LeftBottomY;
				Two.X = (LeftBottomX + RightBottomX) / 2;
				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}

		}

		// Реализация алгоритма Marching squares
		public void MarchingSquares()
		{
			double radius = ((pictureBox4.Height - 1) / 2);
			double XX = 0;
			double YY = 0;
			double Distans = 0;
			double MarshalLen = Max / 14;
			for (double k = 0; k <= Max; k += MarshalLen)
			{

				for (int i = 0; i <= 480 - (480 / SizeGrid); i += 480 / SizeGrid)
				{
					for (int j = 0; j <= 480 - (480 / SizeGrid); j += 480 / SizeGrid)
					{
						//координаты углов квадрата
						int LeftTopY = i;
						int LeftTopX = j;
						int RightTopY = i;
						int RightTopX = j + (480 / SizeGrid);
						int LeftBottomY = i + (480 / SizeGrid);
						int LeftBottomX = j;
						int RightBottomY = i + (480 / SizeGrid);
						int RightBottomX = j + (480 / SizeGrid);

						if (checkBox2.Checked == true)
						{
							XX = (radius * -1) + j;
							YY = radius - i;
							Distans = Math.Sqrt(XX * XX + YY * YY);
							if (Distans <= radius)
							{
								MarchigSquaresGraphics(LeftTopY, LeftTopX, RightTopY,
				  RightTopX, LeftBottomY, LeftBottomX, RightBottomY, RightBottomX, k);
							}
						}
						else if(checkBox1.Checked == true)
						{
							MarchigSquaresGraphics(LeftTopY, LeftTopX, RightTopY,
				   RightTopX, LeftBottomY, LeftBottomX, RightBottomY, RightBottomX, k);
						}


					}

				}
			}
		}

		//линии уровня с интерполяцией
		public void MarchigSquaresGraphics( int LeftTopY, int LeftTopX, int RightTopY,
			int RightTopX, int LeftBottomY, int LeftBottomX, int RightBottomY, int RightBottomX, double MarshalLen )
		{
			//gr = Graphics.FromImage(bmFunction);

			double Number = MarshalLen;

			double K1 = (pixsel[LeftTopY, LeftTopX]);
			double K2 = (pixsel[RightTopY, RightTopX]);
			double K3 = (pixsel[RightBottomY, RightBottomX]);
			double K4 = (pixsel[LeftBottomY, LeftBottomX]);

			if (K1 > Number) Mask[0, 0] = true;
			else Mask[0, 0] = false;

			if (K2 > Number) Mask[0, 1] = true;
			else Mask[0, 1] = false;

			if (K3 > Number) Mask[1, 1] = true;
			else Mask[1, 1] = false;

			if (K4 > Number) Mask[1, 0] = true;
			else Mask[1, 0] = false;


			//С интерполяцией
			if ((Mask[0, 0] == false) && (Mask[0, 1] == false) && (Mask[1, 1] == false) && (Mask[1, 0] == true))    //1
			{
				One = LeftM(LeftTopY, LeftTopX, LeftBottomY, K1, K4, Number);
				Two = BottomM(LeftBottomY, LeftBottomX, RightBottomX, K4, K3, Number);
				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}
			else if ((Mask[0, 0] == false) && (Mask[0, 1] == false) && (Mask[1, 1] == true) && (Mask[1, 0] == false))//2
			{
				One = RightM(RightTopY, RightTopX, RightBottomY, K2, K3, Number);
				Two = BottomM(LeftBottomY, LeftBottomX, RightBottomX, K4, K3, Number);
				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}
			else if ((Mask[0, 0] == false) && (Mask[0, 1] == false) && (Mask[1, 1] == true) && (Mask[1, 0] == true)) //3
			{
				One = LeftM(LeftTopY, LeftTopX, LeftBottomY, K1, K4, Number);
				Two = RightM(RightTopY, RightTopX, RightBottomY, K2, K3, Number);
				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF(); ;
			}
			else if ((Mask[0, 0] == false) && (Mask[0, 1] == true) && (Mask[1, 1] == false) && (Mask[1, 0] == false)) //4
			{
				One = TopM(LeftTopY, LeftTopX, RightTopX, K1, K2, Number);
				Two = RightM(RightTopY, RightTopX, RightBottomY, K2, K3, Number);
				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}
			else if ((Mask[0, 0] == false) && (Mask[0, 1] == true) && (Mask[1, 1] == false) && (Mask[1, 0] == true)) //5
			{
				One = TopM(LeftTopY, LeftTopX, RightTopX, K1, K2, Number);
				Two = LeftM(LeftTopY, LeftTopX, LeftBottomY, K1, K4, Number);
				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
				////////////////////////////////////////////////////////////////
				One = RightM(RightTopY, RightTopX, RightBottomY, K2, K3, Number);
				Two = BottomM(LeftBottomY, LeftBottomX, RightBottomX, K4, K3, Number);
				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}
			else if ((Mask[0, 0] == false) && (Mask[0, 1] == true) && (Mask[1, 1] == true) && (Mask[1, 0] == false)) //6
			{
				One = TopM(LeftTopY, LeftTopX, RightTopX, K1, K2, Number);
				Two = BottomM(LeftBottomY, LeftBottomX, RightBottomX, K4, K3, Number);
				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}
			else if ((Mask[0, 0] == false) && (Mask[0, 1] == true) && (Mask[1, 1] == true) && (Mask[1, 0] == true)) //7
			{
				One = TopM(LeftTopY, LeftTopX, RightTopX, K1, K2, Number);
				Two = LeftM(LeftTopY, LeftTopX, LeftBottomY, K1, K4, Number);
				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}
			else if ((Mask[0, 0] == true) && (Mask[0, 1] == false) && (Mask[1, 1] == false) && (Mask[1, 0] == false)) //8
			{
				One = TopM(LeftTopY, LeftTopX, RightTopX, K1, K2, Number);
				Two = LeftM(LeftTopY, LeftTopX, LeftBottomY, K1, K4, Number);
				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}
			else if ((Mask[0, 0] == true) && (Mask[0, 1] == false) && (Mask[1, 1] == false) && (Mask[1, 0] == true)) //9
			{
				One = TopM(LeftTopY, LeftTopX, RightTopX, K1, K2, Number);
				Two = BottomM(LeftBottomY, LeftBottomX, RightBottomX, K4, K3, Number);
				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}
			else if ((Mask[0, 0] == true) && (Mask[0, 1] == false) && (Mask[1, 1] == true) && (Mask[1, 0] == false)) //10
			{
				One = TopM(LeftTopY, LeftTopX, RightTopX, K1, K2, Number);
				Two = RightM(RightTopY, RightTopX, RightBottomY, K2, K3, Number);
				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
				/////////////////////////////////////////////////////////////
				One = LeftM(LeftTopY, LeftTopX, LeftBottomY, K1, K4, Number);
				Two = BottomM(LeftBottomY, LeftBottomX, RightBottomX, K4, K3, Number);
				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}
			else if ((Mask[0, 0] == true) && (Mask[0, 1] == false) && (Mask[1, 1] == true) && (Mask[1, 0] == true)) //11
			{
				One = TopM(LeftTopY, LeftTopX, RightTopX, K1, K2, Number);
				Two = RightM(RightTopY, RightTopX, RightBottomY, K2, K3, Number);
				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}
			else if ((Mask[0, 0] == true) && (Mask[0, 1] == true) && (Mask[1, 1] == false) && (Mask[1, 0] == false)) //12
			{
				One = LeftM(LeftTopY, LeftTopX, LeftBottomY, K1, K4, Number);
				Two = RightM(RightTopY, RightTopX, RightBottomY, K2, K3, Number);
				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}
			else if ((Mask[0, 0] == true) && (Mask[0, 1] == true) && (Mask[1, 1] == false) && (Mask[1, 0] == true)) //13
			{
				One = RightM(RightTopY, RightTopX, RightBottomY, K2, K3, Number);
				Two = BottomM(LeftBottomY, LeftBottomX, RightBottomX, K4, K3, Number);
				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}
			else if ((Mask[0, 0] == true) && (Mask[0, 1] == true) && (Mask[1, 1] == true) && (Mask[1, 0] == false)) //14
			{
				One = LeftM(LeftTopY, LeftTopX, LeftBottomY, K1, K4, Number);
				Two = BottomM(LeftBottomY, LeftBottomX, RightBottomX, K4, K3, Number);
				CurcleLine = new Curcle(One, Two, Color.FromArgb(54, 57, 63), gr);
				CurcleLine.LineDisplayF();
			}

		}

		public PointF RightM( int RightTopY, int RightTopX, int RightBottomY, double B, double D, double Z )
		{
			PointF Q = new PointF();

			Q.X = (float)RightTopX;
			Q.Y = (float)RightTopY + ((float)(RightBottomY - RightTopY) * (float)((Z - B) / (D - B)));

			return Q;
		}

		public PointF BottomM( int LeftBottomY, int LeftBottomX, int RightBottomX, double C, double D, double Z )
		{
			PointF Q = new PointF();

			Q.Y = (float)LeftBottomY;
			Q.X = (float)LeftBottomX + ((float)(RightBottomX - LeftBottomX) * (float)((Z - C) / (D - C)));

			return Q;
		}

		public PointF LeftM( int LeftTopY, int LeftTopX, int LeftBottomY, double A, double C, double Z )
		{
			PointF Q = new PointF();

			Q.X = (float)LeftTopX;
			Q.Y = (float)LeftTopY + ((float)(LeftBottomY - LeftTopY) * (float)((Z - A) / (C - A)));

			return Q;
		}

		public PointF TopM( int LeftTopY, int LeftTopX, int RightTopX, double A, double B, double Z )
		{
			PointF Q = new PointF();

			Q.Y = (float)LeftTopY;
			Q.X = (float)LeftTopX + ((float)(RightTopX - LeftTopX) * (float)((Z - A) / (B - A)));

			return Q;
		}

		private void label69_Click( object sender, EventArgs e )
		{

		}

		async private void checkBox5_CheckedChanged( object sender, EventArgs e )
		{
			await Task.Run(() =>
			{
				if (checkBox5.Checked == true)// с линиями уровня
				{
					this.Invoke(new Action(() => { checkBox6.Checked = false; }));


					if ((checkBox1.Checked == true)) // декартова
					{
						this.Invoke(new Action(() => { BlockUiElements(); }));
						if (checkBox3.Checked == true)//Показать сетку
						{
							if (bmFunction != null)
							{
								gr = Graphics.FromImage(bmFunction);
							}
							else
							{
								bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								gr = Graphics.FromImage(bmGrid);
							}

							for (int x = (480 / SizeGrid); x < pictureBox4.Width; x += (480 / SizeGrid))
							{
								Start.X = 0;
								Start.Y = x;

								End.X = pictureBox4.Width - 1;
								End.Y = x;

								curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
								curcle.LineDisplay();

								Start.X = x;
								Start.Y = 0;

								End.X = x;
								End.Y = pictureBox4.Height - 1;

								curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
								curcle.LineDisplay();
							}
							if (bmFunction != null)
							{
								if (checkBox7.Checked == true)
								{
									MarchingSquares(); // Ставим линии уровня
								}
								else if (checkBox7.Checked == false)
								{
									MarchingSquaresOriginal(); // Ставим линии уровня
								}
							}

							if (bmFunction != null)
							{
								this.Invoke(new Action(() => { pictureBox4.Image = bmFunction; }));

							}
							else
							{
								this.Invoke(new Action(() => { pictureBox4.Image = bmGrid; }));

							}
						}
						else
						{
							if (bmFunction != null)
							{
								gr = Graphics.FromImage(bmFunction);

								if (checkBox7.Checked == true)
								{
									MarchingSquares(); // Ставим линии уровня
								}
								else if (checkBox7.Checked == false)
								{
									MarchingSquaresOriginal(); // Ставим линии уровня
								}
								this.Invoke(new Action(() => { pictureBox4.Image = bmFunction; }));
							}
							else
							{
								this.Invoke(new Action(() => { pictureBox4.Image = null; }));
							}
						}

						this.Invoke(new Action(() => { OpenUiElements(); }));


					} //декартова 
					else if ((checkBox2.Checked == true))
					{
						//this.Invoke(new Action(() => { OpenUiElements(); }));
						this.Invoke(new Action(() => { checkBox1.Checked = false; }));
						this.Invoke(new Action(() => { BlockUiElements(); }));
						//pictureBox4.BorderStyle = BorderStyle.None;
						if (checkBox3.Checked == true) //с сеткой
						{

							if (bmFunction != null)
							{
								//bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								//gr = Graphics.FromImage(bmGrid);
								//DisplayFunctionPolyar();
								gr = Graphics.FromImage(bmFunction);
								DisplayGridPolyar();
								// Рисуем сетку полярной системы координат

								if (checkBox7.Checked == true)
								{
									MarchingSquares(); // Ставим линии уровня
								}
								else if (checkBox7.Checked == false)
								{
									MarchingSquaresOriginal(); // Ставим линии уровня
								}

								//GenericCoordinats();
								this.Invoke(new Action(() => { pictureBox4.Image = bmFunction; }));

							}
							else
							{
								bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								gr = Graphics.FromImage(bmGrid);
								DisplayGridPolyar(); ;
								// Рисуем сетку полярной системы координат
								this.Invoke(new Action(() => { pictureBox4.Image = bmGrid; }));

								//GenericCoordinats();
							}
						}
						else
						{
							if (bmFunction != null)
							{
								//bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								//gr = Graphics.FromImage(bmGrid);
								//DisplayFunctionPolyar();
								gr = Graphics.FromImage(bmFunction);


								if (checkBox7.Checked == true)
								{
									MarchingSquares(); // Ставим линии уровня
								}
								else if (checkBox7.Checked == false)
								{
									MarchingSquaresOriginal(); // Ставим линии уровня
								}

								//GenericCoordinats();
								this.Invoke(new Action(() => { pictureBox4.Image = bmFunction; }));

							}

						}

						this.Invoke(new Action(() => { OpenUiElements(); }));

					}  // полярная

				}
				else
				{
					this.Invoke(new Action(() => { OpenUiElements(); }));
					if (checkBox6.Checked == true)
					{
						this.Invoke(new Action(() => { checkBox5.Checked = false; }));


					}
					else
					{
						this.Invoke(new Action(() => { checkBox5.Checked = true; }));

					}

				}
			});
		}

		async private void checkBox6_CheckedChanged( object sender, EventArgs e )
		{
			await Task.Run(() =>
			{

				if (checkBox6.Checked == true)
				{
					this.Invoke(new Action(() =>
					{
						checkBox5.Checked = false;
						BlockUiElements();

					}));


					if ((checkBox1.Checked == true))      // декартова
					{
						this.Invoke(new Action(() =>
						{
							BlockUiElements();

						}));

						if (checkBox3.Checked == true) //с сеткой
						{

							if (bmFunction != null)
							{
								DisplayFunction(); //Перерисовка функции

								gr = Graphics.FromImage(bmFunction);

								//Отрисовка сетки 
								for (int x = (480 / SizeGrid); x < pictureBox4.Width; x += (480 / SizeGrid))
								{
									Start.X = 0;
									Start.Y = x;

									End.X = pictureBox1.Width - 1;
									End.Y = x;

									curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
									curcle.LineDisplay();

									Start.X = x;
									Start.Y = 0;

									End.X = x;
									End.Y = pictureBox1.Height - 1;

									curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
									curcle.LineDisplay();
								}



								this.Invoke(new Action(() => { pictureBox4.Image = bmFunction; }));



							}
							else
							{

								bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								gr = Graphics.FromImage(bmGrid);

								//Отрисовка сетки 
								for (int x = (480 / SizeGrid); x < pictureBox4.Width; x += (480 / SizeGrid))
								{
									Start.X = 0;
									Start.Y = x;

									End.X = pictureBox1.Width - 1;
									End.Y = x;

									curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
									curcle.LineDisplay();

									Start.X = x;
									Start.Y = 0;

									End.X = x;
									End.Y = pictureBox1.Height - 1;

									curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
									curcle.LineDisplay();
								}

								this.Invoke(new Action(() => { pictureBox4.Image = null; }));
								this.Invoke(new Action(() => { pictureBox4.Image = bmGrid; }));

							}


						}
						else //без сетки
						{

							if (bmFunction != null)
							{
								//this.Invoke(new Action(() =>
								//{

								//	BlockUiElements();
								////gr = Graphics.FromImage(bmFunction);
								//   }));

								DisplayFunction();

								this.Invoke(new Action(() => { pictureBox4.Image = bmFunction; }));

							}
							else
							{
								this.Invoke(new Action(() => { pictureBox4.Image = null; }));
								//this.Invoke(new Action(() =>
								//{
								//	OpenUiElements();
								//}));
							}
						}


					}  //декатова
					else if (checkBox2.Checked == true)
					{
						this.Invoke(new Action(() =>
						{
							BlockUiElements();
						}));

						if (checkBox3.Checked == true)
						{
							if (bmFunction != null)
							{
								//bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								//gr = Graphics.FromImage(bmGrid);
								DisplayFunctionPolyar();
								gr = Graphics.FromImage(bmFunction);
								DisplayGridPolyar();
								// Рисуем сетку полярной системы координат


								//GenericCoordinats();
								this.Invoke(new Action(() => { pictureBox4.Image = bmFunction; }));

							}
							else
							{
								bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								gr = Graphics.FromImage(bmGrid);
								DisplayGridPolyar(); ;
								// Рисуем сетку полярной системы координат
								this.Invoke(new Action(() => { pictureBox4.Image = bmGrid; }));

								//GenericCoordinats();
							}
						}
						else
						{
							if (bmFunction != null)
							{
								//bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								//gr = Graphics.FromImage(bmGrid);
								DisplayFunctionPolyar();
								gr = Graphics.FromImage(bmFunction);

								// Рисуем сетку полярной системы координат


								//GenericCoordinats();
								this.Invoke(new Action(() => { pictureBox4.Image = bmFunction; }));

							}
							else
							{

								// Рисуем сетку полярной системы координат
								this.Invoke(new Action(() => { pictureBox4.Image = null; }));

								//GenericCoordinats();
							}
						}


					}   // полярная

					this.Invoke(new Action(() =>
				{
					OpenUiElements();

				}));

				}
				else
				{
					this.Invoke(new Action(() =>
					{
						OpenUiElements();

					}));

					if (checkBox5.Checked == true)
					{
						this.Invoke(new Action(() => { checkBox6.Checked = false; }));
						//this.Invoke(new Action(() => { OpenUiElements(); }));

					}
					else
					{
						this.Invoke(new Action(() => { checkBox6.Checked = true; }));
						//this.Invoke(new Action(() => { OpenUiElements(); }));
					}
				}

			});
		}

		//Со сглаживанием 
		async private void checkBox7_CheckedChanged( object sender, EventArgs e )
		{
			await Task.Run(() =>
			{
				if (checkBox7.Checked == true)
				{
					this.Invoke(new Action(() =>
					{
						checkBox8.Checked = false;


					}));

					if (checkBox1.Checked == true)
					{
						this.Invoke(new Action(() =>
						{

							BlockUiElements();

						}));
						// c сеткой
						if (checkBox3.Checked == true)
						{
							if (checkBox5.Checked == true)  // с линиями уровня
							{
								if (bmFunction != null)
								{
									this.Invoke(new Action(() =>
									{

										BlockUiElements();

									}));
									DisplayFunction(); //Перерисовка функции

									gr = Graphics.FromImage(bmFunction);

									//Отрисовка сетки 
									for (int x = (480 / SizeGrid); x < pictureBox4.Width; x += (480 / SizeGrid))
									{
										Start.X = 0;
										Start.Y = x;

										End.X = pictureBox1.Width - 1;
										End.Y = x;

										curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
										curcle.LineDisplay();

										Start.X = x;
										Start.Y = 0;

										End.X = x;
										End.Y = pictureBox1.Height - 1;

										curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
										curcle.LineDisplay();
									}

									MarchingSquares(); // Ставим линии уровня

									this.Invoke(new Action(() =>
								{
									pictureBox4.Image = (Image)(bmFunction);
								}));
								}
								else
								{
									//DisplayFunction(); //Перерисовка функции

									bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
									gr = Graphics.FromImage(bmGrid);

									//Отрисовка сетки 
									for (int x = (480 / SizeGrid); x < pictureBox4.Width; x += (480 / SizeGrid))
									{
										Start.X = 0;
										Start.Y = x;

										End.X = pictureBox1.Width - 1;
										End.Y = x;

										curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
										curcle.LineDisplay();

										Start.X = x;
										Start.Y = 0;

										End.X = x;
										End.Y = pictureBox1.Height - 1;

										curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
										curcle.LineDisplay();
									}

									this.Invoke(new Action(() => { pictureBox4.Image = bmGrid; }));
								}

							}
							else if (checkBox5.Checked == false)// без линий уровня
							{
								if (bmFunction != null)
								{
									this.Invoke(new Action(() =>
									{

										BlockUiElements();

									}));
									DisplayFunction(); //Перерисовка функции

									gr = Graphics.FromImage(bmFunction);

									//Отрисовка сетки 
									for (int x = (480 / SizeGrid); x < pictureBox4.Width; x += (480 / SizeGrid))
									{
										Start.X = 0;
										Start.Y = x;

										End.X = pictureBox1.Width - 1;
										End.Y = x;

										curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
										curcle.LineDisplay();

										Start.X = x;
										Start.Y = 0;

										End.X = x;
										End.Y = pictureBox1.Height - 1;

										curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
										curcle.LineDisplay();
									}

									this.Invoke(new Action(() =>
									{
										pictureBox4.Image = (Image)(bmFunction);
									}));
								}
								else
								{
									bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
									gr = Graphics.FromImage(bmGrid);


									//Отрисовка сетки 
									for (int x = (480 / SizeGrid); x < pictureBox4.Width; x += (480 / SizeGrid))
									{
										Start.X = 0;
										Start.Y = x;

										End.X = pictureBox1.Width - 1;
										End.Y = x;

										curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
										curcle.LineDisplay();

										Start.X = x;
										Start.Y = 0;

										End.X = x;
										End.Y = pictureBox1.Height - 1;

										curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
										curcle.LineDisplay();
									}
									// Рисуем сетку полярной системы координат
									this.Invoke(new Action(() => { pictureBox4.Image = bmGrid; }));
								}
							}


						}
						else // без сетки
						{
							if (bmFunction != null)
							{
								this.Invoke(new Action(() =>
								{

									BlockUiElements();

								}));

								if (checkBox5.Checked == true)  // с линиями уровня
								{
									DisplayFunction(); //Перерисовка функции


									MarchingSquares(); // Ставим линии уровня


									this.Invoke(new Action(() =>
								{
									pictureBox4.Image = (Image)(bmFunction);
								}));
								}
								else if (checkBox5.Checked == false)// без линий уровня
								{
									DisplayFunction(); //Перерисовка функции

									this.Invoke(new Action(() =>
								{
									pictureBox4.Image = (Image)(bmFunction);
								}));
								}
							}
							else
							{
								this.Invoke(new Action(() =>
								{
									pictureBox4.Image = null;
								}));

							}

						}


					}       //декартова
					else if (checkBox2.Checked == true)
					{
						this.Invoke(new Action(() =>
						{
							checkBox8.Checked = false;
							BlockUiElements();

						}));
						// c сеткой
						if (checkBox3.Checked == true)
						{
							if (bmFunction != null)
							{
								this.Invoke(new Action(() =>
								{

									BlockUiElements();

								}));

								if (checkBox5.Checked == true)  // с линиями уровня
								{
									DisplayFunctionPolyar(); //Перерисовка функции

									gr = Graphics.FromImage(bmFunction);

									//сетка для полярной системы координ
									DisplayGridPolyar();


									MarchingSquares(); // Ставим линии уровня


									this.Invoke(new Action(() =>
								{

									pictureBox4.Image = (Image)(bmFunction);
								}));
								}
								else if (checkBox5.Checked == false)// без линий уровня
								{
									DisplayFunctionPolyar(); //Перерисовка функции

									gr = Graphics.FromImage(bmFunction);

									//сетка для полярной системы координ
									DisplayGridPolyar();



									this.Invoke(new Action(() =>
									{

										pictureBox4.Image = (Image)(bmFunction);
									}));
								}
							}
							else
							{
								bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								gr = Graphics.FromImage(bmGrid);
								DisplayGridPolyar(); ;
								// Рисуем сетку полярной системы координат
								this.Invoke(new Action(() => { pictureBox4.Image = bmGrid; }));
							}

						}
						else //без сетки
						{
							if (bmFunction != null)
							{
								this.Invoke(new Action(() =>
								{

									BlockUiElements();

								}));
								if (checkBox5.Checked == true)  // с линиями уровня
								{
									DisplayFunctionPolyar();//Перерисовка функции

									gr = Graphics.FromImage(bmFunction);


									MarchingSquares(); // Ставим линии уровня


									this.Invoke(new Action(() =>
								{

									pictureBox4.Image = (Image)(bmFunction);
								}));
								}
								else if (checkBox5.Checked == false)// без линий уровня
								{
									DisplayFunctionPolyar();//Перерисовка функции
									this.Invoke(new Action(() =>
								{
									pictureBox4.Image = (Image)(bmFunction);
								}));

								}

							}
							else
							{
								this.Invoke(new Action(() =>
								{
									pictureBox4.Image = null;
								}));
							}

						}

					}

					this.Invoke(new Action(() =>
					{
						OpenUiElements();

					}));

				}
				else
				{
					this.Invoke(new Action(() =>
					{
						OpenUiElements();

					}));

					if (checkBox8.Checked == true)
					{
						this.Invoke(new Action(() => { checkBox7.Checked = false; }));
						//this.Invoke(new Action(() => { OpenUiElements(); }));

					}
					else
					{
						this.Invoke(new Action(() => { checkBox7.Checked = true; }));
						//this.Invoke(new Action(() => { OpenUiElements(); }));
					}
				}
			});
		}

		//без сглаживания
		async private void checkBox8_CheckedChanged( object sender, EventArgs e )
		{
			await Task.Run(() =>
			{
				if (checkBox8.Checked == true)
				{


					if (checkBox1.Checked == true)
					{
						this.Invoke(new Action(() =>
						{
							checkBox7.Checked = false;
							BlockUiElements();

						}));
						// c сеткой
						if (checkBox3.Checked == true)
						{
							if (checkBox5.Checked == true)  // с линиями уровня
							{
								if (bmFunction != null)
								{
									this.Invoke(new Action(() =>
									{
										checkBox7.Checked = false;
										BlockUiElements();

									}));
									DisplayFunction(); //Перерисовка функции

									gr = Graphics.FromImage(bmFunction);

									//Отрисовка сетки 
									for (int x = (480 / SizeGrid); x < pictureBox4.Width; x += (480 / SizeGrid))
									{
										Start.X = 0;
										Start.Y = x;

										End.X = pictureBox1.Width - 1;
										End.Y = x;

										curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
										curcle.LineDisplay();

										Start.X = x;
										Start.Y = 0;

										End.X = x;
										End.Y = pictureBox1.Height - 1;

										curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
										curcle.LineDisplay();
									}

									MarchingSquaresOriginal(); // Ставим линии уровня

									this.Invoke(new Action(() =>
								{
									pictureBox4.Image = (Image)(bmFunction);
								}));
								}
								else
								{
									//DisplayFunction(); //Перерисовка функции

									bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
									gr = Graphics.FromImage(bmGrid);

									//Отрисовка сетки 
									for (int x = (480 / SizeGrid); x < pictureBox4.Width; x += (480 / SizeGrid))
									{
										Start.X = 0;
										Start.Y = x;

										End.X = pictureBox1.Width - 1;
										End.Y = x;

										curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
										curcle.LineDisplay();

										Start.X = x;
										Start.Y = 0;

										End.X = x;
										End.Y = pictureBox1.Height - 1;

										curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
										curcle.LineDisplay();
									}

									this.Invoke(new Action(() => { pictureBox4.Image = bmGrid; }));
								}

							}
							else if (checkBox5.Checked == false)// без линий уровня
							{
								if (bmFunction != null)
								{
									this.Invoke(new Action(() =>
									{
										checkBox7.Checked = false;
										BlockUiElements();

									}));
									DisplayFunction(); //Перерисовка функции

									gr = Graphics.FromImage(bmFunction);

									//Отрисовка сетки 
									for (int x = (480 / SizeGrid); x < pictureBox4.Width; x += (480 / SizeGrid))
									{
										Start.X = 0;
										Start.Y = x;

										End.X = pictureBox1.Width - 1;
										End.Y = x;

										curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
										curcle.LineDisplay();

										Start.X = x;
										Start.Y = 0;

										End.X = x;
										End.Y = pictureBox1.Height - 1;

										curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
										curcle.LineDisplay();
									}

									this.Invoke(new Action(() =>
									{
										pictureBox4.Image = (Image)(bmFunction);
									}));
								}
								else
								{
									bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
									gr = Graphics.FromImage(bmGrid);


									//Отрисовка сетки 
									for (int x = (480 / SizeGrid); x < pictureBox4.Width; x += (480 / SizeGrid))
									{
										Start.X = 0;
										Start.Y = x;

										End.X = pictureBox1.Width - 1;
										End.Y = x;

										curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
										curcle.LineDisplay();

										Start.X = x;
										Start.Y = 0;

										End.X = x;
										End.Y = pictureBox1.Height - 1;

										curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
										curcle.LineDisplay();
									}
									// Рисуем сетку полярной системы координат
									this.Invoke(new Action(() => { pictureBox4.Image = bmGrid; }));
								}
							}


						}
						else // без сетки
						{
							if (bmFunction != null)
							{
								this.Invoke(new Action(() =>
								{
									checkBox7.Checked = false;
									BlockUiElements();

								}));

								if (checkBox5.Checked == true)  // с линиями уровня
								{
									DisplayFunction(); //Перерисовка функции


									MarchingSquaresOriginal(); // Ставим линии уровня


									this.Invoke(new Action(() =>
								{
									pictureBox4.Image = (Image)(bmFunction);
								}));
								}
								else if (checkBox5.Checked == false)// без линий уровня
								{
									DisplayFunction(); //Перерисовка функции

									this.Invoke(new Action(() =>
								{
									pictureBox4.Image = (Image)(bmFunction);
								}));
								}
							}
							else
							{
								this.Invoke(new Action(() =>
								{
									pictureBox4.Image = null;
								}));

							}

						}


					}       //декартова
					else if (checkBox2.Checked == true)
					{
						this.Invoke(new Action(() =>
						{
							checkBox7.Checked = false;
							BlockUiElements();

						}));
						// c сеткой
						if (checkBox3.Checked == true)
						{
							if (bmFunction != null)
							{
								this.Invoke(new Action(() =>
								{
									checkBox7.Checked = false;
									BlockUiElements();

								}));

								if (checkBox5.Checked == true)  // с линиями уровня
								{
									DisplayFunctionPolyar(); //Перерисовка функции

									gr = Graphics.FromImage(bmFunction);

									//сетка для полярной системы координ
									DisplayGridPolyar();


									MarchingSquaresOriginal(); // Ставим линии уровня


									this.Invoke(new Action(() =>
								{

									pictureBox4.Image = (Image)(bmFunction);
								}));
								}
								else if (checkBox5.Checked == false)// без линий уровня
								{
									DisplayFunctionPolyar(); //Перерисовка функции

									gr = Graphics.FromImage(bmFunction);

									//сетка для полярной системы координ
									DisplayGridPolyar();



									this.Invoke(new Action(() =>
									{

										pictureBox4.Image = (Image)(bmFunction);
									}));
								}
							}
							else
							{
								bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								gr = Graphics.FromImage(bmGrid);
								DisplayGridPolyar(); ;
								// Рисуем сетку полярной системы координат
								this.Invoke(new Action(() => { pictureBox4.Image = bmGrid; }));
							}

						}
						else //без сетки
						{
							if (bmFunction != null)
							{
								this.Invoke(new Action(() =>
								{
									checkBox7.Checked = false;
									BlockUiElements();

								}));

								if (checkBox5.Checked == true)  // с линиями уровня
								{
									DisplayFunctionPolyar();//Перерисовка функции

									gr = Graphics.FromImage(bmFunction);


									MarchingSquaresOriginal(); // Ставим линии уровня


									this.Invoke(new Action(() =>
								{

									pictureBox4.Image = (Image)(bmFunction);
								}));
								}
								else if (checkBox5.Checked == false)// без линий уровня
								{
									DisplayFunctionPolyar();//Перерисовка функции
									this.Invoke(new Action(() =>
								{
									pictureBox4.Image = (Image)(bmFunction);
								}));

								}

							}
							else
							{
								this.Invoke(new Action(() =>
								{
									pictureBox4.Image = null;
								}));
							}

						}

					}

					this.Invoke(new Action(() =>
					{
						OpenUiElements();
					}));

				}
				else
				{
					this.Invoke(new Action(() =>
					{
						OpenUiElements();

					}));

					if (checkBox7.Checked == true)
					{
						this.Invoke(new Action(() => { checkBox8.Checked = false; }));
						//this.Invoke(new Action(() => { OpenUiElements(); }));

					}
					else
					{
						this.Invoke(new Action(() => { checkBox8.Checked = true; }));
						//this.Invoke(new Action(() => { OpenUiElements(); }));
					}
				}
			});
		}


		////Задать функцию(Кнопка удалена с формы)
		//private void button3_Click( object sender, EventArgs e )
		//{
		//	F = true;
		//	getFunction = new GetFunction();
		//	getFunction.ShowDialog();

		//	ZFunctionDecartStatic =  getFunction.ZFunctionTxt.Text;
		//	ZFunctionPolyarStatic =  getFunction.textBox2.Text;
		//	ZFunctionDecartDinamic = getFunction.ZFunctionDinamicTxb.Text;
		//	ZFunctionPolyarDinamic = getFunction.textBox1.Text;

		//	getFunction.ZFunctionTxt.Text  =      ZFunctionDecartStatic  ;
		//	getFunction.textBox2.Text       =     ZFunctionPolyarStatic   ;
		//	getFunction.ZFunctionDinamicTxb.Text =ZFunctionDecartDinamic  ;
		//	getFunction.textBox1.Text     =       ZFunctionPolyarDinamic  ;
		//}

		public double ZnathFunctionDecartStatic( double x, double y )
		{
			//At = new Function($"{ZFunctionDecartStatic}");
			string XX = x.ToString().Replace(",", ".");
			string YY = y.ToString().Replace(",", ".");

			//XX = XX.Replace(",", ".");
			//YY = YY.Replace(",", ".");

			ExpressionParser parser = new ExpressionParser();
			parser.Values.Add("x", XX);
			parser.Values.Add("y", YY);
			Zfunction = parser.Parse($"{ZFunctionDecartStatic}");

			return Zfunction;

		}

		public double ZnathfunctionPolyarStatic( double fi, double r )
		{
			//ZFunctionPolyarStatic = textBox2.Text;
			//At = new Function($"At(fi,r) =  {ZFunctionPolyarStatic}");
			string FI = fi.ToString().Replace(",", ".");
			string R = r.ToString().Replace(",", ".");

			//FI = FI.Replace(",", ".");
			//R = R.Replace(",", ".");

			//el1 = new Expression($"At({FI},{R})", At);
			//Zfunction = el1.calculate();


			ExpressionParser parser = new ExpressionParser();
			parser.Values.Add("fi", FI);
			parser.Values.Add("r", R);
			Zfunction = parser.Parse($"{ZFunctionPolyarStatic}");

			return Zfunction;
		}

		public double ZnathFunctionDecartDinamic( double x, double y, double t )
		{

			string XX = x.ToString().Replace(",", ".");
			string YY = y.ToString().Replace(",", ".");
			string T = t.ToString().Replace(",", ".");

			ExpressionParser parser = new ExpressionParser();
			parser.Values.Add("x", XX);
			parser.Values.Add("y", YY);
			parser.Values.Add("t", T);
			Zfunction = parser.Parse($"{ZFunctionDecartDinamic}");

			return Zfunction;
		}

		public double ZnathfunctionPolyarDinamic( double fi, double r, double t )
		{

			string FI = fi.ToString().Replace(",", ".");
			string R = r.ToString().Replace(",", ".");
			string T = t.ToString().Replace(",", ".");

			ExpressionParser parser = new ExpressionParser();
			parser.Values.Add("fi", FI);
			parser.Values.Add("r", R);
			parser.Values.Add("t", T);
			Zfunction = parser.Parse($"{ZFunctionPolyarDinamic}");




			return Zfunction;
		}


		private void GetFunctonS_Click_1( object sender, EventArgs e )
		{
			BlockUiElements();

			F = true;
			getFunction = new GetFunction(ZFunctionDecartStatic, ZFunctionPolyarStatic, ZFunctionDecartDinamic, ZFunctionPolyarDinamic);
			getFunction.ShowDialog();

			ZFunctionDecartStatic = getFunction.ZFunctionDecartStatic;
			ZFunctionPolyarStatic = getFunction.ZFunctionPolyarStatic;
			ZFunctionDecartDinamic = getFunction.ZFunctionDecartDinamic;
			ZFunctionPolyarDinamic = getFunction.ZFunctionPolyarDinamic;
			getFunction.Close();

			OpenUiElements();
		}

		private void Graph2D_Load( object sender, EventArgs e )
		{

		}
		/// <summary>
		/// Parallel
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		async private void ParallelBtn_Click( object sender, EventArgs e )
		{
			//TimeParallel = 0;
			//Time = 0;
			timer = new Stopwatch();
			timer.Start();
			await Task.Run(() =>
			{
				this.Invoke(new Action(() =>
				{

					BlockUiElements();
				}));

				if (checkBox1.Checked == true)
				{
					try
					{

						// c сеткой
						if (checkBox3.Checked == true)
						{
							if (checkBox5.Checked == true)  // с линиями уровня
							{
								DisplayFunctionParallel(); //Перерисовка функции


								gr = Graphics.FromImage(bmFunction);

								//Отрисовка сетки 
								for (int x = (480 / SizeGrid); x < pictureBox4.Width; x += (480 / SizeGrid))
								{
									Start.X = 0;
									Start.Y = x;

									End.X = pictureBox1.Width - 1;
									End.Y = x;

									curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
									curcle.LineDisplay();

									Start.X = x;
									Start.Y = 0;

									End.X = x;
									End.Y = pictureBox1.Height - 1;

									curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
									curcle.LineDisplay();
								}
								if (checkBox7.Checked == true)
								{
									MarchingSquares(); // Ставим линии уровня
								}
								else if (checkBox7.Checked == false)
								{
									MarchingSquaresOriginal(); // Ставим линии уровня
								}

							}
							else if (checkBox5.Checked == false)// без линий уровня
							{

								DisplayFunctionParallel(); //Перерисовка функции

								gr = Graphics.FromImage(bmFunction);

								//Отрисовка сетки 
								for (int x = (480 / SizeGrid); x < pictureBox4.Width; x += (480 / SizeGrid))
								{
									Start.X = 0;
									Start.Y = x;

									End.X = pictureBox1.Width - 1;
									End.Y = x;

									curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
									curcle.LineDisplay();

									Start.X = x;
									Start.Y = 0;

									End.X = x;
									End.Y = pictureBox1.Height - 1;

									curcle = new Curcle(Start, End, Color.FromArgb(32, 34, 37), gr);
									curcle.LineDisplay();
								}
							}



							this.Invoke(new Action(() =>
							{
								pictureBox4.Image = (Image)(bmFunction);
							}));

						}
						else // без сетки
						{
							if (checkBox5.Checked == true)  // с линиями уровня
							{
								DisplayFunctionParallel(); //Перерисовка функции

								if (checkBox7.Checked == true)
								{
									MarchingSquares(); // Ставим линии уровня
								}
								else if (checkBox7.Checked == false)
								{
									MarchingSquaresOriginal(); // Ставим линии уровня
								}

								this.Invoke(new Action(() =>
								{
									pictureBox4.Image = (Image)(bmFunction);
								}));
							}
							else if (checkBox5.Checked == false)// без линий уровня
							{
								DisplayFunctionParallel(); //Перерисовка функции

								this.Invoke(new Action(() =>
								{
									pictureBox4.Image = (Image)(bmFunction);
								}));
							}


						}
					}
					catch
					{

						this.Invoke(new Action(() =>
						{

							button2.Enabled = false;


							bmFunction = null; // Удаляем нашу функцию
							pictureBox4.Image = null;
							checkBox2.Checked = false;

							if (checkBox3.Checked == true)
							{
								DisplayGridStartForm(); // Отрисовка сетки при декартовой систему координат
							}
							else if (checkBox4.Checked == true)
							{
								bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								gr = Graphics.FromImage(bmGrid);
								pictureBox4.Image = bmGrid;
							}
						}));
					}
				}       //декартова
				else if (checkBox2.Checked == true)
				{
					try
					{

						// c сеткой
						if (checkBox3.Checked == true)
						{
							if (checkBox5.Checked == true)  // с линиями уровня
							{
								DisplayFunctionPolyarParallel(); //Перерисовка функции

								gr = Graphics.FromImage(bmFunction);

								//сетка для полярной системы координ
								DisplayGridPolyar();

								if (checkBox7.Checked == true)
								{
									MarchingSquares(); // Ставим линии уровня
								}
								else if (checkBox7.Checked == false)
								{
									MarchingSquaresOriginal(); // Ставим линии уровня
								}

								this.Invoke(new Action(() =>
								{

									pictureBox4.Image = (Image)(bmFunction);
								}));
							}
							else if (checkBox5.Checked == false)// без линий уровня
							{
								DisplayFunctionPolyarParallel(); //Перерисовка функции

								gr = Graphics.FromImage(bmFunction);

								//сетка для полярной системы координ
								DisplayGridPolyar();



								this.Invoke(new Action(() =>
								{

									pictureBox4.Image = (Image)(bmFunction);
								}));
							}

						}
						else //без сетки
						{

							if (checkBox5.Checked == true)  // с линиями уровня
							{
								DisplayFunctionPolyarParallel();//Перерисовка функции

								gr = Graphics.FromImage(bmFunction);


								if (checkBox7.Checked == true)
								{
									MarchingSquares(); // Ставим линии уровня
								}
								else if (checkBox7.Checked == false)
								{
									MarchingSquaresOriginal(); // Ставим линии уровня
								}

								this.Invoke(new Action(() =>
								{

									pictureBox4.Image = (Image)(bmFunction);
								}));
							}
							else if (checkBox5.Checked == false)// без линий уровня
							{
								DisplayFunctionPolyarParallel();//Перерисовка функции
								this.Invoke(new Action(() =>
								{
									pictureBox4.Image = (Image)(bmFunction);
								}));

							}
						}
					}
					catch
					{
						this.Invoke(new Action(() =>
						{



							button2.Enabled = false;

							bmFunction = null; // Удаляем нашу функцию
							checkBox1.Checked = false;
							pictureBox4.Image = null;

							if (checkBox3.Checked == true) // с сеткой
							{
								BlockUiElements();

								//pictureBox4.BorderStyle = BorderStyle.None;

								bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								gr = Graphics.FromImage(bmGrid);

								DisplayGridPolyar();  // Рисуем сетку полярной системы координат

								pictureBox4.Image = bmGrid;
								OpenUiElements();

							}
							else if (checkBox4.Checked == true)
							{
								bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
								gr = Graphics.FromImage(bmGrid);
								pictureBox4.Image = bmGrid;
							}
						}));
					}
				}

				this.Invoke(new Action(() =>
				{
					OpenUiElements();
				}));
			});
			timer.Stop();

			TimeParallel = Convert.ToInt32(timer.ElapsedMilliseconds);
			if (Time != 0)
			{
				if (Time > TimeParallel)
				{
					TimeDifference = Time - TimeParallel;
				}
				else
				{
					TimeDifference = TimeParallel - Time;
				}
			}
			label8.Text = TimeParallel.ToString() + " ms";
			label11.Text = TimeDifference.ToString() + " ms";


		}

		public void DisplayFunctionParallel()
		{
			bmFunction = new Bitmap(pictureBox4.Width, pictureBox4.Height);
			gr = Graphics.FromImage(bmFunction);

			gr.SmoothingMode = SmoothingMode.AntiAlias;
			F = true;
			try
			{
				GetFunctionZnathParallel(); //просчитываем значения в узлах сетки и проводим интерполяцию данных 

				for (int i = 0; i < 481; i++)
				{
					for (int j = 0; j < 481; j++)
						//экранные координаты
						bmFunction.SetPixel(j, i, getColor(pixsel[i, j]));
				}

			}
			catch (Exception)
			{

				//thread.Abort();
				this.Invoke(new Action(() =>
				{
					MessageBox.Show("Введите пожалуйста корректно функцию или проверьте не присутствует ли деление на 0", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);

					if (thread != null)
						thread.Abort();

					if (F)
					{

						bmFunction = null; // Удаляем нашу функцию
						pictureBox4.Image = null;
						//checkBox2.Checked = false;

						if (checkBox3.Checked == true)
						{
							DisplayGridStartForm(); // Отрисовка сетки при декартовой систему координат
						}
						else if (checkBox4.Checked == true)
						{
							bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
							gr = Graphics.FromImage(bmGrid);
							pictureBox4.Image = bmGrid;
						}

						//pictureBox4.Image = null;
						OpenUiElements();
						return;


						//pictureBox4.Image = null;
						//OpenUiElements();
						//return;
					}
					else
					{


						button2.Enabled = false;
						//thread.Abort();
						bitmaps.Clear();
						OpenUiElements();
						StopThreadBtn.Enabled = false;
						label80.Text = "";
						progressBar1.Value = 0;
						F = true;
						k = 0;
					}


				}));
			}


		}

		public void GetFunctionZnathParallel()
		{
			pixsel = new double[481, 481];
			//16
			GridCoordinats = new double[SizeGrid + 1, SizeGrid + 1];


			ParallelFuncThread.Clear();
			ParallelFuncThread.Add(new Thread(() => GetFunctionLeftTop()));
			ParallelFuncThread.Add(new Thread(() => GetFunctionRightTop()));
			ParallelFuncThread.Add(new Thread(() => GetFunctionRightBottom()));
			ParallelFuncThread.Add(new Thread(() => GetFunctionLeftBottom()));
			foreach (Thread parallel in ParallelFuncThread)
				parallel.Start();
			foreach (Thread parallel in ParallelFuncThread)
				parallel.Join();


			//1 шаг ищём максимальное и минимальное значение в узлах исходной сетки 
			MinOriginal = GridCoordinats[0, 0];
			MaxOriginal = GridCoordinats[0, 0];

			for (int i = 0; i < SizeGrid + 1; i++)
			{
				for (int j = 0; j < SizeGrid + 1; j++)
				{
					if (GridCoordinats[i, j] < MinOriginal)
						MinOriginal = GridCoordinats[i, j];

					if (GridCoordinats[i, j] > MaxOriginal)
						MaxOriginal = GridCoordinats[i, j];

				}
			}

			//Минимальное  значение функции умноженное на -1
			MaxZnatLine = MinOriginal * -1;


			//Устанавливает значения в узлы сетки 
			ParallelFuncThread.Clear();
			ParallelFuncThread.Add(new Thread(() => GetFunctionLeftTopResize()));
			ParallelFuncThread.Add(new Thread(() => GetFunctionRightTopResize()));
			ParallelFuncThread.Add(new Thread(() => GetFunctionRightBottomResize()));
			ParallelFuncThread.Add(new Thread(() => GetFunctionLeftBottomResize()));
			foreach (Thread parallel in ParallelFuncThread)
				parallel.Start();
			foreach (Thread parallel in ParallelFuncThread)
				parallel.Join();

			Min = GridCoordinats[0, 0];
			Max = GridCoordinats[0, 0];

			//Находим масимальное значение в узлах изменённой сетки 
			for (int i = 0; i < SizeGrid + 1; i++)
			{
				for (int j = 0; j < SizeGrid + 1; j++)
				{
					if (GridCoordinats[i, j] < Min)
						Min = GridCoordinats[i, j];

					if (GridCoordinats[i, j] > Max)
						Max = GridCoordinats[i, j];
				}
			}

			PointX = 0;
			PointY = 0;
			//переносим значения в узлах сетки на пиксельную сетку
			for (int i = 0; i < 481; i += 480 / SizeGrid)
			{
				for (int j = 0; j < 481; j += 480 / SizeGrid)
				{
					pixsel[i, j] = GridCoordinats[PointY, PointX];
					PointX++;
				}
				PointY++;
				PointX = 0;
			}

			for (int i = 0; i <= 480 - (480 / SizeGrid); i += 480 / SizeGrid)
			{
				for (int j = 0; j <= 480 - (480 / SizeGrid); j += 480 / SizeGrid)
				{
					//координаты углов квадрата
					int LeftTopY = i;
					int LeftTopX = j;
					int RightTopY = i;
					int RightTopX = j + (480 / SizeGrid);
					int LeftBottomY = i + (480 / SizeGrid);
					int LeftBottomX = j;
					int RightBottomY = i + (480 / SizeGrid);
					int RightBottomX = j + (480 / SizeGrid);

					PointSquart(LeftTopY, LeftTopX, RightTopY, RightTopX,
						LeftBottomY, LeftBottomX, RightBottomY, RightBottomX);

				}

			}

		}

		public void DisplayFunctionPolyarParallel()
		{
			bmFunction = new Bitmap(pictureBox4.Width, pictureBox4.Height);
			gr = Graphics.FromImage(bmFunction);
			int XX = 0;
			int YY = 0;
			double Distans = 0;
			int radius = ((pictureBox4.Height - 1) / 2);
			gr.SmoothingMode = SmoothingMode.AntiAlias;
			F = true;

			try
			{
				//	// Просчитать узлы
				GetFunctionZnathPolyarParallel();

				for (int i = 0; i < 481; i++)
				{

					for (int j = 0; j < 481; j++)
					{
						XX = (radius * -1) + j;
						YY = radius - i;
						Distans = Math.Sqrt(XX * XX + YY * YY);
						if (Distans <= radius)
							bmFunction.SetPixel(j, i, getColor(pixsel[i, j]));

					}
				}
			}
			//MessageBox.Show("Введите пожалуйста корректно функцию или проверьте не присутствует ли деление на 0", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
			catch (Exception)
			{
				//thread.Abort();

				this.Invoke(new Action(() =>
				{
					MessageBox.Show("Введите пожалуйста корректно функцию или проверьте не присутствует ли деление на 0", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);

					if (thread != null)
						thread.Abort();
					if (F)
					{

						button2.Enabled = false;

						bmFunction = null; // Удаляем нашу функцию
						checkBox1.Checked = false;
						pictureBox4.Image = null;

						if (checkBox3.Checked == true) // с сеткой
						{
							BlockUiElements();

							//pictureBox4.BorderStyle = BorderStyle.None;

							bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
							gr = Graphics.FromImage(bmGrid);

							DisplayGridPolyar();  // Рисуем сетку полярной системы координат

							pictureBox4.Image = bmGrid;
							OpenUiElements();

						}
						else if (checkBox4.Checked == true)
						{
							bmGrid = new Bitmap(pictureBox4.Height, pictureBox4.Width);
							gr = Graphics.FromImage(bmGrid);
							pictureBox4.Image = bmGrid;
						}


						//pictureBox4.Image = null;
						OpenUiElements();
						return;

					}
					else
					{


						button2.Enabled = false;
						//thread.Abort();
						bitmaps.Clear();
						OpenUiElements();
						StopThreadBtn.Enabled = false;
						label80.Text = "";
						progressBar1.Value = 0;
						F = true;
						k = 0;
					}


				}));
			}

			//break;
		}
		public void GetFunctionZnathPolyarParallel()
		{
			pixsel = new double[481, 481];
			//16
			GridCoordinats = new double[SizeGrid + 1, SizeGrid + 1];



			ParallelFuncThread.Clear();
			ParallelFuncThread.Add(new Thread(() => GetFunctionLeftTopPolyar()));
			ParallelFuncThread.Add(new Thread(() => GetFunctionRightTopPolyar()));
			ParallelFuncThread.Add(new Thread(() => GetFunctionRightBottomPolyar()));
			ParallelFuncThread.Add(new Thread(() => GetFunctionLeftBottomPolyar()));
			foreach (Thread parallel in ParallelFuncThread)
				parallel.Start();
			foreach (Thread parallel in ParallelFuncThread)
				parallel.Join();

			//1 шаг ищём максимальное и минимальное значение в узлах исходной сетки 
			MinOriginal = GridCoordinats[0, 0];
			MaxOriginal = GridCoordinats[0, 0];

			for (int i = 0; i < SizeGrid + 1; i++)
			{
				for (int j = 0; j < SizeGrid + 1; j++)
				{
					if (GridCoordinats[i, j] < MinOriginal)
						MinOriginal = GridCoordinats[i, j];

					if (GridCoordinats[i, j] > MaxOriginal)
						MaxOriginal = GridCoordinats[i, j];
				}
			}

			//Минимальное  значение функции умноженное на -1
			MaxZnatLine = MinOriginal * -1;


			ParallelFuncThread.Clear();
			ParallelFuncThread.Add(new Thread(() => GetFunctionLeftTopResizePolyar()));
			ParallelFuncThread.Add(new Thread(() => GetFunctionRightTopResizePolyar()));
			ParallelFuncThread.Add(new Thread(() => GetFunctionRightBottomResizePolyar()));
			ParallelFuncThread.Add(new Thread(() => GetFunctionLeftBottomResizePolyar()));
			foreach (Thread parallel in ParallelFuncThread)
				parallel.Start();
			foreach (Thread parallel in ParallelFuncThread)
				parallel.Join();


			Min = GridCoordinats[0, 0];
			Max = GridCoordinats[0, 0];

			//Находим масимальное значение в узлах изменённой сетки 
			for (int i = 0; i < SizeGrid + 1; i++)
			{
				for (int j = 0; j < SizeGrid + 1; j++)
				{
					if (GridCoordinats[i, j] < Min)
						Min = GridCoordinats[i, j];

					if (GridCoordinats[i, j] > Max)
						Max = GridCoordinats[i, j];
				}
			}

			PointX = 0;
			PointY = 0;
			//переносим значения в узлах сетки на пиксельную сетку
			for (int i = 0; i < 481; i += 480 / SizeGrid)
			{
				for (int j = 0; j < 481; j += 480 / SizeGrid)
				{
					pixsel[i, j] = GridCoordinats[PointY, PointX];
					PointX++;
				}
				PointY++;
				PointX = 0;
			}

			for (int i = 0; i <= 480 - (480 / SizeGrid); i += 480 / SizeGrid)
			{
				for (int j = 0; j <= 480 - (480 / SizeGrid); j += 480 / SizeGrid)
				{
					//координаты углов квадрата
					int LeftTopY = i;
					int LeftTopX = j;
					int RightTopY = i;
					int RightTopX = j + (480 / SizeGrid);
					int LeftBottomY = i + (480 / SizeGrid);
					int LeftBottomX = j;
					int RightBottomY = i + (480 / SizeGrid);
					int RightBottomX = j + (480 / SizeGrid);

					PointSquart(LeftTopY, LeftTopX, RightTopY, RightTopX,
						LeftBottomY, LeftBottomX, RightBottomY, RightBottomX);

				}

			}
		}

		#region DecartFunctionParallel
		public void GetFunctionLeftTop()
		{
			double ShagLeftTop = SizeAxis / (SizeGrid / 2);
			double ShagXLeftTop = 0;
			double ShagYLeftTop = 0;
			//заполняем значениями сетку
			for (int i = 0; i < ((SizeGrid / 2) + 1); i++)
			{
				for (int j = 0; j < ((SizeGrid / 2) + 1); j++)
				{                //8
					double x = (SizeAxis * -1) + ShagXLeftTop;
					//8
					double y = SizeAxis - ShagYLeftTop;

					try
					{

						if (ParallelCardr == false)
						{

							string XX = x.ToString().Replace(",", ".");
							string YY = y.ToString().Replace(",", ".");
							//?
							ExpressionParser parser = new ExpressionParser();
							parser.Values.Add("x", XX);
							parser.Values.Add("y", YY);
							GridCoordinats[i, j] = parser.Parse($"{ZFunctionDecartStatic}");
						}
						else
						{
							string XX = x.ToString().Replace(",", ".");
							string YY = y.ToString().Replace(",", ".");
							string T = ParamFunction.ToString().Replace(",", ".");

							ExpressionParser parser = new ExpressionParser();
							parser.Values.Add("x", XX);
							parser.Values.Add("y", YY);
							parser.Values.Add("t", T);
							GridCoordinats[i, j] = parser.Parse($"{ZFunctionDecartDinamic}");
						}
					}
					catch
					{
						foreach (Thread parallel in ParallelFuncThread)
							parallel.Abort();
						return;
					}



					ShagXLeftTop += ShagLeftTop;
				}
				ShagXLeftTop = 0;
				ShagYLeftTop += ShagLeftTop;

			}
		}
		public void GetFunctionRightTop()
		{
			double ShagRightTop = SizeAxis / (SizeGrid / 2);
			double ShagXRightTop = ShagRightTop * ((SizeGrid / 2) + 1);
			double ShagYRightTop = 0;

			for (int i = 0; i <= SizeGrid / 2; i++)
			{
				for (int j = ((SizeGrid / 2) + 1); j < SizeGrid + 1; j++)
				{                //8
					double x = (SizeAxis * -1) + ShagXRightTop;
					//8
					double y = SizeAxis - ShagYRightTop;

					try
					{

						if (ParallelCardr == false)
						{

							string XX = x.ToString().Replace(",", ".");
							string YY = y.ToString().Replace(",", ".");
							//?
							ExpressionParser parser = new ExpressionParser();
							parser.Values.Add("x", XX);
							parser.Values.Add("y", YY);
							GridCoordinats[i, j] = parser.Parse($"{ZFunctionDecartStatic}");
						}
						else
						{
							string XX = x.ToString().Replace(",", ".");
							string YY = y.ToString().Replace(",", ".");
							string T = ParamFunction.ToString().Replace(",", ".");

							ExpressionParser parser = new ExpressionParser();
							parser.Values.Add("x", XX);
							parser.Values.Add("y", YY);
							parser.Values.Add("t", T);
							GridCoordinats[i, j] = parser.Parse($"{ZFunctionDecartDinamic}");
						}

						ShagXRightTop += ShagRightTop;
					}
					catch
					{
						foreach (Thread parallel in ParallelFuncThread)
							parallel.Abort();
						return;
					}
				}
				ShagXRightTop = ShagRightTop * ((SizeGrid / 2) + 1);
				ShagYRightTop += ShagRightTop;

			}
		}
		public void GetFunctionLeftBottom()
		{
			double ShagLeftBottom = SizeAxis / (SizeGrid / 2);
			double ShagXLeftBottom = 0;
			double ShagYLeftBottom = ShagLeftBottom * ((SizeGrid / 2) + 1);

			for (int i = (SizeGrid / 2) + 1; i < SizeGrid + 1; i++)
			{
				for (int j = 0; j < ((SizeGrid / 2) + 1); j++)
				{                //8
					double x = (SizeAxis * -1) + ShagXLeftBottom;
					//8
					double y = SizeAxis - ShagYLeftBottom;

					try
					{

						if (ParallelCardr == false)
						{

							string XX = x.ToString().Replace(",", ".");
							string YY = y.ToString().Replace(",", ".");
							//?
							ExpressionParser parser = new ExpressionParser();
							parser.Values.Add("x", XX);
							parser.Values.Add("y", YY);
							GridCoordinats[i, j] = parser.Parse($"{ZFunctionDecartStatic}");
						}
						else
						{
							string XX = x.ToString().Replace(",", ".");
							string YY = y.ToString().Replace(",", ".");
							string T = ParamFunction.ToString().Replace(",", ".");

							ExpressionParser parser = new ExpressionParser();
							parser.Values.Add("x", XX);
							parser.Values.Add("y", YY);
							parser.Values.Add("t", T);
							GridCoordinats[i, j] = parser.Parse($"{ZFunctionDecartDinamic}");
						}

						ShagXLeftBottom += ShagLeftBottom;
					}
					catch
					{
						foreach (Thread parallel in ParallelFuncThread)
							parallel.Abort();
						return;
					}
				}
				ShagXLeftBottom = 0;
				ShagYLeftBottom += ShagLeftBottom;

			}
		}
		public void GetFunctionRightBottom()
		{
			double ShagRightBottom = SizeAxis / (SizeGrid / 2);
			double ShagXRightBottom = ShagRightBottom * ((SizeGrid / 2) + 1);
			double ShagYRightBottom = ShagRightBottom * ((SizeGrid / 2) + 1);

			for (int i = (SizeGrid / 2) + 1; i < SizeGrid + 1; i++)
			{
				for (int j = (SizeGrid / 2) + 1; j < SizeGrid + 1; j++)
				{                //8
					double x = (SizeAxis * -1) + ShagXRightBottom;
					//8
					double y = SizeAxis - ShagYRightBottom;

					try
					{

						if (ParallelCardr == false)
						{

							string XX = x.ToString().Replace(",", ".");
							string YY = y.ToString().Replace(",", ".");
							//?
							ExpressionParser parser = new ExpressionParser();
							parser.Values.Add("x", XX);
							parser.Values.Add("y", YY);
							GridCoordinats[i, j] = parser.Parse($"{ZFunctionDecartStatic}");
						}
						else
						{
							string XX = x.ToString().Replace(",", ".");
							string YY = y.ToString().Replace(",", ".");
							string T = ParamFunction.ToString().Replace(",", ".");

							ExpressionParser parser = new ExpressionParser();
							parser.Values.Add("x", XX);
							parser.Values.Add("y", YY);
							parser.Values.Add("t", T);
							GridCoordinats[i, j] = parser.Parse($"{ZFunctionDecartDinamic}");
						}
						ShagXRightBottom += ShagRightBottom;
					}
					catch
					{
						foreach (Thread parallel in ParallelFuncThread)
							parallel.Abort();
						return;
					}
				}
				ShagXRightBottom = ShagRightBottom * ((SizeGrid / 2) + 1);
				ShagYRightBottom += ShagRightBottom;

			}
		}

		//////////////////////////////////////

		public void GetFunctionLeftTopResize()
		{
			double ShagLeftTopResize = SizeAxis / (SizeGrid / 2);
			double ShagXLeftTopResize = 0;
			double ShagYLeftTopResize = 0;
			//заполняем значениями сетку
			for (int i = 0; i < ((SizeGrid / 2) + 1); i++)
			{
				for (int j = 0; j < ((SizeGrid / 2) + 1); j++)
				{                //8
					double x = (SizeAxis * -1) + ShagXLeftTopResize;
					//8
					double y = SizeAxis - ShagYLeftTopResize;

					try
					{

						if (MinOriginal >= 0)
						{
							if (ParallelCardr == false)
							{

								string XX = x.ToString().Replace(",", ".");
								string YY = y.ToString().Replace(",", ".");
								//?
								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("x", XX);
								parser.Values.Add("y", YY);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionDecartStatic}");
							}
							else
							{
								string XX = x.ToString().Replace(",", ".");
								string YY = y.ToString().Replace(",", ".");
								string T = ParamFunction.ToString().Replace(",", ".");

								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("x", XX);
								parser.Values.Add("y", YY);
								parser.Values.Add("t", T);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionDecartDinamic}");
							}
							ShagXLeftTopResize += ShagLeftTopResize;
						}
						else
						{

							if (ParallelCardr == false)
							{

								string XX = x.ToString().Replace(",", ".");
								string YY = y.ToString().Replace(",", ".");
								//?
								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("x", XX);
								parser.Values.Add("y", YY);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionDecartStatic}") + MaxZnatLine;
							}
							else
							{
								string XX = x.ToString().Replace(",", ".");
								string YY = y.ToString().Replace(",", ".");
								string T = ParamFunction.ToString().Replace(",", ".");

								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("x", XX);
								parser.Values.Add("y", YY);
								parser.Values.Add("t", T);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionDecartDinamic}") + MaxZnatLine;
							}
							ShagXLeftTopResize += ShagLeftTopResize;
						}
					}
					catch
					{
						foreach (Thread parallel in ParallelFuncThread)
							parallel.Abort();
						return;
					}


				}
				ShagXLeftTopResize = 0;
				ShagYLeftTopResize += ShagLeftTopResize;

			}
		}
		public void GetFunctionRightTopResize()
		{
			double ShagRightTopResize = SizeAxis / (SizeGrid / 2);
			double ShagXRightTopResize = ShagRightTopResize * ((SizeGrid / 2) + 1);
			double ShagYRightTopResize = 0;

			for (int i = 0; i <= SizeGrid / 2; i++)
			{
				for (int j = ((SizeGrid / 2) + 1); j < SizeGrid + 1; j++)
				{                //8
					double x = (SizeAxis * -1) + ShagXRightTopResize;
					//8
					double y = SizeAxis - ShagYRightTopResize;

					try
					{

						if (MinOriginal >= 0)
						{
							if (ParallelCardr == false)
							{

								string XX = x.ToString().Replace(",", ".");
								string YY = y.ToString().Replace(",", ".");
								//?
								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("x", XX);
								parser.Values.Add("y", YY);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionDecartStatic}");
							}
							else
							{
								string XX = x.ToString().Replace(",", ".");
								string YY = y.ToString().Replace(",", ".");
								string T = ParamFunction.ToString().Replace(",", ".");

								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("x", XX);
								parser.Values.Add("y", YY);
								parser.Values.Add("t", T);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionDecartDinamic}");
							}
							ShagXRightTopResize += ShagRightTopResize;
						}
						else
						{

							if (ParallelCardr == false)
							{

								string XX = x.ToString().Replace(",", ".");
								string YY = y.ToString().Replace(",", ".");
								//?
								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("x", XX);
								parser.Values.Add("y", YY);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionDecartStatic}") + MaxZnatLine;
							}
							else
							{
								string XX = x.ToString().Replace(",", ".");
								string YY = y.ToString().Replace(",", ".");
								string T = ParamFunction.ToString().Replace(",", ".");

								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("x", XX);
								parser.Values.Add("y", YY);
								parser.Values.Add("t", T);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionDecartDinamic}") + MaxZnatLine;
							}
							ShagXRightTopResize += ShagRightTopResize;
						}
					}
					catch
					{
						foreach (Thread parallel in ParallelFuncThread)
							parallel.Abort();
						return;
					}


				}
				ShagXRightTopResize = ShagRightTopResize * ((SizeGrid / 2) + 1);
				ShagYRightTopResize += ShagRightTopResize;

			}
		}
		public void GetFunctionLeftBottomResize()
		{
			double ShagLeftBottomResize = SizeAxis / (SizeGrid / 2);
			double ShagXLeftBottomResize = 0;
			double ShagYLeftBottomResize = ShagLeftBottomResize * ((SizeGrid / 2) + 1);

			for (int i = (SizeGrid / 2) + 1; i < SizeGrid + 1; i++)
			{
				for (int j = 0; j < ((SizeGrid / 2) + 1); j++)
				{                //8
					double x = (SizeAxis * -1) + ShagXLeftBottomResize;
					//8
					double y = SizeAxis - ShagYLeftBottomResize;


					try
					{

						if (MinOriginal >= 0)
						{
							if (ParallelCardr == false)
							{

								string XX = x.ToString().Replace(",", ".");
								string YY = y.ToString().Replace(",", ".");
								//?
								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("x", XX);
								parser.Values.Add("y", YY);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionDecartStatic}");
							}
							else
							{
								string XX = x.ToString().Replace(",", ".");
								string YY = y.ToString().Replace(",", ".");
								string T = ParamFunction.ToString().Replace(",", ".");

								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("x", XX);
								parser.Values.Add("y", YY);
								parser.Values.Add("t", T);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionDecartDinamic}");
							}
							ShagXLeftBottomResize += ShagLeftBottomResize;
						}
						else
						{

							if (ParallelCardr == false)
							{

								string XX = x.ToString().Replace(",", ".");
								string YY = y.ToString().Replace(",", ".");
								//?
								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("x", XX);
								parser.Values.Add("y", YY);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionDecartStatic}") + MaxZnatLine;
							}
							else
							{
								string XX = x.ToString().Replace(",", ".");
								string YY = y.ToString().Replace(",", ".");
								string T = ParamFunction.ToString().Replace(",", ".");

								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("x", XX);
								parser.Values.Add("y", YY);
								parser.Values.Add("t", T);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionDecartDinamic}") + MaxZnatLine;
							}
							ShagXLeftBottomResize += ShagLeftBottomResize;
						}
					}
					catch
					{
						foreach (Thread parallel in ParallelFuncThread)
							parallel.Abort();
						return;
					}

				}
				ShagXLeftBottomResize = 0;
				ShagYLeftBottomResize += ShagLeftBottomResize;

			}
		}
		public void GetFunctionRightBottomResize()
		{
			double ShagRightBottomResize = SizeAxis / (SizeGrid / 2);
			double ShagXRightBottomResize = ShagRightBottomResize * ((SizeGrid / 2) + 1);
			double ShagYRightBottomResize = ShagRightBottomResize * ((SizeGrid / 2) + 1);

			for (int i = (SizeGrid / 2) + 1; i < SizeGrid + 1; i++)
			{
				for (int j = (SizeGrid / 2) + 1; j < SizeGrid + 1; j++)
				{                //8
					double x = (SizeAxis * -1) + ShagXRightBottomResize;
					//8
					double y = SizeAxis - ShagYRightBottomResize;

					try
					{

						if (MinOriginal >= 0)
						{
							if (ParallelCardr == false)
							{

								string XX = x.ToString().Replace(",", ".");
								string YY = y.ToString().Replace(",", ".");
								//?
								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("x", XX);
								parser.Values.Add("y", YY);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionDecartStatic}");
							}
							else
							{
								string XX = x.ToString().Replace(",", ".");
								string YY = y.ToString().Replace(",", ".");
								string T = ParamFunction.ToString().Replace(",", ".");

								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("x", XX);
								parser.Values.Add("y", YY);
								parser.Values.Add("t", T);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionDecartDinamic}");
							}
							ShagXRightBottomResize += ShagRightBottomResize;
						}
						else
						{

							if (ParallelCardr == false)
							{

								string XX = x.ToString().Replace(",", ".");
								string YY = y.ToString().Replace(",", ".");
								//?
								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("x", XX);
								parser.Values.Add("y", YY);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionDecartStatic}") + MaxZnatLine;
							}
							else
							{
								string XX = x.ToString().Replace(",", ".");
								string YY = y.ToString().Replace(",", ".");
								string T = ParamFunction.ToString().Replace(",", ".");

								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("x", XX);
								parser.Values.Add("y", YY);
								parser.Values.Add("t", T);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionDecartDinamic}") + MaxZnatLine;
							}
							ShagXRightBottomResize += ShagRightBottomResize;
						}
					}
					catch
					{
						foreach (Thread parallel in ParallelFuncThread)
							parallel.Abort();
						return;
					}

				}
				ShagXRightBottomResize = ShagRightBottomResize * ((SizeGrid / 2) + 1);
				ShagYRightBottomResize += ShagRightBottomResize;

			}
		}
		#endregion

		#region PolyarFunctionParallel			
		public void GetFunctionLeftTopPolyar()
		{
			double ShagLeftTop = SizeAxis / (SizeGrid / 2);
			double ShagXLeftTop = 0;
			double ShagYLeftTop = 0;

			//заполняем значениями сетку
			for (int i = 0; i < ((SizeGrid / 2) + 1); i++)
			{
				for (int j = 0; j < ((SizeGrid / 2) + 1); j++)
				{                //8
					double x = (SizeAxis * -1) + ShagXLeftTop;
					//8
					double y = SizeAxis - ShagYLeftTop;

					double r = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
					double fi = Math.Atan2(y, x);

					try
					{

						if (ParallelCardr == false)
						{

							string FI = fi.ToString().Replace(",", ".");
							string R = r.ToString().Replace(",", ".");
							ExpressionParser parser = new ExpressionParser();
							parser.Values.Add("fi", FI);
							parser.Values.Add("r", R);
							GridCoordinats[i, j] = parser.Parse($"{ZFunctionPolyarStatic}");
						}
						else
						{
							string FI = fi.ToString().Replace(",", ".");
							string R = r.ToString().Replace(",", ".");
							string T = ParamFunction.ToString().Replace(",", ".");

							ExpressionParser parser = new ExpressionParser();
							parser.Values.Add("fi", FI);
							parser.Values.Add("r", R);
							parser.Values.Add("t", T);
							GridCoordinats[i, j] = parser.Parse($"{ZFunctionPolyarDinamic}");
						}
					}
					catch
					{
						foreach (Thread parallel in ParallelFuncThread)
							parallel.Abort();
						return;
					}


					ShagXLeftTop += ShagLeftTop;
				}
				ShagXLeftTop = 0;
				ShagYLeftTop += ShagLeftTop;

			}
		}
		public void GetFunctionRightTopPolyar()
		{
			double ShagRightTop = SizeAxis / (SizeGrid / 2);
			double ShagXRightTop = ShagRightTop * ((SizeGrid / 2) + 1);
			double ShagYRightTop = 0;

			for (int i = 0; i <= SizeGrid / 2; i++)
			{
				for (int j = ((SizeGrid / 2) + 1); j < SizeGrid + 1; j++)
				{                //8
					double x = (SizeAxis * -1) + ShagXRightTop;
					//8
					double y = SizeAxis - ShagYRightTop;


					double r = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
					double fi = Math.Atan2(y, x);

					try
					{

						if (ParallelCardr == false)
						{

							string FI = fi.ToString().Replace(",", ".");
							string R = r.ToString().Replace(",", ".");
							ExpressionParser parser = new ExpressionParser();
							parser.Values.Add("fi", FI);
							parser.Values.Add("r", R);
							GridCoordinats[i, j] = parser.Parse($"{ZFunctionPolyarStatic}");
						}
						else
						{
							string FI = fi.ToString().Replace(",", ".");
							string R = r.ToString().Replace(",", ".");
							string T = ParamFunction.ToString().Replace(",", ".");

							ExpressionParser parser = new ExpressionParser();
							parser.Values.Add("fi", FI);
							parser.Values.Add("r", R);
							parser.Values.Add("t", T);
							GridCoordinats[i, j] = parser.Parse($"{ZFunctionPolyarDinamic}");
						}
					}
					catch
					{
						foreach (Thread parallel in ParallelFuncThread)
							parallel.Abort();
						return;
					}

					ShagXRightTop += ShagRightTop;
				}
				ShagXRightTop = ShagRightTop * ((SizeGrid / 2) + 1);
				ShagYRightTop += ShagRightTop;

			}
		}
		public void GetFunctionLeftBottomPolyar()
		{
			double ShagLeftBottom = SizeAxis / (SizeGrid / 2);
			double ShagXLeftBottom = 0;
			double ShagYLeftBottom = ShagLeftBottom * ((SizeGrid / 2) + 1);

			for (int i = (SizeGrid / 2) + 1; i < SizeGrid + 1; i++)
			{
				for (int j = 0; j < ((SizeGrid / 2) + 1); j++)
				{                //8
					double x = (SizeAxis * -1) + ShagXLeftBottom;
					//8
					double y = SizeAxis - ShagYLeftBottom;

					double r = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
					double fi = Math.Atan2(y, x);
					try
					{

						if (ParallelCardr == false)
						{

							string FI = fi.ToString().Replace(",", ".");
							string R = r.ToString().Replace(",", ".");
							ExpressionParser parser = new ExpressionParser();
							parser.Values.Add("fi", FI);
							parser.Values.Add("r", R);
							GridCoordinats[i, j] = parser.Parse($"{ZFunctionPolyarStatic}");
						}
						else
						{
							string FI = fi.ToString().Replace(",", ".");
							string R = r.ToString().Replace(",", ".");
							string T = ParamFunction.ToString().Replace(",", ".");

							ExpressionParser parser = new ExpressionParser();
							parser.Values.Add("fi", FI);
							parser.Values.Add("r", R);
							parser.Values.Add("t", T);
							GridCoordinats[i, j] = parser.Parse($"{ZFunctionPolyarDinamic}");
						}
					}
					catch
					{
						foreach (Thread parallel in ParallelFuncThread)
							parallel.Abort();
						return;
					}

					ShagXLeftBottom += ShagLeftBottom;
				}
				ShagXLeftBottom = 0;
				ShagYLeftBottom += ShagLeftBottom;

			}
		}
		public void GetFunctionRightBottomPolyar()
		{
			double ShagRightBottom = SizeAxis / (SizeGrid / 2);
			double ShagXRightBottom = ShagRightBottom * ((SizeGrid / 2) + 1);
			double ShagYRightBottom = ShagRightBottom * ((SizeGrid / 2) + 1);

			for (int i = (SizeGrid / 2) + 1; i < SizeGrid + 1; i++)
			{
				for (int j = (SizeGrid / 2) + 1; j < SizeGrid + 1; j++)
				{                //8
					double x = (SizeAxis * -1) + ShagXRightBottom;
					//8
					double y = SizeAxis - ShagYRightBottom;

					double r = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
					double fi = Math.Atan2(y, x);
					try
					{

						if (ParallelCardr == false)
						{

							string FI = fi.ToString().Replace(",", ".");
							string R = r.ToString().Replace(",", ".");
							ExpressionParser parser = new ExpressionParser();
							parser.Values.Add("fi", FI);
							parser.Values.Add("r", R);
							GridCoordinats[i, j] = parser.Parse($"{ZFunctionPolyarStatic}");
						}
						else
						{
							string FI = fi.ToString().Replace(",", ".");
							string R = r.ToString().Replace(",", ".");
							string T = ParamFunction.ToString().Replace(",", ".");

							ExpressionParser parser = new ExpressionParser();
							parser.Values.Add("fi", FI);
							parser.Values.Add("r", R);
							parser.Values.Add("t", T);
							GridCoordinats[i, j] = parser.Parse($"{ZFunctionPolyarDinamic}");
						}
					}
					catch
					{
						foreach (Thread parallel in ParallelFuncThread)
							parallel.Abort();
						return;
					}


					ShagXRightBottom += ShagRightBottom;
				}
				ShagXRightBottom = ShagRightBottom * ((SizeGrid / 2) + 1);
				ShagYRightBottom += ShagRightBottom;

			}
		}

		//////////////////////////////////////

		public void GetFunctionLeftTopResizePolyar()
		{
			double ShagLeftTopResize = SizeAxis / (SizeGrid / 2);
			double ShagXLeftTopResize = 0;
			double ShagYLeftTopResize = 0;
			//заполняем значениями сетку
			for (int i = 0; i < ((SizeGrid / 2) + 1); i++)
			{
				for (int j = 0; j < ((SizeGrid / 2) + 1); j++)
				{                //8
					double x = (SizeAxis * -1) + ShagXLeftTopResize;
					//8
					double y = SizeAxis - ShagYLeftTopResize;

					double r = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
					double fi = Math.Atan2(y, x);
					try
					{


						if (MinOriginal >= 0)
						{

							if (ParallelCardr == false)
							{

								string FI = fi.ToString().Replace(",", ".");
								string R = r.ToString().Replace(",", ".");
								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("fi", FI);
								parser.Values.Add("r", R);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionPolyarStatic}");
							}
							else
							{
								string FI = fi.ToString().Replace(",", ".");
								string R = r.ToString().Replace(",", ".");
								string T = ParamFunction.ToString().Replace(",", ".");

								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("fi", FI);
								parser.Values.Add("r", R);
								parser.Values.Add("t", T);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionPolyarDinamic}");
							}

							ShagXLeftTopResize += ShagLeftTopResize;
						}
						else
						{
							if (ParallelCardr == false)
							{

								string FI = fi.ToString().Replace(",", ".");
								string R = r.ToString().Replace(",", ".");
								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("fi", FI);
								parser.Values.Add("r", R);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionPolyarStatic}") + MaxZnatLine;
							}
							else
							{
								string FI = fi.ToString().Replace(",", ".");
								string R = r.ToString().Replace(",", ".");
								string T = ParamFunction.ToString().Replace(",", ".");

								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("fi", FI);
								parser.Values.Add("r", R);
								parser.Values.Add("t", T);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionPolyarDinamic}") + MaxZnatLine;
							}

							ShagXLeftTopResize += ShagLeftTopResize;
						}

					}
					catch
					{
						foreach (Thread parallel in ParallelFuncThread)
							parallel.Abort();
						return;
					}

				}
				ShagXLeftTopResize = 0;
				ShagYLeftTopResize += ShagLeftTopResize;

			}
		}
		public void GetFunctionRightTopResizePolyar()
		{
			double ShagRightTopResize = SizeAxis / (SizeGrid / 2);
			double ShagXRightTopResize = ShagRightTopResize * ((SizeGrid / 2) + 1);
			double ShagYRightTopResize = 0;

			for (int i = 0; i <= SizeGrid / 2; i++)
			{
				for (int j = ((SizeGrid / 2) + 1); j < SizeGrid + 1; j++)
				{                //8
					double x = (SizeAxis * -1) + ShagXRightTopResize;
					//8
					double y = SizeAxis - ShagYRightTopResize;

					double r = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
					double fi = Math.Atan2(y, x);
					try
					{

						if (MinOriginal >= 0)
						{

							if (ParallelCardr == false)
							{

								string FI = fi.ToString().Replace(",", ".");
								string R = r.ToString().Replace(",", ".");
								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("fi", FI);
								parser.Values.Add("r", R);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionPolyarStatic}");
							}
							else
							{
								string FI = fi.ToString().Replace(",", ".");
								string R = r.ToString().Replace(",", ".");
								string T = ParamFunction.ToString().Replace(",", ".");

								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("fi", FI);
								parser.Values.Add("r", R);
								parser.Values.Add("t", T);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionPolyarDinamic}");
							}
							ShagXRightTopResize += ShagRightTopResize;
						}
						else
						{

							if (ParallelCardr == false)
							{

								string FI = fi.ToString().Replace(",", ".");
								string R = r.ToString().Replace(",", ".");
								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("fi", FI);
								parser.Values.Add("r", R);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionPolyarStatic}") + MaxZnatLine;
							}
							else
							{
								string FI = fi.ToString().Replace(",", ".");
								string R = r.ToString().Replace(",", ".");
								string T = ParamFunction.ToString().Replace(",", ".");

								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("fi", FI);
								parser.Values.Add("r", R);
								parser.Values.Add("t", T);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionPolyarDinamic}") + MaxZnatLine;
							}

							ShagXRightTopResize += ShagRightTopResize;
						}
					}
					catch
					{
						foreach (Thread parallel in ParallelFuncThread)
							parallel.Abort();
						return;
					}

				}
				ShagXRightTopResize = ShagRightTopResize * ((SizeGrid / 2) + 1);
				ShagYRightTopResize += ShagRightTopResize;

			}
		}
		public void GetFunctionLeftBottomResizePolyar()
		{
			double ShagLeftBottomResize = SizeAxis / (SizeGrid / 2);
			double ShagXLeftBottomResize = 0;
			double ShagYLeftBottomResize = ShagLeftBottomResize * ((SizeGrid / 2) + 1);

			for (int i = (SizeGrid / 2) + 1; i < SizeGrid + 1; i++)
			{
				for (int j = 0; j < ((SizeGrid / 2) + 1); j++)
				{                //8
					double x = (SizeAxis * -1) + ShagXLeftBottomResize;
					//8
					double y = SizeAxis - ShagYLeftBottomResize;

					double r = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
					double fi = Math.Atan2(y, x);
					try
					{

						if (MinOriginal >= 0)
						{


							if (ParallelCardr == false)
							{

								string FI = fi.ToString().Replace(",", ".");
								string R = r.ToString().Replace(",", ".");
								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("fi", FI);
								parser.Values.Add("r", R);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionPolyarStatic}");
							}
							else
							{
								string FI = fi.ToString().Replace(",", ".");
								string R = r.ToString().Replace(",", ".");
								string T = ParamFunction.ToString().Replace(",", ".");

								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("fi", FI);
								parser.Values.Add("r", R);
								parser.Values.Add("t", T);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionPolyarDinamic}");
							}
							ShagXLeftBottomResize += ShagLeftBottomResize;
						}
						else
						{


							if (ParallelCardr == false)
							{

								string FI = fi.ToString().Replace(",", ".");
								string R = r.ToString().Replace(",", ".");
								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("fi", FI);
								parser.Values.Add("r", R);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionPolyarStatic}") + MaxZnatLine;
							}
							else
							{
								string FI = fi.ToString().Replace(",", ".");
								string R = r.ToString().Replace(",", ".");
								string T = ParamFunction.ToString().Replace(",", ".");

								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("fi", FI);
								parser.Values.Add("r", R);
								parser.Values.Add("t", T);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionPolyarDinamic}") + MaxZnatLine;
							}

							ShagXLeftBottomResize += ShagLeftBottomResize;
						}
					}
					catch
					{
						foreach (Thread parallel in ParallelFuncThread)
							parallel.Abort();
						return;
					}

				}
				ShagXLeftBottomResize = 0;
				ShagYLeftBottomResize += ShagLeftBottomResize;

			}
		}
		public void GetFunctionRightBottomResizePolyar()
		{
			double ShagRightBottomResize = SizeAxis / (SizeGrid / 2);
			double ShagXRightBottomResize = ShagRightBottomResize * ((SizeGrid / 2) + 1);
			double ShagYRightBottomResize = ShagRightBottomResize * ((SizeGrid / 2) + 1);

			for (int i = (SizeGrid / 2) + 1; i < SizeGrid + 1; i++)
			{
				for (int j = (SizeGrid / 2) + 1; j < SizeGrid + 1; j++)
				{                //8
					double x = (SizeAxis * -1) + ShagXRightBottomResize;
					//8
					double y = SizeAxis - ShagYRightBottomResize;

					double r = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
					double fi = Math.Atan2(y, x);
					try
					{

						if (MinOriginal >= 0)
						{

							if (ParallelCardr == false)
							{

								string FI = fi.ToString().Replace(",", ".");
								string R = r.ToString().Replace(",", ".");
								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("fi", FI);
								parser.Values.Add("r", R);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionPolyarStatic}");
							}
							else
							{
								string FI = fi.ToString().Replace(",", ".");
								string R = r.ToString().Replace(",", ".");
								string T = ParamFunction.ToString().Replace(",", ".");

								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("fi", FI);
								parser.Values.Add("r", R);
								parser.Values.Add("t", T);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionPolyarDinamic}");
							}
							ShagXRightBottomResize += ShagRightBottomResize;
						}
						else
						{

							if (ParallelCardr == false)
							{

								string FI = fi.ToString().Replace(",", ".");
								string R = r.ToString().Replace(",", ".");
								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("fi", FI);
								parser.Values.Add("r", R);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionPolyarStatic}") + MaxZnatLine;
							}
							else
							{
								string FI = fi.ToString().Replace(",", ".");
								string R = r.ToString().Replace(",", ".");
								string T = ParamFunction.ToString().Replace(",", ".");

								ExpressionParser parser = new ExpressionParser();
								parser.Values.Add("fi", FI);
								parser.Values.Add("r", R);
								parser.Values.Add("t", T);
								GridCoordinats[i, j] = parser.Parse($"{ZFunctionPolyarDinamic}") + MaxZnatLine;
							}

							ShagXRightBottomResize += ShagRightBottomResize;
						}
					}
					catch
					{
						foreach (Thread parallel in ParallelFuncThread)
							parallel.Abort();
						return;

					}

				}
				ShagXRightBottomResize = ShagRightBottomResize * ((SizeGrid / 2) + 1);
				ShagYRightBottomResize += ShagRightBottomResize;

			}
		}

		#endregion

	}
}