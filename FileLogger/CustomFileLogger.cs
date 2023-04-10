using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileLogger
{
    class CustomFileLogger : ILogger
    {
        private readonly string fileName;
        private readonly string categoryName;

        /// <summary>
        /// Creates a new file logger object.
        /// </summary>
        /// <param name="categoryName">The type of message being sent.</param>
        public CustomFileLogger(string categoryName)
        {
            fileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) //TODO: Change to ApplicationData folder before submission
                + Path.DirectorySeparatorChar
                + $"CS3500-{categoryName}.log";

            this.categoryName = categoryName;
        }

        /// <summary>
        /// Allows the user to provide additional context to sections of their code
        /// by setting tags to nested areas of the code.
        /// </summary>
        /// <param name="state">What we want to be written.</param>
        /// <returns>A disposable object to be used in a using statement that
        /// represents a region of logging code.</returns>
        /// <exception cref="NotImplementedException"> This exception will always be thrown as
        /// we are not implementing this method.</exception>
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new log entry that is appended onto the file.
        /// </summary>
        /// <param name="logLevel">The level of importance of the message.</param>
        /// <param name="eventId">ID of the event you</param>
        /// <param name="state">The information you want in the log entry.</param>
        /// <param name="exception">The exception related to this entry</param>
        /// <param name="formatter">A Function that creates a string message of the state and exception.</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string date = DateTime.Now.ToString("yyyy'-'MM'-'dd h:mm:ss tt");
            string message = formatter(state, exception);
            string threadID = Thread.CurrentThread.ManagedThreadId.ToString();
            string logLevelFirstFive = new string(logLevel.ToString().Take(5).ToArray());

            File.AppendAllText(fileName, $"{date} ({threadID}) - {logLevelFirstFive} - {formatter(state, exception)} \n");
        }
    }
}
