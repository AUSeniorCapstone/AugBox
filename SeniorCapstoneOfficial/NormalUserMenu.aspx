 <%@ Page Async ="true" Language="C#" AutoEventWireup="true" CodeBehind="NormalUserMenu.aspx.cs" Inherits="SeniorCapstoneOfficial.NormalUserMenu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Style.css" rel="stylesheet" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
   <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
  <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
      
  <script>
  $( function() {
      var availableTags = testArray
    $( "#EmailAddress" ).autocomplete({
        source: availableTags
    });
  } );
  </script>
    <style>
body {font-family: "Lato", sans-serif;
      overflow-x: hidden;
}
.ui-helper-hidden-accessible { display:none;}
.ui-autocomplete { 
     max-height: 200px;
     width: 250px;
     overflow-y: auto; 
     overflow-x: hidden;  
     margin-top: 20px;
     border: 1px solid;
     border-color: #c6c1c1;

}
.ui-state-active 
{
  width: inherit;
  background-color: #c6c1c1;
  cursor: pointer;
}
.ui-menu-item a {
text-decoration: none;
display: block;
line-height: 1.5;
zoom: 1;
}
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

.SearchUserButton1{
    background-color: #555;
    color: white;
    border: none;
    outline: none;
    cursor: pointer;
    padding: 7px 8px;
    font-size: 17px;
    width: 140px;
    border-radius: 25px;
}
.SearchUserButton1:hover {
    background-color: #777;
}

.ExportButton{
    background-color: #555;
    color: white;
    border: none;
    outline: none;
    cursor: pointer;
    padding: 7px 8px;
    font-size: 17px;
    width: 140px;
    position:absolute;
    right:15%;
    border-radius: 25px;
}
.ExportButton:hover {
    background-color: #777;
}
.AddUserBox{
    padding-right: 10px;
    border:1px solid #e3e3e3;
    margin-bottom: 10px;
    box-shadow: 0 1px 1px rgba(0,0,0,0.1);
    width: 250px;
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
        <div style="text-align:center; position:relative;" >
        <asp:TextBox ID="EmailAddress" CssClass="AddUserBox" runat="server" Font-Size="18px" Height="30px" AutoPostBack="False"></asp:TextBox> 
        <asp:Button ID="SearchForStudent" runat="server" Text="Search Student" CssClass="SearchUserButton1" OnClick="SearchForStudent_Click" onkeyup="this.value" />
        <asp:Button ID="Exportbtn" runat="server" Text="Export All Users" OnClick="Exportbtn_Click"  CssClass="ExportButton"/> 
            </div>
            <div style="text-align:center">
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
        <asp:Label ID="InvalidEmailLabel" runat="server" Text="Student Not Found"></asp:Label> <br />
        <br />
        </div>
        </div>
    </form>
</body>
</html>
