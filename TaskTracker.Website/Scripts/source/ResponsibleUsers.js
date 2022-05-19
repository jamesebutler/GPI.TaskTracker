// JScript File
var ResponsibleUser = {
    xMousePos: 0,
    yMousePos: 0,
    xMousePosMax: 0,
    yMousePosMax: 0,
    desiredUsername: '',
    populateResponsibleUsers: function(ctl, userCtl, facilityBtn, userMode,showInactiveUsers, txtCtl) {
        try {
            if (ctl !== null) {
                var plantcode = ctl.value;
                if (plantcode.length > 0 && plantcode !== "?") {
                    SiteDropDownsWS.GetDDLData(plantcode, userCtl, userMode, (showInactiveUsers === 'True'), this.OnSucceeded, this.OnFailed);
                    try { $get(facilityBtn).title = ctl.options[ctl.selectedIndex].text } catch (e) { }
                    var shortFacility = ctl.options[ctl.selectedIndex].text.substring(0, 15) + "...";
                    try { $get(facilityBtn).innerText = shortFacility } catch (e) { }
                }
            }
        }
        catch (e) {
            alert(e)
        }
    },

    populateResponsibleUsersOnDemand: function(ctl, userCtl, facilityBtn, userMode, showInactiveUsers, txtCtl) {
        try {
            if (ctl !== null) {
                var plantcode = ctl.value;
                if (plantcode.length > 0 && plantcode !== "?") {
                    var list = $get(userCtl);
                    if (list.options.length <= 1) {
                        SiteDropDownsWS.GetDDLData(plantcode, userCtl, userMode, (showInactiveUsers==='True'), this.OnSucceeded, this.OnFailed);
                        try { $get(facilityBtn).innerText = ctl.options[ctl.selectedIndex].text } catch (e) { }
                    }
                }
            }
        }
        catch (e) {
            alert(e)
        }
    },

    populateResponsibleUsersByUsername: function(ctl, userCtl, facilityBtn, userMode, txtCtl) {
        //var userName = $get(txtCtl).value;
        if (ctl !== null) {
            var plantcode = ctl.value;
            if (plantcode.length > 0 && plantcode !== "?") {
                try {
                    $get(userCtl).style.display = '';
                }
                catch (e) { }
                SiteDropDownsWS.GetUserList(plantcode, userCtl, userMode, this.OnSucceeded, this.OnFailed);
                try { $get(facilityBtn).innerText = ctl.options[ctl.selectedIndex].text } catch (e) { }
            }
            else {
                try {
                    $get(facilityBtn).innerText = ctl.options[ctl.selectedIndex].text;
                    $get(userCtl).style.display = 'none';
                    $get(txtCtl).style.display = '';
                }
                catch (e) { }
            }
        }
    },
    //    populateResponsibleUsers: function(ctl, userCtl, facilityBtn, userMode) {
    //        if (ctl !== null) {
    //            var plantcode = ctl.value;
    //            if (plantcode.length > 0) {
    //                SiteDropDownsWS.GetDDLData(plantcode, userCtl, userMode, this.OnSucceeded, this.OnFailed);
    //                try { $get(facilityBtn).innerText = ctl.options[ctl.selectedIndex].text } catch (e) { }
    //                //document.myform.mylist.options[w].text;
    //            }
    //        }
    //    },

    ResponsibleUserIsSelected: function(sender, args) {
        if (sender !== null) {
            if (args.Value === "") {
                args.IsValid = false;
                sender.IsValid = false;
            }
            else {
                args.IsValid = true;
                sender.IsValid = true;
            }
        }
    },

    OnUserSelected: function(source, eventArgs) {
        alert(eventArgs.get_value());
        source._element.value = '';
    },

    SetResponsibleUser: function(ctl) {
        if (ctl !== null) {
            var target = $get(_txtTargetID);
            var user = $get(target.value); //"_txtResponsibleUser");
            var userName = $get(_txtResponsibleUserName);
            // jQuery(target.value).selectRange(0, 10);
            if (user !== null && userName !== null) {
                var fullName = jQuery.trim(ctl.options[ctl.selectedIndex].text).split(',');
                if (fullName.length >= 2) {
                    user.value = fullName[1] + ' ' + fullName[0] + this.addSpace('|', 500) + jQuery.trim(ctl.options[ctl.selectedIndex].value);
                }
                else {
                    user.value = jQuery.trim(ctl.options[ctl.selectedIndex].text);
                }
                user.title = jQuery.trim(ctl.options[ctl.selectedIndex].value);
                userName.value = ctl.value;
            }
            $find('_bhResponsibleUser').hidePopup();
        }
    },

    ClearResponsibleUser: function(ctl) {
        if (ctl !== null) {
            try {
                $get(ctl).selectedIndex = -1;
                var currentId = $get(ctl).id;
                $('#' + currentId).selectpicker('val', '');
                $('#' + currentId).selectpicker('refresh');

            }
            catch (e) {

            }

        }
        //         var target = $get(_txtTargetID);
        //         var user = $get(target.value); //"_txtResponsibleUser");
        //         var userName = $get(_txtResponsibleUserName);
        //         
        //         if (user!=null && userName!=null){
        //            userName.value='';
        //            user.title='';
        //            user.value='';            
        //         }
        //         $find('_bhResponsibleUser').hidePopup();

    },

    ShowResponsibleUser: function(ctl) {
        //var mouse = this.captureMousePosition;
        var target = $get(_txtTargetID);
        target.value = ctl.id;
        event.cancelBubble = true;
        var ddlCategory = $get(_ddlResponsibleUser);
        ddlCategory.selectedIndex = -1;
        var popExtender = $find('_bhResponsibleUser');
        //popExtender.set_OffsetY(ctl.offsetTop - target.offsetTop + document.body.scrollTop);
        var pos = this.findPos(target);
        var posctl = this.findPos(ctl);
        popExtender.set_OffsetY(posctl[1]); //ctl.offsetTop + 80 + document.body.scrollTop);
        popExtender.set_OffsetX(posctl[0]);

        popExtender.set_OffsetY(pos[1]); //ctl.offsetTop + 80 + document.body.scrollTop);
        popExtender.set_OffsetX(pos[0]);

        popExtender.set_OffsetY(this.getTop(ctl));
        popExtender.showPopup();

    },

    getTop: function(e) {
        var offset = e.offsetTop;
        if (e.offsetParent !== null) {
            offset += this.getTop(e.offsetParent);
        }
        return offset;
    },

    getLeft: function(e) {
        var offset = e.offsetLeft;
        if (e.offsetParent !== null) {
            offset += this.getLeft(e.offsetParent);
        }
        return offset;
    },

    OnSucceeded: function(data) {
        var ddlUser;
        var i;
        try {
            if (data !== null) {
                ddlUser = $get(data[0].Text); 
            }


            if (ddlUser !== null && data.length > 0) {
                if (ResponsibleUser.desiredUsername === '') {
                    if (ddlUser.selectedIndex >= 0) {
                        ResponsibleUser.desiredUsername = ddlUser.options[ddlUser.selectedIndex].value;
                    }
                }
                ResponsibleUser.ClearOptionsFastAlt(ddlUser.id); 
                for (i = 1; i < data.length; ++i) {
                    ResponsibleUser.addOption(ddlUser, data[i]);
                }
                if (ResponsibleUser.desiredUsername !== '') {
                    $("#" + ddlUser.id + " option[value='" + ResponsibleUser.desiredUsername + "']").attr("selected", "true");
                    ResponsibleUser.desiredUsername = '';
                }
                else {
                    ddlUser.selectedIndex = 1;
                }
                ddlUser.focus();
                
                $('#' + ddlUser.id).selectpicker('refresh');
            }
        }
        catch (e) { alert(e); }
        return data;
    },

    ClearOptionsFastAlt: function(id) {
        document.getElementById(id).innerHTML = "";
    },

    addOption: function(selectbox, datavalue) {
        var data = datavalue;
        var optn;
        if (data.Text.length > 0 ) {
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
                                    try {
                                        optn.runtimeStyle.backgroundColor = attr[1];
                                        optn.runtimeStyle.fontWeight = 'bold';
                                    }
                                    catch (e) { }
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
            //optn.disabled = true;
            selectbox.options.add(optn);
        }
    },

    OnFailed: function(error) {
        // Alert user to the error.
        alert(error.get_message());
    },

    addSpace: function(argvalue, numlength) {
        var i;
        if (numlength <= 0)
        { numlength = 10; }

        if (argvalue.length < numlength) {
            for (i = argvalue.length; i < numlength; i++)
            { argvalue = " " + argvalue; }
        }

        return argvalue;

    },

    findPos: function(obj) {
        var curleft = curtop = 0;
        if (obj.offsetParent) {
            do {
                curleft += obj.offsetLeft;
                curtop += obj.offsetTop;
            } while (obj = obj.offsetParent);
        }
        return [curleft, curtop];
    },

    captureMousePosition: function(e) {
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
            this.xMousePosMax = window.innerWidth + window.pageXOffset;
            this.yMousePosMax = window.innerHeight + window.pageYOffset;
        } else if (document.all) {
            // When the page scrolls in IE, the event's mouse position
            // reflects the position from the top/left of the screen the
            // user is looking at. scrollLeft/Top is the amount the user
            // has scrolled into the page. clientWidth/Height is the height/
            // width of the current page the user is looking at. So, to be
            // consistent with Netscape (above), add the scroll offsets to
            // both so we end up with an absolute value on the page, no
            // matter if the user has scrolled or not.
            this.xMousePos = window.event.x + document.body.scrollLeft;
            this.yMousePos = window.event.y + document.body.scrollTop;
            this.xMousePosMax = document.body.clientWidth + document.body.scrollLeft;
            this.yMousePosMax = document.body.clientHeight + document.body.scrollTop;
        } else if (document.getElementById) {
            // Netscape 6 behaves the same as Netscape 4 in this regard
            this.xMousePos = e.pageX;
            this.yMousePos = e.pageY;
            this.xMousePosMax = window.innerWidth + window.pageXOffset;
            this.yMousePosMax = window.innerHeight + window.pageYOffset;
        }
        window.status = "xMousePos=" + this.xMousePos + ", yMousePos=" + this.yMousePos + ", xMousePosMax=" + this.xMousePosMax + ", yMousePosMax=" + this.yMousePosMax;
    },

    PerformNameSearch: function(prefixText, dataControl) {
        //var searchBox = document.getElementById(prefixText);
        //var searchType = document.getElementById(context);
        //var searchTypeValue = "last";
        //var domainList = document.getElementById(domain);

        //        if (searchType != null) {
        //            if (searchType.checked == true) {
        //                searchTypeValue = "first";
        //            }
        //        }
        if (prefixText.length >= 3) {
            SiteDropDownsWS.GetEmployeeCompletionList(prefixText, dataControl, ResponsibleUser.onSearchComplete, ResponsibleUser.onSearchError, ResponsibleUser.onSearchTimeout);
        }
        else {
            //ResponsibleUser.clearSearchBox();
            $("#" + dataControl).empty();
        }
    },

    onSearchComplete: function(arg) {
        //var list = '_lbSearchResults'; //$('_lbSearchResults');
        if (arg.length > 0) {
            ResponsibleUser.PopulateListBox(arg[0], arg, "$");
        }
        else {
            $("#" + arg[0]).empty();
        }
    },

    onSearchError: function(arg) {

    },

    onSearchTimeout: function(arg) {

    },

    clearSearchBox: function() {
        $("#_lbSearchResults").empty();
    },

    PopulateListBox: function(listID, values, delim) {

        var listBox = document.getElementById(listID);
        if (listBox !== null) {
            //Delete all list items from the listbox                 
            listBox.innerHTML = "";

            //Populate the listbox                
            var len = 0;
            var valLength = values.length
            for (var i = 0; i < valLength; i++) {
                var listValues = values[i].split(delim);
                if (listValues.length === 2) {
                    if (listValues[0].length > 0) {
                        listBox.options[len] = new Option(listValues[0].trim(), listValues[1].trim(), false, false);
                        len++;
                    }
                }
            }
            listBox.selectedIndex = 0;
        }
    },

    HidePopup: function(behaviorId) {
        var bh = $find(behaviorId);
        if (bh !== null) {
            bh.hidePopup();
        }
    },

    SetUserAndFacility: function(facilityLabel, facilityListBox, userList, selectedList) {
        var selectedUser = "";
        $("#" + selectedList + " option:selected").each(function() {
            var $option = $(this);
            selectedUser = $option.val();
        });
        var selectedPlantCode = selectedUser.split('|');
        $("#" + facilityListBox + " option[value='" + selectedPlantCode[0] + "']").attr("selected", "true");
        ResponsibleUser.desiredUsername = selectedPlantCode[1];

        $('#' + facilityListBox).trigger('onchange');


    }

    ,

    SetUser: function(userList, selectedList, ResponsibleUser, targetCtrl) {
        var selectedUser = "";
        var selectedUserValue = "";
        $("#" + selectedList + " option:selected").each(function() {
            var $option = $(this);
            selectedUser = $option.text();
            selectedUserValue = $option.val();
            selectedUserValue = selectedUserValue.split('|');
            selectedUser = selectedUser.split('  ');
        });
        //var selectedPlantCode = selectedUser.split('|');


        if (selectedUserValue.length > 1) {
            $('#' + userList).val(selectedUserValue[1]);
        }
        if (selectedUser.length > 1) {
            $('#' + ResponsibleUser).html(selectedUser[0]);
        }
    }



};