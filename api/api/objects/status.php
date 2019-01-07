<?php
    class User
    {
        // database connection and table name
        private $conn;
        private $table_name = "status";
    
        // object properties
        public $id;
        public $user_id;
        public $status;
        public $updated;

        // constructor
        public function __constructor()
        {
            $this->conn = $db;
        }
    }
?>