const survival = document.getElementById("survivalRadio");
const scoring = document.getElementById("scoringRadio");
const noMiss = document.getElementById("noMissCheck");
const deathCount = document.getElementById("deathCountContainer");
const score = document.getElementById("Score");

function updateShotTypes() {
    const game = document.getElementById("gameSelect").value;
    const difficulty = document.getElementById("difficultySelect").value;

    const select = document.getElementById("shotTypeSelect");

    select.innerHTML = "";

    if (!shotTypes[game])
        return;

    let availableShots = shotTypes[game];


    // HSiFS Extra
    if (game === "HSiFS") {

        const hSiFSExtraShots = [
            "Reimu",
            "Marisa",
            "Aya",
            "Cirno"
        ];

        if (difficulty === "Extra") {
            availableShots = hSiFSExtraShots;
        }
        else {
            availableShots = availableShots.filter(
                shot => !hSiFSExtraShots.includes(shot)
            );
        }
    }


    availableShots.forEach(
        shot => {
            let option = document.createElement("option");

            option.value = shot;
            option.text = shot;

            select.appendChild(option);
        });
}

function updateDifficulties() {
    const game = document.getElementById("gameSelect").value;

    const select = document.getElementById("difficultySelect");

    select.innerHTML = "";

    if (!difficulties[game])
        return;

    difficulties[game].forEach(
        difficulty => {
            let option =
                document.createElement(
                    "option");

            option.value = difficulty;

            option.text = difficulty;

            select.appendChild(option);
        });
}

function updateDeathCount() {
    deathCount.style.display = survival.checked && !noMiss.checked ? "block" : "none";
    score.style.display = scoring.checked ? "block" : "none";
}

function updateFinals() {

    const game = document.getElementById("gameSelect").value;
    const difficulty = document.getElementById("difficultySelect").value;

    const container = document.getElementById("finalContainer");
    const select = document.getElementById("finalSelect");

    select.innerHTML = "";

    if (game !== "IN" || difficulty === "Extra") {

        container.style.display = "none";

        return;
    }

    container.style.display = "block";

    finals[game].forEach(final => {

        const option = document.createElement("option");

        option.value = final;
        option.text = final;

        select.appendChild(option);

    });

}

function updateConditions() {

    const game = document.getElementById("gameSelect").value;

    // 3-е условие
    const thirdContainer = document.getElementById("thirdConditionContainer");
    const thirdCheck = document.getElementById("thirdConditionCheck");

    if (thirdConditionGames.includes(game)) {
        thirdContainer.style.display = "block";
    } else {
        thirdContainer.style.display = "none";
        thirdCheck.checked = false;
    }

    // 4-е условие
    const fourthContainer = document.getElementById("fourthConditionContainer");
    const fourthCheck = document.getElementById("fourthConditionCheck");

    if (fourthConditionGames.includes(game)) {
        fourthContainer.style.display = "block";
    } else {
        fourthContainer.style.display = "none";
        fourthCheck.checked = false;
    }
}

function updateThirdConditionLabel() {

    const game = document.getElementById("gameSelect").value;
    const label = document.getElementById("thirdConditionLabel");

    if (thirdConditionNames[game]) {
        label.textContent = thirdConditionNames[game];
    }
    else {
        label.textContent = "Без третьего условия";
    }
}

document
    .getElementById("gameSelect")
    .addEventListener(
        "change", () => {
        updateDifficulties();
        updateShotTypes();       
        updateFinals();
        updateConditions();
        updateThirdConditionLabel();
    });

document
    .getElementById("difficultySelect")
    .addEventListener("change", () => {
        updateShotTypes();
        updateFinals();
    });

survival.addEventListener("change", updateDeathCount);
scoring.addEventListener("change", updateDeathCount);
noMiss.addEventListener("change", updateDeathCount);
score.addEventListener("change", updateDeathCount);

document.addEventListener("DOMContentLoaded", () => {
    updateDifficulties();
    updateShotTypes();
    updateFinals();
    updateDeathCount();
    updateConditions();
    updateThirdConditionLabel();
});

const replayFile = document.getElementById("ReplayFile");

const replayDate = document.getElementById("ReplayDate");

replayFile.addEventListener(
    "change",
    function () {
        if (this.files.length === 0)
            return;

        const file = this.files[0];

        const date = new Date(file.lastModified);

        date.setMinutes(date.getMinutes() - date.getTimezoneOffset());

        replayDate.value = date.toISOString().slice(0, 19);
    });