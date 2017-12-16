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
using System.Threading;

namespace pcbang
{
	public partial class Form1 : Form
	{
		public int drawMode;    //그리기 모드
		private Point startPoint;   //시작 점
		private Point nowPoint; //현재 점
		private Pen myPen;      //펜
		private ArrayList saveData; //그림 객체 정보 저장

        // 채팅 처리를 전담하는 Network 클래스 객체 변수 선언
        private Network net = null;
        private Thread server_th = null; // 채팅 서버 스레드 선언

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

            net = new Network(this); // 네트워크 클래스 객체 생성
        }
		//뭐령,,
		private void Form1_Load(object sender, EventArgs e)
		{
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			ResizeRedraw = true;

            // 채팅 서버를 시작시키는 스레드를 생성
            server_th = new Thread(new ThreadStart(net.ServerStart));
            server_th.Start(); // 채팅 서버 시작
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (btn_Connect.Text == "접속중...")
                { //클라이언트로 수행 중
                    net.DisConnect(); //채팅 서버와 연결되어 있으면 연결 끊기
                }
                else
                { //서버로 수행 중
                    net.ServerStop(); //채팅 서버 실행 중지
                    if (server_th.IsAlive) server_th.Abort(); //ServerStart 스레드를 종료
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); //예외 메시지 출력
            }
        }
    }
}
