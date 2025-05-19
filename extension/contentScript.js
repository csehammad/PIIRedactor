// Content script for PII Redactor + Clipboard history

// Helper to send text to background for masking
function maskText(text) {
  return new Promise((resolve) => {
    chrome.runtime.sendMessage({ type: 'mask', text }, (response) => {
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
    // Put masked text on the clipboard
    event.clipboardData.setData('text/plain', masked);
    // Record in history
    chrome.runtime.sendMessage({ type: 'addClipboard', text: masked });
  });
}

document.addEventListener('copy', handleCopy, true);

// Intercept paste events to sanitize clipboard content before insertion
function handlePaste(event) {
  const clipboardData = event.clipboardData.getData('text/plain');
  if (!clipboardData) return;

  event.preventDefault();
  maskText(clipboardData).then((masked) => {
    // Insert masked text into the target
    if (event.target.value !== undefined) {
      event.target.value = masked;
    } else {
      event.target.innerText = masked;
    }
    // Record in history
    chrome.runtime.sendMessage({ type: 'addClipboard', text: masked });
  });
}

document.addEventListener('paste', handlePaste, true);

// Sanitize text in input fields on-the-fly
async function sanitizeField(element) {
  if (!element) return;
  const current = element.value ?? element.innerText;
  const masked  = await maskText(current);
  if (element.value !== undefined) {
    element.value = masked;
  } else {
    element.innerText = masked;
  }
}

function handleInput(event) {
  const el = event.target;
  if (el.tagName === 'INPUT' || el.tagName === 'TEXTAREA' || el.isContentEditable) {
    sanitizeField(el);
  }
}

document.addEventListener('input', handleInput, true);

// Ensure all fields are sanitized on form submission
function handleSubmit(event) {
  const form = event.target;
  const els  = form.querySelectorAll('input, textarea, [contenteditable="true"]');
  els.forEach(sanitizeField);
}

document.addEventListener('submit', handleSubmit, true);
