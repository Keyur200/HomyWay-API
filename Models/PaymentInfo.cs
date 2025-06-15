using System;
using System.Collections.Generic;

namespace HomyWayAPI.Models;

public partial class PaymentInfo
{
    public int Id { get; set; }

    public int? BookingId { get; set; }

    public string? PaymentMethod { get; set; }

    public string? PaymentId { get; set; }

    public string? CreatedDate { get; set; }

    public virtual Booking? Booking { get; set; }
}
