"use strict";

var adminName = $("#adminname").html();
var clientName = $("#namefield").html();

if (adminName != clientName) {
    disbleUserPanel();
}
else {
    preparePanelForAdmin();
}

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.start().then(function () {
    var username = $("#namefield").html();
    connection.invoke("SendName",username);
});

connection.on("ReceiveMessage", function (sender, message, currentUser, minval, maxval) {

    var indexes = document.getElementById("actions").children.length;
    var i;
    var index = -1;
    for (i = 0; i < indexes; i++) {
        if (document.getElementById("users").children[i].innerHTML == sender) {
            index = i;
        }
    }
    if (index > -1) {
        document.getElementById("actions").children[index].innerHTML = message;
    }

    var clientName = $("#namefield").html();
    if (clientName == currentUser) {
        enableUserPanel(minval,maxval);
    }
    else {
        disbleUserPanel();
    }
});

connection.on("ReceiveName", function (newUserName, userList) {
    var row = document.getElementById("users");
    var clientName = $("#namefield").html();
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

$("#gobutton").click(function () {
    if ($("#gobutton").hasClass("disabled")) {
        return;
    }
    var message = $("#valueinput").val();
    var username = $("#namefield").html();
    var money = $("#wallet").html();
    var pool = $("#pool").html();
    if (message != "") {
        money = money - message;
        pool = pool + message;
    }
    $("#pool").html(pool);
    $("#wallet").html(money);
    connection.invoke("SendMessage", username, message);
});

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
    $("#info").html("Wait for your turn");
    $("#gobutton").html("Czekam");
    $("#info").removeClass("text-success");
    $("#info").addClass("text-secondary");
    $("#valueinput").val("");


};

function enableUserPanel(minval, maxval, askPrevious) {

    $("#passbutton").removeClass("disabled");
    $("#valueinput").prop("disabled", false);
    $("#gobutton").removeClass("disabled");
    $("#info").html("Your move");
    $("#gobutton").html("Wait");
    $("#valueinput").val(minval);
    $("#info").removeClass("text-secondary");
    $("#info").addClass("text-success");
    $("#valueinput").prop("min", minval);
    $("#valueinput").prop("max", maxval);
    if ($("#valueinput").attr("min") > 0) {
        $("#gobutton").html("Wchodzę");
    }
};

function preparePanelForAdmin() {
    $("#passbutton").addClass("disabled");
    $("#valueinput").prop("disabled", true);
    $("#gobutton").html("Start");
    $("#info").html("Wait for users and start the game");
}

$("#valueinput").change(function () {
    if ($("#valueinput").val() == 0 || $("#valueinput").val() == null ) {
        $("#gobutton").html("Czekam");
    }
    else {
        if ($("#valueinput").attr("min")>0) {
            $("#gobutton").html("Wchodzę");
        }
        $("#gobutton").html("Przebijam");
    }
});