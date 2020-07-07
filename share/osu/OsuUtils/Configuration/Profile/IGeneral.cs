using System.Security;

namespace OsuUtils.Configuration.Profile
{
    public interface IGeneral
    {
        public string UserName { get; }
        public SecureString Password { get; }
        public string Language { get; }

    }
}
