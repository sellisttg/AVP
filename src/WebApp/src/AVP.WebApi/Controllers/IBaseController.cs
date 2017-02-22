﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AVP.WebApi.Controllers
{
    public abstract partial class IBaseController : Controller
    {
        public JsonSerializerSettings jsonHideNulls = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

    }
}
