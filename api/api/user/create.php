<?php
    include_once '../config/core.php';

    // required headers
    header("Access-Control-Allow-Origin: " . $siteDir);
    header("Content-Type: application/json; charset=UTF-8");
    header("Access-Control-Allow-Methods: POST");
    header("Access-Control-Max-Age: 3600");
    header("Access-Control-Allow-Headers: Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With");
    
    // files needed to connect to database
    include_once '../config/database.php';
    include_once '../objects/user.php';
    
    // get database connection
    $database = new Database();
    $db = $database->getConnection();

    // instantiate user object
    $user = new User($db);
    
    // get posted data
    $data = json_decode(file_get_contents("php://input"));
    
    // make sure data exist
    if($data === NULL)
    {
        // set response code
        http_response_code(412); //Precondition Failed
    
        // display message: unable to create user
        echo json_encode(array("message" => "No input given."));
        exit();
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
        // set response code
        http_response_code(400); //ok

        echo json_encode(array("message" => "User with that email already exists.", "email" => $user->email));
        exit();
    }
    // create the user
    if($user->create()){
    
        // set response code
        http_response_code(200); //ok
    
        // display message: user was created
        echo json_encode(array("message" => "User was created."));
    }
    
    // message if unable to create user
    else{
    
        // set response code
        http_response_code(400); //	Bad Request
    
        // display message: unable to create user
        echo json_encode(array("message" => "Unable to create user."));
    }
?>