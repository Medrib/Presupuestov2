using System.Net;
using System.Text.Json.Serialization;
using Track.Order.Common.Errors;

namespace Track.Order.Common.Api.Common;

public class ApiResponse : IApiErrorResponse
{
    [JsonIgnore]
    public bool Success
    {
        get
        {
            IEnumerable<ApiError>? errors = Errors;
            if (errors == null)
            {
                return true;
            }

            return !errors.Any();
        }
    }

    [JsonInclude]
    public IEnumerable<ApiError>? Errors { get; set; }

    [JsonIgnore]
    public HttpStatusCode HttpStatus { get; private set; } 
    public static ApiResponse BuildErrorResponse(IEnumerable<IturriError> iErrors, HttpStatusCode? httpStatus = null)
    {
        return new ApiResponse
        {
            Errors = iErrors.Select((IturriError ie) => new ApiError(ie.Code!, ie.Message!)),
            HttpStatus = httpStatus ?? iErrors.ToHeigherHttpStatusCode()
        };
    }
}
