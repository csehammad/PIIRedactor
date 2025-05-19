# PII Redactor Chrome Extension

This extension automatically detects and masks common personally identifiable information (PII) in clipboard contents and text input fields before it can be submitted.

## Features
- Replaces email addresses, phone numbers, and Social Security numbers with placeholders.
- Sanitizes text copied to the clipboard.
- Sanitizes text as you type or paste into input fields and before form submission.
- Simple options page for enabling/disabling the extension and customizing placeholder text.

## Development
1. Open Chrome and navigate to `chrome://extensions`.
2. Enable **Developer mode**.
3. Click **Load unpacked** and select the `extension` folder in this repository.
4. Adjust options via the extension's options page if needed.

All processing happens locally in the browser.
