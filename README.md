# Bank CCTV Theft Detection (C# / .NET)

M.Tech final project · Bharathidasan University, India · 2023 · solo project

A Windows desktop application that watches a CCTV / webcam feed during restricted hours
(for example when a bank is closed). When it detects motion or an unknown face, it captures
the frame and alerts the admin by **email (with the image)** and **SMS**.

## How it works
1. **Timer settings:** the admin sets the restricted time period.
2. **Motion detection:** frames from the camera are compared with a background frame
   (grayscale → difference → threshold → erosion) to find moving objects (AForge.NET).
3. **Face detection and recognition:** faces are detected and matched against known users
   stored in a SQL Server database (Luxand FaceSDK).
4. **Alerts:** if motion or an unknown face is found in the restricted period, the app saves a
   snapshot and sends an email alert with the image (SMTP) and an SMS alert.

## Files
| File | Purpose |
|---|---|
| `Start1.cs` | start screen |
| `LoadCamera.cs` | camera selection, motion detection loop, alerts |
| `Login.cs` | face detection / recognition with Luxand FaceSDK, database, alerts |
| `Camera.cs` | camera wrapper and frame handling |
| `CaptureDeviceForm.cs` | capture device selection form |
| `MotionDetector1.cs`, `MotionDetector3Optimized.cs` | motion detection algorithms |

**Tech:** C#, .NET Windows Forms, AForge.NET (imaging), Luxand FaceSDK, SQL Server (LocalDB), SMTP

## Credits
`MotionDetector1.cs` and `MotionDetector3Optimized.cs` are adapted from the open-source
*Motion Detection* sample by **Andrew Kirillov** (© 2005-2006), built on AForge.NET.
Face detection uses the commercial **Luxand FaceSDK** (needs your own license key).

## Note
This is an **archived student project**. The original Visual Studio solution was not kept, so
the code here was **recovered from the printed source-code chapter of the 2023 project report**.
Line breaks from the printed pages remain, and designer files / references are missing, so it
will not build as-is.

For safety, all passwords, email addresses, the SMS account details, phone numbers and the
FaceSDK license key were replaced with placeholders such as `YOUR_APP_PASSWORD`.

Author: Marimuthu Mayavan · [mayavan-m.github.io](https://mayavan-m.github.io)
