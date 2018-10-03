<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminMenu.aspx.cs" Inherits="SeniorCapstoneOfficial.AdminMenu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Style.css" rel="stylesheet" />
<style>
body {font-family: "Lato", sans-serif;}

.tablink1 {
    background-color: #555;
    color: white;
    right:0;
    border: none;
    outline: none;
    cursor: pointer;
    padding: 14px 16px;
    font-size: 17px;
    width: 15%;
    position:absolute;
    bottom:0;
}

.tablink1:hover {
    background-color: #777;
}
.tablink2 {
    background-color: #555;
    color: white;
    right:15%;
    border: none;
    outline: none;
    cursor: pointer;
    padding: 14px 16px;
    font-size: 17px;
    width: 15%;
    position:absolute;
    bottom:0;

}

.tablink2:hover {
    background-color: #777;
}
.tablink3 {
    background-color: #555;
    color: white;
    right:30%;
    border: none;
    outline: none;
    cursor: pointer;
    padding: 14px 16px;
    font-size: 17px;
    width: 15%;
    position:absolute;
    bottom:0;

}

.tablink3:hover {
    background-color: #777;
}

.AUGLogo{
    height:75px;
    width:75px;
    right:100%;
}

.SearchUserButton{
    background-color: #555;
    color: white;
    border: none;
    outline: none;
    cursor: pointer;
    padding: 7px 8px;
    font-size: 17px;
    width: 125px;
}

.SearchUserButton:hover {
    background-color: #777;
}

</style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="height:75px; background-color:#033459; position:relative;">         
             <asp:Image ID="AUGLogoLittle" runat="server" ImageUrl="~/Images/AugustaLittle.png" CssClass="AUGLogo" />
            <asp:Button ID="LogoutButton" runat="server" CssClass="tablink1" Text="Logout" OnClick="LogoutBtn_Click" />
            <asp:Button ID="AdminPageButton" runat="server" CssClass="tablink2" Text="Admin Page" />
            <asp:Button ID="StudentSearchButton" runat="server" CssClass="tablink3" Text="Student Search" OnClick="StudentSearchBtn_Click" />
        </div>

    <div style="width:100%">
    <div style="float:left; width:50%; text-align:center">
        <br />
        <br />
                    <asp:Label ID="SearchUserLabel" runat="server" Font-Size="30px">Search User</asp:Label>
        <br />
        <br />
                    <asp:TextBox ID="SearchUserTextBox" runat="server" Font-Size="18px" Height="33px"> </asp:TextBox>
        <asp:Button ID="SearchUserButton" runat="server" Text ="Search User" OnClick="SearchUserButton_Click" CssClass="SearchUserButton" />
        <br />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
            ControlToValidate="FirstNameTextBox" ValidationGroup="AddUserGroup" 
            ErrorMessage="Required Field" />
        <br />
        <asp:PlaceHolder ID="SearchUserPlaceHolder" runat="server"></asp:PlaceHolder>

        </div>
        <div style="float:right; width:50%; text-align:center">
            <br />
            <br />
            <asp:Label ID="AddUserLabel" runat="server" Font-Size="30px">Add User</asp:Label>
            <br />
            <br />
            <asp:Label ID="FistNameLabel" runat="server" Font-Size="18px">First Name: </asp:Label>
            <asp:TextBox ID="FirstNameTextbox" runat="server" Font-Size="14px" Height="25px" > </asp:TextBox>
            <asp:RequiredFieldValidator ID="vldtxtNewName" runat="server"
            ControlToValidate="FirstNameTextBox" ValidationGroup="AddUserGroup" 
            ErrorMessage="Required Field" />
            <br />
            <asp:Label ID="LastNameLabel" runat="server" Font-Size="18px">Last Name: </asp:Label>
            <asp:TextBox ID="LastNameTextbox" runat="server" Font-Size="14px" Height="25px"> </asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
            ControlToValidate="LastNameTextBox" ValidationGroup="AddUserGroup" 
            ErrorMessage="Required Field" />
             <br />
            <asp:Label ID="UserNameLabel" runat="server" Font-Size="18px">Username: </asp:Label>
            <asp:TextBox ID="UserNameTextBox" runat="server" Font-Size="14px" Height="25px"> </asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
            ControlToValidate="UserNameTextBox" ValidationGroup="AddUserGroup" 
            ErrorMessage="Required Field" />
             <br />
            <asp:Label ID="PasswordLabel" runat="server" Font-Size="18px">Password: </asp:Label>
            <asp:TextBox ID="PasswordTextBox" runat="server" Font-Size="14px" Height="25px"> </asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
            ControlToValidate="PasswordTextbox" ValidationGroup="AddUserGroup" 
            ErrorMessage="Required Field" />
            <br />
            <br />
            <asp:Label ID="AdminLabel" runat="server" Font-Size="18px">Admin: </asp:Label>
            <asp:CheckBox ID="AdminCheckbox" runat="server" />
            <br />
            <br />
            <asp:Button ID="AddUserButton" runat="server" Text="Add User" OnClick="AddUserButton_Click" CssClass="SearchUserButton" CausesValidation="true" ValidationGroup="AddUserGroup" />
            <br />
            <asp:placeholder ID="PH" runat="server" ></asp:placeholder>
         </div>
        
    </div>
        
    </form>
</body>
</html>
