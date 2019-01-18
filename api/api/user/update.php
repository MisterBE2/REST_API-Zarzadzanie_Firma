<?php
    include_once '../shared/standard_headers.php';
    
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
    $user->email = $data->email;
    $jwt=isset($data->jwt) ? $data->jwt : "";

    // Verify user exist
    if(!$user->emailExists() || $user->email == '')
    {
        // set response code
        http_response_code(400); //ok

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

            // Update all values, because emailExist() populates them with old ones
            $user->firstname = $data->firstname;
            $user->lastname = $data->lastname;
            $user->password = $data->password;
            $user->position = $data->position;
            $user->permission = 1;

            // delete the user
            if($user->update()){
            
                // set response code
                http_response_code(200); //ok
            
                // display message: user was created
                echo json_encode(array("message" => "User updated sucesfully.", "jwt" => Util::encodeJWTFromUser($user)));
                exit();
            }
            
            // message if unable to create user
            else{
            
                // set response code
                http_response_code(400); //	Bad Request
            
                // display message: unable to create user
                echo json_encode(array("message" => "Update failed."));
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