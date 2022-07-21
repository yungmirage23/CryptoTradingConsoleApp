using ConsoleCrypto.Models.Requests;
using ConsoleCrypto.Server;
using ConsoleCrypto.Server.ResponseWriter;
using Newtonsoft.Json;
using System.Reflection;

internal partial class ControllerHandler : IHandler
{
    private readonly Dictionary<string, Func<object>> _routes;
    public ControllerHandler(Assembly requestAssembly)
    {
        this._routes = requestAssembly.GetTypes().Where(x => typeof(IController).IsAssignableFrom(x)).SelectMany(Controller => Controller.GetMethods().Select(Method => new
        {
            Controller,
            Method
        })
        ).ToDictionary
        (
           key => GetPath(key.Controller, key.Method),
           value => GetEndpointMethod(value.Controller, value.Method)
        );
    }

    private Func<object> GetEndpointMethod(System.Type controller, MethodInfo methodInfo)
    {
        return () => methodInfo.Invoke(Activator.CreateInstance(controller), Array.Empty<object>());
    }

    private string GetPath(System.Type controller, MethodInfo methodInfo)
    {
        string name = controller.Name;
        if (name.EndsWith("controller", StringComparison.InvariantCultureIgnoreCase)) ;
        name = name.Substring(0, name.Length - "controller".Length);
        if (methodInfo.Name.Equals("Index", StringComparison.InvariantCultureIgnoreCase))
            return "/" + name;
        return "/" + name + "/" + methodInfo.Name;
    }

    public void Handle(Stream stream, ServerRequest request)
    {
        if (!_routes.TryGetValue(request.Path, out var func))
            ResponseWriter.WriteStatus(System.Net.HttpStatusCode.NotFound, stream);
        else
        {
            ResponseWriter.WriteStatus(System.Net.HttpStatusCode.OK, stream);
            WriteControllerResponse(func(), stream);
        }
    }

    public async Task HandleAsync(Stream stream, ServerRequest request)
    {
        if (!_routes.TryGetValue(request.Path, out var func))
            await ResponseWriter.WriteStatusAsync(System.Net.HttpStatusCode.NotFound, stream);
        else
        {
            await ResponseWriter.WriteStatusAsync(System.Net.HttpStatusCode.OK, stream);
            await WriteControllerResponseAsync(func(), stream);
        }
    }
    private void WriteControllerResponse(object response, Stream stream)
    {
        if (response is string str)
        {
            using var writer = new StreamWriter(stream, leaveOpen: true);
            writer.Write(str);
        }
        else if (response is byte[] buffer)
        {
            stream.Write(buffer, 0, buffer.Length);
        }
        else
        {
            WriteControllerResponse(JsonConvert.SerializeObject(response), stream);
        }
    }
    private async Task WriteControllerResponseAsync(object response, Stream stream)
    {
        if (response is string str)
        {
            using var writer = new StreamWriter(stream, leaveOpen: true);
           await writer.WriteAsync(str);
        }
        else if (response is byte[] buffer)
        {
           await stream.WriteAsync(buffer, 0, buffer.Length);
        }
        else if(response is Task task)
        {
            await task;
            await WriteControllerResponseAsync(task.GetType().GetProperty("Result").GetValue(task),stream);
        }
        else
        {
           await WriteControllerResponseAsync(JsonConvert.SerializeObject(response), stream);
        }
    }
}