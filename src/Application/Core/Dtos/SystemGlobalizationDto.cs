namespace Application.Core.Dtos;

public record SystemGlobalizationDto(string Key, Dictionary<string, string> Resource) { }