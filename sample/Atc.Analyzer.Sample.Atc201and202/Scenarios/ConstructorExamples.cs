namespace Atc.Analyzer.Sample.Atc201and202.Scenarios;

internal class ConstructorExamples
{
    private readonly string name;
    private readonly int age;
    private readonly bool isActive;
    private readonly DateTime createdDate;

    // Valid: Constructor with two parameters on separate lines
    public ConstructorExamples(
        string name,
        int age)
    {
        this.name = name;
        this.age = age;
        isActive = true;
        createdDate = DateTime.UtcNow;
    }

    // Valid: Constructor with multiple parameters on separate lines
    public ConstructorExamples(
        string name,
        int age,
        bool isActive,
        DateTime createdDate)
    {
        this.name = name;
        this.age = age;
        this.isActive = isActive;
        this.createdDate = createdDate;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Constructor params: {name}, {age}, {isActive}, {createdDate}");
    }
}