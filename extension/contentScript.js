// Content script for PII Redactor

// Helper to send text to background for masking
function maskText(text) {
  return new Promise((resolve) => {
    chrome.runtime.sendMessage({type: 'mask', text}, (response) => {
      resolve(response ? response.text : text);
    });
  });
}

// Intercept copy events to sanitize clipboard
function handleCopy(event) {
  const selection = document.getSelection();
  if (!selection) return;
  const text = selection.toString();
  if (!text) return;
  event.preventDefault();
  maskText(text).then((masked) => {
    event.clipboardData.setData('text/plain', masked);
    chrome.runtime.sendMessage({type: 'addClipboard', text: masked});
  });
}

document.addEventListener('copy', handleCopy, true);

// Intercept paste events to sanitize clipboard content before insertion
function handlePaste(event) {
  const clipboardData = event.clipboardData.getData('text/plain');
  if (!clipboardData) return;
  event.preventDefault();
  maskText(clipboardData).then((masked) => {
    if (event.target.value !== undefined) {
      event.target.value = masked;
    } else {
      event.target.innerText = masked;
    }
    chrome.runtime.sendMessage({type: 'addClipboard', text: masked});
  });
}

document.addEventListener('paste', handlePaste, true);

// Sanitize text in input fields
async function sanitizeField(element) {
  if (!element) return;
  const value = element.value || element.innerText;
  const masked = await maskText(value);
  if (element.value !== undefined) {
    element.value = masked;
  } else {
    element.innerText = masked;
  }
}

function handleInput(event) {
  const element = event.target;
  if (element.tagName === 'INPUT' || element.tagName === 'TEXTAREA' || element.isContentEditable) {
    sanitizeField(element);
  }
}

document.addEventListener('input', handleInput, true);

// Intercept form submissions to ensure fields are sanitized
function handleSubmit(event) {
  const form = event.target;
  const elements = form.querySelectorAll('input, textarea, [contenteditable="true"]');
  elements.forEach((el) => sanitizeField(el));
}

document.addEventListener('submit', handleSubmit, true);
