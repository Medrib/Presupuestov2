using System.Text.Json.Serialization;

namespace Track.Order.Common;

public class ApiError
{
    [JsonInclude]
    public string ErrorCode { get; init; } = string.Empty;
    [JsonInclude]
    public string ErrorMessage { get; init; } = string.Empty;

    public ApiError() { }

    public ApiError( string errorCode, string errorMessage)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

}
