﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KnilServer.Application.Common
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode {  get; set; }
        public bool IsSuccess { get; set; }
        public object? Result { get; set; }
        public string DisplayMessage { get; set; } = "";
        public List<APIError> Errors { get; set; } = new();
        public List<APIWarning> Warnings { get; set; } = new();

        public void AddError(string errormessage)
        {
            APIError error = new APIError(description: errormessage);
            Errors.Add(error);  
        }
        public void AddWarning(string errormessage)
        {
            APIWarning warning = new APIWarning(description: errormessage);
            Warnings.Add(warning);
        }
    }
}
