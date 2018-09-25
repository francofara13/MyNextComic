function insertComics() {
    $.ajax({
        type: "GET",
        url: "../Home/InsertComics",
        success: function (result) {
            alert("Terminado!");
        }
    });
}

function getRecommendation() {
    $.ajax({
        type: "GET",
        url: "../Home/GetRecommendation",
        success: function (result) {
            alert(result);
        }
    });
}