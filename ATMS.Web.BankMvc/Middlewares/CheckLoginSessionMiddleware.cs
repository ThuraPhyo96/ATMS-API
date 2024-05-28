using ATMS.Web.Dto.Dtos;
using ATMS.Web.Shared;

namespace ATMS.Web.BankMvc.Middlewares
{
    public class CheckLoginSessionMiddleware
    {
        private readonly RequestDelegate _next;

        public CheckLoginSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, DapperService dapperService)
        {
            var requestUrl = context.Request.Path.ToString().ToLower();
            if (requestUrl.Equals("/account/index") || requestUrl.Equals("/account"))
                goto result;

            var cookies = context.Request.Cookies;

            if(cookies["UserId"] is null || cookies["UserSessionId"] is null)
            {
                context.Response.Redirect("/Account/Index");
                goto result;
            }

            string userId = cookies["UserId"]!.ToString();
            string userSessionId = cookies["UserSessionId"]!.ToString();

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userSessionId))
            {
                context.Response.Redirect("/Account/Index");
                goto result;
            }

            (string getQuery, Dictionary<string, object> getParameters) = GetUserSessionQueryAndParameters(userSessionId, userId);
            var userSessions = dapperService.Query<UserSessionDto>(getQuery, getParameters);
            var userSession = userSessions.FirstOrDefault();

            if (userSession is null)
            {
                context.Response.Redirect("/Account/Index");
                goto result;
            }

            DateTime sessionInterval = userSession.SessionInterval;
            if (sessionInterval < DateTime.Now)
            {
                context.Response.Redirect("/Account/Index");
                goto result;
            }

            result: await _next(context);
        }

        private static (string, Dictionary<string, object>) GetUserSessionQueryAndParameters(string userSessionId, string userId)
        {
            string query = @"SELECT * FROM UserSessions WHERE UserSessionId = @UserSessionId AND UserId = @UserId";

            Dictionary<string, object> parameters = new()
            {
                { "@UserSessionId", userSessionId },
                { "@UserId", userId },
            };

            return (query, parameters);
        }
    }

    public static class CheckLoginSessionMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoginMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CheckLoginSessionMiddleware>();
        }
    }
}
