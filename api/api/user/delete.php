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

             // delete the user
            if($user->delete()){
                Response::res200(
                    new ResponseBody(
                        "User deleted.", 
                        ""
                    ));
            }
            
            // message if unable to create user
            else{
                Response::res400(
                    new ResponseBody(
                        "Unable to delete user.", 
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