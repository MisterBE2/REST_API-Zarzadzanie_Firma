<?php

    include_once "../shared/utilities.php"; 
    include_once "status.php";
    include_once "message.php";

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
        public $newemail;
        public $status;

        // constructor
        public function __construct($db){
            $this->conn = $db;
            $this->permission = 1;
            $this->status = new Status($db);
        }
    
        /**
         * Creates user
         */
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
            $this->position=Util::clear($this->position);
        
            // bind the values
            $stmt->bindParam(':firstname', $this->firstname);
            $stmt->bindParam(':lastname', $this->lastname);
            $stmt->bindParam(':email', $this->email);
            $stmt->bindParam(':position', $this->position);
            $stmt->bindParam(':permission', $this->permission, PDO::PARAM_INT);
            
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
         * Deletes user
         */
        function delete(){
            
            $tempStatus = new Status($this->conn);
            $tempMessage = new Message($this->conn);

            //Delete status
            $query = "DELETE FROM " . $tempStatus->table_name ." WHERE user_id = :id";
            $stmt = $this->conn->prepare($query);
            $stmt->bindParam(':id', $this->id);

            if(!$stmt->execute()){
                Response::res500(
                    new ResponseBody(
                        "Unable to delete user's status.", 
                        $stmt->errorInfo()
                    ));
            }  

            //Delete messages

            $query = "DELETE FROM " . $tempMessage->table_name ." WHERE (from_user_id = :id OR to_user_id = :id)";
            $stmt = $this->conn->prepare($query);
            $stmt->bindParam(':id', $this->id);

            if(!$stmt->execute()){
                Response::res500(
                    new ResponseBody(
                        "Unable to delete user's messages.", 
                        $stmt->errorInfo()
                    ));
            }   

            //Delete user
            $query = "DELETE FROM " . $this->table_name ." WHERE id = :id";
            $stmt = $this->conn->prepare($query);
            $stmt->bindParam(':id', $this->id);

            if(!$stmt->execute()){
                Response::res500(
                    new ResponseBody(
                        "Unable to delete user.", 
                        $stmt->errorInfo()
                    ));
            }   

            return true;
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
                $this->created = $row['created'];
                $this->updated = $row['updated'];
                $this->status = $row['status'];
        
                return true;
            }

            return false;
        }

        function get($email)
        {
            if(!Util::isEmpty($email))
            {
                $user = new User($this->conn);
                $user->email = $email;
                if($user->emailExists())
                {
                    return array(
                        "id" => $user->id,
                        "firstname" => $user->firstname,
                        "lastname" => $user->lastname,
                        "email" => $user->email,
                        "permission" => $user->permission,
                        "position" => $user->position,
                        "created" => $user->created,
                        "updated" => $user->updated,
                        "status" => $user->status
                    );
                }
                else
                {
                    Response::res400(
                        new ResponseBody(
                            "Target user does not exist.", 
                            ""
                        ));
                }
            }
            else
            {
                $query = "SELECT * FROM " . $this->table_name . " ORDER BY lastname DESC";
                $stmt = $this->conn->prepare( $query );
                $stmt->execute();
    
                if($stmt->rowCount()>0){
                    
                    $users = array();
              
                    while($row = $stmt->fetch(PDO::FETCH_ASSOC))
                    {
                        extract($row);

                        $tempStatus = new Status($this->conn);
                        $tempStatus->user_id = $id;
                        $tempStatus->get();

                        if($id != $this->id)
                        {
                            $user = array(
                                "id" => $id,
                                "firstname" => $firstname,
                                "lastname" => $lastname,
                                "email" => $email,
                                "permission" => $permission,
                                "position" => $position,
                                "created" => $created,
                                "updated" => $updated,
                                "status" => $tempStatus->status
                            );
    
                            array_push($users, $user);
                        }
                    }

                    return $users;
                }
                else
                {
                    Response::res400(
                        new ResponseBody(
                            "Target user does not exist.", 
                            ""
                        ));
                }
            }
        }

        // check if given email exist in the database
        function emailExists(){

            $tempStatus = new Status($this->conn);
        
            // query to check if email exists
            $query = "SELECT u.id, u.firstname, u.lastname, u.password, u.position, u.permission, u.created, u.updated, s.status
                    FROM " . $this->table_name . " as u
                        LEFT JOIN ".$tempStatus->table_name." as s 
                            on s.user_id = u.id 
                    WHERE email = ?
                    LIMIT 0,1";

            // // query to check if email exists
            // $query = "SELECT id, firstname, lastname, password, position, permission, created, updated
            // FROM " . $this->table_name . "
            // WHERE email = ?
            // LIMIT 0,1";
            
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
                $this->created = $row['created'];
                $this->updated = $row['updated'];
                $this->status = $row['status'];

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
                        email = :email,
                        position = :position,
                        permission = :permission
                        {$password_set}
                    WHERE id = :id";
        
            // prepare the query
            $stmt = $this->conn->prepare($query);
        
            // sanitize
            $this->firstname=Util::clear($this->firstname);
            $this->lastname=Util::clear($this->lastname);
            $this->email=Util::clear($this->email);
            $this->position=Util::clear($this->position);
            $this->newemail=Util::clear($this->newemail);
        
            // bind the values from the form
            $stmt->bindParam(':firstname', $this->firstname);
            $stmt->bindParam(':lastname', $this->lastname);

            if(Util::isEmpty($this->newemail))
                $stmt->bindParam(':email', $this->email);
            else
                $stmt->bindParam(':email', $this->newemail);
           
            $stmt->bindParam(':position', $this->position);
            $stmt->bindParam(':permission', $this->permission, PDO::PARAM_INT);
        
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
                $this->email = $this->newemail;
                return true;
            }
        
            return false;
        }
    }
?>