using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace ESBCkedtor.Models
{
    public class ConfluenceResponse
    {
        public ErrorResult ErrorResult { get; set; }
        public string Message { get; set; }
    }
}
