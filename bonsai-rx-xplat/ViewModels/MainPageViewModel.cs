using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Bonsai;
using Bonsai.Expressions;
using bonsai_rx_xplat.Models;

namespace bonsai_rx_xplat.ViewModels
{
    public class MainPageViewModel : IQueryAttributable
    {
        ExpressionBuilderGraph Workflow;
        IDisposable Running;

        public Command WorkflowTriggerCommand { get; private set; }
        public Command StartInteractionCommand { get; private set; }
        public Command CanvasInteractionCommand { get; private set; }
        public GraphViewCanvas GraphCanvas { get; set; }

        public MainPageViewModel()
        {
            StartInteractionCommand = new Command(eventArgs =>
            {
                var point = eventArgs as PointF[];
                foreach (var transform in GraphCanvas.DrawnTransforms)
                {
                    if (transform.ContainsPoint(point[0]))
                    {
                        transform.OnSelect();
                    }
                }
            });

            CanvasInteractionCommand = new Command(obj =>
            {
                var graphicsView = obj as GraphicsView;
                GraphCanvas.Redraw(graphicsView);
            });

            WorkflowTriggerCommand = new Command(WorkflowTrigger);

            // Temporary workflow build
            Workflow = new ExpressionBuilderGraph();
            var timer = Workflow.Add(new CombinatorBuilder { Combinator = new Bonsai.Reactive.Timer { Period = TimeSpan.FromSeconds(1) } });
            var debug = Workflow.Add(new CombinatorBuilder { Combinator = new DebugSink { } });
            var debug2 = Workflow.Add(new CombinatorBuilder { Combinator = new DebugSink { } });
            Workflow.AddEdge(timer, debug, new ExpressionBuilderArgument());

            // Draw information
            GraphCanvas = new GraphViewCanvas(Workflow);
        }

        private void WorkflowTrigger(object obj)
        {
            var button = (Button)obj;
            if (Running == null)
            {
                StartWorkflow();
                button.Text = "Stop";
            }
            else
            {
                StopWorkflow();
                button.Text = "Start";
            }
        }

        private void StartWorkflow()
        {
            Running = Workflow.BuildObservable().Subscribe();
        }

        private void StopWorkflow()
        {
            Running.Dispose();

            Running = null;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Node addNode = query["AddNode"] as Node;
        }

        public class GraphViewCanvas : IDrawable
        {
            public List<DrawnTransform> DrawnTransforms { get; set; } = new();
            int Offset = 10;
            int Spacing = 100;

            public GraphViewCanvas()
            {

            }

            public GraphViewCanvas(ExpressionBuilderGraph expressionBuilderGraph)
            {
                DrawnTransforms = new List<DrawnTransform>();
                int offset = Offset;
                System.Diagnostics.Debug.WriteLine(offset);
                foreach (var node in expressionBuilderGraph)
                {
                    DrawnTransforms.Add(new DrawnLabeledRectangle(new Point(offset, 10), 50, 50, Colors.Blue, Colors.Red, node.Value.ToString()));
                    offset += Spacing;
                }
            }

            public void Draw(ICanvas canvas, RectF dirtyRect)
            {
                foreach (var transform in DrawnTransforms)
                {
                    transform.Draw(canvas, dirtyRect);
                }
            }

            public void Redraw(GraphicsView graphicsView)
            {
                graphicsView.Invalidate();
            }
        }

        class DebugSink : Sink
        {
            public override IObservable<TSource> Process<TSource>(IObservable<TSource> source)
            {
                return source.Do(val => System.Diagnostics.Debug.WriteLine(val.ToString()));
            }
        }
    }
}
