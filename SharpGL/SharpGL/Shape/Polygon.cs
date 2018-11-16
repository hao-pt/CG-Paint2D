using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SharpGL.Shape
{
    class Polygon:Shape
    {
        public Polygon(Point p1, Point p2, ShapeMode shape = ShapeMode.POLYGON)
        {
            pStart = p1;
            pEnd = p2;
            ShapeKind = shape;
        }
        public override void draw(OpenGL gl)
        {
            // Neu nguoi dung da click chuot phai de ket thuc ve da giac
            // thi noi duong thang tu pStart den pEnd
            if (SharpGL.Form_Paint.isRigtClick)
            {
                drawLine(gl, pEnd, pStart);
                SharpGL.Form_Paint.isDown = 0; // Ket thuc ve da giac
                SharpGL.Form_Paint.isRigtClick = false; // reset lai
                                     // reset lai cac toa do
                pStart = new Point(-1, -1);
                pEnd = new Point(-1, -1);
                SharpGL.Form_Paint.pMid = new Point(-1, -1);

            }
            else // Nguoc lai
                drawLine(gl, SharpGL.Form_Paint.pMid, pEnd);
        }
    }
}
