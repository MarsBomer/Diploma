using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Diploma.Services
{
    public interface IAuthService
    {
        Task<IResult> LoginUser(HttpContext context, string? returnUrl);
        Guid GetCurrentUserId(HttpContext context);
        Task SetCurrentUserName(HttpContext context, User? user = null);
    }

    public class AuthService : IAuthService
    {
        ApplicationContext _dbContext;

        public AuthService(ApplicationContext context)
        {
            _dbContext = context;
        }

        public async Task<IResult> LoginUser(HttpContext context, string? returnUrl)
        {
            // получаем из формы email и пароль
            var form = context.Request.Form;
            // если email и/или пароль не установлены, посылаем статусный код ошибки 400
            if (!form.ContainsKey("login") || !form.ContainsKey("password"))
                return Results.BadRequest("Email и/или пароль не установлены");

            string email = form["login"];
            string password = form["password"];

            // находим пользователя 
            var user = await _dbContext.Users.Include(u => u.Person).FirstOrDefaultAsync(p => p.Login == email && p.Password == password);
            // если пользователь не найден, отправляем статусный код 401
            if (user is null) return Results.Unauthorized();

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Login), new Claim(ClaimTypes.UserData, user.Id.ToString()) };
            claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r.ToString())).ToList());

            // создаем объект ClaimsIdentity
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            // установка аутентификационных куки
            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            await SetCurrentUserName(context, user);

            return Results.Redirect(returnUrl ?? "/Home/Index");
        }

        public Guid GetCurrentUserId(HttpContext context)
        {
            var currentUserData = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData);

            if (currentUserData == null || !Guid.TryParse(currentUserData.Value, out Guid guid))
            {
                throw new UnauthorizedAccessException();
            }

            return guid;
        }

        public async Task SetCurrentUserName(HttpContext context, User? user = null)
        {
            if (user == null)
            {
                var guid = GetCurrentUserId(context);
                user = await _dbContext.Users.Include(u => u.Person).FirstOrDefaultAsync(u => u.Id == guid);

                if (user == null)
                {
                    throw new UnauthorizedAccessException();
                }
            }            

            context.Session.SetString("CurrentUserFullName", user.Person.FullName);
        }
    }
}
