'use strict';
document.addEventListener('DOMContentLoaded', () => {
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(el => new bootstrap.Tooltip(el));
});

function showToast(message, type = 'info') {
    const toastEl = document.getElementById('liveToast');
    document.getElementById('toastMessage').textContent = message;
    toastEl.className = `toast text-bg-${type}`;
    new bootstrap.Toast(toastEl).show();
}