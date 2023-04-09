using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TowardAgarioStepTwo
{
    [JsonDerivedType(typeof(Person), typeDiscriminator: "Person")]
    [JsonDerivedType(typeof(Student), typeDiscriminator: "Student")]
    public class Person
    {
        public int ID { get; protected set; } = 1;
        public string Name { get; protected set; } = "Jim";


        [JsonConstructor]
        public Person(string Name) 
        {
            this.Name = Name;
            this.ID = ID + 1;
        }
    }
}
