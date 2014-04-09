using System;
namespace _1188.SCMS.CustomEvents
{
    public class CustomValidationErrorEventArgs : EventArgs
    {
        public CustomValidationErrorEventArgs(bool hasError, string errorMessage)
        {
            HasError = hasError;
            ErrorMessage = errorMessage;
        }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
    }
}
