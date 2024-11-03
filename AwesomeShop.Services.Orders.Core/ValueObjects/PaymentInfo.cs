
namespace AwesomeShop.Services.Orders.Core.ValueObjects;

public class PaymentInfo(string cardNumber, string fullName, string expiration, string cvv) {
    public string CardNumber { get; private set; } = cardNumber;
    public string FullName { get; private set; } = fullName;
    public string Expiration { get; private set; } = expiration;
    public string Cvv { get; private set; } = cvv;

    public override bool Equals(object? obj) => obj is PaymentInfo info
        && this.CardNumber == info.CardNumber
        && this.FullName == info.FullName
        && this.Expiration == info.Expiration
        && this.Cvv == info.Cvv;

    public override int GetHashCode() => HashCode.Combine(this.CardNumber, this.FullName, this.Expiration, this.Cvv);
}