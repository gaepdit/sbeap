﻿using MyAppRoot.AppServices.Offices;
using MyAppRoot.Domain.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyAppRoot.AppServices.Staff;

public record StaffSearchDto
{
    public string? Name { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    public string? Role { get; init; }
    public Guid? Office { get; init; }
    public ActiveStatus Status { get; init; } = ActiveStatus.Active;

    public enum ActiveStatus
    {
        Active,
        Inactive,
        All,
    }

    public void TrimAll()
    {
        Name = Name?.Trim();
        Email = Email?.Trim();
    }
}

public class StaffViewDto
{
    public string Id { get; init; } = string.Empty;
    public string GivenName { get; init; } = string.Empty;
    public string FamilyName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? Phone { get; init; }
    public OfficeViewDto? Office { get; init; }

    [UIHint("BoolActive")]
    public bool Active { get; init; } = true;

    [JsonIgnore]
    public string DisplayName =>
        string.Join(" ", new[] { GivenName, FamilyName }.Where(s => !string.IsNullOrEmpty(s)));

    [JsonIgnore]
    public string SortableFullName =>
        string.Join(", ", new[] { FamilyName, GivenName }.Where(s => !string.IsNullOrEmpty(s)));

    public StaffUpdateDto AsUpdateDto() =>
        new() { Id = Id, Phone = Phone, OfficeId = Office?.Id, Active = Active };
}

public class StaffUpdateDto
{
    public string Id { get; init; } = string.Empty;

    [StringLength(ApplicationUser.MaxPhoneLength,
        ErrorMessage = "The Phone Number must not be longer than {1} characters.")]
    public string? Phone { get; init; }

    [Required]
    [Display(Name = "Office")]
    public Guid? OfficeId { get; init; }

    [Required]
    public bool Active { get; init; }
}
