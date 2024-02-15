using System.Diagnostics;

namespace todoList.Middlewares;

public class MyLogMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger logger;


    public MyLogMiddleware(RequestDelegate next, ILogger<MyLogMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task Invoke(HttpContext c)
    {
        var sw = new Stopwatch();
        sw.Start();
        await next.Invoke(c);
        WriteToFile($" date: {DateTime.Now.ToShortDateString()}, beginning time: {DateTime.Now.TimeOfDay}, controller name: {c.Request.Path}, action: {c.Request.Method},"
         + $" user name: {c.User?.FindFirst("userId")?.Value ?? "unconnected"},"
          +$" duration: {sw.ElapsedMilliseconds}ms.");

    }

    private void WriteToFile(string logMessage)
    {
        using(StreamWriter sw = File.AppendText("log.txt"))
        {
            sw.WriteLine(logMessage);
        }
    }
}

public static partial class MiddlewareExtensions
{
    public static IApplicationBuilder UseMyLogMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<MyLogMiddleware>();
    }
}