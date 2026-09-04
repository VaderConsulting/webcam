# webcam

Dave Robinson's working copy of Wei-Meng Lee's VB.NET WinForms webcam server/client that streams BMP frames over TCP. `RemoteMonitoring` (form titled “Server”) opens the default capture driver through `avicap32.dll` (`capCreateCaptureWindowA` / `WM_CAP_DRIVER_CONNECT`), copies preview frames to the clipboard on a timer into a shared `Image` byte array, and listens on `127.0.0.1:500`; each accepted `TcpClient` is tracked in `WebCamClient.AllClients` and is sent the latest BMP when the client writes a `Send` line. `RemoteMonitoringClient` connects to a configurable server IP (designer default `127.0.0.1`, port 500), requests frames of a fixed `SIZEOFIMAGE` (341504 bytes), and shows them in `PictureBox1` over a TV-bezel resource (`tv1.jpg`). VS conversion logs dated Sunday, 31 August 2008 record the ToolsVersion 2.0 → 3.5 upgrade.

**Source last updated:** 2008-08-31 · **Language:** VB.NET · **Target:** .NET Framework (VS 2005 ProductVersion 8.0.50727, ToolsVersion 3.5 after conversion; no `TargetFrameworkVersion`) · **Output:** WinForms executables (`WinExe`)

## Solution structure

| Project | Language | Type | Purpose |
|---------|----------|------|---------|
| `RemoteMonitoring` (`RemoteMonitoring/RemoteMonitoring.vbproj`) | VB.NET | WinForms exe | Webcam preview via `avicap32`; TCP listener on port 500; `WebCamClient` pushes BMP bytes. |
| `RemoteMonitoringClient` (`RemoteMonitoringClient/RemoteMonitoringClient.vbproj`) | VB.NET | WinForms exe | Start/Stop client; `txtServerIP` + `Send`/`Stop` protocol; displays received frames. |

`Backup/` folders are the VS conversion copies from 31 August 2008. Keep `_UpgradeReport_Files` as conversion provenance. `UpgradeLog.XML` is gitignored (internal hostname); a redacted `UpgradeLog.XML.example` is committed in each project.

## How to open

Open `webcam.sln` in Visual Studio 2005 or later (solution format 9.00 / Visual Studio 2005). Nested `RemoteMonitoring/RemoteMonitoring.sln` and `RemoteMonitoringClient/RemoteMonitoringClient.sln` open the projects separately. Requires a Windows webcam (or compatible WDM capture driver) for the server preview, and `avicap32.dll` / `user32.dll` P/Invoke. Default listen/connect address is `127.0.0.1:500`.

## Requirements

- Visual Studio 2005 to 2008

## Attribution and provenance

Working copy from Dave Robinson's OneDrive Historical Dev folder `webcam` of Wei-Meng Lee's DevX VB.NET webcam sample (see `THIRD_PARTY_NOTICES.md`).

- **Original author:** Wei-Meng Lee / DevX (2006)
- **Assembly title / product:** RemoteMonitoring, RemoteMonitoringClient
- **Assembly company:** (empty)
- **Assembly copyright:** Copyright ©  2006
- **Assembly version:** 1.0.0.0
- **Upgrade log:** Sunday, 31 August 2008 11:06 AM (redacted `UpgradeLog.XML.example`)

## License

Original Wei-Meng Lee / DevX publication terms (no license file in the OneDrive folder). This repository does **not** relicense the tree as VaderConsulting MIT. There is no separable Dave Robinson wrapper. See `LICENSE` and `THIRD_PARTY_NOTICES.md`.
