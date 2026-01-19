namespace book_shop_back_end.services
{
    public class DiscountService
    {
        public decimal RegisteredUsersDiscount { get; private set; } = 0m;

        public void SetRegisteredUsersDiscount(decimal percent)
        {
            if(percent < 0 || percent > 99)
            {
                throw new ArgumentException("Cannot be a negative num");
            }
            RegisteredUsersDiscount = percent / 100m;
        }

        public void ClearDiscount()
        {
            RegisteredUsersDiscount = 0m;
        }
    }

}
