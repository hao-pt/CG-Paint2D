using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace SharpGL.Shape
{
    class Pentagon:Shape
    {
        public Pentagon(Point p1, Point p2, ShapeMode shape = ShapeMode.PENTAGON)
        {
            pStart = p1;
            pEnd = p2;
            ShapeKind = shape;
        }
        public override void draw(OpenGL gl)
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
