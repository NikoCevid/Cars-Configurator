using Dao.Models;
using System.Collections.Concurrent;
using System.Text;
using System.Text.RegularExpressions;

namespace Cars.Services
{
    public class LogService
    {
        private readonly ConcurrentQueue<LogEntry> _logQueue = new();
        private readonly string _logFilePath;

        public LogService(IWebHostEnvironment env)
        {
            _logFilePath = Path.Combine(env.WebRootPath, "logs.txt");

            // Ako datoteka ne postoji, stvori je
            if (!File.Exists(_logFilePath))
            {
                File.Create(_logFilePath).Close();
            }

            // ⬇️ UČITAJ POSTOJEĆE LOGOVE IZ DATOTEKE U MEMORIJU
            var lines = File.ReadAllLines(_logFilePath);
            foreach (var line in lines)
            {
                // Format: [timestamp] LEVEL: message
                var match = Regex.Match(line, @"^\[(.*?)\]\s+(\w+):\s+(.*)$");
                if (match.Success)
                {
                    _logQueue.Enqueue(new LogEntry
                    {
                        Timestamp = DateTime.Parse(match.Groups[1].Value),
                        Level = match.Groups[2].Value,
                        Message = match.Groups[3].Value
                    });
                }
            }
        }

        public void Log(string level, string message)
        {
            var log = new LogEntry
            {
                Id = 0,
                Timestamp = DateTime.Now,
                Level = level,
                Message = message
            };

            _logQueue.Enqueue(log);

            var logLine = $"[{log.Timestamp:O}] {level.ToUpper()}: {message}{Environment.NewLine}";
            File.AppendAllText(_logFilePath, logLine, Encoding.UTF8);
        }

        public List<LogEntry> GetLast(int count)
        {
            return _logQueue.Reverse().Take(count).ToList();
        }

        public int Count()
        {
            return _logQueue.Count;
        }
    }
}
