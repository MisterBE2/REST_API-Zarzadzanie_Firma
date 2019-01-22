<?php
    include_once '../shared/standard_headers.php';
    header("Access-Control-Allow-Methods: GET");

    include_once '../shared/utilities.php';
    include_once '../shared/responses.php';
    
    // get posted data
    $data = json_decode(json_encode($_GET));
    
    // get jwt
    $jwt=isset($data->body) ? $data->body : "";
    
    // if jwt is not empty
    if($jwt){

        $decoded = Util::getJWT($jwt);
        $valid = Util::validateJWT($decoded);

        if($decoded && !$valid)
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