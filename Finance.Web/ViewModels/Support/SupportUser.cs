using Finance.Data.Models;

namespace Finance.Services.Models.Support
{
    public class SupportRequestViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string  Text { get; set; } = string.Empty;
        public string Token { get; set; }
    }
}
