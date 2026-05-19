// Close mobile navbar when a nav link is clicked
document.addEventListener('DOMContentLoaded', function() {
    const toggler = document.getElementById('navbarToggler');
    if (!toggler) return;

    const navLinks = document.querySelectorAll('.nav-item .nav-link');
    navLinks.forEach(link => {
        link.addEventListener('click', function() {
            if (toggler.checked) {
                toggler.checked = false;
            }
        });
    });
});
