# ImageProcessing

A lightweight Windows Forms desktop application demonstrating foundational Digital Image Processing (DIP) techniques on loaded bitmaps. Developed as a midterm project targeting **.NET Framework 4.7.2**.

---

## Features

* **File Operations**
* **Open**: Load bitmap and common image formats into the source viewport.
* **Save**: Export the processed result to disk.


* **Digital Image Processing (DIP)**
* **Pixel Copy**: Directly clone pixel buffers from source to target.
* **Grayscale**: Convert full-color RGB channels into balanced grayscale luminance.
* **Inversion**: Invert color values across all active color channels.
* **Mirror Horizontal**: Reflect the image along the vertical axis.
* **Mirror Vertical**: Reflect the image along the horizontal axis.


* **Dual Viewport Interface**: Side-by-side `PictureBox` layout providing immediate visual comparison between the original input and the processed output.

---

## Prerequisites

* **OS**: Windows 10 / 11
* **IDE**: Visual Studio 2022 (with the **.NET desktop development** workload installed)
* **Runtime / SDK**: .NET Framework 4.7.2 Developer Pack

---

## Getting Started

1. **Clone or Download** the repository to your local machine.
2. **Open the Solution**:
* Double-click `ImageProcessing.sln` (or `ImageProcessing.slnx`) inside Visual Studio.


3. **Build the Solution**:
* Press `Ctrl + Shift + B` (or select **Build > Build Solution** from the top menu).


4. **Run the Project**:
* Press `F5` to start debugging, or `Ctrl + F5` to run without debugging.



---

## Usage Guide

1. **Load Image**: Click **File > Open** and select an image file (`.png`, `.jpg`, `.bmp`). The image will render in the left viewport.
2. **Apply Filter**: Navigate to the **DIP** menu and click your desired operation (e.g., *Inversion*, *Greyscaling*). The transformed result will immediately appear in the right viewport.
3. **Export Result**: Select **File > Save** to save the processed output image to your machine.

---

## Project Structure

```text
ImageProcessing/
├── Form1.cs            # Core DIP algorithms and UI event handling
├── Form1.Designer.cs   # Windows Forms Designer auto-generated definitions
├── Form1.resx          # Form resource mapping
├── Program.cs          # Application entry point
├── App.config          # Application runtime configuration
└── ImageProcessing.sln # Visual Studio Solution file

```

---

## Notes

* **Windows Exclusive**: Relies on `System.Windows.Forms` and GDI+ rendering (`System.Drawing`), which run natively on Windows.
* **Zero External Dependencies**: Built entirely using native .NET Framework base classes; no third-party libraries required.
* **Academic Scope**: Created for educational purposes to demonstrate manual raster array manipulation and basic image processing concepts.