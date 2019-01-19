<?php
    include_once '../shared/standard_headers.php';

    // files needed to connect to database
    include_once '../config/database.php';
    include_once '../objects/user.php';
    include_once '../shared/utilities.php';
    include_once '../shared/responses.php';
    
    // get database connection
    $database = new Database();
    $db = $database->getConnection();
    
    // instantiate user object
    $user = new User($db);
    
    // get posted data
    $data = json_decode(file_get_contents("php://input"));
    
    // set product property values
    $user->email = $data->email;
    $email_exists = $user->emailExists();

    if(!$email_exists)
    {
        Response::res400(
            new ResponseBody(
                "User does not exist.", 
                ""
            ));
    }
    
    // check if email exists and if password is correct
    if($email_exists && password_verify($data->password, $user->password)){

        $jwt = Util::encodeJWTFromUser($user);

        // if generating jwt failed 
        if(!$jwt)
        {
            Response::res400(
                new ResponseBody(
                    "Token generation error", 
                    ""
                ));
        }

        Response::res200(
            new ResponseBody(
                "Tokenn created", 
                $jwt
            ));  
    }
    
    // login failed
    else{
        Response::res401(
            new ResponseBody(
                "Email or password incorrect.", 
                ""
            ));
    }
?>