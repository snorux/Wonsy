namespace Wonsy.Extensions
{
    public static class EmbedBuilderExtensions
    {
        public static void AddRequestedByFooter(this EmbedBuilder builder, IInteractionContext context) 
        {
            builder
                .WithFooter(footer => footer.Text = $"Requested by {context.User.Username}#{context.User.Discriminator}")
                .WithCurrentTimestamp();
        }

        public static void WithInformationColor(this EmbedBuilder builder)
            => builder.WithColor(Constants.InformationColor);
        public static void WithWarningColor(this EmbedBuilder builder)
            => builder.WithColor(Constants.WarningColor);

        public static void WithErrorColor(this EmbedBuilder builder)
            => builder.WithColor(Constants.ErrorColor);
    }
}
