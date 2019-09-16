"use strict";


var guid = getGuid();
if (guid === "") window.location.replace("/Login");

var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();


async function connect() {
    while (connection.state === 0) {
        await sleep(10);
    }

    connection.invoke("Start", guid).catch(function (err) {
        return console.log(err);
    });
    console.log("Starting game...");
}




function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

connection.on("Update", function (msg) {
    //console.log("UPDATE: " + msg);
    
    var board = document.getElementById("board");
    board.innerHTML = msg;
});

connection.on("Log", function (message) {
    var e = document.createElement("p");

    e.innerText = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");;

    document.getElementById("log").appendChild(e);


});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

connect();



document.addEventListener("keydown", function (e) {
    e.preventDefault();
    e.stopPropagation();
    //console.log("Pressed " + e.keyCode);

    connection.invoke("KeyPress", guid, e.keyCode).catch(function (err) {
        return console.error(err.toString());
    });
    
});


/*
document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", guid, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
*/