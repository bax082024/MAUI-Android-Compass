# MawiCompass 🧭

A tiny, privacy-friendly GPS-free compass built with .NET MAUI.
Works anywhere—no internet, no accounts, no ads, no tracking.

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
