using Bonsai;
using Bonsai.Expressions;
using bonsai_rx_xplat.Models;
using bonsai_rx_xplat.ViewModels;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace bonsai_rx_xplat.Views;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}

