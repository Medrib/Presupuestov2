using Track.Order.Api.Contracts.Order;

namespace Track.Order.Api.Contracts;

public class DataWithSize<T>
{
    public List<T>? Data { get; set; }
    public int TotalItems { get; set; }
    public UserRoleResponse? UserRole { get; set; }
}
