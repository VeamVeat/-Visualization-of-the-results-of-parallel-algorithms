using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Интерполяция_квадрата
{
	public partial class GetFunction : Form
	{
		public string ZFunctionDecartStatic { get; set; }
		public string ZFunctionPolyarStatic { get; set; }
		public string ZFunctionDecartDinamic { get; set; }
		public string ZFunctionPolyarDinamic { get; set; }
		private double Zfunction { get; set; }
		//private Function At;
		//private Expression el1;

		//private string FunctionTxt { get; set; }


		public GetFunction( string ZFunctionDecartStatic, string ZFunctionPolyarStatic, string ZFunctionDecartDinamic, string ZFunctionPolyarDinamic )
		{
			Get();
			InitializeComponent();
			ZFunctionTxt.Text = ZFunctionDecartStatic;
			textBox2.Text = ZFunctionPolyarStatic;
			ZFunctionDinamicTxb.Text = ZFunctionDecartDinamic;
			textBox1.Text = ZFunctionPolyarDinamic;


		}

		Point moveStart;

		private void panel1_MouseDown_1( object sender, MouseEventArgs e )
		{
			if (e.Button == MouseButtons.Left)
			{
				moveStart = new Point(e.X, e.Y);
			}
		}

		private void panel1_MouseMove_1( object sender, MouseEventArgs e )
		{
			if ((e.Button & MouseButtons.Left) != 0)
			{

				Point deltaPos = new Point(e.X - moveStart.X, e.Y - moveStart.Y);

				this.Location = new Point(this.Location.X + deltaPos.X,
				this.Location.Y + deltaPos.Y);
			}
		}

		//public double ZnathFunctionDecartStatic(double x, double y )
		//{
		//	//ZFunctionDecartStatic = ZFunctionTxt.Text;
		//	At = new Function("At(x,y) = " + ZFunctionDecartStatic);
		//	 el1 = new Expression($"At({x},{y})", At);
		//	Zfunction = el1.calculate();

		//	return Zfunction;

		//}

		//public double ZnathfunctionPolyarStatic(double fi, double r )
		//{
		//	//ZFunctionPolyarStatic = textBox2.Text;
		//	At = new Function("At(fi,r) = " + ZFunctionPolyarStatic);
		//	el1 = new Expression($"At({fi},{r})", At);
		//	Zfunction = el1.calculate();

		//	return Zfunction;
		//}

		//public double ZnathFunctionDecartDinamic(double x, double y , double t )
		//{
		//	//ZFunctionDecartDinamic = ZFunctionDinamicTxb.Text;
		//	At = new Function("At(x,y,t) = " + ZFunctionDecartDinamic);
		//	el1 = new Expression($"At({x},{y},{t})", At);
		//	Zfunction = el1.calculate();

		//	return Zfunction;
		//}

		//public double ZnathfunctionPolyarDinamic(double fi , double r , double t )
		//{
		//	//ZFunctionPolyarDinamic = textBox1.Text;
		//	At = new Function("At(fi,r,t) = " + ZFunctionPolyarDinamic);
		//	el1 = new Expression($"At({fi},{r},{t})", At);
		//	Zfunction = el1.calculate();

		//	return Zfunction;
		//}

		private void button1_Click( object sender, EventArgs e )
		{
			ZFunctionDecartStatic = ZFunctionTxt.Text;
			ZFunctionPolyarStatic = textBox2.Text;
			ZFunctionDecartDinamic = ZFunctionDinamicTxb.Text;
			ZFunctionPolyarDinamic = textBox1.Text;

			this.Close();
		}

		private void pictureBox2_Click( object sender, EventArgs e )
		{

			ZFunctionDecartStatic = ZFunctionTxt.Text;
			ZFunctionPolyarStatic = textBox2.Text;
			ZFunctionDecartDinamic = ZFunctionDinamicTxb.Text;
			ZFunctionPolyarDinamic = textBox1.Text;

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

		private void InformationDecartBtn_Click( object sender, EventArgs e )
		{
			DekardInformation dekardInformation = new DekardInformation();
			dekardInformation.ShowDialog();
		}
	}
}
