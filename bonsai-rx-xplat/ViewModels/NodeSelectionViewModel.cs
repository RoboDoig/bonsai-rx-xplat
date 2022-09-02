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
    public class NodeSelectionViewModel
    {

        public Command GoToMainPageCommand { get; private set; }

        public NodeSelectionViewModel()
        {
            GoToMainPageCommand = new Command(async () => { await Shell.Current.GoToAsync("///MainPage"); });
        }
    }
}
