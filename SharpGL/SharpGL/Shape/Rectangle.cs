using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace SharpGL.Shape
{
    class Rectangle:Shape
    {
        public Rectangle(Point p1, Point p2, ShapeMode shape = ShapeMode.RECTANGLE)
        {
            pStart = p1;
            pEnd = p2;
            ShapeKind = shape;
        }
        public override void draw(OpenGL gl)
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
    }
}
