using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Locator;

namespace LightNap.Scaffolding.ProjectManager
{
    /// <summary>
    /// Manages project operations such as building and adding files to the project.
    /// </summary>
    public class ProjectManager : IProjectManager
    {
        /// <summary>
        /// Registers the default MSBuild instance if a matching SDK is installed. It specifically requires the same .NET SDK version that the scaffolder was
        /// built to target. For example, if you have .NET 9 installed and the scaffolder was built to target .NET 8, this won't work because you can't load
        /// the builder for a different major version of the project. This could potentially be worked around by building via separate process, but that would 
        /// open up a bunch of potential issues.
        /// </summary>
        static ProjectManager()
        {
            if (MSBuildLocator.QueryVisualStudioInstances().Any())
            {
                MSBuildLocator.RegisterDefaults();
            }
        }

        /// <summary>
        /// Indicates whether the project manager can build projects. See <see cref="ProjectManager()"/>. 
        /// </summary>
        public bool CanBuild => MSBuildLocator.QueryVisualStudioInstances().Any();

        /// <summary>
        /// Builds the project at the specified path.
        /// </summary>
        /// <param name="projectPath">The path to the project file.</param>
        /// <returns>A <see cref="ProjectBuildResult"/> indicating the result of the build.</returns>
        public ProjectBuildResult BuildProject(string projectPath)
        {
            Console.WriteLine($"Attempting to build project at: {projectPath}");

            var projectCollection = new ProjectCollection();
            var project = projectCollection.LoadProject(projectPath);
            var buildParameters = new BuildParameters(projectCollection);
            var buildRequest = new BuildRequestData(project.CreateProjectInstance(), ["Build"]);
            var buildResult = BuildManager.DefaultBuildManager.Build(buildParameters, buildRequest);

            if (buildResult.OverallResult != BuildResultCode.Success)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var item in buildResult.ResultsByTarget)
                {
                    Console.WriteLine($"{item.Key}: {item.Value.ResultCode}");
                }
                Console.ResetColor();
                return new ProjectBuildResult() { Success = false };
            }

            var outputPath = project.GetPropertyValue("OutputPath");
            var outputFileName = project.GetPropertyValue("AssemblyName") + ".dll";
            return new ProjectBuildResult()
            {
                OutputAssemblyPath = Path.Combine(project.DirectoryPath, outputPath, outputFileName),
                Success = true
            };
        }
    }
}
