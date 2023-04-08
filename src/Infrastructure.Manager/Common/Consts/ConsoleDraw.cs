using Infrastructure.Manager.Enums;

namespace Infrastructure.Manager.Consts;

public static class ConsoleDraw
{
    public const string lineHead = "#####################################################################################";
    public const string lineFooter = "############################################################################# V 2023.0";
    public const string linePoweredBy = "\n Powered by Serra Tiago";
    public const string img = @" 
                         __________________
                        |◊ :             : ◊|
                        |  : Dump        :  |
                        |  : DataBase    :  |
                        |  : Reset       :  |
                        |  :_____________:  |
                        |     ___________   |
                        |    | __        |  |
                        |    ||  |       |  |
                        \____||__|_______|__|

                      Welcome to Infrastructure Manager !
                ";

    public const string Environment = "----------------------Environments-------------------------- \n";
    public const string Options = "----------------------Options-------------------------- \n";
    public const string Line = "------------------------------------------------------- \n";

    public static void DrawHeader()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(lineHead);
        Console.WriteLine(img);
        Console.WriteLine(lineFooter);
        Console.ResetColor();
        Console.WriteLine("\n");
    }

    public static void DrawMenuEnvironment()
    {
        DrawHeader();
        Console.WriteLine(Environment);
        Console.WriteLine("\t 0 - Exit");
        Console.WriteLine("\t 1 - Development");
        Console.WriteLine("\t 2 - Staging");
        Console.WriteLine("\t 3 - Production");
        Console.Write("\t => ");
    }

    public static void DrawMenuAction(string environment)
    {
        DrawHeader();
        Console.WriteLine(Options);
        Console.WriteLine($"\t Environment: {environment}");
        Console.WriteLine("-----------");
        Console.WriteLine("\t 0 - Back");
        Console.WriteLine("\t 1 - Update database and run dump");
        Console.WriteLine("\t 2 - Run dump only");

        if (environment == EnvironmentDumpManager.Development.ToString())
            Console.WriteLine("\t 3 - Reset");
    }

    public static void DrawInvalidOption()
    {
        DrawHeader();
        Console.ResetColor();
        Console.WriteLine("\n");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid option, please choose again");
        Console.ReadKey();
        Console.ResetColor();
    }

    public static void DrawException(string message)
    {
        Console.WriteLine(Line);
        Console.WriteLine("\n");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.WriteLine("\n");
        Console.WriteLine("Error {•̃_•̃}");
    }

    public static void DrawEnvironment(string environment = "local")
    {
        Console.WriteLine(Line);
        Console.WriteLine($"Environment:  {environment}");
    }

    public static void DrawFeedBack(string message)
    {
        Console.WriteLine(Line);
        Console.WriteLine(message);
    }

    public static void DrawPoweredBy()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n");
        Console.WriteLine("Powered by Serra Tiago");
    }
}