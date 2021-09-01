let movieSearch;
let categoryDropdown;
let ratingDropdown;
let items = Array();

window.onload = function () {
    movieSearch = document.getElementById("SearchFor");
    categoryDropdown = document.getElementById("Category");
    ratingDropdown = document.getElementById("Rating");
    movieSearch.oninput = SearchForMovie;
    categoryDropdown.oninput = CategorySelectionChange;
    ratingDropdown.oninput = RatingSelectionChange;

    let list = document.getElementById("moviesList");
    items = list.getElementsByClassName("movieCard");
    for (let i = 0; i < items.length; i++) {
        items[i].unmatched = false;
        items[i].filteredByRating = false;
        items[i].filteredByCategory = false;
        items[i].moviename = items[i].getElementsByClassName("movieName")[0].innerText;
        items[i].category = items[i].getElementsByClassName("movieCategory")[0].innerText;
        items[i].rating = items[i].getElementsByClassName("movieRating")[0].innerText;
    }
    document.getElementById("resetCategory").onclick = function () {
        for (var option of categoryDropdown.options) {
            option.selected = option.defaultSelected;
        }
        CategoryReset();
    }
    document.getElementById("resetRating").onclick = function () {
        for (var option of ratingDropdown.options) {
            option.selected = option.defaultSelected;
        }
        RatingReset();
    }
}

function SearchForMovie() {
    for (let i = 0; i < items.length; i++) {
        if (items[i].innerText.toUpperCase().indexOf(movieSearch.value.toUpperCase()) > -1) {
            items[i].unmatched = false;
            if (!items[i].filteredByRating && !items[i].filteredByCategory) {
                items[i].style.display = "";
            }
            else {
                items[i].style.display = "none";
            }
        }
        else {
            items[i].unmatched = true;
            items[i].style.display = "none";
        }
    };
}
function CategorySelectionChange() {
    var selected = [];
    for (var option of categoryDropdown.options) {
        if (option.selected) {
            selected.push(option.value)
        }
    }
    filterCategory(selected);
}

function RatingSelectionChange() {
    var selected = [];
    for (var option of ratingDropdown.options) {
        if (option.selected) {
            selected.push(option.value)
        }
    }
    filterRating(selected);
}

function filterCategory(categories) {
    for (let j = 0; j < categories.length; j++) {
        for (let i = 0; i < items.length; i++) {
            if (items[i].category == categories[j]) {
                items[i].filteredByCategory = false
                if (!items[i].unmatched && !items[i].filteredByRating) {
                    items[i].style.display = "";
                    continue;
                }
                else {
                    items[i].style.display = "none"
                }
            }
            else {
                items[i].filteredByCategory = true;
                items[i].style.display = "none";
            }
        };
    };
}
function filterRating(ratings) {
    for (let j = 0; j < ratings.length; j++) {
        for (let i = 0; i < items.length; i++) {
            if (items[i].rating == ratings[j]) {
                items[i].filteredByRating = false;
                if (!items[i].unmatched && !items[i].filteredByCategory) {
                    items[i].style.display = "";
                    continue;
                }
                else {
                    items[i].style.display = "none"
                }

            }
            else {
                items[i].filteredByRating = true;
                items[i].style.display = "none";
            }
        };
    };
}
function CategoryReset() {
    for (let i = 0; i < items.length; i++) {
        items[i].filteredByCategory = false;
        if (!items[i].unmatched && !items[i].filteredByRating) {
            items[i].style.display = "";
            continue;
        }
        else {
            items[i].style.display = "none"
        }
    }
}
function RatingReset() {
    for (let i = 0; i < items.length; i++) {
        items[i].filteredByRating = false;
        if (!items[i].unmatched && !items[i].filteredByCategory) {
            items[i].style.display = "";
            continue;
        }
        else {
            items[i].style.display = "none"
        }
    }
}

