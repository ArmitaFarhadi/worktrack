Console.WriteLine("=== Value Type ===");

int a = 10;
int b = a;
b = 20;

Console.WriteLine($"a = {a}");
Console.WriteLine($"b = {b}");

Console.WriteLine();
Console.WriteLine("=== Reference Type ===");

Person p1 = new Person { Name = "Sara" };
Person p2 = p1;

p2.Name = "Mary";

Console.WriteLine($"p1.Name = {p1.Name}");
Console.WriteLine($"p2.Name = {p2.Name}");

Console.WriteLine();
Console.WriteLine("=== Reference Reassignment ===");

p2 = new Person { Name = "John" };

Console.WriteLine($"p1.Name = {p1.Name}");
Console.WriteLine($"p2.Name = {p2.Name}");

Console.WriteLine();
Console.WriteLine("=== String Immutability ===");

string s1 = "Sara";
string s2 = s1;

s2 = s2.ToUpper();

Console.WriteLine($"s1 = {s1}");
Console.WriteLine($"s2 = {s2}");

Console.WriteLine();
Console.WriteLine("=== Equality ===");

Person e1 = new Person { Name = "Sara" };
Person e2 = new Person { Name = "Sara" };
Person e3 = e1;

Console.WriteLine($"e1 == e2: {e1 == e2}");
Console.WriteLine($"e1.Equals(e2): {e1.Equals(e2)}");
Console.WriteLine($"ReferenceEquals(e1, e2): {ReferenceEquals(e1, e2)}");

Console.WriteLine($"e1 == e3: {e1 == e3}");
Console.WriteLine($"ReferenceEquals(e1, e3): {ReferenceEquals(e1, e3)}");

Console.WriteLine();
Console.WriteLine("=== Parameter Passing ===");

Person person = new Person { Name = "Sara" };

ChangeName(person);
Console.WriteLine($"After ChangeName: {person.Name}");

ReplacePerson(person);
Console.WriteLine($"After ReplacePerson: {person.Name}");

ReplacePersonByRef(ref person);
Console.WriteLine($"After ReplacePersonByRef: {person.Name}");

Console.WriteLine();
Console.WriteLine("=== ref / out / in ===");

int number = 10;
ChangeNumber(ref number);
Console.WriteLine($"ref number: {number}");

CreateNumber(out int createdNumber);
Console.WriteLine($"out number: {createdNumber}");

ReadNumber(in number);

static void ChangeName(Person p)
{
    p.Name = "Mary";
}

static void ReplacePerson(Person p)
{
    p = new Person { Name = "John" };
}

static void ReplacePersonByRef(ref Person p)
{
    p = new Person { Name = "John" };
}

static void ChangeNumber(ref int value)
{
    value += 5;
}

static void CreateNumber(out int value)
{
    value = 50;
}

static void ReadNumber(in int value)
{
    Console.WriteLine($"in number: {value}");
}

class Person
{
    public string Name { get; set; } = "";
}