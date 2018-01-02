using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMQP_Common
{
    // Start , end and response time of message broker response time
    public class Response_Message
    {
        public DateTime request_start { get; set; }
        public DateTime request_end { get; set; }
        public int Response_Time { get; set; }


        public Response_Message()
        {
        }
    }
}
