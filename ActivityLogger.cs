using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityChatbot
{
    public class ActivityLogger
    {
        private List<string> _log = new List<string>();

        public void Log(string action)
        {
            string entry = $"[{DateTime.Now:HH:mm}] {action}";
            _log.Add(entry);
        }

        public string GetRecentLog(int count = 10)
        {
            var recent = _log.TakeLast(Math.Min(count, _log.Count)).ToList();

            if (recent.Count == 0)
                return "No activities logged yet.";

            string result = "Here's a summary of recent actions:\n";
            for (int i = 0; i < recent.Count; i++)
            {
                result += $"{i + 1}. {recent[i]}\n";
            }

            return result.TrimEnd();
        }

        public string GetFullLog()
        {
            if (_log.Count == 0)
                return "No activities logged yet.";

            string result = "Complete activity history:\n";
            for (int i = 0; i < _log.Count; i++)
            {
                result += $"{i + 1}. {_log[i]}\n";
            }

            return result.TrimEnd();
        }

        public int GetCount()
        {
            return _log.Count;
        }
    }
}
