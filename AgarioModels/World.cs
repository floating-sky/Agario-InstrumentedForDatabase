using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Author:    Julia Thomas
/// Partner:   C Wyatt Bruchhauser
/// Date:      4-16-2022
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and C Wyatt Bruchhauser - This work may not 
///            be copied for use in Academic Coursework.
///
/// I, C Wyatt Bruchhauser and Julia Thomas, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
///
/// File Contents
///
/// This wrapper represents the World all objects in our game belong in.
/// </summary>
/// 
namespace AgarioModels
{
    internal class World
    {
        private readonly ILogger _logger;

        public readonly int width;
        public readonly int height;

        /// <summary>
        /// Sets the world height and width to 5000
        /// </summary>
        /// <param name="logger"></param>
        public World(ILogger logger) 
        {
            _logger = logger;
            width =  5000;
            height = 5000;
        }
    }
}
