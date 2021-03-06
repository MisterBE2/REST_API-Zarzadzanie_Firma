<?php
    // show error reporting
    error_reporting(E_ALL);
    
    // set your default time-zone
    date_default_timezone_set('Europe/Warsaw');
    
    $siteDir= "http://localhost/php/Kursy/rest-api-authentication-example/";

    // variables used for jwt
    $api_key = "ProgramowanieAplikacyjne"; // key used to encrypt JWT
    $iss = $siteDir; // (issuer) claim identifies the principal (put out) that issued the JWT.
    $aud = "http://example.com"; // (audience) claim identifies the recipients (reciver) that the JWT is intended for
    $iat = 1356999524; // (issued at) claim identifies the time at which the JWT was issued.
    $nbf = 1357000000; // (not before) claim identifies the time before which the JWT MUST NOT be accepted for processing.
    // W PHP jest funkcja timestamp (do tego wyżej)

    // You can use another useful claim name called exp (expiration time) which identifies the expiration time on or after which the JWT MUST NOT be accepted for processing.
    // https://tools.ietf.org/html/rfc7519#section-4.1  
?>