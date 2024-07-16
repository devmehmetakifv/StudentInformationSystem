"use strict";
console.log('OO');

var connection = new signalR.HubConnectionBuilder().withUrl("/ChatHub").build();

document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, department, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `Broadcast from ${user}[${department}]: ${message}`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userNameInput").value;
    var department = document.getElementById("departmentInput").value;
    var message = document.getElementById("AnnouncementTitle").value;
    connection.invoke("SendMessage", user, department, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});