using System.ComponentModel.DataAnnotations;

namespace winus.Server.Configuration
{
    public class GoogleAuthConfig
    {
        public const string SectionName = "GoogleAuth";
        [MinLength(1)]
        public required string ClientId { get; init; }
        [MinLength(1)]
        public required string ClientSecret { get; init; }
    }
}
