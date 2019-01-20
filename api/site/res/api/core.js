class Core
{
  static setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays*24*60*60*1000));
    var expires = "expires="+ d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
  }

  static getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for(var i = 0; i <ca.length; i++) {
      var c = ca[i];
      while (c.charAt(0) == ' ') {
        c = c.substring(1);
      }
      if (c.indexOf(name) == 0) {
        return c.substring(name.length, c.length);
      }
    }
    return "";
  }

  static deleteCookie(cname)
  {
    document.cookie = cname + "=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
  }

  static updateCooke(cname, exdays)
  {
    this.setCookie(cname, this.getCookie(cname), exdays);
  }

  static sendToHome()
  {
    window.location.href = homeFile + '.html';
  }

  static sendToIndex()
  {
    window.location.href = loginFile + '.html';
  }

  static getCurrentFile()
  {
    let url = window.location.href.split('/');
    url = url[url.length-1];
    url = url.replace(".html", "");
    url = url.replace("#", "");

    return url;
  }
}

var homeFile = "home";
var loginFile = "index";
