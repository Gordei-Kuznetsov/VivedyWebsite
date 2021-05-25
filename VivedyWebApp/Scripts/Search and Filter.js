let movieSearch = document.getElementById(SearchBy);
let categoryDropdown = document.getElementById(categoryFilter);
let ratingDropdown = document.getElementById(ratingDropdown);

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
categoryDropdown.onselectionchange = function () {
    var selected = [];
    for (var option of categoryDropdown.options) {
        if (option.selected) {
            selected.push(option.value)
        }
    }
    filterCategory(selected);
}
function searchMovie() {

}
function filterCategory(categories) {
    items.forEach(item => {
        categories.forEach(category => {
            if (item.category == category) {
                item.style.display = "";
                return;
            }
        });
        item.filtered = true;
        item.style.display = "none";
    });
}
function filterRating(ratings) {
    items.forEach(item => {
        if (item.unmatched) { continue }
        else if (item.filtered) { continue }
        else {
            ratings.forEach(rating => {
                if (item.rating == rating) {
                    item.style.display = "";
                    return;
                }
            });
            item.filtered = true;
            item.style.display = "none";
        }
    });
}
