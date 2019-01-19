<?php
    include_once '../shared/standard_headers.php';

    include_once '../config/database.php';
    include_once '../objects/user.php';
    include_once '../objects/message.php';
    include_once '../shared/responses.php';
    
    $database = new Database();
    $db = $database->getConnection();

    $message = new Message($db);
    $userFrom = new User($db);
    $userTo = new User($db);
    
    $data = json_decode(file_get_contents("php://input"));

    if($data === NULL)
    {
        Response::res401(
            new ResponseBody(
                "No input present.", 
                ""
            ));
    }


    $jwt=isset($data->body) ? $data->body : "";

    if($jwt)
    {   
        $decoded = Util::getJWT($jwt);
        
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

            // if($userTo->email == $userFrom->email)
            // {
            //     Response::res400(
            //         new ResponseBody(
            //             "Massage can not be sent to itself", 
            //             ""
            //         ));
            // }
            
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
    }
    else
    {
        Response::res401(
            new ResponseBody(
                "No token present.", 
                ""
            ));
    }
?>