<?php
    include_once '../shared/standard_headers.php';
    include_once '../shared/utilities.php';
    include_once '../shared/responses.php';
    
    // get posted data
    $data = json_decode(file_get_contents("php://input"));
    
    // get jwt
    $jwt=isset($data->body) ? $data->body : "";
    
    // if jwt is not empty
    if($jwt){

        $decoded = Util::getJWT($jwt);

        if($decoded)
        {
            Response::res200(
                new ResponseBody(
                    "Access granted.",
                    $decoded->data
                ));
        }
        else
        {
            Response::res400(
                new ResponseBody(
                    "Access denied.",
                    ""
                ));
        }
    }
    else{
        Response::res401(
            new ResponseBody(
                "No token present.",
                ""
            ));
    }
?>