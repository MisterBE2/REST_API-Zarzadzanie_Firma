class Loader
{
    static load(scriptURL, done)
    {
        var script = document.createElement('script');
        script.src = scriptURL;
        document.head.appendChild(script);
    }
}

Loader.load("res/api/core.js")

while(Core == null)
{
    Loader.load("res/api/message.js");
    Loader.load("res/api/user.js");
    Loader.load("res/login.js");

    if(Core.getCurrentFile() == homeFile) Loader.load("res/home.js"); 
}
