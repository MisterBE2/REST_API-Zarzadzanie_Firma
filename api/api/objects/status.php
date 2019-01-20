<?php
    class Status
    {
        // database connection and table name
        private $conn;
        public $table_name = "status";
    
        // object properties
        public $id;
        public $user_id;
        public $status;
        public $updated;

        // constructor
        public function __construct($db){
            $this->conn = $db;
        }

        public function set()
        {
            // Check if status already exist
            $query = "SELECT id FROM ".$this->table_name." WHERE user_id = :user_id";
            $stmt = $this->conn->prepare($query);
            $this->status=Util::clear($this->status);
            $stmt->bindParam("user_id", $this->user_id, PDO::PARAM_INT);
            $stmt->execute();

            // Update or create status
            if($stmt->rowCount() > 0)
            {
                $query = "UPDATE ".$this->table_name." SET status = :status WHERE user_id = :user_id";
            }
            else
            {
                $query = "INSERT INTO ".$this->table_name." (user_id, status) VALUES(:user_id, :status)";
            }
            $stmt = $this->conn->prepare($query);
            $stmt->bindParam("user_id", $this->user_id, PDO::PARAM_INT);
            $stmt->bindParam("status", $this->status, PDO::PARAM_STR);

            if($stmt->execute())
                return true;
            
            return false;
        }

        function get()
        {
            // Check if status already exist
            $query = "SELECT status, updated, user_id FROM ".$this->table_name." WHERE user_id = :user_id";
            $stmt = $this->conn->prepare($query);
            $stmt->bindParam("user_id", $this->user_id, PDO::PARAM_INT);
            $stmt->execute();

            if($stmt->rowCount() > 0)
            {   
                $result = $stmt->fetch(PDO::FETCH_ASSOC);

                extract($result);

                $this->status = $status;
                $this->updated = $updated;

                return $result;
            }

            return false;
        }
    }
?>