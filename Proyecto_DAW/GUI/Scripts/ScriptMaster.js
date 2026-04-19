document.addEventListener("DOMContentLoaded", function () {

    const dropdowns = document.querySelectorAll(".dropdown");
    const toggleMenu = document.getElementById("btnToggleMenu");
    const navbarMenu = document.getElementById("navbarMenu");

    // Dropdowns
    dropdowns.forEach(dropdown => {
        const toggle = dropdown.querySelector(".dropdown-toggle");

        toggle.addEventListener("click", function (e) {
            e.preventDefault();

            dropdowns.forEach(d => {
                if (d !== dropdown) d.classList.remove("open");
            });

            dropdown.classList.toggle("open");
        });
    });

    // Click afuera
    document.addEventListener("click", function (e) {
        dropdowns.forEach(dropdown => {
            if (!dropdown.contains(e.target)) {
                dropdown.classList.remove("open");
            }
        });
    });

    // Menú hamburguesa
    toggleMenu.addEventListener("click", function () {
        navbarMenu.classList.toggle("active");
    });

});