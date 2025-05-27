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
                x.Service<WinServiceGotJira>(s =>
                {
                    s.ConstructUsing(clsservice => new WinServiceGotJira());
                    s.WhenStarted(clsservice => clsservice.Start());
                    s.WhenStopped(clsservice => clsservice.Stop());
                });

                x.RunAsLocalSystem();
                x.SetServiceName("WinServiceGotJira");
                x.SetDisplayName("Servicio GotJira");
                x.SetDescription("Actualización Jira y TimeSheet (<GotJira>)");
            });

            int exitCoreValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCoreValue;  
        }
    }
}
