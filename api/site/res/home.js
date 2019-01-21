// Global variables
let lastStatusModalContent = $("#statusModalBody").html();
let lastNewUserModalContent = $("#newUserModalBody").html();
let lastNewUserModalSubmitted;
let lastDeleteUserModalContent = $("#deleteUserMainBody").html();

let mainUser = new User();

let loadedUsers;
let selectedUser;

let currentNewUserModalStatus;
let newUserModalValues = {
    new_user : 0,
    edit_user : 1,
    edit_self : 2
}

//Buttons
$("#home").click(()=>{

});

$("#messages").click(()=>{

});

$("#signout").click(function(){
    Core.deleteCookie("token");
    checkLogged();
});


//User
$(document).ready(()=>{
    loadSelf();
    loadUsers();
});

function initialiseUser(data)
{
    data = data["body"];
    
    //console.log(data);

    $("#firstname_badge").html(data["firstname"]);
    $("#lastname_badge").html(data["lastname"]);
    $("#email_badge").html(data["email"]);
    $("#position_badge").html(data["position"]);
    $("#created_badge").html(data["created"]);

    if(parseInt(data["permission"]) == 0)
    {
        $("#createuser_button").show();
        $("#admin_badge").show();
    }
    else
    {
        $("#createuser_button").hide();
        $("#admin_badge").hide();
    }

    //$("#status_content_badge").html(mainUser.status);

    let status = new Status();
    status.get(Core.getCookie("token"), updateStatus, displayError)
}

function initialiseUserError()
{
    Core.deleteCookie("token");
    Core.sendToIndex();
}

function updateStatus(data)
{
    if(data["body"]["status"] != null)
        $("#status_content_badge").html(data["body"]["status"]);
    else
        $("#status_content_badge").html("");
}

function displayError(data)
{
    console.log("Error");

    try {
        console.log(JSON.parse(data));
    } catch (error) {
        console.log(data);
    }
}

// Co-workers
function addCoworker(firstname, lastname, position, status, email)
{
    let body = '<div class="border bg-white m-2 p-3 rounded" style="width: 20em; display: inline-block;">';
    body += '<h5 class="border-bottom">'+firstname+' '+lastname+'</h5>';
    body += '    Postition: ' + position;

    if(status === null || status == "" || status.length == 0)
    {
        body += '    <br>';
    }
    else
    {
        body += '    <br>';
        body += '    Status: ' + status;
    }
    body += '   <br>';
    body += '    <a href="#" class="badge badge-primary text-white" onclick="privateMessage(\''+email+'\')">PM</a>';

    if(mainUser.permission == 0)
    {
        body += '<a href="#" class="badge badge-success ml-1 text-white" data-toggle="modal" data-target="#newusermodal" onclick="editUser(\''+email+'\')">Edit</a>'
        body += '<a href="#" class="badge badge-danger ml-1 text-white" data-toggle="modal" data-target="#deleteusermodal" onclick="deleteUser(\''+email+'\')">Delete</a>'
    }

    body += '</div>'

    $("#content").append(body);
}

function privateMessage(email)
{

}

function displayUsers(users)
{   
    //console.log(users);
    clearContent();
    loadedUsers = users;
    users.forEach(user => {
        addCoworker(user.firstname, user.lastname, user.position, user.status, user.email);
    });
}

// Delete user

$("#deleteusermodal_button").click(()=>{
    if(selectedUser != null)
    {
        $("#deleteusermodalclose_button").prop("disabled", true);
        $("#deleteusermodalclosetop_button").prop("disabled", true);
        $("#deleteusermodal_button").prop("disabled", true);

        Responsive.placeSpinner("deleteUserMainBody", "Deleting");

        selectedUser.delete(Core.getCookie("token"), deleteSucess, deleteError);
    }
});

function deleteUser(email)
{
    $("#deleteusermodalclose_button").prop("disabled", false);
    $("#deleteusermodalclosetop_button").prop("disabled", false);
    $("#deleteusermodal_button").prop("disabled", false);

    $("#deleteUserMainBody").html(lastDeleteUserModalContent);

    selectedUser = getFromLoadedUsers(email);

    selectedUser = selectedUser[0];

    $("#userToDelete").html(selectedUser.firstname + " " + selectedUser.lastname);
}

function deleteSucess(data)
{
    $("#deleteusermodal").modal("hide");
    selectedUser = null;
    loadUsers();
}

function deleteError(data)
{
    //console.log(data);
    selectedUser = null;
    data = JSON.parse(data);
    //$("#deleteUserMainBody").html("Deleting failed!");
    Responsive.putAlert("deleteUserMainBody", "<span class='font-weight-bold'>Deleting failed!</span><br><span>Server response: " + data["message"]+"</span>");

    $("#deleteusermodalclose_button").prop("disabled", false);
    $("#deleteusermodalclosetop_button").prop("disabled", false);
}

// Status

$("#changestatus_button").click(function(){
    $("#closestatusmodal_button").prop("disabled", false);
    $("#closestatustopmodal_button").prop("disabled", false);
    $("#changestatusmodal_button").prop("disabled", false);

   $("#statusModalBody").html(lastStatusModalContent);
});

$("#changestatusmodal_button").click(function(){
    $("#closestatusmodal_button").prop("disabled", true);
    $("#closestatustopmodal_button").prop("disabled", true);
    $("#changestatusmodal_button").prop("disabled", true);

    let status = new Status(null, $("#status_content_modal").val(), null);
    Responsive.placeSpinner("statusModalBody", "Saving");

    status.set(Core.getCookie("token"), sucessChangeStatus, errorChangeStatus);
});

$(document).keypress(()=>{
    $("#wordsleftstatusmodal_small").html((128-$("#status_content_modal").val().length) + " left");
})

function sucessChangeStatus()
{
    let status = new Status();
    status.get(Core.getCookie("token"), updateStatus, displayError)
    $('#statusModal').modal('hide');
}

function errorChangeStatus()
{
    $('#statusModal').modal('hide');
}

// New user
$("#createuser_button").click(()=>{
    currentNewUserModalStatus = newUserModalValues.new_user;

    $("#newusermodal_label").html("New User");
    $("#createnewusermodal_button").html("Save");

    Responsive.clearAlert("newusermodal_error");
    $("#closenewusermodal_button").prop("disabled", false);
    $("#closenewusertopmodal_button").prop("disabled", false);
    $("#createnewusermodal_button").prop("disabled", false);
    $("#positionmodal").prop("disabled", false);

    $("#newUserModalBody").html(lastNewUserModalContent);
});

$("#createnewusermodal_button").click(()=>{
    if(validateInputs())
    {
        $("#closenewusermodal_button").prop("disabled", true);
        $("#closenewusertopmodal_button").prop("disabled", true);
        $("#createnewusermodal_button").prop("disabled", true);

        var user = new User();
        user.firstname = $("#firstnamemodal").val();
        user.lastname = $("#lastnamemodal").val();
        user.position = $("#positionmodal").val();
        let password = $("#passwordmodal").val()
        //console.log(user);

        switch (currentNewUserModalStatus) {
            case newUserModalValues.new_user:
                user.email = $("#emailmodal").val();
                user.create(Core.getCookie("token"), password, newUserSucess, newUserError);
                lastNewUserModalSubmitted = Responsive.placeSpinner("newUserModalBody", "Creating");
                break;

            case newUserModalValues.edit_user:
                if(user.email != $("#emailmodal").val())
                    user.newemail = $("#emailmodal").val();
            
                user.email = selectedUser.email;
            
                user.update(Core.getCookie("token"), password, updateUserSucess, updateUserError);
                lastNewUserModalSubmitted = Responsive.placeSpinner("newUserModalBody", "Updating");
                break;

            case newUserModalValues.edit_self:
                if(user.email != $("#emailmodal").val())
                    user.newemail = $("#emailmodal").val();

                user.email = mainUser.email;
                
                user.update(Core.getCookie("token"), password, updateUserSelfSucess, updateUserSelfError);
                lastNewUserModalSubmitted = Responsive.placeSpinner("newUserModalBody", "Updating");
                break;
        }
    }
});

$("#newuser_form").keypress(()=>{
    Responsive.clearAlert("newusermodal_error");
    clearErrorInputs();
});

function validateInputs()
{
    let $inputs = $('#newuser_form :input');
    let valid = true;

    $inputs.each(function() {
        if($(this).val().length == 0)
        {
            Responsive.redInputField($(this).attr('id'), "This field can not be empty");
            if(valid)
                valid = false;
        }
        else
        {
            Responsive.clearInputField($(this).attr('id'));
        }
    });

    return valid;
}

function clearErrorInputs()
{
    let $inputs = $('#newuser_form :input');
    let valid = true;

    $inputs.each(function() {
        Responsive.clearInputField($(this).attr('id'));
    });
}

function newUserSucess(data)
{
    $('#newusermodal').modal('hide');
    loadUsers();
}

function newUserError(data)
{
    $("#newUserModalBody").html(lastNewUserModalContent);
    $("#closenewusermodal_button").prop("disabled", false);
    $("#closenewusertopmodal_button").prop("disabled", false);
    $("#createnewusermodal_button").prop("disabled", false);
    $("#newUserModalBody").html(lastNewUserModalSubmitted);
    data = JSON.parse(data);
    //console.log(data);
    Responsive.putAlertAfter("newusermodal_error", data["message"]);
}

// Account settings
$("#settings_button").click(()=>{

    $("#newUserModalBody").html(lastNewUserModalContent);
    currentNewUserModalStatus = newUserModalValues.edit_self;

    $("#closenewusermodal_button").prop("disabled", false);
    $("#closenewusertopmodal_button").prop("disabled", false);
    $("#createnewusermodal_button").prop("disabled", false);
    $("#positionmodal").prop("disabled", false);

    $("#newusermodal_label").html("Account Settings");
    $("#createnewusermodal_button").html("Update");

    $("#firstnamemodal").val(mainUser.firstname);
    $("#lastnamemodal").val(mainUser.lastname);
    $("#emailmodal").val(mainUser.email);
    $("#positionmodal").val(mainUser.position);

    if(mainUser.permission > 0)
    {
        $("#positionmodal").prop("disabled", true);
        $("#emailmodal").prop("disabled", true);
    }
    lastNewUserModalContent = $("#newUserModalBody").html();
});

function updateUserSelfSucess(data)
{
    Core.setCookie("token", data["body"], 1);
    loadSelf();
    $('#newusermodal').modal('hide');
}

function updateUserSelfError()
{
    $("#newUserModalBody").html(lastNewUserModalContent);
    Core.deleteCookie("token");
    Core.sendToIndex();
}

// edit user

function editUser(email)
{
    currentNewUserModalStatus = newUserModalValues.edit_user;
    $("#newUserModalBody").html(lastNewUserModalContent);

    $("#closenewusermodal_button").prop("disabled", false);
    $("#closenewusertopmodal_button").prop("disabled", false);
    $("#createnewusermodal_button").prop("disabled", false);
    $("#positionmodal").prop("disabled", false);

    $("#newusermodal_label").html("User Settings");
    $("#createnewusermodal_button").html("Update");

    let user = getFromLoadedUsers(email);
    selectedUser = user;

    $("#firstnamemodal").val(user.firstname);
    $("#lastnamemodal").val(user.lastname);
    $("#emailmodal").val(user.email);
    $("#positionmodal").val(user.position);

    if(mainUser.permission > 0)
    {
        $("#positionmodal").prop("disabled", true);
        $("#emailmodal").prop("disabled", true);
    }

    lastNewUserModalContent = $("#newUserModalBody").html();
}

function updateUserSucess(data)
{
    loadUsers();
    selectedUser = null;
    $('#newusermodal').modal('hide');
}

function updateUserError()
{
    $("#closestatusmodal_button").prop("disabled", false);
    $("#closestatustopmodal_button").prop("disabled", false);
    $("#changestatusmodal_button").prop("disabled", false);

    data = JSON.parse(data);
    Responsive.putAlert("newuser_form", "<span class='font-weight-bold'>Updateing failed!</span><br><span>Server response: " + data["message"]+"</span>");
}


// content

function clearContent()
{
    $("#content").html("");
}

function loadUsers()
{
    clearContent();
    mainUser.get(Core.getCookie("token"), null, displayUsers, displayError);
}

function loadSelf()
{
    mainUser.validate(Core.getCookie("token"), initialiseUser, initialiseUserError, mainUser);
}

function getFromLoadedUsers(email)
{
    return loadedUsers.filter(obj => {
        return obj.email == email
    })[0];
}