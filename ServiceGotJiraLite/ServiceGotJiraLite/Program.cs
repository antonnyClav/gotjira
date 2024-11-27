using System;
using Topshelf;

namespace AddAtlassianGotJiraJiras
{
    class Program
    {
        static void Main(string[] args)
        {
             var exitCode = HostFactory.Run(x =>
            {
                x.Service<WinServiceGotJiraLite>(s =>
                {
                    s.ConstructUsing(clsservice => new WinServiceGotJiraLite());
                    s.WhenStarted(clsservice => clsservice.Start());
                    s.WhenStopped(clsservice => clsservice.Stop());
                });

                x.RunAsLocalSystem();
                x.SetServiceName("WinServiceGotJiraLite");
                x.SetDisplayName("Servicio GotJiraLite");
                x.SetDescription("Actualización Jira y TimeSheet (<GotJiraLite>)");
            });

            int exitCoreValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCoreValue;  
        }
    }
}
