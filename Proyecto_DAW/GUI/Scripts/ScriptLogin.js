document.addEventListener("DOMContentLoaded", function () {

    const botones = document.querySelectorAll(".toggle-password");

    botones.forEach(function (btn) {

        btn.addEventListener("click", function () {

            const container = btn.closest(".login-pass-wrap");
            const input = container.querySelector("input");

            if (input.type === "password") {
                input.type = "text";
                btn.title = "Ocultar contraseña";
            } else {
                input.type = "password";
                btn.title = "Mostrar contraseña";
            }

        });

    });

});