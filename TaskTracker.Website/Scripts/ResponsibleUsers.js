var ResponsibleUser={populateResponsibleUsers:function(a,c){if(a!==null){var b=a.value;SiteDropDownsWS.GetDDLData(b,c,this.OnSucceeded,this.OnFailed)}},SetResponsibleUser:function(a){if(a!==null){var e=$get(_txtTargetID),b=$get(e.value),d=$get(_txtResponsibleUserName);if(b!==null&&d!==null){var c=jQuery.trim(a.options[a.selectedIndex].text).split(",");if(c.length>=2)b.value=c[1]+" "+c[0]+this.addSpace("|",500)+jQuery.trim(a.options[a.selectedIndex].value);else b.value=jQuery.trim(a.options[a.selectedIndex].text);b.title=jQuery.trim(a.options[a.selectedIndex].value);d.value=a.value}$find("_bhResponsibleUser").hidePopup()}},ShowResponsibleUser:function(b){var d=$get(_txtTargetID);d.value=b.id;event.cancelBubble=true;var c=$get(_ddlResponsibleUser);c.selectedIndex=-1;var a=$find("_bhResponsibleUser");a.set_OffsetY(this.getTop(b)+10);a.set_OffsetX(this.getLeft(b)+10);a.showPopup()},getTop:function(a){var b=a.offsetTop;if(a.offsetParent!==null)b+=this.getTop(a.offsetParent);return b},getLeft:function(a){var b=a.offsetLeft;if(a.offsetParent!==null)b+=this.getLeft(a.offsetParent);return b},OnSucceeded:function(b){var a,c;if(b!==null)a=$get(b[0].Text);if(a!==null){ResponsibleUser.ClearOptionsFastAlt(a.id);for(c=1;c<b.length;++c)ResponsibleUser.addOption(a,b[c]);a.selectedIndex=-1}return b},ClearOptionsFastAlt:function(a){document.getElementById(a).innerHTML=""},addOption:function(e,f){var b=f,a;if(b.Text.length>0&&b.Value.length>0){a=document.createElement("OPTION");a.text=b.Text;a.value=b.Value;if(b.Attributes.CssStyle.Value!==null){var c=b.Attributes.CssStyle.Value.split(";");if(c!==null)for(i=0;i<c.length;i++)if(c[i]!==null){var d=c[i].split(":");if(d.length>=2)if(d[0]=="background-color"){a.runtimeStyle.backgroundColor=d[1];a.runtimeStyle.fontWeight="bold"}}}e.options.add(a)}else if(b.Value.length>0){a=document.createElement("OPTION");a.text=b.Text;a.value=b.Value;a.disabled=true;e.options.add(a)}},OnFailed:function(a){alert(a.get_message())},addSpace:function(a,b){var c;if(b<=0)b=10;if(a.length<b)for(c=a.length;c<b;c++)a=" "+a;return a}}