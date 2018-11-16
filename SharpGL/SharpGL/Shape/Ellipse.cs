using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace SharpGL.Shape
{
    class Ellipse:Shape
    {
        public Ellipse(Point p1, Point p2, ShapeMode shape = ShapeMode.ELLIPSE)
        {
            pStart = p1;
            pEnd = p2;
            ShapeKind = shape;
        }
        public override void draw(OpenGL gl)
        {
            #region Ve ellipse bang thuat toan Midpoint
            // Gia su ban dau xet tai tam 0(0, 0)

            // Tinh lai toa do y cua pStart, pEnd
            Point p1, p2;
            p1 = new Point(pStart.X, gl.RenderContextProvider.Height - pStart.Y);
            p2 = new Point(pEnd.X, gl.RenderContextProvider.Height - pEnd.Y);

            // Tinh tam C(xc, yc) cua ellipse
            // Dat tam C la trung diem cua doan thang noi pStart va pEnd
            int xc = SharpGL.Form_Paint.Round((double)(p1.X + p2.X) / 2);
            int yc = SharpGL.Form_Paint.Round((double)(p1.Y + p2.Y) / 2);

            // Goi A(xa, ya) la giao diem cua 0x va ellipse
            int xa = p2.X;
            int ya = SharpGL.Form_Paint.Round((double)(p1.Y + p2.Y) / 2);

            // Goi B(xb, yb) la giao diem cua 0y va ellipse
            int xb = SharpGL.Form_Paint.Round((double)(p1.X + p2.X) / 2);
            int yb = p1.Y;

            // Tinh rx va ry
            double rx;
            SharpGL.Form_Paint.calculateDistance(xa, ya, xc, yc, out rx);
            double ry;
            SharpGL.Form_Paint.calculateDistance(xb, yb, xc, yc, out ry);

            // Diem dau
            int x = 0;
            int y = SharpGL.Form_Paint.Round(ry);

            double ry2 = ry * ry; // ry^2
            double rx2 = rx * rx; // rx^2

            double p = ry2 - rx2 * ry + (1 / 4) * rx2;
            double A = 2 * ry2 * x;
            double B = 2 * rx2 * y;

            // Ve 4 diem dau
            SharpGL.Form_Paint.put4Pixel(gl, xc, yc, x, y);
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
                SharpGL.Form_Paint.put4Pixel(gl, xc, yc, x, y);
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
                SharpGL.Form_Paint.put4Pixel(gl, xc, yc, x, y);
            }

            #endregion
        }
    }
}
