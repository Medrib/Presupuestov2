using System.Net;
using Track.Order.Common.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Track.Order.Common;

public static class ObjectResultExtensions
{
    public static IActionResult BuildResult<T>(this IturriResult iturriResult, HttpStatusCode httpStatusCode = HttpStatusCode.OK) where T : class
    {
        if (iturriResult.IsSuccess)
        {
            T? dataAsT = iturriResult.GetDataAsT<T>();
            if (dataAsT == null)
            {
                return new NoContentResult();
            }

            return new OkObjectResult(ApiResponse<T>.BuildSuccessResponse(dataAsT))
            {
                StatusCode = (int)httpStatusCode
            };
        }

        return new ObjectResult(Api.Common.ApiResponse.BuildErrorResponse(iturriResult.Errors!))
        {
            StatusCode = (int)iturriResult.ToHeigherHttpStatusCode()
        };
    }

    public static ObjectResult BuildErrorResult(this IturriResult result, HttpStatusCode? errorHttpStatusCode = null)
    {
        return new ObjectResult(Api.Common.ApiResponse.BuildErrorResponse(result.Errors!))
        {
            StatusCode = (int)(errorHttpStatusCode ?? result.ToHeigherHttpStatusCode())
        };
    }
}
