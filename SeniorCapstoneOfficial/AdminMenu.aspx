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
    border-radius: 25px;
}

.SearchUserButton:hover {
    background-color: #777;
}

.DeleteUserButton{
    background-color: #555;
    color: white;
    border: none;
    outline: none;
    cursor: pointer;
    padding: 7px 8px;
    font-size: 14px;
    width: 75px;
    border-radius: 25px;
}

.DeleteUserButton:hover {
    background-color: #777;
}

.AddUserBox{
    border:1px solid #e3e3e3;
    margin-bottom: 10px;
    box-shadow: 0 1px 1px rgba(0,0,0,0.1);
}

.AddUserBox:hover{
    border-color: #a0a0a0 #b9b9b9 #b9b9b9 #b9b9b9;
}

AddUserBox:focus{
    border-color:#4d90fe;
}

</style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="height:75px; background-color:#033459; position:relative; top: 0px; left: 0px;">         
             <asp:Image ID="AUGLogoLittle" runat="server" ImageUrl="~/Images/AugustaLittle.png" CssClass="AUGLogo" />
            <asp:Button ID="LogoutButton" runat="server" CssClass="tablink1" Text="Logout" OnClick="LogoutBtn_Click" />
            <asp:Button ID="AdminPageButton" runat="server" CssClass="tablink2" Text="Admin Page" />
            <asp:Button ID="StudentSearchButton" runat="server" CssClass="tablink3" Text="Student Search" OnClick="StudentSearchBtn_Click" />
        </div>
        <script language="javascript">
    function Validate() {
        return confirm('Are you sure you want to delete user?');
    }
</script>

    <div style="width:100%">
    <div style="float:left; width:50%; text-align:center">
        <br />
        <br />
                    <asp:Label ID="SearchUserLabel" runat="server" Font-Size="30px">Search User</asp:Label>
        <br />
        <br />
                    <asp:TextBox ID="SearchUserTextBox" CssClass="AddUserBox" runat="server" Font-Size="18px" Height="30px"> </asp:TextBox>
        <asp:Button ID="SearchUserButton" runat="server" Text ="Search User" OnClick="SearchUserButton_Click" CssClass="SearchUserButton"  />
        <br />
        <br />
        <asp:Label ID="Label0" runat="server" Visible="false"></asp:Label>
        <asp:Button ID="Button0" runat="server" Text="Delete" Visible="false" OnClick="DeleteButton_Click" CssClass="DeleteUserButton" OnClientClick="return Validate();" />
        <br />
        <asp:Label ID="Label1" runat="server" Visible="false"></asp:Label>
        <asp:Button ID="Button1" runat="server" Text="Delete" Visible="false" OnClick="DeleteButton_Click" CssClass="DeleteUserButton" OnClientClick="return Validate();" />
        <br />
        <asp:Label ID="Label2" runat="server" Visible="false"></asp:Label>
        <asp:Button ID="Button2" runat="server" Text="Delete" Visible="false" OnClick="DeleteButton_Click" CssClass="DeleteUserButton" OnClientClick="return Validate();" />
        <br />
        <asp:Label ID="Label3" runat="server" Visible="false"></asp:Label>
        <asp:Button ID="Button3" runat="server" Text="Delete" Visible="false" OnClick="DeleteButton_Click" CssClass="DeleteUserButton" OnClientClick="return Validate();" />
        <br />
        <asp:Label ID="Label4" runat="server" Visible="false"></asp:Label>
        <asp:Button ID="Button4" runat="server" Text="Delete" Visible="false" OnClick="DeleteButton_Click" CssClass="DeleteUserButton" OnClientClick="return Validate();" />
        <br />
        <asp:Label ID="Label5" runat="server" Visible="false"></asp:Label>
        <asp:Button ID="Button5" runat="server" Text="Delete" Visible="false" OnClick="DeleteButton_Click" CssClass="DeleteUserButton" OnClientClick="return Validate();" />
        <br />
        <asp:Label ID="Label6" runat="server" Visible="false"></asp:Label>
        <asp:Button ID="Button6" runat="server" Text="Delete" Visible="false" OnClick="DeleteButton_Click" CssClass="DeleteUserButton" OnClientClick="return Validate();" />
        <br />
        <asp:Label ID="Label7" runat="server" Visible="false"></asp:Label>
        <asp:Button ID="Button7" runat="server" Text="Delete" Visible="false" OnClick="DeleteButton_Click" CssClass="DeleteUserButton" OnClientClick="return Validate();" />
        <br />
        <asp:Label ID="Label8" runat="server" Visible="false"></asp:Label>
        <asp:Button ID="Button8" runat="server" Text="Delete" Visible="false" OnClick="DeleteButton_Click" CssClass="DeleteUserButton" OnClientClick="return Validate();" />
        <br />
        <asp:Label ID="Label9" runat="server" Visible="false"></asp:Label>
        <asp:Button ID="Button9" runat="server" Text="Delete" Visible="false" OnClick="DeleteButton_Click" CssClass="DeleteUserButton" OnClientClick="return Validate();" />
        <br />
        <asp:Label ID="Label10" runat="server" Visible="false"></asp:Label>
        <asp:Button ID="Button10" runat="server" Text="Delete" Visible="false" OnClick="DeleteButton_Click" CssClass="DeleteUserButton" OnClientClick="return Validate();" />
        <br />
        <asp:Label ID="Label11" runat="server" Visible="false"></asp:Label>
        <asp:Button ID="Button11" runat="server" Text="Delete" Visible="false" OnClick="DeleteButton_Click" CssClass="DeleteUserButton" OnClientClick="return Validate();" />
        <br />
        <asp:Label ID="Label12" runat="server" Visible="false"></asp:Label>
        <asp:Button ID="Button12" runat="server" Text="Delete" Visible="false" OnClick="DeleteButton_Click" CssClass="DeleteUserButton" OnClientClick="return Validate();" />
        <br />
        <asp:Label ID="Label13" runat="server" Visible="false"></asp:Label>
        <asp:Button ID="Button13" runat="server" Text="Delete" Visible="false" OnClick="DeleteButton_Click" CssClass="DeleteUserButton" OnClientClick="return Validate();" />
        <br />
        <asp:Label ID="Label14" runat="server" Visible="false"></asp:Label>
        <asp:Button ID="Button14" runat="server" Text="Delete" Visible="false" OnClick="DeleteButton_Click" CssClass="DeleteUserButton" OnClientClick="return Validate();" />
        <br />
        <asp:Label ID="Label15" runat="server" Visible="false"></asp:Label>
        <asp:Button ID="Button15" runat="server" Text="Delete" Visible="false" OnClick="DeleteButton_Click" CssClass="DeleteUserButton" OnClientClick="return Validate();" />
        <br />
        <asp:Label ID="Label16" runat="server" Visible="false"></asp:Label>
        <asp:Button ID="Button16" runat="server" Text="Delete" Visible="false" OnClick="DeleteButton_Click" CssClass="DeleteUserButton" OnClientClick="return Validate();" />
        <br />
        <asp:Label ID="Label17" runat="server" Visible="false"></asp:Label>
        <asp:Button ID="Button17" runat="server" Text="Delete" Visible="false" OnClick="DeleteButton_Click" CssClass="DeleteUserButton" OnClientClick="return Validate();" />
        <br />
        <asp:Label ID="Label18" runat="server" Visible="false"></asp:Label>
        <asp:Button ID="Button18" runat="server" Text="Delete" Visible="false" OnClick="DeleteButton_Click" CssClass="DeleteUserButton" OnClientClick="return Validate();" />
        <br />
        <asp:Label ID="Label19" runat="server" Visible="false"></asp:Label>
        <asp:Button ID="Button19" runat="server" Text="Delete" Visible="false" OnClick="DeleteButton_Click" CssClass="DeleteUserButton" OnClientClick="return Validate();" />
        <br />

        </div>
        <div style="float:right; width:50%; text-align:center">
            <br />
            <br />
            <asp:Label ID="AddUserLabel" runat="server" Font-Size="30px">Add User</asp:Label>
            <br />
            <br />
            <asp:Label ID="FistNameLabel" runat="server" Font-Size="18px" Width="100px">First Name: </asp:Label>
            <asp:TextBox ID="FirstNameTextbox" cssClass="AddUserBox" runat="server" Font-Size="14px" Height="25px" > </asp:TextBox>
            <asp:RequiredFieldValidator ID="vldtxtNewName" runat="server"
            ControlToValidate="FirstNameTextBox" ValidationGroup="AddUserGroup" 
            ErrorMessage="Required Field" ForeColor="Red" />
            <br />
            <asp:Label ID="LastNameLabel" runat="server" Font-Size="18px" Width="100px">Last Name: </asp:Label>
            <asp:TextBox ID="LastNameTextbox"  cssClass="AddUserBox" runat="server" Font-Size="14px" Height="25px"> </asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
            ControlToValidate="LastNameTextBox" ValidationGroup="AddUserGroup" 
            ErrorMessage="Required Field" ForeColor="Red" />
             <br />
            <asp:Label ID="UserNameLabel" runat="server" Font-Size="18px" Width="100px">Username: </asp:Label>
            <asp:TextBox ID="UserNameTextBox" cssClass="AddUserBox" runat="server" Font-Size="14px" Height="25px"> </asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
            ControlToValidate="UserNameTextBox" ValidationGroup="AddUserGroup" 
            ErrorMessage="Required Field" ForeColor="Red"/>
             <br />
            <asp:Label ID="PasswordLabel" runat="server" Font-Size="18px" Width="100px">Password: </asp:Label>
            <asp:TextBox ID="PasswordTextBox" cssClass="AddUserBox" runat="server" Font-Size="14px" Height="25px"> </asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
            ControlToValidate="PasswordTextbox" ValidationGroup="AddUserGroup" 
            ErrorMessage="Required Field" ForeColor="Red" />
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

