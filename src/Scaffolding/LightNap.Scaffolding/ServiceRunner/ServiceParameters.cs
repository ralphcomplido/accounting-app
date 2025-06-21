using System.Diagnostics.CodeAnalysis;

namespace LightNap.Scaffolding.ServiceRunner
{
    /// <summary>
    /// Represents the parameters required for the service.
    /// </summary>
    public class ServiceParameters
    {
        /// <summary>
        /// The class name to scaffold.
        /// </summary>
        public readonly string ClassName;

        /// <summary>
        /// The path to the /src directory relative to the working directory.
        /// </summary>
        public readonly string SourcePath;

        /// <summary>
        /// The core project name.
        /// </summary>
        public readonly string CoreProjectName;

        /// <summary>
        /// The Web API project name.
        /// </summary>
        public readonly string WebApiProjectName;

        /// <summary>
        /// The Angular project name.
        /// </summary>
        public readonly string AngularProjectName;

        /// <summary>
        /// The Web API project path.
        /// </summary>
        public readonly string WebApiProjectPath;

        /// <summary>
        /// The Web API project file path.
        /// </summary>
        public readonly string WebApiProjectFilePath;

        /// <summary>
        /// The core project path.
        /// </summary>
        public readonly string CoreProjectPath;

        /// <summary>
        /// The entity's namespace.
        /// </summary>
        public readonly string EntityNamespace;

        /// <summary>
        /// The core project file path.
        /// </summary>
        public readonly string CoreProjectFilePath;

        /// <summary>
        /// The front-end application path.
        /// </summary>
        public readonly string FrontEndAppPath;

        /// <summary>
        /// Whether to skip generating Angular components.
        /// </summary>
        public readonly bool SkipComponents;

        /// <summary>
        /// Whether to automatically overwrite existing files.
        /// </summary>
        public readonly bool Overwrite;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceParameters"/> class.
        /// </summary>
        /// <param name="className">The class name.</param>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="coreProjectName">The core project name.</param>
        /// <param name="webApiProjectName">The Web API project name.</param>
        /// <param name="angularProjectName">The Angular project name.</param>
        /// <param name="angularProjectName">The Angular project name.</param>
        /// <param name="skipComponents">True to skip [re]generating Angular components.</param>
        /// <param name="overwrite">True to overwrite existing files.</param>
        [SetsRequiredMembers]
        public ServiceParameters(string className, string sourcePath, string entityNamespace, string coreProjectName, string webApiProjectName,
            string angularProjectName, bool skipComponents, bool overwrite)
        {
            this.ClassName = className;
            this.SourcePath = Path.GetFullPath(sourcePath);
            this.EntityNamespace = entityNamespace;
            this.CoreProjectName = coreProjectName;
            this.WebApiProjectName = webApiProjectName;
            this.AngularProjectName = angularProjectName;
            this.SkipComponents = skipComponents;
            this.Overwrite = overwrite;

            this.WebApiProjectPath = Path.Combine(this.SourcePath, webApiProjectName);
            this.WebApiProjectFilePath = Path.Combine(this.WebApiProjectPath, $"{webApiProjectName}.csproj");
            this.CoreProjectPath = Path.Combine(this.SourcePath, coreProjectName);
            this.CoreProjectFilePath = Path.Combine(this.CoreProjectPath, $"{coreProjectName}.csproj");
            this.FrontEndAppPath = Path.Combine(this.SourcePath, angularProjectName, "src/app");
        }
    }
}
