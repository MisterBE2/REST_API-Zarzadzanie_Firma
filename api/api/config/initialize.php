<?php
    include_once 'core.php';

    // required headers
    header("Access-Control-Allow-Origin: " . $siteDir);
    header("Content-Type: application/json; charset=UTF-8");
    header("Access-Control-Allow-Methods: POST");
    header("Access-Control-Max-Age: 3600");
    header("Access-Control-Allow-Headers: Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With");
    
    // files needed to connect to database
    include_once 'database.php';
    include_once '../objects/user.php';
    
    // get database connection
    $database = new Database();
    $db = $database->getConnection();

    // instantiate user object
    $user = new User($db);

    // get posted data
    $data = json_decode(file_get_contents("php://input"));
    
    // set Admin default property values
    $user->firstname = "admin";
    $user->lastname = "admin";
    $user->email = "admin";
    $user->password = "admin";
    $user->position = "CEO";
    $user->permission = "0";

    if($user->adminExist())
    {
        // set response code
        http_response_code(200); //ok

        echo json_encode(array("message" => "Server already initialized."));
        exit();
    }
    
    // create the Admin
    if($user->create()){
    
        // set response code
        http_response_code(200); //ok

        echo json_encode(array("message" => "Server initialized sucesfully."));
    }
    
    // message if unable to create user
    else{
    
        // set response code
        http_response_code(500); //	Internal error
    
        // display message: unable to create user
        echo json_encode(array("message" => "Unable to initialize server."));
    }
?>