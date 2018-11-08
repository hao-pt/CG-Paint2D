using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace SharpGL
{
	// Kieu enum cho nut chon mau
	public enum ButtonColor {
		LEFT,
		RIGHT
	}

	// Kieu enum cho nut chon hinh ve
	public enum ShapeMode {
		LINE,
		CIRCLE,
		RECTANGLE,
		ELLIPSE,
		TRIANGLE,
		PENTAGON,
		HEXAGON,
		FLOOD_FILL
	}

	public partial class Form1 : Form
	{
		Color colorUserColor; // Bien mau de ve hinh
		ShapeMode shShape; // 0 neu muon ve duong thang, 1 neu duong tron, ...

		Point pStart, pEnd; // Toa do diem dau va diem cuoi
							// Point thuoc lop System.Drawing
		int isDown; // Bien kiem soat con tro chuot co dang duoc giu khong
		int currentSize; // Kich co ve hien tai

		ButtonColor currentButtonColor; // Nut chon mau hien tai

		// De repaint duoc thi can mot doi tuong Bitmap de luu tru lại tat cac nhung gi user ve
		// Ý tưởng: Khi người dùng mouse up (dứt chuột) thì cùng lúc đó ta vẽ lên bitmap bằng Graphics
		Bitmap bm; // Dung de luu lai tat ca nhung gi nguoi dung ve tren OpenGLControll
		Graphics gr; // Dung doi tuong Graphics de ve len Bitmap

		public Form1()
		{
			InitializeComponent();
			colorUserColor = Color.White; // Gia tri mac dinh la mau trang
			currentButtonColor = ButtonColor.LEFT; // Mac dinh la nut ben trai
			shShape = ShapeMode.LINE; // Mac dinh ve duong thang
			cBox_Choose_Size.SelectedIndex = 0; // Mac dinh net ve hien thi la 1


			// Cap phat vung nho cho Bitmap
			bm = new Bitmap(this.Width, this.Height); // kich thuoc bitmap bang voi form1
			gr = Graphics.FromImage(bm); // Truyen doi tuong Bitmap vao de ve
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
			{
				// Neu nguoi dung chon mau tai button trai thi cap nhat back color cho nut do
				// va nguoc lai
				if (currentButtonColor == ButtonColor.LEFT)
					bt_Left_Color.BackColor = colorDialog1.Color;
				else
					bt_Right_Color.BackColor = colorDialog1.Color;

				colorUserColor = colorDialog1.Color; // Luu lai mau user chon
			}
		}

		// Nguoi dung chon chuc nang ve duong thang
		private void bt_Line_Click(object sender, EventArgs e)
		{
			shShape = ShapeMode.LINE; // Nguoi dung chon ve duong thang
		}
		
		// Nguoi dung chon chuc nang ve hinh chu nhat
		private void bt_Rec_Click(object sender, EventArgs e)
		{
			shShape = ShapeMode.RECTANGLE;
		}

		// Nguoi dung chon chuc nang ve tam giac deu
		private void bt_Triangle_Click(object sender, EventArgs e)
		{
			shShape = ShapeMode.TRIANGLE;
		}
		// Bat su kien nguoi dung ve ngu giac deu
		private void bt_Pentagon_Click(object sender, EventArgs e)
		{
			shShape = ShapeMode.PENTAGON;
		}

		// Bat su kien nguoi dung ve luc giac deu
		private void bt_Hexagon_Click(object sender, EventArgs e)
		{
			shShape = ShapeMode.HEXAGON;
		}

		// Bat su kien nguoi dung ve duong tron
		private void bt_Circle_Click(object sender, EventArgs e)
		{
			shShape = ShapeMode.CIRCLE;
		}

		// Bat su kien nguoi dung ve ellipse
		private void bt_Ellipse_Click(object sender, EventArgs e)
		{
			shShape = ShapeMode.ELLIPSE;
		}

		// Ham khoi tao cho opengl
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
			gl.Enable(OpenGL.GL_LINE_SMOOTH); // Lam tron cac diem ve, cho duong thang muot hon
			gl.Begin(OpenGL.GL_LINES);
			gl.Vertex(pStart.X, gl.RenderContextProvider.Height - pStart.Y);
			gl.Vertex(pEnd.X, gl.RenderContextProvider.Height - pEnd.Y);
			gl.End();
			gl.Flush();
			gl.Disable(OpenGL.GL_LINE_SMOOTH);
		}

		// Ham tinh khoang cach giua pStart va pEnd
		private void calculateDistance(out double d) {
			d = Math.Sqrt(Math.Pow(pStart.X - pEnd.X, 2) + Math.Pow(pStart.Y - pEnd.Y, 2));
		}

		private void put8Pixel(OpenGL gl, int a, int b, int x, int y) {
			gl.PointSize(currentSize);
			gl.Begin(OpenGL.GL_POINTS);
			gl.Vertex(a + x, b + y);
			gl.Vertex(a + x, b - y);
			gl.Vertex(a - x, b + y);
			gl.Vertex(a - x, b - y);
			gl.Vertex(a + y, b + x);
			gl.Vertex(a - y, b + x);
			gl.Vertex(a + y, b - x);
			gl.Vertex(a - y, b - x);
			gl.End();
			gl.Flush();

		}

		// Ham ve hinh tron
		private void drawCircle(OpenGL gl)
		{
			#region Cach 1: Su dung luong giac
			/*
			// Y tuong: Ve duong tron bang cach chia duong tron thanh cac segments 
			// (segments cac lon thi duong cong se cang muot)
			// Ta se chay Goc alpha tu 0 - 360 độ va alpha += 360 / totalSegments cho mỗi lần duyệt
			// Lưu ý: goc alpha đươc tính theo radian nên cần phải đổi sang radian:
			//		alpha_rad = alpha * 2*PI / 360 = alpha * PI/180
			// Toa độ x, y mỗi lần duyệt bằng: x = r*cos(alpha_rad), y = r*sin(alpha_rad)

			const int totalSegments = 90; // số lượng các segments
			// Ban kinh la đoặn thẳng nối từ pStart đến pEnd
			double r;
			calculateDistance(out r);

			// Bat dau ve
			gl.Enable(OpenGL.GL_LINE_SMOOTH);
			gl.Begin(OpenGL.GL_LINE_LOOP);


			for (int alpha = 0; alpha < 360; alpha += 360 / totalSegments) {
				// Đổi về radian
				double alpha_rad = alpha * Math.PI / 180;
				// Tinh x, y
				gl.Vertex(pEnd.X + r * Math.Cos(alpha_rad), pEnd.Y + r * Math.Sin(alpha_rad));
			}

			gl.End();
			gl.Flush();
			gl.Disable(OpenGL.GL_LINE_SMOOTH);
			*/
			#endregion
			#region Cach 2: Su dung thuat toan MidPoint
			// Ban kinh la 1 nửa của đường chéo hình vuông, tức là 1 nửa của pStart và pEnd
			double r;
			calculateDistance(out r);
			r /= 2;

			// Tam duong tron tai trung diem cua doan thang noi pStart và pEnd
			int xc = (pStart.X + pEnd.X) / 2;
			int yc = (pStart.Y + pEnd.Y) / 2;

			// Giả sử xét tâm tại 0
			int x = 0;
			int y = (int)r;
			int p = (int)(5/4 - r);

			// Ve diem  dau (0, r)
			put8Pixel(gl, xc, yc, x, y);

			while (x < y) {
				x++;
				if (p < 0)
					p += 2 * x + 3; 
				else {
					y--;
					p += 2 * (x - y) + 5;
				}
				put8Pixel(gl, xc, yc, x, y);
			}
			#endregion
		}

		// Ham ve hinh chu nhat
		private void drawRec(OpenGL gl) {
			gl.Enable(OpenGL.GL_LINE_SMOOTH); // Lam tron cac diem ve, cho duong thang muot hon
			gl.Begin(OpenGL.GL_LINE_LOOP);
			// Toa do diem dau (x1, y1)
			// Toa do diem cuoi (x2, y2)
			gl.Vertex(pStart.X, gl.RenderContextProvider.Height - pStart.Y);
			// Toa do diem 2 (x2, y1)
			gl.Vertex(pEnd.X, gl.RenderContextProvider.Height - pStart.Y);
			// Toa do diem 3 (x2, y2)
			gl.Vertex(pEnd.X, gl.RenderContextProvider.Height - pEnd.Y);
			// Toa do diem 4 (x1, y2)
			gl.Vertex(pStart.X, gl.RenderContextProvider.Height - pEnd.Y);
			gl.End();
			gl.Flush();
			gl.Disable(OpenGL.GL_LINE_SMOOTH);
		}

		// Ham ve tam giac
		private void drawTriangle(OpenGL gl) {
			gl.Enable(OpenGL.GL_LINE_SMOOTH); // Lam tron cac diem ve, cho duong thang muot hon
			gl.Begin(OpenGL.GL_LINE_LOOP); // Ve tam giac
			gl.Vertex(pStart.X, gl.RenderContextProvider.Height - pStart.Y); // Dinh A(x1, y1)
			gl.Vertex(pEnd.X, gl.RenderContextProvider.Height - pEnd.Y); // Dinh B(x2, y2)

			if (pEnd.X < pStart.X) // Neu nguoi dung keo chuot qua trai
				gl.Vertex(pStart.X + Math.Abs(pStart.X - pEnd.X), gl.RenderContextProvider.Height - pEnd.Y);
			else // Neu nguoi dung keo chuot qua phai
				gl.Vertex(pStart.X - Math.Abs(pStart.X - pEnd.X), gl.RenderContextProvider.Height - pEnd.Y);
																		// Dinh C(x1 - abs(x2 - x1), y2)
			gl.End(); // Kết thúc
			gl.Flush(); // Thuc hien ve ngay thay vi phai doi sau 1 thoi gian
						// Bản chất khi vẽ thì nó vẽ lên vùng nhớ Buffer
						// Do đó cần dùng hàm Flush để đẩy vùng nhớ Buffer này lên màn hình
			gl.Disable(OpenGL.GL_LINE_SMOOTH);
		}

		private void drawPentagon(OpenGL gl) {
			#region Cach 1: Dung luong giac
			/*
			// Ý tưởng: ngũ giác đều chia đường tròn thành 5 đoạn. Mỗi đoạn cách nhau 72 độ
			// Làm tương tự như thuật toán vẽ đường tròn theo cách lượng giác
			const int totalSegments = 5; // số lượng các segments
			// Ban kinh la đoặn thẳng nối từ pStart đến pEnd
			double r;
			calculateDistance(out r);

			// Bat dau ve
			gl.Enable(OpenGL.GL_LINE_SMOOTH);
			gl.Begin(OpenGL.GL_LINE_LOOP);

			for (int alpha = 0; alpha < 360; alpha += 360 / totalSegments)
			{
				// Đổi về radian
				double alpha_rad = alpha * Math.PI / 180;
				// Tinh x, y
				gl.Vertex(pStart.X + r * Math.Cos(alpha_rad), pStart.Y + r * Math.Sin(alpha_rad));
			}

			gl.End();
			gl.Flush();
			gl.Disable(OpenGL.GL_LINE_SMOOTH);
			*/

			#endregion
			#region Cach 2: Dung phep quay diem
			// Ý tưởng: Các đỉnh của ngũ giác đều quay 1 goc alpha = 72*PI/180 độ (đổi về radian)
			// B1: Gán pStart là tâm
			// B2: Quay pEnd theo công thức
			//	x' = x*cos(alpha) - sin(alpha)*y
			//	y' = x*sin(alpha) + y*cos(alpha)
			const int totalSegments = 5; // số lượng các segments

			// Ban kinh bằng 1 nửa của đoạn thẳng pStart, pEnd
			double r;
			calculateDistance(out r);
			r /= 2;

			// Tam duong tron tai trung diem cua doan thang noi pStart và pEnd
			int xc = (pStart.X + pEnd.X) / 2;
			int yc = (pStart.Y + pEnd.Y) / 2;

			int x = 0;
			int y = (int)r;

			// Bat dau ve
			gl.Enable(OpenGL.GL_LINE_SMOOTH);
			gl.Begin(OpenGL.GL_LINE_LOOP);

			for (int alpha = 0; alpha < 360; alpha += 360 / totalSegments)
			{
				// Đổi về radian
				double alpha_rad = alpha * Math.PI / 180;
				// Tinh x, y
				gl.Vertex(xc + x * Math.Cos(alpha_rad) - y * Math.Sin(alpha_rad)
					, yc + x * Math.Sin(alpha_rad) + y * Math.Cos(alpha_rad));
			}

			gl.End();
			gl.Flush();
			gl.Disable(OpenGL.GL_LINE_SMOOTH);
			#endregion
		}
		private void drawHexagon(OpenGL gl) {
			// Ý tưởng: Các đỉnh của ngũ giác đều quay 1 goc alpha = 60*PI/180 độ (đổi về radian)
			// B1: Gán pStart là tâm
			// B2: Quay pEnd theo công thức
			//	x' = x*cos(alpha) - sin(alpha)*y
			//	y' = x*sin(alpha) + y*cos(alpha)
			const int totalSegments = 6; // số lượng các segments
			// Ban kinh bằng 1 nửa của đoạn thẳng đi qua pStart, pEnd
			double r;
			calculateDistance(out r);
			r /= 2;

			// Tam duong tron tai trung diem cua doan thang noi pStart và pEnd
			int xc = (pStart.X + pEnd.X) / 2;
			int yc = (pStart.Y + pEnd.Y) / 2;

			int x = 0;
			int y = (int)r;

			// Bat dau ve
			gl.Enable(OpenGL.GL_LINE_SMOOTH);
			gl.Begin(OpenGL.GL_LINE_LOOP);

			for (int alpha = 0; alpha < 360; alpha += 360 / totalSegments)
			{
				// Đổi về radian
				double alpha_rad = alpha * Math.PI / 180;
				// Tinh x, y
				gl.Vertex(xc + x * Math.Cos(alpha_rad) - y * Math.Sin(alpha_rad)
					, yc + x * Math.Sin(alpha_rad) + y * Math.Cos(alpha_rad));
			}

			gl.End();
			gl.Flush();
			gl.Disable(OpenGL.GL_LINE_SMOOTH);
		}

		// Cac ham ve khac ...

		private void openGLControl_OpenGLDraw(object sender, RenderEventArgs args)
		{
			if (isDown == 1) // Neu nguoi dung dang Mouse down thi moi ve
			{
				// get the OpenGL object
				OpenGL gl = openGLControl.OpenGL;
				// clear the color and depth buffer
				// Xóa màn hình
				gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

				// Chon mau
				gl.Color(colorUserColor.R / 255.0, colorUserColor.G / 255.0, colorUserColor.B / 255.0, 0);

				// Thiet lap size cua net ve
				gl.LineWidth(currentSize);

				// Stopwatch ho tro do thoi gian
				Stopwatch myTimer = new Stopwatch();
				myTimer.Start(); // bat dau do
				
				// Ve voi cho nay
				// ...
				switch (shShape)
				{
					case ShapeMode.LINE:
						// Ve doan thang
						drawLine(gl);
						break;
					case ShapeMode.CIRCLE:
						// Ve duong tron
						drawCircle(gl);
						break;
					case ShapeMode.RECTANGLE:
						// Ve hinh chu nhat
						drawRec(gl);
						break;
					case ShapeMode.ELLIPSE:
						// Ve ellipse
						break;
					case ShapeMode.TRIANGLE:
						// Ve tam giac deu
						drawTriangle(gl);
						break;
					case ShapeMode.PENTAGON:
						// Ve ngu giac deu
						drawPentagon(gl);
						break;
					case ShapeMode.HEXAGON:
						// Ve luc giac deu
						drawHexagon(gl);
						break;
					case ShapeMode.FLOOD_FILL:
						// To mau bang thuat toan flood fill
						floodFill(pStart.X, pStart.Y);
						break;
				}

				myTimer.Stop(); // ket thuc do
				TimeSpan Time = myTimer.Elapsed; // Lay thoi gian troi qua
				tb_Time.Text = String.Format("{0} (sec)", Time.TotalSeconds); // In ra tb_Time
			}
			
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

		// Ham getPixelColor
		private void getPixelColor(Point p, out Byte[] color) {
			OpenGL gl = openGLControl.OpenGL;
			color = new Byte[3];
			
			gl.ReadPixels(p.X, p.Y, 1, 1, OpenGL.GL_RGB, OpenGL.GL_FLOAT, color);
		}

		// Ham set pixel color
		private void setPixelColor(Point p) {
			OpenGL gl = openGLControl.OpenGL;
			// set mau
			gl.Color(colorUserColor.R / 255.0, colorUserColor.G / 255.0, colorUserColor.B / 255.0, 0);
			gl.Begin(OpenGL.GL_POINTS);
			gl.Vertex(p.X, p.Y);
			gl.End();
			gl.Flush();
		}

		private void floodFill(int x, int y) {
			Byte[] color;
			getPixelColor(pStart, out color);

			// Thuat toan to mau theo vet loang
			// Neu color cua pixel khac bien va chua to mau
			if (color[0] == 0 && color[1] == 0 && color[2] == 0)
			{
				setPixelColor(pStart);
				floodFill(x + 1, y);
				floodFill(x - 1, y);
				floodFill(x, y + 1);
				floodFill(x, y - 1);
			}
		}

		// Ham xu ly su kien to mau theo vet loang
		private void bt_Flood_Fill_Click(object sender, EventArgs e)
		{
			shShape = ShapeMode.FLOOD_FILL;
		}

		// Khi nguoi dung click button chon mau ben trai
		private void bt_Left_Color_Click(object sender, EventArgs e)
		{
			currentButtonColor = ButtonColor.LEFT;
			colorUserColor = bt_Left_Color.BackColor; // cap nhat mau hien tai
		}

		// Khi nguoi dung click button chon mau ben phai
		private void bt_Right_Color_Click(object sender, EventArgs e)
		{
			currentButtonColor = ButtonColor.RIGHT;
			colorUserColor = bt_Right_Color.BackColor; // Cap nhat mau  hien tai 
													   //khi nguoi dung click button phai
		}

		// Cap nhat diem cuoi khi nguoi dung dang keo chuot
		private void ctrl_OpenGLControl_MouseMove(object sender, MouseEventArgs e)
		{
			// Neu chuot dang di chuyen thi moi cap nhat diem pEnd
			if (isDown == 1)
			{
				// Cap nhat diem cuoi
				pEnd = new Point(e.Location.X, e.Location.Y);
				// In toa do khi di chuyen chuot 
				lb_Coor.Text = e.X.ToString() + ", " + e.Y.ToString();
			}
			
		}

		// Cap nhat toa do diem cuoi khi nguoi dung buong chuot ra
		private void ctrl_OpenGLControl_MouseUp(object sender, MouseEventArgs e)
		{
			openGLControl.Cursor = Cursors.Default; // Tra ve con tro chuot nhu cu
			isDown = 0; // chuot het di chuyen

			pStart.X = pStart.Y = 0;
			pEnd.X = pEnd.Y = 0;

			//// Ve len bitmap
			//Pen pen = new Pen(colorUserColor);
			//gr.DrawLine(pen, pStart, pEnd);
			//this.BackgroundImage = (Bitmap)bm.Clone(); // Set lai background
		}

		private void cBox_Choose_Size_SelectedIndexChanged(object sender, EventArgs e)
		{
			currentSize = int.Parse(cBox_Choose_Size.Text);
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit(); // Tắt chuong trinh
		}

		// Cap nhat diem dau khi nguoi dung bat dau giu chuot
		private void ctrl_OpenGLControl_MouseDown(object sender, MouseEventArgs e)
		{
			// Cap nhat toa do diem dau
			pStart = new Point(e.Location.X, e.Location.Y); // e la tham so lien quan den su kien chon diem
			pEnd = new Point(e.X, e.Y); // Mac dinh pEnd = pStart
			openGLControl.Cursor = Cursors.Cross; // Thay doi hinh dang con tro chuot khi ve
			isDown = 1; // Chuot dang bat dau di chuyen
		}

	}
}
