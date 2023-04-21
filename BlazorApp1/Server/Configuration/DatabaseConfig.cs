using System.ComponentModel.DataAnnotations;

namespace winus.Server.Configuration
{
    public class DatabaseConfig
    {
        public const string SectionName = "Database";
        [MinLength(1)]
        public required string ConnectionString { get; init; }
    }
}
