namespace LightNap.Scaffolding.AssemblyManager
{

    /// <summary>
    /// Provides helper methods for working with types.
    /// </summary>
    public static class TypeHelper
    {
        private static readonly HashSet<Type> SupportedEfTypes = new HashSet<Type>
            {
                typeof(string),
                typeof(DateTime),
                typeof(DateTimeOffset),
                typeof(TimeSpan),
                typeof(Guid),
                typeof(decimal),
                typeof(int),
                typeof(bool),
                typeof(byte),
                typeof(char),
                typeof(double),
                typeof(float),
                typeof(long),
                typeof(short),
                typeof(uint),
                typeof(ulong),
                typeof(ushort),
                typeof(sbyte)
            };

        /// <summary>
        /// Gets the property details of a given type.
        /// </summary>
        /// <param name="type">The type to get property details from.</param>
        /// <returns>A list of property details.</returns>
        public static List<TypePropertyDetails> GetPropertyDetails(Type type)
        {
            List<TypePropertyDetails> propertiesDetails = new List<TypePropertyDetails>();

            foreach (var property in type.GetProperties())
            {
                try
                {
                    // Check if the property type is a common Entity Framework type or an enum
                    if (property.PropertyType.IsPrimitive ||
                        SupportedEfTypes.Contains(property.PropertyType) ||
                        property.PropertyType.IsEnum ||
                        (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                            (Nullable.GetUnderlyingType(property.PropertyType)!.IsEnum ||
                            SupportedEfTypes.Contains(Nullable.GetUnderlyingType(property.PropertyType)!))))
                    {
                        propertiesDetails.Add(new TypePropertyDetails(property.PropertyType, property.Name, property.GetMethod != null, property.SetMethod != null));
                    }
                    else
                    {
                        Console.WriteLine($"Ignoring '{property.Name}': Not a type supported in this scaffolder ({property.PropertyType.Name})");
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
