function connectWithGoogle() {
    window.location.href = "https://accounts.google.com";
}

const postmanButton = document.getElementById("postmanButton");

postmanButton.addEventListener("click", function() {
    window.open('postman://app', '_blank');
});

function login() {
    const id = document.getElementById("signInID").value.trim()
    const password = document.getElementById("signInPassword").value.trim();
    const uri = '/login';

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
            },
        body: JSON.stringify({ id: id, password:password})
    })
    .then(response => {
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
        alert('Authentication token: ', data.token);
        window.location.href = "https://localhost:5130/tasks.html";
    })
    .catch(error => {
        console.error('Error during login:', error);
    });
}
