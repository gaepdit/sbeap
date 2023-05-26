namespace Sbeap.Domain.Extensions;

public static class StringExtensions
{
    public static string ConcatWithSeparator(this IEnumerable<string?> items, string separator = " ") =>
        string.Join(separator, items.Where(s => !string.IsNullOrEmpty(s)));
}
