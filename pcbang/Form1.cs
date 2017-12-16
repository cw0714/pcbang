using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace pcbang
{
	public partial class Form1 : Form
	{
		public int drawMode;    //그리기 모드
		private Point startPoint;   //시작 점
		private Point nowPoint; //현재 점
		private Pen myPen;      //펜
		private ArrayList saveData; //그림 객체 정보 저장\\

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			foreach (DrawData outData in saveData)
				outData.drawData(e.Graphics);
		}

		public Form1()
		{
			InitializeComponent();
            label1.Text = label2.Text = label3.Text = label4.Text = label5.Text = label6.Text = "유저 없음";
            textBox1.Focus();
        }

		private void Form1_Load(object sender, EventArgs e)
		{
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			ResizeRedraw = true;
		}
	}
}
