using KanbanApp.Views;

namespace KanbanApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(TaskPage), typeof(TaskPage));
	}
}
