namespace Wonsy.Extensions
{
    public static class BasicExtensions
    {
        public static string FormattedValue(this IUser user, bool withId = false) => withId ? $"{user.Username}{(user.Discriminator == "0000" ? "" : $"#{user.Discriminator}")} [{user.Id}]" : $"{user.Username}{(user.Discriminator == "0000" ? "" : $"#{user.Discriminator}")}";

        public static string FormattedValue(this IGuild guild, bool withId = false) => guild == null ? "PRIVATE" : withId ? $"{guild.Name} [{guild.Id}]" : $"{guild.Name}";

        public static string FormattedValue(this IChannel channel, bool withId = false) => channel == null ? "PRIVATE" : withId ? $"{channel.Name} [{channel.Id}]" : $"{channel.Name}";
    }
}
