# Welcome to the Office Management System!

## System Description

The Task Manager system is a web application designed to facilitate task management within the context of users and administrators.
It provides a user-friendly interface for users to create, update, and delete their tasks, and edit their profile.
while administrators have additional privileges to manage users and their own tasks as well.
the system is built using ASP.NET MVC Web API technology and utilizes JSON Web Tokens (JWT) for authorization.

## Table of Contents
- [System Features](#system-features)
- [System Architecture](#system-architecture)
- [Data Protection](#data-protection)
- [Requirements](#requirements)
- [Installation Instructions](#installation-instructions)
- [Screenshots](#screenshots)
- [Contact Us](#contact-us)

## System Features

- **Management board:** Ability to add, and delete user/director in the tasks system.
- **User board:** Ability to update personal details.
- **Tasks Tracking:** Documentation and management of tasks processes for each member.
- **Google Account Integration:**  Users can log in using their Google accounts for enhanced convenience and security.
- **Logging:** All user actions and system operations are logged for auditing and security purposes.

## System Architecture

The application follows a client-server architecture, with a client-side application interacting with server-side APIs, which in turn interacts with json files which are used as database to store and retrieve information. Here's how the various components of the system interact:

### Client-Side Application:

- Developed in HTML, CSS, and JavaScript, the client-side application is responsible for presenting the user interface (UI) to the end-users.
- It communicates with the server-side APIs, to perform CRUD (Create, Read, Update, Delete) operations and retrieve data via HTTP requests (GET, POST, PUT, DELETE) over the network.

### Server-Side APIs:

- The server-side APIs, developed in .NET, act as an intermediary between the client-side application and the json files.
- They handle incoming HTTP requests from the client-side application and execute corresponding logic.

### Illustrative illustration:

<img src="https://github.com/noa1020/core_project_task_management/assets/146897162/442ef97b-0b4a-42e3-ab57-7a353a901e6c" alt="Architectural specification" width="600" style="border:1px solid black">

### Data Protection:

The system ensures data security and integrity by using JSON files to store both user information and tasks. the system implements token-based authentication to control access to resources.

## Requirements

- .NET Framework 4.7.2 or higher
- Web browser with support for HTML5 and CSS3

## Installation Instructions

1. Download the code files from GitHub.
2. Install the required development environment.
3. Start the server.
4. Launch the client on a web browser.
5. On the home page you can click on the Run with Postman button to see the sample readings.

## Screenshots

### Login page:
<img src="https://github.com/noa1020/core_project_task_management/assets/146897162/1a96beee-4718-4884-87bd-a1a647402836" alt="Homepage" width="900" style="border:1px solid black">

### Tasks page:
<img src="https://github.com/noa1020/core_project_task_management/assets/146897162/8aea647f-b792-4e50-9ffc-52114a87179d" alt="Homepage" width="900" style="border:1px solid black">

### Edit task:
<img src="https://github.com/noa1020/core_project_task_management/assets/146897162/2fa7b862-0b31-4431-81f1-56715f248ce8" alt="Homepage" width="900" style="border:1px solid black">

### Update personal details:
<img src="https://github.com/noa1020/core_project_task_management/assets/146897162/4124aadf-e748-4c57-abc5-c61fa0b78d98" alt="Homepage" width="900" style="border:1px solid black">

### Management page:
<img src="https://github.com/noa1020/core_project_task_management/assets/146897162/be4163d8-8227-4753-890c-a2b2f6e9b6f8" alt="Homepage" width="900" style="border:1px solid black">


## Contact Us

For any questions or further assistance, please contact us via email: Noa0533181648@gmail.com or LinkedIn: <a href="https://www.linkedin.com/in/noa-katsoer-72b7592a5/">Noa Katsoer</a>

