# PII Redactor Clipboard Manager

This repository contains a Windows 10 desktop application that monitors the clipboard and removes sensitive information. The application includes:

- Clipboard manager with history
- Action buttons for each entry to copy or delete
- Clear-all option to quickly remove history
- Alternating colors for easier reading of history items
- Configurable PII detection settings
- Enterprise templates for common policies
- Offline ML model integration for PII detection
- Basic installer using Inno Setup

See `src/PIIRedactorApp` for the WPF application.

## Building

```bash
dotnet build
```

## Running Tests

```bash
dotnet test
```

## Custom Patterns

Edit `config.json` to add or modify regex patterns used for redaction. You can also select predefined templates in the Settings window.

## Cross Platform

A fallback clipboard provider is included for non-Windows systems, but only basic functionality is available.
