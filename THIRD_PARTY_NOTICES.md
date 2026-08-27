# Third-Party Notices — webcam

This tree is Dave Robinson's working copy of Wei-Meng Lee's DevX VB.NET webcam
TCP-streaming sample (`RemoteMonitoring` + `RemoteMonitoringClient`). There is
no separable VaderConsulting wrapper. Do not treat the sample source as
VaderConsulting MIT-licensed original work.

## Wei-Meng Lee / DevX (2006)

VB.NET 2005 WinForms pair that captures webcam frames through AVICap
(`avicap32.dll` / `user32.dll` P/Invoke) and streams BMP bytes over TCP
port 500. The sample is the follow-up to Lee's DevX articles:

- *Teach Your Old Web Cam New Tricks: Use Video Captures in Your .NET Applications*
- *Building an Enhanced Security System with a Web Cam and a Servo*
- Learn2Develop.Net: *Monitor Your Web Cam from a Remote Computer*

The remote-viewing article shows a server named `RemoteMonitoring` and a client
named `RemoteMonitoringClient`, with the same `WebCamClient` class, `Send`/`LF`
protocol, `SIZEOFIMAGE = 341504`, and localhost `127.0.0.1:500` defaults found
in this folder. Assembly title/product match those project names; assembly
copyright is `Copyright ©  2006` with empty `AssemblyCompany`.

No DevX or author license file was present in the OneDrive Historical Dev
folder. Original publication terms (if any) remain with Wei-Meng Lee / DevX.
VaderConsulting/Dave Robinson did not author this sample.

## Visual Studio conversion artefacts

`UpgradeLog.XML` (gitignored; redacted `UpgradeLog.XML.example` committed),
`_UpgradeReport_Files/`, and `Backup/` are Visual Studio 2008 conversion
output dated Sunday 31 August 2008. Those are tooling artefacts, not a
separate third-party product.
