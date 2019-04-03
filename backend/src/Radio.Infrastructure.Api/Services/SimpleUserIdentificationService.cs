using System;
using Microsoft.AspNetCore.Http;

namespace Radio.Infrastructure.Api.Services
{
    public class SimpleUserIdentificationService : ISimpleUserIdentificationService
    {
        private static readonly string USER_ID_KEY = "USER_ID";

        public Guid GetOrCreateUserId(HttpContext context)
        {
            var existingUserId = GetUserId(context);
            if (!string.IsNullOrEmpty(existingUserId))
            {
                return Guid.Parse(existingUserId);
            }

            return Guid.Parse(CreateUserId(context));
        }

        private static string GetUserId(HttpContext context)
        {
            return context.Request.Cookies[USER_ID_KEY];
        }

        private static string CreateUserId(HttpContext context)
        {
            var newUserId = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None
            };

            context.Response.Cookies.Append(USER_ID_KEY, newUserId, cookieOptions);

            return newUserId;
        }
    }
}
