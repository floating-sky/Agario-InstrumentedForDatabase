using System.Text.Json;
namespace TowardAgarioStepTwo
{
    
    public class Program
    {
        static void Main(string[] args) 
        {
            Person person = new Student("Julia", 3, 2);


            string message = JsonSerializer.Serialize<Person>(person, new JsonSerializerOptions {WriteIndented = true} );

            Console.WriteLine(message);
            Person temp = JsonSerializer.Deserialize<Person>(message) 
            ?? throw new Exception("bad json"); ;
        }
    }
}
