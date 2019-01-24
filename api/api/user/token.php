<?php
    include_once '../shared/standard_headers.php';
    header("Access-Control-Allow-Methods: GET");

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
    if(count((array)$_GET) > 0)
    $data = json_decode(json_encode($_GET));
        else
    $data = json_decode(file_get_contents("php://input"));

    //var_dump($data->email);
    
    // var_dump($_GET);
    // var_dump($_POST);
    // var_dump(file_get_contents("php://input"));
    // exit();

    if($data === NULL)
    {
        Response::res401(
            new ResponseBody(
                "No input present.", 
                ""
            ));
    }
    else if(count((array)$data) == 2)
    {
        $result = Util::isEmptyArray($data);

        if(count((array)$result) > 0)
        {
            Response::res401(new ResponseBody("Invalid input.", $result));
        }
    }
    else
        Response::res401(new ResponseBody("Invalid input.", ""));

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