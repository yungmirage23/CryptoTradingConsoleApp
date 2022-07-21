using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCrypto.Server.ResponseWriter
{
    internal static class ResponseWriter
    {
        public static void WriteStatus(HttpStatusCode statusCode, Stream stream)
        {
            using var writer = new StreamWriter(stream, leaveOpen: true);
            writer.WriteLine($"HTTP/1.0 {(int)statusCode} {statusCode}");
            writer.WriteLine();
        }
        public static async Task WriteStatusAsync(HttpStatusCode statusCode, Stream stream)
        {
            using var writer = new StreamWriter(stream, leaveOpen: true);
            await writer.WriteLineAsync($"HTTP/1.0 {(int)statusCode} {statusCode}");
            await writer.WriteLineAsync();
        }
    }
}
