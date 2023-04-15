using Microsoft.Extensions.Localization;
using Application.Common.Commands;
using Application.Common.Enums;
using Application.Common.Interfaces;
using Domain.Common.Entities;
using Infrastructure.Manager.Interfaces;
using MediatR;

namespace Infrastructure.Manager.Common.Dumps;

public abstract class Dump<TEntity, TQuery, TCommand> : IDump
    where TEntity : Entity
    where TQuery : IRequest<ResponseCommand>
    where TCommand : IRequest<ResponseCommand>
{
    private IExecutionContext _executionContext;
    protected readonly IStringLocalizer Localizer;
    protected readonly IMediator Mediator;
    protected readonly int _order;
    public int Order => _order;

    protected Dump(IExecutionContext executionContext, IStringLocalizer localizer, IMediator mediator, string dumpName, int order = 0)
    {
        _executionContext = executionContext;
        Localizer = localizer;
        Mediator = mediator;

        _order = order;

        Console.WriteLine("\t" + dumpName);
    }

    protected virtual async Task<bool> CanSaveAsync(TQuery query)
    {
        try
        {
            var result = await Mediator.Send(query);

            return result is null || result.Status == ResponseStatusCommand.NotFound;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    protected virtual async Task SaveAsync(TQuery query, TCommand command)
    {
        if (await CanSaveAsync(query))
        {
            var result = (ResponseCommand)await Mediator.Send(command);

            if (result.Status == ResponseStatusCommand.Error)
                foreach (var error in result.Errors)
                    PrintError(string.Format("{0} - {1}", error.Key, error.Value));
        }
    }

    public virtual Task DumpAsync()
        => Task.FromResult(true);

    protected virtual void PrintError(string errorMessage)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(errorMessage);
        Console.ForegroundColor = ConsoleColor.Green;
    }

    public virtual string GetTemplate(string template, string path = "")
    {
        var result = "";
        var filePath = Environment.CurrentDirectory;

        if (!string.IsNullOrEmpty(path))
            filePath += "/Dumps/Templates/" + path + "/" + template;
        else
            filePath += "/Dumps/Templates/" + template;

        if (!File.Exists(filePath)) return result;

        var fileStream = new FileStream(filePath, FileMode.Open);

        using StreamReader reader = new(fileStream);
        result = reader.ReadToEnd();

        return result;
    }

}