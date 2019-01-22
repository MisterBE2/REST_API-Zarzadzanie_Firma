<?php
    include_once '../shared/standard_headers.php';
    header("Access-Control-Allow-Methods: PATCH");

    // files needed to connect to database
    include_once '../config/database.php';
    include_once '../objects/user.php';
    include_once '../shared/utilities.php';
    include_once '../shared/responses.php';
    
    // get database connection
    $database = new Database();
    $db = $database->getConnection();

    // instantiate user object
    $sender = new User($db);
    $user = new User($db);
    
    // get posted data
    $data = json_decode(file_get_contents("php://input"));

    if($data === NULL)
    {
        Response::res401(
            new ResponseBody(
                "No input present.", 
                ""
            ));
    }
    else if(count((array)$data) == 7)
    {
        $result = Util::isEmptyArray($data);

        if(count((array)$result) > 0)
        {
            if(!in_array("password", $result))
                Response::res401(new ResponseBody("Invalid input.", $result));
        }
    }
    else
        Response::res401(new ResponseBody("Invalid input.", ""));
    
    $decoded = Util::getJWT($data->body);

    if($decoded)
    {
        $sender->email = $decoded->data->email;
        if(!$sender->emailExists())
        {
            Response::res400(
                new ResponseBody(
                    "Sender does not exist.", 
                    ""
                ));
        }
        else if($decoded->data->permission >= 1)
        {
            if($decoded->data->email != $data->email)
            {
                Response::res400(
                    new ResponseBody(
                        "Insufficient permission.", 
                        ""
                    ));
            }
        }

        $user->email = $data->email;
        if(!$user->emailExists())
        {
            Response::res400(
                new ResponseBody(
                    "User does not exist.", 
                    ""
                ));
        }

        if($user->email != $data->newemail)
        {
            $user->email = $data->newemail;
            if($user->emailExists())
            {
                Response::res400(
                    new ResponseBody(
                        "User already exist.", 
                        ""
                    ));
            }
        }

        // Update all values, because emailExist() populates them with old ones
        $user->firstname = $data->firstname;
        $user->lastname = $data->lastname;
        $user->password = $data->password;
        $user->position = $data->position;
        $user->newemail = $data->newemail;
        $user->email = $data->email;

        // delete the user
        if($user->update()){
            Response::res200(
                new ResponseBody(
                    "User updated sucesfully.", 
                    Util::encodeJWTFromUser($user)
                ));
        }
        
        // message if unable to create user
        else{
            Response::res400(
                new ResponseBody(
                    "Update failed.", 
                    ""
                ));
        }
    }
    else
    {
        Response::res401(
            new ResponseBody(
                "Invalid token.", 
                ""
            ));
    }
?>