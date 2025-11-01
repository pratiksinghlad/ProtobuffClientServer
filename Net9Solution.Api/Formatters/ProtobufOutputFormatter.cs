using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Google.Protobuf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Net9Solution.Api.Formatters
{
    public class ProtobufOutputFormatter : OutputFormatter
    {
        public ProtobufOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/x-protobuf"));
        }

        protected override bool CanWriteType(Type type)
        {
            return typeof(IMessage).IsAssignableFrom(type);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var response = context.HttpContext.Response;
            var message = (IMessage)context.Object;

            if (message != null)
            {
                var memoryStream = new MemoryStream();
                message.WriteTo(memoryStream);
                memoryStream.Position = 0;
                await memoryStream.CopyToAsync(response.Body);
            }
        }
    }
}
