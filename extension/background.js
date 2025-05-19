// Background service worker for PII Redactor + Clipboard history

// Default settings
const DEFAULT_SETTINGS = {
  enabled: true,
  maskEmail: '[EMAIL]',
  maskPhone: '[PHONE]',
  maskSSN: '[SSN]'
};

// Load settings from storage
async function getSettings() {
  return new Promise((resolve) => {
    chrome.storage.sync.get(DEFAULT_SETTINGS, (items) => {
      resolve(Object.assign({}, DEFAULT_SETTINGS, items));
    });
  });
}

// Regex patterns for common PII
const EMAIL_REGEX = /[\w.-]+@[\w.-]+\.[A-Za-z]{2,6}/g;
const PHONE_REGEX = /\b\d{3}[-.\s]?\d{3}[-.\s]?\d{4}\b/g;
const SSN_REGEX   = /\b\d{3}-?\d{2}-?\d{4}\b/g;

function maskText(text, settings) {
  if (!text) return text;
  let result = text;
  result = result.replace(EMAIL_REGEX, settings.maskEmail);
  result = result.replace(PHONE_REGEX, settings.maskPhone);
  result = result.replace(SSN_REGEX,   settings.maskSSN);
  return result;
}

chrome.runtime.onMessage.addListener((message, sender, sendResponse) => {
  // PII masking
  if (message.type === 'mask') {
    getSettings().then((settings) => {
      if (!settings.enabled) {
        sendResponse({ text: message.text });
      } else {
        sendResponse({ text: maskText(message.text, settings) });
      }
    });
    return true; // async

  // Clipboard history
  } else if (message.type === 'addClipboard') {
    addClipboardEntry(message.text).then(() => sendResponse({ success: true }));
    return true;

  } else if (message.type === 'getClipboard') {
    getClipboardHistory().then((history) => sendResponse({ history }));
    return true;

  } else if (message.type === 'updateClipboard') {
    updateClipboardEntry(message.id, message.text).then(() => sendResponse({ success: true }));
    return true;

  } else if (message.type === 'deleteClipboard') {
    deleteClipboardEntry(message.id).then(() => sendResponse({ success: true }));
    return true;
  }

  // silently ignore unknown messages
  return false;
});

// Clipboard helpers

async function getClipboardHistory() {
  return new Promise((resolve) => {
    chrome.storage.local.get({ clipboardHistory: [] }, (data) => {
      resolve(data.clipboardHistory);
    });
  });
}

async function saveClipboardHistory(history) {
  // keep only last 20 entries
  return new Promise((resolve) => {
    chrome.storage.local.set({ clipboardHistory: history.slice(-20) }, resolve);
  });
}

async function addClipboardEntry(text) {
  const history = await getClipboardHistory();
  history.push({ id: Date.now(), text });
  await saveClipboardHistory(history);
}

async function updateClipboardEntry(id, text) {
  const history = await getClipboardHistory();
  const idx = history.findIndex((e) => e.id === id);
  if (idx !== -1) {
    history[idx].text = text;
    await saveClipboardHistory(history);
  }
}

async function deleteClipboardEntry(id) {
  const history = await getClipboardHistory();
  await saveClipboardHistory(history.filter((e) => e.id !== id));
}
