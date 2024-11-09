using LightNap.Scaffolding.AssemblyManager;

namespace LightNap.Scaffolding.Tests
{
    [TestClass]
    public class TypePropertyDetailsTests
    {
        [TestMethod]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var type = typeof(int);
            var name = "TestName";

            // Act
            var details = new TypePropertyDetails(type, name, true, true);

            // Assert
            Assert.AreEqual(type, details.Type);
            Assert.AreEqual(name, details.Name);
            Assert.AreEqual("testName", details.CamelName);
            Assert.AreEqual("int", details.ServerTypeString);
            Assert.AreEqual("number", details.ClientTypeString);
        }

        [TestMethod]
        public void GetServerTypeString_ShouldReturnCorrectString()
        {
            // Arrange & Act & Assert
            Assert.AreEqual("int", TypePropertyDetails.GetServerTypeString(typeof(int)));
            Assert.AreEqual("long", TypePropertyDetails.GetServerTypeString(typeof(long)));
            Assert.AreEqual("string", TypePropertyDetails.GetServerTypeString(typeof(string)));
            Assert.AreEqual("Guid", TypePropertyDetails.GetServerTypeString(typeof(Guid)));
            Assert.AreEqual("double", TypePropertyDetails.GetServerTypeString(typeof(double)));
            Assert.AreEqual("DateTime", TypePropertyDetails.GetServerTypeString(typeof(DateTime)));
            Assert.AreEqual("float", TypePropertyDetails.GetServerTypeString(typeof(float)));
            Assert.AreEqual("decimal", TypePropertyDetails.GetServerTypeString(typeof(decimal)));
            Assert.AreEqual("short", TypePropertyDetails.GetServerTypeString(typeof(short)));
            Assert.AreEqual("byte", TypePropertyDetails.GetServerTypeString(typeof(byte)));
            Assert.AreEqual("ushort", TypePropertyDetails.GetServerTypeString(typeof(ushort)));
            Assert.AreEqual("uint", TypePropertyDetails.GetServerTypeString(typeof(uint)));
            Assert.AreEqual("ulong", TypePropertyDetails.GetServerTypeString(typeof(ulong)));
            Assert.AreEqual("bool", TypePropertyDetails.GetServerTypeString(typeof(bool)));
            Assert.AreEqual("char", TypePropertyDetails.GetServerTypeString(typeof(char)));
            Assert.AreEqual("CustomType", TypePropertyDetails.GetServerTypeString(typeof(CustomType)));
        }

        [TestMethod]
        public void GetClientTypeString_ShouldReturnCorrectString()
        {
            // Arrange & Act & Assert
            Assert.AreEqual("number", TypePropertyDetails.GetClientTypeString(typeof(int)));
            Assert.AreEqual("number", TypePropertyDetails.GetClientTypeString(typeof(long)));
            Assert.AreEqual("string", TypePropertyDetails.GetClientTypeString(typeof(string)));
            Assert.AreEqual("string", TypePropertyDetails.GetClientTypeString(typeof(Guid)));
            Assert.AreEqual("number", TypePropertyDetails.GetClientTypeString(typeof(double)));
            Assert.AreEqual("string", TypePropertyDetails.GetClientTypeString(typeof(DateTime)));
            Assert.AreEqual("number", TypePropertyDetails.GetClientTypeString(typeof(float)));
            Assert.AreEqual("number", TypePropertyDetails.GetClientTypeString(typeof(decimal)));
            Assert.AreEqual("number", TypePropertyDetails.GetClientTypeString(typeof(short)));
            Assert.AreEqual("number", TypePropertyDetails.GetClientTypeString(typeof(byte)));
            Assert.AreEqual("number", TypePropertyDetails.GetClientTypeString(typeof(ushort)));
            Assert.AreEqual("number", TypePropertyDetails.GetClientTypeString(typeof(uint)));
            Assert.AreEqual("number", TypePropertyDetails.GetClientTypeString(typeof(ulong)));
            Assert.AreEqual("string", TypePropertyDetails.GetClientTypeString(typeof(bool)));
            Assert.AreEqual("string", TypePropertyDetails.GetClientTypeString(typeof(char)));
            Assert.AreEqual("string", TypePropertyDetails.GetClientTypeString(typeof(CustomType)));
        }

        private class CustomType { }
    }
}