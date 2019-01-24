<?php
    include_once '../shared/standard_headers.php';
    header("Access-Control-Allow-Methods: POST");
    
    // files needed to connect to database
    include_once '../config/database.php';
    include_once '../objects/status.php';
    include_once '../objects/user.php';
    include_once '../shared/utilities.php';
    include_once '../shared/responses.php';
    
    // get database connection
    $database = new Database();
    $db = $database->getConnection();

    $status = new Status($db);
    $user = new User($db);
    
    if(count((array)$_POST) > 0)
    $data = json_decode(json_encode($_POST));
        else
    $data = json_decode(file_get_contents("php://input"));
    
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
    
    $decoded = Util::getJWT($data->body);

    if($decoded)
    {
        $user->email = $decoded->data->email;
        if(!$user->emailExists())
        {
            Response::res400(
                new ResponseBody(
                    "User does not exist.", 
                    ""
                ));
        }

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
?>