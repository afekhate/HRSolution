using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Utilities.JsonResult.WebResult.ErrorResult
{

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class ErrorResult
        {
            public string message { get; set; }
            public bool isSuccessful { get; set; }
            public int retId { get; set; }
            public int bulkUploadId { get; set; }
        }

        

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Root
    {
        public bool hasError { get; set; }
        public string message { get; set; }
        public string statusCode { get; set; }
       
    }

    public class ApiResponse<T>
    {
        public bool hasError { get; set; }
        public string message { get; set; }
        public string statusCode { get; set; }
        public T Result { get; set; }
    }

  



}
