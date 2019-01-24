<?php
    include_once '../shared/standard_headers.php';
    header("Access-Control-Allow-Methods: GET");

    include_once '../shared/utilities.php';
    include_once '../shared/responses.php';
    
    // get posted data
    if(count((array)$_GET) > 0)
    $data = json_decode(json_encode($_GET));
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
    else if(count((array)$data) >= 1)
    {
        $result = Util::isEmptyArray($data);

        if(count((array)$result) > 0)
        {
            Response::res401(new ResponseBody("Invalid input.", $result));
        }
    }
    else
        Response::res401(new ResponseBody("Invalid input.", ""));
    
    // get jwt
    $jwt=isset($data->body) ? $data->body : "";
    
    // if jwt is not empty
    if($jwt){

        $decoded = Util::getJWT($jwt);

        $result = Util::isEmptyArray((array)$decoded->data);
        if(count((array)$result) > 0)
        {
            Response::res401(new ResponseBody("Invalid input.", $result));
        }

        $valid = Util::validateJWT($decoded);

        if($decoded && $valid)
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