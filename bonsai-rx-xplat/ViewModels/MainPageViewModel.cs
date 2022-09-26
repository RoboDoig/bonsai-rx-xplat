using System.ComponentModel;
using System.Reactive.Linq;
using Bonsai.Expressions;
using bonsai_rx_xplat.Models;

namespace bonsai_rx_xplat.ViewModels
{
    public class MainPageViewModel : IQueryAttributable, INotifyPropertyChanged
    {
        ExpressionBuilderGraph Workflow;
        IDisposable Running;

        public Command WorkflowTriggerCommand { get; private set; }
        public Command StartInteractionCommand { get; private set; }
        public Command EndInteractionCommand { get; private set; }
        public Command DragInteractionCommand { get; private set; }
        public Command CanvasInteractionCommand { get; private set; }
        public Command DoubleTapCommand { get; private set; }
        public GraphViewCanvas GraphCanvas { get; set; }

        private GraphNode LastSelectedGraphNode;
        private GraphNode currentSelectedGraphNode;
        public GraphNode CurrentSelectedGraphNode
        {
            get
            {
                return currentSelectedGraphNode;
            }
            set
            {
                currentSelectedGraphNode = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentSelectedGraphNode)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentGraphNodeProperties)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public PropertyData[] CurrentGraphNodeProperties
        {
            get
            {
                if (CurrentSelectedGraphNode != null)
                {
                    var builder = ExpressionBuilder.Unwrap(CurrentSelectedGraphNode.Node.Value);
                    var workflowElement = ExpressionBuilder.GetWorkflowElement(builder);
                    var instance = workflowElement ?? builder;
                    return instance.GetType().GetProperties().Select(x => new PropertyData { PropertyInformation = x, Target = instance}).ToArray();
                }
                return null;
            }
        }

        public MainPageViewModel()
        {
            System.Diagnostics.Debug.WriteLine(DeviceInfo.Platform);

            // Start interaction
            StartInteractionCommand = new Command(eventArgs =>
            {
                var point = eventArgs as PointF[];
                foreach (var graphNode in GraphCanvas.GraphNodes)
                {
                    if (graphNode.ContainsPoint(point[0]))
                    {
                        graphNode.OnSelect(point[0]);
                        CurrentSelectedGraphNode = graphNode;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentGraphNodeProperties)));

                        if (LastSelectedGraphNode != null && LastSelectedGraphNode != graphNode)
                        {
                            LastSelectedGraphNode.OnDeselect();
                        }

                        LastSelectedGraphNode = graphNode;
                        return;
                    }
                }
            });

            // Drag interaction
            DragInteractionCommand = new Command(eventArgs =>
            {
                var point = eventArgs as PointF[];
                foreach (var graphNode in GraphCanvas.GraphNodes)
                {
                    if (graphNode.ContainsPoint(point[0]) && graphNode == CurrentSelectedGraphNode)
                    {
                        graphNode.OnDrag(point[0]);
                        return;
                    }
                }
            });

            // End interaction
            EndInteractionCommand = new Command(eventArgs =>
            {
                var point = eventArgs as PointF[];
                foreach (var graphNode in GraphCanvas.GraphNodes)
                {
                    if (graphNode.ContainsPoint(point[0]) && graphNode != CurrentSelectedGraphNode)
                    {
                        // reset position
                        CurrentSelectedGraphNode.SnapbackPosition();

                        // add / remove edge
                        // if no edge to target exists, create it
                        if (!CurrentSelectedGraphNode.Node.Successors.Any(x => x.Target == graphNode.Node))
                        {
                            Workflow.AddEdge(CurrentSelectedGraphNode.Node, graphNode.Node, new ExpressionBuilderArgument());
                        }
                        // else remove the existing edge
                        else
                        {
                            Workflow.RemoveEdge(CurrentSelectedGraphNode.Node, CurrentSelectedGraphNode.Node.Successors.Where(x => x.Target == graphNode.Node).First());
                        }
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
            if (query.Count > 0)
            {
                Node addNode = query["AddNode"] as Node;
                Workflow.Add(addNode.Builder());
                GraphCanvas.UpdateGraphViewCanvas(Workflow);
            }
        }

        public class GraphViewCanvas : IDrawable
        {
            public List<GraphNode> GraphNodes = new();
            int Offset = 10;
            int Spacing = 100;

            public GraphViewCanvas()
            {

            }

            public GraphViewCanvas(ExpressionBuilderGraph expressionBuilderGraph)
            {
                GraphNodes = new List<GraphNode>();
                int offset = Offset;
                foreach (var node in expressionBuilderGraph)
                {
                    GraphNodes.Add(new GraphNode(node, new Point(offset, 10)));
                    offset += Spacing;
                }
            }

            public void UpdateGraphViewCanvas(ExpressionBuilderGraph expressionBuilderGraph)
            {
                GraphNodes = new List<GraphNode>();
                int offset = Offset;
                foreach (var node in expressionBuilderGraph)
                {
                    GraphNodes.Add(new GraphNode(node, new Point(offset, 10)));
                    offset += Spacing;
                }
            }

            public void Draw(ICanvas canvas, RectF dirtyRect)
            {
                foreach (var graphNode in GraphNodes)
                {
                    // Draw the node itself
                    graphNode.Draw(canvas, dirtyRect);

                    // Draw successor edges
                    foreach (var edge in graphNode.Node.Successors)
                    {
                        var targetGraphNode = GraphNodes.Where(x => x.Node == edge.Target).First();
                        canvas.StrokeSize = 2;
                        canvas.StrokeColor = Colors.Black;
                        canvas.DrawLine(graphNode.DrawnTransform.CenterOrigin, targetGraphNode.DrawnTransform.CenterOrigin);
                    }
                }
            }

            public void Redraw(GraphicsView graphicsView)
            {
                graphicsView.Invalidate();
            }
        }
    }
}
