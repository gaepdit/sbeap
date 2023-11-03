using Sbeap.Domain.Entities.SicCodes;

namespace Sbeap.Domain.Data;

public static class SicCodeData
{
    public static IEnumerable<SicCode> GetSicCodes { get; } = new List<SicCode>
    {
        new() { Id = "0171", Description = "Berry Crops" },
        new() { Id = "0172", Description = "Grapes" },
        new() { Id = "0173", Description = "Tree Nuts", Active = false },
        new() { Id = "0174", Description = "Citrus Fruits" },
        new() { Id = "0175", Description = "Deciduous Tree Fruits" },
        new() { Id = "0179", Description = "Fruits And Tree Nuts" },
    };
}
