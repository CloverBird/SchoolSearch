namespace ProcessingLargeAmountOfData
{
    // base class to Student and Teacher
    public class Person
    {
        public string LastName { get; private set; }

        public string FirstName { get; private set; }

        public Person(string lastName, string firstName)
        {
            LastName = lastName;
            FirstName = firstName;
        }

        // щоб зробити легшим виведення в інших місцях програми
        public override string ToString()
        {
            return $"{LastName} {FirstName}";
        }
    }
}