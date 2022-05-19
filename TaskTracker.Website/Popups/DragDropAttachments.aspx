<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DragDropAttachments.aspx.vb" Inherits="Popups_DragDropAttachments" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
    <script type="text/javascript" src="https://rawgit.com/enyo/dropzone/master/dist/dropzone.js"></script>
    <link rel="stylesheet" href="https://rawgit.com/enyo/dropzone/master/dist/dropzone.css" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <style>
        .dropzone {
            height: 100%;
            width: 100%;
        }
    </style>
    <script type="text/javascript">
        // Prevent Dropzone from auto discovering this element:
        Dropzone.options.myAwesomeDropzone = true;
        // This is useful when you want to create the
        // Dropzone programmatically later

        // Disable auto discover for all elements:
        Dropzone.autoDiscover = true;
    </script>
</head>
<body>
    <form id="form1" runat="server" class="dropzone">
        
        <div>
             <div class="dz-default dz-message">
                 <asp:Label ID="_lblDropZoneMessage" runat="server" Text="<%$IPResources:Global,Drag Files or Click Here to Upload%>"></asp:Label>
             </div>
            <div class="fallback">
                <input name="file" type="file" multiple />
            </div>
            
        </div>
    </form>
</body>

</html>
