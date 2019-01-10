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
    include_once '../shared/utilities.php';
    
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
        // set response code
        http_response_code(404);

        echo json_encode(array("message" => "User does not exist."));
        exit();
    }
    
    // check if email exists and if password is correct
    if($email_exists && password_verify($data->password, $user->password)){

        $jwt = Util::encodeJWTFromUser($user);

        // if generating jwt failed 
        if(!$jwt)
        {
             // set response code
            http_response_code(500);
        
            // generate jwt
            echo json_encode(
                array(
                    "message" => Util::$error
                )
            );
            exit();
        }

        // set response code
        http_response_code(200);
    
        // generate jwt
        echo json_encode(
                array(
                    "jwt" => $jwt
                )
            );
    
    }
    
    // login failed
    else{
    
        // set response code
        http_response_code(401);

        echo json_encode(array("message" => "Email or password incorrect."));
    }
?>