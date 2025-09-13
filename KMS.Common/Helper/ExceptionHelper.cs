using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KMS.Common.Helper
{
    [Serializable]
    public class ServiceExceptionHelper : Exception
    {
        public ServiceExceptionHelper(string message) : base($"BadRequestException|{message}")
        {
        }
    }
}