using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ReversiApp.Areas.Identity.Data;

namespace ReversiApp.services
{
    public class UsernameEmailSigninManager : SignInManager<User>
    {
        public UsernameEmailSigninManager(UserManager<User> userManager, Microsoft.AspNetCore.Http.IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<User> claimsFactory, Microsoft.Extensions.Options.IOptions<IdentityOptions> optionsAccessor, Microsoft.Extensions.Logging.ILogger<SignInManager<User>> logger, Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider schemes, IUserConfirmation<User> confirmation)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
        }

        public async Task<SignInResult> PasswordSignBothInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var user = await UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                return SignInResult.Failed;
            }

            return await PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        }
    }
}
