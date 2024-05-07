using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Net;

namespace Track.Order.Common.Errors;

public static class ErrorsExtensions
{
    public static HttpStatusCode ToHeigherHttpStatusCode(this IEnumerable<IturriError> errors)
    {
        return errors.Max(new IturriErrorHttpStatusComparer())?.HttpStatus ?? HttpStatusCode.InternalServerError;
    }
}
