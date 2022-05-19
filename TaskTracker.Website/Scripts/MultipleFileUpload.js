// JScript File

//function addFileUploadBox()
//{
//    if (!document.getElementById || !document.createElement)
//        return false;
//		
//    var uploadArea = document.getElementById ("upload-area");
//	
//    if (!uploadArea)
//        return;

//    var newLine = document.createElement ("br");
//    uploadArea.appendChild (newLine);
//    uploadArea.appendChild (newLine);
//	
//	var startDiv = document.createElement ("div");
//	//var endDiv = document.createElement ("</div>");
//    var newUploadBox = document.createElement ("input");
//   
//    // Set up the new input for file uploads
//    newUploadBox.type = "file";
//    newUploadBox.size = "70";
//    //newUploadBox.style="display:none;"
//	
//    // The new box needs a name and an ID
//    if (!addFileUploadBox.lastAssignedId)
//        addFileUploadBox.lastAssignedId = 100;

//    
//    startDiv.setAttribute("style","position: absolute; top: " +  (addFileUploadBox.lastAssignedId - 99) * 30 + "px; left: 0px; z-index: 99;");
//    startDiv.innerHTML="<input name='txtFile" + addFileUploadBox.lastAssignedId + "' type='text' id='txtFile" + addFileUploadBox.lastAssignedId + "' style='width: 520px;' /><input type='submit' name'ctl00$_cphMain$_btnFileUpload' value='Browse' onclick='Javascript:triggerFileUpload(" + addFileUploadBox.lastAssignedId +");return false;' id='ctl00__cphMain__btnFileUpload' class='Button'  style='cursor: hand;' />";
//	uploadArea.appendChild (startDiv);
//		    
//    newUploadBox.setAttribute ("id", "dynamic" + addFileUploadBox.lastAssignedId);
//    newUploadBox.setAttribute ("name", "dynamic" + addFileUploadBox.lastAssignedId);
//    newUploadBox.setAttribute ("style", "z-index: 100;position: absolute; -moz-opacity: 0; filter: alpha(opacity: 0); cursor: hand;");
//    newUploadBox.setAttribute ("onchange","FileSelected(this,"+addFileUploadBox.lastAssignedId+");");
//    uploadArea.appendChild (newUploadBox);
//    //uploadArea.appendChild (endDiv);
//    addFileUploadBox.lastAssignedId++;
//}

//function triggerFileUpload(id)
//	  {
//	    var fileUp = document.getElementById("dynamic"+id);
//	    
//		if (fileUp!=null){
//		    fileUp.focus();
//		    fileUp.click();
//		    window.focus();
//		    return true;
//		}
//		else{
//		    alert('null');
//		}
//	  }
//function FileSelected(obj,id){
//    if (obj!=null){
//        var txt = document.getElementById("txtFile"+id);
//        if (txt!=null){
//            txt.value=obj.value;
//            //obj.value="";
//        }
//    }
//}

var MultiFileUpload = {
 TriggerFileUpload:function()
 {
   //var lstFileList = document.getElementById('lstFileList');
   var tdFileInputs = document.getElementById('tdFileInputs');
   var fileInput = tdFileInputs.lastChild;
  
    if(fileInput.nodeName === '#text')
       {
            fileInput = fileInput.previousSibling
       }
        var fileUp = fileInput;//document.getElementById("fileInput");
		if (fileUp!==null){
		    //fileUp.focus();
		    fileUp.click();
		    //window.focus();
		    return true;
		}
 },

 AddUploadFile:function()
       {
           var lstFileList = document.getElementById('lstFileList');
           var tdFileInputs = document.getElementById('tdFileInputs');
           var fileInput = tdFileInputs.lastChild;
           if(fileInput.nodeName === '#text')
           {
                fileInput = fileInput.previousSibling
           }
           // Make sure we have a selected file
           if(fileInput.value === null || fileInput.value.length === 0)
               return;
           //////////////////////////////////////////////////////////////////////
           //////// CODE BY QUANG VO: http://connectionstringexamples.com ///////
           /////////EMAIL:            quangvo@qvis.com.au                 ///////
           //////////////////////////////////////////////////////////////////////
           // Create a new file input
           var newFileInput = fileInput.cloneNode(false);
           newFileInput.value = null;
           newFileInput.id += 'A'; // A unique id
           newFileInput.name = newFileInput.id;
           tdFileInputs.appendChild(newFileInput);
           // Hide the file input and add an entry to the select box
          fileInput.style.display = 'none';
           lstFileList.options[lstFileList.options.length] = new Option(fileInput.value, fileInput.id);
       },
       
       RemoveUploadFile:function()
       {
           var tdFileInputs = document.getElementById('tdFileInputs');
           var lstFileList = document.getElementById('lstFileList');
           // Make sure we have a selection
           if(lstFileList.options.length === 0 || lstFileList.selectedIndex < 0)
               return;
           //////////////////////////////////////////////////////////////////////
           //////// CODE BY QUANG VO: http://connectionstringexamples.com ///////
           /////////EMAIL:            quangvo@qvis.com.au                 ///////
           //////////////////////////////////////////////////////////////////////
           // Remove the option from the select file input element specified by the option
           var fileInputId = lstFileList.options[lstFileList.selectedIndex].value;
           lstFileList.options[lstFileList.selectedIndex] = null;
           var fileInput = document.getElementById(fileInputId);
           tdFileInputs.removeChild(fileInput);
       }
  }