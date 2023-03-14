namespace EmrCalculateApi
{
    public class HttpsRedirectMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpsRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.IsHttps)
            {
                await _next(context);
            }
            else
            {
                var httpsUrl = "https://" + context.Request.Host + context.Request.Path;
                context.Response.Redirect(httpsUrl);
            }
        }
    }
}
