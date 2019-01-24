class User
{
    constructor(id, firstname, lastname, email, position, created, updated, permission, status, newemail)
    {
        this.id = id;
        this.firstname = firstname;
        this.lastname = lastname;
        this.email = email;
        this.position = position;
        this.created = created;
        this.updated = updated;
        this.permission = permission;
        this.status = status;
        this.newemail = newemail == null ? "" : newemail;
    }

    getToken(password, sucess, error)
    {
        let _data = {
            "email" : this.email,
            "password" : password
        }

        console.log(_data);

        $.ajax({
            method: "GET", 
            dataType: "json",
            url: "../../api/api/user/token.php",
            data: _data,
            success: function (data) {
                sucess(data);
            },
            error: function (xhr, status) {
                error(xhr.responseText);
            }
          })
    }

    validate(token, sucess, error, userRef)
    {
        $.ajax({
            method: "GET", 
            dataType: "json",
            url: "../../api/api/user/validate.php",
            data: {"body" : token},
            success: function (data) {
                if(userRef != null)
                {
                    let temp = data["body"];

                    userRef.id = temp["id"];
                    userRef.firstname = temp["firstname"];
                    userRef.lastname = temp["lastname"];
                    userRef.email = temp["email"];
                    userRef.position = temp["position"];
                    userRef.created = temp["created"];
                    userRef.updated = temp["updated"];
                    userRef.permission = parseInt(temp["permission"]);
                    userRef.status = temp["status"];
                }

                sucess(data);
            },
            error: function (xhr, status) {
                error(xhr.responseText);
            }
          })
    }

    create(token, password, sucess, error)
    {
        let data = {
            "email" : this.email,
            "body" : token,
            "firstname" : this.firstname,
            "lastname" : this.lastname,
            "position" : this.position,
            "password" : password,
        }

        //console.log(data);

        $.ajax({
            method: "POST", 
            dataType: "json",
            url: "../../api/api/user/create.php",
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

    update(token, password, sucess, error)
    {
        let data = {
            "email" : this.email,
            "body" : token,
            "firstname" : this.firstname,
            "lastname" : this.lastname,
            "position" : this.position,
            "password" : password,
            "newemail" : this.newemail
        }

        //console.log(data);

        $.ajax({
            method: "POST", 
            dataType: "json",
            url: "../../api/api/user/update.php",
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

    delete(token, sucess, error)
    {
        let data = {
            "email" : this.email,
            "body" : token
        }

        //console.log(data);

        $.ajax({
            method: "DELETE", 
            dataType: "json",
            url: "../../api/api/user/delete.php",
            data: JSON.stringify(data),
            success: function (data) {
                console.log(data);
                sucess(data);
            },
            error: function (xhr, status) {
                console.log(xhr.responseText);
                error(xhr.responseText);
            }
        })
    }

    get(token, email, sucess, error)
    {
        let data;

        if(email == null || email == "")
        {
            data = {
                "body" : token
            }
        }
        else
        {
            data = {
                "email" : email,
                "body" : token
            }
        };

        $.ajax({
            method: "GET", 
            dataType: "json",
            url: "../../api/api/user/get.php",
            data: data,
            success: function (data) {

                data = data["body"];
                let users= [];

                data.forEach(e => {
                    let user = new User(
                        e["id"],
                        e["firstname"],
                        e["lastname"],
                        e["email"],
                        e["position"],
                        e["created"],
                        e["updated"],
                        parseInt(e["permission"]),
                        e["status"]
                    );

                    users.push(user);
                });

                sucess(users);
            },
            error: function (xhr, status) {
                error(xhr.responseText);
            }
        })
    }
}