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
    connection.invoke("SendName", username);
});

connection.on("ReceiveMessage", function (sender, message) {

    decodeGameInfo(message);
    var usr=getclientuser();
    $("#hand1").html(usr.firstcard);
    $("#hand2").html(usr.sencondcard);
    $("#flop1").html(game.flop1);
    $("#flop2").html(game.flop2);
    $("#flop3").html(game.flop3);
    $("#turn").html(game.turn);
    $("#river").html(game.river);

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

    $("#pool").html(game.pool);
    var clientName = $("#namefield").html();
    if (clientName == game.currentuser) {
        enableUserPanel(game.minbid, game.maxbid);
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
    if (message != "") {
        money = money - message;
    }
    $("#wallet").html(money);
    connection.invoke("SendMessage", username, message);
});

$("#passbutton").click(function () {
    if ($("#passbutton").hasClass("disabled")) {
        return;
    }
    var username = $("#namefield").html();
    connection.invoke("SendMessage", username, "pass");
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
    if ($("#valueinput").val() == 0 || $("#valueinput").val() == null) {
        $("#gobutton").html("Czekam");
    }
    else {
        if ($("#valueinput").attr("min") > 0) {
            $("#gobutton").html("Wchodzę");
        }
        $("#gobutton").html("Przebijam");
    }
});

var userlist=[];
var game;
function decodeGameInfo(fullinfo) {
    userlist = [];
    var usersInfo = fullinfo.split(":")[0].split(";");
    var gameInfo = fullinfo.split(":")[1];
    var i;
    for (i = 0; i < usersInfo.length; i++) {
        var newuser = new User(usersInfo[i]);
        userlist.push(newuser);
    }
    game = new Game(gameInfo);
};

function getclientuser() {
    var username = $("#namefield").html();
    var i;
    for (i = 0; i < userlist.length; i++) {
        if (userlist[i].name == username) {
            return userlist[i];
        }
    }
};

class User {
    constructor(userInfo) {

        var splittedInfo = userInfo.split("^");
        this.name = splittedInfo[0];
        this.wallet = splittedInfo[1];
        this.action = splittedInfo[2];
        this.active = splittedInfo[3];
        this.firstcard = splittedInfo[4];
        this.sencondcard = splittedInfo[5];
    }
}

class Game {
    constructor(gameInfo) {
        var splittedInfo = gameInfo.split("^");
        this.currentuser = splittedInfo[0];
        this.minbid = splittedInfo[1];
        this.maxbid = splittedInfo[2];
        this.pool = splittedInfo[3];
        this.recivedmessage = splittedInfo[4];
        this.flop1 = splittedInfo[5];
        this.flop2 = splittedInfo[6];
        this.flop3 = splittedInfo[7];
        this.turn = splittedInfo[8];
        this.river = splittedInfo[9];
    }
}