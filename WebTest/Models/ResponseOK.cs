namespace BackEnd.Models
{
    public class ResponseOK
    {
        public int? Status { get; set; }
        public int? Code { get; set; }
        public string? UserMessage { get; set; }
        public string? InternalMessage { get; set; }
        public string? MoreInfo { get; set; }
        public dynamic? data { get; set; }
    }
}
