namespace Edu.Unisabana.Tyvs.Domain.Model;

public class Person
{
    public string Name { get; }
    public int Id { get; }
    public int Age { get; }
    public Gender Gender { get; }
    public bool IsAlive { get; }

    public Person(string name, int id, int age, Gender gender, bool isAlive)
    {
        Name = name;
        Id = id;
        Age = age;
        Gender = gender;
        IsAlive = isAlive;
    }
}
