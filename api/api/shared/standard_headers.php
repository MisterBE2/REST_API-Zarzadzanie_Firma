<?php
    include_once '../config/core.php';

    // required headers
    header("Access-Control-Allow-Origin: " . $siteDir);
    header("Content-Type: application/json; charset=UTF-8");
    header("Access-Control-Max-Age: 3600");
    header("Access-Control-Allow-Headers: Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With");
?>