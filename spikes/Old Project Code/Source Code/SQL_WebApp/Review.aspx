<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Review.aspx.cs" Inherits="SQL_WebApp.Review" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <link rel='stylesheet' type="text/css" href="Styles\Grid.css"/>
        <link rel='stylesheet' type="text/css" href="Styles\ionicons.min.css"/>
        <link rel="stylesheet" type="text/css" href="Styles\Style.css"/>
        <link rel="stylesheet" type="text/css" href="Styles\queries.css"/>
        <link href="https://fonts.googleapis.com/css?family=Raleway:100,300,300i,400,400i" rel="stylesheet"/>
        <link href="https://fonts.googleapis.com/css?family=Arimo" rel="stylesheet" />

        <title>Time Card Review</title>
    </head>

    <body>
        <header>
            <div class="row">
                <img src="Content\Images\Eagle10.png" alt="Eagle Technologies logo"/>
                <nav class="main-nav">
                    <ul>
                        <li><a href="./Default.aspx">Time Entry</a></li>
                        <li><a href="./Management.aspx">Management</a></li>
                        <li><a href="./Review.aspx">Review</a></li>
                        <li><a href="./Logout.aspx">Logout</a></li>
                    </ul>
                </nav>
            </div>
        </header>
        
        <section class="review">
            <div class="row">
                <form id="form1" runat="server">
                    <div class="row">
                        <div class="col span-1-of-3">
                            <asp:Label runat="server">Pay-Period Range:</asp:Label>
                        </div>
                        <div class="col span-2-of-3">
                            <asp:DropDownList runat="server" ID="drpDateSelect" CssClass="ddl" AutoPostBack="true" OnSelectedIndexChanged="drpSelect_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <asp:GridView runat="server" ID="TimeReview" CssClass="GridView">
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                        </asp:GridView>
                        <asp:Label runat="server" Visible="false" ID="lblNotice">No entries available for the selected pay period.</asp:Label>
                    </div>
                </form>
            </div>
        </section>
    </body>
</html>
