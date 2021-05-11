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
    let windowpostion = window.scrollY >= 700;

    header.classList.toggle('bg-secondary', windowpostion);
    header.classList.toggle('navbar-red', windowpostion);
    header.classList.toggle('navbarred', windowpostion);
})