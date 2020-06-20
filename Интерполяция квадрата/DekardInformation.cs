using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Интерполяция_квадрата
{
	public partial class DekardInformation : Form
	{
		public DekardInformation()
		{
			InitializeComponent();
			Get();
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

		private void pictureBox2_Click( object sender, EventArgs e )
		{
			this.Close();
		}

		private void button1_Click( object sender, EventArgs e )
		{
			this.Close();
		}
	}
}
