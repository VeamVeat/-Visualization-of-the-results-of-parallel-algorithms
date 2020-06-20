namespace Интерполяция_квадрата
{
	partial class SettingCadr
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.panel1 = new System.Windows.Forms.Panel();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textBoxT = new System.Windows.Forms.TextBox();
			this.label74 = new System.Windows.Forms.Label();
			this.textBoxShagT = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.checkBoxParallel = new System.Windows.Forms.CheckBox();
			this.bunifuElipse1 = new Bunifu.Framework.UI.BunifuElipse(this.components);
			this.bunifuElipse2 = new Bunifu.Framework.UI.BunifuElipse(this.components);
			this.bunifuElipse3 = new Bunifu.Framework.UI.BunifuElipse(this.components);
			this.t = new Bunifu.Framework.UI.BunifuElipse(this.components);
			this.shagT = new Bunifu.Framework.UI.BunifuElipse(this.components);
			this.kolCadr = new Bunifu.Framework.UI.BunifuElipse(this.components);
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(47)))), ((int)(((byte)(52)))));
			this.panel1.Controls.Add(this.pictureBox2);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(292, 22);
			this.panel1.TabIndex = 0;
			this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
			this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
			// 
			// pictureBox2
			// 
			this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pictureBox2.Image = global::Интерполяция_квадрата.Properties.Resources.Gnome_Window_Close_32__1_;
			this.pictureBox2.Location = new System.Drawing.Point(269, 5);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(15, 15);
			this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox2.TabIndex = 2;
			this.pictureBox2.TabStop = false;
			this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(187)))), ((int)(((byte)(190)))));
			this.label2.Location = new System.Drawing.Point(8, 1);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(123, 20);
			this.label2.TabIndex = 110;
			this.label2.Text = "Настройка кадров";
			this.label2.Click += new System.EventHandler(this.label74_Click);
			// 
			// textBoxT
			// 
			this.textBoxT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(107)))), ((int)(((byte)(119)))));
			this.textBoxT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxT.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.textBoxT.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(187)))), ((int)(((byte)(190)))));
			this.textBoxT.Location = new System.Drawing.Point(54, 86);
			this.textBoxT.Multiline = true;
			this.textBoxT.Name = "textBoxT";
			this.textBoxT.Size = new System.Drawing.Size(83, 30);
			this.textBoxT.TabIndex = 111;
			this.textBoxT.Text = "2";
			this.textBoxT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label74
			// 
			this.label74.AutoSize = true;
			this.label74.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(188)))), ((int)(((byte)(188)))));
			this.label74.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label74.ForeColor = System.Drawing.Color.Black;
			this.label74.Location = new System.Drawing.Point(59, 58);
			this.label74.Name = "label74";
			this.label74.Size = new System.Drawing.Size(68, 18);
			this.label74.TabIndex = 110;
			this.label74.Text = "Укажите t";
			this.label74.Click += new System.EventHandler(this.label74_Click);
			// 
			// textBoxShagT
			// 
			this.textBoxShagT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(107)))), ((int)(((byte)(119)))));
			this.textBoxShagT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxShagT.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.textBoxShagT.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(187)))), ((int)(((byte)(190)))));
			this.textBoxShagT.Location = new System.Drawing.Point(159, 86);
			this.textBoxShagT.Multiline = true;
			this.textBoxShagT.Name = "textBoxShagT";
			this.textBoxShagT.Size = new System.Drawing.Size(83, 30);
			this.textBoxShagT.TabIndex = 113;
			this.textBoxShagT.Text = "0,02";
			this.textBoxShagT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(188)))), ((int)(((byte)(188)))));
			this.label4.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label4.ForeColor = System.Drawing.Color.Black;
			this.label4.Location = new System.Drawing.Point(167, 57);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(62, 18);
			this.label4.TabIndex = 112;
			this.label4.Text = "Шаг по t";
			// 
			// button1
			// 
			this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(47)))), ((int)(((byte)(52)))));
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
			this.button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(187)))), ((int)(((byte)(190)))));
			this.button1.Location = new System.Drawing.Point(19, 214);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(109, 31);
			this.button1.TabIndex = 114;
			this.button1.Text = "ОК";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(107)))), ((int)(((byte)(119)))));
			this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBox1.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(187)))), ((int)(((byte)(190)))));
			this.textBox1.Location = new System.Drawing.Point(107, 172);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(83, 30);
			this.textBox1.TabIndex = 115;
			this.textBox1.Text = "100";
			this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(188)))), ((int)(((byte)(188)))));
			this.label1.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.ForeColor = System.Drawing.Color.Black;
			this.label1.Location = new System.Drawing.Point(82, 146);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(133, 18);
			this.label1.TabIndex = 110;
			this.label1.Text = "Количество кадров ";
			this.label1.Click += new System.EventHandler(this.label74_Click);
			// 
			// button2
			// 
			this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(47)))), ((int)(((byte)(52)))));
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button2.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
			this.button2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(187)))), ((int)(((byte)(190)))));
			this.button2.Location = new System.Drawing.Point(164, 214);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(109, 31);
			this.button2.TabIndex = 116;
			this.button2.Text = "ОТМЕНА";
			this.button2.UseVisualStyleBackColor = false;
			this.button2.Click += new System.EventHandler(this.button2_Click_1);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(188)))), ((int)(((byte)(188)))));
			this.label3.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label3.ForeColor = System.Drawing.Color.Black;
			this.label3.Location = new System.Drawing.Point(105, 34);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(74, 20);
			this.label3.TabIndex = 122;
			this.label3.Text = "t - Время";
			// 
			// checkBoxParallel
			// 
			this.checkBoxParallel.AutoSize = true;
			this.checkBoxParallel.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
			this.checkBoxParallel.ForeColor = System.Drawing.Color.Coral;
			this.checkBoxParallel.Location = new System.Drawing.Point(112, 124);
			this.checkBoxParallel.Name = "checkBoxParallel";
			this.checkBoxParallel.Size = new System.Drawing.Size(73, 22);
			this.checkBoxParallel.TabIndex = 123;
			this.checkBoxParallel.Text = "Parallel";
			this.checkBoxParallel.UseVisualStyleBackColor = true;
			// 
			// bunifuElipse1
			// 
			this.bunifuElipse1.ElipseRadius = 35;
			this.bunifuElipse1.TargetControl = this;
			// 
			// bunifuElipse2
			// 
			this.bunifuElipse2.ElipseRadius = 35;
			this.bunifuElipse2.TargetControl = this.button1;
			// 
			// bunifuElipse3
			// 
			this.bunifuElipse3.ElipseRadius = 35;
			this.bunifuElipse3.TargetControl = this.button2;
			// 
			// t
			// 
			this.t.ElipseRadius = 20;
			this.t.TargetControl = this.textBoxT;
			// 
			// shagT
			// 
			this.shagT.ElipseRadius = 20;
			this.shagT.TargetControl = this.textBoxShagT;
			// 
			// kolCadr
			// 
			this.kolCadr.ElipseRadius = 20;
			this.kolCadr.TargetControl = this.textBox1;
			// 
			// SettingCadr
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(188)))), ((int)(((byte)(188)))));
			this.ClientSize = new System.Drawing.Size(292, 258);
			this.Controls.Add(this.checkBoxParallel);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.textBoxShagT);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textBoxT);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label74);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "SettingCadr";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SettingCadr";
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.Label label74;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label1;
		public System.Windows.Forms.TextBox textBoxT;
		public System.Windows.Forms.TextBox textBoxShagT;
		public System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button2;
		public System.Windows.Forms.Panel panel1;
		public System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label3;
		public System.Windows.Forms.CheckBox checkBoxParallel;
		private Bunifu.Framework.UI.BunifuElipse bunifuElipse1;
		private Bunifu.Framework.UI.BunifuElipse bunifuElipse2;
		private Bunifu.Framework.UI.BunifuElipse bunifuElipse3;
		private Bunifu.Framework.UI.BunifuElipse t;
		private Bunifu.Framework.UI.BunifuElipse shagT;
		private Bunifu.Framework.UI.BunifuElipse kolCadr;
	}
}