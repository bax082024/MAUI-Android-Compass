# MawiCompass 🧭

A tiny, privacy-friendly GPS-free compass built with .NET MAUI.
Works anywhere 

- **no internet, no accounts, no ads, no tracking**.

- Uses the phone’s magnetometer + tilt compensation to show Magnetic North

- Optional True North mode via manual declination (east = +, west = −)

- Haptic “snap” when you align to N/E/S/W

- Interference warning if magnetic readings look off

- Smooth, dark UI with a red north needle, minor ticks every 10°, and an info page

Why GPS-free? It still points north even if GPS is jammed, spoofed, or offline.

--- 

## Features

- 🧭 Magnetic heading (tilt-compensated) — no GPS required

- 🎯 True North toggle with declination input & persistent settings

- 💥 Haptic feedback near 0°/90°/180°/270°

- ⚠️ Interference detection (typical Earth field 25–65 µT)

- 🌙 Dark theme, clear dial, minor ticks every 10°

- ℹ️ Declination info page with links to trusted resources

- 🛡️ No network calls, ads, analytics, or permissions (Android)

---  

## Screenshot

<img src="Resources/Images/app.png" alt="1" width="220">

---

## Tech Stack

- .NET 8 + .NET MAUI

- MAUI Essentials: Compass, Magnetometer, HapticFeedback

- Simple sensor fusion & smoothing via a Low-pass filter

---

## Getting Started

**Prerequisites:**

- Visual Studio 2022 with the .NET MAUI workload

- Android SDK / Emulator (for development)

- A real device for testing (emulators usually have no magnetometer)

---

## Run (Debug)

1. Open the solution in VS.

2. Select Android and your physical device in the target dropdown.

3. Press ▶ Run.

On emulators, the heading will stay at 0° because there’s no magnetometer.

---

## Build Release APK (sideload)

1. Set Configuration to Release.

2. Build → Archive → Distribute → Ad Hoc → APK.

3. Sign with your keystore and install on device:
	- 'adb install -r path\to\MawiCompass-Signed.apk'

---

## Download

- **Android APK (signed):** [Download MawiCompass](APK/com.bax.mawicompass.apk)

> If your browser opens a preview page on GitHub, use the **Download raw file** button.  
> After downloading, you may need to allow installs from unknown sources.

### Sideload instructions
1. Copy the APK to your phone (or open this page on the phone and download it).
2. Tap the APK → allow **Install unknown apps** for your browser/files app when prompted.
3. Install and open **MawiCompass**.

**Notes**
- The APK is signed with a key dedicated to this app. Future updates will use the **same** key so you can update in-place.
- If you ever see “App not installed,” uninstall any older build signed with a different key first.
- Architecture: built for **arm64-v8a** (most modern devices). If you need x86 or armeabi-v7a, rebuild with those ABIs.


## True North / Declination

- Toggle True North on/off in the UI.

- Enter declination in degrees:

	- East = positive (e.g., +2.5)

	- West = negative (e.g., -4)

- Values are saved using Preferences.

- Declination changes slowly. Set it once for your region or open the Info page to find it.

--- 

## Permissions

- Android: none required for magnetic heading.
(We don’t request location; True North uses manual declination.)

- Note: Some OEMs can throttle sensors in background; the app is intended for foreground use.

--- 

Calibration & accuracy

- If readings drift, move the phone in a figure-8 to calibrate.

- Avoid nearby magnets/metal (car mounts, laptops, speakers).

- The app shows Interference if the measured field is atypical.

Disclaimer: Not for aviation or other safety-critical navigation.

--- 

## License

MIT License

Copyright (c) 2025 <bax082024>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the “Software”), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

---




