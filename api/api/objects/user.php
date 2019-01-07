<?php
    class User
    {
        // database connection and table name
        private $conn;
        private $table_name = "users";
    
        // object properties
        public $id;
        public $firstname;
        public $lastname;
        public $email;
        public $password;
        public $permision;
        public $created;
        public $updated;

        // constructor
        public function __constructor()
        {
            $this->conn = $db;
        }
    }
?>