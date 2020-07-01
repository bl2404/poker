"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.start();

document.getElementById("gobutton").addEventListener("click", function (event) {
    var message = document.getElementById("valueinput").value.toString();
    var username = document.getElementById("namefield").innerHTML;
    connection.invoke("SendMessage", username, message);
    event.preventDefault();
});