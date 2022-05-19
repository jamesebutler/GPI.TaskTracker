// JScript File

// JScript File
var Recurrence = {
   
    setRadioButton: function (ctl,userCtl, facilityBtn) {
        if (ctl !== null) {
            var plantcode = ctl.value;
            SiteDropDownsWS.GetDDLData(plantcode, userCtl,this.OnSucceeded, this.OnFailed);
            try{$get(facilityBtn).innerText=ctl.options[ctl.selectedIndex].text}catch(e){}
            //document.myform.mylist.options[w].text;

        } 
    },

    ResponsibleUserIsSelected: function(sender, args){
        if (sender!==null){
            if (args.Value===""){
                args.IsValid = false;
                sender.IsValid=false;    
            }
            else{
                args.IsValid = true;
                sender.IsValid=true;    
            }
        }
    },
    
    SetResponsibleUser: function (ctl) {
        if (ctl !== null) {
            var target = $get(_txtTargetID);
            var user = $get(target.value); //"_txtResponsibleUser");
            var userName = $get(_txtResponsibleUserName);
           // jQuery(target.value).selectRange(0, 10);
            if (user !== null && userName !== null) {
                var fullName = jQuery.trim(ctl.options[ctl.selectedIndex].text).split(',');
                if (fullName.length >= 2) {
                    user.value = fullName[1] + ' ' + fullName[0] + this.addSpace('|',500) + jQuery.trim(ctl.options[ctl.selectedIndex].value);
                }
                else {
                    user.value = jQuery.trim(ctl.options[ctl.selectedIndex].text)  ;
                }
                user.title = jQuery.trim(ctl.options[ctl.selectedIndex].value);
                userName.value = ctl.value;
            }
            $find('_bhResponsibleUser').hidePopup();
        }
    },
    
    ClearResponsibleUser: function (){
         var target = $get(_txtTargetID);
         var user = $get(target.value); //"_txtResponsibleUser");
         var userName = $get(_txtResponsibleUserName);
         
         if (user!==null && userName!==null){
            userName.value='';
            user.title='';
            user.value='';            
         }
         $find('_bhResponsibleUser').hidePopup();
    },

    ShowResponsibleUser: function (ctl) {
        //var mouse = this.captureMousePosition;
        var target = $get(_txtTargetID);
        target.value = ctl.id;
        event.cancelBubble = true;
        var ddlCategory = $get(_ddlResponsibleUser);
        ddlCategory.selectedIndex = -1;
        var popExtender = $find('_bhResponsibleUser');
        var pos = this.findPos(target);
        var posctl = this.findPos(ctl);
        popExtender.set_OffsetY(posctl[1]); //ctl.offsetTop + 80 + document.body.scrollTop);
        popExtender.set_OffsetX(posctl[0]);
            
        popExtender.set_OffsetY(pos[1]); //ctl.offsetTop + 80 + document.body.scrollTop);
        popExtender.set_OffsetX(pos[0]);
            
        popExtender.set_OffsetY(this.getTop(ctl));
        popExtender.showPopup();

    },

    getTop: function (e) {
        var offset = e.offsetTop;
        if (e.offsetParent !== null) {
            offset += this.getTop(e.offsetParent);
        }
        return offset;
    },

    getLeft: function (e) {
        var offset = e.offsetLeft;
        if (e.offsetParent !== null) {
            offset += this.getLeft(e.offsetParent);
        }
        return offset;
    }, 

    OnSucceeded: function (data) {
       var ddlUser;
       var i;
        if (data!==null){
            ddlUser = $get(data[0].Text);//(_ddlResponsibleUser);
        }
        
        
        if (ddlUser !== null) {        
            ResponsibleUser.ClearOptionsFastAlt(ddlUser.id); // ddlCategory.options.length=0;
            for (i = 1; i < data.length; ++i) {
                ResponsibleUser.addOption(ddlUser, data[i]);
            }
            ddlUser.selectedIndex = 1;
            ddlUser.focus();
        }
        return data;
    },

    ClearOptionsFastAlt: function (id) {
        document.getElementById(id).innerHTML = "";
    },

    addOption: function (selectbox, datavalue) {
        var data = datavalue;
        var optn;
        if (data.Text.length > 0 && data.Value.length > 0) {
            optn = document.createElement("OPTION");
            optn.text = data.Text;
            optn.value = data.Value;
            if (data.Attributes.CssStyle.Value !== null) {
                var resource = data.Attributes.CssStyle.Value.split(';');
                if (resource !== null) {
                    for (i = 0; i < resource.length; i++) {
                        if (resource[i] !== null) {
                            var attr = resource[i].split(':');
                            if (attr.length >= 2) {
                                if (attr[0] === "background-color") {
                                    optn.runtimeStyle.backgroundColor = attr[1];
                                    optn.runtimeStyle.fontWeight = 'bold';
                                }
                            }
                        }
                    }
                }
            }
            selectbox.options.add(optn);
        }
        else if (data.Value.length > 0) {

            optn = document.createElement("OPTION");
            optn.text = data.Text;
            optn.value = data.Value;
            optn.disabled = true;
            selectbox.options.add(optn);
        }
    },

    OnFailed: function (error) {
        // Alert user to the error.
        alert(error.get_message());
    },
    
    addSpace:function(argvalue, numlength) {
      var i;
      if (numlength <= 0)
        {numlength = 10;}

      if (argvalue.length < numlength) {
        for(i = argvalue.length; i < numlength; i++)
          {argvalue = " " + argvalue;}
      }

      return argvalue;

    },
    
    findPos:function(obj) {
	    var curleft = curtop = 0;
        if (obj.offsetParent) {
            do {
			    curleft += obj.offsetLeft;
			    curtop += obj.offsetTop;
            } while (obj = obj.offsetParent);
        }
            return [curleft,curtop];
    },
    
    captureMousePosition:function(e) {
    if (document.layers) {
        // When the page scrolls in Netscape, the event's mouse position
        // reflects the absolute position on the screen. innerHight/Width
        // is the position from the top/left of the screen that the user is
        // looking at. pageX/YOffset is the amount that the user has
        // scrolled into the page. So the values will be in relation to
        // each other as the total offsets into the page, no matter if
        // the user has scrolled or not.
        this.xMousePos = e.pageX;
        this.yMousePos = e.pageY;
        this.xMousePosMax = window.innerWidth+window.pageXOffset;
        this.yMousePosMax = window.innerHeight+window.pageYOffset;
    } else if (document.all) {
        // When the page scrolls in IE, the event's mouse position
        // reflects the position from the top/left of the screen the
        // user is looking at. scrollLeft/Top is the amount the user
        // has scrolled into the page. clientWidth/Height is the height/
        // width of the current page the user is looking at. So, to be
        // consistent with Netscape (above), add the scroll offsets to
        // both so we end up with an absolute value on the page, no
        // matter if the user has scrolled or not.
        this.xMousePos = window.event.x+document.body.scrollLeft;
        this.yMousePos = window.event.y+document.body.scrollTop;
        this.xMousePosMax = document.body.clientWidth+document.body.scrollLeft;
        this.yMousePosMax = document.body.clientHeight+document.body.scrollTop;
    } else if (document.getElementById) {
        // Netscape 6 behaves the same as Netscape 4 in this regard
        this.xMousePos = e.pageX;
        this.yMousePos = e.pageY;
        this.xMousePosMax = window.innerWidth+window.pageXOffset;
        this.yMousePosMax = window.innerHeight+window.pageYOffset;
    }
    window.status = "xMousePos=" + this.xMousePos + ", yMousePos=" + this.yMousePos + ", xMousePosMax=" + this.xMousePosMax + ", yMousePosMax=" + this.yMousePosMax;
},

ValidateCheckBoxList:function(sender, args)
    {
        if (args.Value.length>0){
            args.IsValid=true;
            sender.IsValid=true;}
        else{
	        args.IsValid = false;
	        sender.IsValid=false;}
	    return
    }


};