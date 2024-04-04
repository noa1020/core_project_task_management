const uri = '/todoList';
let tasks = [];
const token = localStorage.getItem('token');

//Fetches tasks from the server
function GetTasks() {
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
    }).then(data => DisplayTasks(data))
        .then(IsManager())
        .then(LoadingUserDetails())
        .catch(error => alert('Unable to get tasks.' + error));
}

// Initial call to fetch tasks
GetTasks();

//Fetch current user from the server
function LoadingUserDetails() {
    fetch('/myUser', {
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
    }).then(data => DisplayUser(data))
        .catch(error => alert('Unable to get user information.' + error));
}

//Adds a new task to the todo list
function AddTask() {
    const addNameTextbox = document.getElementById('add-name');
    const task = {
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
        body: JSON.stringify(task)
    }).then(response => response.json())
        .then(() => {
            GetTasks();
            addNameTextbox.value = '';
        }).catch(error => alert('Unable to add task.' + error));
}

// Deletes task from the todo list
function DeleteTask(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': token
        },
    }).then(() => GetTasks())
        .catch(error => alert('Unable to delete task.' + error));
}

//Displays an edit form for a specific task
function DisplayEditForm(id) {
    const task = tasks.find(task => task.id === id);
    document.getElementById('edit-name').value = task.name;
    document.getElementById('edit-id').value = task.id;
    document.getElementById('edit-isDone').checked = task.isDone == true;
    document.getElementById('editForm').style.display = 'block';
}

//Updates an task by sending a PUT request to the server.
function UpdateTask() {
    const taskId = document.getElementById('edit-id').value;
    const task = {
        id: parseInt(taskId, 10),
        isDone: document.getElementById('edit-isDone').checked,
        name: document.getElementById('edit-name').value.trim()
    };
    fetch(`${uri}/${taskId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': token
        },
        body: JSON.stringify(task)
    })
        .then(() => GetTasks())
        .catch(error => alert('Unable to update task.' + error));
    CloseInput();
    return false;
}

//Hides the edit form.
function CloseInput() {
    document.getElementById('editForm').style.display = 'none';
}

//Displays the count of tasks in the list.
function DisplayCount(taskCount) {
    const name = (taskCount === 1) ? 'task' : 'task kinds';
    document.getElementById('counter').innerText = `${taskCount} ${name}`;
}

/// Displays the taska in the list.
function DisplayTasks(tasks_list) {
    const tBody = document.getElementById('tasks');
    tBody.innerHTML = '';
    DisplayCount(tasks_list.length);
    const button = document.createElement('button');
    tasks_list.forEach(task => {
        let isDoneCheckbox = document.createElement('input');
        isDoneCheckbox.type = 'checkbox';
        isDoneCheckbox.disabled = true;
        isDoneCheckbox.checked = task.isDone;
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.addEventListener('click', () => {
            DisplayEditForm(task.id);
        });
        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `DeleteTask(${task.id})`);
        let tr = tBody.insertRow();
        let td1 = tr.insertCell(0);
        td1.appendChild(isDoneCheckbox);
        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(task.name);
        td2.appendChild(textNode);
        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);
        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });
    tasks = tasks_list;
}

//Function to check if the user is a manager and display management button if user id is '1'.
function IsManager() {
    const tokenParts = token.split('.');
    const type = JSON.parse(atob(tokenParts[1])).type;
    if (type === 'Admin') {
        document.getElementById('managementButton').style.display = 'block';
    }
}

//Function to display user details in the update form.
function DisplayUser(user) {
    updateName.value = user.username;
    updatePassword.value = user.password;
}

// Event listener to display edit details popup on button click.
document.getElementById('editDetailsButton').addEventListener('click', function () {
    document.getElementById('editDetailsPopup').style.display = 'block';
});

// Function to close the edit details popup.
function ClosePopup() {
    document.getElementById('editDetailsPopup').style.display = 'none';
}

// Function to save user details by sending PUT request to server.
function UpdateUser() {
    const user = {
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
        body: JSON.stringify(user)
    }).then(() => {
            DisplayUser(user);
        }).catch(error => alert('Unable to change user details.' + error));
    ClosePopup();
}
