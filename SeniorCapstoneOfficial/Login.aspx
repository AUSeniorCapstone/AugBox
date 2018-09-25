<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SeniorCapstoneOfficial.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>Login</title>
    <link href="Style.css" rel="stylesheet" />
    <style>
        @import url('https://fonts.googleapis.com/family-Bitter|Crete+Round|Pacifico');
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <section>
         <div class="container">
             <div class="inner1">
                 <span id="spanid"><img src="Images/Augusta.png" alt="Augusta logo" /></span>
             </div>
             <div class="inner2">
                 <h3>AugBox</h3>                   
                 <asp:TextBox ID="txturname" placeholder="Username" runat="server"></asp:TextBox>
                 <asp:TextBox ID="txtpassword" placeholder="Password" TextMode="Password" runat="server"></asp:TextBox>
                 <asp:Label ID="lblErrorMessage" runat="server" Text="Incorrect User Credentials"></asp:Label>
                 <asp:Button ID="Button1" runat="server" CssClass="btn" Text="Submit" OnClick="Button1_Click" />
             </div>
         </div>
     </section>           
    </form>       
</body>
</html>
