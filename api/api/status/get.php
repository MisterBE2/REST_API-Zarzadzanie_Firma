<?php
    include_once '../shared/standard_headers.php';

    include_once '../config/database.php';
    include_once '../objects/user.php';
    include_once '../objects/status.php';
    include_once '../shared/responses.php';
    
    $database = new Database();
    $db = $database->getConnection();

    $status = new Status($db);
    $user = new User($db);

    $data = json_decode(file_get_contents("php://input"));

    if($data === NULL)
    {
        Response::res401(
            new ResponseBody(
                "No input present.", 
                ""
            ));
    }
    else if(count((array)$data) == 1)
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
    
        $status->user_id = $decoded->data->id;

        if($result = $status->get()){
            Response::res200(
                new ResponseBody(
                    "Status retrived", 
                    $result
                ));
        }
        else{
            Response::res400(
                new ResponseBody(
                    "Unable to retrive Status", 
                    ""
                ));
        }
    }   
    else
    {
        Response::res500(
            new ResponseBody(
                "Token could not be decoded.", 
                ""
            )); 
    }
?>