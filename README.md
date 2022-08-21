Sample project
==============

Tools
-----
.Net core is required to run the project [Download](https://dotnet.microsoft.com/en-us/download)

How to build
------------
Use visual studio or the following command on the root path of the project.
```
dotnet build
```

How to run the unit tests
------------
Use visual studio or the following command on the root path of the project.
```
dotnet test
```

How to run the project
------------
Use visual studio or the following command on the root path of the project.

```
dotnet run --project  .\SumFinder\SumFinder.csproj {path settings file}
```

For example

```
dotnet run --project  .\SumFinder\SumFinder.csproj .\SumFinder\appSettings.json
```

The app is expecting a .json file with the following format:

```
{
  "numbers": [ 1, 9, 5, 0, 20, -4, 12, 16, 7 ],
  "target": 12
}
```