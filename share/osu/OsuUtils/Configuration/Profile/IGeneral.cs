using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace OsuUtils.Configuration.Profile
{
    public interface IGeneral
    {
        public string UserName { get; }
        public SecureString Password { get; }
        public string Language { get; }

    }
}
