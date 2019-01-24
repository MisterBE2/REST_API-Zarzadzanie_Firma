<?php
    include_once "../config/core.php";
    include_once 'responses.php';
    include_once '../config/database.php';
    include_once '../objects/user.php';

    include_once '../libs/php-jwt-master/src/BeforeValidException.php';
    include_once '../libs/php-jwt-master/src/ExpiredException.php';
    include_once '../libs/php-jwt-master/src/SignatureInvalidException.php';
    include_once '../libs/php-jwt-master/src/JWT.php';
    use \Firebase\JWT\JWT;

    class Util
    {
        public static $error = "";

        /**
         * Returns text with escaped html entities and removed tags
         * 
         * @param string $data Data to be sanitised 
         */
        public static function clear($data)
        {
            return htmlspecialchars(strip_tags($data));
        }

        /**
         * Checks if string is empty or NULL
         */
        public static function isEmpty($data)
        {
            if(isset($data))
            {
                if($data === null)
                    return true;
                else if(!is_numeric($data))
                {
                    if($data === "") return true;
                }
            }
            else
                return true;

            return false;
        }

        /**
         * Checks if array of user imputs is empty
         */
        public static function isEmptyArray($data)
        {
            $result = array();
            foreach ($data as $key => $value) {
                if(Util::isEmpty($value))
                    array_push($result, $key);
            }

            return $result;
        }

        // public static function encodeJWT($firstName, $secondName, $id, $email, $position, $premission)
        // {
        //     global $iss;
        //     global $aud;
        //     global $iat;
        //     global $nbf;
        //     global $api_key;
            
        //     $token = array(
        //         "iss" => $iss,
        //         "aud" => $aud,
        //         "iat" => $iat,
        //         "nbf" => $nbf,
        //         "data" => array(
        //             "id" => $id,
        //             "firstname" => $firstName,
        //             "lastname" => $lastName,
        //             "email" => $email,
        //             "position" => $position,
        //             "permission" => $premission
        //         )
        //         );

        //         // generate jwt
        //         return JWT::encode($token, $api_key);
        // }

        public static function encodeJWTFromUser($user)
        {
            if(get_class($user) != "User")
            {
                $error = "User not defined";
                return false;
            }

            global $iss;
            global $aud;
            global $iat;
            global $nbf;
            global $api_key;

            $token = array(
                "iss" => $iss,
                "aud" => $aud,
                "iat" => $iat,
                "nbf" => $nbf,
                "data" => array(
                    "id" => $user->id,
                    "firstname" => $user->firstname,
                    "lastname" => $user->lastname,
                    "email" => $user->email,
                    "position" => $user->position,
                    "permission" => $user->permission,
                    "created" =>  $user->created,
                    "updated" =>  $user->updated
                )
                );

                // generate jwt
                return JWT::encode($token, $api_key);
        }

        public static function validateJWT($data)
        {
            // global $iss;
            // global $iat;
            // global $nbf;

            // if($data->iss = $iss) return false;

            $user = $data->data;

            $database = new Database();
            $db = $database->getConnection();
        
            $userTrusted = new User($db);
            $userTrusted->email = $user->email;

            if(!$userTrusted->emailExists()) return false;

            if($userTrusted->id != $user->id ||
                $userTrusted->firstname != $user->firstname ||
                $userTrusted->lastname != $user->lastname ||
                $userTrusted->position != $user->position ||
                $userTrusted->permission != $user->permission ||
                $userTrusted->created != $user->created
            ) return false;

            return true;
        }

        public static function getJWT($raw)
        {
            global $api_key;
            
            try {
                // decode jwt
                return JWT::decode($raw, $api_key, array('HS256'));       
            }
            catch (Exception $e){

                Response::res500(
                    new ResponseBody(
                        "Unable to decode token.", 
                        $e->getMessage()
                    ));

                return false;
            }
        }
    }
?>