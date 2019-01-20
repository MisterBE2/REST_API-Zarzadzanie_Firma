<?php
    include_once '../shared/standard_headers.php';

    // files needed to connect to database
    include_once '../config/database.php';
    include_once '../objects/user.php';
    include_once '../shared/responses.php';
    
    // get database connection
    $database = new Database();
    $db = $database->getConnection();

    // instantiate user object
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
    else if(count((array)$data) == 6)
    {
        $result = Util::isEmptyArray($data);

        if(count((array)$result) > 0)
        {
            Response::res401(new ResponseBody("Invalid input.", $result));
        }
    }
    else
        Response::res401(new ResponseBody("Invalid input.", ""));
    
    $decoded = Util::getJWT($data->body);

    if($decoded)
    {
        $user->email = $data->email;
        if($user->emailExists())
        {
            Response::res400(
                new ResponseBody(
                    "User already exist.", 
                    ""
                ));
        }

        if($decoded->data->permission >= 1)
        {
            Response::res400(
                new ResponseBody(
                    "Insufficient permission.", 
                    ""
                ));
        }

        // set user property values
        $user->firstname = $data->firstname;
        $user->lastname = $data->lastname;
        $user->email = $data->email;
        $user->password = $data->password;
        $user->position = $data->position;
        $user->permission = 1;
        
        if($user->emailExists())
        {
            Response::res400(
                new ResponseBody(
                    "User with that email already exists.", 
                    ""
                ));
        }
        // create the user
        if($user->create()){
            Response::res200(
                new ResponseBody(
                    "User was created.", 
                    ""
                ));
        }
        
        // message if unable to create user
        else{
            Response::res400(
                new ResponseBody(
                    "Unable to create user.", 
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