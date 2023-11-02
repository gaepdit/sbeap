using System.Text.RegularExpressions;

namespace Sbeap.Domain.Data;

public static partial class Regexes
{
    //-- Email RegEx: https://regex101.com/r/umVx9J/1
    private const string SimpleEmailPattern = @"\b[\w\.-]+@[\w\.-]+\.\w{2,4}\b";

    [GeneratedRegex(SimpleEmailPattern)]
    public static partial Regex EmailRegex();
}
