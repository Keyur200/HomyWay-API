namespace HomyWayAPI.DTO
{
    public class PaymentDTO
    {
        public int Id { get; set; }

        public int? BookingId { get; set; }

        public string? PaymentMethod { get; set; }

        public string? PaymentId { get; set; }

        public string? CreatedDate { get; set; }
    }
}
