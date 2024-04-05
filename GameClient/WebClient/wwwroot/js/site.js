document.querySelector('form').addEventListener('submit', function(event) {
    event.preventDefault(); // Prevent the form from submitting

    // Select the username and password fields
    var usernameField = document.querySelector('#username');
    var passwordField = document.querySelector('#password');

    // Check if the fields are empty
    if (usernameField.value === '') {
        // If the username field is empty, add the .error class to it
        usernameField.classList.add('error');
    }

    if (passwordField.value === '') {
        // If the password field is empty, add the .error class to it
        passwordField.classList.add('error');
    }
});