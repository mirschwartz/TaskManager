using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskManager.Data;

namespace _5_29TaskManager.Web
{
    public class TasksHub : Hub
    {
        public void AddTask(string title)
        {
            TaskManagerRepository repo = new TaskManagerRepository(Properties.Settings.Default.ConStr);
            Task task = new Task
            {
                Title = title,
                Status = false
            };
            int id = repo.AddTask(task);
            task.Id = id;
            Clients.All.newTask(task);
        }

        public void TaskChanged(int id)
        {
            string email = Context.User.Identity.Name;
            UserManager manager = new UserManager(Properties.Settings.Default.ConStr);
            User user = manager.GetUser(email);
            TaskManagerRepository repo = new TaskManagerRepository(Properties.Settings.Default.ConStr);
            Task task = repo.GetTask(id);
            if(task.Status == true)
            {
                if (user.Id == task.UserId)
                {
                    repo.RemoveTask(task.Id);
                    Clients.All.RemoveTask(task.Id);
                }
            }
            else
            {
                repo.MarkAsTaken(id, user.Id);
                Clients.All.MarkAsCompleted(new { Id = task.Id, Title = task.Title, Status = true, UserId = user.Id,
                    FirstName = user.FirstName, LastName = user.LastName });
            }
        }
    }
}