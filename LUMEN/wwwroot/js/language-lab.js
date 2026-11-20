'use strict';

let completedCount = 0;
let correctCount = 0;
let xpEarned = 0;

const exercises = [
    { source: 'en', target: 'es', text: 'Hello', answer: 'hola' },
    { source: 'en', target: 'es', text: 'Thank you', answer: 'gracias' },
    { source: 'en', target: 'fr', text: 'Good morning', answer: 'bonjour' },
    { source: 'en', target: 'fr', text: 'How are you', answer: 'comment allez-vous' },
    { source: 'en', target: 'de', text: 'Yes', answer: 'ja' },
    { source: 'en', target: 'de', text: 'No', answer: 'nein' },
    { source: 'en', target: 'it', text: 'Please', answer: 'per favore' },
    { source: 'en', target: 'pt', text: 'Goodbye', answer: 'adeus' },
    { source: 'es', target: 'en', text: 'Hola', answer: 'hello' },
    { source: 'fr', target: 'en', text: 'Merci', answer: 'thank you' }
];

let currentExercise = 0;

document.addEventListener('DOMContentLoaded', () => {
    loadExercise();

    document.getElementById('checkAnswerBtn')?.addEventListener('click', checkAnswer);
    document.getElementById('nextExerciseBtn')?.addEventListener('click', nextExercise);

    document.getElementById('translateBtn')?.addEventListener('click', translateText);
});

function loadExercise() {
    const exercise = exercises[currentExercise];
    const langNames = {
        'en': 'English', 'es': 'Spanish', 'fr': 'French', 'de': 'German',
        'it': 'Italian', 'pt': 'Portuguese', 'zh': 'Chinese', 'ja': 'Japanese',
        'ko': 'Korean', 'ar': 'Arabic'
    };

    document.getElementById('exerciseText').textContent =
        `Translate from ${langNames[exercise.source]} to ${langNames[exercise.target]}: "${exercise.text}"`;
    document.getElementById('userAnswer').value = '';
    document.getElementById('feedbackContainer').innerHTML = '';
}

function checkAnswer() {
    const userAnswer = document.getElementById('userAnswer').value.trim().toLowerCase();
    const correctAnswer = exercises[currentExercise].answer.toLowerCase();
    const feedbackContainer = document.getElementById('feedbackContainer');

    if (userAnswer === correctAnswer) {
        feedbackContainer.innerHTML = '<div class="alert alert-success"><i class="bi bi-check-circle"></i> Correct! Well done! +5 XP</div>';
        correctCount++;
        xpEarned += 5;
    } else {
        feedbackContainer.innerHTML = `<div class="alert alert-danger"><i class="bi bi-x-circle"></i> Incorrect. The correct answer is: "${correctAnswer}"</div>`;
    }

    completedCount++;
    updateProgress();
}

function nextExercise() {
    currentExercise = (currentExercise + 1) % exercises.length;
    loadExercise();
}

function updateProgress() {
    document.getElementById('completedCount').textContent = completedCount;
    document.getElementById('correctCount').textContent = correctCount;
    document.getElementById('xpEarned').textContent = xpEarned;
}

async function translateText() {
    const sourceLang = document.getElementById('sourceLanguage').value;
    const targetLang = document.getElementById('targetLanguage').value;
    const inputText = document.getElementById('inputText').value.trim();

    if (!inputText) {
        alert('Please enter text to translate');
        return;
    }

    const resultDiv = document.getElementById('translationResult');
    const translatedText = document.getElementById('translatedText');

    translatedText.textContent = 'Translating...';
    resultDiv.style.display = 'block';

    // Simulate translation (in production, use a real translation API)
    setTimeout(() => {
        const translations = {
            'hello': { 'es': 'hola', 'fr': 'bonjour', 'de': 'hallo', 'it': 'ciao', 'pt': 'olá', 'zh': '你好', 'ja': 'こんにちは', 'ko': '안녕하세요', 'ar': 'مرحبا' },
            'thank you': { 'es': 'gracias', 'fr': 'merci', 'de': 'danke', 'it': 'grazie', 'pt': 'obrigado', 'zh': '谢谢', 'ja': 'ありがとう', 'ko': '감사합니다', 'ar': 'شكرا' },
            'good morning': { 'es': 'buenos días', 'fr': 'bonjour', 'de': 'guten morgen', 'it': 'buongiorno', 'pt': 'bom dia', 'zh': '早上好', 'ja': 'おはようございます', 'ko': '좋은 아침', 'ar': 'صباح الخير' }
        };

        const lowerText = inputText.toLowerCase();
        let translation = `[${targetLang.toUpperCase()}] Translation of "${inputText}"`;

        if (translations[lowerText] && translations[lowerText][targetLang]) {
            translation = translations[lowerText][targetLang];
        }

        translatedText.textContent = translation;
    }, 500);
}