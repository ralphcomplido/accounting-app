using Humanizer;
using LightNap.Scaffolding.AssemblyManager;
using LightNap.Scaffolding.ServiceRunner;
using System.Diagnostics.CodeAnalysis;

namespace LightNap.Scaffolding.TemplateManager
{
    /// <summary>
    /// Manages template parameters and their replacements.
    /// </summary>
    public class TemplateParameters
    {
        public readonly string PascalName;
        public readonly string PascalNamePlural;
        public readonly string NameForNamespace;
        public readonly string CamelName;
        public readonly string CamelNamePlural;
        public readonly string KebabName;
        public readonly string KebabNamePlural;
        public readonly string ClientIdType;
        public readonly string ServerIdType;
        public readonly string ServerPropertiesList;
        public readonly string ServerOptionalPropertiesList;
        public readonly string ServerPropertiesToDto;
        public readonly string ServerPropertiesFromDto;
        public readonly string ClientPropertiesList;
        public readonly string ClientOptionalPropertiesList;
        public readonly string CoreNamespace;
        public readonly string WebApiNamespace;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateParameters"/> class.
        /// </summary>
        /// <param name="pascalName">The Pascal case name.</param>
        /// <param name="propertiesDetails">The list of property details.</param>
        /// <param name="serviceParameters">The service parameters.</param>
        [SetsRequiredMembers]
        public TemplateParameters(string pascalName, List<TypePropertyDetails> propertiesDetails, ServiceParameters serviceParameters)
        {
            this.PascalName = pascalName;
            this.PascalNamePlural = pascalName.Pluralize();

            // If the pluralized name is the same as the singular name, add an underscore to the name so that we don't get ambiguity errors in the generated code due to
            // the namespace and type being identical. This seemed like the least impactful way to fix the issue.
            this.NameForNamespace = pascalName.Pluralize();
            if (this.PascalName == this.NameForNamespace) { this.NameForNamespace = $"{pascalName}_"; }

            this.CamelName = pascalName.Camelize();
            this.CamelNamePlural = pascalName.Camelize().Pluralize();
            this.KebabName = pascalName.Kebaberize();
            this.KebabNamePlural = pascalName.Kebaberize().Pluralize();

            // Take a guess that the shortest property ending with "id" is the id property.
            TypePropertyDetails? idProperty = propertiesDetails.Where(p => p.Name.EndsWith("id", StringComparison.OrdinalIgnoreCase)).OrderBy(id => id.Name.Length).FirstOrDefault();
            this.ClientIdType = idProperty?.ClientTypeString ?? "string";
            this.ServerIdType = idProperty?.ServerTypeString ?? "string";

            // I promise I will come back and move this to the templates now that we have the processor in place.
            this.ServerPropertiesList = string.Join("\n\t\t", propertiesDetails.Where(p => p != idProperty).Select(p => $"public {p.ServerTypeString} {p.Name} {{ get; set; }}"));
            this.ServerOptionalPropertiesList = string.Join("\n\t\t", propertiesDetails.Where(p => p != idProperty).Select(p => $"public {p.ServerTypeString}? {p.Name} {{ get; set; }}"));
            this.ServerPropertiesToDto = string.Join("\n\t\t\t", propertiesDetails.Where(p => p != idProperty).Select(p => $"dto.{p.Name} = item.{p.Name};"));
            this.ServerPropertiesFromDto = string.Join("\n\t\t\t", propertiesDetails.Where(p => p != idProperty).Select(p => $"item.{p.Name} = dto.{p.Name};"));
            this.ClientPropertiesList = string.Join("\n\t", propertiesDetails.Where(p => p != idProperty).Select(p => $"{p.CamelName}: {p.ClientTypeString};"));
            this.ClientOptionalPropertiesList = string.Join("\n\t", propertiesDetails.Where(p => p != idProperty).Select(p => $"{p.CamelName}?: {p.ClientTypeString};"));

            this.CoreNamespace = serviceParameters.CoreProjectName;
            this.WebApiNamespace = serviceParameters.WebApiProjectName;
        }
    }
}
