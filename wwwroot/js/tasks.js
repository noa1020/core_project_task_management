const uri = '/todoList';
let tasks = [];
const urlParams = new URLSearchParams(window.location.search);
const token = localStorage.getItem('token');

//Fetches items from the server
function getItems() {
    fetch(uri, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': token
        },
    }).then(response => {
        if (!response.ok) {
            throw new Error('Access denied. Please check your permissions.');
        }
        return response.json();
    }).then(data => _displayItems(data))
        .then(isManager())
        .catch(error => console.error('Unable to get items.', error));
}
// Initial call to fetch items
getItems();

function LoadingUserDetails(){
    fetch('/myUser',{
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': token
        },
    }).then(response => {
        if (!response.ok) {
            throw new Error('Problem retrieving user information.');
        }
        return response.json();
    }).then(data => _displayUser(data))
        .catch(error => console.error('Unable to get user information.', error));


}
LoadingUserDetails()

//Adds a new item to the todo list
function addItem() {
    const addNameTextbox = document.getElementById('add-name');
    const item = {
        isDone: false,
        name: addNameTextbox.value.trim()
    };
    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': token
        },
        body: JSON.stringify(item)
    }).then(response => response.json())
        .then(() => {
            getItems();
            addNameTextbox.value = '';
        }).catch(error => console.error('Unable to add item.', error));
}

/** Deletes an item from the todo list
 * @param {string} id - The id of the item to delete
 */
function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': token
        },
    }).then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

/** Displays an edit form for a specific item
 * @param {string} id - The id of the item to edit
 */
function displayEditForm(id) {
    const item = tasks.find(item => item.id === id);
    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-isDone').checked = item.isDone==true;
    document.getElementById('editForm').style.display = 'block';
}

/** Updates an item by sending a PUT request to the server.
 * @returns {boolean} Returns false to prevent default form submission.
 */
function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        isDone: document.getElementById('edit-isDone').checked,
        name: document.getElementById('edit-name').value.trim()
    };
    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': token
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));
    closeInput();
    return false;
}

//Hides the edit form.
function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

/** Displays the count of items in the list.
 * @param {number} itemCount - The number of items in the list.
 */
function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'task' : 'task kinds';
    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

/** Displays the items in the list.
 * @param {Array} data - The array of items to display.
 */
function _displayItems(data) {
    const tBody = document.getElementById('tasks');
    tBody.innerHTML = '';
    _displayCount(data.length);
    const button = document.createElement('button');
    data.forEach(item => {
        let isDoneCheckbox = document.createElement('input');
        isDoneCheckbox.type = 'checkbox';
        isDoneCheckbox.disabled = true;
        isDoneCheckbox.checked = item.isDone;
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.addEventListener('click', () => {
            displayEditForm(item.id);
        });
        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);
        let tr = tBody.insertRow();
        let td1 = tr.insertCell(0);
        td1.appendChild(isDoneCheckbox);
        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(item.name);
        td2.appendChild(textNode);
        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);
        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });
    tasks = data;
}

function isManager() {
    const tokenParts = token.split('.');
    const userId = JSON.parse(atob(tokenParts[1])).id;
    if (userId === '1')
        document.getElementById('managementButton').style.display = 'block';
}

const updateName=document.getElementById("updateName");
const updatePassword=document.getElementById("updatePassword");
function _displayUser(user){
    updateName.value=user.username;
    updatePassword.value=user.password;

}
document.getElementById('editDetailsButton').addEventListener('click', function() {
    document.getElementById('editDetailsPopup').style.display = 'block';
});

function closePopup() {
    document.getElementById('editDetailsPopup').style.display = 'none';
}

function saveDetails() {
    const item = {
        password: updatePassword.value.trim(),
        username: updateName.value.trim()
    };
    fetch('/User', {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': token
        },
        body: JSON.stringify(item)
    }).then(response => response.json())
        .then(() => {
            _displayUser();
        }).catch(error => console.error('Unable to change user details.', error));
    closePopup();
}
