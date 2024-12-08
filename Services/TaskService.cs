
using KanbanApp.Models;
using KanbanApp.Services.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic;

namespace KanbanApp.Services
{
    public class TaskService : ITaskService
    {
        private readonly string _connectionString; 

        public TaskService()
        {
            SQLitePCL.Batteries.Init();
            _connectionString = $"Data Source=E:/repo_sharp/KanbanAppdatabase.db";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS TaskItemModel (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Description TEXT NOT NULL,
                DueDate TIMESTAMP,
                Status TEXT
            )";
            using var command = new SqliteCommand(createTableQuery, connection);
            command.ExecuteNonQuery();
        }

        public List<TaskItemModel> GetTasks()
        {
            var tasks = new List<TaskItemModel>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            string query = "SELECT Id, Title, Description, DueDate, Status FROM TaskItemModel";
            using var command = new SqliteCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                tasks.Add(new TaskItemModel
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1), 
                    Description = reader.GetString(2), 
                    DueDate = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3), 
                    Status = reader.IsDBNull(4) ? null : reader.GetString(4) 

                });
            }

            return tasks;
        }

        public int SaveTask(TaskItemModel task)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();      

            string query = task.Id == 0
                ? "INSERT INTO TaskItemModel (Title,Description, DueDate, Status) VALUES (@Title, @Description, @DueDate, @Status)"
                : "UPDATE TaskItemModel SET Title = @Title, Description = @Description, DueDate = @DueDate, Status = @Status WHERE Id = @Id";
                
            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Title", task.Title);
            command.Parameters.AddWithValue("@DueDate", task.DueDate);
            command.Parameters.AddWithValue("@Status", task.Status);
            command.Parameters.AddWithValue("@Description", task.Description);
            if (task.Id != 0) command.Parameters.AddWithValue("@Id", task.Id);

            return command.ExecuteNonQuery();
        }

        public int DeleteTask(TaskItemModel task)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();        

            string query = "DELETE FROM TaskItem WHERE Id = @Id";
            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Id", task.Id);

            return command.ExecuteNonQuery();
        }
    }

}
