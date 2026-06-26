using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CybersecurityChatbot
{
    public class TaskStorageHelper
    {
        private const string FilePath = "tasks.json";

        // LOAD TASKS
        public List<CyberTask> LoadTasks()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    return new List<CyberTask>();
                }

                string json = File.ReadAllText(FilePath);

                return JsonConvert.DeserializeObject<List<CyberTask>>(json)
                       ?? new List<CyberTask>();
            }
            catch
            {
                return new List<CyberTask>();
            }
        }

        // SAVE TASKS
        public void SaveTasks(List<CyberTask> tasks)
        {
            try
            {
                string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);

                File.WriteAllText(FilePath, json);
            }
            catch
            {
            }
        }

        // ADD TASK
        public void AddTask(string title, string description, string reminder)
        {
            List<CyberTask> tasks = LoadTasks();

            int newId = tasks.Count > 0
                ? tasks.Max(t => t.Id) + 1
                : 1;

            CyberTask task = new CyberTask()
            {
                Id = newId,
                Title = title,
                Description = description,
                Reminder = reminder,
                IsComplete = false,
                CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
            };

            tasks.Add(task);

            SaveTasks(tasks);
        }

        // MARK COMPLETE
        public void MarkAsComplete(int id)
        {
            List<CyberTask> tasks = LoadTasks();

            CyberTask task = tasks.FirstOrDefault(t => t.Id == id);

            if (task != null)
            {
                task.IsComplete = true;

                SaveTasks(tasks);
            }
        }

        // DELETE TASK
        public void DeleteTask(int id)
        {
            List<CyberTask> tasks = LoadTasks();

            tasks.RemoveAll(t => t.Id == id);

            SaveTasks(tasks);
        }
    }
}
