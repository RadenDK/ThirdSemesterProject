document.addEventListener('DOMContentLoaded', function() {
    // Select the username and password fields
    var usernameField = document.querySelector('#username');
    var passwordField = document.querySelector('#password');

    // Select the submit button
    var submitButton = document.querySelector('button#submit-button');

    // Add event listeners for mousedown and mouseup events
    submitButton.addEventListener('mousedown', function() {
        this.classList.add('pressed');
    });

    submitButton.addEventListener('mouseup', function() {
        this.classList.remove('pressed');
    });

    // Function to check if the fields are empty
    function checkFields() {
        if (usernameField.value === '' || passwordField.value === '') {
            // If either field is empty, disable the button and change its color to grey
            submitButton.disabled = true;
            submitButton.style.backgroundColor = 'rgb(23, 23, 23);';

        } else {
            // If neither field is empty, enable the button and change its color back to the original color
            submitButton.disabled = false;
            submitButton.style.backgroundColor = '#007bff';
        }
    }

    // Add event listeners to the input fields that call the checkFields function when their values change
    usernameField.addEventListener('input', checkFields);
    passwordField.addEventListener('input', checkFields);

    // Call the checkFields function once when the page loads to set the initial state of the button
    checkFields();

    document.querySelector('form').addEventListener('submit', function(event) {
        event.preventDefault(); // Prevent the form from submitting

        // Check if the fields are empty
        if (usernameField.value === '') {
            // If the username field is empty, add the .error class to it
            usernameField.classList.add('error');
        } else {
            // If the username field is not empty, remove the .error class from it
            usernameField.classList.remove('error');
        }

        if (passwordField.value === '') {
            // If the password field is empty, add the .error class to it
            passwordField.classList.add('error');
        } else {
            // If the password field is not empty, remove the .error class from it
            passwordField.classList.remove('error');
        }
    });
});