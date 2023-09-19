using GaEpd.AppLibrary.Domain.Entities;
using Sbeap.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Sbeap.AppServices.DtoBase;

public abstract record SimpleNamedEntityViewDto
(
    Guid Id,
    string Name,
    bool Active
) : INamedEntity;

public abstract record SimpleNamedEntityCreateDto
(
    [Required(AllowEmptyStrings = false)]
    [StringLength(SbeapStandardNamedEntity.MaximumNameLength,
        MinimumLength = SbeapStandardNamedEntity.MinimumNameLength)]
    string Name
);

public abstract record SimpleNamedEntityUpdateDto
(
    Guid Id,
    [Required(AllowEmptyStrings = false)]
    [StringLength(SbeapStandardNamedEntity.MaximumNameLength,
        MinimumLength = SbeapStandardNamedEntity.MinimumNameLength)]
    string Name,
    bool Active
);
