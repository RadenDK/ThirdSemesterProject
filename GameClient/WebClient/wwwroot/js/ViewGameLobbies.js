$(document).ready(function () {
    $("#searchField").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $(".table-row").filter(function () {
            $(this).toggle($(this).find('.lobby-name').text().toLowerCase().startsWith(value));
        });
    });

    $(".table-row").click(function () {
        var lobbyId = $(this).data("lobby-id");

        // Check if the lobby is private
        if ($(this).find('.bi-lock-fill').length > 0) {
            // Show the password modal
            $("#passwordModal").show();
        } else {
            // If the lobby is not private, redirect immediately
            window.location.href = "http://localhost:5028/GameLobby/GameLobby?lobbyId=" + lobbyId;
        }
    });

    $("#submitPassword").click(function () {
        var lobbyId = $(".table-row").data("lobby-id");
        var password = $("#passwordInput").val();

        // Redirect to the GameLobby action in the GameLobbyController with the lobbyId and password
        var url = "http://localhost:5028/GameLobby/GameLobby?lobbyId=" + lobbyId;
        if (password !== null) {
            url += "&password=" + encodeURIComponent(password);
        }
        window.location.href = url;
    });


// Attach a click event listener to each table header
    $('th').click(function() {
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
        return function(a, b) {
            var valA = getCellValue(a, index);
            var valB = getCellValue(b, index);

            // If numeric values are being compared, parse them to float
            if($.isNumeric(valA) && $.isNumeric(valB)) {
                return parseFloat(valA) > parseFloat(valB) ? 1 : -1;
            } else {
                return valA.toString().localeCompare(valB);
            }
        }
    }

    function getCellValue(row, index) {
        return $(row).children('td').eq(index).text();
    }
}   );