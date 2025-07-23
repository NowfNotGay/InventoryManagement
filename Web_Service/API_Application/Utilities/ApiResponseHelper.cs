using Core.BaseClass;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Utilities;

public static class ApiResponseHelper
{
    public static IActionResult HandleResult<T>(ResultService<T> result, ControllerBase controller)
    {
        return result.Code switch
        {
            "0" => controller.Ok(result),
            "1" => controller.NotFound(result),
            _ => controller.BadRequest(result)
        };
    }
}
