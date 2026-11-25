using System;

namespace TodoListApp
{
    class Program
    {
        // Константы вместо "магических значений"
        private const int InitialCapacity = 2;
        private const int GrowthFactor = 2;
        private const string StatusDoneText = "сделано";
        private const string StatusNotDoneText = "не сделано";
        private const string DateFormat = "dd.MM.yyyy HH:mm";

        // Данные приложения вынесены на уровень класса для удобного доступа из методов
        private static string[] todos = new string[InitialCapacity];
        private static bool[] statuses = new bool[InitialCapacity];
        private static DateTime[] dates = new DateTime[InitialCapacity];
        private static int taskCount = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Гарибов и Свирид");

            Console.WriteLine("Введите имя:");
            string firstName = Console.ReadLine();

            Console.WriteLine("Введите фамилию:");
            string lastName = Console.ReadLine();

            Console.WriteLine("Введите год рождения:");
            int birthYear = Convert.ToInt32(Console.ReadLine());

            int age = DateTime.Now.Year - birthYear;
            Console.WriteLine($"Добавлен новый пользователь {firstName} {lastName}, возраст - {age}");

            RunCommandLoop(firstName, lastName, birthYear);
        }

        private static void RunCommandLoop(string firstName, string lastName, int birthYear)
        {
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

                string[] commandParts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                string command = commandParts[0].ToLower();
                string arguments = commandParts.Length > 1 ? commandParts[1].Trim() : string.Empty;

                switch (command)
                {
                    case "help":
                        PrintHelp();
                        break;
                    case "profile":
                        PrintProfile(firstName, lastName, birthYear);
                        break;
                    case "add":
                        AddTask(arguments);
                        break;
                    case "view":
                        ViewTasks();
                        break;
                    case "done":
                        MarkTaskAsDone(arguments);
                        break;
                    case "delete":
                        DeleteTask(arguments);
                        break;
                    case "update":
                        UpdateTask(arguments);
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

        private static void PrintHelp()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("  help — список всех команд");
            Console.WriteLine("  profile — данные пользователя (<Имя> <Фамилия>, <Год рождения>)");
            Console.WriteLine("  add \"текст\" — добавить новую задачу");
            Console.WriteLine("  view — показать все задачи");
            Console.WriteLine("  done <индекс> — отметить задачу выполненной");
            Console.WriteLine("  delete <индекс> — удалить задачу");
            Console.WriteLine("  update <индекс> \"новый текст\" — изменить текст задачи");
            Console.WriteLine("  exit — завершить программу");
        }

        private static void PrintProfile(string firstName, string lastName, int birthYear)
        {
            Console.WriteLine($"{firstName} {lastName}, {birthYear}");
        }

        private static void AddTask(string arguments)
        {
            string taskText = arguments.Trim('"');
            if (string.IsNullOrWhiteSpace(taskText))
            {
                Console.WriteLine("Формат: add \"текст задачи\". Текст не может быть пустым.");
                return;
            }

            if (taskCount >= todos.Length)
            {
                ExpandArrays();
            }

            todos[taskCount] = taskText;
            statuses[taskCount] = false;
            dates[taskCount] = DateTime.Now;
            taskCount++;

            Console.WriteLine($"Задача добавлена: \"{taskText}\"");
        }

        private static void ViewTasks()
        {
            Console.WriteLine("Список задач:");
            if (taskCount == 0)
            {
                Console.WriteLine("(пусто)");
                return;
            }

            for (int i = 0; i < taskCount; i++)
            {
                string statusText = statuses[i] ? StatusDoneText : StatusNotDoneText;
                string formattedDate = dates[i].ToString(DateFormat);
                Console.WriteLine($"{i + 1} {todos[i]} {statusText} {formattedDate}");
            }
        }

        private static void MarkTaskAsDone(string arguments)
        {
            if (!TryParseTaskIndex(arguments, out int index))
            {
                Console.WriteLine("Ошибка: укажите корректный номер задачи. Формат: done <индекс>");
                return;
            }

            statuses[index] = true;
            dates[index] = DateTime.Now;
            Console.WriteLine($"Задача №{index + 1} отмечена как выполненная.");
        }

        private static void DeleteTask(string arguments)
        {
            if (!TryParseTaskIndex(arguments, out int index))
            {
                Console.WriteLine("Ошибка: укажите корректный номер задачи. Формат: delete <индекс>");
                return;
            }

            string deletedTaskText = todos[index];

            // Сдвигаем элементы всех трёх массивов влево
            for (int i = index; i < taskCount - 1; i++)
            {
                todos[i] = todos[i + 1];
                statuses[i] = statuses[i + 1];
                dates[i] = dates[i + 1];
            }

            // Очищаем последний дублирующийся элемент
            taskCount--;
            todos[taskCount] = null;
            statuses[taskCount] = false;
            dates[taskCount] = default;

            Console.WriteLine($"Задача \"{deletedTaskText}\" удалена.");
        }

        private static void UpdateTask(string arguments)
        {
            string[] updateParts = arguments.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            if (updateParts.Length < 2 || !TryParseTaskIndex(updateParts[0], out int index))
            {
                Console.WriteLine("Формат: update <индекс> \"новый текст задачи\"");
                return;
            }

            string newText = updateParts[1].Trim().Trim('"');
            if (string.IsNullOrWhiteSpace(newText))
            {
                Console.WriteLine("Новый текст задачи не может быть пустым.");
                return;
            }

            todos[index] = newText;
            dates[index] = DateTime.Now;
            Console.WriteLine($"Задача №{index + 1} обновлена.");
        }

        private static void ExpandArrays()
        {
            int newCapacity = todos.Length * GrowthFactor;

            string[] newTodos = new string[newCapacity];
            bool[] newStatuses = new bool[newCapacity];
            DateTime[] newDates = new DateTime[newCapacity];

            for (int i = 0; i < todos.Length; i++)
            {
                newTodos[i] = todos[i];
                newStatuses[i] = statuses[i];
                newDates[i] = dates[i];
            }

            todos = newTodos;
            statuses = newStatuses;
            dates = newDates;

            Console.WriteLine($"[Расширение массивов до {newCapacity} элементов]");
        }

        // Вспомогательный метод для проверки индекса (пользователь вводит с 1, в массиве с 0)
        private static bool TryParseTaskIndex(string input, out int index)
        {
            index = -1;
            if (int.TryParse(input, out int userNumber) && userNumber >= 1 && userNumber <= taskCount)
            {
                index = userNumber - 1;
                return true;
            }
            return false;
        }
    }
}

 