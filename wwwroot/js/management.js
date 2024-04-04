const uri = '/User';
let users = [];
//URL parameters from current window location.
const urlParams = new URLSearchParams(window.location.search);

//Token retrieved from localStorage.
const token = localStorage.getItem('token');

//Function to fetch all users data from server.
function GetUsers() {
    fetch('/allUsers', {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': token
        },
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Access denied. Please check your permissions.');
            }
            return response.json();
        })
        .then(data => DisplayUsers(data))
        .catch(error => alert('Unable to get users.', error));
}

// Call getUsers function to fetch user data on page load.
GetUsers();

//Function to add a new user to the database.
function AddUser() {
    const addNameTextbox = document.getElementById('add-name');
    const addPasswordTextBox = document.getElementById('add-password');
    const addStatusTextBox = document.getElementById('userType');
    const user = {
        password: addPasswordTextBox.value.trim(),
        username: addNameTextbox.value.trim(),
        status: addStatusTextBox.value.trim()=="Admin"?0:1
    };
    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': token
        },
        body: JSON.stringify(user)
    })
        .then(response => response.json())
        .then(() => {
            GetUsers();
            addNameTextbox.value = '';
            addPasswordTextBox.value = '';
        })
        .catch(error => alert('Unable to add user.', error));
}

// Function to delete user from the database.
function DeleteUser(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': token
        },
    })
        .then(() => GetUsers())
        .catch(error => alert('Unable to delete user.', error));
}

//Function to display count of user on the page.
function DisplayCount(userCount) {
    const name = (userCount === 1) ? 'user' : 'users';
    document.getElementById('counter').innerText = `${userCount} ${name}`;
}

// Function to display user users on the page.
function DisplayUsers(users_list) {
    const tBody = document.getElementById('users');
    tBody.innerHTML = '';
    DisplayCount(users_list.length);
    const button = document.createElement('button');
    users_list.forEach(user => {
        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `DeleteUser(${user.id})`);
        let tr = tBody.insertRow();
        let td1 = tr.insertCell(0);
        let userIdTextNode = document.createTextNode(user.id);
        td1.appendChild(userIdTextNode);
        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(user.username);
        td2.appendChild(textNode);
        let td3 = tr.insertCell(2);
        let passwordTextNode = document.createTextNode(user.password);
        td3.appendChild(passwordTextNode);
        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    users = users_list;
}
