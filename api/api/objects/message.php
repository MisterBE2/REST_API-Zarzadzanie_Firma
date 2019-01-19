<?php
    class Message
    {
        // database connection and table name
        private $conn;
        public $table_name = "messages";
    
        // object properties
        public $id;
        public $to_user_id;
        public $from_user_id;
        public $message;
        public $updated;

        // constructor
        public function __construct($db){
            $this->conn = $db;
        }

        public function send()
        {
            $query = "INSERT INTO ".$this->table_name." (from_user_id, to_user_id, message) VALUES(:from_user_id, :to_user_id, :message)";
            $stmt = $this->conn->prepare($query);
            $this->message=Util::clear($this->message);
            $stmt->bindParam("from_user_id", $this->from_user_id, PDO::PARAM_INT);
            $stmt->bindParam("to_user_id", $this->to_user_id, PDO::PARAM_INT);
            $stmt->bindParam("message", $this->message, PDO::PARAM_STR);

            if($stmt->execute())
                return true;
            
            return false;
        }

        public function get($page=0, $ammount=50)
        {
            if(!is_numeric($page) || !is_numeric($ammount))
            {
                Response::res401(
                    new ResponseBody(
                        "Pagination and ammount needs to be a number.", 
                        ""
                    ));
            }
            else if($page<0 || $ammount<1)
            {
                Response::res401(
                    new ResponseBody(
                        "Pagination needs to be at least 0, and ammount needs to be greater than zero.", 
                        ""
                    ));
            }

            $query = "SELECT id, to_user_id, from_user_id, sended, message 
                    FROM ".$this->table_name."
                    WHERE (
                        (from_user_id = :userFrom AND to_user_id = :userTo) OR 
                        (from_user_id = :userTo AND to_user_id = :userFrom)
                        ) 
                    ORDER BY sended ASC 
                    LIMIT :ammount OFFSET :offset";
            
            $stmt = $this->conn->prepare($query);

            $stmt->bindParam("userFrom", $this->from_user_id, PDO::PARAM_INT);
            $stmt->bindParam("userTo", $this->to_user_id, PDO::PARAM_INT);
            $stmt->bindParam("ammount", $ammount, PDO::PARAM_INT);
            
            $offset = $ammount * $page;
            $stmt->bindParam("offset", $offset, PDO::PARAM_INT);

            if($stmt->execute())
            {
                $response=array();

                while ($row = $stmt->fetch(PDO::FETCH_ASSOC)){
                    extract($row);
             
                    $message=array(
                        "from_user_id" => $from_user_id,
                        "to_user_id" => $to_user_id,
                        "date" => $sended,
                        "message" => $message,
                    );
             
                    array_push($response, $message);
                }

                return $response;
            }
            
            Response::res500(
                new ResponseBody(
                    "Query execution failed", 
                    ""
                ));

            
            return false;
        }
    }
?>