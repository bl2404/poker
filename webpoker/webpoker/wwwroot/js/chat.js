"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
connection.start();

connection.on("ReceiveMessage", function (user, message) {
    var indexes = document.getElementById("actions").children.length;
    var i;
    var index = -1;
    for (i = 0; i < indexes; i++) {
        if (document.getElementById("users").children[i].innerHTML == user) {
            index = i;
        }
    }
    if (index > -1) {
        document.getElementById("actions").children[index].innerHTML = message;
    }
});

document.getElementById("gobutton").addEventListener("click", function (event) {
    var message = document.getElementById("valueinput").value;
    var username = document.getElementById("namefield").innerHTML;
    connection.invoke("SendMessage", username, message);
    event.preventDefault();
});