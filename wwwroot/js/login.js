// Function to check if a user is already logged in and redirect to tasks page.
function CheckUser() {
    const token = localStorage.getItem('token');
    if (token) {
        const decodedToken = JSON.parse(atob(token.split('.')[1]));
        const currentTime = Math.floor(Date.now() / 1000);
        if (decodedToken.exp > currentTime)
            window.location.href = "./html/tasks.html";
    }
}

// Check if user is already logged in on page load.
CheckUser();

//Function to handle authentication token received from login.
function HandleAuthToken(token) {
    if (typeof token === 'object') {
        alert("Unexpected response. Please try again.");
    } else {
        localStorage.setItem('token', "Bearer " + token);
        window.location.href = "./html/tasks.html";
    }
}

// Function to handle user login by sending credentials to server.
function Login(name, password) {
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
    }).then(response => HandleAuthToken(response))
        .catch(error => {
            alert('Error during login: ' + error);
        });
}

//save name and password after login and send to login function
saveDetails = () => {
    const password = document.getElementById('signInPassword').value;
    const name = document.getElementById('signInName').value;
    Login(name, password)
}

//handle the google button
function handleCredentialResponse(response) {
    if (response.credential) {
        var idToken = response.credential;
        var decodedToken = parseJwt(idToken);
        var userId = decodedToken.sub; // User ID
        var userName = decodedToken.name; // User Name

        Login(userName, userId);
    } else {
        console.error('Google Sign-In was cancelled.');
    }
}

function parseJwt(token) {
    try {
        var base64Url = token.split('.')[1];
        var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        var jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));

        return JSON.parse(jsonPayload);
    } catch (error) {
        console.error('Error parsing JWT token:', error);
        return null;
    }
}
