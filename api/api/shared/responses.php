<?php

    class ResponseBody
    {
        public $message;
        public $body;

        // constructor
        public function __construct($message, $body){
            $this->message = $message;
            $this->body = $body;
        }
    }
    class Response
    {
        /**
         * Returns 200 response code - ok
         */
        public static function res200($response)
        {
            http_response_code(200); //ok
            echo json_encode(
                array(
                    "message" => $response->message,
                    "body" => $response->body
                )
            );
            exit();
        }

        /**
         * Returns 400 response code - bad request
         */
        public static function res400($response)
        {
            http_response_code(400);
            echo json_encode(
                array(
                    "message" => $response->message,
                    "body" => $response->body
                )
            );
            exit();
        }

        /**
         * Returns 401 response code - Unauthorized
         */
        public static function res401($response)
        {
            http_response_code(401);
            echo json_encode(
                array(
                    "message" => $response->message,
                    "body" => $response->body
                )
            );
            exit();
        }

        /**
         * Returns 412 response code - Precondition Failed
         */
        public static function res412($response)
        {
            http_response_code(412);
            echo json_encode(
                array(
                    "message" => $response->message,
                    "body" => $response->body
                )
            );
            exit();
        }

        /**
         * Returns 500 response code - Internal Server Error
         */
        public static function res500($response)
        {
            http_response_code(500);
            echo json_encode(
                array(
                    "message" => $response->message,
                    "body" => $response->body
                )
            );
            exit();
        }
    }
?>