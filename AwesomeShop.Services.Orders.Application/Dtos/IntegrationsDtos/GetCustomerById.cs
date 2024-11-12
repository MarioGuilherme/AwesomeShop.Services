namespace AwesomeShop.Services.Orders.Application.Dtos.IntegrationsDtos;

public class GetCustomerById {
    public Guid Id { get; private set; }
    public string FullName { get; private set; }
    public DateTime BirthDate { get; private set; }
    public AddressDto Address { get; private set; }
}