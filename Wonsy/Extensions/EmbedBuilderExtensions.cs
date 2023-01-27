﻿namespace Wonsy.Extensions
{
    public static class EmbedBuilderExtensions
    {
        public static void AddRequestedByFooter(this EmbedBuilder builder, ICommandContext context) 
        {
            builder
                .WithFooter(footer => footer.Text = $"Requested by {context.User.Username}#{context.User.Discriminator}")
                .WithCurrentTimestamp();
        }
    }
}