# PII Redactor Chrome Extension

This extension automatically detects and masks common personally identifiable information (PII) in clipboard contents and text input fields before it can be submitted.

## Features
- Replaces email addresses, phone numbers, and Social Security numbers with placeholders.
- Sanitizes text copied to the clipboard and stores a history of masked entries.
- Sanitizes text as you type or paste into input fields and before form submission.
- Popup clipboard manager lets you view, edit, or delete masked clipboard entries.
- Simple options page for enabling/disabling the extension and customizing placeholder text.

## Development
1. Open Chrome and navigate to `chrome://extensions`.  
2. Enable **Developer mode**.  
3. Click **Load unpacked** and select the `extension` folder in this repository.  
4. Adjust options via the extension’s options page if needed.  
5. Click the extension icon to open the clipboard manager popup.

All processing happens locally in the browser.
