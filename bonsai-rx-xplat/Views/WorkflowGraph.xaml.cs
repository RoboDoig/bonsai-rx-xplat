using bonsai_rx_xplat.ViewModels;

namespace bonsai_rx_xplat.Views;

public partial class WorkflowGraph : ContentView
{
	public WorkflowGraph()
	{
		InitializeComponent();
	}

	private void Redraw(object sender, TouchEventArgs e)
	{
		System.Diagnostics.Debug.WriteLine("Redraw");
		WorkflowView.Invalidate();
	}
}