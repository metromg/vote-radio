using Radio.Infrastructure.DbAccess.Configuration;

namespace Radio.Startup.Console
{
    public class Program
    {
        //TODO: Setup fluent scheduler
        public static void Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");

            var containerForDbInitialization = Bootstrapper.BootstrapContainerForDbInitialization();

            DbInitializer.Initialize(containerForDbInitialization);
        }
    }
}
