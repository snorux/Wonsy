namespace Wonsy
{
    internal class Program
    {
        static void Main(string[] args)
            => new Wonsy().RunAsync().GetAwaiter().GetResult();
    }
}