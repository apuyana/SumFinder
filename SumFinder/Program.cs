using SumFinder;
using System.Text.Json;

// Load input from config.

string configPath = "appSettings.json";

if (args?.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
{
    string commandPath = args[0];
    Console.WriteLine($"Try to load settings from path {commandPath}");

    if (File.Exists(commandPath))
    {
        configPath = commandPath;
    }
    else
    {
        Console.WriteLine($"Path {commandPath} could not be found");
    }
}

FinderSettings? finderSettings = null;

if (File.Exists(configPath))
{
    string configContent = await File.ReadAllTextAsync(configPath);
    finderSettings = JsonSerializer.Deserialize<FinderSettings>(configContent);
}

if (finderSettings == null)
{
    Console.WriteLine($"No settings found in {configPath}, default path is 'appSettings.json'");
    return;
}

var finder = new Finder();

IList<string>? results = await finder.SumWithTargetAsync(
    numbers: finderSettings.Value.numbers,
    target: finderSettings.Value.target);

for (int i = 0; i < results?.Count; i++)
{
    if (results[0].Length > 0)
    {
        Console.WriteLine($"+ {results[i]}");
    }
}