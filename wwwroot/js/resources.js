async function loadArticle(name) {
    const container =
        document.getElementById("articleContent");

    try {
        const response =
            await fetch(`/resources/${name}.html`);

        if (!response.ok)
            throw new Error();

        container.innerHTML =
            await response.text();
    }
    catch
    {
        container.innerHTML = `
        <div class="alert alert-danger">

            Не удалось загрузить статью или же её пока что нет!

        </div>`;
    }
}

document.addEventListener("DOMContentLoaded", () => {
    loadArticle("welcome");
});