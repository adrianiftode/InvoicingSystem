using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        protected ActionResult<TModel> Result<TItem, TModel>(Result<TItem> result, Func<TItem, TModel> map,
            string messageWhenNotPresent = null)
        {
            if (result.Status != ResultStatus.Success)
            {
                return Fail(result, messageWhenNotPresent);
            }

            return map(result.Item);
        }

        protected ActionResult<TModel> CreatedResult<TItem, TModel>(Result<TItem> result, Func<TItem, TModel> map,
            string actionName,
            object routeValues,
            string messageWhenNotPresent = null)
        {
            if (result.Status != ResultStatus.Success)
            {
                return Fail(result, messageWhenNotPresent);
            }

            return CreatedAtAction(actionName, routeValues, map(result.Item));
        }

        protected ActionResult<TResponse> OkOrNotFound<TResponse>(TResponse response)
        {
            if (response == null)
            {
                return NotFound();
            }

            return response;
        }

        private ActionResult Fail<TItem>(Result<TItem> result, string messageWhenNotPresent)
        {
            if (result.Status == ResultStatus.NotPresent)
            {
                return BadRequest(new
                {
                    error =
                        messageWhenNotPresent ?? "Entry could not be modified because is not present."
                });
            }

            if (result.Status == ResultStatus.Forbidden)
            {
                return Forbid();
            }

            return null;
        }
    }
}
