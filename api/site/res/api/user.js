class User
{
    constructor(id, firstname, lastname, email, position, created, updated, permission, status)
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
    }

    getToken(password, sucess, error)
    {
        let data = {
            "email" : this.email,
            "password" : password
        }

        $.ajax({
            method: "POST", 
            dataType: "json",
            url: "../../api/api/user/token.php",
            data: JSON.stringify(data),
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
        let data = {
            "body" : token,
        }

        $.ajax({
            method: "POST", 
            dataType: "json",
            url: "../../api/api/user/validate.php",
            data: JSON.stringify(data),
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
                    userRef.permission = temp["permission"];
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
            "email" : this.user_to,
            "body" : token,
            "firstname" : this.firstname,
            "lastname" : this.lastname,
            "position" : this.position,
            "password" : password,
        }

        $.ajax({
            method: "POST", 
            dataType: "json",
            url: "../../api/api/user/create.php",
            data: JSON.stringify(data),
            success: function (data) {
                sucess(data);
            },
            error: function (xhr, status) {
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
            method: "POST", 
            dataType: "json",
            url: "../../api/api/user/get.php",
            data: JSON.stringify(data),
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
                        e["permission"],
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