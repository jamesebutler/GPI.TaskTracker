// JScript File
var MultiFileUpload = {
    toggleAttachments:function(fileId,linkId,value) 
    {
        var fileId = document.getElementById(fileId);
        var linkId = document.getElementById(linkId);
        if (fileId!==null && linkId!==null){
            if (value===1){
                fileId.style.display='none';
                linkId.style.display='';
            }
            else{
                fileId.style.display='';
                linkId.style.display='none';
            }
        }
    }
,

is_URL_Available:function(url)
	    {
	        var script = document.createElement("script");
	        script.src = url;
	        script.type = "text/javascript";
	        var head = document.getElementsByTagName("head")[0];
	        head.appendChild(script);
	        if (self.getstatus) {
	            head.removeChild(script);
	            return  true;
	          } else {
	            head.removeChild(script);
	            return false;
	          }
	    }
}