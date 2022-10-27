using Microsoft.AspNetCore.Identity;
using Sbeap.Domain.Offices;

namespace Sbeap.Domain.Identity;

public class ApplicationUser : IdentityUser, IEntity<string>
{
    // Properties from external login provider (email is part of IdentityUser)
    [ProtectedPersonalData]
    [StringLength(150)]
    public string FirstName { get; init; } = string.Empty;

    [ProtectedPersonalData]
    [StringLength(150)]
    public string LastName { get; init; } = string.Empty;


    // Editable user/staff properties
    public const int MaxPhoneLength = 25;

    [StringLength(MaxPhoneLength)]
    public string? Phone { get; set; }

    [InverseProperty("StaffMembers")]
    public Office? Office { get; set; }

    public bool Active { get; set; } = true;
}
