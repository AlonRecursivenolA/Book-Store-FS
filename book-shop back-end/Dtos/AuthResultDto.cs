namespace book_shop_back_end.Dtos
{
    public class AuthResultDto
    {
        public bool Succeeded { get; set; }
        public string? Token { get; set; }
        public object? User { get; set; }
    }
}
