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


}
