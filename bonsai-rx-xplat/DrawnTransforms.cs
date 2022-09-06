using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bonsai_rx_xplat
{
    public class DrawnTransform
    {
        protected Color FillColor;
        public DrawnTransform()
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
        protected PointF StartPoint;
        protected float Width;
        protected float Height;
        protected float StrokeSize;
        protected Color StrokeColor;

        public DrawnRectangle(PointF startPoint, float width, float height, float strokeSize, Color strokeColor, Color fillColor)
        {
            StartPoint = new PointF(startPoint.X, startPoint.Y);
            Width = width;
            Height = height;
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
            canvas.FillRectangle(StartPoint.X, StartPoint.Y, Width, Height);
        }

        public override bool ContainsPoint(PointF point)
        {
            RectF boundingRect = new RectF(StartPoint.X, StartPoint.Y, Width, Height);
            return boundingRect.Contains(point);
        }

        public override void OnSelect()
        {
            FillColor = Colors.Blue;
        }
    }

    public class DrawnLabeledRectangle : DrawnRectangle
    {
        protected string Label;

        public DrawnLabeledRectangle(PointF startPoint, float width, float height, float strokeSize, Color strokeColor, Color fillColor, string label) : base(startPoint, width, height, strokeSize, strokeColor, fillColor)
        {
            Label = label;
        }

        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            base.Draw(canvas, dirtyRect);
            canvas.DrawString(Label, StartPoint.X + (Width / 2), StartPoint.Y + (Height / 2), HorizontalAlignment.Center);
        }
    }
}
