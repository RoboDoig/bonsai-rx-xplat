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
    [QueryProperty("AddNode", "AddNode")]
    public class MainPageViewModel
    {
        ExpressionBuilderGraph Workflow;
        IDisposable Running;

        private Node addNode;
        public Node AddNode {
            get { return addNode; }
            set
            {
                addNode = value;
                System.Diagnostics.Debug.WriteLine(addNode.Name);
            }
        }

        public Command WorkflowTriggerCommand { get; private set; }
        public Command StartInteractionCommand { get; private set; }
        public GraphViewCanvas GraphCanvas { get; set; }

        public MainPageViewModel()
        {
            StartInteractionCommand = new Command(eventArgs =>
            {
                var point = eventArgs as PointF[];
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
                    DrawnTransforms.Add(new DrawnLabeledRectangle(new Point(offset, 10), 50, 50, 6, Colors.Blue, Colors.Blue, node.Value.ToString()));
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
