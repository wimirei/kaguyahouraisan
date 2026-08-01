document.addEventListener("DOMContentLoaded", () => {

    const html = document.documentElement;
    const button = document.getElementById("themeButton");

    if (!button)
        return;

    const icon = button.querySelector("i");

    const savedTheme =
        localStorage.getItem("theme") ||
        (window.matchMedia("(prefers-color-scheme: dark)").matches
            ? "dark"
            : "light");

    setTheme(savedTheme);

    button.addEventListener("click", () => {

        const current = html.getAttribute("data-bs-theme");

        const newTheme =
            current === "dark"
                ? "light"
                : "dark";

        setTheme(newTheme);
    });

    function setTheme(theme) {

        html.setAttribute("data-bs-theme", theme);
        localStorage.setItem("theme", theme);

        if (!icon) return;

        // 🌙 dark, 🌸 light
        icon.className =
            theme === "dark"
                ? "bi bi-moon-stars-fill"
                : "bi bi-sun";
    }

});