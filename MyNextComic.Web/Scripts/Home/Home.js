function insertComics() {
    $.ajax({
        type: "GET",
        url: "../Home/InsertComics",
        success: function (result) {
            alert("Terminado!");
        }
    });
}