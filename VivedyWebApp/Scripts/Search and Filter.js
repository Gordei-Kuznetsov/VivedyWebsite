let movieSearch = document.getElementById("SearchFor");
let categoryDropdown = document.getElementById("categoryFilter");
let ratingDropdown = document.getElementById("ratingDropdown");
let items = [];
document.onload = () => {
    let ul = document.getElementById("Movies");
    items = ul.getElementsByTagName("li");
    items.forEach(item => {
        item.unmatched = false;
        item.filteredByRating = false;
        item.filteredByCategory = false;
        item.moviename = item.getElementsByClassName("moviename")[0].innerText;
        item.category = item.getElementsByClassName("moviecategory")[0].innerText;
        item.rating = item.getElementsByClassName("movierating")[0].innerText;
    });
}
function SelectionChange() {
    var selected = [];
    for (var option of categoryDropdown.options) {
        if (option.selected) {
            selected.push(option.value)
        }
    }
    filterCategory(selected);
}
function SearchForMovie() {
    items.forEach(item => {
        if (item.value.toUpperCase().indexOf(movieSearch.value.toUpperCase()) > -1) {
            item.unmatched = false;
            if (!item.filteredByRating && !item.filteredByCategory) {
                item.style.display = "";
            }
            else {
                item.style.display = "none";
            }
        }
        else {
            item.unmatched = true;
            item.style.display = "none";
        }


    });
}
function filterCategory(categories) {
    items.forEach(item => {
        categories.forEach(category => {
            if (item.category == category) {
                if (!item.unmatched && !item.filteredByRating) {
                    item.filteredByCategory = false
                    item.style.display = "";
                    return;
                }
                else {
                    item.style.display = "none"
                }
            }

        });
        item.filteredByCategory = true;
        item.style.display = "none";
    });
}
function filterRating(ratings) {
    items.forEach(item => {
        ratings.forEach(rating => {
            if (item.rating == rating) {
                if (!item.unmatched && item.filterCategory) {
                    item.filteredByRating = false;
                    item.style.display = "";
                    return;
                }
                else {
                    item.style.display = "none"
                }

            }
        });
        item.filteredByRating = true;
        item.style.display = "none";
    });
}




