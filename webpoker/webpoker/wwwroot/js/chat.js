var adminName = $("#adminname").html();
var clientName = $("#namefield").html();
resetView();

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.start().then(function () {
    var username = $("#namefield").html();
    connection.invoke("SendName", username);
});

connection.on("ReceiveMessage", function (sender, message) {
    decodeMessage(message);
    if(game!=null)
        adminName = game.admin;
    clearUserPanels();
    createUserPanel();
    $("#wallet").html(getclientuser().wallet);

    if (game != null) {
        $("#pool").html(game.pool);


        if (game.finish == "True") {
            resetView();
            return;
        }

        showCards();


        var clientName = $("#namefield").html();
        if (clientName == game.currentuser) {
            enableUserPanel(game.minbid, game.maxbid);
        }
        else {
            disbleUserPanel();
        }
    }

});

$("#gobutton").click(function () {
    if ($("#gobutton").hasClass("disabled")) {
        return;
    }
    var message = $("#valueinput").val();
    var username = $("#namefield").html();

    connection.invoke("SendMessage", username, message,true);
});

$("#passbutton").click(pass());

function pass() {
    if ($("#passbutton").hasClass("disabled")) {
        return;
    }
    var username = $("#namefield").html();
    connection.invoke("SendMessage", username, "pass", true);
}

function automaticPass() {
    var username = $("#namefield").html();
    connection.invoke("SendMessage", username, "pass", true);
}

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

function enableUserPanel(minval, maxval) {

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
    if ($("#valueinput").attr("min") == 0) {
        $("#passbutton").addClass("disabled");
    }
    setTimeout(automaticPass, 15000);
};

function preparePanelForAdmin() {
    $("#passbutton").addClass("disabled");
    $("#valueinput").prop("disabled", true);
    $("#gobutton").html("Start");
    $("#info").html("Wait for users and start the game");
    $("#valueinput").val("");
    $("#gobutton").removeClass("disabled");
};

function showCards() {
    var usr = getclientuser();
    $("#hand1").html(usr.firstcard);
    $("#hand2").html(usr.sencondcard);
    $("#flop1").html(game.flop1);
    $("#flop2").html(game.flop2);
    $("#flop3").html(game.flop3);
    $("#turn").html(game.turn);
    $("#river").html(game.river);
    colorCards(document.getElementById("handcards"));
    colorCards(document.getElementById("tablecards"));
}

function colorCards(element) {
    var i;
    for (i = 0; i < element.children.length; i++) {
        var text = element.children[i].innerHTML;
        if (text.includes("♦") || text.includes("♥")) {
            element.children[i].style.color = "red";
        }
        else {
            element.children[i].style.color = "black";
        }
    }
}

function createUserPanel() {
    var row = document.getElementById("userpanels");
    var i;
    for (i = 0; i < userlist.length; i++) {

        var user = userlist[i];
        var userColumn = document.createElement("div");
        userColumn.className = "col - 2 border text - center";

        var nameRow = document.createElement("div");
        nameRow.innerHTML = user.name;
        userColumn.appendChild(nameRow);

        var actionRow = document.createElement("div");
        actionRow.innerHTML = user.action;
        userColumn.appendChild(actionRow);

        row.appendChild(userColumn);

        if (user.active == false) {
            //userColumn.classList.add("text-secondary");
        }
        if (game != null) {
            if (game.currentuser == user.name) {
                //userColumn.classList.add("text - primary");
            }
        }
    }
};

function clearUserPanels() {
    $("#userpanels").empty();
}


$("#valueinput").change(function () {
    if ($("#valueinput").val() == 0 || $("#valueinput").val() == null) {
        $("#gobutton").html("Czekam");
    }
    else {
        if ($("#valueinput").attr("min") > 0) {
            $("#gobutton").html("Wchodzę");
        }
        if ($("#valueinput").attr("min") == 0) {
            $("#passbutton").addClass("disabled");
        }
        $("#gobutton").html("Przebijam");
    }
});

var userlist=[];
var game;
function decodeMessage(fullinfo) {
    userlist = [];
    game = null;
    var usersInfo = fullinfo.split(":")[0].split(";");
    var gameInfo = fullinfo.split(":")[1];

    decodeUsersInfo(usersInfo);
    decodeGameinfo(gameInfo);
};

function decodeUsersInfo(usersInfo) {
    var i;
    for (i = 0; i < usersInfo.length; i++) {
        var newuser = new User(usersInfo[i]);
        userlist.push(newuser);
    }
};

function decodeGameinfo(gameInfo) {
    if (gameInfo != "") {
        game = new Game(gameInfo);
    }
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

function resetView() {
    if (adminName != clientName) {
        disbleUserPanel();
    }
    else {
        preparePanelForAdmin();
    }
}

window.onbeforeunload = function () {
    return '';
};

window.onunload = function () { //odchodzenie przed rozpoczeciem gry
    var username = $("#namefield").html();
    connection.invoke("SendMessage", username, "pass", false);
    return '';
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
        this.flop1 = splittedInfo[4];
        this.flop2 = splittedInfo[5];
        this.flop3 = splittedInfo[6];
        this.turn = splittedInfo[7];
        this.river = splittedInfo[8];
        this.finish = splittedInfo[9];
        this.admin = splittedInfo[10];
    }
}
