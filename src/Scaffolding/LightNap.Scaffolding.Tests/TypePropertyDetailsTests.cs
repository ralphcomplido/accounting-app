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
            var details = new TypePropertyDetails(type, name, false, true, true);

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
            Assert.AreEqual("byte", TypePropertyDetails.GetBackEndType(typeof(byte)));
            Assert.AreEqual("short", TypePropertyDetails.GetBackEndType(typeof(short)));
            Assert.AreEqual("ushort", TypePropertyDetails.GetBackEndType(typeof(ushort)));
            Assert.AreEqual("int", TypePropertyDetails.GetBackEndType(typeof(int)));
            Assert.AreEqual("uint", TypePropertyDetails.GetBackEndType(typeof(uint)));
            Assert.AreEqual("long", TypePropertyDetails.GetBackEndType(typeof(long)));
            Assert.AreEqual("ulong", TypePropertyDetails.GetBackEndType(typeof(ulong)));
            Assert.AreEqual("float", TypePropertyDetails.GetBackEndType(typeof(float)));
            Assert.AreEqual("double", TypePropertyDetails.GetBackEndType(typeof(double)));
            Assert.AreEqual("decimal", TypePropertyDetails.GetBackEndType(typeof(decimal)));
            Assert.AreEqual("bool", TypePropertyDetails.GetBackEndType(typeof(bool)));
            Assert.AreEqual("char", TypePropertyDetails.GetBackEndType(typeof(char)));
            Assert.AreEqual("string", TypePropertyDetails.GetBackEndType(typeof(string)));
            Assert.AreEqual("Guid", TypePropertyDetails.GetBackEndType(typeof(Guid)));
            Assert.AreEqual("DateTime", TypePropertyDetails.GetBackEndType(typeof(DateTime)));
            Assert.AreEqual("DateTimeOffset", TypePropertyDetails.GetBackEndType(typeof(DateTimeOffset)));
            Assert.AreEqual("TimeSpan", TypePropertyDetails.GetBackEndType(typeof(TimeSpan)));
            Assert.AreEqual("CustomType", TypePropertyDetails.GetBackEndType(typeof(CustomType)));
        }

        [TestMethod]
        public void GetFrontEndType_ShouldReturnCorrectString()
        {
            // Arrange & Act & Assert
            Assert.AreEqual("number", TypePropertyDetails.GetFrontEndType(typeof(byte)));
            Assert.AreEqual("number", TypePropertyDetails.GetFrontEndType(typeof(short)));
            Assert.AreEqual("number", TypePropertyDetails.GetFrontEndType(typeof(ushort)));
            Assert.AreEqual("number", TypePropertyDetails.GetFrontEndType(typeof(int)));
            Assert.AreEqual("number", TypePropertyDetails.GetFrontEndType(typeof(uint)));
            Assert.AreEqual("number", TypePropertyDetails.GetFrontEndType(typeof(long)));
            Assert.AreEqual("number", TypePropertyDetails.GetFrontEndType(typeof(ulong)));
            Assert.AreEqual("number", TypePropertyDetails.GetFrontEndType(typeof(float)));
            Assert.AreEqual("number", TypePropertyDetails.GetFrontEndType(typeof(double)));
            Assert.AreEqual("number", TypePropertyDetails.GetFrontEndType(typeof(decimal)));
            Assert.AreEqual("boolean", TypePropertyDetails.GetFrontEndType(typeof(bool)));
            Assert.AreEqual("string", TypePropertyDetails.GetFrontEndType(typeof(char)));
            Assert.AreEqual("string", TypePropertyDetails.GetFrontEndType(typeof(string)));
            Assert.AreEqual("string", TypePropertyDetails.GetFrontEndType(typeof(Guid)));
            Assert.AreEqual("Date", TypePropertyDetails.GetFrontEndType(typeof(DateTime)));
            Assert.AreEqual("Date", TypePropertyDetails.GetFrontEndType(typeof(DateTimeOffset)));
            Assert.AreEqual("string", TypePropertyDetails.GetFrontEndType(typeof(TimeSpan)));
            Assert.AreEqual("string", TypePropertyDetails.GetFrontEndType(typeof(CustomType)));
        }

        private class CustomType { }
    }
}