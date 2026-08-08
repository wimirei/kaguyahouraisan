// ---------- Лучшие результаты ----------

const btnSurvival = document.getElementById("btnSurvival");
const btnScoring = document.getElementById("btnScoring");

const survivalTable = document.getElementById("survivalTable");
const scoringTable = document.getElementById("scoringTable");

btnSurvival.addEventListener("click", () => {

    survivalTable.style.display = "";
    scoringTable.style.display = "none";

    btnSurvival.classList.replace("btn-outline-primary", "btn-primary");
    btnScoring.classList.replace("btn-primary", "btn-outline-primary");

});

btnScoring.addEventListener("click", () => {

    survivalTable.style.display = "none";
    scoringTable.style.display = "";

    btnScoring.classList.replace("btn-outline-primary", "btn-primary");
    btnSurvival.classList.replace("btn-primary", "btn-outline-primary");

});


// ---------- Таблицы всех игр по шоттипам и сложностям ----------

let currentGame = "@TouhouGame.EoSD";
let currentCategory = "Survival";

const shotTables = document.querySelectorAll(".shot-table");
const gameButtons = document.querySelectorAll(".game-btn");

const btnShotSurvival = document.getElementById("btnShotSurvival");
const btnShotScoring = document.getElementById("btnShotScoring");

function updateShotTables() {

    shotTables.forEach(table => {

        if (table.dataset.game === currentGame &&
            table.dataset.category === currentCategory) {
            table.style.display = "";
        }
        else {
            table.style.display = "none";
        }

    });

}


// Переключение игры

gameButtons.forEach(button => {

    button.addEventListener("click", () => {

        currentGame = button.dataset.game;

        gameButtons.forEach(btn => {
            btn.classList.replace("btn-primary", "btn-outline-primary");
        });

        button.classList.replace("btn-outline-primary", "btn-primary");

        updateShotTables();

    });

});


// Сурв

btnShotSurvival.addEventListener("click", () => {

    currentCategory = "Survival";

    btnShotSurvival.classList.replace("btn-outline-primary", "btn-primary");
    btnShotScoring.classList.replace("btn-primary", "btn-outline-primary");

    updateShotTables();

});


// Скоринг

btnShotScoring.addEventListener("click", () => {

    currentCategory = "Scoring";

    btnShotScoring.classList.replace("btn-outline-primary", "btn-primary");
    btnShotSurvival.classList.replace("btn-primary", "btn-outline-primary");

    updateShotTables();

});


updateShotTables();

const showUnproven = document.getElementById("showUnproven");

showUnproven.addEventListener("change", () => {

    const url = new URL(window.location);

    if (showUnproven.checked)
        url.searchParams.set("showUnproven", "true");
    else
        url.searchParams.delete("showUnproven");

    window.location = url;
});