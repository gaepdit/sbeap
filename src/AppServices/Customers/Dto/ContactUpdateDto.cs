﻿using GaEpd.AppLibrary.Extensions;
using Sbeap.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sbeap.AppServices.Customers.Dto;

public record ContactUpdateDto
{
    // Authorization handler assist properties

    public bool IsDeleted { get; set; }
    public bool CustomerIsDeleted { get; set; }
    public Guid CustomerId { get; set; }

    // Entity update properties

    public string? Honorific { get; init; }

    [Display(Name = "First name")]
    public string? GivenName { get; init; }

    [Display(Name = "Last name")]
    public string? FamilyName { get; init; }

    public string? Title { get; init; }

    [EmailAddress]
    [StringLength(150)]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email address")]
    public string? Email { get; init; }

    public string? Notes { get; init; }

    public IncompleteAddress Address { get; init; } = default!;

    [UsedImplicitly]
    public List<PhoneNumber> PhoneNumbers { get; } = [];

    // Read-only properties

    [JsonIgnore]
    public string Name => new[] { Honorific, GivenName, FamilyName }.ConcatWithSeparator();
}
