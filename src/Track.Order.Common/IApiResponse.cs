using System.Net;
using System.Text.Json.Serialization;

namespace Track.Order.Common;

public interface IApiResponse<T> : IApiSuccessResponse<T>, IApiErrorResponse
{
    [JsonIgnore]
    HttpStatusCode HttpStatus { get; set; }
}
