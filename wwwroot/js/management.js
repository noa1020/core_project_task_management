const uri = '/User';
let users = [];
const urlParams = new URLSearchParams(window.location.search);
const token = localStorage.getItem('token');

function getItems() {
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
    .then(data => _displayItems(data))
    .catch(error => console.error('Unable to get items.', error));
}
getItems();

function addItem() {
    const addNameTextbox = document.getElementById('add-name');
    const addPasswordTextBox = document.getElementById('add-password');

    const item = {
        password: addPasswordTextBox.value.trim(),
        username: addNameTextbox.value.trim()
    };

    fetch(uri, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': token
            },
            body: JSON.stringify(item)
        })
        .then(response => response.json())
        .then(() => {
            getItems();
            addNameTextbox.value = '';
            addPasswordTextBox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
            method: 'DELETE',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': token
            },
        })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}


function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'user' : 'users';
    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('users');
    tBody.innerHTML = '';
    _displayCount(data.length);
    const button = document.createElement('button');
    data.forEach(item => {    
        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);
        let tr = tBody.insertRow();
        let td1 = tr.insertCell(0);
        let userIdTextNode = document.createTextNode(item.id);
        td1.appendChild(userIdTextNode);
        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(item.username);
        td2.appendChild(textNode);
        let td3 = tr.insertCell(2);
        let passwordTextNode = document.createTextNode(item.password);
        td3.appendChild(passwordTextNode);
        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    users = data;
}