using System;
using System.Collections;
using System.Configuration.Install;
using System.ServiceProcess;
using System.ComponentModel;
using System.Management;

[RunInstaller(true)]
public class MyProjectInstaller : Installer
{
    private ServiceInstaller serviceInstaller1;
    private ServiceProcessInstaller processInstaller;

    public MyProjectInstaller()
    {
        // Instantiate installers for process and services.
        processInstaller = new ServiceProcessInstaller();
        serviceInstaller1 = new ServiceInstaller();

        // The services run under the system account.
        processInstaller.Account = ServiceAccount.LocalSystem;
        //processInstaller.Committed += serviceInstaller_Committed;

        // The services are started manually.
        serviceInstaller1.StartType = ServiceStartMode.Manual;

        // ServiceName must equal those on ServiceBase derived classes.
        serviceInstaller1.ServiceName = "AAMyService";

        // Add installers to collection. Order is not important.
        Installers.Add(serviceInstaller1);
        Installers.Add(processInstaller);
    }

    void serviceInstaller_Committed(object sender, InstallEventArgs e)
    {
        using (ManagementObject service = new ManagementObject(new ManagementPath("Win32_Service.Name='AAMyService'")))
        {
            object[] wmiParams = new object[11];
            wmiParams[6] = @"NT Service\AAMyService";
            service.InvokeMethod("Change", wmiParams);
        }
    }
}