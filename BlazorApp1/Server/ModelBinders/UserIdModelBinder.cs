using Grpc.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BlazorApp1.Server.ModelBinders
{
    public class UserIdModelBinder : IModelBinder
    {
        public UserIdModelBinder()
        {
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var r = bindingContext.HttpContext.User.Claims.SingleOrDefault(x => x.OriginalIssuer == "LOCAL AUTHORITY" && x.Issuer == "LOCAL AUTHORITY" && x.Type == ClaimTypes.NameIdentifier);
            if (r == null)
            {
                //throw new ArgumentException("No userid(ClaimTypes.NameIdentifier) with issuer LocalAuthority found in claims");
                bindingContext.Result = ModelBindingResult.Failed();
            }
            else
            {
                var model = r.Value;
                bindingContext.Result = ModelBindingResult.Success(model);
            }

            return Task.CompletedTask;
        }
    }

    public class NullUserIdModelBinder : IModelBinder
    {
        public NullUserIdModelBinder()
        {
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var r = bindingContext.HttpContext.User.Claims.SingleOrDefault(x => x.OriginalIssuer == "LOCAL AUTHORITY" && x.Issuer == "LOCAL AUTHORITY" && x.Type == ClaimTypes.NameIdentifier);
            if (r == null)
            {
                bindingContext.Result = ModelBindingResult.Success(null);

            }
            else
            {

                var model = r.Value;
                bindingContext.Result = ModelBindingResult.Success(model);
            }
            return Task.CompletedTask;
        }
    }




}
