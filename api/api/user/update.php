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
        Response::res400(
            new ResponseBody(
                "User does not exist.", 
                ""
            ));
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
                    Response::res400(
                        new ResponseBody(
                            "Insufficient permission.", 
                            ""
                        ));
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
    }
    else{
        Response::res401(
            new ResponseBody(
                "Invalid token.", 
                ""
            ));
    }
?>