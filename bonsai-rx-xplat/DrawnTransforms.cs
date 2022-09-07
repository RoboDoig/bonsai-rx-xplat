using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bonsai_rx_xplat
{
    // Drawing logic for different canvas shapes
    public class DrawnTransform
    {
        protected Color FillColor;
        protected Color DefaultFillColor;
        protected Color SelectedFillColor;
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

        public virtual void OnDeselect()
        {

        }
    }

    public class DrawnRectangle : DrawnTransform
    {
        protected PointF StartPoint;
        protected float Width;
        protected float Height;

        public DrawnRectangle(PointF startPoint, float width, float height, Color fillColor, Color selectedFillColor)
        {
            StartPoint = new PointF(startPoint.X, startPoint.Y);
            Width = width;
            Height = height;
            FillColor = fillColor;
            DefaultFillColor = fillColor;
            SelectedFillColor = selectedFillColor;
        }

        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            base.Draw(canvas, dirtyRect);
            canvas.FillColor = FillColor;
            canvas.FillRectangle(StartPoint.X, StartPoint.Y, Width, Height);
        }

        public override bool ContainsPoint(PointF point)
        {
            RectF boundingRect = new RectF(StartPoint.X, StartPoint.Y, Width, Height);
            return boundingRect.Contains(point);
        }

        public override void OnSelect()
        {
            FillColor = SelectedFillColor;
        }

        public override void OnDeselect()
        {
            FillColor = DefaultFillColor;
        }
    }

    public class DrawnLabeledRectangle : DrawnRectangle
    {
        protected string Label;

        public DrawnLabeledRectangle(PointF startPoint, float width, float height, Color fillColor, Color selectedFillColor, string label) 
            : base(startPoint, width, height, fillColor, selectedFillColor)
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
