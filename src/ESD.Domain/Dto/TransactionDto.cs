namespace ESD.Domain.Dto;

public class TransactionDto
{
    public Guid Id { get; set; }

    public string CreatedDate { get; set; } = string.Empty;

    public long Amount { get; set; }

    public string Note { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;
}
