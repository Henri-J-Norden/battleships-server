"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

var guid = getGuid();

var cookies = document.cookie.split(";");
for (var i = 0; i < cookies.length; i++) {
    var cookie = cookies[i].trim().split("=");
    if (cookie[0] === "guid") {
        guid = cookie[1];
    }
}

if (guid === "") console.log("ERROR: cookie 'guid' not found!");

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + ": " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    var msglist = document.getElementById("messagesList");
    msglist.insertBefore(li, msglist.firstChild);
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", guid, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});