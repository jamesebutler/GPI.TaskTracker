// JScript File
ScriptCombineAttribute = true;
if (document.getElementById) {
    window.alert = function (txt) {
        createCustomAlert(txt);
    }
}

function SetActiveNav(id) {
    $(document).ready(function () {
        $(".nav li").removeClass("active");//this will remove the active class from  
        //previously active menu item 
        $('#' + id).addClass('active');
    });
}

//this.clearTableBody = function (table) {
//    function empty() {
//        while (this.firstChild) this.removeChild(this.firstChild);
//    }
//    empty.apply(table.tBodies[0]);   
//};
this.clearTableBody = function (table) {
    $(table.tBodies[0]).empty();
};
function ExportExcel(fileName) {
    PopupWindow(fileName, "Excel", 600, 600, "yes", "yes");

}

function PopupWindow(mypage, myname, w, h, scroll, menu, fullscreen) {
    var win = null;
    var LeftPosition = (screen.width) ? (screen.width - w) / 2 : 0;
    var TopPosition = (screen.height) ? (screen.height - h) / 2 : 0;
    if (fullscreen === "true") { w = screen.width; h = screen.height - 100; LeftPosition = 0; TopPosition = 0; fullscreen = "yes"; }
    settings = 'status=yes,height=' + h + ',width=' + w + ',top=' + TopPosition + ',left=' + LeftPosition + ',scrollbars=' + scroll + ',resizable,menubar=' + menu;
    win = window.open(mypage, myname, settings)
    if (win !== null) try { win.focus(); } catch (err) { }
    return win;
}

function OpenOrCloseSpan(spanTag) {
    var st = document.getElementById(spanTag);
    if (st === null) {
        st = document.getElementsByName(spanTag);
    }
    if (st !== null) {
        if (st.style !== null) {
            if (st.style.display === 'none')
                st.style.display = '';
            else
                st.style.display = 'none';
        }
    }
}
function ValidateForm(valGroup) {
    Page_ClientValidate(valGroup);
    return Page_IsValid;
}
function CheckForm(valGroup) {
    try {

        Page_ClientValidate(valGroup);
        if (!Page_IsValid) {
            var s = new StringBuilder;

            //var s = "";
            var i;
            var validatorList = Page_Validators;

            for (i = 0; i < validatorList.length; i++) {
                if (!validatorList[i].isvalid &&
					 typeof (validatorList[i].errormessage) === "string") {
                    if (s.buffer.length === 0) {
                        s.Append("<ul class=''>")
                    }
                    s.Append("<li class=''>");
                    s.Append(validatorList[i].errormessage);
                    s.Append("</li>");
                }
            }
            if (s.buffer.length > 0) {
                s.Append("</ul>")
                //alert(s.ConvertToString());
                //createCustomAlert(s.ConvertToString(), 'Validation Warning');
                //$('#valModalTitle').html('Validation Warning');
                $('#valModalBody').html(s.ConvertToString());
                $('#errorModal').modal('show');
                Page_ClientValidate('');
                Page_IsValid = true;
                return false;
                //				var summary = document.getElementById(valSummary)
                //				if (summary!=null){
                //					summary.innerHTML=s.ConvertToString();
                //					document.getElementById(valClose).click();
                //					document.getElementById(valTrigger).click ();
                //					Page_IsValid=true;
                //					return false;
                //				}
            }
        }
        else {
            Page_IsValid = true;
            DisplayBusy();
            return true;

        }

    }

    catch (err) {

    }

}

var JQRedirectURL = '#';
function JQConfirmRedirect(url, confirmBeforeRedirect) {
    JQRedirectURL = url;
    if (confirmBeforeRedirect === false) {
        $(window.location).attr('href', JQRedirectURL);
    }
    else {
        $("#dialog-confirm").dialog("open");
    }
}


function createCustomAlert(message, title) {
    //_AlertMessage
    var msg = $('#_lblAlertInfo');
    var trigger = $('#_alertInfo');
    var header = $('#_lblAlertTitle');

    if (header !== null) {
        if (title !== null)
            header.innerHTML = title;
        else
            header.innerHTML = "International Paper";
    }
    if (msg !== null) {
        msg.innerHTML = message;
    }

    if (trigger !== null) {
        trigger.show();
    }
    return false;
}


//var bannerVisible=true;
function HideShowBanner(pnl, img, expandImage, collapseImage) {
    var bannerPanel = $get(pnl);
    var imgShowHideHeader = $get(img);
    if (bannerPanel !== null && imgShowHideHeader !== null) {
        if (bannerVisible === true) {
            //hide
            bannerPanel.style.display = 'None';
            bannerVisible = false;
            imgShowHideHeader.src = expandImage;
        }
        else {
            //show
            bannerPanel.style.display = '';
            bannerVisible = true;
            imgShowHideHeader.src = collapseImage;
        }
        SetCookie("HideShowBanner", bannerVisible, 30);
    }
}

function SetCookie(cookieName, cookieValue, nDays) {
    var today = new Date();
    var expire = new Date();
    if (nDays === null || nDays === 0)
        nDays = 1;
    expire.setTime(today.getTime() + 3600000 * 24 * nDays);
    document.cookie = cookieName + "=" + escape(cookieValue)
                 + ";path=/CEMR;expires=" + expire.toGMTString();
}


function RedirectAfterTimeOut() {
    confirm('The Session timed out');
    window.location = window.location.href;
    //window.location = window.location.href;
}
function LoadEvents() {
    try {

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        //preLoadImages();       
        if (!prm.get_isInAsyncPostBack()) {
            prm.add_beginRequest(BeginRequest);
            prm.add_endRequest(EndRequest);
        }
        window.scrollTo(0, 0);
        if (window.parent !== null) {
            window.parent.parent.scrollTo(0, 0);
        }
    }
    catch (err) {

    }

}
var timeoutId;
function BeginRequest(sender, args) {
    try {
        //$get('ClientEvents').innerHTML += "PRM:: End of async request.<br/>";
        Page_HasBeenSubmitted = true;
        cursor_wait();
        DisableSubmitButtons();
        //timeoutId = setTimeout('DisplayBusy();', 1000);//1/4 second delay to see if we should display the busy screen
        DisplayBusy();
        
    }
    catch (err) {

    }
}
function HideBusyModal() {
    $('#backgroundModal').modal('hide');
}
function DisplayBusy() {
    $('#backgroundModal').modal('show');
    //var busyButton = document.getElementById('ctl00__imbBusy');
    //if (busyButton != null) {
    //    busyButton.click();
    //}

    //var popUpExtender = $find("_mpeBusy");
    //if (popUpExtender) {
    //    popUpExtender.show();
    //}
}
function DisplayCEMRAnimation(imgId, imgUrl) {
    document.getElementById(imgId).src = imgUrl;
}

function ResetCEMRAnimation() {
    document.getElementById('ctl00__imgProgress').src = "../Images/cemr_animation.gif";
}

function EndRequest(sender, args) {
    try {
        clearTimeout(timeoutId);
        EnableSubmitButtons();
        cursor_clear();
        HideBusyModal();
        Page_HasBeenSubmitted = false;
        window.scrollTo(0, 0);
        if (window.parent!==null){
            window.parent.parent.scrollTo(0, 0);
        }
    }
    catch (err) {

    }
}


function preLoadImages() {
    if (document.images) {
        //preload_image = new Image(25,25); 
        //preload_image.src="../Images/TreesAndBlueSky.jpg"; 
    }
}

function cursor_wait() {
    try {
        document.body.style.cursor = 'wait';
    }
    catch (err) {

    }
}
function cursor_clear() {
    try {
        document.body.style.cursor = 'default';
    }
    catch (err) {

    }
}

function DisableButton(buttonElem) {
    buttonElem.value = 'Please Wait...';
    buttonElem.disabled = true;
}
function StringBuilder() {
    this.buffer = [];
}
StringBuilder.prototype.Append = function (str) {
    this.buffer[this.buffer.length] = str;
};
StringBuilder.prototype.ConvertToString = function () {
    return this.buffer.join('');
};

//var Page_HasBeenSubmitted=false;
var _saveDisableSubmitButtons = new Array();
function DisableSubmitButtons() {
    var ct = 0;

    try {
        var tagElements = document.getElementsByTagName('INPUT');
        for (var k = 0 ; k < tagElements.length; k++) {
            if (tagElements[k].type === 'submit') {
                if (tagElements[k].disabled === false) {
                    _saveDisableSubmitButtons[ct] = tagElements[k].id;
                    ct++;
                    tagElements[k].disabled = true;
                    tagElements[k].style.visibility = 'hidden';
                }
            }
        }
    }
    catch (ex) {

    }
    return true;
}

function EnableSubmitButtons() {
    try {
        for (var k = 0 ; k < _saveDisableSubmitButtons.length; k++) {
            var btn = document.getElementById(_saveDisableSubmitButtons[k]);
            if (btn !== null) {
                btn.style.visibility = 'visible';
                btn.disabled = false
            }
        }
    }
    catch (ex) {

    }
}

function PrintDiv(divID) {
    var prtContent = document.getElementById(divID);
    if (prtContent !== null) {
        var headContent = document.getElementsByTagName('head')[0].outerHTML;
        var WinPrint = window.open('', '', 'left=0,top=0,width=800,height=1000,toolbar=0,scrollbars=0,status=0');
        WinPrint.document.write(headContent);
        WinPrint.document.write(prtContent.innerHTML);
        WinPrint.document.close();
        WinPrint.focus();
        WinPrint.print();
        WinPrint.close();
    }
}

var IFR = function () {
    var tries = 0;
    var id = '';
    var callBackFunction = "document.getElementById('ctl00__btnCloseBusy').click()";
    return {
        loadExcelViewer: function (src, iframeID, callBack) {
            id = iframeID;
            document.getElementById(iframeID).src = src;
            if (callBackFunction === null) callBackFunction = callBack;
            this.isIframeLoaded();
        },

        isIframeLoaded: function () {
            tries++;
            var retValue;
            if (document.getElementById(id).readyState === 'interactive' || document.getElementById(id).readyState === 'complete')
                retValue = true
            else
                retValue = false

            if (retValue === false && tries <= 30) {
                this.schedule();
            }
            else
                window.setTimeout(callBackFunction, 1);
        },
        closeMe: function () {
            self.close();
        },
        schedule: function () {
            DisplayBusy();
            window.setTimeout("IFR.isIframeLoaded()", 20000);
        }
    };
}();

function CheckIfListChecked(ctrl) {
    var chkBoxList = document.getElementById(document.getElementById(ctrl.id).controltovalidate);
    if (chkBoxList !== null) {
        var chkBoxCount = chkBoxList.getElementsByTagName("input");
        for (var i = 0; i < chkBoxCount.length; i++) {
            if (chkBoxCount.item(i).checked) {
                return true;
            }
        }
    }
    return false;
}
function resizeFrame(f) {
    try {
        var newHeight = f.contentWindow.document.body.scrollHeight + 100;
        if (newHeight < 300) newHeight = 300;
        f.style.height = newHeight + "px";
    }
    catch (e) { }
}
function SelectRadioButton(rbl) {
    rbl = document.getElementById(rbl);
    if (rbl !== null) {
        if (rbl.checked === false) {
            rbl.checked = true;
            setTimeout('__doPostBack("' + rbl.id + '");', 0);
        }
    }
}

$(document).ready(function () {
    //scroll the message box to the top offset of browser's scrool bar
    $(window).scroll(function () {
        $('#message_box').animate({ top: $(window).scrollTop() + "px" }, { queue: false, duration: 350 });
    });
    //when the close button at right corner of the message box is clicked 
    $('#close_message').click(function () {
        //the messagebox gets scrool down with top property and gets hidden with zero opacity 
        $('#message_box').animate({ top: "+=15px", opacity: 0 }, "slow");
    });
});

function ToggleGridControls(mainControl, checked, controlIds) {
    var controlList = controlIds.toString().split(",");
    for (var i = 0; i < controlList.length; i++) {
        var obj = document.getElementById(controlList[i]);
        if (obj !== null) {
            obj.disabled = (checked === false);

        }
    }
    var row;
    var mainControlObj = document.getElementById(mainControl);
    if (checked === true) {
        row = $(mainControlObj).parents('tr:first');
        row.find('td').css('backgroundColor', 'yellow').end();
    }
    else {
        row = $(mainControlObj).parents('tr:first');
        row.find('td').css('backgroundColor', '#f0f0f6').end();
    }

}
function maxTextboxLength(text, maxLen, expand) {
    var maxlength = new Number(maxLen);
    var isPermittedKeystroke;
    var enteredKeystroke;
    var ret = true;

    try {
        if (text !== null) {
            // Allow non-printing, arrow and delete keys
            enteredKeystroke = window.event.keyCode;
            isPermittedKeystroke = ((enteredKeystroke < 32) // Non printing
            || (enteredKeystroke >= 33 && enteredKeystroke <= 40)    // Page Up, Down, Home, End, Arrow
            || (enteredKeystroke === 46))  // Delete
            if (maxlength > 0) {
                if (!isPermittedKeystroke) {
                    if (text.value.length >= maxlength) {
                        text.value = text.value.substring(0, maxlength);
                        ret = false;
                    }
                }
                else {
                    if (text.value.length > maxlength) {
                        text.value = text.value.substring(0, maxlength);
                        ret = true;
                    }
                }

            }

            if (expand === true) {
                text.style.height = 'auto';
                text.style.height = text.scrollHeight + 'px';
            }
        }
        window.event.returnValue = ret;
        return ret;
    }
    catch (err) {

    }
}
//$(document).ready(function () {

//    //jQuery.each(jQuery('textarea[data-autoresize]'), function () {
//    //    var offset = this.offsetHeight - this.clientHeight;

//    //    var resizeTextarea = function (el) {
//    //        jQuery(el).css('height', 'auto').css('height', el.scrollHeight + offset);
//    //    };
//    //    jQuery(this).on('keyup input', function () { resizeTextarea(this); });//.removeAttr('data-autoresize');
//    //});

//    $('textarea').autoResize();


//});
//jQuery.each(jQuery('textarea[data-autoresize]'), function () {
//    var offset = this.offsetHeight - this.clientHeight;

//    var resizeTextarea = function (el) {
//        jQuery(el).css('height', 'auto').css('height', el.scrollHeight + offset);
//    };
//    jQuery(this).on('keyup input', function () { resizeTextarea(this); }).removeAttr('data-autoresize');
//});

// function goodbye(e) {
// alert('bye bye');

//	if(!e) e = window.event;
//	e.returnValue=false;
//	//e.cancelBubble is supported by IE - this will kill the bubbling process.
//	e.cancelBubble = true;
//	e.returnValue = 'Please Save you changes.  You sure you want to leave this page?'; //This is displayed on the dialog

//	//e.stopPropagation works in Firefox.
//	if (e.stopPropagation) {
//		e.stopPropagation();
//		e.preventDefault();
//	}
//}
//window.onbeforeunload=goodbye;



//$(function() {
//		$( ".selector" ).datepicker({});		
//		var dates = $('input[type=text][id*=_txtDateFrom],input[type=text][id*=_txtDateTo]').datepicker({			
//			defaultDate: "+1w",
//			changeMonth: true,
//			numberOfMonths: 1,
//			changeYear: true,		
//			showOtherMonths: true,	
//			showButtonPanel: true,			
//			buttonImageOnly: false,
//			minDate:new Date(2007, 1 - 1, 1),
//			maxDate:new Date(2030, 1 - 1, 1),
//			onSelect: function( selectedDate ) {			
//			    var dateFrom = dates[0].id;
//				var option = this.id == dateFrom ? "minDate" : "maxDate",
//					instance = $( this ).data( "datepicker" );
//					date = $.datepicker.parseDate(
//						instance.settings.dateFormat ||
//						$.datepicker._defaults.dateFormat,
//						selectedDate, instance.settings );
//				dates.not( this ).datepicker( "option", option, date );
//			}
//		});
//	});

//$(function() {$( ".selector" ).datepicker({}); var dates = $('input[type=text][id*=ctl01__cphMainContent_JQDateRange1__txtDateFrom],input[type=text][id*=ctl01__cphMainContent_JQDateRange1__txtDateTo ]').datepicker({			defaultDate: "+1w",		changeMonth: true,		numberOfMonths: 1,		changeYear: true,		showOtherMonths: true,		showButtonPanel: true,		buttonImageOnly: false,		minDate:new Date(2007, 1 - 1, 1),		maxDate:new Date(2030, 1 - 1, 1),		onSelect: function( selectedDate ) {		    var dateFrom = dates[0].id;			var option = this.id == dateFrom ? "minDate" : "maxDate",				instance = $( this ).data( "datepicker" );				date = $.datepicker.parseDate(					instance.settings.dateFormat ||					$.datepicker._defaults.dateFormat,					selectedDate, instance.settings );			dates.not( this ).datepicker( "option", option, date );		}	});});

//IFR.loadExcelViewer('Images/cemr_animation_report.gif'");DisplayBusy(); 
function ApplySelectFilter() {
    $(document).ready(function () {
        if ($('.selectpicker') !== null) {
            //liveSearchStyle
            try {
                $('.selectpicker').selectpicker();
            }
            catch (e) { }
        }
    });
}

$(document).ready(function () {
    $(".modal").draggable({ handle: ".modal-header" });
    $(".modal").appendTo("form");
});

Sys.Application.add_load(ApplySelectFilter);
if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();