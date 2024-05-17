using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace DailyManager
{
    public class Task // class with all task variables
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class TaskManager
    {
        private const string FilePath = "tasks.json"; //path to JSON file

        public List<Task> LoadTasks()
        {
            if (!File.Exists(FilePath))
            {
                return new List<Task>(); // creating new JSON file
            }

            var json = File.ReadAllText(FilePath);
            return
                JsonConvert.DeserializeObject<List<Task>>(
                    json); //  method converts string into from JSON into a list of Task type objects
        }

        public void SaveTasks(List<Task> tasks)
        {
            var json = JsonConvert.SerializeObject(tasks,
                Formatting.Indented); // method converts list of variable into string text for JSON
            File.WriteAllText(FilePath, json); // write all converted text into file
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var taskManager = new TaskManager();
            var tasks = taskManager.LoadTasks();
            while (true) // safe loop for serving as main manu
            {
                Console.WriteLine("--------------------");
                Console.WriteLine("1. Show all Tasks");
                Console.WriteLine("2. Add Task");
                Console.WriteLine("3. Edit Task");
                Console.WriteLine("4. Del. Task");
                Console.WriteLine("5. Save & Exit");
                var choice = Console.ReadLine();
                Console.WriteLine("--------------------");
                switch (choice)
                {
                    case "1":
                        ViewTasks(tasks);
                        break;

                    case "2":
                        AddTask(tasks);
                        break;

                    case "3":
                        EditTask(tasks);
                        break;

                    case "4":
                        DeleteTask(tasks);
                        break;

                    case "5":
                        taskManager.SaveTasks(tasks);
                        return;
                }
            }
        }

        private static void ViewTasks(List<Task> tasks) // View all tasks
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("No Task to show.");
                return;
            }

            Console.WriteLine("Task list:");
            for (int i = 0; i < tasks.Count; i++)
            {
                var task = tasks[i];
                Console.WriteLine($"{i + 1}. Title: {task.Title}");
                Console.WriteLine($"   Description: {task.Description}");
                Console.WriteLine($"   Deadline: {task.Deadline:yyyy-MM-dd}");
                Console.WriteLine($"   Status: {(task.IsCompleted ? "Done" : "Not done")}");
                Console.WriteLine();
            }
        }

        private static void AddTask(List<Task> tasks) // Adding new task
        {
            Console.Write("Enter Task name: ");
            var title = Console.ReadLine();
            Console.WriteLine();
            Console.Write("Enter Task description: ");
            var description = Console.ReadLine();
            Console.WriteLine();
            Console.Write("Enter Deadline date (yyyy-mm-dd): ");
            var deadlineInput = Console.ReadLine();
            DateTime deadline;
            while (!DateTime.TryParse(deadlineInput, out deadline))
            {
                Console.Write("Wrong date format, Try again (yyyy-mm-dd): ");
                deadlineInput = Console.ReadLine();
            }
            Console.WriteLine();
            var newTask = new Task
            {
                Title = title,
                Description = description,
                Deadline = deadline,
                IsCompleted = false
            };

            tasks.Add(newTask);
            Console.WriteLine("Task added successfully!");
        }

        private static void EditTask(List<Task> tasks) // Editing existent task
        {
            Console.WriteLine("Choose Task to edit by # from list:");
            Console.WriteLine();
            ViewTasks(tasks);
            if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
            {
                var task = tasks[taskNumber - 1]; // Get the task by the specified number
                Console.WriteLine("Chose line to edit:");
                while (true)
                {
                    Console.WriteLine("--------------------");
                    Console.WriteLine("1. Name");
                    Console.WriteLine("2. Description");
                    Console.WriteLine("3. Deadline");
                    Console.WriteLine("4. Status");
                    Console.WriteLine("5. End Operation");
                    var e_choice = Console.ReadLine();
                    if (e_choice == "5")
                    {
                    }
                    else
                    {
                        Console.WriteLine("--------------------"); //Purely visual, preventing "--------------" from repeating
                    }

                    switch (e_choice)
                    {
                        case "1":
                            Console.Write("Enter the new task title (leave blank to keep the current): ");
                            var title = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(title))
                            {
                                task.Title = title; // NAME CHANGE
                            }

                            break;

                        case "2":
                            Console.Write("Enter the new task description (leave blank to keep the current): ");
                            var description = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(description))
                            {
                                task.Description = description; // DESCRIPTION CHANGE
                            }

                            break;

                        case "3":
                            Console.Write(
                                "Enter the new task deadline (yyyy-MM-dd, leave blank to keep the current): ");
                            var deadlineInput = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(deadlineInput))
                            {
                                DateTime deadline;
                                if (DateTime.TryParse(deadlineInput, out deadline))
                                {
                                    task.Deadline = deadline; // DEADLINE CHANGE
                                }
                                else
                                {
                                    Console.WriteLine("Invalid date format. Deadline not changed.");
                                }
                            }

                            break;

                        case "4":
                            Console.Write("Is the task completed? (yes/no, leave blank to keep the current): ");
                            var isCompletedInput = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(isCompletedInput))
                            {
                                if (isCompletedInput.ToLower() == "yes")
                                {
                                    task.IsCompleted = true; // STATUS CHANGE TO "YES"
                                }
                                else if (isCompletedInput.ToLower() == "no")
                                {
                                    task.IsCompleted = false; // STATUS CHANGE TO "NO"
                                }
                                else
                                {
                                    Console.WriteLine("Invalid input. Completion status not changed.");
                                }
                            }

                            break;

                        case "5":
                            return;
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid task number.");
            }
        }

        private static void DeleteTask(List<Task> tasks) // Delete existent task
        {
            Console.WriteLine("Choose Task to delete by # from list:");
            Console.WriteLine();
            ViewTasks(tasks);
            if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
            {
                var task = tasks[taskNumber - 1];
                Console.WriteLine();
                Console.WriteLine("Are you sure you want to delete this task? (yes/no): "); // Confirm deletion
                var confirmation = Console.ReadLine();
                if (confirmation.ToLower() == "yes")
                {
                    tasks.RemoveAt(taskNumber - 1); // Remove the task from the list
                    Console.WriteLine();
                    Console.WriteLine("Task deleted successfully!");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Task deletion cancelled.");
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Invalid task number.");
            }
        }
    }
}