function login()
{
    let user = new User();
    user.email = $("#email").val();
    user.getToken($("#password").val(), loginSucess, loginError);
}

function loginSucess(data)
{
    let message = "<div id='alert' class='alert alert-success col-md-5 align-self-center' role='alert'>Signing in</div>";
    $("#alert").remove();
    $('#allerts').after(message);
    Core.setCookie("token", data["body"], 1);
    checkLoggedInterval = window.setInterval(checkLogged, 10000);
    Core.sendToHome();
}

function loginError(data)
{
    console.log(data);
    let message = "<div id='alert' class='alert alert-danger col-md-5 mt-3 align-self-center' role='alert'>Login or password incorrect.</div>";
    $("#alert").remove();
    $('#allerts').after(message);
}

//let checkLoggedInterval = window.setInterval(checkLogged, 5000);

function checkLogged()
{
    let user = new User();
    user.validate(Core.getCookie("token"), checkLoggedSucess, checkLoggedError);
}

function checkLoggedSucess(data)
{
    //Core.updateCooke("token", 1);
    //console.log("checkLoggedSucess");
    if(Core.getCurrentFile() != homeFile)
    {
        let message = "<div id='alert' class='alert alert-success col-md-5 align-self-center' role='alert'>Signing in.</div>";
        $("#alert").remove();
        $('#allerts').after(message);
        Core.sendToHome();
    }
}

function checkLoggedError(data)
{
    console.log("checkLoggedError");
    Core.deleteCookie("token");
    if(Core.getCurrentFile() == homeFile)
    {
        Core.sendToIndex();
    }

    //clearInterval(checkLoggedInterval);
}

if(Core.getCurrentFile() == loginFile)
{
    $(document).keypress((e)=>{
        if(e.keyCode == 13)
            login();
    });
}
