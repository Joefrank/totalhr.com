using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace totalhr.core
{
    public class ParamAdapter  : FilterAttribute, IActionFilter
    {
        private readonly string _parameterName;

        public ParamAdapter(string parameterName)
        {
            _parameterName = parameterName;
        }
 
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var valueProviderResult = filterContext.Controller.ValueProvider.GetValue(_parameterName);
 
            if (valueProviderResult != null)
            {
                var orders = valueProviderResult.AttemptedValue.ToString().Split(',');
                filterContext.ActionParameters[_parameterName] = Array.ConvertAll(orders, s => int.Parse(s));
            }
        }
 
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}
