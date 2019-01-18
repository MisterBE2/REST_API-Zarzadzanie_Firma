<?php
    include_once '../shared/standard_headers.php';
    include_once '../shared/utilities.php';
    
    // get posted data
    $data = json_decode(file_get_contents("php://input"));
    
    // get jwt
    $jwt=isset($data->jwt) ? $data->jwt : "";
    
    // if jwt is not empty
    if($jwt){

        $decoded = Util::getJWT($jwt);

        if($decoded)
        {
            // set response code
            http_response_code(200);
    
            // show user details
            echo json_encode(array(
                "message" => "Access granted.",
                "data" => $decoded->data
            ));
        }
        else
        {
             // set response code
             http_response_code(401);
        
             // tell the user access denied  & show error message
             echo json_encode(array(
                 "message" => "Access denied.",
             ));
        }
    }
    
    // show error message if jwt is empty
    else{
    
        // set response code
        http_response_code(401);
    
        // tell the user access denied
        echo json_encode(array("message" => "Access denied."));
    }
?>