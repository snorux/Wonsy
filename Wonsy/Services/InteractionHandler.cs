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

            _client.SlashCommandExecuted += SlashCommandExecuted;
        }

        private async Task SlashCommandExecuted(SocketSlashCommand command)
        {
            try
            {
                // Create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules.
                var context = new SocketInteractionContext(_client, command);

                // Execute the incoming command.
                var result = await _interactionService.ExecuteCommandAsync(context, _serviceProvider);

                if (!result.IsSuccess)
                    switch (result.Error)
                    {
                        case InteractionCommandError.UnmetPrecondition:
                            // implement
                            break;
                        default:
                            break;
                    }
            }
            catch
            {
                // If Slash Command execution fails it is most likely that the original interaction acknowledgement will persist. It is a good idea to delete the original
                // response, or at least let the user know that something went wrong during the command execution.
                if (command.Type is InteractionType.ApplicationCommand)
                    await command.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
            }
        }
    }
}
