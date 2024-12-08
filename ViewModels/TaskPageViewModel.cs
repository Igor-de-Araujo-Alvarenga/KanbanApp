using KanbanApp.Models;
using KanbanApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KanbanApp.ViewModels
{
    public class TaskPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ITaskService taskService;
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

        public ICommand SaveTaskCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public TaskPageViewModel(ITaskService taskService, TaskItemModel task = null)
        {
            this.taskService = taskService;
            Task = task ?? new TaskItemModel();
            SaveTaskCommand = new Command(SaveTask);
            CancelCommand = new Command(Cancel);    
        }

        private async void SaveTask()
        {
            if (string.IsNullOrWhiteSpace(Task.Title))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Title is required.", "OK");
                return;
            }

            if (Task.Id == 0)
                Task.Status = EnumStatusModels.Ready.ToString();
            
            taskService.SaveTask(Task);
            await App.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void Cancel()
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
        }


        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
