<?php
    class User
    {
        // database connection and table name
        private $conn;
        private $table_name = "messages";
    
        // object properties
        public $id;
        public $to_user_id;
        public $from_user_id;
        public $message;
        public $updated;

        // constructor
        public function __constructor()
        {
            $this->conn = $db;
        }
    }
?>