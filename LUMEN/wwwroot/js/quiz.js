'use strict';
let timeRemaining, timerInterval;
function startQuizTimer(minutes) {
    timeRemaining = minutes * 60;
    timerInterval = setInterval(() => {
        timeRemaining--;
        const m = Math.floor(timeRemaining / 60);
        const s = timeRemaining % 60;
        const el = document.getElementById('timeRemaining');
        if (el) el.textContent = `${m}:${s.toString().padStart(2, '0')}`;
        if (timeRemaining <= 0) { clearInterval(timerInterval); document.getElementById('quizForm')?.submit(); }
    }, 1000);
}