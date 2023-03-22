namespace ESBCkedtor.Models
{
    public class ErrorResult
    {
        public int statusCode { get; set; }
        public ErrorData data { get; set; }
        public string message { get; set; }
        public string reason { get; set; }
    }
    public class ErrorData
    {
        public bool authorized { get; set; }
        public bool valid { get; set; }
        public bool allowedInReadOnlyMode { get; set; }
        public object errors { get; set; }
        public bool successful { get; set; }
    }
}
