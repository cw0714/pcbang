using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pcbang
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
            label1.Text = label2.Text = label3.Text = label4.Text = label5.Text = label6.Text = "유저 없음";
            textBox1.Focus();
        }
	}
}
