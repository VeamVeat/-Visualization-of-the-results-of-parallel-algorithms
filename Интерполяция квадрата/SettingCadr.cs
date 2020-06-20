using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Интерполяция_квадрата
{
	public partial class SettingCadr : Form
	{
		public Graph2D graph2D;
		public int Flag = 0;

		public SettingCadr()
		{
			Get();
			graph2D = new Graph2D();
			InitializeComponent();
		}
		
		Point moveStart;
		private void panel1_MouseDown( object sender, MouseEventArgs e )
		{
			if (e.Button == MouseButtons.Left)
			{
				moveStart = new Point(e.X, e.Y);
			}

		}

		private void panel1_MouseMove( object sender, MouseEventArgs e )
		{
			if ((e.Button & MouseButtons.Left) != 0)
			{

				Point deltaPos = new Point(e.X - moveStart.X, e.Y - moveStart.Y);

				this.Location = new Point(this.Location.X + deltaPos.X,
				this.Location.Y + deltaPos.Y);
			}
		}

		private void button1_Click( object sender, EventArgs e )
		{
			Block();
			this.Close();
		}
		public void OpenF() { Flag = 1; }
		public void Block() { Flag = 0; }

		private void label74_Click( object sender, EventArgs e )
		{

		}

		private void pictureBox2_Click( object sender, EventArgs e )
		{

			OpenF();
			this.Close();
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

		

		private void button2_Click_1( object sender, EventArgs e )
		{
			OpenF();
			this.Close();
		}
	}
}