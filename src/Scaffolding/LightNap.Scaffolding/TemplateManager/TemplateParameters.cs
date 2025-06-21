using Humanizer;
using LightNap.Scaffolding.AssemblyManager;
using LightNap.Scaffolding.ServiceRunner;
using System.Collections.ObjectModel;
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
        public readonly string EntityNamespace;
        public readonly string NameForNamespace;
        public readonly string CamelName;
        public readonly string CamelNamePlural;
        public readonly string KebabName;
        public readonly string KebabNamePlural;
        public readonly string CoreNamespace;
        public readonly string WebApiNamespace;
        public readonly ReadOnlyCollection<TypePropertyDetails> AllProperties;
        public readonly TypePropertyDetails IdProperty;
        public readonly ReadOnlyCollection<TypePropertyDetails> GetProperties;
        public readonly ReadOnlyCollection<TypePropertyDetails> SetProperties;
        public readonly ReadOnlyCollection<string> AdditionalDtoNamespaces;

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
            this.EntityNamespace = serviceParameters.EntityNamespace;

            // If the pluralized name is the same as the singular name, add an underscore to the name so that we don't get ambiguity errors in the generated code due to
            // the namespace and type being identical. This seemed like the least impactful way to fix the issue.
            this.NameForNamespace = pascalName.Pluralize();
            if (this.PascalName == this.NameForNamespace) { this.NameForNamespace = $"{pascalName}_"; }

            this.CamelName = pascalName.Camelize();
            this.CamelNamePlural = pascalName.Camelize().Pluralize();
            this.KebabName = pascalName.Kebaberize();
            this.KebabNamePlural = pascalName.Kebaberize().Pluralize();

            this.AllProperties = propertiesDetails.AsReadOnly();

            // Take a guess that the shortest property ending with "id" is the id property. If there is none, then we'll nudge in the right direction.
            this.IdProperty = propertiesDetails.Where(p => p.Name.EndsWith("id", StringComparison.OrdinalIgnoreCase)).OrderBy(id => id.Name.Length).FirstOrDefault()
                ?? new TypePropertyDetails(typeof(int), "Id", false, true, true);
            this.GetProperties = propertiesDetails.Where(p => p != this.IdProperty && p.CanGet).ToList().AsReadOnly();
            this.SetProperties = propertiesDetails.Where(p => p != this.IdProperty && p.CanSet).ToList().AsReadOnly();

            this.CoreNamespace = serviceParameters.CoreProjectName;
            this.WebApiNamespace = serviceParameters.WebApiProjectName;

            this.AdditionalDtoNamespaces = propertiesDetails
                                            .Select(p => p.Type.Namespace)
                                            .Cast<string>()
                                            .OrderBy(ns => ns)
                                            .Distinct()
                                            .ToList()
                                            .AsReadOnly();
        }
    }
}
