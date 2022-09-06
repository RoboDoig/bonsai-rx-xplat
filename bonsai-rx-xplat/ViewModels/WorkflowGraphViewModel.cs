using Microsoft.Maui.Graphics.Text;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bonsai_rx_xplat.ViewModels
{
    public class WorkflowGraphViewModel : IDrawable
    {
        public Command StartInteractionCommand { get; set; }
        public Command EndInteractionCommand { get; set; }
        public Command DragCommand { get; set; }
        private Dictionary<Tuple<int, int>, DrawnTransform> PixelReference = new();
        public ObservableCollection<DrawnTransform> DrawnTransforms { get; set; } = new();

        public WorkflowGraphViewModel()
        {
            System.Diagnostics.Debug.WriteLine("WorkflowGraph");
            DrawnTransforms.Add(new DrawnRectangle(PixelReference, new PointF(10, 10), new Point(90, 100), 6, Colors.Red, Colors.Black));

            StartInteractionCommand = new Command(StartInteraction);

            EndInteractionCommand = new Command((eventArgs) => {
                var point = eventArgs as PointF[];
            });

            DragCommand = new Command((eventArgs) =>
            {
                var point = eventArgs as PointF[];
            });
        }

        private void StartInteraction(object eventArgs)
        {
            var point = eventArgs as PointF[];

            foreach (var transform in DrawnTransforms)
            {
                if (transform.ContainsPoint(point[0]))
                {
                    transform.OnSelect();
                }
            }
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            foreach (var transform in DrawnTransforms)
            {
                transform.Draw(canvas, dirtyRect);
            }
        }

        // TODO - move transforms to some drawing class
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
}
