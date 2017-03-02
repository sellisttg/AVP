using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AVP.WebApi.Controllers
{
    /// <summary>
    /// IBaseController defines custom properties standard to all controllers in the AVP.WebApi service
    /// </summary>
    public abstract partial class IBaseController : Controller
    {
        /// <summary>
        /// Custom Json filter to hide null objects/properties when serializing json
        /// </summary>
        public JsonSerializerSettings jsonHideNulls = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

    }
}
