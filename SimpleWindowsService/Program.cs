using System;
using System.DirectoryServices.AccountManagement;
using System.ServiceProcess;

namespace SimpleWindowsService
{
    // Refer to http://www.cnblogs.com/lc-chenlong/p/3948760.html for publish the windows service.
    // Refer to https://stackoverflow.com/questions/7764088/net-console-application-as-windows-service to for host windows service.
    class Program
    {
        #region Nested classes to support running as service
        public const string ServiceName = "AAMyService";

        public class Service : ServiceBase
        {
            public Service()
            {
                ServiceName = Program.ServiceName;
            }

            protected override void OnStart(string[] args)
            {
                Program.Start(args);
            }

            protected override void OnStop()
            {
                Program.Stop();
            }
        }
        #endregion

        static void Main(string[] args)
        {
            try
            {
                if (!Environment.UserInteractive)
                {
                    Log.Info("Run as windows service.");
                    // running as service
                    using (var service = new Service())
                    {
                        ServiceBase.Run(service);
                    }
                }
                else
                {
                    Console.WriteLine("Run as console application.");
                    Log.Info("Run as console application.");

                    // running as console app
                    Start(args);

                    Console.WriteLine("Press any key to stop...");
                    Console.ReadKey(true);

                    Stop();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }

        public static void Process()
        {
            Log.Info("Enter Process method.");
            try
            {
                using (var principalContext = new PrincipalContext(ContextType.Machine))
                {
                    var userPrincipal = UserPrincipal.FindByIdentity(principalContext, IdentityType.Sid, "S-1-5-21-1157947708-1632297382-124367426-500");
                    if (userPrincipal != null)
                    {
                        string samAccountName = string.Format("SamAccountName for Administrator user is {0}", userPrincipal.SamAccountName);
                        string desc = string.Format("Description for Administrator user is {0}", userPrincipal.Description);
                        Log.Info(samAccountName);
                        Log.Info(desc);
                        Console.WriteLine(samAccountName);
                        Console.WriteLine(desc);
                    }
                    else
                    {
                        Log.Warning("UserPrincipal for Administrator user is null");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }

        private static void Start(string[] args)
        {
            Process();
        }

        private static void Stop()
        {
            // onstop code here
            Log.Info("Enter Stop method.");
        }
    }
}
