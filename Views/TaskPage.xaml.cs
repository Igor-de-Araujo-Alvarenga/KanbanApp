using KanbanApp.Models;
using KanbanApp.Services;
using KanbanApp.Services.Interfaces;
using KanbanApp.ViewModels;

namespace KanbanApp.Views;

public partial class TaskPage : ContentPage
{
    public TaskPage(ITaskService taskService, TaskItemModel task = null)
    {
        InitializeComponent();
        BindingContext =  new TaskPageViewModel(taskService, task);
        Status.IsVisible = task != null;
        BtnSaveTask.Text = task == null ? "Save" : "Update"; 
    }
}