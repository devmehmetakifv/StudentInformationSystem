"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/ChatHub").build();

document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, department, message) {
    console.log(user, department, message);
    var targetNavbar = document.getElementById('mainNav'); // Use the correct ID or class selector

    // Create h3 element
    var h3 = document.createElement('h3');
    h3.className = 'form-control bg-light border-0 small fallDownAnimation'; // Add the animation class here
    h3.textContent = `Broadcast from ${user}[${department}]: ${message}`;

    // Append h3 to the target navbar
    targetNavbar.appendChild(h3);

    // Optional: Remove the h3 after some time
    setTimeout(() => {
        h3.remove();
    }, 5000); // Adjust time as needed
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