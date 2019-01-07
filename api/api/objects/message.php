<?php
    class User
    {
        // database connection and table name
        private $conn;
        private $table_name = "messages";
    
        // object properties
        public $id;
        public $user_id;
        public $message;
        public $updated;

        // constructor
        public function __constructor()
        {
            $this->conn = $db;
        }
    }
?>