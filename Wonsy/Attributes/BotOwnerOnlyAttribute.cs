using Microsoft.Extensions.DependencyInjection;

namespace Wonsy.Attributes
{
    public class BotOwnerOnlyAttribute : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context, ICommandInfo commandInfo, IServiceProvider services)
        {
            var config = services.GetRequiredService<IOptions<Configuration>>().Value;
            var isOwner = config.BotConfig.BotOwners.Any(x => x == context.User.Id);

            return Task.FromResult(isOwner ? PreconditionResult.FromSuccess() : PreconditionResult.FromError("You must be added to \"BotOwners\" in config.json to use this command."));
        }
    }
}
