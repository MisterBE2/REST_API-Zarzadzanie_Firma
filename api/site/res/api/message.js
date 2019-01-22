// Messages pagination
var messages_ammount = 100;

class Message
{
    constructor(user_from, user_to, message, sended)
    {
        this.message = message;
        this.user_from = user_from;
        this.user_to = user_to;
        this.sended = sended;
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
                //console.log();
                sucess(data);
            },
            error: function (xhr, status) {
                //console.log();
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

        //console.log(data);

        $.ajax({
            method: "GET", 
            dataType: "json",
            url: "../../api/api/message/get.php",
            data: data,
            success: function (data) {
                data = data["body"];

                //console.log(data);

                let messagesRaw = data["messages"];
                let messages = [];
                let users = {};
                users[data["userFrom"]["id"]] = data["userFrom"]["email"];
                users[data["userTo"]["id"]] = data["userTo"]["email"];

                //console.log(users);

                messagesRaw.forEach(e => {
                    let mes = new Message();
                    mes.user_from = users[e["from_user_id"]];
                    mes.user_to = users[e["to_user_id"]];
                    mes.message = e["message"];
                    mes.sended = e["date"];

                    messages.push(mes);
                });

                sucess(messages);
            },
            error: function (xhr, status) {
                //console.log(JSON.parse(xhr.responseText));
                error(xhr.responseText);
            }
        })
    }
}