function logInSubmit() {
    event.preventDefault();
    $.ajax({
        url: "/Account/LogIn",
        data: $("#LogInForm").serialize(),
        type: 'POST',
        success: function (result) {
            if (result === true) {
                $("#modelError").prop("hidden", "hidden");
                location.href = "/Account/Account";
            } else {
                $("#logInContainer").html(result);
                $("#modelError").prop("hidden", "");
            }
        }
    });
}

function signUpSubmit() {
    event.preventDefault();
    $.ajax({
        url: "/Account/SignUp",
        data: $("#SignUpForm").serialize(),
        type: 'POST',
        success: function (result) {
            if (result === true) {
                location.href = "/Account/Account";
                $("#SignUpModelError").prop("hidden", "hidden");
            } else {
                $("#signUpContainer").html(result);
                $("#SignUpModelError").prop("hidden", "");
            }
        }
    });
}
