using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TowardAgarioStepTwo
{
    internal class Program
    {
        static Person person = new Person();
        string message = JsonSerializer.Serialize<Person>(person);
        Console.WriteLine(message);
    }
}
