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

let toastPlacement = {
    left : 0,
    right : 1
}

let userType = {
    receiver : 0,
    sender : 1
}

let isChangeStatusModalOpen = false;
let updateInterval = setInterval(updateGlobal, 10000);

let currentMessagedUser;

//Buttons
$("#home").click(()=>{

});

$("#messages").click(()=>{

});

$("#signout").click(function(){
    Core.deleteCookie("token");
    checkLogged();
});

$(document).keypress((e)=>{

    if(($("#newusermodal").data('bs.modal') || {})._isShown)
    {
        Responsive.clearAlert("newusermodal_error");
        clearErrorInputs();

        if(e.keyCode == 13)
        {
            sendNewUserModal();
        }
    }
    else if(($("#statusModal").data('bs.modal') || {})._isShown )
    {
        if(e.keyCode == 13)
        {
            changeStatusAction();
        }

        $("#wordsleftstatusmodal_small").html((128-$("#status_content_modal").val().length) + " left");
    }
    else if(($("#pmmodal").data('bs.modal') || {})._isShown )
    {
        if(e.keyCode == 13)
        {
            sendMessageModal();
        }
    }
});

function updateGlobal()
{
    if(($("#pmmodal").data('bs.modal') || {})._isShown && currentMessagedUser != null)
    {
        let message = new Message();
        message.user_to = currentMessagedUser.email;
        message.get(Core.getCookie("token"), 0, loadMessagesSucess, loadMessagesError);
    }
}

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
function addCoworker(firstname, lastname, position, status, email, permission)
{
    let body = '<div class="border bg-white m-2 p-3 rounded" style="width: 20em; display: inline-block;">';
    body += '<p class="border-bottom"><span class="font-weight-bold mr-2">'+firstname+' '+lastname+'</span>';
    
    if(permission == 0)
        body+= '<span class="badge badge-success text-white">ADM</span>';
    
    body += '</p>';
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
    body += '    <a href="#" class="badge badge-primary text-white" data-target="#pmmodal" data-toggle="modal" onclick="privateMessage(\''+email+'\')">PM</a>';

    if(mainUser.permission == 0)
    {
        body += '<a href="#" class="badge badge-success ml-1 text-white" data-toggle="modal" data-target="#newusermodal" onclick="editUser(\''+email+'\')">Edit</a>'
        body += '<a href="#" class="badge badge-danger ml-1 text-white" data-toggle="modal" data-target="#deleteusermodal" onclick="deleteUser(\''+email+'\')">Delete</a>'
    }

    body += '</div>'

    $("#content").append(body);
}


function displayUsers(users)
{   
    //console.log(users);
    clearContent();
    loadedUsers = users;
    users.forEach(user => {
        addCoworker(user.firstname, user.lastname, user.position, user.status, user.email, user.permission);
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

    isChangeStatusModalOpen = true;

    $("#closestatusmodal_button").prop("disabled", false);
    $("#closestatustopmodal_button").prop("disabled", false);
    $("#changestatusmodal_button").prop("disabled", false);

   $("#statusModalBody").html(lastStatusModalContent);
});

function changeStatusAction()
{
    $("#closestatusmodal_button").prop("disabled", true);
    $("#closestatustopmodal_button").prop("disabled", true);
    $("#changestatusmodal_button").prop("disabled", true);

    let status = new Status(null, $("#status_content_modal").val(), null);
    Responsive.placeSpinner("statusModalBody", "Saving");

    status.set(Core.getCookie("token"), sucessChangeStatus, errorChangeStatus);
}

$("#changestatusmodal_button").click(function(){
    changeStatusAction();
});

function sucessChangeStatus()
{
    isChangeStatusModalOpen = false;
    let status = new Status();
    status.get(Core.getCookie("token"), updateStatus, displayError)
    $('#statusModal').modal('hide');
}

function errorChangeStatus()
{
    isChangeStatusModalOpen = false;
    $('#statusModal').modal('hide');
}

// New user
$("#createuser_button").click(()=>{
    currentNewUserModalStatus = newUserModalValues.new_user;

    setNewUserHeaderText("New User");
    setNewUserActionButtonText("Save");
    setNewUserControl(true);

    Responsive.clearAlert("newusermodal_error");

    $("#newUserModalBody").html(lastNewUserModalContent);
});

function setNewUserHeaderText(text)
{
    $("#newusermodal_label").html(text);
}

function setNewUserActionButtonText(text)
{
    $("#createnewusermodal_button").html(text);
}

function setNewUserControl(isEnabled)
{   
    isEnabled = !isEnabled;
    $("#closenewusermodal_button").prop("disabled", isEnabled);
    $("#closenewusertopmodal_button").prop("disabled", isEnabled);
    $("#createnewusermodal_button").prop("disabled", isEnabled);
    $("#positionmodal").prop("disabled", isEnabled);

}

function populateNewUserModal(user)
{
    $("#firstnamemodal").val(user.firstname);
    $("#lastnamemodal").val(user.lastname);
    $("#emailmodal").val(user.email);
    $("#positionmodal").val(user.position);

}

function getNewUserModal()
{
    var user = new User();
    user.firstname = $("#firstnamemodal").val();
    user.lastname = $("#lastnamemodal").val();
    user.position = $("#positionmodal").val();
    user.email = $("#emailmodal").val();

    return user;
}

function sendNewUserModal()
{
    if(validateInputs())
    {
        setNewUserControl(false);

        let user = getNewUserModal();
        let password = $("#passwordmodal").val();
        //console.log(user);

        switch (currentNewUserModalStatus) {
            case newUserModalValues.new_user:
                user.create(Core.getCookie("token"), password, newUserSucess, newUserError);
                lastNewUserModalSubmitted = Responsive.placeSpinner("newUserModalBody", "Creating");
                break;

            case newUserModalValues.edit_user:
                user.email = selectedUser.email;
                user.newemail = $('#emailmodal').val();
            
                user.update(Core.getCookie("token"), password, updateUserSucess, updateUserError);
                lastNewUserModalSubmitted = Responsive.placeSpinner("newUserModalBody", "Updating");
                break;

            case newUserModalValues.edit_self:
                user.email = mainUser.email;
                user.newemail = $('#emailmodal').val();
                
                user.update(Core.getCookie("token"), password, updateUserSelfSucess, updateUserSelfError);
                lastNewUserModalSubmitted = Responsive.placeSpinner("newUserModalBody", "Updating");
                break;
        }
    }
}

$("#createnewusermodal_button").click(()=>{
    sendNewUserModal();
});

function validateInputs()
{
    let $inputs = $('#newuser_form :input');
    let valid = true;

    $inputs.each(function() {
        if(currentNewUserModalStatus == newUserModalValues.new_user)
        {
            if($(this).val().length == 0)
            {
                Responsive.redInputField($(this).attr('id'), "This field can not be empty");
                if(valid)
                    valid = false;
            }
        }
        else if($(this).val().length == 0 && $(this).prop("id") != "passwordmodal")
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

    $inputs.each(function() {
        Responsive.clearInputField($(this).attr('id'));
    });
}

function newUserSucess(data)
{
    currentNewUserModalStatus = null;
    loadUsers();
    $('#newusermodal').modal('hide');
}

function newUserError(data)
{
    setNewUserControl(false);
    $("#newUserModalBody").html(lastNewUserModalSubmitted);
    data = JSON.parse(data);
    //console.log(data);
    Responsive.putAlertAfter("newusermodal_error", data["message"]);
}

// Account settings
$("#settings_button").click(()=>{

    $("#newUserModalBody").html(lastNewUserModalContent);
    currentNewUserModalStatus = newUserModalValues.edit_self;

    setNewUserControl(true);
    setNewUserHeaderText("Account Settings");
    setNewUserActionButtonText("Update");

    populateNewUserModal(mainUser);

    if(mainUser.permission > 0)
    {
        $("#positionmodal").prop("disabled", true);
        $("#emailmodal").prop("disabled", true);
    }

    lastNewUserModalContent = $("#newUserModalBody").html();
});

function updateUserSelfSucess(data)
{
    currentNewUserModalStatus = null;
    Core.setCookie("token", data["body"], 1);
    loadSelf();
    $('#newusermodal').modal('hide');
}

function updateUserSelfError(data)
{
    setNewUserControl(true);
    data = JSON.parse(data);
    $("#newUserModalBody").html(lastNewUserModalContent);
    Responsive.putAlert("newusermodal_error", "<span class='font-weight-bold'>Updating failed!</span><br><span>Server response: " + data["message"]+" "+JSON.stringify(data["body"])+"</span>");
}

// edit user

function editUser(email)
{
    currentNewUserModalStatus = newUserModalValues.edit_user;
    $("#newUserModalBody").html(lastNewUserModalContent);

    setNewUserControl(true);
    setNewUserHeaderText("User Settings");
    setNewUserActionButtonText("Update");

    let user = getFromLoadedUsers(email);
    selectedUser = user;

    populateNewUserModal(user);

    if(mainUser.permission > 0)
    {
        $("#positionmodal").prop("disabled", true);
        $("#emailmodal").prop("disabled", true);
    }

    lastNewUserModalContent = $("#newUserModalBody").html();
}

function updateUserSucess(data)
{
    currentNewUserModalStatus = null;
    loadUsers();
    selectedUser = null;
    $('#newusermodal').modal('hide');
}

function updateUserError(data)
{
    setNewUserControl(true);
    data = JSON.parse(data);
    $("#newUserModalBody").html(lastNewUserModalContent);
    Responsive.putAlert("newusermodal_error", "<span class='font-weight-bold'>Updating failed!</span><br><span>Server response: " + data["message"]+" "+JSON.stringify(data["body"])+"</span>");
}

// PM

function addToast(message, user, _userType, time, placement, elementId)
{
    let body = '<div role="alert" '+(placement == toastPlacement.right ? 'style="margin-left: auto;' : '')+'aria-live="assertive" aria-atomic="true" class="toast '+(_userType == userType.sender ? 'border border-primary' : '')+'" data-autohide="false"><div class="toast-header"><img src="res/img/user_icon.svg" class="rounded mr-2" alt=""><strong class="mr-auto">'+user+'</strong><small>'+time+'</small></div><div class="toast-body">'+message+'</div></div>';

    $("#"+elementId).append(body);
    $('.toast').toast("show");
}

function privateMessage(email)
{
    $("#usernamepmmodal").html(email);

    currentMessagedUser = getFromLoadedUsers(email);

    let message = new Message();
    message.user_to = email;

    message.get(Core.getCookie("token"), 0, loadMessagesSucess, loadMessagesError);
}

function loadMessagesSucess(data)
{
    $("#pmmodal_mainbody").html("");

    data.forEach(e => {
        let utype;
        let side;

        if(mainUser.email == e.user_from)
            utype = userType.sender;
        
        if(utype == userType.sender)
            side = toastPlacement.right;
    
        addToast(e.message, e.user_from, utype, e.sended, side, "pmmodal_mainbody");
    });

    scrollBottomMessages();
}

function scrollBottomMessages()
{
    var element = document.getElementById("pmmodal_mainbody");
    element.scrollTop = element.scrollHeight;
}

function loadMessagesError(data)
{
    console.log(data);
}

$("#pmmodalsend_button").click(()=>{
    sendMessageModal();
});

function sendMessageModal()
{
    if($("#messagemodal_input").val().length > 0)
    {
        let message = new Message();
        message.user_to = currentMessagedUser.email;
        message.user_from = mainUser.email;
        message.message = $("#messagemodal_input").val();
    
        message.send(Core.getCookie("token"), sendMessageSucess, sendMessageError);
        $("#messagemodal_input").val("");
    }
}

function sendMessageSucess(data)
{
    let message = new Message();
    message.user_to = currentMessagedUser.email;

    message.get(Core.getCookie("token"), 0, loadMessagesSucess, loadMessagesError);
}

function sendMessageError(data)
{
    console.log(data);
}

$("#pmmodal").on('hidden.bs.modal', ()=>{
    currentMessagedUser = false;
    $("#pmmodal_mainbody").html("");
});

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

(function($) {
    $.fn.goTo = function() {
        $('html, body').animate({
            scrollTop: $(this).offset().top + 'px'
        }, 'fast');
        return this; // for chaining...
    }
})(jQuery);