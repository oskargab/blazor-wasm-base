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
            var r = bindingContext.HttpContext.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (r == null)
            {
                throw new ArgumentException("No userid(ClaimTypes.NameIdentifier) found in claims");
            }
            var model = r.Value;
            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }
    }


    public class UserIdValueProvider : BindingSourceValueProvider
    {
        private readonly ClaimsPrincipal _claimsPrincipal;

        public UserIdValueProvider(BindingSource bindingSource, ClaimsPrincipal claimsPrincipal) : base(bindingSource)
        {
            _claimsPrincipal = claimsPrincipal;
        }

        public override bool ContainsPrefix(string prefix)
            => _claimsPrincipal.HasClaim(claim => claim.Type == prefix);

        public override ValueProviderResult GetValue(string key)
        {
            var userIdValue = _claimsPrincipal.FindFirstValue(key);
            return userIdValue != null ? new ValueProviderResult(userIdValue) : ValueProviderResult.None;
        }
    }
    public static class UserIdBindingSource
    {
        public static readonly BindingSource UserId = new(
            "UserId", // ID of our BindingSource, must be unique
            "BindingSource_UserId", // Display name
            isGreedy: false, // Marks whether the source is greedy or not
            isFromRequest: true); // Marks if the source is from HTTP Request
    }

    public class UserIdValueProviderFactory : IValueProviderFactory
    {
        public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            context.ValueProviders.Add(new UserIdValueProvider(UserIdBindingSource.UserId, context.ActionContext.HttpContext.User));
            return Task.CompletedTask;
        }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public class FromUserIdAttribute : Attribute, IBindingSourceMetadata
    {
        public FromUserIdAttribute()
        {
        }
        public BindingSource BindingSource => UserIdBindingSource.UserId;
    }

    

   
}
