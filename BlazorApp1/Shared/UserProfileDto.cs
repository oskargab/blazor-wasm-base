namespace BlazorApp1.Shared
{
    public class UserProfileDto
    {
        public required string DisplayName { get; set; }
        public required DiscordAccountDto? DiscordAccount { get; set; }
        public required GoogleAccountDto? GoogleAccount {get; set; }

    }
}