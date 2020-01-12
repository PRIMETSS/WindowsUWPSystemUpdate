# WindowsUWPSystemUpdate [Win10 IoT Core]
 Force Windows Updates on RPi 3 Windows IoT UWP

 Windows 10 IoT Current OS Version: 10.0.17763.107 failed to download / install updates via Windows Update.
 
 "\\10.1.1.28\c$\Data\ProgramData\SoftwareDistribution\ReportingEvents.log" Always reported 0 Updates to process
 {050F3909-2A9D-4F63-8A47-6A406EC41298}	2020-01-12 11:43:35:849+1030	1	147 [AGENT_DETECTION_FINISHED]	101	{00000000-0000-0000-0000-000000000000}	0	0	Update;API;16454Windows10IOTCore.IOTCoreDefaultApplication_rz84sjny4rf58	
 Success	Software Synchronization	Windows Update Client successfully detected 0 updates.	WK95uV225Ue0wrVj.2.3.1.2.3.1.1.0.0.3.0"

Changing Flighting mode seemed not to help.
For Days and multiple reboots did not help and often the Device Portal Windows Update showed "Downloading updates"
But the "\\10.1.1.28\c$\Data\ProgramData\SoftwareDistribution\Download" folder did not change over this time period

After Deploying this UWP app and calling Windows.System.Update.SystemUpdateManager.StartInstall(Windows.System.Update.SystemUpdateStartInstallAction.UpToReboot);
Updates start to flow again

Package Manifest needs this capability set in XML or Manefest UI Page

````
    <Capabilities\>
        <Capability Name="internetClient" />
        <iot:Capability Name="systemManagement"/>
    </Capabilities>
````

Issues: Can throw Exception that has no Message Text associated with it!!

System Event PowerShell: Get-CimInstance Win32_NTLogEvent | Where-Object {$_.Category -eq "1"}
