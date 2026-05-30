<%@ Application Language="C#" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {

        ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition
        {
            Path = "https://code.jquery.com/jquery-3.7.1.min.js",
            DebugPath = "https://code.jquery.com/jquery-3.7.1.js",
            CdnSupportsSecureConnection = true,
            LoadSuccessExpression = "window.jQuery"
        });
        System.Web.HttpContext.Current?.Session?.Clear();
    }

    void Application_End(object sender, EventArgs e)
    {
    }

    void Application_Error(object sender, EventArgs e)
    {
    }

    void Session_Start(object sender, EventArgs e)
    {

        HttpCookie cookie = new HttpCookie("ASP.NET_SessionId");
        cookie.Value = Session.SessionID;
        cookie.HttpOnly = true;
        Response.Cookies.Set(cookie);

    }

    void Session_End(object sender, EventArgs e)
    {
    }

</script>
