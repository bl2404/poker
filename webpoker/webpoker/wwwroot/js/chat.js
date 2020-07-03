"use strict";

var adminName = document.getElementById("adminname").innerHTML;
var clientName = document.getElementById("namefield").innerHTML;

if (adminName != clientName) {
    disbleUserPanel();
}
else {
    preparePanelForAdmin();
}


var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.start().then(function () {
    var username = document.getElementById("namefield").innerHTML;
    connection.invoke("SendName",username);
});

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

connection.on("ReceiveName", function (newUserName, userList) {
    var row = document.getElementById("users");
    var clientName = document.getElementById("namefield").innerHTML;
    var i;

    if (row.children.length == 0) {
        for (i = 0; i < userList.length; i++) {
            if (userList[i] != clientName) {
                createUser(userList[i]);
            }
        }
    }
    else {
        var alreadyExist = false;

        for (i = 0; i < row.children.length; i++) {
            if (row.children[i].innerHTML == newUserName) {
                alreadyExist = true;
                break;
            }
        }
        if (alreadyExist == false && newUserName != clientName) {
            createUser(newUserName);
        }
    }
});

connection.on("ReceiveStartSignal", function (currentUser) {
    var clientName = document.getElementById("namefield").innerHTML;
    if (clientName == currentUser) {
        enableUserPanel();
    }
    else {
        disbleUserPanel();
    }
});

document.getElementById("gobutton").onclick = function (event) {
    if ($("#gobutton").hasClass("disabled")) {
        return;
    }
    var message = document.getElementById("valueinput").value.toString();
    var username = document.getElementById("namefield").innerHTML;
    connection.invoke("SendMessage", username, message);
    if (document.getElementById("info").innerHTML == "Wait for users and start the game") {
        document.getElementById("info").innerHTML = "";
        document.getElementById("gobutton").innerHTML = "Go";
    }
    connection.invoke("SendStartSignal");
    event.preventDefault();
};

function createUser(name) {
    var row = document.getElementById("users");
    var actionRow = document.getElementById("actions");
    var div = document.createElement("div");
    div.className = "col";
    div.innerHTML = name;
    row.appendChild(div);

    var divAction = document.createElement("div");
    divAction.className = "col";
    actionRow.appendChild(divAction);
};

function disbleUserPanel() {

    $("#passbutton").addClass("disabled");
    $("#valueinput").prop("disabled", true);
    $("#gobutton").addClass("disabled");
    document.getElementById("info").innerHTML = "Wait for your turn";

};

function enableUserPanel() {

    $("#passbutton").removeClass("disabled");
    $("#valueinput").prop("disabled", false);
    $("#gobutton").removeClass("disabled");
    document.getElementById("info").innerHTML = "Your move";
    document.getElementById("gobutton").innerHTML = "Wait";
    document.getElementById("valueinput").value = 0;
};

function preparePanelForAdmin() {
    $("#passbutton").addClass("disabled");
    $("#valueinput").prop("disabled", true);
    document.getElementById("gobutton").innerHTML = "Start";
    document.getElementById("info").innerHTML = "Wait for users and start the game";
}

$("#valueinput").change(function () {

    if ($("#valueinput").val() == 0) {
        document.getElementById("gobutton").innerHTML = "Wait";
    }
    else {
        document.getElementById("gobutton").innerHTML = "Przebij";
    }
});