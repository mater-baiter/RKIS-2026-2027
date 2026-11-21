using System;

namespace TodoListApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнил Гарибов и Свирид");

            Console.WriteLine("Введите имя:");
            string firstName = Console.ReadLine();

            Console.WriteLine("Введите фамилию:");
            string secondName = Console.ReadLine();

            Console.WriteLine("Введите год рождения");
            int birthYear = Convert.ToInt32(Console.ReadLine());
            int currentYear = DateTime.Now.Year;
            int age = currentYear - birthYear;
            
            Console.WriteLine($"Добавлен новый пользователь {firstName} {secondName}, возраст - {age}");

            string[] todos = new string[2];
            int count = 0;

            while (true)
            {
                Console.WriteLine();
                Console.Write("Введите команду (help — список команд): ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Пустая команда. Введите 'help' для списка команд.");
                    continue;
                }

                string[] parts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                string command = parts[0].ToLower();

                switch (command)
                {
                    case "help":
                        PrintHelp();
                        break;

                    case "profile":
                        Console.WriteLine($"{firstName} {secondName}, {birthYear}");
                        break;

                    case "add":
                        if (parts.Length < 2)
                        {
                            Console.WriteLine("Формат: add \"текст задачи\"");
                            break;
                        }


                        string raw = parts[1].Trim();
                        string taskText = raw.Trim('"');

                        if (string.IsNullOrWhiteSpace(taskText))
                        {
                            Console.WriteLine("Текст задачи не может быть пустым.");
                            break;
                        }


                        if (count >= todos.Length)
                        {

                            string[] newTodos = new string[todos.Length * 2];
                            for (int i = 0; i < todos.Length; i++)
                            {
                                newTodos[i] = todos[i];
                            }
                            todos = newTodos;
                            Console.WriteLine($"[Расширение массива до {todos.Length} элементов]");
                        }

                        todos[count] = taskText;
                        count++;
                        Console.WriteLine($"Задача добавлена: \"{taskText}\"");
                        break;

                    case "view":
                        Console.WriteLine("Список задач:");
                        bool anyTasks = false;
                        for (int i = 0; i < count; i++)
                        {
                            if (!string.IsNullOrWhiteSpace(todos[i]))
                            {
                                Console.WriteLine($"{i + 1}. {todos[i]}");
                                anyTasks = true;
                            }
                        }
                        if (!anyTasks)
                        {
                            Console.WriteLine("(пусто)");
                        }
                        break;

                    case "exit":
                        Console.WriteLine("Выход из программы. До свидания!");
                        return;

                    default:
                        Console.WriteLine($"Неизвестная команда: '{command}'. Введите 'help'.");
                        break;
                }
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("  help            — список всех команд");
            Console.WriteLine("  profile         — данные пользователя (<Имя> <Фамилия>, <Год рождения>)");
            Console.WriteLine("  add \"текст\"     — добавить новую задачу");
            Console.WriteLine("  view            — показать все задачи");
            Console.WriteLine("  exit            — завершить программу");

        }
    }
}  //привет я поменял код

