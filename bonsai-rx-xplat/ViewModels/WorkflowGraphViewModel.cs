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
        public Command CanvasInteractionCommand { get; set; }
        public Command StartInteractionCommand { get; set; }
        public Command EndInteractionCommand { get; set; }
        public Command DragCommand { get; set; }
        private Dictionary<Tuple<int, int>, DrawnTransform> PixelReference = new();
        public ObservableCollection<DrawnTransform> DrawnTransforms { get; set; } = new();

        public WorkflowGraphViewModel()
        {
            DrawnTransforms.Add(new DrawnRectangle(PixelReference, new PointF(10, 10), new Point(90, 100), 6, Colors.Red, Colors.Black));

            CanvasInteractionCommand = new Command(obj =>
            {
                var graphicsView = obj as GraphicsView;
                graphicsView.Invalidate();
            });

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
    }
}
