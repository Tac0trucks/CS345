# ImageProcessing

Simple Windows Forms image processing demo project (student midterm).  
Targets `.NET Framework 4.7.2`.

## Requirements
- Microsoft Visual Studio (open the solution via __File > Open > Project/Solution__)  
- .NET Framework 4.7.2 developer targeting pack installed

## Build & Run
1. Open `ImageProcessing.slnx` in Visual Studio.  
2. Set configuration to `Debug` or `Release`.  
3. Build and run (F5) the `ImageProcessing` project.

## Features
- Basic digital image processing utilities (filters and sample DIP operations).  
- Windows Forms UI (`Form1`) for loading/applying filters and viewing results.  
- Device and device manager classes for hardware/IO abstractions (if present in environment).

## Important files
- `ImageProcessing.csproj` — project file (targets .NET Framework v4.7.2)  
- `Form1.cs`, `Form1.Designer.cs`, `Form1.resx` — main UI  
- `BasicDIP.cs`, `CITLIBFilters.cs` — image processing/filter implementations  
- `Device.cs`, `DeviceManager.cs` — device handling abstractions  
- `Program.cs` — app entry point

## Notes
- Project is configured for `AnyCPU`. See `Properties` for app settings and resources.  
- Adjust input/output paths and device settings in code if running outside the original environment.

## Contributing
Small fixes and improvements welcome. Open a PR against the `main` branch.
