ImageProcessing
A small Windows Forms image-processing demo that provides basic digital image processing (DIP) operations on loaded images. Built as a midterm project using .NET Framework 4.7.2.
Features
•	Open and save images (File > Open, File > Save)
•	DIP operations (DIP menu):
•	Pixel Copy — copy pixels between images
•	Greyscaling — convert image to grayscale
•	Inversion — invert image colors
•	Mirror Horizontal — flip image horizontally
•	Mirror Vertical — flip image vertically
•	Simple dual PictureBox UI: original image and processed result
Getting started
1.	Open the solution in Visual Studio: Midterm/ImageProcessing/ImageProcessing.slnx
2.	Restore NuGet packages (if any) and build the solution. Target framework: .NET Framework 4.7.2.
3.	Run the project (F5). The application window provides menus for loading, processing, and saving images.
Usage
•	Load an image via File > Open.
•	Choose a processing operation under DIP. The processed output appears in the second PictureBox.
•	Save the processed image via File > Save.
Project structure (important files)
•	Form1.cs / Form1.Designer.cs — main WinForms UI and event handlers
•	ImageProcessing.slnx — solution file
Notes
•	Windows-only WinForms application.
•	No external dependencies required beyond the .NET Framework target.
•	Intended for educational/demonstration use.