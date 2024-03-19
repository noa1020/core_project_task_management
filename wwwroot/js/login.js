function connectWithGoogle() {
    window.location.href = "https://accounts.google.com";
}

const postmanButton = document.getElementById("postmanButton");

postmanButton.addEventListener("click", function () {
    window.open('postman://app', '_blank');
});

function handleAuthToken(token) {
    if (typeof token === 'object') {
        alert("Unexpected response. Please try again.");
    } else {
        localStorage.setItem('token', "Bearer " + token);
        window.location.href = "./html/tasks.html";
    }
}

function login() {
    const name = document.getElementById("signInName").value.trim()
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

function checkUser() {
    if (localStorage.getItem("token")) {
        window.location.href = "./html/tasks.html";
    }
}
checkUser();