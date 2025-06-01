namespace Finance.Services.Models
{
    public class CurrencyModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsPopular { get; set; }

        public CurrencyModel(int id, string title, bool isPopular) 
        {
            Id = id;
            Title = title;
            IsPopular = isPopular;
        }
    }
}
