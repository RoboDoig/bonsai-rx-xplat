using Bonsai.Dag;
using Bonsai.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bonsai_rx_xplat.Models
{
    public class GraphNode
    {
        public Bonsai.Dag.Node<ExpressionBuilder, ExpressionBuilderArgument> Node;
        public DrawnTransform DrawnTransform;
        public List<GraphNode> Successors = new();

        private PointF StartInteractionPosition;

        public GraphNode(Node<ExpressionBuilder, ExpressionBuilderArgument> node, PointF position)
        {
            Node = node;
            DrawnTransform = new DrawnLabeledRectangle(position, 50, 50, Colors.Blue, Colors.Red, node.Value.ToString());
        }

        public bool ContainsPoint(PointF point) => DrawnTransform.ContainsPoint(point);
        public void OnSelect(PointF point)
        {
            DrawnTransform.OnSelect(point);
            StartInteractionPosition = DrawnTransform.Origin;
        }
        public void OnDeselect() => DrawnTransform.OnDeselect();
        public void OnDrag(PointF point)
        {
            DrawnTransform.OnDrag(point);
        }
        public void SetPosition(PointF point) => DrawnTransform.SetPosition(point);
        public void SnapbackPosition()
        {
            DrawnTransform.SetPosition(StartInteractionPosition);
        }
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            DrawnTransform.Draw(canvas, dirtyRect);
        }
    }
}
