<%@ Page Async ="true" Language="C#" AutoEventWireup="true" CodeBehind="NormalUserMenu.aspx.cs" Inherits="SeniorCapstoneOfficial.NormalUserMenu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Style.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
         <br />
        <asp:Button ID="SearchForStudent" runat="server" Text="Search Student" OnClick="SearchForStudent_Click" />
        <asp:TextBox ID="EmailAddress" runat="server" ></asp:TextBox>
         <asp:GridView ID="GridView1" runat="server"></asp:GridView>
        <asp:Label ID="Label1" runat="server"></asp:Label>
    </div>
        
        <p>
         <asp:Label ID="Label2" runat="server"></asp:Label>
         </p>
        <p>
         <asp:Label ID="Label3" runat="server"></asp:Label>
        </p>
          <p>
         <asp:Label ID="Label4" runat="server"></asp:Label>
        </p>
        <p>
          <asp:Button ID="Exportbtn" runat="server" Text="Export" OnClick="Exportbtn_Click" /> 
        </p>
        <p>
        <asp:Label ID="InvalidEmailLabel" runat="server" Text="Student Not Found"></asp:Label> <br />
         <asp:Button ID="LogoutBtn" runat="server" Text="Logout" OnClick="LogoutBtn_Click" /> 
            <asp:Button ID="AdminPage" runat="server" Text="Admin Page" OnClick="AdminPage_Click" />
        </p>
    </form>
</body>
</html>
