using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
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
    public class MainPageViewModel
    {
        ExpressionBuilderGraph Workflow;
        IDisposable Running;
        public ObservableCollection<Node> Nodes { get; set; } = new();

        public Command AddNodeCommand { get; private set; }
        public Command WorkflowTriggerCommand { get; private set; }

        public MainPageViewModel()
        {
            Nodes.Add(new Node { Name = "Timer" });
            Nodes.Add(new Node { Name = "Take" });

            AddNodeCommand = new Command(() => { System.Diagnostics.Debug.WriteLine("Add button clicked, not implemented"); });
            WorkflowTriggerCommand = new Command(WorkflowTrigger);
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
            Workflow = new ExpressionBuilderGraph();

            var timer = Workflow.Add(new CombinatorBuilder { Combinator = new Bonsai.Reactive.Timer { Period = TimeSpan.FromSeconds(1) } });
            var debug = Workflow.Add(new CombinatorBuilder { Combinator = new DebugSink { } });

            Workflow.AddEdge(timer, debug, new ExpressionBuilderArgument());

            Running = Workflow.BuildObservable().Subscribe();
        }

        private void StopWorkflow()
        {
            Running.Dispose();

            Running = null;
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
