using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Models
{
    public class ClientScriptConfig
    {
        /// <summary>
        /// This is used mainly when we have more than one calendar instance on the page 
        /// </summary>
        public int PageClientId { get; set; }
        
        /// <summary>
        /// we name array to be created by service, this is to remove dependency from service
        /// </summary>
        public string JsArrayEventName { get; set; }

        /// <summary>
        /// This is the method called when we click on an event
        /// </summary>
        public string EventClickCallBack { get; set; }

        /// <summary>
        /// This is the method called when we click on an active TD (day of current month)
        /// </summary>
        public string ActiveTdClickCallBack { get; set; }

        /// <summary>
        /// This is method called when you click on inactive TD (next or prev month)
        /// </summary>
        public string InActiveTdClickCallBack { get; set; }

    }
}
