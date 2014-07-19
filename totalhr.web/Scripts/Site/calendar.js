
var eventid = 0;
var CalendarId = null;


function ManageActiveDay(objTd) {
    var objTdId = objTd.id;
    eventid = 0;
    
    var urltoload = "/Calendar/CreateEvent/" + CalendarId;
    $("#dvPopup").load(urltoload);
    $('#dvPopup').css("display", "");
    $('#dvPopup').css("min-height", "600px");
}

function ManageEvent(evt) {
    var sEventClientId = evt.id;
    eventid = ArrEvents[sEventClientId][1];
    var urltoload = "/Calendar/EditEvent/" + eventid;
    $("#dvPopup").load(urltoload);
    $('#dvPopup').css("display", "");
    $('#dvPopup').css("min-height", "600px");
    event.stopPropagation();
    event.preventDefault();
}

function GrabValues() {
    var objStartTime = document.getElementById('StartTime');
    var objEndTime = document.getElementById('EndTime');
       
    objStartTime.value += " " + document.getElementById('starthour').value + ":" + document.getElementById('startminute').value + ":00";
    objEndTime.value += " " + document.getElementById('endhour').value + ":" + document.getElementById('endminute').value + ":00";

    return true;
}

function DisposePopup() {
    $('#dvPopup').fadeOut("slow");
    $('#dvPopup').css("display", "none");
}

function PickEventTargetSelection(obj, objid) {
    $('#' + objid).fadeIn("slow");
}

function DialogAddReminder(objid) {
    $('#' + objid).fadeIn("slow");
}

function SaveReminder() {
   var vFrom = $('#txtReminderFrom').val();
   var vFrequency =   $('#txtReminderFrequency').val();
   var vFrequencyType = $('#ddlFrequencyType').val();
       
   if (!isNaN(vFrequency) || !$('#txtReminderFrom').val() || !isNaN(vFrequencyType)) {
       alert('Invalid reminder please correct values entered.');
       return;
   }
}

function OpenSelector(mode) {
    if (mode == 'USERS') {
        window.open('/Account/GetCompanyUsers/');
    }else if (mode == 'DEPARTMENT') {
        window.open('/Account/GetCompanyDepartments/');
    }
}

function Expand(objid) {
    $('#' + objid).slideDown("slow");
}

function Hide(objid) {
    $('#' + objid).slideUp("slow");
}


    
function ApplyRepeatSelection(obj) {
    var objInput = $('#' + obj.id).children("input[type=radio]");
    var objInfo = $('#' + obj.id).children("i");
   
    objInput.attr('checked', true);
        
    var val = objInput.attr('value');
    previousSelectedId = val;
        
    $('#repeatOptions').fadeIn("slow");

    if (prevSelDisplayObj != null) {
        prevSelDisplayObj.css("display", "none");
    }
        
    if (val == OnDates || val == MonthlyOnDates) {
        $('#spRepeatBattery').fadeIn("slow");
        prevSelDisplayObj = $('#spRepeatBattery');
    }else if (val == DailyMonToFri || val == OnDayOfTheWeek) {
        $('#spUntilDate').fadeIn("slow");
        prevSelDisplayObj = $('#spUntilDate');
    }else if (val == YearlyOnSameDate) {
        $('#spRepeatValue').fadeIn("slow");
        prevSelDisplayObj = $('#spRepeatValue');
    }

    var currenttitle = objInfo.attr('title');
    $('#sprepeatinfo').html(currenttitle);
}
    
function ShowDetails(obj) {
    obj.style.fontWeight = "bold";
}
function HideDetails(obj) {
    obj.style.fontWeight = "normal";
}
    
//validate date entered.
function KeepDateIn(inputid) {
    var arrlen = 0;
        
    if (RepeatsToSave['RepeatTypeId_' + previousSelectedId] == null) {
        RepeatsToSave['RepeatTypeId_' + previousSelectedId] = [1];
        arrlen = 1;
    } else {
        arrlen = RepeatsToSave['RepeatTypeId_' + previousSelectedId][0]++;
    }

    RepeatsToSave['RepeatTypeId_' + previousSelectedId][arrlen] = $('#' + inputid).val();
        
    $('#spRepeatTypeAdded_' + previousSelectedId).append($('#' + inputid).val());
    $('#' + inputid).val('');
    prevSelDisplayObj.fadeOut("slow");
    $('#repeatOptions').fadeOut("slow");
    $('#spRepeatTypeCount_' + previousSelectedId).html(RepeatsToSave['RepeatTypeId_' + previousSelectedId][0] + " Added");
}
    
function SetUntilDateIn(objid) {
    RepeatsToSave['RepeatTypeId_' + previousSelectedId] = [1];
    RepeatsToSave['RepeatTypeId_' + previousSelectedId][1] = $('#' + objid).val();
       
    $('#' + objid).val('');
    prevSelDisplayObj.fadeOut("slow");
    $('#repeatOptions').fadeOut("slow");
    $('#spRepeatTypeCount_' + previousSelectedId).html("Until " + RepeatsToSave['RepeatTypeId_' + previousSelectedId][1]);
}
    
function ShowRepeats(objid) {
    $('#' + objid).fadeIn("slow");

}

function ToggleExpand(objid) {
    $('#' + objid).slideToggle(500);
}

function DisplayInfo(obj) {
    
}