
    const connection = new signalR.HubConnectionBuilder()
    .withUrl("/gameHub")
    .build();

    connection.on("ReceiveMessage", (user, message) => {
    const chatMessages = document.getElementById("chat-messages");
    const messageElement = document.createElement("div");
    messageElement.classList.add("message");
    messageElement.innerHTML = `<span class="sender">${user}</span><p class="text">${message}</p>`;
    chatMessages.appendChild(messageElement);
});

    connection.start().then(() => {
    console.log("SignalR Connected.");
}).catch(err => console.error("SignalR Connection Error: ", err.toString()));

    function sendMessage() {
    const user = "User"; // Replace with actual user
    const message = document.getElementById("chat-text-input").value;
    if (message.trim() !== "") {
    console.log("Sending message: ", message);
    connection.invoke("SendMessage", user, message).then(() => {
    console.log("Message sent successfully.");
}).catch(err => console.error("Send Message Error: ", err.toString()));
    document.getElementById("chat-text-input").value = "";
} else {
    console.log("Message is empty, not sending.");
}
}

    document.getElementById("send-chat-btn").addEventListener("click", () => {
    console.log("Send button clicked.");
    sendMessage();
});

    document.getElementById("chat-text-input").addEventListener("keypress", (event) => {
    if (event.key === "Enter") {
    console.log("Enter key pressed.");
    sendMessage();
    event.preventDefault();
}
});
