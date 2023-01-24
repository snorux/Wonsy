namespace Wonsy.Services
{
    internal class InteractionHandler
    {
        private readonly Configuration configuration;

        public InteractionHandler(IOptions<Configuration> config)
        {
            configuration = config.Value;
        }
    }
}
