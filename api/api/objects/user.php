<?php

    include_once "../shared/utilities.php"; 

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
        public $permission;
        public $position;
        public $created;
        public $updated;

        // constructor
        public function __construct($db){
            $this->conn = $db;
            $this->permission = 1;
        }
    
        // create new user record
        function create(){
            
             // insert query
             $query = "INSERT INTO " . $this->table_name . "
             SET
                 firstname = :firstname,
                 lastname = :lastname,
                 email = :email,
                 password = :password,
                 position = :position,
                 permission = :permission";

            // prepare the query
            $stmt = $this->conn->prepare($query);
        
            // sanitize
            $this->firstname=Util::clear($this->firstname);
            $this->lastname=Util::clear($this->lastname);
            $this->email=Util::clear($this->email);
            $this->password=Util::clear($this->password);
        
            // bind the values
            $stmt->bindParam(':firstname', $this->firstname);
            $stmt->bindParam(':lastname', $this->lastname);
            $stmt->bindParam(':email', $this->email);
            $stmt->bindParam(':position', $this->position);
            $stmt->bindParam(':permission', $this->permission);
            
            // hash the password before saving to database
            $password_hash = password_hash($this->password, PASSWORD_BCRYPT);

            if($password_hash == false) return false;

            $stmt->bindParam(':password', $password_hash);
        
            // execute the query, also check if query was successful
            if($stmt->execute()){
                return true;
            }
        
            return false;
        }
        
        /**
         * check if admin already exists
         */
        function adminExist(){
        
            // query to check if email exists
            $query = "SELECT id, firstname, lastname, password, position, permission
                    FROM " . $this->table_name . "
                    WHERE permission = 0
                    LIMIT 0,1";
        
            // prepare the query
            $stmt = $this->conn->prepare( $query );
        
            $stmt->execute();

            $num = $stmt->rowCount();
        
            // if email admin, assign values to object properties for easy access and use for php sessions
            if($num>0){
        
                // get record details / values
                $row = $stmt->fetch(PDO::FETCH_ASSOC);
        
                // assign values to object properties
                $this->id = $row['id'];
                $this->firstname = $row['firstname'];
                $this->lastname = $row['lastname'];
                $this->password = $row['password'];
                $this->permission = $row['permission'];
                $this->position = $row['position'];
        
                return true;
            }

            return false;
        }

        // check if given email exist in the database
        function emailExists(){
        
            // query to check if email exists
            $query = "SELECT id, firstname, lastname, password, position, permission
                    FROM " . $this->table_name . "
                    WHERE email = ?
                    LIMIT 0,1";
        
            // prepare the query
            $stmt = $this->conn->prepare( $query );
        
            // sanitize
            $this->email=Util::clear($this->email);
        
            // bind given email value
            $stmt->bindParam(1, $this->email);
        
            // execute the query
            $stmt->execute();
        
            // get number of rows
            $num = $stmt->rowCount();
        
            // if email exists, assign values to object properties for easy access and use for php sessions
            if($num>0){
        
                // get record details / values
                $row = $stmt->fetch(PDO::FETCH_ASSOC);
        
                // assign values to object properties
                $this->id = $row['id'];
                $this->firstname = $row['firstname'];
                $this->lastname = $row['lastname'];
                $this->password = $row['password'];
                $this->permission = $row['permission'];
                $this->position = $row['position'];

                // return true because email exists in the database
                return true;
            }
        
            // return false if email does not exist in the database
            return false;
        }
        
        // update a user record
        public function update(){
        
            // if password needs to be updated
            $password_set=!empty($this->password) ? ", password = :password" : "";
        
            // if no posted password, do not update the password
            $query = "UPDATE " . $this->table_name . "
                    SET
                        firstname = :firstname,
                        lastname = :lastname,
                        email = :email
                        {$password_set}
                    WHERE id = :id";
        
            // prepare the query
            $stmt = $this->conn->prepare($query);
        
            // sanitize
            $this->firstname=Util::clear($this->firstname);
            $this->lastname=Util::clear($this->lastname);
            $this->email=Util::clear($this->email);
        
            // bind the values from the form
            $stmt->bindParam(':firstname', $this->firstname);
            $stmt->bindParam(':lastname', $this->lastname);
            $stmt->bindParam(':email', $this->email);
        
            // hash the password before saving to database
            if(!empty($this->password)){
                $this->password=Util::clear($this->password);
                $password_hash = password_hash($this->password, PASSWORD_BCRYPT);
                $stmt->bindParam(':password', $password_hash);
            }
        
            // unique ID of record to be edited
            $stmt->bindParam(':id', $this->id);
        
            // execute the query
            if($stmt->execute()){
                return true;
            }
        
            return false;
        }
    }
?>