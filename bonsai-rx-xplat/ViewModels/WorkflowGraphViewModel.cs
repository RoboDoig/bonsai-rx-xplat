using Microsoft.Maui.Graphics.Text;
using System;
using System.Collections.Generic;
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
        private List<DrawnTransform> DrawnTransforms = new();

        public WorkflowGraphViewModel()
        {
            // TODO - init new transforms
            DrawnTransforms.Add(new DrawnLine(PixelReference, new PointF(10, 10), new Point(90, 100), 6, Colors.Red));
            DrawnTransforms.Add(new DrawnLine(PixelReference, new PointF(20, 10), new Point(80, 70), 6, Colors.Blue));

            StartInteractionCommand = new Command((eventArgs) => {
                var point = eventArgs as PointF[];
                //System.Diagnostics.Debug.WriteLine(point[0].X);
                //System.Diagnostics.Debug.WriteLine(point[0].Y);

                Tuple<int, int> pixelPos = new Tuple<int, int>((int)point[0].X, (int)point[0].Y);
                if (PixelReference.ContainsKey(pixelPos))
                {
                    System.Diagnostics.Debug.WriteLine(PixelReference[pixelPos]);
                }
            });

            EndInteractionCommand = new Command((eventArgs) => {
                var point = eventArgs as PointF[];
                //System.Diagnostics.Debug.WriteLine("End");
            });

            DragCommand = new Command((eventArgs) =>
            {
                var point = eventArgs as PointF[];
                //System.Diagnostics.Debug.WriteLine(point[0].X);
                //System.Diagnostics.Debug.WriteLine(point[0].Y);
            });
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
            public DrawnTransform(Dictionary<Tuple<int, int>, DrawnTransform> pixelReference)
            {

            }

            public virtual void Draw(ICanvas canvas, RectF dirtyRect)
            {

            }
        }

        public class DrawnLine : DrawnTransform
        {
            private PointF StartPoint;
            private PointF EndPoint;
            private float StrokeSize;
            private Color StrokeColor;

            public DrawnLine(Dictionary<Tuple<int, int>, DrawnTransform> pixelReference,
                PointF startPoint, PointF endPoint, float strokeSize, Color strokeColor) : base(pixelReference)
            {
                StartPoint = new PointF(startPoint.X, startPoint.Y);
                EndPoint = new PointF(endPoint.X, endPoint.Y);
                StrokeSize = strokeSize;
                StrokeColor = strokeColor;

                // Update occupied pixels - bresenham algorithm
                foreach (Tuple<int, int> pixelLoc in DrawingHelpers.LinePixels((int)StartPoint.X, (int)StartPoint.Y, (int)EndPoint.X, (int)EndPoint.Y)
                {
                    pixelReference[pixelLoc] = this;
                }
            }

            public override void Draw(ICanvas canvas, RectF dirtyRect)
            {
                base.Draw(canvas, dirtyRect);
                canvas.StrokeColor = StrokeColor;
                canvas.StrokeSize = StrokeSize;
                canvas.DrawLine(StartPoint, EndPoint);
            }
        }
    }
}
