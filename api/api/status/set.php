<?php
    include_once '../shared/standard_headers.php';
    
    // files needed to connect to database
    include_once '../config/database.php';
    include_once '../objects/status.php';
    include_once '../shared/utilities.php';
    include_once '../shared/responses.php';
    
    // get database connection
    $database = new Database();
    $db = $database->getConnection();

    // instantiate user object
    $status = new Status($db);
    
    // get posted data
    $data = json_decode(file_get_contents("php://input"));
    
    // make sure data exist
    if($data === NULL)
    {
        http_response_code(412); //Precondition Failed
    
        echo json_encode(array("message" => "No input given."));
        exit();
    }
    
    $jwt=isset($data->jwt) ? $data->jwt : "";

    // if jwt is not empty
    if($jwt){

        $decoded = Util::getJWT($jwt);

        if($decoded)
        {
            $status->status = $data->status;
            $status->user_id = $decoded->data->id;

            if($status->set()){
                Response::res200(
                    new ResponseBody(
                        "Status updated sucesfully.", 
                        ""
                    ));
            }
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