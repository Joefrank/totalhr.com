//document.onclick = AdjustEvents;

var eventid = 0;
var CalendarId = null;
var MSG_ADDED;
var prevTargetSelected = null;
var ckSelectedTargetUsers = '';
var ckSelectedDepartment = '';

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

    $('#spAttendeeDesc_' + objid).css("color", "green");
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
    var url;
    var func;

    if (mode == 'USERS') {
        url = '/Account/GetCompanyUsersJson/';
        func = 1;
    }else if (mode == 'DEPARTMENT') {
        url = '/Account/GetCompanyDepartmentsJson/';
        func = 2;
    }
   

    $.getJSON(url, null, function (data) {
        var div = $('#dvAttendeesOptions');
        div.html("<br/> " + "Users received from server: " + "<br/>");
        
        $.each(data, function (i, item) {
            

            if (func == 1)
                PrintUser(div, item);
            else if (func == 2)
                PrintDepartment(div, item);

            
        });
        div.slideDown("slow");
    });
    
    
}

function PrintUser(div, item) {
    div.append("<input type='checkbox' id='ckuser_" + item.id + "' value='" + item.id + "' onclick='PickUser(this);' />" + item.firstname + " " + item.surname);    
}

function PrintDepartment(div, item) {
    
    div.append("<input type='checkbox' id='ckdepartment_" + item.id + "' value='" + item.id + "' onclick='PickDepartment(this);' />" + item.Name);
}

function PickUser(objck) {
    ckSelectedTargetUsers += (ckSelectedTargetUsers == '') ? objck.value : ',' + objck.value;
    $('#InvitedUserIds').val(ckSelectedTargetUsers);
}

function PickDepartment(objck) {
    ckSelectedDepartment += (ckSelectedDepartment == '') ? objck.value : ',' + objck.value;
    $('#InvitedDepartmentIds').val(ckSelectedDepartment);
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


function ApplyAttendeeTargetSelection(obj, id) {
    var objInput = $('#' + obj.id).children("input[type=radio]");
    var objInfo = $('#' + obj.id).children("i");

    if (prevTargetSelected != null) {
        var opt =$('#' + prevTargetSelected.id).children("input[type=radio]");
        opt.attr('checked', false);
        var desc = $('#' + prevTargetSelected.id).children("#spAttendeeDesc_" + id);
        desc.css("color", "black");
    }
   // alert(objInput.attr('checked'));
    objInput.attr('checked', true);    
   
    //doesn't update twice BUG

    prevTargetSelected = obj;
}

function ShowDetails(obj) {
    obj.style.fontWeight = "bold";
}
function HideDetails(obj) {
    obj.style.fontWeight = "normal";
}
    
/* validate date entered. ERROR_INVALID_DATE expected to be on page */
function KeepDateIn(inputid) {
    var arrlen = 0;
    var dateEntered = $.trim($('#' + inputid).val());

    //if (!isDate(dateEntered)) {
    //    alert(ERROR_INVALID_DATE);
    //    return;
    //}

    if (RepeatsToSave['RepeatTypeId_' + previousSelectedId] == null) {
        RepeatsToSave['RepeatTypeId_' + previousSelectedId] = [1];
        arrlen = 1;
    } else {
        arrlen = RepeatsToSave['RepeatTypeId_' + previousSelectedId][0]++;
    }

    RepeatsToSave['RepeatTypeId_' + previousSelectedId][arrlen] = dateEntered;
        
    $('#spRepeatTypeAdded_' + previousSelectedId).append('<br/>' + dateEntered);
    $('#' + inputid).val('');
    prevSelDisplayObj.fadeOut("slow");
    $('#repeatOptions').fadeOut("slow");
    $('#spRepeatTypeCount_' + previousSelectedId).html(RepeatsToSave['RepeatTypeId_' + previousSelectedId][0] + ' ' + MSG_ADDED);
    $('#dvRepeatHead').html($('#spRepeatDesc_' + previousSelectedId).html() + ' - ' + RepeatsToSave['RepeatTypeId_' + previousSelectedId][0] + ' ' + MSG_ADDED);
}
    
function SetUntilDateIn(objid) {
    var dateEntered = $.trim($('#' + objid).val());

    //if (!isDate(dateEntered)) {
    //    alert(ERROR_INVALID_DATE);
    //    return;
    //}

    RepeatsToSave['RepeatTypeId_' + previousSelectedId] = [1];
    RepeatsToSave['RepeatTypeId_' + previousSelectedId][1] = dateEntered;
       
    $('#' + objid).val('');
    prevSelDisplayObj.fadeOut("slow");
    $('#repeatOptions').fadeOut("slow");
    $('#spRepeatTypeCount_' + previousSelectedId).html("Until " + RepeatsToSave['RepeatTypeId_' + previousSelectedId][1]);

    $('#dvRepeatHead').html($('#spRepeatDesc_' + previousSelectedId).html() + ' - ' + RepeatsToSave['RepeatTypeId_' + previousSelectedId][0] + ' ' + MSG_ADDED);
}
    
function ShowRepeats(objid) {
    $('#sppreview').html($('#' + objid).html());
}

function ToggleExpand(objid, bCondition, hdnMessage) {
    if (bCondition != null && bCondition == true || bCondition == null) {
        $('#' + objid).slideToggle(500);
    } else if (hdnMessage != null) {
        alert($('#' + hdnMessage).val());
    } else {
        alert('Action invalidated.');
    }
}

function DisplayInfo(obj) {
    
}


// parse a date in yyyy-mm-dd format
function parseDate(input) {
    var parts = input.match(/(\d+)/g);
    // new Date(year, month [, date [, hours[, minutes[, seconds[, ms]]]]])
    return new Date(parts[2], parts[1] - 1, parts[0]); // months are 0-based
}