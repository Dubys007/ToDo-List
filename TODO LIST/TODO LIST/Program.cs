using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime Date { get; set; }


}


class TodoContext : DbContext
{
    public DbSet<TodoItem> TodoItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=DESKTOP-IFH0N3N\\\\SQLEXPRESS;Initial Catalog=TASK4;Integrated Security=True;");
    }
}











class Program
{

    static void Main(string[] args)
    {
        using (var context = new TodoContext())
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Todo App");
                Console.WriteLine("1. Add new item");
                Console.WriteLine("2. Edit item");
                Console.WriteLine("3. Delete item");
                Console.WriteLine("4. Search items");
                Console.WriteLine("5. Exit");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddItem(context);
                        break;
                    case "2":
                        EditItem(context);
                        break;
                    case "3":
                        DeleteItem(context);
                        break;
                    case "4":
                        SearchItems(context);
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine();
            }
        }
    }

    static void AddItem(TodoContext context)
    {
        Console.Write("Enter title: ");
        string title = Console.ReadLine();
        Console.Write("Enter date (yyyy-mm-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
        {
            Console.WriteLine("Invalid date format. Item not added.");
            return;
        }

        TodoItem newItem = new TodoItem
        {
            Title = title,
            Date = date
        };

        context.TodoItems.Add(newItem);
        context.SaveChanges();
        Console.WriteLine("Item added successfully.");
    }

    static void EditItem(TodoContext context)
    {
        Console.Write("Enter the title of the item to edit: ");
        string searchTitle = Console.ReadLine();

        TodoItem item = context.TodoItems.FirstOrDefault(i => i.Title == searchTitle);
        if (item == null)
        {
            Console.WriteLine("Item not found.");
            return;
        }

        Console.WriteLine($"Found item: {item.Title} - {item.Date}");

        Console.Write("Enter new title (leave empty to keep current): ");
        string newTitle = Console.ReadLine();
        if (!string.IsNullOrEmpty(newTitle))
        {
            item.Title = newTitle;
        }

        Console.Write("Enter new date (yyyy-mm-dd) (leave empty to keep current): ");
        string newDateStr = Console.ReadLine();
        if (!string.IsNullOrEmpty(newDateStr))
        {
            if (!DateTime.TryParse(newDateStr, out DateTime newDate))
            {
                Console.WriteLine("Invalid date format. Item not updated.");
                return;
            }

            item.Date = newDate;
        }

        context.SaveChanges();
        Console.WriteLine("Item updated successfully.");
    }

    static void DeleteItem(TodoContext context)
    {
        Console.Write("Enter the title of the item to delete: ");
        string searchTitle = Console.ReadLine();

        TodoItem item = context.TodoItems.FirstOrDefault(i => i.Title == searchTitle);
        if (item == null)
        {
            Console.WriteLine("Item not found.");
            return;
        }

        context.TodoItems.Remove(item);
        context.SaveChanges();
        Console.WriteLine("Item deleted successfully.");
    }

    static void SearchItems(TodoContext context)
    {
        Console.WriteLine("Search options:");
        Console.WriteLine("1. Search by date");
        Console.WriteLine("2. Search by word");
        Console.Write("Enter your choice: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                SearchByDate(context);
                break;
            case "2":
                SearchByWord(context);
                break;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
    }

    static void SearchByDate(TodoContext context)
    {
        Console.Write("Enter date to search (yyyy-mm-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime searchDate))
        {
            Console.WriteLine("Invalid date format. Please try again.");
            return;
        }

        List<TodoItem> results = context.TodoItems.Where(i => i.Date.Date == searchDate.Date).ToList();

        if (results.Count == 0)
        {
            Console.WriteLine("No items found for the specified date.");
            return;
        }

        Console.WriteLine($"Found {results.Count} item(s) for the specified date:");
        foreach (var item in results)
        {
            Console.WriteLine($"{item.Title} - {item.Date}");
        }
    }

    static void SearchByWord(TodoContext context)
    {
        Console.Write("Enter word to search: ");
        string searchWord = Console.ReadLine();

        List<TodoItem> results = context.TodoItems.Where(i => i.Title.Contains(searchWord, StringComparison.OrdinalIgnoreCase)).ToList();

        if (results.Count == 0)
        {
            Console.WriteLine("No items found containing the specified word.");
            return;
        }

        Console.WriteLine($"Found {results.Count} item(s) containing the specified word:");
        foreach (var item in results)
        {
            Console.WriteLine($"{item.Title} - {item.Date}");
        }
    }

}





