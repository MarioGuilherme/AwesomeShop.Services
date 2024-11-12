namespace AwesomeShop.Services.Orders.Application.Dtos.IntegrationsDtos;

public class AddressDto {
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string ZipCode { get; private set; }
}