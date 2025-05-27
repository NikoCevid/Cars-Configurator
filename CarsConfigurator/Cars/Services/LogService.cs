using System.Collections.Concurrent;
using Dao.Models;

namespace Cars.Services
{
    public class LogService
    {
        private static readonly ConcurrentQueue<LogEntry> _logs = new();
        private static int _counter = 0;

        public void Log(string level, string message)
        {
            var entry = new LogEntry
            {
                Id = ++_counter,
                Timestamp = DateTime.Now,
                Level = level,
                Message = message
            };

            _logs.Enqueue(entry);
        }

        public IEnumerable<LogEntry> GetLastNLogs(int n)
        {
            return _logs.Reverse().Take(n);
        }

        public int Count()
        {
            return _logs.Count;
        }
    }
}
