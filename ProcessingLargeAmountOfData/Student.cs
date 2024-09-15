namespace ProcessingLargeAmountOfData
{
    public class Student : Person
    {
        public int Grade { get; private set; }

        public int Classroom { get; private set; }

        public int Bus { get; private set; }

        public Teacher Teacher { get; private set; }

        public Student(string[] fields) : base(fields[0], fields[1])
        {
            Grade = int.Parse(fields[2]);
            Classroom = int.Parse(fields[3]);
            Bus = int.Parse(fields[4]);
            Teacher = new Teacher(fields[5], fields[6]);
        }

        // не використовуваний метод, проте дуже корисний (скоріше за все), якщо б ми розширювали далі програму
        public void PrintStudent()
        {
            Console.WriteLine($"{base.ToString()},{Grade},{Classroom},{Bus},{Teacher}");
        }
    }
}