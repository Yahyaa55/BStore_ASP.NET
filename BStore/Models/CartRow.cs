namespace BStore.Models
{
    public class CartRow
    {
        public int Id { get; set; }
        public int Quantity { get; set; } = 0;
        public double TotalRow { get; set; } = 0;
        public int CartId { get; set; }
        public Cart Cart { get; set; } = default!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;
    }
}
