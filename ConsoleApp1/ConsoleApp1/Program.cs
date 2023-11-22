using System;
using System.IO;

public class ConsoleExplorer
{
    private string currentPath;
    private int selectedOption;

    public ConsoleExplorer()
    {
        try
        {
            currentPath = Directory.GetCurrentDirectory();
            selectedOption = 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка инициализации: {ex.Message}");
            Environment.Exit(1);
        }
    }

    public void Run()
    {
        while (true)
        {
            try
            {
                DisplayCurrentContents();
                HandleInput();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка выполнения: {ex.Message}");
            }
        }
    }

    private void DisplayCurrentContents()
    {
        Console.Clear();
        Console.WriteLine($"Содержимое {currentPath}:");

        string[] directories = Directory.GetDirectories(currentPath);
        string[] files = Directory.GetFiles(currentPath);

        for (int i = 0; i < directories.Length; i++)
        {
            Console.ForegroundColor = (i == selectedOption && i < directories.Length) ? ConsoleColor.Yellow : ConsoleColor.White;
            Console.WriteLine($"[ПАПКА] {Path.GetFileName(directories[i])}");
        }

        for (int i = 0; i < files.Length; i++)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[ФАЙЛ] {Path.GetFileName(files[i])}");
        }

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\nИспользуйте стрелки вверх и вниз для выбора, Enter для открытия. Нажмите 'Esc', чтобы вернуться назад.");
    }

    private void HandleInput()
    {
        ConsoleKeyInfo key = Console.ReadKey(true);

        switch (key.Key)
        {
            case ConsoleKey.UpArrow:
                if (selectedOption > 0)
                {
                    selectedOption--;
                }
                break;

            case ConsoleKey.DownArrow:
                if (selectedOption < Directory.GetDirectories(currentPath).Length - 1)
                {
                    selectedOption++;
                }
                break;

            case ConsoleKey.Enter:
                NavigateToSelectedOption();
                break;

            case ConsoleKey.Escape:
                NavigateUp();
                break;
        }
    }

    private void NavigateUp()
    {
        try
        {
            currentPath = Directory.GetParent(currentPath)?.FullName ?? currentPath;
            selectedOption = 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при переходе вверх: {ex.Message}");
        }
    }

    private void NavigateToSelectedOption()
    {
        try
        {
            string[] directories = Directory.GetDirectories(currentPath);

            if (directories.Length > 0 && selectedOption >= 0 && selectedOption < directories.Length)
            {
                currentPath = directories[selectedOption];
                selectedOption = 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при переходе: {ex.Message}");
        }
    }
}

public class Program
{
    public static void Main()
    {
        ConsoleExplorer explorer = new ConsoleExplorer();
        explorer.Run();
    }
}
