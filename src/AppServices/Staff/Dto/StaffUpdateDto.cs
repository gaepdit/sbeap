using Sbeap.Domain.Identity;
using System.ComponentModel.DataAnnotations;

namespace Sbeap.AppServices.Staff.Dto;

public record StaffUpdateDto
(
    string Id,
    [StringLength(ApplicationUser.MaxPhoneLength,
        ErrorMessage = "The Phone Number must not be longer than {1} characters.")]
    string? Phone,
    [Required] [Display(Name = "Office")] Guid? OfficeId,
    [Required] bool Active
);
