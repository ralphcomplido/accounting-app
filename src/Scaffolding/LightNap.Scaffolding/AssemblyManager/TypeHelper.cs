namespace LightNap.Scaffolding.AssemblyManager
{

    /// <summary>
    /// Provides helper methods for working with types.
    /// </summary>
    public static class TypeHelper
    {
        /// <summary>
        /// Gets the property details of a given type.
        /// </summary>
        /// <param name="type">The type to get property details from.</param>
        /// <returns>A list of property details.</returns>
        public static List<TypePropertyDetails> GetPropertyDetails(Type type)
        {
            List<TypePropertyDetails> propertiesDetails = [];

            foreach (var property in type.GetProperties())
            {
                try
                {
                    // Check if the property type is a common Entity Framework type
                    if (property.PropertyType.IsPrimitive ||
                        property.PropertyType == typeof(string) ||
                        property.PropertyType == typeof(DateTime) ||
                        property.PropertyType == typeof(Guid) ||
                        property.PropertyType == typeof(decimal) ||
                        property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                            (Nullable.GetUnderlyingType(property.PropertyType) == typeof(int) ||
                            Nullable.GetUnderlyingType(property.PropertyType) == typeof(DateTime) ||
                            Nullable.GetUnderlyingType(property.PropertyType) == typeof(Guid) ||
                            Nullable.GetUnderlyingType(property.PropertyType) == typeof(decimal)))
                    {                        
                        propertiesDetails.Add(new TypePropertyDetails(property.PropertyType, property.Name, property.GetMethod != null, property.SetMethod != null));
                        Console.WriteLine($"Found {property.Name} ({property.PropertyType.Name})");
                    }
                    else
                    {
                        Console.WriteLine($"Ignoring '{property.Name}': Not a type supported in this scaffolder  ({property.PropertyType.Name})");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ignoring '{property.Name}': {ex.Message}");
                }
            }

            return propertiesDetails;
        }
    }
}
