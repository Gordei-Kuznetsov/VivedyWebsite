function SearchForMovie() {
    var input, filter, ul, li, h3, i, txtValue;
    input = document.getElementById("myInput");
    filter = input.value.toUpperCase();
    ul = document.getElementById("movies");
    li = ul.getElementsByTagName("li");
    for (i = 0; i < li.length; i++) {
        h3 = li[i].getElementsByClassName("moviename")[0];
        txtValue = h3.textContent || h3.innerText

        if (txtValue.toUpperCase().indexOf(filter) > -1) {
            li[i].style.display = "";
        }
        else {
            li[i].style.display = "none";
        }
    }
}
