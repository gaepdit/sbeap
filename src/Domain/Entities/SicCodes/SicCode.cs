using System.Text;

namespace Sbeap.Domain.Entities.SicCodes;

public class SicCode : IEntity<string>
{
    [Key, MaxLength(4)]
    public string Id { get; set; } = null!;

    [MaxLength(60)]
    public string Description { get; set; } = null!;
    public bool Active { get; set; } = true;

    public string Display
    {
        get
        {
            var sn = new StringBuilder();
            sn.Append($"{Id} – {Description}");
            if (!Active) sn.Append(" [Inactive]");
            return sn.ToString();
        }
    }
}
