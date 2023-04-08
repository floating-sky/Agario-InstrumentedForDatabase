using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileLogger
{
    public class CustomFileLogProvider : ILoggerProvider
    {
        /// <summary>
        /// Creates a new custom logger and then returns it to the user.
        /// </summary>
        /// <param name="categoryName">The type of logger you would like to create.</param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new CustomFileLogger(categoryName);
        }

        /// <summary>
        /// Closes the logger file.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
