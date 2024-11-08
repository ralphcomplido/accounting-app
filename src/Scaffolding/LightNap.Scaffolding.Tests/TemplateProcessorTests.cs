using LightNap.Scaffolding.ServiceRunner;
using LightNap.Scaffolding.TemplateManager;
using System.Text.RegularExpressions;

namespace LightNap.Scaffolding.Tests
{
    [TestClass]
    public class TemplateProcessorTests
    {
        [TestMethod]
        public void ProcessTemplate_ShouldReplacePlaceholdersWithValues()
        {
            // Arrange
            var templateContent =
                @"namespace <#= CoreNamespace #>.<#= NameForNamespace #>.Response.Dto
                {
                    public class <#= PascalName #>Dto
                    {
                        // TODO: Finalize which fields to include when returning this item.
                        public <#= ServerIdType #> Id { get; set; }
                        <#= ServerPropertiesList #>
                    }
                }";
            var templateParameters = new TemplateParameters("TestClassDetails",
                [
                    new(typeof(int), "Id"),
                    new(typeof(string), "TestString"),
                    new(typeof(DateTime), "TestDateTime")
                ],
                new ServiceParameters("", "./", "LightNap.Core", "", "", false));

            // Act
            var result = TemplateProcessor.ProcessTemplate(templateContent, templateParameters);

            // Assert
            var expected =
                @"namespace LightNap.Core.TestClassDetails_.Response.Dto
                {
                    public class TestClassDetailsDto
                    {
                        // TODO: Finalize which fields to include when returning this item.
                        public int Id { get; set; }
                        public string TestString { get; set; }
                        public DateTime TestDateTime { get; set; }
                    }
                }";

#pragma warning disable SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.
            Assert.AreEqual(Regex.Replace(@"\s+", " ", expected), Regex.Replace(@"\s+", " ", result));
#pragma warning restore SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.
        }
    }
}