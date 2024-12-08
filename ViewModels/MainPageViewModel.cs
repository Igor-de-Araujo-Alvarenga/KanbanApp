using KanbanApp.Models;
using KanbanApp.Services;
using KanbanApp.Services.Interfaces;
using KanbanApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KanbanApp.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private ITaskService taskService;
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<TaskItemModel> Tasks { get; set; } = new ObservableCollection<TaskItemModel>();

        public ObservableCollection<TaskItemModel> ReadyTasks { get; set; } = new ObservableCollection<TaskItemModel>();
        public ObservableCollection<TaskItemModel> InProgressTasks { get; set; } = new ObservableCollection<TaskItemModel>();
        public ObservableCollection<TaskItemModel> DoneTasks { get; set; } = new ObservableCollection<TaskItemModel>();
        public ICommand OpenEditCommand;
        public ICommand NextStatusCommand;
        public ICommand OpenAddCommand;

        private TaskItemModel _task;

        public TaskItemModel Task
        {
            get => _task;
            set
            {
                _task = value;
                OnPropertyChanged();
            }
        }

        public MainPageViewModel(ITaskService _taskService)
        {
            taskService = _taskService;
            OpenEditCommand = new Command(OpenEditTask);
            OpenAddCommand = new Command(OpenAddTask);
            NextStatusCommand = new Command(NextStatus);
        }
        public void InitializeTasks()
        {
            Tasks.Clear();
            foreach (var task in taskService.GetTasks())
                Tasks.Add(task);

            UpdateFilteredTasks();
        }

        private void UpdateFilteredTasks()
        {
            ReadyTasks.Clear();
            InProgressTasks.Clear();
            DoneTasks.Clear();

            foreach (var task in Tasks)
            {
                switch (task.Status.ToLower().Replace(" ", ""))
                {
                    case "ready":
                        ReadyTasks.Add(task);
                        break;
                    case "inprogress":
                        InProgressTasks.Add(task);
                        break;
                    case "done":
                        DoneTasks.Add(task);
                        break;
                }
            }

            OnPropertyChanged(nameof(ReadyTasks));
            OnPropertyChanged(nameof(InProgressTasks));
            OnPropertyChanged(nameof(DoneTasks));
        }


        private async void OpenEditTask()
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new TaskPage(taskService, Task));
        }
        private async void OpenAddTask()
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new TaskPage(taskService));
        }

        private void NextStatus()
        {
            if (Task.Status.ToLower().Replace(" ", "") == EnumStatusModels.InProgress.ToString().ToLower())
                Task.Status = "Done";
            if (Task.Status == EnumStatusModels.Ready.ToString())
                Task.Status = "In progress";
            

            taskService.SaveTask(Task);
            InitializeTasks();
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
