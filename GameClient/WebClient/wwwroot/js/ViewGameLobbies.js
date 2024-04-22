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

$(document).ready(function() {
    // Attach click event handlers
    $('#lockHeader').click(function() {
        sortTable(0);
    });
    $('#lobbyNameHeader').click(function() {
        sortTable(1);
    });
    $('#ownerHeader').click(function() {
        sortTable(2);
    });
    $('#playersHeader').click(function() {
        sortTable(3);
    });

    var lastSortedColumn = -1;
    var lastSortOrderAsc = true;

    // Function to sort table
    function sortTable(columnIndex) {
        var table = $('.table');
        var rows = table.find('tr:gt(0)').toArray();

        // Determine sort order (asc or desc)
        var sortOrderAsc = lastSortedColumn !== columnIndex || !lastSortOrderAsc;
        lastSortedColumn = columnIndex;
        lastSortOrderAsc = sortOrderAsc;

        // Sort rows array
        rows.sort(function(a, b) {
            var A, B;
            if (columnIndex === 0) { // lock column
                A = $(a).children('td').eq(columnIndex).has('i').length;
                B = $(b).children('td').eq(columnIndex).has('i').length;
            } else if (columnIndex === 3) { // players column
                A = Number($(a).children('td').eq(columnIndex).text().split('/')[0]);
                B = Number($(b).children('td').eq(columnIndex).text().split('/')[0]);
            } else { // lobby name and owner columns
                A = $(a).children('td').eq(columnIndex).text().toUpperCase();
                B = $(b).children('td').eq(columnIndex).text().toUpperCase();
            }

            if (A < B) {
                return sortOrderAsc ? -1 : 1;
            }
            if (A > B) {
                return sortOrderAsc ? 1 : -1;
            }
            return 0;
        });

        // Remove current rows (except header)
        table.find('tr:gt(0)').remove();

        // Add sorted rows
        $.each(rows, function(index, row) {
            table.append(row);
        });
    }
});