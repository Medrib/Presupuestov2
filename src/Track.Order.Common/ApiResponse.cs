using System.Net;
using System.Text.Json.Serialization;
using Track.Order.Common.Errors;

namespace Track.Order.Common;

public class ApiResponse<T> : IApiResponse<T>, IApiSuccessResponse<T>, IApiErrorResponse
{
    [JsonIgnore]
    public HttpStatusCode HttpStatus { get; set; }

    [JsonIgnore]
    public bool Success
    {
        get
        {
            IEnumerable<ApiError> errors = Errors;
            if (errors == null) { return true; }

            return !errors.Any();
        }
    }

    [JsonInclude]
    public T? Data { get; set; }

    [JsonInclude]
    public IEnumerable<ApiError> Errors { get; set;}

    public static ApiResponse<T> BuildSuccessResponse(T data, HttpStatusCode httpStatus = HttpStatusCode.OK)
    {
        return new ApiResponse<T>
        {
            Data = data,
            Errors = null,
            HttpStatus = httpStatus
        };
    }
}
