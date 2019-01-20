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
    else if(count((array)$data) >= 1)
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
        $user->email = $decoded->data->email;
        if(!$user->emailExists())
        {
            Response::res400(
                new ResponseBody(
                    "User does not exist.", 
                    ""
                ));
        }


        $email = isset($data->email) ? $data->email : "";
        if($result = $user->get($email)){
            Response::res200(
                new ResponseBody(
                    "User(s) retrived.", 
                    $result
                ));
        }
        
        else{
            Response::res400(
                new ResponseBody(
                    "Unable to retrive user.", 
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