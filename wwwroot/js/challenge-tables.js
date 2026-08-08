let currentGame = null;

// Переключение игры

function showGame(id) {
    document.querySelectorAll(".game-table")
        .forEach(x => x.style.display = "none");

    const el = document.getElementById(id);

    if (el) {
        el.style.display = "block";
        currentGame = id;
    }

    updateUnproven();
}

// Неподтвержденные результаты

function updateUnproven() {
    const checkbox = document.getElementById("showUnproven");

    if (!checkbox)
        return;

    const showUnproven = checkbox.checked;

    document.querySelectorAll(".game-table").forEach(gameTable => {
        gameTable.querySelectorAll("tbody tr").forEach(row => {
            const players = row.querySelectorAll(".player-item");

            let visiblePlayers = 0;

            players.forEach(player => {
                const proven = player.dataset.proven === "true";

                if (proven || showUnproven) {
                    player.style.display = "inline";
                    visiblePlayers++;
                }
                else {
                    player.style.display = "none";
                }
            });


            // Пересчет количества игроков

            const countCell = row.querySelector(".players-count");

            if (countCell) {
                countCell.textContent = visiblePlayers;
            }

            // Прочерк если игроков нет

            const noPlayers = row.querySelector(".no-players");

            if (noPlayers) {
                noPlayers.style.display =
                    visiblePlayers === 0 ? "inline" : "none";
            }

            // А запятая где?

            const visiblePlayerItems =
                Array.from(players)
                    .filter(x => x.style.display !== "none");

            visiblePlayerItems.forEach((player, index) => {
                const comma = player.querySelector(".comma");

                if (comma) {
                    comma.style.display =
                        index < visiblePlayerItems.length - 1
                            ? "inline"
                            : "none";
                }
            });

        });
    });
}

document.getElementById("showUnproven")
    .addEventListener("change", function () {
        updateUnproven();
    });

// Загрузка страницы

window.addEventListener("load", function () {
    const first = document.querySelector(".game-table");

    if (first) {
        first.style.display = "block";
        currentGame = first.id;
    }

    updateUnproven();
});