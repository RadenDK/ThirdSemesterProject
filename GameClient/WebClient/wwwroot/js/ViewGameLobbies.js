$(document).ready(function () {
    $("#search-lobby-field").on("input", function () {
        var value = $(this).val().toLowerCase();
        var hasResults = false;
        $(".table-row").filter(function () {
            var nameMatch = $(this).find('.lobby-name').text().toLowerCase().startsWith(value);
            var ownerMatch = $(this).find('.owner').text().toLowerCase().startsWith(value);
            var match = nameMatch || ownerMatch;
            $(this).toggle(match);
            if (match) {
                hasResults = true;
            }
        });
        if (hasResults) {
            $("#no-results").hide();
        } else {
            $("#no-results").show();
        }
    })
});

    $(".table-row").click(function (event) {
        var lobbyId = $(this).data("lobby-id");

        // Check if the lobby is private
        if ($(this).find('.bi-lock-fill').length > 0) {
            // Show the password modal
            $("#passwordModal").show();
        } else {
            // If the lobby is not private, send a POST request with a null password
            var requestBody = {
                gameLobbyId: lobbyId,
                lobbyPassword: null
            };

            fetch("https://localhost:7292/GameLobby/GameLobby", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(requestBody)
            }).then(response => {
                if (response.ok) {
                    // Parse the response body as text
                    response.text().then(html => {
                        // Insert the HTML into your page
                        document.body.innerHTML = html;
                    });
                } else {
                    // Handle errors
                    console.error("Error:", response);
                }
            });
        }

        // Prevent the default action
        event.preventDefault();
    });


    $("#submitPassword").click(function (event) {
        var lobbyId = $(".table-row").data("lobby-id");
        var password = $("#passwordInput").val();

        // Create the request body
        var requestBody = {
            gameLobbyId: lobbyId,
            lobbyPassword: password
        };

        // Send a POST request to the GameLobby action
        fetch("https://localhost:7292/GameLobby/GameLobby", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(requestBody)
        }).then(response => {
            if (response.ok) {
                // Parse the response body as text
                response.text().then(html => {
                    // Insert the HTML into your page
                    document.body.innerHTML = html;
                });
            } else {
                // Handle errors
                console.error("Error:", response);
            }
        });

        // Prevent the default action
        event.preventDefault();
    });


    // Attach a click event listener to each table header
    $('th').click(function () {
        var table = $(this).parents('table').eq(0);
        var rows = table.find('tr:gt(0)').toArray().sort(comparer($(this).index()));
        this.asc = !this.asc;

        // If this.asc is not true, reverse the array
        if (!this.asc) {
            rows = rows.reverse();
        }

        // Replace existing rows with sorted rows
        for (var i = 0; i < rows.length; i++) {
            table.append(rows[i]);
        }
    });

    function comparer(index) {
        return function (a, b) {
            var valA = getCellValue(a, index);
            var valB = getCellValue(b, index);

            // If numeric values are being compared, parse them to float
            if ($.isNumeric(valA) && $.isNumeric(valB)) {
                return parseFloat(valA) > parseFloat(valB) ? 1 : -1;
            } else {
                return valA.toString().localeCompare(valB);
            }
        }
    }

    function getCellValue(row, index) {
        return $(row).children('td').eq(index).text();

}


// Get the modal
var modal = document.querySelector('.modal');

// Get the close button
var closeButton = document.querySelector('.close-modal-button');

// When the user clicks on the close button, start the closing animation
closeButton.onclick = function() {
    modal.classList.add('modal-closing');

    // After the transition is complete, hide the modal
    setTimeout(function() {
        modal.style.display = "none";
        modal.classList.remove('modal-closing');
    }, 300); // Match this with the duration of your CSS transition
}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function(event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}