using Core.Domain;

namespace SearchService.Models;

public class Transaction : EntityBase
{
    public string CreatedDate { get; set; } = string.Empty;

    public long Amount { get; set; }

    public string Note { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;
}
