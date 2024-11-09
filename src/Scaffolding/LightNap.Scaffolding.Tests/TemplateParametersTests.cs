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
            Assert.AreEqual("TestClass", templateParameters.PascalName);
            Assert.AreEqual("TestClasses", templateParameters.PascalNamePlural);
            Assert.AreEqual("TestClasses", templateParameters.NameForNamespace);
            Assert.AreEqual("testClass", templateParameters.CamelName);
            Assert.AreEqual("testClasses", templateParameters.CamelNamePlural);
            Assert.AreEqual("test-class", templateParameters.KebabName);
            Assert.AreEqual("test-classes", templateParameters.KebabNamePlural);
            Assert.AreEqual("number", templateParameters.ClientIdType);
            Assert.AreEqual("int", templateParameters.ServerIdType);
            Assert.IsTrue(templateParameters.ServerPropertiesList.Contains("public string TestString { get; set; }"));
            Assert.IsTrue(templateParameters.ServerPropertiesList.Contains("public int TestInt { get; set; }"));
            Assert.IsTrue(templateParameters.ServerOptionalPropertiesList.Contains("public string? TestString { get; set; }"));
            Assert.IsTrue(templateParameters.ServerOptionalPropertiesList.Contains("public int? TestInt { get; set; }"));
            Assert.IsTrue(templateParameters.ServerPropertiesToDto.Contains("dto.TestString = item.TestString;"));
            Assert.IsTrue(templateParameters.ServerPropertiesToDto.Contains("dto.TestInt = item.TestInt;"));
            Assert.IsTrue(templateParameters.ServerPropertiesFromDto.Contains("item.TestString = dto.TestString;"));
            Assert.IsTrue(templateParameters.ServerPropertiesFromDto.Contains("item.TestInt = dto.TestInt;"));
            Assert.IsTrue(templateParameters.ClientPropertiesList.Contains("testString: string;"));
            Assert.IsTrue(templateParameters.ClientPropertiesList.Contains("testInt: number;"));
            Assert.IsTrue(templateParameters.ClientOptionalPropertiesList.Contains("testString?: string;"));
            Assert.IsTrue(templateParameters.ClientOptionalPropertiesList.Contains("testInt?: number;"));
        }
    }
}