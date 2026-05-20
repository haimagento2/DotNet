// Initialize section toggles
function initializeSectionToggles() {
    const sections = [
        { buttonId: 'toggle-customer', sectionId: 'customer-section' },
        { buttonId: 'toggle-license', sectionId: 'license-section' },
        { buttonId: 'toggle-system', sectionId: 'system-section' }
    ];

    sections.forEach(({ buttonId, sectionId }) => {
        const button = document.getElementById(buttonId);
        const section = document.getElementById(sectionId);
        
        if (button && section) {
            button.addEventListener('click', function(e) {
                e.preventDefault();
                e.stopPropagation();
                
                const chevron = button.querySelector('.chevron');
                const isVisible = section.style.display !== 'none';
                
                section.style.display = isVisible ? 'none' : 'block';
                
                if (chevron) {
                    chevron.classList.toggle('expanded');
                }
            });
        }
    });
}

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

