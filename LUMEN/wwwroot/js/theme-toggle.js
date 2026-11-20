'use strict';
document.addEventListener('DOMContentLoaded', () => {
    const toggle = document.getElementById('themeToggle');
    const html = document.documentElement;
    const saved = localStorage.getItem('theme') || 'light';
    html.setAttribute('data-bs-theme', saved);

    toggle?.addEventListener('click', () => {
        const current = html.getAttribute('data-bs-theme');
        const next = current === 'light' ? 'dark' : 'light';
        html.setAttribute('data-bs-theme', next);
        localStorage.setItem('theme', next);
    });
});