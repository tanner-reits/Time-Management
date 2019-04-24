<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="time-entry.aspx.cs" Inherits="SQL_WebApp.time_entry" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <link rel='stylesheet' type="text/css" href="Styles\Grid.css"/>
        <link rel='stylesheet' type="text/css" href="Styles\ionicons.min.css"/>
        <link rel="stylesheet" type="text/css" href="Styles\Style.css"/>
        <link rel="stylesheet" type="text/css" href="Styles\queries.css"/>
        <link href="https://fonts.googleapis.com/css?family=Raleway:100,300,300i,400,400i" rel="stylesheet"/>

        <title>Time Card Entry</title>
    </head>

    <body>
        <header>
            <div class="row">
                <img src="Images\Eagle10.png" alt="Eagle Technologies logo"/>
                <nav class="main-nav">
                    <ul>
                        <li><a href="./Time-Entry.aspx">Time Entry</a></li>
                        <li><a href="./Management.aspx">Management</a></li>
                        <li><a href="./Review.aspx">Review</a></li>
                        <li><a href="./Logout.aspx">Logout</a></li>
                    </ul>
                </nav>
            </div>
        </header>
        
        <section class="entry">
            <div class="row">
                <asp:Label runat="server" ID="lblWelcome"></asp:Label>
            </div>
            <div class="row">
                <form id="form1" runat="server" class="entry-fields">
                    <div class="row">
                        <asp:GridView runat="server" ID="TimeView" CssClass="GridView" ShowHeaderWhenEmpty="True">
                            <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                        </asp:GridView>
                    </div>
                    <div class="row">
                        <div class="col span-1-of-3"></div>
                        <div class="col span-2-of-3">
                            <asp:Button runat="server" Text="Edit Current Period" ID="btnEdit" CssClass="btnEdit" CausesValidation="false" OnClick="btnEdit_Click" />
                        </div>
                    </div>
                    <div class="row">
                        <asp:Label runat="server" ID="lblInfo">Or please enter the following job information.</asp:Label>
                    </div>
                    <div class="row">
                        <div class="col span-1-of-3">
                            <asp:Label runat="server">Date:</asp:Label>
                        </div>
                        <div class="col span-2-of-3">
                            <asp:DropDownList runat="server" ID="drpDate" CssClass="ddl" AutoPostBack="true" OnSelectedIndexChanged="drpDate_SelectedIndexChanged"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="dateValid" runat="server" ControlToValidate="drpDate" ErrorMessage="Required" CssClass="valid" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col span-1-of-3">
                            <asp:Label runat="server">Project:</asp:Label>
                        </div>
                        <div class="col span-2-of-3">
                            <asp:DropDownList runat="server" ID="drpProject" CssClass="ddl" AutoPostBack="true" OnSelectedIndexChanged="drpProject_SelectedIndexChanged"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="projectValid" runat="server" ControlToValidate="drpProject" ErrorMessage="Required" CssClass="valid" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col span-1-of-3">
                            <asp:Label runat="server">Station:</asp:Label>
                        </div>
                        <div class="col span-2-of-3">
                            <asp:DropDownList runat="server" ID="drpStation" CssClass="ddl" AutoPostBack="true" OnSelectedIndexChanged="drpStation_SelectedIndexChanged"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="stationValid" runat="server" ControlToValidate="drpStation" ErrorMessage="Required" CssClass="valid" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col span-1-of-3">
                            <asp:Label runat="server">Labor Code:</asp:Label>
                        </div>
                        <div class="col span-2-of-3">
                            <asp:DropDownList runat="server" ID="drpLabor" CssClass="ddl"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="laborValid" runat="server" ControlToValidate="drpLabor" ErrorMessage="Required" CssClass="valid" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col span-1-of-3">
                            <asp:Label runat="server">Hours:</asp:Label>
                        </div>
                        <div class="col span-2-of-3">
                            <asp:TextBox runat="server" ID="txtHours" CssClass="txtHours"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="hoursValid" runat="server" ControlToValidate="txtHours" ErrorMessage="Required" CssClass="valid" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col span-1-of-3">
                            <asp:Label runat="server">On-the-Road:</asp:Label>
                        </div>
                        <div class="col span-2-of-3">
                            <asp:CheckBox runat="server" ID="chkRoad" CssClass="chk"></asp:CheckBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col span-1-of-3"></div>
                        <div class="col span-2-of-3">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit New Entry" OnClick="btnSubmit_Click"></asp:Button>
                        </div>
                    </div>
                </form>
            </div>
        </section>
    </body>
</html>
