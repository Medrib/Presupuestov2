using Microsoft.AspNetCore.Mvc;
using System.Net;
using Track.Order.Common.Errors;

namespace Track.Order.Common.Models;

public class IturriResult
{
    public IEnumerable<IturriError>? Errors { get; set; }
    public bool IsSuccess => Data != null;
    public bool IsFailure => !IsSuccess;

    public object? Data { get; set; }

    private IturriResult(object? data, IEnumerable<IturriError>? errors)
    {
        Data = data;
        Errors = errors;
    }

    public static IturriResult Success<T>(T value)
    {
        return new IturriResult(value, null);
    }

    public static IturriResult Success(object value)
    {
        return new IturriResult(value, null);
    }

    public static IturriResult Fail(IturriError error)
    {
        return new IturriResult(null, new List<IturriError> { error });
    }

    public T? GetDataAsT<T>()
    {
        return (T)(Data ?? null);
    }

    public HttpStatusCode ToHeigherHttpStatusCode()
    {
        HttpStatusCode result = HttpStatusCode.OK;
        if (Errors != null)
        {
            result = Errors.ToHeigherHttpStatusCode();
        }

        if (Data == null && Errors == null)
        {
            result = HttpStatusCode.InternalServerError;
        }

        return result;
    }
}
