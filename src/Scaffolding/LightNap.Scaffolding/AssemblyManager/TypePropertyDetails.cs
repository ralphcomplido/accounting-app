using Humanizer;
using System.Diagnostics.CodeAnalysis;

namespace LightNap.Scaffolding.AssemblyManager
{
    /// <summary>
    /// Represents the details of a type property.
    /// </summary>
    public class TypePropertyDetails
    {
        /// <summary>
        /// The property type.
        /// </summary>
        public readonly Type Type;

        /// <summary>
        /// The property name.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Camel case.
        /// </summary>
        public readonly string CamelName;

        /// <summary>
        /// The string representation of the property type for the back-end (C#).
        /// </summary>
        public readonly string BackEndType;

        /// <summary>
        /// The string representation of the property type for the front-end (Typescript).
        /// </summary>
        public readonly string FrontEndType;

        /// <summary>
        /// True if the type is nullable.
        /// </summary>
        public readonly bool IsNullable;

        /// <summary>
        /// True if the property is required.
        /// </summary>
        public readonly bool IsRequired;

        /// <summary>
        /// True if the property has a getter.
        /// </summary>
        public readonly bool CanGet;

        /// <summary>
        /// True if the property has a setter.
        /// </summary>
        public readonly bool CanSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypePropertyDetails"/> class.
        /// </summary>
        /// <param name="type">The type of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="isRequired">True if the property is required.</param>
        /// <param name="canGet">True if the property has a getter.</param>
        /// <param name="canSet">True if the property has a setter.</param>
        [SetsRequiredMembers]
        public TypePropertyDetails(Type type, string name, bool isRequired, bool canGet, bool canSet)
        {
            this.Type = type;
            this.Name = name;
            this.IsRequired = isRequired;
            this.CanGet = canGet;
            this.CanSet = canSet;
            this.CamelName = this.Name.Camelize();
            this.BackEndType = TypePropertyDetails.GetBackEndType(type);
            this.FrontEndType = TypePropertyDetails.GetFrontEndType(type);
            this.IsNullable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Gets the back-end type string for the specified type.
        /// </summary>
        /// <param name="type">The type to get the back-end type string for.</param>
        /// <returns>The C# type string.</returns>
        public static string GetBackEndType(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return TypePropertyDetails.GetBackEndType(Nullable.GetUnderlyingType(type)!);
            }

            if (type == typeof(byte)) { return "byte"; }
            if (type == typeof(short)) { return "short"; }
            if (type == typeof(ushort)) { return "ushort"; }
            if (type == typeof(int)) { return "int"; }
            if (type == typeof(uint)) { return "uint"; }
            if (type == typeof(long)) { return "long"; }
            if (type == typeof(ulong)) { return "ulong"; }
            if (type == typeof(float)) { return "float"; }
            if (type == typeof(double)) { return "double"; }
            if (type == typeof(decimal)) { return "decimal"; }
            if (type == typeof(bool)) { return "bool"; }
            if (type == typeof(char)) { return "char"; }
            if (type == typeof(string)) { return "string"; }
            if (type == typeof(Guid)) { return "Guid"; }
            if (type == typeof(DateTime)) { return "DateTime"; }
            if (type == typeof(DateTimeOffset)) { return "DateTimeOffset"; }
            if (type == typeof(DateOnly)) { return "DateOnly"; }
            if (type == typeof(TimeOnly)) { return "TimeOnly"; }
            if (type == typeof(TimeSpan)) { return "TimeSpan"; }
            return type.Name;
        }

        /// <summary>
        /// Gets the front-end type string for the specified type.
        /// </summary>
        /// <param name="type">The type to get the front-end type string for.</param>
        /// <returns>The TypeScript type string.</returns>
        public static string GetFrontEndType(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return TypePropertyDetails.GetFrontEndType(Nullable.GetUnderlyingType(type)!);
            }

            if (type == typeof(int) ||
                type == typeof(long) ||
                type == typeof(double) ||
                type == typeof(float) ||
                type == typeof(decimal) ||
                type == typeof(short) ||
                type == typeof(byte) ||
                type == typeof(ushort) ||
                type == typeof(uint) ||
                type == typeof(ulong))
            {
                return "number";
            }
            if (type == typeof(bool))
            {
                return "boolean";
            }
            if (type == typeof(DateTime) || type == typeof(DateTimeOffset))
            {
                return "Date";
            }
            // string, char, Guid, TimeSpan, enum, and anything I've overlooked.
            return "string";
        }
    }
}
