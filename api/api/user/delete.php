<?php
    include_once '../shared/standard_headers.php';
    header("Access-Control-Allow-Methods: DELETE");

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
    if(count((array)$_GET) > 0)
        $data = json_decode(json_encode($_GET));
    else
        $data = json_decode(file_get_contents("php://input"));

    // var_dump($_GET);
    // var_dump($_POST);
    // var_dump(file_get_contents("php://input"));
    // exit();
    
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
        $user->email = $data->email;
        if(!$user->emailExists())
        {
            Response::res400(
                new ResponseBody(
                    "User does not exist.", 
                    ""
                ));
        }

        if($decoded->data->permission >= 1)
        {
            Response::res400(
                new ResponseBody(
                    "Insufficient permission.", 
                    ""
                ));
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
?>