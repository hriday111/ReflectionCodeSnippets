// CREATING YOUR OWN ATTR
using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public class LabDescriptionAttribute : Attribute
{
    public string Description { get; }
    public LabDescriptionAttribute(string description)
    {
        this.Description = description;
    }
}


//Retrieving from an Assembly the Types Marked with a Given Attribute

using System;
using System.Reflection;
using System.Linq;

// Example Class using the attribute from point 1
[LabDescription("This is a critical lab class.")]
public class CriticalLab
{
    // ...
}

public static void FindAttributedTypes()
{
    // Get the current assembly (where the CriticalLab class is defined)
    Assembly assembly = Assembly.GetExecutingAssembly();

    // Find all types in the assembly that have the LabDescriptionAttribute applied
    var typesWithAttribute = assembly.GetTypes()
        .Where(type => type.IsDefined(typeof(LabDescriptionAttribute), false));

    Console.WriteLine("Types found with LabDescriptionAttribute:");
    foreach (Type type in typesWithAttribute)
    {
        var attr = type.GetCustomAttribute<LabDescriptionAttribute>();
        Console.WriteLine($"- {type.Name}: {attr.Description}");
    }
}



//Retrieving from an Assembly the Types that Inherit from a Specific Abstract Class
using System;
using System.Reflection;
using System.Linq;

public abstract class BaseLabComponent { } // The target abstract class
public class InputComponent : BaseLabComponent { }
public class OutputComponent : BaseLabComponent { }

public static void FindInheritingTypes()
{
    Type baseType = typeof(BaseLabComponent);
    Assembly assembly = Assembly.GetExecutingAssembly();

    var inheritedTypes = assembly.GetTypes()
        .Where(type => !type.IsAbstract && type.IsSubclassOf(baseType));

    Console.WriteLine("Types found inheriting from BaseLabComponent:");
    foreach (Type type in inheritedTypes)
    {
        Console.WriteLine($"- {type.Name}");
    }
}

//Retrieving from a Type the Properties Marked with an Attribute
using System.Reflection;

// Re-using LabDescriptionAttribute from point 1
public class DataModel
{
    [LabDescription("Primary ID field.")]
    public int Id { get; set; }

    public string Name { get; set; } // No attribute
}

public static void FindAttributedProperties()
{
    Type dataType = typeof(DataModel);
    
    // Get all properties
    PropertyInfo[] properties = dataType.GetProperties();

    Console.WriteLine("Properties found with LabDescriptionAttribute:");
    foreach (PropertyInfo prop in properties)
    {
        if (prop.IsDefined(typeof(LabDescriptionAttribute), false))
        {
            var attr = prop.GetCustomAttribute<LabDescriptionAttribute>();
            Console.WriteLine($"- Property: {prop.Name}, Description: {attr.Description}");
        }
    }
}

//Checking Whether a Type Implements a Generic Interface
using System;
using System.Linq;
using System.Collections.Generic;

public class LabData<T> : List<T> { } // Inherits from a generic interface

public static void CheckGenericInterface()
{
    Type targetType = typeof(LabData<int>);
    Type genericInterface = typeof(IEnumerable<>);

    // Get all implemented interfaces
    var interfaces = targetType.GetInterfaces();

    // Check if any of the interfaces match the generic interface definition
    bool implementsGeneric = interfaces.Any(
        i => i.IsGenericType && i.GetGenericTypeDefinition() == genericInterface
    );

    Console.WriteLine($"Does {targetType.Name} implement {genericInterface.Name}? {implementsGeneric}");
    // Output: Does LabData`1 implement IEnumerable`1? True
}

//Setting the Value of a Property on a Type Instance Using SetValue()

using System.Reflection;

public class Settings
{
    public int LabVersion { get; set; }
}

public static void SetPropertyValue()
{
    // 1. Get an instance of the class
    Settings settingsInstance = new Settings { LabVersion = 1 };
    Console.WriteLine($"Initial Version: {settingsInstance.LabVersion}"); // 1

    // 2. Get the PropertyInfo for 'LabVersion'
    Type settingsType = typeof(Settings);
    PropertyInfo versionProperty = settingsType.GetProperty("LabVersion");

    // 3. Set the new value
    // Arguments: (object obj, object value)
    if (versionProperty != null && versionProperty.CanWrite)
    {
        versionProperty.SetValue(settingsInstance, 2);
        Console.WriteLine($"Updated Version: {settingsInstance.LabVersion}"); // 2
    }
}
