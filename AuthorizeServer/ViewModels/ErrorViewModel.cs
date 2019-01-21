using System;

namespace AuthorizeServer.Models
{
    public class ErrorViewModel
    {
        public ErrorViewModel(string error):base()
        {
            this.Error = error;
        }

        public ErrorViewModel()
        {

        }

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string Error { get; set; }
        public string ErrorDescription { get; set; }
    }
}