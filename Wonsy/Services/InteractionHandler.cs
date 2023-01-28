namespace Wonsy.Services
{
    public class InteractionHandler
    {
        private readonly InteractionService _interactionService;
        private readonly IServiceProvider _serviceProvider;
        private readonly DiscordSocketClient _client;

        public InteractionHandler(InteractionService interactionService, IServiceProvider serviceProvider, DiscordSocketClient client)
        {
            _interactionService = interactionService;
            _serviceProvider = serviceProvider;
            _client = client;

            _client.InteractionCreated += HandleInteraction;
            _interactionService.SlashCommandExecuted += SlashCommandExecuted;
        }

        private async Task HandleInteraction(SocketInteraction interaction)
        {
            try
            {
                var context = new SocketInteractionContext(_client, interaction);
                var result = await _interactionService.ExecuteCommandAsync(context, _serviceProvider);

            }
            catch
            {
                if (interaction.Type is InteractionType.ApplicationCommand)
                    await interaction.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
            }
        }

        private async Task SlashCommandExecuted(SlashCommandInfo info, IInteractionContext context, IResult result)
        {
            if (!result.IsSuccess)
            {
                switch (result.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        await context.Interaction.RespondAsync(result.ErrorReason);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
