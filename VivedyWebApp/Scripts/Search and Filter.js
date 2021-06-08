let movieSearch = document.getElementById("SearchFor");
let categoryDropdown = document.getElementById("categoryFilter");
let ratingDropdown = document.getElementById("ratingFilter");
let items = Array();
window.onload = function () {
    let ul = document.getElementById("Movies");
    items = ul.getElementsByTagName("li");
    for (let i = 0; i < items.length; i++) {
        items[i].unmatched = false;
        items[i].filteredByRating = false;
        items[i].filteredByCategory = false;
        items[i].moviename = items[i].getElementsByClassName("moviename")[0].innerText;
        items[i].category = items[i].getElementsByClassName("moviecategory")[0].innerText;
        items[i].rating = items[i].getElementsByClassName("movierating")[0].innerText;
    }
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



