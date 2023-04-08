using Infrastructure.Manager;
using Infrastructure.Manager.Consts;
using Infrastructure.Manager.Enums;

async Task MenuEnvironment(string[] args = null)
{
    if (args is not null && args.Length == 1)
        args = null;

    EnvironmentDumpManager environmentChoose;

    if (args is null)
    {
        ConsoleDraw.DrawMenuEnvironment();
        bool isNumeric = int.TryParse(Console.ReadLine().ToString(), out int option);

        if (!isNumeric)
        {
            ConsoleDraw.DrawInvalidOption();
            await MenuEnvironment();
            return;
        }

        if (option is 0)
            return;

        EnvironmentDumpManager GetEnvironment(int option)
        {
            return option switch
            {
                1 => EnvironmentDumpManager.Development,
                2 => EnvironmentDumpManager.Staging,
                3 => EnvironmentDumpManager.Production,
                _ => EnvironmentDumpManager.Development
            };
        }

        await ActionMenu(GetEnvironment(option));
    }
    else
    {
        if (args.Length < 2)
        {
            ConsoleDraw.DrawFeedBack("Invalid paremeters");
            return;
        }

        EnvironmentDumpManager GetEnvironment(string option)
        {
            return option switch
            {
                "development" => EnvironmentDumpManager.Development,
                "staging" => EnvironmentDumpManager.Staging,
                "production" => EnvironmentDumpManager.Production,
                _ => EnvironmentDumpManager.Development
            };
        }

        EnvironmentDumpActionManager GetAction(string action)
        {
            return action switch
            {
                "reset" => EnvironmentDumpActionManager.Reset,
                "update" => EnvironmentDumpActionManager.Update,
                "dump" => EnvironmentDumpActionManager.Dump,
                _ => EnvironmentDumpActionManager.None
            };
        }

        environmentChoose = GetEnvironment(args[0]);

        EnvironmentDumpActionManager actionChoose = GetAction(args[1]);

        if (actionChoose is EnvironmentDumpActionManager.None)
            return;

        await ActionMenu(environmentChoose, actionChoose);
    }
}

async Task ActionMenu(EnvironmentDumpManager environment, EnvironmentDumpActionManager? action = null)
{
    if (action is not null && action is EnvironmentDumpActionManager.None)
        return;

    if (action is null)
    {
        ConsoleDraw.DrawMenuAction(environment.ToString());
        bool isNumeric = int.TryParse(Console.ReadLine().ToString(), out int actionChoose);

        if (!isNumeric)
        {
            ConsoleDraw.DrawInvalidOption();
            await ActionMenu(environment);
            return;
        }

        if (actionChoose is 0)
        {
            await MenuEnvironment();
            return;
        }

        EnvironmentDumpActionManager GetAction(int action)
        {
            return action switch
            {
                1 => EnvironmentDumpActionManager.Update,
                2 => EnvironmentDumpActionManager.Dump,
                3 => EnvironmentDumpActionManager.Reset,
                _ => EnvironmentDumpActionManager.None
            };
        }

        action = GetAction(actionChoose);
    }

    await Run(environment, action.Value);
}

async Task Run(EnvironmentDumpManager environment, EnvironmentDumpActionManager action)
{
    try
    {
        await new Startup(environment, action).RunAsync();
    }
    catch (Exception e)
    {
        ConsoleDraw.DrawException($"{e.Message} - {e.InnerException.Message} - {e.StackTrace.ToString()}");
    }
}

await MenuEnvironment(Environment.GetCommandLineArgs());
ConsoleDraw.DrawPoweredBy();