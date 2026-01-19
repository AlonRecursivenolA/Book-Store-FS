namespace book_shop_back_end.Dtos
{
    public class LoginDto
    {
        public string UserNameOrEmail { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
