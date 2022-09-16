using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailService.Sender
{
    internal abstract class GenericEmail
    {
        public abstract IEmail Object();
    }
}
