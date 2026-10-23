using System;
using System.Collections.Generic;
using System.Text;

namespace UESAN.ECOMMERCE.CORE.Core.DTOs
{
    public class ChatRequestDTO{
        public string Message { get; set; }        
    }

    public class ChatResponseDTO
    {
        public string Response { get; set; }
        public string  Model { get; set; }
    }

    public class MyOrdersRequestDTO
    {
        public int UserId{ get; set; }
        public string Message { get; set; }
    }

    public class ContextResponseDTO 
    {
        public string Answer { get; set; }
        public string Model { get; set; }
        public string Context { get; set; }
    }


}
