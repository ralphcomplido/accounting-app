using LightNap.Scaffolding.AssemblyManager;
using LightNap.Scaffolding.ProjectManager;
using LightNap.Scaffolding.ServiceRunner;
using System.CommandLine;
using System.CommandLine.Parsing;

Argument<string> classNameArgument =
    new("className",
        description: "The name of the entity to scaffold for");
Option<string> namespaceOption =
    new("--namespace",
        getDefaultValue: () => "LightNap.Core.Data.Entities",
        description: "The namespace of the entity");
Option<string> srcPathOption =
    new("--src-path",
        getDefaultValue: () => "./",
        description: "The path to the /src folder of the repo");
Option<string> coreProjectNameOption =
    new("--core-project",
        getDefaultValue: () => "LightNap.Core",
        description: "The name of the core project");
Option<string> webApiProjectNameOption =
    new("--web-api-project",
        getDefaultValue: () => "LightNap.WebApi",
        description: "The name of the web API project");
Option<string> angularProjectNameOption =
    new("--angular-project",
        getDefaultValue: () => "lightnap-ng",
        description: "The name of the Angular project");
Option<bool> skipComponentsOption =
    new("--skip-components",
        getDefaultValue: () => false,
        description: "Skip [re]generating Angular components");
Option<bool> overwriteOption =
    new("--overwrite",
        getDefaultValue: () => false,
        description: "Automatically overwrite files if they exist");


var rootCommand = new RootCommand()
{
    classNameArgument,
    namespaceOption,
    srcPathOption,
    coreProjectNameOption,
    webApiProjectNameOption,
    angularProjectNameOption,
    skipComponentsOption,
    overwriteOption
};

rootCommand.SetHandler((className, namespaceValue, srcPath, coreProjectName, webApiProjectName, angularProjectName, skipComponents, overwrite) =>
{
    ServiceRunner runner = new(new ProjectManager(), new AssemblyManager());
    runner.Run(new ServiceParameters($"{namespaceValue}.{className}", srcPath, namespaceValue, coreProjectName, webApiProjectName, angularProjectName, skipComponents, overwrite));
},
classNameArgument, namespaceOption, srcPathOption, coreProjectNameOption, webApiProjectNameOption, angularProjectNameOption, skipComponentsOption, overwriteOption);

return await rootCommand.InvokeAsync(args);