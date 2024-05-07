namespace Track.Order.Common;

public interface IApiSuccessResponse<T>
{
    T? Data { get; set; }
}
