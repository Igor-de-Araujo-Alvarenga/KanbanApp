using KanbanApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanApp.Services.Interfaces
{
    public interface ITaskService
    {
        public List<TaskItemModel> GetTasks();
        public int SaveTask(TaskItemModel task);
        public int DeleteTask(TaskItemModel task);
    }
}
