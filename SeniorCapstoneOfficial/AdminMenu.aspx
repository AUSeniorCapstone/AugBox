<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminMenu.aspx.cs" Inherits="SeniorCapstoneOfficial.AdminMenu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Style.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="width:100%">
    <div style="float:left; width:50%; text-align:center">
                    <asp:Label ID="SearchUserLabel" runat="server">Search User</asp:Label>
        <br />
        <br />
                    <asp:TextBox ID="SearchUserTextBox" runat="server"> </asp:TextBox>
        <asp:Button ID="SearchUserButton" runat="server" Text ="Search User" OnClick="SearchUserButton_Click" />
        <br />
        <br />
        <asp:PlaceHolder ID="SearchUserPlaceHolder" runat="server"></asp:PlaceHolder>

        </div>
        <div style="float:right; width:50%; text-align:center">
            <asp:Button ID="LogoutBtn" runat="server" Text="Logout" OnClick="LogoutBtn_Click" /> <br />
            <asp:Button ID="StudentSearchBtn" runat="server" Text="Student Search Page" OnClick="StudentSearchBtn_Click" /> <br />
            <asp:Label ID="AddUserLabel" runat="server">Add User</asp:Label>
            <br />
            <br />
            <asp:Label ID="FistNameLabel" runat="server">First Name: </asp:Label>
            <asp:TextBox ID="FirstNameTextbox" runat="server"> </asp:TextBox>
            <br />
            <asp:Label ID="LastNameLabel" runat="server">Last Name: </asp:Label>
            <asp:TextBox ID="LastNameTextbox" runat="server"> </asp:TextBox>
             <br />
            <asp:Label ID="UserNameLabel" runat="server">Username: </asp:Label>
            <asp:TextBox ID="UserNameTextBox" runat="server"> </asp:TextBox>
             <br />
            <asp:Label ID="PasswordLabel" runat="server">Password: </asp:Label>
            <asp:TextBox ID="PasswordTextBox" runat="server"> </asp:TextBox>
            <br />
            <asp:Label ID="AdminLabel" runat="server">Admin: </asp:Label>
            <asp:CheckBox ID="AdminCheckbox" runat="server" />
            <br />
            <asp:Button ID="AddUserButton" runat="server" Text="Add User" OnClick="AddUserButton_Click" />
            <asp:placeholder ID="PH" runat="server" ></asp:placeholder>
        </div>
        
    </div>
        
    </form>
</body>
</html>
