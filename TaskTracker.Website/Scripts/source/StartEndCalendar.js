function ValidateStartEndDates(sender, args)
    {
        if (args.Value===""){
            args.IsValid=false;
            sender.IsValid=false;
        }
        else{
            args.IsValid = true;
            sender.IsValid=true;              
        }
}

function UpdateStartEndDates(StartDateValue,EndDateValue)
    {
        var StartDate = $find("ce1");
        var EndDate = $find("ce2");
      
      if (StartDate!==null && EndDate!==null){
        StartDate.set_selectedDate(StartDateValue);
        EndDate.set_selectedDate(EndDateValue);
      }       
    }
        
function isValidDate(strDate){
    var dteDate;
    var day, month, year;
    var datePat = /^(\d{1,2})(\/)(\d{1,2})(\/)(\d{4})$/;
    var matchArray = strDate.match(datePat);

    if (matchArray === null)
    return false;

    day = matchArray[1]; // p@rse date into variables
    month = matchArray[3];
    year = matchArray[5];
    month--;

    dteDate=new Date(year,month,day);
    return this.value=((day===dteDate.getDate()) && (month===dteDate.getMonth()) && (year===dteDate.getFullYear()));
}

function setStartEndDate(ddMonth,ddYear,txtDay,txtDate){
ddMonth=document.getElementById(ddMonth);
ddYear=document.getElementById(ddYear);
txtDay=document.getElementById(txtDay);
txtDate=document.getElementById(txtDate);
    if (ddMonth!==null && ddYear!==null && txtDay!==null){
    var DateValues = txtDate.value.split('/');
    var dt = new Date(DateValues[2],DateValues[0],DateValues[1]);
        ddMonth.value=dt.getMonth();
        ddYear.value=dt.getFullYear();
        txtDay.value=dt.getDate();
    }
}

   
    function CalendarShowing(sender,args)
    {
        var myDateRange = new DateRange();
        if (sender!==null && sender._isAnimating===false){
        //The calendar should be displayed until a date has been selected
            //sender.show(); 
            var mth;
            var yr;
            if (sender._id===myDateRange.StartDateCalID){/*Start Date*/
                mth = document.getElementById (myDateRange.StartMonthDDLID);
                yr = document.getElementById (myDateRange.StartYearDDLID);
            }
            else{
                mth = document.getElementById (myDateRange.EndMonthDDLID);
                yr = document.getElementById (myDateRange.EndYearDDLID);
            }
            if (mth!==null && yr!==null && sender._getEffectiveVisibleDate()!==null){
                mth.value = sender._getEffectiveVisibleDate().getMonth()+1;
                yr.value=sender._getEffectiveVisibleDate().getFullYear();
            }  
       }
//       else if (sender._isAnimating==null || sender._isAnimating==true){
//            if (sender._id==myDateRange.StartDateCalID){/*Start Date*/
//                  $find(myDateRange.StartDatePCE).hidePopup();
//            }
//            else{
//                  $find(myDateRange.EndDatePCE).hidePopup();
//            }
//       }
//       
//       else{
//       
//       }
    }
    
    function CalendarClosing(sender,args)
    {
        var myDateRange = new DateRange();
        if (sender!==null && sender._isAnimating===true){
        //The calendar should be displayed until a date has been selected
            sender.show();
            var mth;
            var yr;
            if (sender._id===myDateRange.StartDateCalID){/*Start Date*/
                mth = document.getElementById (myDateRange.StartMonthDDLID);
                yr = document.getElementById (myDateRange.StartYearDDLID);
            }
            else{
                mth = document.getElementById (myDateRange.EndMonthDDLID);
                yr = document.getElementById (myDateRange.EndYearDDLID);
            }
            if (mth!==null && yr!==null && sender._getEffectiveVisibleDate()!==null){
                mth.value = sender._getEffectiveVisibleDate().getMonth()+1;
                yr.value=sender._getEffectiveVisibleDate().getFullYear();
            }  
       }
       else if (sender._isAnimating===null || sender._isAnimating===false){
            if (sender._id===myDateRange.StartDateCalID){/*Start Date*/
                  $find(myDateRange.StartDatePCE).hidePopup();
            }
            else{
                  $find(myDateRange.EndDatePCE).hidePopup();
            }
       }
       
       else{
       
       }
    }
    function SetNewDate(sender,args)
    {     
        var myDateRange = new DateRange();
        if (sender!==null && sender._mode==='days'){
            var dateValue = (sender.get_selectedDate().getMonth() + 1) + '/' + sender.get_selectedDate().getDate() + '/' + sender.get_selectedDate().getFullYear();
            var mth;
            var yr;
            if (sender._id===myDateRange.StartDateCalID){/*Start Date*/
                document.getElementById (myDateRange.StartDateID).value=dateValue;   
                mth = document.getElementById (myDateRange.StartMonthDDLID);
                yr = document.getElementById (myDateRange.StartYearDDLID);
                $find(myDateRange.StartDatePCE).hidePopup();
            }
            else{
                document.getElementById (myDateRange.EndDateID).value=dateValue;   
                mth = document.getElementById (myDateRange.EndMonthDDLID);
                yr = document.getElementById (myDateRange.EndYearDDLID);
                $find(myDateRange.EndDatePCE).hidePopup();
            }
            if (mth!==null && yr!==null && sender.get_selectedDate()!==null){
                mth.value = sender.get_selectedDate().getMonth()+1;
                yr.value=sender.get_selectedDate().getFullYear();
            }  
             ValidateDateRange();
       }
       else{
          sender.show(); 
       }
    }
    
    function ValidateDateRange()
    {
        //var StartDate = $find("ce1");
        var EndDate = $find("ce2");
        var myDateRange = new DateRange();
        var StartDateValue = new Date(document.getElementById (myDateRange.StartDateID).value);
        var EndDateValue = new Date(document.getElementById (myDateRange.EndDateID).value);
        if( EndDateValue <StartDateValue)
        {                      
            var errMsg = 'The End Date['+document.getElementById (myDateRange.EndDateID).value+'] has to be >= the Start Date ['+document.getElementById (myDateRange.StartDateID).value+']';
            alert(errMsg);            
            EndDate.set_selectedDate(document.getElementById (myDateRange.StartDateID).value);                  
            document.getElementById (myDateRange.EndDateID).value=  document.getElementById (myDateRange.StartDateID).value;           
        }                                
    } 
    
    function DisplayCalendar(index)
    {
        var myDateRange = new DateRange();
        if (index===1){
            $find(myDateRange.StartDatePCE).showPopup(); 
            $find(myDateRange.StartDateCalID).show();
            CalendarShowing($find(myDateRange.StartDateCalID));
        }
        else {
            $find(myDateRange.EndDatePCE).showPopup(); 
            $find(myDateRange.EndDateCalID).show();
            CalendarShowing($find(myDateRange.EndDateCalID));
        }
    }
    function SetMonthYear(mth,yr,index)
    {
        var myDateRange = new DateRange();
        var obj;
        if (index===1){
            obj = $find(myDateRange.StartDateCalID);  
        }
        else {
            obj = $find(myDateRange.EndDateCalID); 
        }
        var mthObj = document.getElementById (mth);
        var yrObj = document.getElementById (yr);  
        if (obj!==null) obj._switchMonth(new Date(mthObj.value +'/1/'+yrObj.value),true);   
    }
    function ShowCalendar(index) 
    {
        if (index===1){
            setTimeout ('DisplayCalendar(1)', 50 );
        }
        else {
            setTimeout ('DisplayCalendar(2)', 50 );
        }
    }
