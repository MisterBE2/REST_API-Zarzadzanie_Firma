<?php
    include_once '../shared/standard_headers.php';
    header("Access-Control-Allow-Methods: POST");

    include_once '../config/database.php';
    include_once '../objects/user.php';
    include_once '../objects/message.php';
    include_once '../shared/responses.php';
    
    $database = new Database();
    $db = $database->getConnection();

    $message = new Message($db);
    $userFrom = new User($db);
    $userTo = new User($db);
    
    if(isset($_POST))
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
    else if(count((array)$data) == 3)
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
        $userFrom->email = $decoded->data->email;
        if(!$userFrom->emailExists())
        {
            Response::res400(
                new ResponseBody(
                    "UserFrom does not exist.", 
                    ""
                ));
        }

        $userTo->email = $data->email;
        if(!$userTo->emailExists())
        {
            Response::res400(
                new ResponseBody(
                    "UserTo does not exist.", 
                    ""
                ));
        }

        $message->message = $data->message;
        $message->to_user_id = $userTo->id;
        $message->from_user_id = $userFrom->id;

        if($message->send()){
            Response::res200(
                new ResponseBody(
                    "Message sended", 
                    ""
                ));
        }
        else{
            Response::res400(
                new ResponseBody(
                    "Unable to send message", 
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