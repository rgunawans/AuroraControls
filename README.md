# AuroraControls

A custom UI component library for .NET WinForms. Built to provide clean, functional, and modern enhancements to standard Windows Forms controls.

### Target Frameworks
* **.NET 8.0+** (`net8.0-windows`)
* **.NET Framework 4.7.2+** (`net472`)
* **.NET Framework 4.5** (`net45`)

### Controls & Features
* **`AButton`**: Customizable hover and focus appearance (`HoverColor`, `FocusColor`).
* **`ACheckBox`**: Enhanced checked and unchecked background and border states (`CheckedColor`, `UncheckedColor`).
* **`ALabelTimer`**: Built-in clock/timer formatting with customizable interval, display formats, and date-time updates.
* **`ATextBox`**:
  * `Mandatory`: Visual required-field indicators (`MandatoryColor`).
  * `SelectAllOnFocus`: Automatically highlight text when control gains focus.
  * `TextCasing`: Text formatting support (`Normal`, `LowerCase`, `UpperCase`, `TitleCase`, `SentenceCase`).
  * Custom focus styling (`FocusBorderColor`, `FocusBackColor`).

### Getting Started
1. Clone the repository:
   ```bash
   git clone https://github.com/rgunawans/AuroraControls.git
   ```
2. Build solution:
   ```bash
   dotnet build
   ```
3. Run tests:
   ```bash
   dotnet test
   ```
4. Reference `AuroraControls.dll` in your WinForms project or generate a NuGet package:
   ```bash
   dotnet pack -c Release
   ```

### Support
Support future development: https://paypal.me/rgunawans
