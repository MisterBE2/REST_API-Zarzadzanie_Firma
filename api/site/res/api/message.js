// Messages pagination
var messages_ammount = 100;

class Message
{
    constructor(user_from, user_to, message)
    {
        this.message = message;
        this.user_from = user_from;
        this.user_to = user_to;
    }

    send(token, sucess, error)
    {
        let data = {
            "message" : this.message,
            "email" : this.user_to,
            "body" : token
        }

        $.ajax({
            method: "POST", 
            dataType: "json",
            url: "../../api/api/message/send.php",
            data: JSON.stringify(data),
            success: function (data) {
                sucess(data);
            },
            error: function (xhr, status) {
                error(xhr.responseText);
            }
          })
    }

    get(token, page, sucess, error)
    {
        let data = {
            "email" : this.user_to,
            "body" : token,
            "page" : page,
            "ammount" : messages_ammount
        }

        console.log(data);

        $.ajax({
            method: "POST", 
            dataType: "json",
            url: "../../api/api/message/get.php",
            data: JSON.stringify(data),
            success: function (data) {
                sucess(data);
            },
            error: function (xhr, status) {
                console.log(JSON.parse(xhr.responseText));
                error(xhr.responseText);
            }
        })
    }
}