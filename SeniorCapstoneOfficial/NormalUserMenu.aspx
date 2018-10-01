<%@ Page Async ="true" Language="C#" AutoEventWireup="true" CodeBehind="NormalUserMenu.aspx.cs" Inherits="SeniorCapstoneOfficial.NormalUserMenu" %>

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
    width: 130px;
}
.SearchUserButton:hover {
    background-color: #777;
}

</style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
         <div style="height:75px; background-color:#033459; position:relative;">         
             <asp:Image ID="AUGLogoLittle" runat="server" ImageUrl="~/Images/AugustaLittle.png" CssClass="AUGLogo" />
            <asp:Button ID="LogoutButton" runat="server" CssClass="tablink1" Text="Logout" OnClick="LogoutBtn_Click" />
            <asp:Button ID="AdminPageButton" runat="server" CssClass="tablink2" Text="Admin Page" OnClick="AdminPage_Click" />
            <asp:Button ID="StudentSearchButton" runat="server" CssClass="tablink3" Text="Student Search" />
        </div>
         <br />
        <br />
        <div style="text-align:center">
            <asp:Label runat="server" Text="Student Search" Font-Size="37px"></asp:Label>
        </div>
        <br />
        <br />
        <div style="text-align:center">
        <asp:Button ID="SearchForStudent" runat="server" Text="Search Student" CssClass="SearchUserButton" OnClick="SearchForStudent_Click" />
        <asp:TextBox ID="EmailAddress" runat="server" Font-Size="18px" Height="33px" ></asp:TextBox> 
            <br />
            <br />
         <asp:Label ID="Label1" runat="server"></asp:Label>
        <br />
        <br />       
         <asp:Label ID="Label2" runat="server"></asp:Label>
        <br />
        <br />
         <asp:Label ID="Label3" runat="server"></asp:Label>
        <br />
        <br />
         <asp:Label ID="Label4" runat="server"></asp:Label>
        <br />
        <br />
            <asp:Label ID="Label5" runat="server"></asp:Label>
        <br />
        <br />
            <asp:PlaceHolder ID="FolderPH" runat="server"></asp:PlaceHolder>
        <br />
        <br />
          <asp:Button ID="Exportbtn" runat="server" Text="Export" OnClick="Exportbtn_Click"  CssClass="SearchUserButton"/> 
            <br />
            <br />
        <asp:Label ID="InvalidEmailLabel" runat="server" Text="Student Not Found"></asp:Label> <br />
        <br />
        </div>
    </form>
</body>
</html>
