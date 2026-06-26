using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityChatbot
{
    public class TaskManager
    {
        private TaskStorageHelper _storage;
        private ActivityLogger _logger;

        public TaskManager(ActivityLogger logger)
        {
            _storage = new TaskStorageHelper();
            _logger = logger;
        }

        public string AddTask(string title, string description, string reminder)
        {
            _storage.AddTask(title, description, reminder);

            string logMessage = $"Task added: '{title}'";
            if (!string.IsNullOrEmpty(reminder))
                logMessage += $" (Reminder set for {reminder})";

            _logger.Log(logMessage);

            return $"Task '{title}' added successfully!";
        }

        public List<CyberTask> GetAllTasks()
        {
            return _storage.LoadTasks();
        }

        public string MarkAsComplete(int id)
        {
            CyberTask task = GetTaskById(id);
            if (task == null)
                return "Task not found.";

            _storage.MarkAsComplete(id);
            _logger.Log($"Task marked complete: '{task.Title}'");

            return $"Task '{task.Title}' marked as complete!";
        }

        public string DeleteTask(int id)
        {
            CyberTask task = GetTaskById(id);
            if (task == null)
                return "Task not found.";

            _storage.DeleteTask(id);
            _logger.Log($"Task deleted: '{task.Title}'");

            return $"Task '{task.Title}' deleted successfully!";
        }

        public CyberTask GetTaskById(int id)
        {
            return _storage.LoadTasks().Find(t => t.Id == id);
        }
    }
}
