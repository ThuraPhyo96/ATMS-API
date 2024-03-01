namespace ATMS.Web.Dto.Dtos
{
    public class ResponseDto
    {
        public int StatusCode { get; set; }

        public string StatusMessage { get; set; }

        public ResponseDto()
        {

        }

        public ResponseDto(int statusCode, string statusMessage)
        {
            StatusCode = statusCode;
            StatusMessage = statusMessage;
        }
    }
}
