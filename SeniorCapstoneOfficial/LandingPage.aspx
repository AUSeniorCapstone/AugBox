<%@ Page Async ="true" Language="C#" AutoEventWireup="true" CodeBehind="LandingPage.aspx.cs" Inherits="SeniorCapstoneOfficial.LandingPage" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <div style="height:75px; background-color:#033459; position:relative; top: 0px; left: 0px;">         
             <asp:Image ID="AUGLogoLittle" runat="server" ImageUrl="~/Images/AugustaLittle.png" CssClass="AUGLogo" />
            <asp:Button ID="LogoutButton" runat="server" CssClass="tablink1" Text="Logout" OnClick="LogoutBtn_Click" />
            <asp:Button ID="AdminPageButton" runat="server" CssClass="tablink2" Text="Admin Page" OnClick="AdminPage_Click" />
            <asp:Button ID="StudentSearchButton" runat="server" CssClass="tablink3" Text="Student Search" OnClick="StudentSearchBtn_Click" />
        </div>
        <div>
             <div style="width:100%">
    <div style="float:left; width:50%; text-align:center">
    <h2>
        Last Logins
    </h2>
        <div>
            <asp:PlaceHolder runat="server" ID="LoginChart"></asp:PlaceHolder>
        </div>
    </div>
  
                 <div style="float: right; width: 50%; text-align: center">
                                             <h2>
        Top Five Users (# of Logins)
    </h2>
                     <asp:PlaceHolder runat="server" ID="topChart"></asp:PlaceHolder>
                 </div>
                 
        </div>
    </div>

    </form>
</body>
</html>
