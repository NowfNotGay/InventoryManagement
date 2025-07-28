using Core.BaseClass;
using Microsoft.AspNetCore.Mvc;

namespace API_Application.Utilities;

public static class ApiResponseHelper
{
    public static IActionResult HandleResult<T>(ControllerBase controller,ResultService<T> result)
    {
        return result.Code switch
        {
            "0" => controller.Ok(result),
            "1" => controller.BadRequest(result),
            _ => controller.NotFound(result)
        };
    }
}
