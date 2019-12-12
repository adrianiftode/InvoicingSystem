using Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using System;


namespace Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        protected ActionResult<TModel> Result<TItem, TModel>(Result<TItem> result, Func<TItem, TModel> map)
        {
            if (result.Status != ResultStatus.Success)
            {
                return Fail(result);
            }

            return map(result.Item);
        }

        protected ActionResult<TModel> Result<TItem, TModel>((TItem item, Result result) result, Func<TItem, TModel> map)
        {
            if (result.result.Status != ResultStatus.Success)
            {
                return Fail(result);
            }

            return map(result.item);
        }

        protected ActionResult<TModel> CreatedResult<TItem, TModel>(Result<TItem> result, Func<TItem, TModel> map,
            string actionName,
            object routeValues)
        {
            if (result.Status != ResultStatus.Success)
            {
                return Fail(result);
            }

            return CreatedAtAction(actionName, routeValues, map(result.Item));
        }

        protected ActionResult<TModel> CreatedResult<TItem, TModel>((TItem item, Result result) result, Func<TItem, TModel> map,
            string actionName,
            object routeValues)
        {
            if (result.result.Status != ResultStatus.Success)
            {
                return Fail(result);
            }

            return CreatedAtAction(actionName, routeValues, map(result.item));
        }

        protected ActionResult<TResponse> OkOrNotFound<TResponse>(TResponse response)
        {
            if (response == null)
            {
                return NotFound();
            }

            return response;
        }

        private ActionResult Fail<TItem>(Result<TItem> result)
        {
            if (result.Status == ResultStatus.NotPresent)
            {
                if (result.Errors.Any())
                {
                    return NotFound(new
                    {
                        value = result.Errors
                    });
                }

                return NotFound();
            }

            if (result.Status == ResultStatus.Forbidden)
            {
                return Forbid();
            }

            if (result.Status == ResultStatus.InvalidOperation)
            {
                return BadRequest(new
                {
                    value = result.Errors
                });
            }

            return null;
        }

        private ActionResult Fail<TItem>((TItem item, Result result) result)
        {
            if (result.result.Status == ResultStatus.NotPresent)
            {
                if (result.result.Errors.Any())
                {
                    return NotFound(new
                    {
                        value = result.result.Errors
                    });
                }

                return NotFound();
            }

            if (result.result.Status == ResultStatus.Forbidden)
            {
                return Forbid();
            }

            if (result.result.Status == ResultStatus.InvalidOperation)
            {
                return BadRequest(new
                {
                    value = result.result.Errors
                });
            }

            return null;
        }
    }
}
