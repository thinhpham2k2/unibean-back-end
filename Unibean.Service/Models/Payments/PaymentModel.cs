namespace Unibean.Service.Models.Payments;

public class PaymentModel
{
    public string Id { get; set; }
    public string StudentId { get; set; }
    public string StudentName { get; set; }
    public string Token { get; set; }
    public decimal? Amount { get; set; }
    public string Method { get; set; }
    public string Message { get; set; }
    public DateTime? DateCreated { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
