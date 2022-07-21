using ConsoleCrypto.Server;

internal interface IHandler
{
    void Handle(Stream stream, ServerRequest serverRequest);
    Task HandleAsync(Stream stream, ServerRequest serverRequest);
}