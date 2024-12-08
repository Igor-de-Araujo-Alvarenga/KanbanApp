using KanbanApp.Models;
using KanbanApp.Services;
using KanbanApp.Services.Interfaces;
using KanbanApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace KanbanApp.Views;

public partial class KanbanPage : ContentPage
{
    MainPageViewModel _mainPageViewModel;
    public KanbanPage(MainPageViewModel mainPageViewModel)
	{
        InitializeComponent();
        _mainPageViewModel = mainPageViewModel;
        BindingContext = _mainPageViewModel;
    }

    protected override void OnAppearing()
    {
        _mainPageViewModel.InitializeTasks();
        base.OnAppearing();
    }

    public void OpenAdd(Object sender, EventArgs e)
    {
        _mainPageViewModel.OpenAddCommand.Execute(null);
    }

    public void OpenEdit(Object sender, EventArgs e) 
    {
        var button = (Button)sender;
        _mainPageViewModel.Task = (TaskItemModel)button.BindingContext;
        _mainPageViewModel.OpenEditCommand.Execute(null);
    }
    public void NextStatus(Object sender, EventArgs e) 
    {
        var button = (Button)sender;
        _mainPageViewModel.Task = (TaskItemModel)button.BindingContext;
        _mainPageViewModel.NextStatusCommand.Execute(null);
    }
}