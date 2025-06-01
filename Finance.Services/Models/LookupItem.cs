namespace Finance.Services.Models
{
    public class LookupItem
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public LookupItem(int id, string title) 
        {
            Id = id;
            Title = title;
        }
    }
}
