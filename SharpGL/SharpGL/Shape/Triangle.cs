using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace SharpGL.Shape
{
    class Triangle:Shape
    {
        public Triangle(Point p1, Point p2, ShapeMode shape = ShapeMode.TRIANGLE)
        {
            pStart = p1;
            pEnd = p2;
            ShapeKind = shape;
        }
        public override void draw(OpenGL gl)
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
            SharpGL.Form_Paint.calculateDistance(p1, p2, out r);
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
    }
}
