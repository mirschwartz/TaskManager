using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Data
{
    public class TaskManagerRepository
    {
        private string _connection { get; set; }

        public TaskManagerRepository(string connection)
        {
            _connection = connection;
        }

        public IEnumerable<Task> GetAllTasks()
        {
            using (var context = new TasksDBDataContext(_connection))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Task>(t => t.User);
                context.LoadOptions = loadOptions;
                return context.Tasks.ToList();
            }
        }

        public int AddTask(Task task)
        {
            using (var context = new TasksDBDataContext(_connection))
            {
                context.Tasks.InsertOnSubmit(task);
                context.SubmitChanges();
                return task.Id;
            }
        }

        public void MarkAsTaken(int taskId, int userId)
        {
            using (var context = new TasksDBDataContext(_connection))
            {
                context.ExecuteCommand("UPDATE Tasks SET Status = 'true', UserId = {0} WHERE Id = {1}", userId, taskId);
            }
        }

        public void RemoveTask(int id)
        {
            using (var context = new TasksDBDataContext(_connection))
            {
                context.ExecuteCommand("DELETE FROM Tasks WHERE Id = {0}", id);
            }
        }

        public Task GetTask(int id)
        {
            using (var context = new TasksDBDataContext(_connection))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Task>(t => t.User);
                context.LoadOptions = loadOptions;
                return context.Tasks.FirstOrDefault(t => t.Id == id);
            }
        }
    }
}