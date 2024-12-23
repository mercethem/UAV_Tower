const loginForm = document.querySelector("form");
const loadingSpinner = document.getElementById("loading-spinner");

loginForm.addEventListener("submit", () => {
    loadingSpinner.style.display = "flex";
});
