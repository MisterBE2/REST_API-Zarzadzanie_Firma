<?php
    include_once '../shared/standard_headers.php';
    
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

    // get jwt
    $jwt=isset($data->jwt) ? $data->jwt : "";
    $email=isset($data->email) ? $data->email : "";

    $user->email = $email;

    if(!$user->emailExists() || $email == "")
    {
        // set response code
        http_response_code(400);

        echo json_encode(array("message" => "User does not exist."));
        exit();
    }

    // if jwt is not empty
    if($jwt){

        $decoded = Util::getJWT($jwt);

        if($decoded)
        {
            if($decoded->data->permission >= 1)
            {
                if($decoded->data->email != $user->email)
                {
                    // set response code
                    http_response_code(400);

                    echo json_encode(array("message" => "Insufficient permission."));
                    exit();
                }
            }

             // delete the user
            if($user->delete()){
            
                // set response code
                http_response_code(200); //ok
            
                // display message: user was created
                echo json_encode(array("message" => "User was deleted."));
                exit();
            }
            
            // message if unable to create user
            else{
            
                // set response code
                http_response_code(400); //	Bad Request
            
                // display message: unable to create user
                echo json_encode(array("message" => "Unable to delete user."));
                exit();
            }
        }
        else
        {
            // set response code
            http_response_code(401);
        
            // tell the user access denied
            echo json_encode(array("message" => "Invalid token."));
            exit();
        }
    }
    else{
    
        // set response code
        http_response_code(401);
    
        // tell the user access denied
        echo json_encode(array("message" => "Invalid token."));
    }
?>