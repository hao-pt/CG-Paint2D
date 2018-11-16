using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace SharpGL.Shape
{
    class Line : Shape
    {
        public Line(Point p1,Point p2,string shape = "LINE")
        {
            pStart = p1;
            pEnd = p2;
            ShapeKind = shape;
        }
        public override void draw(OpenGL gl)
        {          
            gl.Enable(OpenGL.GL_LINE_SMOOTH); // Lam tron cac diem ve, cho duong thang muot hon
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(pStart.X, gl.RenderContextProvider.Height - pStart.Y);
            gl.Vertex(pEnd.X, gl.RenderContextProvider.Height - pEnd.Y);
            gl.End();
            gl.Flush();
            gl.Disable(OpenGL.GL_LINE_SMOOTH);
        }
    }
}
