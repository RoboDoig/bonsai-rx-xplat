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
        public Command TouchCommand { get; set; }
        public Command DragCommand { get; set; }

        public WorkflowGraphViewModel()
        {
            TouchCommand = new Command((eventArgs) => {
                var point = eventArgs as PointF[];
                System.Diagnostics.Debug.WriteLine(point[0].X);
                System.Diagnostics.Debug.WriteLine(point[0].Y);
            });

            DragCommand = new Command((eventArgs) =>
            {
                var point = eventArgs as PointF[];
                System.Diagnostics.Debug.WriteLine(point[0].X);
                System.Diagnostics.Debug.WriteLine(point[0].Y);
            });
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 6;
            canvas.DrawLine(10, 10, 90, 100);

            canvas.DrawString("Blop", 20, 20, 100, 100, HorizontalAlignment.Center, VerticalAlignment.Center);
        }
    }
}
