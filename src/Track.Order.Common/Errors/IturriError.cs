using System.Net;

namespace Track.Order.Common.Errors;

public class IturriError
{
    public string? Code { get; init; } = string.Empty;
    public string? Message { get; init; } = string.Empty;
    public HttpStatusCode HttpStatus { get; private set; }

    public IturriError(string? code, string? message, HttpStatusCode httpStatus)
    {
        Code = code;
        Message = message;
        HttpStatus = httpStatus;
    }
}
