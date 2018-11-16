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
using System.Runtime.InteropServices; // Su dung ham chuyen doi sang IntPtr
namespace SharpGL
{
	// Kieu enum cho nut chon mau
	public enum ButtonColor
	{
		LEFT,
		RIGHT
	}

	// Kieu enum cho nut chon hinh ve
	public enum ShapeMode
	{
		LINE,
		CIRCLE,
		RECTANGLE,
		ELLIPSE,
		TRIANGLE,
		PENTAGON,
		HEXAGON,
		POLYGON,
		FLOOD_FILL
	}

	// Kiểu enum Menu cho checklistbox
	public enum Menu
	{
		DRAWING,
		TRANSLATE,
		ROTATE,
		SCALE
	}

	// Kieu MyBitMap de luu cac control points da ve
	public struct MyBitMap
	{
		public List<Point> controlPoints;
		public Color colorUse;
		public ShapeMode type;
		public int brushSize;
		// Phuong thuc khoi tao cho struct MyBitMap
		public MyBitMap(Color _color, ShapeMode _type, int size)
		{
			controlPoints = new List<Point>(); // Khoi tao list
											   // Gan cac thong so can thiet
			colorUse = _color;
			type = _type;
			brushSize = size;
		}
	}

	public partial class Form_Paint : Form
	{
		// Tọa độ điểm di chuyển sau khi chọn menu de thuc hien phep translate, rotate & scale
		Point menuStart, menuEnd;
        // 2 Tọa độ tạm thời
        Point menuPoint1, menuPoint2;
		// Mac dinh check list box la drawing
		Menu chooseItem = SharpGL.Menu.DRAWING;

		// Biến kiểm tra chúng ta có pushMatrix hay không. Mac dinh la false
		bool isPushMatrix = false;

		Color colorUserColor; // Bien mau de ve hinh
		ShapeMode shShape; // 0 neu muon ve duong thang, 1 neu duong tron, ...

		Point pStart, pEnd; // Toa do diem dau va diem cuoi
							// Point thuoc lop System.Drawing
		Point pMid; // De ve da giac
		int isDown; // Bien kiem soat con tro chuot co dang duoc giu khong
		int currentSize; // Kich co ve hien tai

		ButtonColor currentButtonColor; // Nut chon mau hien tai

		List<MyBitMap> bm = new List<MyBitMap>(); // Dung de luu tru cac doi tuong da ve
		bool isRigtClick = false;

		public Form_Paint()
		{
			InitializeComponent();
			colorUserColor = Color.White; // Gia tri mac dinh la mau trang
			currentButtonColor = ButtonColor.LEFT; // Mac dinh la nut ben trai
			shShape = ShapeMode.LINE; // Mac dinh ve duong thang
			cBox_Choose_Size.SelectedIndex = 0; // Mac dinh net ve hien thi la 1
			chkLstBox_Options.SetItemChecked(0, true); // Mặc đinh là tick vào ô Drawing 
													   // Khoi tao toa diem ban dau
			pStart = new Point(-1, -1);

			//// Cap phat vung nho cho Bitmap
			//bm = new Bitmap(this.Width, this.Height); // kich thuoc bitmap bang voi form1
			//gr = Graphics.FromImage(bm); // Truyen doi tuong Bitmap vao de ve
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

			// Unchecked các menu còn lại
			for (int i = 0; i < 4; i++)
			{
				chkLstBox_Options.SetItemChecked(i, false);
			}

			// Check menu Drawing
			chkLstBox_Options.SetItemChecked(0, true);
			// Thiet lap che do la DRAWING
			chooseItem = SharpGL.Menu.DRAWING;

			shShape = ShapeMode.LINE; // Nguoi dung chon ve duong thang
		}

		// Nguoi dung chon chuc nang ve hinh chu nhat
		private void bt_Rec_Click(object sender, EventArgs e)
		{
			// Unchecked các menu còn lại
			for (int i = 0; i < 4; i++)
			{
				chkLstBox_Options.SetItemChecked(i, false);
			}

			// Check menu Drawing
			chkLstBox_Options.SetItemChecked(0, true);
			chooseItem = SharpGL.Menu.DRAWING;
			shShape = ShapeMode.RECTANGLE;
		}

		// Nguoi dung chon chuc nang ve tam giac deu
		private void bt_Triangle_Click(object sender, EventArgs e)
		{
			// Unchecked các menu còn lại
			for (int i = 0; i < 4; i++)
			{
				chkLstBox_Options.SetItemChecked(i, false);
			}
			// Check menu Drawing
			chkLstBox_Options.SetItemChecked(0, true);
			chooseItem = SharpGL.Menu.DRAWING;
			shShape = ShapeMode.TRIANGLE;
		}
		// Bat su kien nguoi dung ve ngu giac deu
		private void bt_Pentagon_Click(object sender, EventArgs e)
		{
			// Unchecked các menu còn lại
			for (int i = 0; i < 4; i++)
			{
				chkLstBox_Options.SetItemChecked(i, false);
			}
			// Check menu Drawing
			chkLstBox_Options.SetItemChecked(0, true);
			chooseItem = SharpGL.Menu.DRAWING;
			shShape = ShapeMode.PENTAGON;
		}

		// Bat su kien nguoi dung ve luc giac deu
		private void bt_Hexagon_Click(object sender, EventArgs e)
		{
			// Unchecked các menu còn lại
			for (int i = 0; i < 4; i++)
			{
				chkLstBox_Options.SetItemChecked(i, false);
			}
			// Check menu Drawing
			chkLstBox_Options.SetItemChecked(0, true);
			chooseItem = SharpGL.Menu.DRAWING;
			shShape = ShapeMode.HEXAGON;
		}

		// Bat su kien nguoi dung ve duong tron
		private void bt_Circle_Click(object sender, EventArgs e)
		{
			// Unchecked các menu còn lại
			for (int i = 0; i < 4; i++)
			{
				chkLstBox_Options.SetItemChecked(i, false);
			}
			// Check menu Drawing
			chkLstBox_Options.SetItemChecked(0, true);
			chooseItem = SharpGL.Menu.DRAWING;
			shShape = ShapeMode.CIRCLE;
		}

		// Bat su kien nguoi dung ve ellipse
		private void bt_Ellipse_Click(object sender, EventArgs e)
		{

			// Unchecked các menu còn lại
			for (int i = 0; i < 4; i++)
			{
				chkLstBox_Options.SetItemChecked(i, false);
			}
			// Check menu Drawing
			chkLstBox_Options.SetItemChecked(0, true);
			chooseItem = SharpGL.Menu.DRAWING;
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
		private void drawLine(OpenGL gl)
		{
			gl.Enable(OpenGL.GL_LINE_SMOOTH); // Lam tron cac diem ve, cho duong thang muot hon
			gl.Begin(OpenGL.GL_LINES);
			gl.Vertex(pStart.X, gl.RenderContextProvider.Height - pStart.Y);
			gl.Vertex(pEnd.X, gl.RenderContextProvider.Height - pEnd.Y);
			gl.End();
			gl.Flush();
			gl.Disable(OpenGL.GL_LINE_SMOOTH);
		}
		// Overide ham ve doan thang co truyen diem p1, p2
		private void drawLine(OpenGL gl, Point p1, Point p2)
		{
			gl.Enable(OpenGL.GL_LINE_SMOOTH); // Lam tron cac diem ve, cho duong thang muot hon
			gl.Begin(OpenGL.GL_LINES);
			gl.Vertex(p1.X, gl.RenderContextProvider.Height - p1.Y);
			gl.Vertex(p2.X, gl.RenderContextProvider.Height - p2.Y);
			gl.End();
			gl.Flush();
			gl.Disable(OpenGL.GL_LINE_SMOOTH);
		}

		// Ham tinh khoang cach giua pStart va pEnd
		private void calculateDistance(Point a, Point b, out double d)
		{
			d = Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
		}

		private void calculateDistance(int xa, int ya, int xb, int yb, out double d)
		{
			d = Math.Sqrt(Math.Pow(xa - xb, 2) + Math.Pow(ya - yb, 2));
		}

		// Ham ve cac diem trong diem doi xung trong duong tron
		private void put8Pixel(OpenGL gl, int a, int b, int x, int y)
		{
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
			// Tinh lai toa do y cua pStart, pEnd
			Point p1, p2;
			p1 = new Point(pStart.X, gl.RenderContextProvider.Height - pStart.Y);
			p2 = new Point(pEnd.X, gl.RenderContextProvider.Height - pEnd.Y);

			// Ban kinh la cung chinh la canh cua hinh vuong 
			//do duong tron noi tiep hinh vuong co duong cheo di qua p1 va pSymmetry
			double r;
			calculateDistance(p1, p2, out r);
			r = r / (2 * Math.Sqrt(2));

			// Tam duong tron tai trung diem cua doan thang noi pStart và pSymmetry
			int xc = (p1.X + p2.X) / 2;
			int yc = (p1.Y + p2.Y) / 2;

			// Giả sử xét tâm tại 0
			int x = 0;
			int y = (int)r;
			int p = (int)(5 / 4 - r);

			// Ve diem  dau (0, r)
			put8Pixel(gl, xc, yc, x, y);

			while (x < y)
			{
				x++;
				if (p < 0)
					p += 2 * x + 3;
				else
				{
					y--;
					p += 2 * (x - y) + 5;
				}
				put8Pixel(gl, xc, yc, x, y);
			}
			#endregion
		}
		// Ham overide ve cac diem trong diem doi xung trong duong tron co them tham so pointSize
		private void put8Pixel(OpenGL gl, int a, int b, int x, int y, int pointSize)
		{
			gl.PointSize(pointSize);
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
		// Overide ham ve hinh tron co them 2 tham so diem: p1, p2
		private void drawCircle(OpenGL gl, Point p1, Point p2, int pointSize)
		{
			#region Cach 2: Su dung thuat toan MidPoint
			// Tinh lai toa do y cua pStart, pEnd
			Point _p1, _p2;
			_p1 = new Point(p1.X, gl.RenderContextProvider.Height - p1.Y);
			_p2 = new Point(p2.X, gl.RenderContextProvider.Height - p2.Y);

			// Ban kinh la cung chinh la canh cua hinh vuong 
			//do duong tron noi tiep hinh vuong co duong cheo di qua p1 va pSymmetry
			double r;
			calculateDistance(_p1, _p2, out r);
			r = r / (2 * Math.Sqrt(2));

			// Tam duong tron tai trung diem cua doan thang noi pStart và pSymmetry
			int xc = (_p1.X + _p2.X) / 2;
			int yc = (_p1.Y + _p2.Y) / 2;

			// Giả sử xét tâm tại 0
			int x = 0;
			int y = (int)r;
			int p = (int)(5 / 4 - r);

			// Ve diem  dau (0, r)
			put8Pixel(gl, xc, yc, x, y, pointSize);

			while (x < y)
			{
				x++;
				if (p < 0)
					p += 2 * x + 3;
				else
				{
					y--;
					p += 2 * (x - y) + 5;
				}
				put8Pixel(gl, xc, yc, x, y, pointSize);
			}
			#endregion
		}

		// Ham ve cac diem doi xung trong ellipse
		private void put4Pixel(OpenGL gl, int a, int b, int x, int y)
		{
			gl.PointSize(currentSize);
			gl.Begin(OpenGL.GL_POINTS);
			gl.Vertex(a + x, b + y);
			gl.Vertex(a + x, b - y);
			gl.Vertex(a - x, b - y);
			gl.Vertex(a - x, b + y);
			gl.End();
			gl.Flush();
		}

		// Ham lam tron
		int Round(double x)
		{
			return (int)(x + 0.5);
		}

		// Ham ve ellipse bang thuat toan Midpoint
		private void drawEllipse(OpenGL gl)
		{
			#region Ve ellipse bang thuat toan Midpoint
			// Gia su ban dau xet tai tam 0(0, 0)

			// Tinh lai toa do y cua pStart, pEnd
			Point p1, p2;
			p1 = new Point(pStart.X, gl.RenderContextProvider.Height - pStart.Y);
			p2 = new Point(pEnd.X, gl.RenderContextProvider.Height - pEnd.Y);

			// Tinh tam C(xc, yc) cua ellipse
			// Dat tam C la trung diem cua doan thang noi pStart va pEnd
			int xc = Round((double)(p1.X + p2.X) / 2);
			int yc = Round((double)(p1.Y + p2.Y) / 2);

			// Goi A(xa, ya) la giao diem cua 0x va ellipse
			int xa = p2.X;
			int ya = Round((double)(p1.Y + p2.Y) / 2);

			// Goi B(xb, yb) la giao diem cua 0y va ellipse
			int xb = Round((double)(p1.X + p2.X) / 2);
			int yb = p1.Y;

			// Tinh rx va ry
			double rx;
			calculateDistance(xa, ya, xc, yc, out rx);
			double ry;
			calculateDistance(xb, yb, xc, yc, out ry);

			// Diem dau
			int x = 0;
			int y = Round(ry);

			double ry2 = ry * ry; // ry^2
			double rx2 = rx * rx; // rx^2

			double p = ry2 - rx2 * ry + (1 / 4) * rx2;
			double A = 2 * ry2 * x;
			double B = 2 * rx2 * y;

			// Ve 4 diem dau
			put4Pixel(gl, xc, yc, x, y);
			// Xét vùng 1: 0 < |dy/dx| <= 1
			int k = 0;
			while (A < B)
			{
				x++;
				if (p < 0)
				{
					A += 2 * ry2;
					p += A + ry2;
				}
				else
				{
					y--;
					A += 2 * ry2;
					B -= 2 * rx2;
					p += A - B + ry2;
				}
				put4Pixel(gl, xc, yc, x, y);
			}

			// Xét vùng 2: |dy/dx| > 1
			float xlast = x, ylast = y;
			A = 2 * ry2 * xlast;
			B = 2 * rx2 * ylast;
			p = ry2 * Math.Pow((xlast + 1 / 2), 2) + rx2 * Math.Pow((ylast - 1), 2) - rx2 * ry2;

			k = 0;
			while (y != 0)
			{
				y--;
				if (p < 0)
				{
					x++;
					A += 2 * ry2;
					B -= 2 * rx2;
					p += A - B + rx2;
				}
				else
				{
					B -= 2 * rx2;
					p += -B + rx2;
				}
				put4Pixel(gl, xc, yc, x, y);
			}

			#endregion
		}

		// Ham overide ve cac diem doi xung trong ellipse co them tham so pointSize 
		private void put4Pixel(OpenGL gl, int a, int b, int x, int y, int pointSize)
		{
			gl.PointSize(pointSize);
			gl.Begin(OpenGL.GL_POINTS);
			gl.Vertex(a + x, b + y);
			gl.Vertex(a + x, b - y);
			gl.Vertex(a - x, b - y);
			gl.Vertex(a - x, b + y);
			gl.End();
			gl.Flush();
		}

		// Overide ham ve ellipse co them 2 tham so diem: p1, p2
		private void drawEllipse(OpenGL gl, Point p1, Point p2, int pointSize)
		{
			#region Ve ellipse bang thuat toan Midpoint
			// Gia su ban dau xet tai tam 0(0, 0)
			// Tinh lai toa do y cua p1, p2
			Point _p1 = new Point(p1.X, gl.RenderContextProvider.Height - p1.Y);
			Point _p2 = new Point(p2.X, gl.RenderContextProvider.Height - p2.Y);
			// Tinh tam C(xc, yc) cua ellipse
			// Dat tam C la trung diem cua doan thang noi pStart va pEnd
			int xc = Round((double)(_p1.X + _p2.X) / 2);
			int yc = Round((double)(_p1.Y + _p2.Y) / 2);

			// Goi A(xa, ya) la giao diem cua 0x va ellipse
			int xa = _p2.X;
			int ya = Round((double)(_p1.Y + _p2.Y) / 2);

			// Goi B(xb, yb) la giao diem cua 0y va ellipse
			int xb = Round((double)(_p1.X + _p2.X) / 2);
			int yb = _p1.Y;

			// Tinh rx va ry
			double rx;
			calculateDistance(xa, ya, xc, yc, out rx);
			double ry;
			calculateDistance(xb, yb, xc, yc, out ry);

			// Diem dau
			int x = 0;
			int y = Round(ry);

			double ry2 = ry * ry; // ry^2
			double rx2 = rx * rx; // rx^2

			double p = ry2 - rx2 * ry + (1 / 4) * rx2;
			double A = 2 * ry2 * x;
			double B = 2 * rx2 * y;

			// Ve 4 diem dau
			put4Pixel(gl, xc, yc, x, y, pointSize);
			// Xét vùng 1: 0 < |dy/dx| <= 1
			int k = 0;
			while (A < B)
			{
				x++;
				if (p < 0)
				{
					A += 2 * ry2;
					p += A + ry2;
				}
				else
				{
					y--;
					A += 2 * ry2;
					B -= 2 * rx2;
					p += A - B + ry2;
				}
				put4Pixel(gl, xc, yc, x, y, pointSize);
			}

			// Xét vùng 2: |dy/dx| > 1
			float xlast = x, ylast = y;
			A = 2 * ry2 * xlast;
			B = 2 * rx2 * ylast;
			p = ry2 * Math.Pow((xlast + 1 / 2), 2) + rx2 * Math.Pow((ylast - 1), 2) - rx2 * ry2;

			k = 0;
			while (y != 0)
			{
				y--;
				if (p < 0)
				{
					x++;
					A += 2 * ry2;
					B -= 2 * rx2;
					p += A - B + rx2;
				}
				else
				{
					B -= 2 * rx2;
					p += -B + rx2;
				}
				put4Pixel(gl, xc, yc, x, y, pointSize);
			}

			#endregion
		}

		// Ham ve hinh chu nhat
		private void drawRec(OpenGL gl)
		{
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
		// Overide ham ve rectangle co them 2 tham so diem: p1, p2
		private void drawRec(OpenGL gl, Point p1, Point p2)
		{
			gl.Enable(OpenGL.GL_LINE_SMOOTH); // Lam tron cac diem ve, cho duong thang muot hon
			gl.Begin(OpenGL.GL_LINE_LOOP);
			// Toa do diem dau (x1, y1)
			// Toa do diem cuoi (x2, y2)
			gl.Vertex(p1.X, gl.RenderContextProvider.Height - p1.Y);
			// Toa do diem 2 (x2, y1)
			gl.Vertex(p2.X, gl.RenderContextProvider.Height - p1.Y);
			// Toa do diem 3 (x2, y2)
			gl.Vertex(p2.X, gl.RenderContextProvider.Height - p2.Y);
			// Toa do diem 4 (x1, y2)
			gl.Vertex(p1.X, gl.RenderContextProvider.Height - p2.Y);
			gl.End();
			gl.Flush();
			gl.Disable(OpenGL.GL_LINE_SMOOTH);
		}


		// Ham ve tam giac
		private void drawTriangle(OpenGL gl)
		{
			#region VeTamGiacCan
			/*
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
			*/
			#endregion
			#region Ve Tam giac deu bang pp quay diem
			// Ý tưởng: Các đỉnh của ngũ giác đều quay 1 goc alpha = 120*PI/180 độ (đổi về radian)
			// B1: Gán pStart là tâm
			// B2: Quay pEnd theo công thức
			//	x' = x*cos(alpha) - sin(alpha)*y
			//	y' = x*sin(alpha) + y*cos(alpha)
			const int totalSegments = 3; // số lượng các segments
										 /*
										 // Ban kinh bằng 1 nửa của đoạn thẳng pStart, pEnd
										 double r;
										 calculateDistance(pStart, pEnd, out r);
										 r /= 2;

										 // Tam duong tron tai trung diem cua doan thang noi pStart và pEnd
										 int xc = (pStart.X + pEnd.X) / 2;
										 int yc = gl.RenderContextProvider.Height - ((pStart.Y + pEnd.Y) / 2);
										 */

			// Tinh lai toa do y cua pStart, pEnd
			Point p1, p2;
			p1 = new Point(pStart.X, gl.RenderContextProvider.Height - pStart.Y);
			p2 = new Point(pEnd.X, gl.RenderContextProvider.Height - pEnd.Y);

			// Ban kinh la cung chinh la canh cua hinh vuong 
			//do duong tron noi tiep hinh vuong co duong cheo di qua p1 va pSymmetry
			double r;
			calculateDistance(p1, p2, out r);
			r = r / (2 * Math.Sqrt(2));

			// Tam duong tron tai trung diem cua doan thang noi pStart và pEnd
			int xc = (p1.X + p2.X) / 2;
			int yc = (p1.Y + p2.Y) / 2;


			// Gia su xet tai tam 0(0, 0)
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
		// Overide ham ve rectangle co them 2 tham so diem: p1, p2
		private void drawTriangle(OpenGL gl, Point p1, Point p2)
		{
			#region Ve Tam giac deu bang pp quay diem
			// Ý tưởng: Các đỉnh của ngũ giác đều quay 1 goc alpha = 120*PI/180 độ (đổi về radian)
			// B1: Gán pStart là tâm
			// B2: Quay pEnd theo công thức
			//	x' = x*cos(alpha) - sin(alpha)*y
			//	y' = x*sin(alpha) + y*cos(alpha)
			const int totalSegments = 3; // số lượng các segments
										 /*
										 // Ban kinh bằng 1 nửa của đoạn thẳng pStart, pEnd
										 double r;
										 calculateDistance(p1, p2, out r);
										 r /= 2;

										 // Tam duong tron tai trung diem cua doan thang noi pStart và pEnd
										 int xc = (p1.X + p2.X) / 2;
										 int yc = gl.RenderContextProvider.Height - ((p1.Y + p2.Y) / 2);
										 */

			// Tinh lai toa do y cua p1, p2
			Point _p1, _p2;
			_p1 = new Point(p1.X, gl.RenderContextProvider.Height - p1.Y);
			_p2 = new Point(p2.X, gl.RenderContextProvider.Height - p2.Y);

			// Ban kinh la cung chinh la canh cua hinh vuong 
			//do duong tron noi tiep hinh vuong co duong cheo di qua _p1 va _p2
			double r;
			calculateDistance(_p1, _p2, out r);
			r = r / (2 * Math.Sqrt(2));

			// Tam duong tron tai trung diem cua doan thang noi _p1 va _p2
			int xc = (_p1.X + _p2.X) / 2;
			int yc = (_p1.Y + _p2.Y) / 2;

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

		// Ham ve ngu giac deu
		private void drawPentagon(OpenGL gl)
		{
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


			// Tinh lai toa do y cua pStart, pEnd
			Point p1, p2;
			p1 = new Point(pStart.X, gl.RenderContextProvider.Height - pStart.Y);
			p2 = new Point(pEnd.X, gl.RenderContextProvider.Height - pEnd.Y);

			// Ban kinh la cung chinh la canh cua hinh vuong 
			//do duong tron noi tiep hinh vuong co duong cheo di qua p1 va pSymmetry
			double r;
			calculateDistance(p1, p2, out r);
			r = r / (2 * Math.Sqrt(2));

			// Tam duong tron tai trung diem cua doan thang noi pStart và pEnd
			int xc = (p1.X + p2.X) / 2;
			int yc = (p1.Y + p2.Y) / 2;

			// Gia su xet tai tam 0(0, 0)
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
		// Overide ham ve Pentagon co them 2 tham so diem: p1, p2
		private void drawPentagon(OpenGL gl, Point p1, Point p2)
		{
			#region Cach 2: Dung phep quay diem
			// Ý tưởng: Các đỉnh của ngũ giác đều quay 1 goc alpha = 72*PI/180 độ (đổi về radian)
			// B1: Gán pStart là tâm
			// B2: Quay pEnd theo công thức
			//	x' = x*cos(alpha) - sin(alpha)*y
			//	y' = x*sin(alpha) + y*cos(alpha)
			const int totalSegments = 5; // số lượng các segments

			// Tinh lai toa do y cua p1, p2
			Point _p1, _p2;
			_p1 = new Point(p1.X, gl.RenderContextProvider.Height - p1.Y);
			_p2 = new Point(p2.X, gl.RenderContextProvider.Height - p2.Y);
			
			// Ban kinh la cung chinh la canh cua hinh vuong 
			//do duong tron noi tiep hinh vuong co duong cheo di qua _p1 va _p2
			double r;
			calculateDistance(_p1, _p2, out r);
			r = r / (2 * Math.Sqrt(2));

			// Tam duong tron tai trung diem cua doan thang noi _p1 va _p2
			int xc = (_p1.X + _p2.X) / 2;
			int yc = (_p1.Y + _p2.Y) / 2;

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
		// Ham ve luc giac deu
		private void drawHexagon(OpenGL gl)
		{
			// Ý tưởng: Các đỉnh của ngũ giác đều quay 1 goc alpha = 60*PI/180 độ (đổi về radian)
			// B1: Gán pStart là tâm
			// B2: Quay pEnd theo công thức
			//	x' = x*cos(alpha) - sin(alpha)*y
			//	y' = x*sin(alpha) + y*cos(alpha)
			const int totalSegments = 6; // số lượng các segments
										 // Ban kinh bằng 1 nửa của đoạn thẳng đi qua pStart, pEnd

			// Tinh lai toa do y cua pStart, pEnd
			Point p1, p2;
			p1 = new Point(pStart.X, gl.RenderContextProvider.Height - pStart.Y);
			p2 = new Point(pEnd.X, gl.RenderContextProvider.Height - pEnd.Y);

			// Ban kinh la cung chinh la canh cua hinh vuong 
			//do duong tron noi tiep hinh vuong co duong cheo di qua p1 va pSymmetry
			double r;
			calculateDistance(p1, p2, out r);
			r = r / (2 * Math.Sqrt(2));

			// Tam duong tron tai trung diem cua doan thang noi pStart và pEnd
			int xc = (p1.X + p2.X) / 2;
			int yc = (p1.Y + p2.Y) / 2;

			// Gia su xet tai tam 0(0, 0)
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
		// Overide ham ve luc giac deu co them 2 tham so diem: p1, p2
		private void drawHexagon(OpenGL gl, Point p1, Point p2)
		{
			// Ý tưởng: Các đỉnh của ngũ giác đều quay 1 goc alpha = 60*PI/180 độ (đổi về radian)
			// B1: Gán pStart là tâm
			// B2: Quay pEnd theo công thức
			//	x' = x*cos(alpha) - sin(alpha)*y
			//	y' = x*sin(alpha) + y*cos(alpha)
			const int totalSegments = 6; // số lượng các segments
										 // Ban kinh bằng 1 nửa của đoạn thẳng đi qua pStart, pEnd
			// Tinh lai toa do y cua p1, p2
			Point _p1, _p2;
			_p1 = new Point(p1.X, gl.RenderContextProvider.Height - p1.Y);
			_p2 = new Point(p2.X, gl.RenderContextProvider.Height - p2.Y);

			// Ban kinh la cung chinh la canh cua hinh vuong 
			//do duong tron noi tiep hinh vuong co duong cheo di qua _p1 va _p2
			double r;
			calculateDistance(_p1, _p2, out r);
			r = r / (2 * Math.Sqrt(2));

			// Tam duong tron tai trung diem cua doan thang noi _p1 va _p2
			int xc = (_p1.X + _p2.X) / 2;
			int yc = (_p1.Y + _p2.Y) / 2;

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

		// Ve da giac bat ky
		private void drawPolygon(OpenGL gl)
		{
			// Neu nguoi dung da click chuot phai de ket thuc ve da giac
			// thi noi duong thang tu pStart den pEnd
			if (isRigtClick)
			{
				drawLine(gl, pEnd, pStart);
				isDown = 0; // Ket thuc ve da giac
				isRigtClick = false; // reset lai
									 // reset lai cac toa do
				pStart = new Point(-1, -1);
				pEnd = new Point(-1, -1);
				pMid = new Point(-1, -1);

			}
			else // Nguoc lai
				drawLine(gl, pMid, pEnd);
		}
		// Overide ham ve luc giac deu co them 1 tham so: List<Point>
		private void drawPolygon(OpenGL gl, List<Point> lstPoints)
		{
			if (lstPoints.Count > 1)
			{
				for (int i = 0; i < lstPoints.Count - 1; i++)
				{
					drawLine(gl, lstPoints[i], lstPoints[i + 1]);
				}
			}
		}

		// Ham translate
		private void translate(OpenGL gl)
		{
			gl.PushMatrix();
			// Tinh khoang doi trx va try
			int xTrans = menuEnd.X - menuStart.X;
			int yTrans = -menuEnd.Y + menuStart.Y;

			// Thoi hien translate
			gl.Translate(xTrans, yTrans, 0);

			isPushMatrix = true;
		}

		private void repaint(OpenGL gl)
		{
			for (int i = 0; i < bm.Count; i++)
			{
				// KHoi tao 2 bien p1, p2
				Point p1, p2;

				// Chon mau
				gl.Color(bm[i].colorUse.R / 255.0, bm[i].colorUse.G / 255.0, bm[i].colorUse.B / 255.0, 0);
				// Thiet lap size cua net ve
				gl.LineWidth(bm[i].brushSize);

				switch (bm[i].type)
				{
					case ShapeMode.LINE:
						// Ve doan thang
						p1 = new Point(bm[i].controlPoints[0].X, bm[i].controlPoints[0].Y);
						p2 = new Point(bm[i].controlPoints[1].X, bm[i].controlPoints[1].Y);
						drawLine(gl, p1, p2);
						break;
					case ShapeMode.CIRCLE:
						// Ve duong tron
						p1 = new Point(bm[i].controlPoints[0].X, bm[i].controlPoints[0].Y);
						p2 = new Point(bm[i].controlPoints[1].X, bm[i].controlPoints[1].Y);
						drawCircle(gl, p1, p2, bm[i].brushSize);
						break;
					case ShapeMode.RECTANGLE:
						// Ve hinh chu nhat
						p1 = new Point(bm[i].controlPoints[0].X, bm[i].controlPoints[0].Y);
						p2 = new Point(bm[i].controlPoints[1].X, bm[i].controlPoints[1].Y);
						drawRec(gl, p1, p2);
						break;
					case ShapeMode.ELLIPSE:
						// Ve ellipse
						p1 = new Point(bm[i].controlPoints[0].X, bm[i].controlPoints[0].Y);
						p2 = new Point(bm[i].controlPoints[1].X, bm[i].controlPoints[1].Y);
						drawEllipse(gl, p1, p2, bm[i].brushSize);
						break;
					case ShapeMode.TRIANGLE:
						// Ve tam giac deu
						p1 = new Point(bm[i].controlPoints[0].X, bm[i].controlPoints[0].Y);
						p2 = new Point(bm[i].controlPoints[1].X, bm[i].controlPoints[1].Y);
						drawTriangle(gl, p1, p2);
						break;
					case ShapeMode.PENTAGON:
						// Ve ngu giac deu
						p1 = new Point(bm[i].controlPoints[0].X, bm[i].controlPoints[0].Y);
						p2 = new Point(bm[i].controlPoints[1].X, bm[i].controlPoints[1].Y);
						drawPentagon(gl, p1, p2);
						break;
					case ShapeMode.HEXAGON:
						// Ve luc giac deu
						p1 = new Point(bm[i].controlPoints[0].X, bm[i].controlPoints[0].Y);
						p2 = new Point(bm[i].controlPoints[1].X, bm[i].controlPoints[1].Y);
						drawHexagon(gl, p1, p2);
						break;
					case ShapeMode.POLYGON:
						// Ve da giac
						drawPolygon(gl, bm[i].controlPoints);
						break;
					case ShapeMode.FLOOD_FILL:
						p1 = new Point(bm[i].controlPoints[0].X, bm[i].controlPoints[0].Y);
						// To mau bang thuat toan flood fill
						floodFill(gl, p1.X, p1.Y, colorUserColor, bm[i].colorUse);
						break;
				}
			}
		}

		// Ham draw fill rectangle
		private void drawFillRec(OpenGL gl, Point p1, Point p2)
		{
			gl.Begin(OpenGL.GL_QUADS); // Dung ham ve tu giac
									   // Toa do diem dau (x1, y1)
									   // Toa do diem cuoi (x2, y2)
			gl.Vertex(p1.X, gl.RenderContextProvider.Height - p1.Y);
			// Toa do diem 2 (x2, y1)
			gl.Vertex(p2.X, gl.RenderContextProvider.Height - p1.Y);
			// Toa do diem 3 (x2, y2)
			gl.Vertex(p2.X, gl.RenderContextProvider.Height - p2.Y);
			// Toa do diem 4 (x1, y2)
			gl.Vertex(p1.X, gl.RenderContextProvider.Height - p2.Y);
			gl.End();
			gl.Flush();
		}

		// Ham tinh toan toa do cua control points cua 1 doi tuong va tra ve list cac diem control point cua hinh do
		// Control Point duoc tinh dua vao diem dau va diem cuoi cua 1 doi tuong
		private void makeListControlPoints(Point p1, Point p2, ShapeMode sh, out List<Point> res)
		{
			res = new List<Point>(); // Khoi tao
			if (sh == ShapeMode.LINE)
			{
				// Duong thang thi chi co 2  control points
				res.Add(p1);
				res.Add(p2);
			}
			else
			{
				int x1, y1;
				int x2, y2;
				int averageX;
				int averageY;

				if (sh == ShapeMode.ELLIPSE || sh == ShapeMode.RECTANGLE)
				{
					// Ngoai da giac ra thi cac hinh deu co
					// 8 control points
					// Toa do diem dau: (x1, y1)
					// Toa do diem cuoi: (x2, y2)
					x1 = p1.X; y1 = p1.Y;
					x2 = p2.X; y2 = p2.Y;


					// Goi averageX, averageY lan lượt là trung bình của x1, x2 va y1, y2
					averageX = Round((x1 + x2) / 2.0);
					averageY = Round((y1 + y2) / 2.0);

					res.Add(p1); // Diem 1: (x1, y1)
					res.Add(new Point(averageX, y1)); // Diem 2: (averageX, y1)
					res.Add(new Point(x2, y1)); // Diem 3: (x2, y1)
					res.Add(new Point(x2, averageY)); // Diem 4: (x2, averageY)
					res.Add(new Point(x2, y2)); // Diem 5: (x2, y2)
					res.Add(new Point(averageX, y2)); // Diem 6: (averageX, y2)
					res.Add(new Point(x1, y2)); // Diem 7: (x1, y2)
					res.Add(new Point(x1, averageY)); // Diem 8: (x1, averageY)
				}
				else
				{
					// Do cac hinh duong tron, tam giac deu, ngu giac deu, luc giac deu 
					// deu ve theo cach dong duong tron noi tiep hinh vuong
					// Cho nen ta can phai tinh lai toa do p2.
					// Con lai tuong tu nhu Ellipse va Rectangle

					if (sh == ShapeMode.CIRCLE)
					{
						// Toa do diem dau: p1(x1, y1)
						// Toa do diem cuoi: pSymmetry(x2, y2)
						x1 = p1.X; y1 = p1.Y;
						x2 = p2.X; y2 = p2.Y;

						// Goi averageX, averageY lan lượt là trung bình của x1, x2 va y1, y2
						averageX = Round((x1 + x2) / 2.0);
						averageY = Round((y1 + y2) / 2.0);

						res.Add(new Point(x1, y1)); // Diem 1: (x1, y1)
						res.Add(new Point(averageX, y1)); // Diem 2: (averageX, y1)
						res.Add(new Point(x2, y1)); // Diem 3: (x2, y1)
						res.Add(new Point(x2, averageY)); // Diem 4: (x2, averageY)
						res.Add(new Point(x2, y2)); // Diem 5: (x2, y2)
						res.Add(new Point(averageX, y2)); // Diem 6: (averageX, y2)
						res.Add(new Point(x1, y2)); // Diem 7: (x1, y2)
						res.Add(new Point(x1, averageY)); // Diem 8: (x1, averageY)

					}
					else if (sh == ShapeMode.TRIANGLE)
					{
						// Tinh lại tọa độ của pStart, pEnd dựa theo đỉnh của tam giác đều
						// Mỗi đỉnh tam giác đều xoay 1 góc 120 độ

						OpenGL gl = openGLControl.OpenGL;

						// Tinh lai toa doa p1, pSymmetry do tinh diem cua tam giac so thuc hien phep quay
						p1 = new Point(p1.X, gl.RenderContextProvider.Height - p1.Y);
						p2 = new Point(p2.X, gl.RenderContextProvider.Height - p2.Y);

						// Ban kinh la cung chinh la canh cua hinh vuong 
						//do duong tron noi tiep hinh vuong co duong cheo di qua p1 va pSymmetry
						double r;
						calculateDistance(p1, p2, out r);
						r = r / (2 * Math.Sqrt(2));

						// Tam duong tron tai trung diem cua doan thang noi pStart và pSymmetry
						int xc = (p1.X + p2.X) / 2;
						int yc = (p1.Y + p2.Y) / 2;

						// Gia su xet tai tam 0(0, 0)
						int x = 0;
						int y = (int)r;

						Point pV1, pV2, pV3;

						// Đổi về radian
						double alpha_rad = 0 * Math.PI / 180;
						pV1 = new Point(Round(xc + x * Math.Cos(alpha_rad) - y * Math.Sin(alpha_rad))
						, Round(yc + x * Math.Sin(alpha_rad) + y * Math.Cos(alpha_rad)));

						alpha_rad = 120 * Math.PI / 180;
						pV2 = new Point(Round(xc + x * Math.Cos(alpha_rad) - y * Math.Sin(alpha_rad))
								, Round(yc + x * Math.Sin(alpha_rad) + y * Math.Cos(alpha_rad)));

						alpha_rad = 240 * Math.PI / 180;
						pV3 = new Point(Round(xc + x * Math.Cos(alpha_rad) - y * Math.Sin(alpha_rad))
								, Round(yc + x * Math.Sin(alpha_rad) + y * Math.Cos(alpha_rad)));


						// Tinh x1, y1 va x2, y2
						// Va tinh lai toa do y cua 2 diem do do ham drawFillRec no tu dong mac dinh toa do truyen vao la cua winform
						x1 = pV2.X; y1 = gl.RenderContextProvider.Height - pV1.Y;
						x2 = pV3.X; y2 = gl.RenderContextProvider.Height - pV3.Y;

						// Goi averageX, averageY lan lượt là trung bình của x1, x2 va y1, y2
						averageX = Round((x1 + x2) / 2.0);
						averageY = Round((y1 + y2) / 2.0);

						res.Add(new Point(x1, y1)); // Diem 1: (x1, y1)
						res.Add(new Point(averageX, y1)); // Diem 2: (averageX, y1)
						res.Add(new Point(x2, y1)); // Diem 3: (x2, y1)
						res.Add(new Point(x2, averageY)); // Diem 4: (x2, averageY)
						res.Add(new Point(x2, y2)); // Diem 5: (x2, y2)
						res.Add(new Point(averageX, y2)); // Diem 6: (averageX, y2)
						res.Add(new Point(x1, y2)); // Diem 7: (x1, y2)
						res.Add(new Point(x1, averageY)); // Diem 8: (x1, averageY)
					}
					else if (sh == ShapeMode.PENTAGON)
					{
						// Tinh lại tọa độ của pStart, pEnd dựa theo đỉnh của Pentagon
						// Mỗi đỉnh tam giác đều xoay 1 góc 72 độ
						OpenGL gl = openGLControl.OpenGL;

						// Tinh lai toa doa p1, pSymmetry do tinh diem cua tam giac so thuc hien phep quay
						p1 = new Point(p1.X, gl.RenderContextProvider.Height - p1.Y);
						p2 = new Point(p2.X, gl.RenderContextProvider.Height - p2.Y);

						// Ban kinh la cung chinh la canh cua hinh vuong 
						//do duong tron noi tiep hinh vuong co duong cheo di qua p1 va pSymmetry
						double r;
						calculateDistance(p1, p2, out r);
						r = r / (2 * Math.Sqrt(2));

						// Tam duong tron tai trung diem cua doan thang noi pStart và pSymmetry
						int xc = (p1.X + p2.X) / 2;
						int yc = (p1.Y + p2.Y) / 2;

						// Gia su xet tai tam 0(0, 0)
						int x = 0;
						int y = (int)r;

						Point pV3; // Tinh toa do đỉnh thứ 3 của pentagon theo ngược chiều kim đống hồ

						// Đổi về radian
						double alpha_rad = 144 * Math.PI / 180;
						pV3 = new Point(Round(xc + x * Math.Cos(alpha_rad) - y * Math.Sin(alpha_rad))
						, Round(yc + x * Math.Sin(alpha_rad) + y * Math.Cos(alpha_rad)));


						// Tinh x1, y1 va x2, y2
						// Va tinh lai toa do y cua 2 diem do do ham drawFillRec no tu dong mac dinh toa do truyen vao la cua winform
						x1 = p1.X; y1 = gl.RenderContextProvider.Height - p1.Y;
						x2 = p2.X; y2 = gl.RenderContextProvider.Height - pV3.Y;

						// Goi averageX, averageY lan lượt là trung bình của x1, x2 va y1, y2
						averageX = Round((x1 + x2) / 2.0);
						averageY = Round((y1 + y2) / 2.0);

						res.Add(new Point(x1, y1)); // Diem 1: (x1, y1)
						res.Add(new Point(averageX, y1)); // Diem 2: (averageX, y1)
						res.Add(new Point(x2, y1)); // Diem 3: (x2, y1)
						res.Add(new Point(x2, averageY)); // Diem 4: (x2, averageY)
						res.Add(new Point(x2, y2)); // Diem 5: (x2, y2)
						res.Add(new Point(averageX, y2)); // Diem 6: (averageX, y2)
						res.Add(new Point(x1, y2)); // Diem 7: (x1, y2)
						res.Add(new Point(x1, averageY)); // Diem 8: (x1, averageY)
					}
					else if (sh == ShapeMode.HEXAGON)
					{
						// Tinh lại tọa độ của pStart, pEnd dựa theo đỉnh của Hexagon
						// Mỗi đỉnh tam giác đều xoay 1 góc 60 độ
						OpenGL gl = openGLControl.OpenGL;

						// Tinh lai toa doa p1, pSymmetry do tinh diem cua tam giac so thuc hien phep quay
						p1 = new Point(p1.X, gl.RenderContextProvider.Height - p1.Y);
						p2 = new Point(p2.X, gl.RenderContextProvider.Height - p2.Y);

						// Ban kinh la cung chinh la canh cua hinh vuong 
						//do duong tron noi tiep hinh vuong co duong cheo di qua p1 va pSymmetry
						double r;
						calculateDistance(p1, p2, out r);
						r = r / (2 * Math.Sqrt(2));

						// Tam duong tron tai trung diem cua doan thang noi pStart và pSymmetry
						int xc = (p1.X + p2.X) / 2;
						int yc = (p1.Y + p2.Y) / 2;

						// Gia su xet tai tam 0(0, 0)
						int x = 0;
						int y = (int)r;

						Point pV2, pV6; // Tinh toa do đỉnh thứ 2 va 6 của hexagon theo ngược chiều kim đống hồ

						// Đổi về radian
						double alpha_rad = 60 * Math.PI / 180;
						pV2 = new Point(Round(xc + x * Math.Cos(alpha_rad) - y * Math.Sin(alpha_rad))
						, Round(yc + x * Math.Sin(alpha_rad) + y * Math.Cos(alpha_rad)));

						alpha_rad = 300 * Math.PI / 180;
						pV6 = new Point(Round(xc + x * Math.Cos(alpha_rad) - y * Math.Sin(alpha_rad))
						, Round(yc + x * Math.Sin(alpha_rad) + y * Math.Cos(alpha_rad)));

						// Tinh x1, y1 va x2, y2
						// Va tinh lai toa do y cua 2 diem do do ham drawFillRec no tu dong mac dinh toa do truyen vao la cua winform
						x1 = pV2.X; y1 = gl.RenderContextProvider.Height - p1.Y;
						x2 = pV6.X; y2 = gl.RenderContextProvider.Height - p2.Y;

						// Goi averageX, averageY lan lượt là trung bình của x1, x2 va y1, y2
						averageX = Round((x1 + x2) / 2.0);
						averageY = Round((y1 + y2) / 2.0);

						res.Add(new Point(x1, y1)); // Diem 1: (x1, y1)
						res.Add(new Point(averageX, y1)); // Diem 2: (averageX, y1)
						res.Add(new Point(x2, y1)); // Diem 3: (x2, y1)
						res.Add(new Point(x2, averageY)); // Diem 4: (x2, averageY)
						res.Add(new Point(x2, y2)); // Diem 5: (x2, y2)
						res.Add(new Point(averageX, y2)); // Diem 6: (averageX, y2)
						res.Add(new Point(x1, y2)); // Diem 7: (x1, y2)
						res.Add(new Point(x1, averageY)); // Diem 8: (x1, averageY)
					}

				}
			}
		}

		private void recalculate_pStart_and_pEnd(ShapeMode sh)
		{
			// Do cac hinh duong tron, tam giac deu, ngu giac deu, luc giac deu 
			// deu ve theo cach dong duong tron noi tiep hinh vuong
			// Cho nen ta can phai tinh lai toa do pEnd.
			if (shShape == ShapeMode.CIRCLE || shShape == ShapeMode.TRIANGLE || shShape == ShapeMode.PENTAGON || shShape == ShapeMode.HEXAGON)
			{

				// Lay do dan ra cua X
				int stretchX = Math.Abs(pEnd.X - pStart.X);
				// pEnd la diem doi xung cua pStart va pEnd, pStart nam tren duong cheo ca hinh vuong 
				// Co 4 TH de xet
				// TH1: x, y cung tang
				// TH2: x tang, y giam
				// TH3: x giam, y tang
				// TH4: x giam, y giam

				// Tinh deltaX va deltaY
				int dx = pEnd.X - pStart.X;
				int dy = pEnd.Y - pStart.Y;

				if (dx > 0 && dy > 0)
					pEnd = new Point(pStart.X + stretchX, pStart.Y + stretchX);
				else if (dx > 0 && dy <= 0)
					pEnd = new Point(pStart.X + stretchX, pStart.Y - stretchX);
				else if (dx <= 0 && dy > 0)
					pEnd = new Point(pStart.X - stretchX, pStart.Y + stretchX);
				else
					pEnd = new Point(pStart.X - stretchX, pStart.Y - stretchX);
			}

		}

		private void drawControlPoints(Point p1, Point p2, ShapeMode sh)
		{
			// get the OpenGL object
			OpenGL gl = openGLControl.OpenGL;
			// Chon mau de ve control points. Chon mau Light grey
			gl.Color(Color.Gold.R / 255.0, Color.Gold.G / 255.0, Color.Gold.B / 255.0, 0);
			gl.LineWidth(1); // Size cua net ve

			// Tim cac control points cua doi tuong nay
			List<Point> controlPoints;
			makeListControlPoints(p1, p2, sh, out controlPoints);
			Point p3, p4; // La toa do diem dau va diem cuoi cua 1 control point can ve. Control point la hinh vuong (1x1)
						  // voi p3p4 la duong cheo cua hinh vuong nay

			foreach (var p in controlPoints)
			{
				//p3 = new Point(p.X + 3, p.Y + 3);
				//p4 = new Point(p.X - 3, p.Y - 3);
				//drawFillRec(gl, p3, p4);
				gl.PointSize(6);
				gl.Begin(OpenGL.GL_POINTS);
				gl.Vertex(p.X, gl.RenderContextProvider.Height - p.Y);
				gl.End();
				gl.Flush();
			}
		}

		private void openGLControl_OpenGLDraw(object sender, RenderEventArgs args)
		{
			if (isDown == 1) // Neu nguoi dung dang Mouse down thi moi ve
			{
				// get the OpenGL object
				OpenGL gl = openGLControl.OpenGL;
				// clear the color and depth buffer
				// Xóa màn hình
				gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

				// Thuc hien repaint
				repaint(gl);

				// Chon mau
				gl.Color(colorUserColor.R / 255.0, colorUserColor.G / 255.0, colorUserColor.B / 255.0, 0);
				// Thiet lap size cua net ve
				gl.LineWidth(currentSize);

				// Stopwatch ho tro do thoi gian
				Stopwatch myTimer = new Stopwatch();
				myTimer.Start(); // bat dau do

				//===================================================================//
				//===================================================================//
				//========================Nội dung của Lab 03========================//
				// Xét trường hợp menu được chọn
				// Với mỗi menu được chọn ta sẽ xét một ma trận khác
				// Do đó cần push ma trận model view vào stack
				if (chooseItem == SharpGL.Menu.TRANSLATE) // Dich chuyen
				{

					translate(gl);
                    // Xóa màn hình
                    gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

                    // Chon mau
                    gl.Color(colorUserColor.R / 255.0, colorUserColor.G / 255.0, colorUserColor.B / 255.0, 0);
                    // Thiet lap size cua net ve
                    gl.LineWidth(currentSize);
                    drawLine(gl, menuPoint1, menuPoint2);
				}
				else if (chooseItem == SharpGL.Menu.ROTATE) // Xoay
				{

				}
				else if (chooseItem == SharpGL.Menu.SCALE) // Scale/Zoom
				{

				}
                else if(chooseItem==SharpGL.Menu.DRAWING)
                {
                    // Ve voi cho nay
                    // ...
                    switch (shShape)
                    {
                        case ShapeMode.LINE:
                            // Ve doan thang
                            drawLine(gl);
                            //Shape.Line.draw(gl);
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
                            drawEllipse(gl);
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
                        case ShapeMode.POLYGON:
                            // Ve da giac
                            drawPolygon(gl);
                            break;
                        case ShapeMode.FLOOD_FILL:
                            // To mau bang thuat toan flood fill
                            //floodFill(gl, pStart.X, pStart.Y, colorUserColor, Color.Black);
                            floodFillScanLineStack(gl, pStart.X, pStart.Y, colorUserColor, Color.Black);
                            break;
                    }
                }

				myTimer.Stop(); // ket thuc do
				TimeSpan Time = myTimer.Elapsed; // Lay thoi gian troi qua
				tb_Time.Text = String.Format("{0} (sec)", Time.TotalSeconds); // In ra tb_Time

				// Kiem tra xem co push matrix vao stack khong?
				if (isPushMatrix == true)
				{
					gl.PopMatrix();
				}
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
		private void getPixelColor(OpenGL gl, int x, int y, out Byte[] color)
		{
			color = new Byte[4 * 1 * 1]; // Components * width * height (RGBA)
			gl.ReadPixels(x, gl.RenderContextProvider.Height - y, 1, 1, OpenGL.GL_RGBA, OpenGL.GL_UNSIGNED_BYTE, color);
		}

		// Ham set pixel color
		private void setPixelColor(OpenGL gl, int x, int y, Color fill_color)
		{

			//Byte[] color = new Byte[4] { fill_color.R, fill_color.G, fill_color.B, fill_color.A };
			//int size = color.Length * Marshal.SizeOf(color[0]); // Tinh kich thuoc cua mang byte
			//IntPtr pnt = Marshal.AllocHGlobal(size); // Khoi tao vung nho cho pnt

			//// Copy color vao pnt
			//Marshal.Copy(color, 0, pnt, color.Length);
			//// Vi tri de raster 
			//gl.RasterPos(x, gl.RenderContextProvider.Height - y);
			//// set mau
			//gl.DrawPixels(2, 2, OpenGL.GL_RGBA, OpenGL.GL_BYTE, pnt);

			//// Free the unmanaged memory.
			//Marshal.FreeHGlobal(pnt);

			gl.Color(fill_color.R / 255.0, fill_color.G / 255.0, fill_color.B / 255.0, fill_color.A);
			gl.PointSize(2);
			gl.Begin(OpenGL.GL_POINTS);
			gl.Vertex(x, gl.RenderContextProvider.Height - y);
			gl.End();
			gl.Flush();


		}

		// Ham to mau theo vet loang
		private void floodFill(OpenGL gl, int x, int y, Color fill_color, Color old_color)
		{
			#region Recursive Flood fill
			//Byte[] current_color;
			//getPixelColor(gl, x, y, out current_color);

			//// Thuat toan to mau theo vet loang
			//// Neu color cua pixel hiện tai chưa tô và khác màu của biên
			////if ((current_color[0] != fill_color.R && current_color[1] != fill_color.G && current_color[2] != fill_color.B 
			////	&& current_color[3] != fill_color.A) &&
			////	(current_color[0] != old_color.R && current_color[1] != old_color.G && current_color[2] != old_color.B 
			////	&& current_color[3] != old_color.A))
			//if (current_color[0] == old_color.R && current_color[1] == old_color.G && current_color[2] == old_color.B
			//	&& current_color[3] == old_color.A)
			//{
			//	setPixelColor(gl, x, y, fill_color);
			//	floodFill(gl, x + 1, y, fill_color, old_color);
			//	floodFill(gl, x - 1, y, fill_color, old_color);
			//	floodFill(gl, x, y + 1, fill_color, old_color);
			//	floodFill(gl, x, y - 1, fill_color, old_color);

			//}
			#endregion
			#region Flood fill stack
			if (fill_color == old_color) return; // Tranh lap vo han

			int[] dx = new int[] { 0, 1, 0, -1 }; // Cac nhanh lan can 4 cua x
			int[] dy = new int[] { -1, 0, 1, 0}; // Cac nhanh lan can 4 cua y

			Stack<Point> s = new Stack<Point>(); // Khoi tao stack
			s.Push(new Point(x, y)); // Push diem dau vao Stack

			while (s.Count != 0) { // Khi stack khac rong
				Point p = s.Pop(); // Pop ra khoi stack
				setPixelColor(gl, p.X, p.Y, fill_color); //To mau
				for (int i = 0; i < 4; i++) {
					int nx = p.X + dx[i];
					int ny = p.Y + dy[i];
					// Lay pixel cua nx, ny
					Byte[] neighbor_color;
					getPixelColor(gl, nx, ny, out neighbor_color);
					// Neu nhu nx, ny chua to thi push vao stack
					if (neighbor_color[0] == old_color.R && neighbor_color[1] == old_color.G && neighbor_color[2] == old_color.B
						&& neighbor_color[3] == old_color.A) {
						s.Push(new Point(nx, ny));
					}
				}

			}
			#endregion

		}

		// Ham to mau floodFill x Scanline (Stack)
		private void floodFillScanLineStack(OpenGL gl, int x, int y, Color fill_color, Color old_color) {
			if (fill_color == old_color) return;

			int x1;
			bool spanAbove, spanBelow;

			Stack<Point> s = new Stack<Point>();
			s.Push(new Point(x, y));

			while (s.Count != 0) { // Neu stack khac rong
				Point p = s.Pop();
				x = p.X;	y = p.Y; // Cap nhat lai x, y

				x1 = x;
				// Lay pixel cua x1, y
				Byte[] pixel;
				getPixelColor(gl, x1, y, out pixel);
				Color color = new Color();
				color = Color.FromArgb(pixel[3], pixel[0], pixel[1], pixel[2]);
				while (x1 >= 0 && color.Equals(old_color)) {
					x1--;
					getPixelColor(gl, x1, y, out pixel);
					color = new Color();
					color = Color.FromArgb(pixel[3], pixel[0], pixel[1], pixel[2]);
				}
				x1++;

				spanAbove = spanBelow = false;

				getPixelColor(gl, x1, y, out pixel);
				color = new Color();
				color = Color.FromArgb(pixel[3], pixel[0], pixel[1], pixel[2]);
				while (x1 < gl.RenderContextProvider.Width && color.Equals(old_color)) {
					setPixelColor(gl, x1, y, fill_color);

					getPixelColor(gl, x1, y - 1, out pixel);
					color = new Color();
					color = Color.FromArgb(pixel[3], pixel[0], pixel[1], pixel[2]);
					if (!spanAbove && gl.RenderContextProvider.Height - y > 0 && color.Equals(old_color))
					{
						s.Push(new Point(x1, y - 1));
						spanAbove = true;
					}
					else if (spanAbove && gl.RenderContextProvider.Height - y > 0 && !color.Equals(old_color)) {
						spanAbove = false;
					}

					getPixelColor(gl, x1, y + 1, out pixel);
					color = new Color();
					color = Color.FromArgb(pixel[3], pixel[0], pixel[1], pixel[2]);
					if (!spanBelow && gl.RenderContextProvider.Height - y < gl.RenderContextProvider.Height - 1 && color.Equals(old_color))
					{
						s.Push(new Point(x1, y + 1));
						spanBelow = true;
					}
					else if (spanAbove && gl.RenderContextProvider.Height - y < gl.RenderContextProvider.Height - 1 && !color.Equals(old_color))
					{
						spanBelow = false;
					}
					x1++;
					getPixelColor(gl, x1, y, out pixel);
					color = new Color();
					color = Color.FromArgb(pixel[3], pixel[0], pixel[1], pixel[2]);
				}
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
				// Xét menu đang chọn
				if (chooseItem == SharpGL.Menu.DRAWING)
				{
					// Cap nhat diem cuoi
					pEnd = new Point(e.Location.X, e.Location.Y);
					// Tinh lai pEnd neu shShape la Circle, Triangle, Pentagon, Hexagon
					recalculate_pStart_and_pEnd(shShape);
				}
				else
				{
					// Cập nhật menuEnd
					menuEnd = new Point(e.Location.X, e.Location.Y);
				}

				// In toa do khi di chuyen chuot 
				lb_Coor.Text = e.X.ToString() + ", " + e.Y.ToString();
			}

		}

		// Cap nhat toa do diem cuoi khi nguoi dung buong chuot ra
		private void ctrl_OpenGLControl_MouseUp(object sender, MouseEventArgs e)
		{
			// Nếu chọn menu Drawing
			if (chooseItem != SharpGL.Menu.DRAWING)
			{
				// Cập nhật lại điểm menuEnd
				menuEnd = new Point(e.X, e.Y);
				isDown = 0;
			}
			else
			{
				// Neu nguoi dung khong ve da giac thi ket thuc viec ve hinh
				if (shShape != ShapeMode.POLYGON)
				{
					openGLControl.Cursor = Cursors.Default; // Tra ve con tro chuot nhu cu
					isDown = 0; // chuot het di chuyen

					// Khi nguoi dung vua ve xong hinh thi ve control points
					drawControlPoints(pStart, pEnd, shShape);

					// Thuc hien lui doi tuong da ve vao List<MyBitMap> bm
					MyBitMap tmp = new MyBitMap(colorUserColor, shShape, currentSize);
					tmp.controlPoints.Add(pStart);
					tmp.controlPoints.Add(pEnd);
					// Them tmp vao bm
					bm.Add(tmp);

					// reset lai toa do
					pStart = new Point(-1, -1);
					pEnd = new Point(-1, -1);
				}

			}

		}

		// Su kien chon size ve
		private void cBox_Choose_Size_SelectedIndexChanged(object sender, EventArgs e)
		{
			currentSize = int.Parse(cBox_Choose_Size.Text);
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit(); // Tắt chuong trinh
		}

		private void bt_Polygon_Click(object sender, EventArgs e)
		{
			shShape = ShapeMode.POLYGON;
		}

		// Xu ly su kien nguoi dung click chuot
		private void openGLControl_MouseClick(object sender, MouseEventArgs e)
		{
			// Neu nguoi dung nhap chuot phai nghia la noi diem pEnd cuoi cung den diem pStart ban dau 
			// va ket thuc ve da giac
			if (e.Button == MouseButtons.Right && shShape == ShapeMode.POLYGON)
			{
				isRigtClick = true; // Danh dau da click chuot phai
									// Luu them diem pStart vao bm để thực hiện kẻ đường thẳng từ pEnd cuối cùng cho đến pStart
				bm[bm.Count - 1].controlPoints.Add(pStart);
			}
		}

		private void chkLstBox_Options_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (chkLstBox_Options.SelectedIndex)
			{
				case 0:
					chooseItem = SharpGL.Menu.DRAWING;
					break;
				case 1:
					chooseItem = SharpGL.Menu.TRANSLATE;
					break;
				case 2:
					chooseItem = SharpGL.Menu.ROTATE;
					break;
				case 3:
					chooseItem = SharpGL.Menu.SCALE;
					break;

			}

			for (int i = 0; i < 4; i++)
			{
				if (i != chkLstBox_Options.SelectedIndex)
				{
					chkLstBox_Options.SetItemChecked(i, false);
				}
			}
		}

		// Cap nhat diem dau khi nguoi dung bat dau giu chuot
		private void ctrl_OpenGLControl_MouseDown(object sender, MouseEventArgs e)
		{
			if (chooseItem == SharpGL.Menu.DRAWING)
			{
				if (shShape != ShapeMode.POLYGON)
				{
					// Cap nhat toa do diem dau
					pStart = new Point(e.Location.X, e.Location.Y); // e la tham so lien quan den su kien chon diem
					pEnd = new Point(e.X, e.Y); // Mac dinh pEnd = pStart
				}
				else // Neu la hinh da giac
				{
					// Neu moi bat dau click
					if (pStart.X == -1)
					{
						pStart = new Point(e.X, e.Y);
						pEnd = new Point(e.X, e.Y); // Mac dinh pEnd = pStart
						pMid = new Point(e.X, e.Y); // Mac dinh pMid = pStart

						// Do day la lan click chuot dau tien de ve da giac nen ta se cap phat vung nho luu
						// da giac nay voi toa do diem ban dau la pStart
						MyBitMap tmp = new MyBitMap(colorUserColor, shShape, currentSize);
						tmp.controlPoints.Add(pStart);
						// Them tmp vao bm
						bm.Add(tmp);
					}
					else // Nguoc lai
					{
						pMid = new Point(pEnd.X, pEnd.Y);
						pEnd = new Point(e.X, e.Y);

						// Luu them toa doa cac dinh khac cua da giac moi lan user click chuot
						bm[bm.Count - 1].controlPoints.Add(e.Location);
					}

				}


			}
			else
			{
				// Cap nhat toa do cho viec thuc hien cac phep transform
				menuStart = menuEnd = new Point(e.X, e.Y);
                double dis = 0;
                while (dis == 0)
                {
                    // Kiểm tra xem vị trí click là của đối tượng nào
                    for (int i = bm.Count - 1; i >= 0; i--)
                    {
                        
                        for (int j = 0; j < bm[i].controlPoints.Count; j++)
                        {
                            calculateDistance(e.Location, bm[i].controlPoints[j], out dis);
                            if (dis <= 100)
                            {
                                shShape = bm[i].type;
                                currentSize = bm[i].brushSize;
                                colorUserColor = bm[i].colorUse;
                                menuStart = menuEnd = e.Location;
                                menuPoint1 = bm[i].controlPoints[0];
                                menuPoint2 = bm[i].controlPoints[1];
                                break;
                            }
                        }
                        if (dis != 0)
                        {
                            break;
                        }
                    }
                }
                
			}



			// In toa do khi click chuot 
			lb_Coor.Text = e.X.ToString() + ", " + e.Y.ToString();
			openGLControl.Cursor = Cursors.Cross; // Thay doi hinh dang con tro chuot khi ve
			isDown = 1; // Chuot dang bat dau di chuyen
		}

	}
}
