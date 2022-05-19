<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DetectScreenResolution.aspx.vb"
    Inherits="DetectScreenResolution" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
     
</head>
<body>
    <form id="form1" runat="server">
        <div>
           <script type="text/javascript" language="javascript">
           var detectSession = '<%=Session("ScreenResolution") %>'; 
           if (detectSession==null||detectSession==''){
                res = "&res="+screen.width+"x"+screen.height+"&d="+screen.colorDepth;
               // top.location.href='<%=Session("DetectScreenResolutionURL")%>'+'&action=set'+res;//"DetectScreenResolution.aspx?action=set"+res;
            }
            </script>
        </div>
    </form>
</body>
</html>
