class Status
{
    constructor(user_id, status, updated)
    {
        this.status = status;
        this.updated = updated;
        this.user_id = user_id;
    }

    get(token, sucess, error)
    {
        let data = {
            "body" : token
        }

        $.ajax({
            method: "GET", 
            dataType: "json",
            url: "../../api/api/status/get.php",
            data: JSON.stringify(data),
            success: function (data) {
                this.status = data["body"]["status"];
                this.updated = data["body"]["updated"];
                this.user_id = data["body"]["user_id"];
                sucess(data);
            },
            error: function (xhr, status) {
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
            data: JSON.stringify(data),
            success: function (data) {
                sucess(data);
            },
            error: function (xhr, status) {
                error(xhr.responseText);
            }
          })
    }
}