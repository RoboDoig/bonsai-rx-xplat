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
using bonsai_rx_xplat.Views;

namespace bonsai_rx_xplat.ViewModels
{
    public class NodeSelectionViewModel
    {
        public ObservableCollection<Node> Nodes { get; set; } = new();
        public Command GoToMainPageCommand { get; private set; }
        public Command<Node> SelectNodeCommand { get; private set; }

        public NodeSelectionViewModel()
        {
            Nodes.Add(new Node { Name = "Timer", Builder = () => new CombinatorBuilder { Combinator = new Bonsai.Reactive.Timer { Period = TimeSpan.FromSeconds(1) } } });
            Nodes.Add(new Node { Name = "Take", Builder = () => new CombinatorBuilder { Combinator = new Bonsai.Reactive.Take { Count = 5 } } });
            //Nodes.Add(new Node { Name = "TakeUntil" });
            //Nodes.Add(new Node { Name = "SelectMany" });
            //Nodes.Add(new Node { Name = "CameraCapture" });
            //Nodes.Add(new Node { Name = "Arduino" });
            //Nodes.Add(new Node { Name = "Router" });
            //Nodes.Add(new Node { Name = "Dealer" });

            GoToMainPageCommand = new Command(async () => { await Shell.Current.GoToAsync("///MainPage", true); });

            SelectNodeCommand = new Command<Node>(async node =>
            {
                // Navigate to main page with selected node 
                await Shell.Current.GoToAsync($"///{nameof(MainPage)}", true,
                    new Dictionary<string, object>
                    {
                        {"AddNode", node }
                    }    
                );
            });
        }
    }
}
