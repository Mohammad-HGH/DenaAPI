namespace DenaAPI.Responses
{
    public class AttributeResponse : BaseResponse
    {
        public List<Models.Attribute>? Attributes { get; set; }
        public string Message { get; set; }
    }
}
