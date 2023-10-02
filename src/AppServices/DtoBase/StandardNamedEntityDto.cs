using GaEpd.AppLibrary.Domain.Entities;
using Sbeap.Domain;
using System.ComponentModel.DataAnnotations;

namespace Sbeap.AppServices.DtoBase;

public abstract record StandardNamedEntityViewDto
(
    Guid Id,
    string Name,
    bool Active
) : INamedEntity;

public abstract record StandardNamedEntityCreateDto
(
    [Required(AllowEmptyStrings = false)]
    [StringLength(AppConstants.MaximumNameLength,
        MinimumLength = AppConstants.MinimumNameLength)]
    string Name
) : INamedEntity;

public abstract record StandardNamedEntityUpdateDto
(
    [Required(AllowEmptyStrings = false)]
    [StringLength(AppConstants.MaximumNameLength,
        MinimumLength = AppConstants.MinimumNameLength)]
    string Name,
    bool Active
) : INamedEntity;
