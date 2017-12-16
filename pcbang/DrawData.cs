/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace pcbang
{
	[Serializable]
	class DrawData
	{
		public Point startPoint;
		public Point endPoint;
		public int drawMode;
		[NonSerialized]
		public Pen pen;
		
		public DrawData(Point x, Point y, int Mode)
		{
			startPoint = x;
			endPoint = y;
			drawMode = Mode;
		}
		public void drawData(Graphics g)
		{
			pen = new Pen(Color.Black);
			Rectangle rect;

			switch (drawMode)
			{
				case 1:	//선, 원, 직사각형, 곡선
					g.DrawLine(pen, startPoint, endPoint);
					break;
				case 2:
					rect = new Rectangle(startPoint.X, startPoint.Y, endPoint.X - startPoint.X, endPoint.Y - startPoint.Y);
					break;
				case 3:
			}
		}
	}
}
*/
