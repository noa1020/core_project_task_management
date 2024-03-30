/**
 * Function to redirect user to Google sign-in page.
 */
function connectWithGoogle() {
    window.location.href = "https://accounts.google.com";
}

/**
 * Function to handle authentication token received from login.
 * @param {string|Object} token - Authentication token received from server.
 */
function handleAuthToken(token) {
    if (typeof token === 'object') {
        alert("Unexpected response. Please try again.");
    } else {
        localStorage.setItem('token', "Bearer " + token);
        window.location.href = "./html/tasks.html";
    }
}

/**
 * Function to handle user login by sending credentials to server.
 */
function login() {
    const name = document.getElementById("signInName").value.trim();
    const password = document.getElementById("signInPassword").value.trim();
    fetch(`/login?name=${name}&password=${password}`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
    }).then(response => {
        if (response.ok) {
            return response.json();
        } else if (response.status === 401) {
            alert('Unauthorized - Invalid credentials');
        } else {
            alert('Error occurred while logging in');
        }
    }).then(response => handleAuthToken(response))
        .catch(error => {
            alert('Error during login:', error);
        });
}

/**
 * Function to check if a user is already logged in and redirect to tasks page.
 */
function checkUser() {
    if (localStorage.getItem("token")) {
        window.location.href = "./html/tasks.html";
    }
}
//open function with the api call to admin token
function openPostman() {
    window.location.href = 'https://crimson-capsule-239063.postman.co/workspace/New-Team-Workspace~19cbe971-1eae-455d-b8b6-2cf42d9e21c0/request/29786284-690d505b-1b2f-4fab-8aec-3f04e64978ac?ctx=documentation';
}


// Check if user is already logged in on page load.
checkUser();
