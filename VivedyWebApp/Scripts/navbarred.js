/*
        $(window).on('scroll', function () {
            if ($(window).scrollTop()) {
                $('nav').addClass('bg-secondary'),
                $('ul').addClass('navbar-navred');
            }
            else {
                $('nav').removeClass('bg-secondary'),
                $('ul').removeClass('navbar-navred');
                $()
            }
        })
*/
/* Windo Navbar Change*/
window.addEventListener('scroll', function() {
    let header = document.querySelector('nav');
    let windowpostion1 = window.scrollY >= 700;

    header.classList.toggle('bg-secondary', windowpostion1);
    header.classList.toggle('navbar-red', windowpostion1);
    header.classList.toggle('navbarred', windowpostion1);
})