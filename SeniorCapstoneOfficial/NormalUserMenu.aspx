 <%@ Page Async ="true" Language="C#" AutoEventWireup="true" CodeBehind="NormalUserMenu.aspx.cs" Inherits="SeniorCapstoneOfficial.NormalUserMenu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Style.css" rel="stylesheet" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
   <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
  <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script src="https://cdn.polyfill.io/v2/polyfill.min.js?features=es6,Intl"></script>
    <link rel="stylesheet" href="https://cdn01.boxcdn.net/platform/elements/7.2.0/en-US/explorer.css" />



    <script type="text/javascript">
     $(document).ready(function () {
         $("#btnTop").click(function () {
             $("#pnl1").slideToggle("slow");
         });
     });
</script>

   <script>
  $( function() {
      var availableTags = testArray
    $( "#EmailAddress" ).autocomplete({
        source: function (request, response) {
            var filteredArray = $.map(availableTags, function (item)
            {                
                if (item.indexOf(request.term) == 0) {
                    return item;
                }
                else {
                    return null;
                }
            }
            );
            response(filteredArray);
        }
    });
  });

  //setting rightdiv height to parent's height
  $(document).ready(function () {
      $('#rightdiv').height($('#around').height());
      $("#inrightdiv").hide()
  });

       //showing recent events when clicking button

        $(document).ready(function()
        {
           
         $("#<%=RecentEventsButton.ClientID%>").click(function()
         {
             $("#inrightdiv").slideToggle(500);
             return false;
         });
     });
 
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

        .ExportButton {
            background-color: #555;
            color: white;
            border: none;
            outline: none;
            cursor: pointer;
            padding: 7px 8px;
            font-size: 17px;
            width: 140px;
            position: absolute;
            right: 15%;
            border-radius: 25px;
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

#around:after{
    content: "";
    display: table;
    clear: both;
}
#around, #around2{
     margin-top: 50px;
     text-align: center;
     overflow: hidden;
     position: relative;
     width: 100%;
}
#rightdiv{
    text-align:center; 
    width:49%; 
    float:right;
    height: 100%;
}
#rightside{
     text-align:center; 
    width:49%; 
    float:right;
    height: 100%;
    margin-top:10px;
}
#inrightdiv{
    margin-top: 50px;
}

#leftside{
    text-align:center; 
    width:49%; 
    float:left;
    margin-top: 10px;
  
}
.aliaseserror{
    color: red;
    font-size: 14px;
}

</style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
         <div style="height:75px; background-color:#033459; position:relative;">         
             <asp:ImageButton ID="AUGLogoLittle" OnClick="ImageButton_Click" runat="server" ImageUrl="~/Images/AugustaLittle.png" CssClass="AUGLogo" />
            <asp:Button ID="LogoutButton" runat="server" CssClass="tablink1" Text="Logout" OnClick="LogoutBtn_Click" />
            <asp:Button ID="AdminPageButton" runat="server" CssClass="tablink2" Text="Admin Page" OnClick="AdminPage_Click" />
            <asp:Button ID="StudentSearchButton" runat="server" CssClass="tablink3" Text="Student Search" OnClick="StudentSearchButton_Click"/>
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
        <div id="around">
            <asp:Label ID="InvalidEmailLabel" runat="server" Text="Student Not Found"></asp:Label> <br />
            <div style="text-align:center; width:49%; float:left">
          <div id ="around2">
              <div id="leftside">
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
            </div>              
       
                <div id="rightside"> 
                    <asp:ImageButton ID="btnTop" runat="server" ImageUrl="~/Images/plus.png" Visible="true" OnClientClick="return false"/>
                <asp:Label ID="Label7" runat="server" Visible="False"></asp:Label>
                <br />          
            <br />
      <div id="pnl1" style="text-align:left; display:none;">
      <asp:PlaceHolder ID="FolderPH" runat="server"></asp:PlaceHolder>
          <div class="container" style="height:600px"></div>
    <!-- Latest version of the picker js for your locale -->
    <script src="https://cdn01.boxcdn.net/platform/elements/7.2.0/en-US/explorer.js"></script>
    <script>
      	var folderId = '0';
      	var accessToken = 'pCHDu9hTeqnQN8eKggQQTgBmff2Cntor';
      	var filePicker = new Box.ContentExplorer();
      	filePicker.show(folderId, accessToken, {
            container: '.container'
        });
    </script>
</div>
                 </div>
             
              </div>
                <br />
                <br />
                <asp:TextBox ID="TextBox1" runat="server" Height="20px" Visible="False" Width="160px" placeholder="Enter Email Alias" CssClass="AddUserBox"></asp:TextBox>
                
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="Button7" runat="server" Visible="false" OnClick="AddButton_Click" CssClass="DeleteUserButton" Text="Add" />
                <asp:Label ID="Aliaseserror" runat="server" CssClass= "aliaseserror" Text="Invalid Email" Visible="false"></asp:Label>
                <br />
                <br />
                <asp:Panel ID="Panel1" runat="server" style="height:200px;width:400px;overflow:visible;border:1px; margin-left:30%;" Visible="False">
                    <asp:Label ID="Label8" runat="server" Visible="False"></asp:Label>
                    <asp:Button ID="Button8" runat="server" Visible="false" OnClick="DeleteButton_Click" CssClass="DeleteUserButton" Text="Delete" />
                    <br />
                    <br />
                    <asp:Label ID="Label9" runat="server" Visible="False"></asp:Label>
                    <asp:Button ID="Button9" runat="server" CssClass="DeleteUserButton" OnClick="DeleteButton_Click" Text="Delete" Visible="false" />
                    <br />
                    <br />
                    <asp:Label ID="Label10" runat="server" Visible="False"></asp:Label>
                    <asp:Button ID="Button10" runat="server" CssClass="DeleteUserButton" OnClick="DeleteButton_Click" Text="Delete" Visible="false" />
                    <br />
                    <br />
                    <asp:Label ID="Label11" runat="server" Visible="False"></asp:Label>
                    <asp:Button ID="Button11" runat="server" CssClass="DeleteUserButton" OnClick="DeleteButton_Click" Text="Delete" Visible="false" />
                    <br />
                    <br />
                    <asp:Label ID="Label12" runat="server" Visible="False"></asp:Label>
                    <asp:Button ID="Button12" runat="server" CssClass="DeleteUserButton" OnClick="DeleteButton_Click" Text="Delete" Visible="false" />
                    <br />
                    <br />
                    <asp:Label ID="Label13" runat="server" Visible="False"></asp:Label>
                    <asp:Button ID="Button13" runat="server" CssClass="DeleteUserButton" OnClick="DeleteButton_Click" Text="Delete" Visible="false" />
                    <br />
                    <br />
                    <asp:Label ID="Label14" runat="server" Visible="False"></asp:Label>
                    <asp:Button ID="Button14" runat="server" CssClass="DeleteUserButton" OnClick="DeleteButton_Click" Text="Delete" Visible="false" />
                    <br />
                    <br />
                    <asp:Label ID="Label15" runat="server" Visible="False"></asp:Label>
                    <asp:Button ID="Button15" runat="server" CssClass="DeleteUserButton" OnClick="DeleteButton_Click" Text="Delete" Visible="false" />
                    <br />
                    <br />
                    <asp:Label ID="Label16" runat="server" Visible="False"></asp:Label>
                    <asp:Button ID="Button16" runat="server" CssClass="DeleteUserButton" OnClick="DeleteButton_Click" Text="Delete" Visible="false" />
                    <br />
                    <br />
                    <asp:Label ID="Label17" runat="server" Visible="False"></asp:Label>
                    <asp:Button ID="Button17" runat="server" CssClass="DeleteUserButton" OnClick="DeleteButton_Click" Text="Delete" Visible="false" />
                    <br />
                    <br />
                    <asp:Label ID="Label18" runat="server" Visible="False"></asp:Label>
                    <asp:Button ID="Button18" runat="server" CssClass="DeleteUserButton" OnClick="DeleteButton_Click" Text="Delete" Visible="false" />
                    <br />
                    <br />
                    <asp:Label ID="Label19" runat="server" Visible="False"></asp:Label>
                    <asp:Button ID="Button19" runat="server" CssClass="DeleteUserButton" OnClick="DeleteButton_Click" Text="Delete" Visible="false" />
                    <br />
                    <br />
                    <asp:Label ID="Label20" runat="server" Visible="False"></asp:Label>
                    <asp:Button ID="Button20" runat="server" CssClass="DeleteUserButton" OnClick="DeleteButton_Click" Text="Delete" Visible="false" />
                    <br />
                    <br />
                    <asp:Label ID="Label21" runat="server" Visible="False"></asp:Label>
                    <asp:Button ID="Button21" runat="server" CssClass="DeleteUserButton" OnClick="DeleteButton_Click" Text="Delete" Visible="false" />
                    <br />
                    <br />
                    <asp:Label ID="Label22" runat="server" Visible="False"></asp:Label>
                    <asp:Button ID="Button22" runat="server" CssClass="DeleteUserButton" OnClick="DeleteButton_Click" Text="Delete" Visible="false" />
                    <br />
                    <br />
                    <asp:Label ID="Label23" runat="server" Visible="False"></asp:Label>
                    <asp:Button ID="Button23" runat="server" CssClass="DeleteUserButton" OnClick="DeleteButton_Click" Text="Delete" Visible="false" />
                    <br />
                    <br />
                    <asp:Label ID="Label24" runat="server" Visible="False"></asp:Label>
                    <asp:Button ID="Button24" runat="server" CssClass="DeleteUserButton" OnClick="DeleteButton_Click" Text="Delete" Visible="false" />
                    <br />
                    <br />
                    <asp:Label ID="Label25" runat="server" Visible="False"></asp:Label>
                    <asp:Button ID="Button25" runat="server" CssClass="DeleteUserButton" OnClick="DeleteButton_Click" Text="Delete" Visible="false" />
                    <br />
                    <asp:Label ID="Label26" runat="server" Visible="False"></asp:Label>
                    <asp:Button ID="Button26" runat="server" CssClass="DeleteUserButton" OnClick="DeleteButton_Click" Text="Delete" Visible="false" />
                    <br />
                    <br />
                    <asp:Label ID="Label27" runat="server" Visible="False"></asp:Label>
                    <asp:Button ID="Button27" runat="server" CssClass="DeleteUserButton" OnClick="DeleteButton_Click" Text="Delete" Visible="false" />
                    <br />
                    <br />
                    <asp:Label ID="Label28" runat="server" Visible="False"></asp:Label>
                    <asp:Button ID="Button28" runat="server" CssClass="DeleteUserButton" OnClick="DeleteButton_Click" Text="Delete" Visible="false" />
                    <br />
                    <br />
                </asp:Panel>
        <br />
        
        <br />
        </div>
              <div id="rightdiv" runat="server">
                 <asp:Button ID="RecentEventsButton" CssClass="ExportButton" runat="server" Text="Recent Events" Visible="False" OnClick="Recent_Click"/>
            <br />
             <div id="inrightdiv" runat="server">
             <asp:PlaceHolder ID="EventHolder" runat="server"></asp:PlaceHolder>
             </div>
            
               </div>
        </div>

        </div>
    </form>
</body>
</html>
