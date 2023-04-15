using Microsoft.Extensions.Localization;
using Application.Common.Interfaces;
using Application.Core.CommandQueries;
using Application.Core.Commands;
using Domain.Core.Entities;
using MediatR;

namespace Infrastructure.Manager.Core.Dumps;

internal class SystemGlobalizationDump : Dump<SystemGlobalization, GetSystemGlobalizationByKeyCommandQuery, SystemGlobalizationRegisterCommand>
{
    public SystemGlobalizationDump(IExecutionContext executionContext, IStringLocalizer localizer, IMediator mediator)
        :base(executionContext, localizer, mediator, "SystemGlobalizationDump")
    {
    }

    public override async Task DumpAsync()
    {
        await Save("Common:Message:Action:NotAllowed", new Dictionary<string, string>() { { "pt-BR", "Ação não permitida, comunique o administrador." }, { "en-US", "Action not allowed, notify administrator." } });
        await Save("Common:Message:Error:AlreadyExists", new Dictionary<string, string>() { { "pt-BR", "O valor {0} já existe cadastrado." }, { "en-US", "The value {0} already exists registered." } });
        await Save("Common:Message:Error:MaxLength:20000", new Dictionary<string, string>() { { "pt-BR", "Máximo 20000 caracteres." }, { "en-US", "Maximum 20000 characters." } });
        await Save("Common:Message:Error:MaxLength:255", new Dictionary<string, string>() { { "pt-BR", "Máximo 255 caracteres." }, { "en-US", "Maximum 255 characters." } });
        await Save("Common:Message:Error:MinLength:1", new Dictionary<string, string>() { { "pt-BR", "Mínimo 1 caracterer." }, { "en-US", "Minimum 1 character." } });
        await Save("Common:Message:Error:MinMaxLengthCustom", new Dictionary<string, string>() { { "pt-BR", "Campo {0} precisa ter entre {1} e {2} caractere(s)." }, { "en-US", "Field {0} must have between {1} and {2} character(s)." } });
        await Save("Common:Message:Error:NotFound:Id", new Dictionary<string, string>() { { "pt-BR", "Id {0} não encontrado." }, { "en-US", "Id {0} not found." } });
        await Save("Common:Message:Error:NotFound:Id:Field", new Dictionary<string, string>() { { "pt-BR", "Id {0} não encontrado para (1})" }, { "en-US", "Id {0} not found. (1})." } });
        await Save("Common:Message:Error:Object:Referenced", new Dictionary<string, string>() { { "pt-BR", "Existem referencias pata este cadastro, por este motivo não é permitido a exclusão." }, { "en-US", "There are references to this registration, for this reason the exclusion is not allowed." } });
        await Save("Common:Message:Required", new Dictionary<string, string>() { { "pt-BR", "Preenchimento obrigatório." }, { "en-US", "Mandatory filling." } });
        await Save("Common:Message:Required:Field", new Dictionary<string, string>() { { "pt-BR", "Campo {0} é obrigatório." }, { "en-US", "Field {0} is mandatory." } });
        await Save("Common:Message:Success:Operation", new Dictionary<string, string>() { { "pt-BR", "Operação realizada com sucesso !" }, { "en-US", "Operation successful !" } });
        await Save("Common:Message:Forbidden", new Dictionary<string, string>() { { "pt-BR", "Acesso negado" }, { "en-US", "Access forbidden" } });
    }

    private async Task Save(string key, Dictionary<string, string> resource)
        =>  await SaveAsync(new GetSystemGlobalizationByKeyCommandQuery(key), new SystemGlobalizationRegisterCommand(key, resource));
}