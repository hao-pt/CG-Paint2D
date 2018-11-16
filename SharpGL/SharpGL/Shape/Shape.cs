using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace SharpGL.Shape
{
    public abstract class  Shape
    {
        protected Point pStart;
        protected Point pEnd;
        protected ShapeMode ShapeKind;

        public abstract void draw(OpenGL gl);
    }
}
