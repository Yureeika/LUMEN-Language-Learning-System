'use strict';

let connection;
let currentRoomId;
let currentUserId;
let currentUserName;

function initChat(roomId, userId, userName) {
    currentRoomId = roomId;
    currentUserId = userId;
    currentUserName = userName;

    // Create SignalR connection
    connection = new signalR.HubConnectionBuilder()
        .withUrl('/chatHub')
        .withAutomaticReconnect([0, 1000, 5000, 10000, 30000])
        .build();

    // Handle incoming messages
    connection.on('ReceiveMessage', (user, message, timestamp) => {
        appendMessage(user, message, timestamp, false);
    });

    // Handle user joined
    connection.on('UserJoined', (userId) => {
        addSystemMessage('A user joined the chat');
    });

    // Handle user left
    connection.on('UserLeft', (userId) => {
        addSystemMessage('A user left the chat');
    });

    // Connection status
    connection.onreconnecting(() => {
        document.getElementById('connectionStatus').textContent = 'Reconnecting...';
        document.getElementById('connectionStatus').className = 'badge bg-warning';
    });

    connection.onreconnected(() => {
        document.getElementById('connectionStatus').textContent = 'Connected';
        document.getElementById('connectionStatus').className = 'badge bg-success';
        connection.invoke('JoinRoom', currentRoomId);
    });

    connection.onclose(() => {
        document.getElementById('connectionStatus').textContent = 'Disconnected';
        document.getElementById('connectionStatus').className = 'badge bg-danger';
    });

    // Start connection
    connection.start()
        .then(() => {
            console.log('Connected to chat hub');
            connection.invoke('JoinRoom', currentRoomId);
        })
        .catch(err => {
            console.error('Connection failed:', err);
            document.getElementById('connectionStatus').textContent = 'Failed';
            document.getElementById('connectionStatus').className = 'badge bg-danger';
        });

    // Handle form submit
    document.getElementById('chatForm').addEventListener('submit', (e) => {
        e.preventDefault();
        sendMessage();
    });

    // Handle room switching
    document.querySelectorAll('.room-link').forEach(link => {
        link.addEventListener('click', (e) => {
            e.preventDefault();
            const newRoomId = parseInt(link.dataset.roomId);
            const newRoomName = link.dataset.roomName;
            switchRoom(newRoomId, newRoomName);
        });
    });

    // Scroll to bottom
    scrollToBottom();
}

function sendMessage() {
    const input = document.getElementById('messageInput');
    const message = input.value.trim();

    if (message && connection) {
        connection.invoke('SendMessage', currentRoomId, message)
            .then(() => {
                input.value = '';
            })
            .catch(err => {
                console.error('Send failed:', err);
                alert('Failed to send message');
            });
    }
}

function appendMessage(user, message, timestamp, isOwn) {
    const container = document.getElementById('chatMessages');
    const div = document.createElement('div');
    div.className = `mb-2 ${isOwn ? 'text-end' : ''}`;

    const time = new Date(timestamp).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
    const bgClass = isOwn ? 'bg-primary text-white' : 'bg-white border';

    div.innerHTML = `
        <div class="d-inline-block p-2 rounded ${bgClass}">
            <strong>${user}</strong>
            <p class="mb-0">${escapeHtml(message)}</p>
            <small class="${isOwn ? 'text-white-50' : 'text-muted'}">${time}</small>
        </div>
    `;

    container.appendChild(div);
    scrollToBottom();
}

function addSystemMessage(text) {
    const container = document.getElementById('chatMessages');
    const div = document.createElement('div');
    div.className = 'text-center mb-2';
    div.innerHTML = `<small class="text-muted"><em>${text}</em></small>`;
    container.appendChild(div);
    scrollToBottom();
}

function switchRoom(roomId, roomName) {
    if (connection) {
        connection.invoke('LeaveRoom', currentRoomId);
        currentRoomId = roomId;
        document.getElementById('currentRoomName').innerHTML = `<i class="bi bi-hash"></i> ${roomName}`;
        connection.invoke('JoinRoom', currentRoomId);

        // Update active state
        document.querySelectorAll('.room-link').forEach(link => {
            link.classList.remove('active');
            if (parseInt(link.dataset.roomId) === roomId) {
                link.classList.add('active');
            }
        });

        // Clear messages and load new ones
        document.getElementById('chatMessages').innerHTML = `
            <div class="text-center text-muted mt-5">
                <i class="bi bi-chat-square-text display-4"></i>
                <p>Loading messages...</p>
            </div>
        `;
    }
}

function scrollToBottom() {
    const container = document.getElementById('chatMessages');
    container.scrollTop = container.scrollHeight;
}

function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

// Allow Enter key to send
document.addEventListener('DOMContentLoaded', () => {
    const input = document.getElementById('messageInput');
    if (input) {
        input.addEventListener('keypress', (e) => {
            if (e.key === 'Enter') {
                e.preventDefault();
                sendMessage();
            }
        });
    }
});