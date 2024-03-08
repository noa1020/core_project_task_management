function connectWithGoogle() {
    window.location.href = "https://accounts.google.com";
}

const postmanButton = document.getElementById("postmanButton");

postmanButton.addEventListener("click", function() {
    window.open('postman://app', '_blank');
});

function login() {
    const id = document.getElementById("signInID").value;
    const password = document.getElementById("signInPassword").value;
    
    fetch('todoList/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ id: id, Password: password }) 
    })
    .then(response => {
        alert(1);
        if (response.ok) {
            return response.json(); 
        } else if (response.status === 401) {
            console.log('Unauthorized - Invalid credentials');
        } else {
            console.log('Error occurred while logging in');
        }
    })
    .then(data => {
        // Handle the token received after successful login
        console.log('Authentication token: ', data.token);
        window.location.href = "http://localhost:5130/tasks.html";
    })
    .catch(error => {
        console.error('Error during login:', error);
    });
}
