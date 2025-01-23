using System.Reflection;
using System.Text;

namespace Application.Utilities;

public static class CsvHelper
{
    public static string GenerateCsv<T>(IEnumerable<T> items)
    {
        if (items == null || !items.Any()) return string.Empty;

        var csvBuilder = new StringBuilder();
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        AppendHeader(csvBuilder, properties);
        AppendRows(csvBuilder, properties, items);

        return csvBuilder.ToString();
    }

    private static void AppendHeader(StringBuilder csvBuilder, PropertyInfo[] properties)
    {
        var header = string.Join(",", properties.Select(p => p.Name));
        csvBuilder.AppendLine(header);
    }

    private static void AppendRows<T>(StringBuilder csvBuilder, PropertyInfo[] properties, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            var row = string.Join(",", properties.Select(p => p.GetValue(item, null)));
            csvBuilder.AppendLine(row);
        }
    }
}