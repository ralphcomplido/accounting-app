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
            Assert.AreEqual("int", details.BackEndType);
            Assert.AreEqual("number", details.FrontEndType);
        }

        [TestMethod]
        public void GetBackEndType_ShouldReturnCorrectString()
        {
            // Arrange & Act & Assert
            Assert.AreEqual("int", TypePropertyDetails.GetBackEndType(typeof(int)));
            Assert.AreEqual("long", TypePropertyDetails.GetBackEndType(typeof(long)));
            Assert.AreEqual("string", TypePropertyDetails.GetBackEndType(typeof(string)));
            Assert.AreEqual("Guid", TypePropertyDetails.GetBackEndType(typeof(Guid)));
            Assert.AreEqual("double", TypePropertyDetails.GetBackEndType(typeof(double)));
            Assert.AreEqual("DateTime", TypePropertyDetails.GetBackEndType(typeof(DateTime)));
            Assert.AreEqual("float", TypePropertyDetails.GetBackEndType(typeof(float)));
            Assert.AreEqual("decimal", TypePropertyDetails.GetBackEndType(typeof(decimal)));
            Assert.AreEqual("short", TypePropertyDetails.GetBackEndType(typeof(short)));
            Assert.AreEqual("byte", TypePropertyDetails.GetBackEndType(typeof(byte)));
            Assert.AreEqual("ushort", TypePropertyDetails.GetBackEndType(typeof(ushort)));
            Assert.AreEqual("uint", TypePropertyDetails.GetBackEndType(typeof(uint)));
            Assert.AreEqual("ulong", TypePropertyDetails.GetBackEndType(typeof(ulong)));
            Assert.AreEqual("bool", TypePropertyDetails.GetBackEndType(typeof(bool)));
            Assert.AreEqual("char", TypePropertyDetails.GetBackEndType(typeof(char)));
            Assert.AreEqual("CustomType", TypePropertyDetails.GetBackEndType(typeof(CustomType)));
        }

        [TestMethod]
        public void GetFrontEndType_ShouldReturnCorrectString()
        {
            // Arrange & Act & Assert
            Assert.AreEqual("number", TypePropertyDetails.GetFrontEndType(typeof(int)));
            Assert.AreEqual("number", TypePropertyDetails.GetFrontEndType(typeof(long)));
            Assert.AreEqual("string", TypePropertyDetails.GetFrontEndType(typeof(string)));
            Assert.AreEqual("string", TypePropertyDetails.GetFrontEndType(typeof(Guid)));
            Assert.AreEqual("number", TypePropertyDetails.GetFrontEndType(typeof(double)));
            Assert.AreEqual("string", TypePropertyDetails.GetFrontEndType(typeof(DateTime)));
            Assert.AreEqual("number", TypePropertyDetails.GetFrontEndType(typeof(float)));
            Assert.AreEqual("number", TypePropertyDetails.GetFrontEndType(typeof(decimal)));
            Assert.AreEqual("number", TypePropertyDetails.GetFrontEndType(typeof(short)));
            Assert.AreEqual("number", TypePropertyDetails.GetFrontEndType(typeof(byte)));
            Assert.AreEqual("number", TypePropertyDetails.GetFrontEndType(typeof(ushort)));
            Assert.AreEqual("number", TypePropertyDetails.GetFrontEndType(typeof(uint)));
            Assert.AreEqual("number", TypePropertyDetails.GetFrontEndType(typeof(ulong)));
            Assert.AreEqual("string", TypePropertyDetails.GetFrontEndType(typeof(bool)));
            Assert.AreEqual("string", TypePropertyDetails.GetFrontEndType(typeof(char)));
            Assert.AreEqual("string", TypePropertyDetails.GetFrontEndType(typeof(CustomType)));
        }

        private class CustomType { }
    }
}