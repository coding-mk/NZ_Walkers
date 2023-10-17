using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NZWalksAPI.CustomerActionFilters;

public class ValidateModuleAttribute : ActionFilterAttribute
{
  public override void OnActionExecuted(ActionExecutedContext context)
  {
    if(context.ModelState.IsValid == false){
        context.Result = new BadRequestResult();
    }
  }
}