namespace AwesomeShop.Services.Orders.Core.ValueObjects;

public class PaymentAddress(string street, string number, string city, string state, string zipCode) {
    public string Street { get; private set; } = street;
    public string Number { get; private set; } = number;
    public string City { get; private set; } = city;
    public string State { get; private set; } = state;
    public string ZipCode { get; private set; } = zipCode;

    public override bool Equals(object? obj) => obj is PaymentAddress address
        && this.Street == address.Street
        && this.Number == address.Number
        && this.City == address.City
        && this.State == address.State
        && this.ZipCode == address.ZipCode;

    public override int GetHashCode() => HashCode.Combine(this.Street, this.Number, this.City, this.State, this.ZipCode);
}