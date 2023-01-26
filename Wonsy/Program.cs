namespace Wonsy
{
    public class Program
    {
        static void Main(string[] args)
            => new Wonsy().RunAsync().GetAwaiter().GetResult();
    }
}