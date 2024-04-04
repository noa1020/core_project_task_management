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
        await WriteToFile($" date: {DateTime.Now.ToShortDateString()}, beginning time: {DateTime.Now.TimeOfDay}, controller name: {c.Request.Path}, action: {c.Request.Method},"
         + $" user name: {c.User?.FindFirst("userId")?.Value ?? "unconnected"},"
          +$" duration: {sw.ElapsedMilliseconds}ms.");

    }

private async Task<bool> WriteToFile(string logMessage)
{
    string filePath = "log.txt";
    using (FileStream fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
    {
        using (StreamWriter streamWriter = new StreamWriter(fileStream))
        {
            await streamWriter.WriteLineAsync(logMessage);
        }
    }
    return true;
}
}

public static partial class MiddlewareExtensions
{
    public static IApplicationBuilder UseMyLogMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<MyLogMiddleware>();
    }
}