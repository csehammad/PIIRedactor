const HISTORY_KEY = 'clipboardHistory';

function loadHistory() {
  chrome.storage.local.get({ [HISTORY_KEY]: [] }, (result) => {
    const history = result[HISTORY_KEY];
    const list = document.getElementById('history');
    list.innerHTML = '';
    history.forEach((entry, index) => {
      const li = document.createElement('li');
      const textarea = document.createElement('textarea');
      textarea.value = entry.text;
      li.appendChild(textarea);
      const save = document.createElement('button');
      save.textContent = 'Save';
      save.addEventListener('click', () => {
        updateEntry(index, textarea.value);
      });
      const del = document.createElement('button');
      del.textContent = 'Delete';
      del.addEventListener('click', () => {
        deleteEntry(index);
      });
      li.appendChild(save);
      li.appendChild(del);
      list.appendChild(li);
    });
  });
}

function updateEntry(index, text) {
  chrome.storage.local.get({ [HISTORY_KEY]: [] }, (result) => {
    const history = result[HISTORY_KEY];
    if (index < history.length) {
      history[index].text = text;
      chrome.storage.local.set({ [HISTORY_KEY]: history }, loadHistory);
    }
  });
}

function deleteEntry(index) {
  chrome.storage.local.get({ [HISTORY_KEY]: [] }, (result) => {
    const history = result[HISTORY_KEY];
    if (index < history.length) {
      history.splice(index, 1);
      chrome.storage.local.set({ [HISTORY_KEY]: history }, loadHistory);
    }
  });
}

document.addEventListener('DOMContentLoaded', loadHistory);
