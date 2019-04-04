using System;
using Microsoft.AspNetCore.Http;

namespace Radio.Infrastructure.Api.Services
{
    // This primitive user service should only be used to tell users apart from each other.
    // It is not meant to be used for authentication or to guarantee the identity of the user, because the user id in the cookie can be easily guessed by an attacker and therefore can't be trusted.
    // This service won't prevent users from spamming either (generating new user ids with every request by not sending the cookie). Consider using a CAPTCHA to prevent spamming or use a regular session-based authentication system with registration and login.
    public interface IPrimitiveUserIdentificationService
    {
        Guid GetOrCreateUserId(HttpContext context);
    }
}
