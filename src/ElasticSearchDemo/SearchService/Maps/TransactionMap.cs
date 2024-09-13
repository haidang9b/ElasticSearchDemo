using CsvHelper.Configuration;
using Domain.Models;

namespace SearchService.Maps
{
    public class TransactionMap : ClassMap<Transaction>
    {
        public TransactionMap()
        {
            Map(m => m.CreatedDate).Name("date");
            Map(m => m.Amount).Name("amount");
            Map(m => m.Note).Name("notes");
            Map(m => m.Code).Name("code");
        }
    }
}
