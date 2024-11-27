using Sugar;
using System;
using Topshelf;
using System.Configuration;

namespace AddAtlassianGotJiraJiras
{
    class Program
    {
        static void Main(string[] args)
        {
            var exitCode = HostFactory.Run(x =>
            {
                x.Service<WinServiceSugar>(s =>
                {
                    s.ConstructUsing(clsservice => new WinServiceSugar());
                    s.WhenStarted(clsservice => clsservice.Start());
                    s.WhenStopped(clsservice => clsservice.Stop());
                });

                x.RunAsLocalSystem();
                x.SetServiceName("WinServiceSugar");
                x.SetDisplayName("Servicio Emx-Sugar");
                x.SetDescription("Actualización Emx-Sugar");
            });

            int exitCoreValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCoreValue;  
        }
    }
}
