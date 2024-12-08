using KanbanApp.ViewModels;
using KanbanApp.Views;

namespace KanbanApp;

public partial class App : Application
{
    public App()
	{
		InitializeComponent();
        MainPage = new AppShell();
	}

}
