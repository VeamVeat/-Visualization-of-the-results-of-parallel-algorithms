using System;
using System.Drawing;
using System.Windows.Forms;

namespace Интерполяция_квадрата
{
	class Curcle
	{
		private Point point { get; set; } // координаты 1  точки
		private Point pointer { get; set; } // координаты  2 точки
		private PointF pointCoordinatStart { get; set; } // координаты 1  точки
		private PointF pointCoordinatEnd { get; set; } // координаты  2 точки
		private Graphics graph { get; set; } // поверхность для рисования
		private Rectangle recth { get; set; }  // 4 чисоа определяющиее расположение и размер фигуры
		private Pen pen1 { get; set; }  // наш карандаш
		private SolidBrush solidBrush { get; set; } // заливка
		private float startAngle { get; set; } = 0.0f;
		private float sweepAngle { get; set; } = 360.0f;
		public Random random { get; set; } = new Random();

		public Curcle( int x, int y, Color color, Graphics graphics )
		{
			graph = graphics;
			solidBrush = new SolidBrush(color);
			pen1 = new Pen(solidBrush, 1);
			point = new Point(x, y);
			recth = new Rectangle(x, y, 2, 2);

		}

		public Curcle( Point p, Point h, Color color, Graphics graphics )
		{
			graph = graphics;
			solidBrush = new SolidBrush(color);
			pen1 = new Pen(solidBrush, 1);
			point = p;
			pointer = h;

		}
		public Curcle( PointF p, PointF h, Color color, Graphics graphics )
		{
			graph = graphics;
			solidBrush = new SolidBrush(color);
			pen1 = new Pen(solidBrush, 1);
			pointCoordinatStart = p;
			pointCoordinatEnd = h;

		}

		public void LineDisplay()
		{
			try
			{

				graph.DrawLine(pen1, point, pointer);
			}
			catch
			{

				//MessageBox.Show("Произошла техническая проблема возможно не связанная с действиями пользователя", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);

			}

		}
		public void LineDisplayF()
		{
			try
			{

				graph.DrawLine(pen1, pointCoordinatStart, pointCoordinatEnd);
			}
			catch
			{

				//MessageBox.Show("Произошла техническая проблема возможно не связанная с действиями пользователя", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);

			}

		}

		public void CurcleDisplay()
		{
			graph.FillPie(solidBrush, recth, startAngle, sweepAngle);
			//graph.DrawEllipse();
		}


	}
}
