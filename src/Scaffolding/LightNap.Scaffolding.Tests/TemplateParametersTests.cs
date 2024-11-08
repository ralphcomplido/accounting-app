using LightNap.Scaffolding.AssemblyManager;
using LightNap.Scaffolding.ServiceRunner;
using LightNap.Scaffolding.TemplateManager;

namespace LightNap.Scaffolding.Tests
{
    [TestClass]
    public class TemplateParametersTests
    {
        [TestMethod]
        public void Constructor_ShouldInitializeReplacementsCorrectly()
        {
            // Arrange
            string pascalName = "TestClass";
            var propertiesDetails = new List<TypePropertyDetails>
            {
                new(typeof(int), "Id"),
                new(typeof(int), "TestInt"),
                new(typeof(string), "TestString")
            };
            var serviceParameters = new ServiceParameters("TestClass", "./", "LightNap.Core", "", "", false);

            // Act
            var templateParameters = new TemplateParameters(pascalName, propertiesDetails, serviceParameters);

            // Assert
            Assert.AreEqual("TestClass", templateParameters.Replacements["PascalName"]);
            Assert.AreEqual("TestClasses", templateParameters.Replacements["PascalNamePlural"]);
            Assert.AreEqual("TestClasses", templateParameters.Replacements["NameForNamespace"]);
            Assert.AreEqual("testClass", templateParameters.Replacements["CamelName"]);
            Assert.AreEqual("testClasses", templateParameters.Replacements["CamelNamePlural"]);
            Assert.AreEqual("test-class", templateParameters.Replacements["KebabName"]);
            Assert.AreEqual("test-classes", templateParameters.Replacements["KebabNamePlural"]);
            Assert.AreEqual("number", templateParameters.Replacements["ClientIdType"]);
            Assert.AreEqual("int", templateParameters.Replacements["ServerIdType"]);
            Assert.IsTrue(templateParameters.Replacements["ServerPropertiesList"].Contains("public string TestString { get; set; }"));
            Assert.IsTrue(templateParameters.Replacements["ServerPropertiesList"].Contains("public int TestInt { get; set; }"));
            Assert.IsTrue(templateParameters.Replacements["ServerOptionalPropertiesList"].Contains("public string? TestString { get; set; }"));
            Assert.IsTrue(templateParameters.Replacements["ServerOptionalPropertiesList"].Contains("public int? TestInt { get; set; }"));
            Assert.IsTrue(templateParameters.Replacements["ServerPropertiesToDto"].Contains("dto.TestString = item.TestString;"));
            Assert.IsTrue(templateParameters.Replacements["ServerPropertiesToDto"].Contains("dto.TestInt = item.TestInt;"));
            Assert.IsTrue(templateParameters.Replacements["ServerPropertiesFromDto"].Contains("item.TestString = dto.TestString;"));
            Assert.IsTrue(templateParameters.Replacements["ServerPropertiesFromDto"].Contains("item.TestInt = dto.TestInt;"));
            Assert.IsTrue(templateParameters.Replacements["ClientPropertiesList"].Contains("testString: string;"));
            Assert.IsTrue(templateParameters.Replacements["ClientPropertiesList"].Contains("testInt: number;"));
            Assert.IsTrue(templateParameters.Replacements["ClientOptionalPropertiesList"].Contains("testString?: string;"));
            Assert.IsTrue(templateParameters.Replacements["ClientOptionalPropertiesList"].Contains("testInt?: number;"));
        }
    }
}