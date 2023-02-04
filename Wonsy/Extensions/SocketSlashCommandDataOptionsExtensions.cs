namespace Wonsy.Extensions
{
    public static class SocketSlashCommandDataOptionsExtensions
    {
        public static string OptionsToString(this IReadOnlyCollection<SocketSlashCommandDataOption> options) 
        {
            List<string> output = new();
            foreach (var option in options)
                output.Add($"[{option.Name}: {option.FormattedValue()}]");

            return string.Join(" ", output);
        }

        public static string FormattedValue(this SocketSlashCommandDataOption options)
        {
            var result = options.Type switch
            {
                ApplicationCommandOptionType.User => ((IUser)options.Value).FormattedValue(),
                ApplicationCommandOptionType.Channel => ((IChannel)options.Value).FormattedValue(),
                ApplicationCommandOptionType.Mentionable => (options.Value is IUser user) ? user.FormattedValue() : options.Value.ToString(),
                _ => options.Value.ToString()
            };

            return result;
        }
    }
}
