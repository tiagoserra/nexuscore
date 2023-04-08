using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Enums;

namespace Application.Common.Commands;

public class ResponseCommand
{
    public ResponseStatusCommand Status { get; } = ResponseStatusCommand.Ok;
    public object Result { get; }
    public Dictionary<string, string> Errors { get; } = new();

    public ResponseCommand(object result)
        => Result = result;

    public ResponseCommand(ResponseStatusCommand status)
        => Status = status;

    public void AddError(string code, string message)
    {
        if (!Errors.ContainsKey(code))
            Errors.Add(code, message);
    }

    public TEntity ConvertTo<TEntity>()
        => (TEntity)Result;
}