class Status
{
    constructor(user_id, status, updated)
    {
        this.status = status;
        this.updated = updated;
        this.user_id = user_id;
    }

    get(email, token, sucess, error)
    {
        $.ajax({
            method: "GET",
            dataType: "json",
            url: "../../api/api/status/get.php",
            data: {body : token, email : email},
            success: function (data) {
                this.status = data["body"]["status"];
                this.updated = data["body"]["updated"];
                this.user_id = data["body"]["user_id"];
                sucess(data);
            },
            error: function (xhr, status) {
                //console.log(xhr.responseText);
                error(xhr.responseText);
            }
          })
    }

    set(token, sucess, error)
    {
        let data = {
            "status" : this.status,
            "body" : token
        }

        $.ajax({
            method: "POST", 
            dataType: "json",
            url: "../../api/api/status/set.php",
            data: data,
            success: function (data) {
                //console.log(data);
                sucess(data);
            },
            error: function (xhr, status) {
                //console.log(xhr.responseText);
                error(xhr.responseText);
            }
          })
    }
}