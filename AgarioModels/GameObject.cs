using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
/// This file represents a Game Object that is any object in our Agario game.
/// </summary>
/// 
namespace AgarioModels
{
    public class GameObject
    {

        public float X { get; set; } 
        public float Y { get; set; }
        
        public int ARGBColor { get; set; }

        public long ID { get; set; }

        public float Mass { get; set; }

        public float radius { get; set; }

        public Vector2 location { get; set; }

        public GameObject() { }
}

    
}
