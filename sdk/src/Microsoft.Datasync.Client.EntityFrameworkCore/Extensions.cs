using Microsoft.EntityFrameworkCore;
using System.Data.Objects;
using System.Text.RegularExpressions;

namespace Microsoft.Datasync.Client.EntityFrameworkCore;

internal static class DatasyncEFExtensions
{
    /// <summary>
    /// Gets the list of <see cref="PropertyInfo"/> elements that correspond to the <see cref="DbSet{T}"/>
    /// elements in the provided <see cref="DbContext"/>
    /// </summary>
    /// <param name="context">The <see cref="DbContext"/> to process.</param>
    /// <returns>A list of <see cref="PropertyInfo"/> objects representing the tables in the context.</returns>
    internal static IList<PropertyInfo> GetTablePropertySet(this DbContext context)
    {
        var properties = new List<PropertyInfo>();
        foreach (var property in context.GetTablePropertySet().GetProperties())
        {
            var setType = property.PropertyType;
            if (setType.IsGenericType && (typeof (DbSet<>).IsAssignableFrom(setType.GetGenericTypeDefinition())))
            {
                properties.Add(property);
            }
        }
        return properties;
    }

    /// <summary>
    /// Based on a specific <see cref="DbContext"/>, determine the actual table name underlying
    /// a type used in a <see cref="DbSet{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity to be processed.</typeparam>
    /// <return>The name of the table that backs the entity.</return>
    internal static string GetTableName<T>(this DbContext context) where T : class
        => ((IObjectContextAdapter) context).ObjectContext.GetTableName<T>();

    /// <summary>
    /// Based on a specific <see cref="ObjectContext"/>, determine the actual table name underlying
    /// a type used in a <see cref="DbSet{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity to be processed.</typeparam>
    /// <return>The name of the table that backs the entity.</return>
    internal static string GetTableName<T>(this ObjectContext context) where T : class
    {
        string sql = context.CreateObjectSet<T>().ToTraceString();
        Regex regex = new("FROM (?<table>.*) AS");
        Match match = regex.Match(sql);
        return match.Groups["table"].Value;
    }
}
