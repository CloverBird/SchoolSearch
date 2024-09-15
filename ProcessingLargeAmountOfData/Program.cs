using ProcessingLargeAmountOfData;

class Program
{
    public static void Main()
    {
        // Починається робота програми
        var schoolsearch = new SchoolSearch("students.txt");

        schoolsearch.Run();
    }
}