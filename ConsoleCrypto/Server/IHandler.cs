using ConsoleCrypto.Server;
using System.IO;
using System.Threading.Tasks;

internal interface IHandler
{
    void Handle(Stream stream, ServerRequest serverRequest);
    Task HandleAsync(Stream stream, ServerRequest serverRequest);
}