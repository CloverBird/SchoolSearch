namespace ProcessingLargeAmountOfData
{
    public class SchoolSearch
    {
        // Список студентів, що будуть зчитані з файлу
        private List<Student> Students;
        // Словник назв команд та дій, що цим командам відповідають
        private readonly Dictionary<char, Action<string[]>> commandsDictionary;

        public SchoolSearch(string fileName)
        {
            // Якщо з ім'я файлу неправильне, або такого файлу немає, то нормально буде кинути виключення
            if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName))
                throw new ArgumentNullException("The file's name is invalid.");

            Students = new List<Student>();
            ReadFile(fileName);

            commandsDictionary = new Dictionary<char, Action<string[]>>
            {
                { 'S', CompleteCommandStudent },
                { 'T', CompleteCommandTeacher },
                { 'C', CompleteCommandClassroom },
                { 'B', CompleteCommandBus }
            };
        }

        #region fillingTheList
        public void ReadFile(string fileName)
        {
            var lines = File.ReadAllLines(fileName);

            for (int i = 0; i < lines.Length; i++)
            {
                var dividedFields = DivideLine(lines[i]);
                //якщо рядок пустий, то просто пропускаємо його
                if (dividedFields == null)
                    continue;

                Students.Add(new Student(dividedFields));
            }
        }
        
        public static string[] DivideLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return null;

            return line.Split(',');
        }
        #endregion

        // Метод, що запускає основні функції програми
        public void Run()
        {
            while (true)
            {
                PrintMenu();

                var answer = Console.ReadLine();
                if (answer == null || answer == string.Empty)
                {
                    Console.WriteLine("\nYou've entered a wrong command.\n");
                    continue;
                }

                if (answer == "Q" || answer == "Quit")
                    break;

                ChooseAndCompleteCommand(answer);
            }
        }

        private static void PrintMenu()
        {
            Console.WriteLine("Choose your action (divided by ':'):");
            Console.WriteLine("S[tudent]: <lastName> - find all students with this last name.");
            Console.WriteLine("S[tudent]:B[us]: <lastName> - find all students with this last name who go to school by this bus route.");
            Console.WriteLine("T[eacher]: <lastName> - find all students those are teached by teacher with this last name");
            Console.WriteLine("C[lassroom]: <number> - find all students int this classroom");
            Console.WriteLine("B[us]: <number> - find all students go to school by this bus route.");
            Console.WriteLine("Q[uit] - quit the program.");
        }

        #region commandsBasePack
        private void ChooseAndCompleteCommand(string answer)
        {
            var command = answer.Replace(" ", "").Split(':');

            if (commandsDictionary.ContainsKey(command[0][0]))
                commandsDictionary[command[0][0]].Invoke(command);
            else
                Console.WriteLine("\nYou've entered the wrong command. Please choose again\n");
        }

        // метод знаходить студентів у основному полі Students, що підходять за заданою умовою condition
        private List<Student> FindStudents(Func<Student, bool> condition)
        {
            var students = new List<Student>();

            foreach (var student in Students)
                if (condition(student))
                    students.Add(student);

            return students;
        }
        // метод друкує студентів, формат виводу залежить від action, що передається до методу як параметр
        private void PrintStudents(List<Student> students, Action<Student> printFields)
        {
            foreach (var student in students)
                printFields(student);

            Console.WriteLine("\n");
        }

        private static bool CommandExists(string command, string fullCommandName)
        {
            return !(command.Length > 1 && command != fullCommandName);
        }
        #endregion

        #region commands
        // друкує студентів, що мають задане прізвище
        private void CompleteCommandStudent(string[] command)
        {
            if (!CommandExists(command[0], "Student")) return;

            List<Student> students;
            // Перевіряємо чи є друга команда
            if (command.Length == 3)
            {
                // якщо існує друга команда і вона правильна, то до друкування додаємо колонку "Bus"
                if (command[1][0] == 'B' && CommandExists(command[1], "Bus"))
                {
                    students = FindStudents(student => student.LastName.ToLower() == command[2].ToLower());
                    PrintStudents(students,
                       student => Console.WriteLine($"|{student,-18}|{student.Grade,-2}|{student.Classroom,-4}|{student.Bus,-3}|{student.Teacher,-19}|"));
                    return;
                }

                else
                {
                    Console.WriteLine("\nYou've entered the wrong command. Please choose again\n");
                    return;
                }
            }
            // якщо тільки одна команда задана
            students = FindStudents(student => student.LastName.ToLower() == command[1].ToLower());
            PrintStudents(students,
                student => Console.WriteLine($"|{student,-18}|{student.Grade,-2}|{student.Classroom,-4}|{student.Teacher,-19}|"));
        }
        // друкує студентів, вчитель яких має задане прізвище
        private void CompleteCommandTeacher(string[] command)
        {
            if (!CommandExists(command[0], "Teacher")) return;

            var students = FindStudents(student => student.Teacher.LastName.ToLower() == command[1].ToLower());

            PrintStudents(students,
                student => Console.WriteLine($"|{student,-18}|{student.Teacher,-19}|"));
        }
        // друкує студентів, що навчаються в заданій класній кімнаті
        private void CompleteCommandClassroom(string[] command)
        {
            if (!CommandExists(command[0], "Classroom")) return;

            int number = int.Parse(command[1]);

            var students = FindStudents(student => student.Classroom == number);

            PrintStudents(students,
                student => Console.WriteLine($"|{student,-18}|{student.Classroom,-4}|"));
        }
        // друкує студентів, що їдуть за особовим автобусним маршрутом
        private void CompleteCommandBus(string[] command)
        {
            if (!CommandExists(command[0], "Bus")) return;

            int bus = int.Parse(command[1]);

            var students = FindStudents(student => student.Bus == bus);

            PrintStudents(students,
                student => Console.WriteLine($"|{student,-18}|{student.Grade,-2}|{student.Classroom,-4}|{student.Bus,-3}|"));
        }
        #endregion
    }
}