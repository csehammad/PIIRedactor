async function fetchHistory() {
  return new Promise((resolve) => {
    chrome.runtime.sendMessage({type: 'getClipboard'}, (response) => {
      resolve(response.history || []);
    });
  });
}

function renderHistory(history) {
  const container = document.getElementById('history');
  container.innerHTML = '';
  history.slice().reverse().forEach((entry) => {
    const div = document.createElement('div');
    div.className = 'entry';
    const textarea = document.createElement('textarea');
    textarea.value = entry.text;
    const saveBtn = document.createElement('button');
    saveBtn.textContent = 'Save';
    saveBtn.addEventListener('click', () => {
      chrome.runtime.sendMessage({type: 'updateClipboard', id: entry.id, text: textarea.value}, load);
    });
    const deleteBtn = document.createElement('button');
    deleteBtn.textContent = 'Delete';
    deleteBtn.addEventListener('click', () => {
      chrome.runtime.sendMessage({type: 'deleteClipboard', id: entry.id}, load);
    });
    div.appendChild(textarea);
    div.appendChild(document.createElement('br'));
    div.appendChild(saveBtn);
    div.appendChild(deleteBtn);
    container.appendChild(div);
  });
}

function load() {
  fetchHistory().then(renderHistory);
}

document.addEventListener('DOMContentLoaded', load);
