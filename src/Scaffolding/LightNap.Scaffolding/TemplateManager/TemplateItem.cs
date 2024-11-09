using LightNap.Scaffolding.Templates;

namespace LightNap.Scaffolding.TemplateManager
{
    /// <summary>
    /// Matches a template to run with its output file.
    /// </summary>
    public class TemplateItem(BaseTemplate template, string outputFile)
    {
        /// <summary>
        /// The template to run.
        /// </summary>
        public readonly BaseTemplate Template = template;

        /// <summary>
        /// The output file path
        /// </summary>
        public readonly string OutputFile = outputFile;
    }
}
