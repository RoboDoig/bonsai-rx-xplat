using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bonsai_rx_xplat
{
    public class DrawnTransform
    {
        public Color FillColor;
        public DrawnTransform(Dictionary<Tuple<int, int>, DrawnTransform> pixelReference)
        {

        }

        public virtual void Draw(ICanvas canvas, RectF dirtyRect)
        {

        }

        public virtual bool ContainsPoint(PointF point)
        {
            return false;
        }

        public virtual void OnSelect()
        {

        }
    }

    public class DrawnRectangle : DrawnTransform
    {
        private PointF StartPoint;
        private PointF EndPoint;
        private float StrokeSize;
        private Color StrokeColor;

        public DrawnRectangle(Dictionary<Tuple<int, int>, DrawnTransform> pixelReference,
            PointF startPoint, PointF endPoint, float strokeSize, Color strokeColor, Color fillColor) : base(pixelReference)
        {
            StartPoint = new PointF(startPoint.X, startPoint.Y);
            EndPoint = new PointF(endPoint.X, endPoint.Y);
            StrokeSize = strokeSize;
            StrokeColor = strokeColor;
            FillColor = fillColor;
        }

        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            base.Draw(canvas, dirtyRect);
            canvas.StrokeColor = StrokeColor;
            canvas.FillColor = FillColor;
            canvas.StrokeSize = StrokeSize;
            canvas.FillRectangle(StartPoint.X, StartPoint.Y, EndPoint.X, EndPoint.Y);
        }

        public override bool ContainsPoint(PointF point)
        {
            RectF boundingRect = new RectF(StartPoint.X, StartPoint.Y, EndPoint.X, EndPoint.Y);
            return boundingRect.Contains(point);
        }

        public override void OnSelect()
        {
            FillColor = Colors.Blue;
        }
    }
}
