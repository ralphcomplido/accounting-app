using LightNap.Scaffolding.AssemblyManager;
using LightNap.Scaffolding.ProjectManager;
using LightNap.Scaffolding.TemplateManager;
using LightNap.Scaffolding.Templates;

namespace LightNap.Scaffolding.ServiceRunner
{
    /// <summary>
    /// Runs the service scaffolding process.
    /// </summary>
    public class ServiceRunner(IProjectManager projectManager, IAssemblyManager assemblyManager)
    {
        /// <summary>
        /// Executes the service scaffolding process with the provided parameters.
        /// </summary>
        /// <param name="parameters">The parameters for the service scaffolding process.</param>
        public void Run(ServiceParameters parameters)
        {
            if (!ValidateParameters(parameters))
            {
                return;
            }

            var projectBuildResult = projectManager.BuildProject(parameters.CoreProjectFilePath);
            if (!projectBuildResult.Success)
            {
                Console.WriteLine("Please fix the build failures and try again.");
                return;
            }

            var type = assemblyManager.LoadType(projectBuildResult.OutputAssemblyPath!, parameters.ClassName);
            if (type == null)
            {
                Console.WriteLine($"Type '{parameters.ClassName}' not found or could not be loaded from assembly.");
                return;
            }

            Console.WriteLine($"Analyzing {type.Name} ({type.FullName})");

            List<TypePropertyDetails> propertiesDetails = TypeHelper.GetPropertyDetails(type);
            TemplateParameters templateParameters = new(type.Name, propertiesDetails, parameters);

            string pascalNamePlural = templateParameters.PascalNamePlural;
            string kebabName = templateParameters.KebabName;
            string kebabNamePlural = templateParameters.KebabNamePlural;

            string executingPath = assemblyManager.GetExecutingPath();

            var templateItems = new List<TemplateItem>
                {
                    new(new CreateDto() { Parameters = templateParameters }, $"{parameters.CoreProjectPath}/{pascalNamePlural}/Dto/Request/Create{type.Name}Dto.cs"),
                    new(new Dto() { Parameters = templateParameters }, $"{parameters.CoreProjectPath}/{pascalNamePlural}/Dto/Response/{type.Name}Dto.cs"),
                    new(new Extensions() { Parameters = templateParameters },$"{parameters.CoreProjectPath}/{pascalNamePlural}/Extensions/{type.Name}Extensions.cs"),
                    new(new Interface() { Parameters = templateParameters }, $"{parameters.CoreProjectPath}/{pascalNamePlural}/Interfaces/I{type.Name}Service.cs"),
                    new(new SearchDto() { Parameters = templateParameters }, $"{parameters.CoreProjectPath}/{pascalNamePlural}/Dto/Request/Search{type.Name}Dto.cs"),
                    new(new Service() { Parameters = templateParameters }, $"{parameters.CoreProjectPath}/{pascalNamePlural}/Services/{type.Name}Service.cs"),
                    new(new UpdateDto() { Parameters = templateParameters }, $"{parameters.CoreProjectPath}/{pascalNamePlural}/Dto/Request/Update{type.Name}Dto.cs"),
                    new(new Controller() { Parameters = templateParameters }, $"{parameters.WebApiProjectPath}/Controllers/{pascalNamePlural}Controller.cs"),

                    new(new Routes() { Parameters = templateParameters }, $"{parameters.ClientAppPath}/{kebabNamePlural}/components/pages/routes.ts"),
                    new(new IndexHtml() { Parameters = templateParameters }, $"{parameters.ClientAppPath}/{kebabNamePlural}/components/pages/index/index.component.html"),
                    new(new IndexCode() { Parameters = templateParameters }, $"{parameters.ClientAppPath}/{kebabNamePlural}/components/pages/index/index.component.ts"),
                    new(new GetHtml() { Parameters = templateParameters }, $"{parameters.ClientAppPath}/{kebabNamePlural}/components/pages/get/get.component.html"),
                    new(new GetCode() { Parameters = templateParameters }, $"{parameters.ClientAppPath}/{kebabNamePlural}/components/pages/get/get.component.ts"),
                    new(new CreateHtml() { Parameters = templateParameters }, $"{parameters.ClientAppPath}/{kebabNamePlural}/components/pages/create/create.component.html"),
                    new(new CreateCode() { Parameters = templateParameters }, $"{parameters.ClientAppPath}/{kebabNamePlural}/components/pages/create/create.component.ts"),
                    new(new EditHtml() { Parameters = templateParameters }, $"{parameters.ClientAppPath}/{kebabNamePlural}/components/pages/edit/edit.component.html"),
                    new(new EditCode() { Parameters = templateParameters }, $"{parameters.ClientAppPath}/{kebabNamePlural}/components/pages/edit/edit.component.ts"),
                    new(new CreateRequest() { Parameters = templateParameters }, $"{parameters.ClientAppPath}/{kebabNamePlural}/models/request/create-{kebabName}-request.ts"),
                    new(new SearchRequest() { Parameters = templateParameters }, $"{parameters.ClientAppPath}/{kebabNamePlural}/models/request/search-{kebabNamePlural}-request.ts"),
                    new(new UpdateRequest() { Parameters = templateParameters }, $"{parameters.ClientAppPath}/{kebabNamePlural}/models/request/update-{kebabName}-request.ts"),
                    new(new Response() { Parameters = templateParameters }, $"{parameters.ClientAppPath}/{kebabNamePlural}/models/response/{kebabName}.ts"),
                    new(new DataService() { Parameters = templateParameters }, $"{parameters.ClientAppPath}/{kebabNamePlural}/services/data.service.ts"),
                    new(new AreaService() { Parameters = templateParameters }, $"{parameters.ClientAppPath}/{kebabNamePlural}/services/{kebabName}.service.ts"),
                };

            if (!parameters.Overwrite)
            {
                foreach (var template in templateItems)
                {
                    if (File.Exists(Path.Combine(parameters.SourcePath, template.OutputFile)))
                    {
                        Console.WriteLine($"Bailing out: File '{template.OutputFile}' already exists!");
                        return;
                    }
                }
            }

            foreach (var template in templateItems)
            {
                string generatedCode = template.Template.TransformText();
                Directory.CreateDirectory(Path.GetDirectoryName(template.OutputFile)!);
                File.WriteAllText(template.OutputFile, generatedCode);

                Console.WriteLine($"Generated {template.OutputFile}");
            }

            Console.WriteLine(
@$"Scaffolding completed successfully. Please see TODO comments in generated code to complete integration.

    {parameters.CoreProjectName}:
    - Update client and server DTO properties in {pascalNamePlural}/Dto to only those you want included.
    - Update extension method mappers between DTOs and the entity in Extensions/{type.Name}Extensions.cs.

    {parameters.WebApiProjectName}:
    - Update the authorization for methods in Controllers/{pascalNamePlural}Controller.cs based on access preferences.
    - Register Web API controller parameter dependency in Extensions/ApplicationServiceExtensions.cs.

    {parameters.AngularProjectName}:
    - Update the models in {kebabNamePlural}/models to match the updated server DTOs.
    - Update authorization for the routes in {kebabNamePlural}/components/pages/routes.ts.
    - Add {kebabNamePlural} routes to the root route collection in routing/routes.ts.");
        }

        /// <summary>
        /// Validates the provided service parameters.
        /// </summary>
        /// <param name="parameters">The parameters to validate.</param>
        /// <returns>True if the parameters are valid; otherwise, false.</returns>
        public static bool ValidateParameters(ServiceParameters parameters)
        {
            if (string.IsNullOrEmpty(parameters.SourcePath))
            {
                Console.WriteLine("Path to /src is required.");
                return false;
            }

            if (!File.Exists(parameters.WebApiProjectFilePath))
            {
                Console.WriteLine($"Web API project not found at: {parameters.WebApiProjectFilePath}");
                return false;
            }

            if (!File.Exists(parameters.CoreProjectFilePath))
            {
                Console.WriteLine($"Core project not found at: {parameters.CoreProjectFilePath}");
                return false;
            }

            if (!Directory.Exists(parameters.ClientAppPath))
            {
                Console.WriteLine($"Angular project not found at: {parameters.ClientAppPath}");
                return false;
            }

            return true;
        }
    }
}