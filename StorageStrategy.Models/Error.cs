﻿namespace StorageStrategy.Models
{
    public class Error
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }

        public Error(string errorMessage, string propertyName = "")
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }
    }
}
