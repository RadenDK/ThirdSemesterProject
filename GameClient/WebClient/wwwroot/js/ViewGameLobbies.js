$(document).ready(function () {
    $("#searchField").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $(".table-row").filter(function () {
            $(this).toggle($(this).find('.lobby-name').text().toLowerCase().startsWith(value));
        });
    });

    $(".table-row").click(function () {
        var lobbyId = $(this).data("lobby-id");
        // Redirect to the GameLobby action in the GameLobbyController with the lobbyId
        window.location.href = "http://localhost:5028/GameLobby/GameLobby?lobbyId=" + lobbyId;
    });
});
