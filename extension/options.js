// Options page script

const DEFAULT_SETTINGS = {
  enabled: true,
  maskEmail: '[EMAIL]',
  maskPhone: '[PHONE]',
  maskSSN: '[SSN]'
};

function restoreOptions() {
  chrome.storage.sync.get(DEFAULT_SETTINGS, (items) => {
    document.getElementById('enabled').checked = items.enabled;
    document.getElementById('maskEmail').value = items.maskEmail;
    document.getElementById('maskPhone').value = items.maskPhone;
    document.getElementById('maskSSN').value = items.maskSSN;
  });
}

document.getElementById('save').addEventListener('click', () => {
  const settings = {
    enabled: document.getElementById('enabled').checked,
    maskEmail: document.getElementById('maskEmail').value,
    maskPhone: document.getElementById('maskPhone').value,
    maskSSN: document.getElementById('maskSSN').value
  };
  chrome.storage.sync.set(settings, () => {
    const status = document.getElementById('status');
    status.textContent = 'Options saved.';
    setTimeout(() => { status.textContent = ''; }, 1000);
  });
});

document.addEventListener('DOMContentLoaded', restoreOptions);
