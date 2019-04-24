﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Management.aspx.cs" Inherits="SQL_WebApp.Management" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <link rel='stylesheet' type="text/css" href="Styles\Grid.css"/>
        <link rel='stylesheet' type="text/css" href="Styles\ionicons.min.css"/>
        <link rel="stylesheet" type="text/css" href="Styles\Style.css"/>
        <link rel="stylesheet" type="text/css" href="Styles\queries.css"/>
        <link href="https://fonts.googleapis.com/css?family=Raleway:100,300,300i,400,400i" rel="stylesheet"/>
        <link href="https://fonts.googleapis.com/css?family=Arimo" rel="stylesheet" />

        <title>Management</title>
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

        <section class="management">
            <div class="row">
                <form id="form1" runat="server">
                    <div class="row">
                        <div class="col span-1-of-3">
                            <asp:Label runat="server" ID="lblEmployee">Employee:</asp:Label>
                        </div>
                        <div class="col span-2-of-3">
                            <asp:DropDownList runat="server" ID="drpEmployee" CssClass="ddl" AutoPostBack="true" OnSelectedIndexChanged="drpEmployee_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <asp:GridView runat="server" ID="EmpReview" CssClass="GridView" AutoGenerateSelectButton="True" OnSelectedIndexChanged="empReview_SelectedIndexChanged">
                            <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                        </asp:GridView>
                        <asp:Button runat="server" ID="btnApprove" Text="Approve All" Visible="false" OnClick="btnApproveAll_Click" />
                        <asp:Label runat="server" Visible="false" ID="lblNotice"></asp:Label>
                    </div>
                    <asp:Panel runat="server" ID="pnlEdit" Visible="false">
                        <div class="row">
                            <div class="col span-1-of-3">
                                <asp:Label runat="server">Date:</asp:Label>
                            </div>
                            <div class="col span-2-of-3">
                                <asp:DropDownList runat="server" ID="drpDate" CssClass="ddl" AutoPostBack="true"></asp:DropDownList>
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
                                <asp:DropDownList runat="server" ID="drpStation" CssClass="ddl" AutoPostBack="true"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="stationValid" runat="server" ControlToValidate="drpStation" ErrorMessage="Required" CssClass="valid" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col span-1-of-3">
                                <asp:Label runat="server">Labor Code:</asp:Label>
                            </div>
                            <div class="col span-2-of-3">
                                <asp:DropDownList runat="server" ID="drpLabor" CssClass="ddl"></asp:DropDownList>
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
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit Changes" OnClick="btnSubmit_Click"></asp:Button>
                                <asp:Button ID="btnDelete" runat="server" Text="Delete Entry" OnClick="btnDelete_Click"></asp:Button>
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" ValidateRequestMode="Disabled"></asp:Button>
                            </div>
                        </div>
                    </asp:Panel>
                </form>
            </div>
        </section>
    </body>
</html>