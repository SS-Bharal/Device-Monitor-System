using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSaveService.Models
{
    public class ServiceResponseModel
    {
        private bool _status = false;
        public bool status { get => _status; set => _status = value; }
        //public bool Status { get => _status; set => _status = value; }
        //public string SuccessMessage { get; set; } = string.Empty;
        public string successMessage { get; set; } = string.Empty;
        //public string ErrorMessage { get; set; } = string.Empty;
        public string errorMessage { get; set; } = string.Empty;
        //public int StatusCode { get => Status ? 200 : 400; }
        public int statusCode { get => status ? 200 : 400; }
        //public dynamic Result { get; set; }
        public dynamic? result { get; set; }
    }
}
