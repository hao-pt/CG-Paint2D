using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace SharpGL.Shape
{
    class Circle:Shape
    {
        public Circle(Point p1, Point p2, ShapeMode shape = ShapeMode.CIRCLE)
        {
            pStart = p1;
            pEnd = p2;
            ShapeKind = shape;
        }
        public override void draw(OpenGL gl)
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
            SharpGL.Form_Paint.calculateDistance(p1, p2, out r);
            r = r / (2 * Math.Sqrt(2));

            // Tam duong tron tai trung diem cua doan thang noi pStart và pSymmetry
            int xc = (p1.X + p2.X) / 2;
            int yc = (p1.Y + p2.Y) / 2;

            // Giả sử xét tâm tại 0
            int x = 0;
            int y = (int)r;
            int p = (int)(5 / 4 - r);

            // Ve diem  dau (0, r)
            SharpGL.Form_Paint.put8Pixel(gl, xc, yc, x, y);

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
                SharpGL.Form_Paint.put8Pixel(gl, xc, yc, x, y);
            }
            #endregion
        }
    }
}
