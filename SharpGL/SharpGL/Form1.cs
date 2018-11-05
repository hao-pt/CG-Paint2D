using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpGL
{
	public partial class Form1 : Form
	{
		Color colorUserColor; // Bien mau de ve hinh
		short shShape; // 0 neu muon ve duong thang, 1 neu duong tron, ...

		Point pStart, pEnd; // Toa do diem dau va diem cuoi
							// Point thuoc lop System.Drawing

		public Form1()
		{
			InitializeComponent();
			colorUserColor = Color.White; // Gia tri mac dinh la mau trang
			shShape = 0; // Mac dinh ve duong thang
		}

		private void openGLControl1_Load(object sender, EventArgs e)
		{

		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void bt_Palette_Click(object sender, EventArgs e)
		{
			// Goi hop thoai chon mau
			if (colorDialog1.ShowDialog() == DialogResult.OK)
				colorUserColor = colorDialog1.Color; // Luu lai mau user chon
		}

		// Nguoi dung chon chuc nang ve duong thang
		private void bt_Line_Click(object sender, EventArgs e)
		{
			shShape = 0; // Nguoi dung chon ve duong thang
		}

		// Cap nhat toa do diem cuoi khi nguoi dung keo chuot di
		private void ctrl_OpenGLControl_MouseUp(object sender, MouseEventArgs e)
		{
			pEnd = e.Location; // Lay toa do diem cuoi
		}

		private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
		{
			// get the openGL object
			OpenGL gl = openGLControl.OpenGL;
			// set the clear color: dat mau nen
			// alpha: do trong suot
			gl.ClearColor(0, 0, 0, 0);
			// set the projection matrix
			// Xet ma tran phep chieu
			// 2D: chỉ quan tam projection matrix
			gl.MatrixMode(OpenGL.GL_PROJECTION);
			// load the identify
			// Xét ma trận hiện hành là ma trận đơn vị
			gl.LoadIdentity();
		}

		// Ham ve doan thang
		private void drawLine(OpenGL gl) {
			gl.Begin(OpenGL.GL_LINES);
			gl.Vertex(pStart.X, gl.RenderContextProvider.Height - pStart.Y);
			gl.Vertex(pEnd.X, gl.RenderContextProvider.Height - pEnd.Y);
			gl.End();
			gl.Flush();
		}

		// Cac ham ve khac ...

		private void openGLControl_OpenGLDraw(object sender, RenderEventArgs args)
		{
			// get the OpenGL object
			OpenGL gl = openGLControl.OpenGL;
			// clear the color and depth buffer
			// Xóa màn hình
			gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

			// Chon mau
			gl.Color(colorUserColor.R / 255.0, colorUserColor.G / 255.0, colorUserColor.B / 255.0, 0);

			// Ve voi cho nay
			// ...
			switch (shShape)
			{
				case 0:
					// Ve doan thang
					drawLine(gl);
					break;
				case 1:
					// Ve duong tron

					break;
				// case 2..n
			}

			#region ViDuVeTamGiac
			/* 
			
			gl.Begin(OpenGL.GL_TRIANGLES); // Ve tam giac
			gl.Vertex2sv(new short[] { 0, 0 }); // Dinh A(0, 0)
			gl.Vertex2sv(new short[] { 200, 200 }); // Dinh B(100, 100)
			gl.Vertex2sv(new short[] { 500, 0 }); // Dinh C(200, 0)
			gl.End(); // Kết thúc
			gl.Flush(); // Thuc hien ve ngay thay vi phai doi sau 1 thoi gian
						// Bản chất khi vẽ thì nó vẽ lên vùng nhớ Buffer
						// Do đó cần dùng hàm Flush để đẩy vùng nhớ Buffer này lên màn hình
			*/
			#endregion
		}


		private void openGLControl_Resized(object sender, EventArgs e)
		{
			// get the OpenGL object
			OpenGL gl = openGLControl.OpenGL;
			// set the projection matrix
			gl.MatrixMode(OpenGL.GL_PROJECTION);
			// load the identify
			gl.LoadIdentity();
			// Create a perspective transformation
			gl.Viewport(0, 0, openGLControl.Width, openGLControl.Height); // Xét cái màn hình: Vẽ toàn bộ cái khung của OpenGL control

			// Hàm set up cái phép chiếu trực giao
			// Ở đây chính là cái size của khung OpenGL control
			gl.Ortho2D(0, openGLControl.Width, 0, openGLControl.Height);
		}

		// Cap nhat diem dau
		private void ctrl_OpenGLControl_MouseDown(object sender, MouseEventArgs e)
		{
			// Cap nhat toa do diem dau
			pStart = e.Location; // e la tham so lien quan den su kien chon diem
			pEnd = pStart; // Mac dinh pEnd = pStart
		}

	}
}
