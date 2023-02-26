namespace BStore.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public double TotalCart { get; set; } = 0;
        public List<CartRow> CartRows { get; set; } = default!;
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = default!;
    }
}
