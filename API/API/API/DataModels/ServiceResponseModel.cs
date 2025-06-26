namespace API.DataModels
{
    public class ServiceResponseModel
    {
        private bool _status = false;
        public bool Status { get => _status; set => _status = value; }
        public string SuccessMessage { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public int StatusCode { get => Status ? 200 : 400; }
        public dynamic Result { get; set; }


    }
}
