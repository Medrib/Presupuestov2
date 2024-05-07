using System.Text.Json.Serialization;

namespace Track.Order.Common;

public interface IApiErrorResponse
{
    [JsonIgnore]
    bool Success { get; }
    IEnumerable<ApiError> Errors { get; protected set; }
}
