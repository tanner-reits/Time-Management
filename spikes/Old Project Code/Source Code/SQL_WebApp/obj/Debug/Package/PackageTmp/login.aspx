<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="SQL_WebApp.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <link rel='stylesheet' type="text/css" href="Styles\Grid.css"/>
        <link rel='stylesheet' type="text/css" href="Styles\ionicons.min.css"/>
        <link rel="stylesheet" type="text/css" href="Styles\Style.css"/>
        <link rel="stylesheet" type="text/css" href="Styles\queries.css"/>
        <link href="https://fonts.googleapis.com/css?family=Raleway:100,300,300i,400,400i" rel="stylesheet"/>
        
        <title>Eagle Time Entry</title>
    </head>

    <body>
        <header>
            <div class="row">
                <img src="Content\Images\Eagle10.png" alt="Eagle Technologies logo"/>
                <h1>Eagle Time Entry</h1>
            </div>
        </header>

        <section class="information">
            <div class="row">
                <asp:Label runat="server" ID="lblError" Visible="false">Invalid username or password. Please try again.</asp:Label>
            </div>
            <div class="row">
                <h2>Please enter your Questica login information below</h2>
            </div>
            <div class="row">
                <form id="forum1" runat="server" class="login">
                    <div class="row">
                        <div class="col span-1-of-3">
                            <asp:Label runat="server" CssClass="infoLabel">Username</asp:Label>
                        </div>
                        <div class="col span-2-of-3">
                            <asp:TextBox ID="txtUsername" runat="server" required="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col span-1-of-3">
                            <asp:Label runat="server" CssClass="infoLabel">Password</asp:Label>
                        </div>
                        <div class="col span-2-of-3">
                            <asp:TextBox TextMode="Password" ID="txtPassword" runat="server" required="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col span-1-of-3">
                            <asp:Label runat="server"></asp:Label>
                        </div>
                        <div class="col span-2-of-3">
                            <asp:Button ID="btnSubmit" runat="server" OnClick="btnLogin_Click" Text="Login"></asp:Button>
                        </div>
                    </div>
                </form>
            </div>
        </section>
    </body>
</html>
