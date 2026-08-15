
const replayFileInput = document.getElementById("replayFile");

if (replayFileInput) {
    // Выбор файла
    replayFileInput.addEventListener("change", parseReplayFile);

    // Зона на которую перетаскивают файл
    const dropZone = replayFileInput.closest(".drop-zone")
        ?? replayFileInput.parentElement;

    if (dropZone) {
        dropZone.addEventListener("dragover", function (event) {
            event.preventDefault();
        });

        dropZone.addEventListener("drop", async function (event) {
            event.preventDefault();

            const files = event.dataTransfer.files;

            if (!files || files.length === 0)
                return;

            // Передаем перетащенный файл в input
            const dataTransfer = new DataTransfer();

            dataTransfer.items.add(files[0]);

            replayFileInput.files = dataTransfer.files;

            // Запускаем парсинг
            document.getElementById("parsingProcess").textContent = "Идёт обработка реплея...";
            await parseReplayFile();
        });
    }
}

    async function parseReplayFile()
    {
        const file = replayFileInput.files[0];

    if (!file)
    return;


    // Проверка расширения
    if (!file.name.toLowerCase().endsWith(".rpy"))
    {
        alert("Можно загружать только файлы реплеев (.rpy).");
    return;
        }


    const formData = new FormData();
    formData.append("file", file);


    try
    {
        const response = await fetch("/Replays/Replay/Parse",
    {
        method: "POST",
        body: formData
    });

    const result = await response.json();

    if (!response.ok || !result.success)
    {
        alert(result.message ?? "Не удалось прочитать файл реплея.");
        return;
    }

        await fillReplayForm(result);


    }
    catch (error)
    {

    alert("Не удалось обработать реплей.");
        }

    }


async function fillReplayForm(result) {

    // Игра
    setSelectValue("gameSelect", result.game);

    // Подождать чтобы логика формы поменялась.
    await new Promise(resolve => setTimeout(resolve, 50));

    // Сложность
    const difficultyNames = [
        "Easy",
        "Normal",
        "Hard",
        "Lunatic",
        "Extra"
    ];

    setSelectValue(
        "difficultySelect",
        difficultyNames[result.difficulty]
    );

    // После изменения сложности updateShotTypes()
    // должен создать правильные варианты шоттипов
    await new Promise(resolve => setTimeout(resolve, 50));

    // Шоттип
    setSelectValue("shotTypeSelect", result.shot);

    // т.к. в GFW нет шоттипов, а на сайте рауты написанны ввиде них, то делаем прикол.
    if (result.game === "GFW") {
        setSelectValue("shotTypeSelect", result.route);
    }

    // Очки
    setInputValue("scoreSelect", result.score);

    // IN финал
    setSelectValue("finalSelect", result.route);

    // Дата
    if (result.timestamp) {
        const date = new Date(result.timestamp);

        if (!isNaN(date.getTime())) {
            const dateInput =
                document.getElementById("ReplayDate");

            if (dateInput) {
                dateInput.value =
                    date.toISOString().split("T")[0];
            }
        }
    }
    }


function setSelectValue(name, value) {
    const select = document.getElementById(name);

    if (!select || value === null || value === undefined)
        return;


    value = value.toString();


    const option = [...select.options]
        .find(x =>
            x.value === value ||
            x.text === value
        );


    if (option) {
        select.value = option.value;


        // Запускаем существующую логику
        select.dispatchEvent(
            new Event("change", { bubbles: true })
        );
    }
    document.getElementById("parsingProcess").textContent = "Успешно загруженно.";
    }


    function setInputValue(name, value)
    {
        const input = document.getElementById(name);

    if (!input || value === null || value === undefined)
    return;


    input.value = value;
    }