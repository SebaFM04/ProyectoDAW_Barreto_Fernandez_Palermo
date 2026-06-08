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

    }

    void Application_End(object sender, EventArgs e)
    {
    }

    void Application_Error(object sender, EventArgs e)
    {
        Exception ex = Server.GetLastError();

        HttpException httpEx = ex as HttpException;
        if (httpEx != null && httpEx.GetHttpCode() == 404)
        {
            Server.ClearError();

            string destino = "MenuPrincipal.aspx";

            if (Context.Session != null && Session["UltimaPagina"] != null)
            {
                destino = Session["UltimaPagina"].ToString();
            }

            Response.Redirect(destino);
        }
    }

    void Session_Start(object sender, EventArgs e)
    {



    }

    void Session_End(object sender, EventArgs e)
    {
    }

</script>
