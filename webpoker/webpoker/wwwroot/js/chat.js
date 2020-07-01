"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
connection.start();

connection.on("ReceiveMessage", function (message) {
    document.getElementById("output").innerHTML = message;
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", message);
    event.preventDefault();
});