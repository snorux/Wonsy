using System.Security.Cryptography;
using System.Text;

namespace Wonsy.Extensions
{
    public static class InteractionContextExtensions
    {
        public static void LogCommandUsed(this IInteractionContext context) 
        {
            var commandName = context.Interaction switch
            {
                SocketSlashCommand socketSlashCommand => socketSlashCommand.CommandName,
                SocketUserCommand socketUserCommand => socketUserCommand.CommandName,
                _ => null
            };

            Log.Information($"Slash Command Used\n\t" +
                $"User: {context.User.Username}#{context.User.Discriminator} [{context.User.Id}]\n\t" +
                $"Server: {(context.Guild == null ? "PRIVATE" : $"{context.Guild.Name} [{context.Guild.Id}]")}\n\t" +
                $"Channel: {(context.Channel == null ? "PRIVATE" : $"{context.Channel.Name} [{context.Channel.Id}]")}\n\t" +
                $"Command: {commandName}");
        }

        public static async Task ReplyCommandException(this IInteractionContext context, string reason) 
        {
            var referenceCode = GenerateRandomCode();
            var commandName = context.Interaction switch
            {
                SocketSlashCommand socketSlashCommand => socketSlashCommand.CommandName,
                SocketUserCommand socketUserCommand => socketUserCommand.CommandName,
                _ => null
            };

            // Send to user
            EmbedBuilder embedBuilder = new();
            embedBuilder.WithErrorColor();
            embedBuilder.WithTitle("Uh oh, something went wrong");
            embedBuilder.WithDescription($"Sorry, an error occured while trying to process the command `{commandName}`.\n\n" +
                $"**Reference ID:** `{referenceCode}`\n" +
                $"Please report the error to {MentionUtils.MentionUser(107389572958158848)} with the above reference id.");

            await context.Interaction.DeferAsync();
            await context.Interaction.FollowupAsync(embed: embedBuilder.Build());

            // Log for our reference:
            Log.Error($"Slash Command Error\n\t" +
                $"Reference ID: {referenceCode}\n\t" +
                $"User: {context.User.Username}#{context.User.Discriminator} [{context.User.Id}]\n\t" +
                $"Server: {(context.Guild == null ? "PRIVATE" : $"{context.Guild.Name} [{context.Guild.Id}]")}\n\t" +
                $"Channel: {(context.Channel == null ? "PRIVATE" : $"{context.Channel.Name} [{context.Channel.Id}]")}\n\t" +
                $"Command: {commandName}\n\t" +
                $"Error Reason: {reason}");
        }

        public static string GenerateRandomCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            const int size = 8;

            byte[] data = new byte[4 * size];
            using var crypto = RandomNumberGenerator.Create();
            crypto.GetBytes(data);

            StringBuilder result = new(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[(int)idx]);
            }

            return result.ToString();
        }
    }
}
