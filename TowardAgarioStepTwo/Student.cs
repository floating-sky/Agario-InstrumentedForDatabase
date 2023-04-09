using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowardAgarioStepTwo
{
    internal class Student : Person
    {
        public float GPA { get; private set; } = 4;

        public Student(string Name, float GPA, int ID) : base(Name)
        {
            this.GPA = GPA;
        }
    }
}
