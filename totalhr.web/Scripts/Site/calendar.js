//document.onclick = AdjustEvents;


var MSG_ADDED;
var MSG_MISSING_REMINDER_TYPE;

var eventid = 0;
var CalendarId = null;
var prevTargetSelected = null;
var ckSelectedTargetUsers = new Array();
var ckSelectedDepartment = new Array();
var SelectedReminderType = null;
var Reminders = new Array();

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
    if (objck.checked) {
        ckSelectedTargetUsers['ckuser_' + objck.value] = objck.value;
    } else {
        ckSelectedTargetUsers['ckuser_' + objck.value] = null;
    }
}

function PickDepartment(objck) {
    if (objck.checked) {
        ckSelectedDepartment['ckdepartment_' + objck.value] = objck.value;
    } else {
        ckSelectedDepartment['ckdepartment_' + objck.value] = null;
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


function ApplyAttendeeTargetSelection(obj, id, selector) {
    var objInput = $('#' + obj.id).children("input[type=radio]");
    var objInfo = $('#' + obj.id).children("i");

    if (prevTargetSelected != null) {
        var opt =$('#' + prevTargetSelected.id).children("input[type=radio]");
        opt.attr('checked', false);
        var desc = $('#' + prevTargetSelected.id).children("#spAttendeeDesc_" + id);
        desc.css("color", "black");
    }
   
    objInput.attr('checked', true);    
   
    //doesn't update twice BUG
    prevTargetSelected = obj;

    if (selector != null)
        OpenSelector(selector);
}

function ShowDetails(obj) {
    obj.style.fontWeight = "bold";
}
function HideDetails(obj) {
    obj.style.fontWeight = "normal";
}
    
/* validate date entered. ERROR_INVALID_DATE expected to be on page */
function KeepDateIn(inputid) {
    var arrOffset = 0;
    var dateEntered = $.trim($('#' + inputid).val());

    //init array if empty
    if (RepeatsToSave['RepeatTypeId_' + previousSelectedId] == null) {
        RepeatsToSave['RepeatTypeId_' + previousSelectedId] = [1];
        arrOffset = 1;
    } else {
        arrOffset = RepeatsToSave['RepeatTypeId_' + previousSelectedId].length;
    }

    //save date to array
    RepeatsToSave['RepeatTypeId_' + previousSelectedId][arrOffset] = dateEntered;    
    RepeatsToSave['RepeatTypeId_' + previousSelectedId][0] = arrOffset;
        
    $('#spRepeatTypeAdded_' + previousSelectedId).append('<br/>' + dateEntered);
    $('#' + inputid).val('');
    prevSelDisplayObj.fadeOut("slow");
    $('#repeatOptions').fadeOut("slow");
    $('#spRepeatTypeCount_' + previousSelectedId).html(arrOffset + ' ' + MSG_ADDED);
    $('#dvRepeatHead').html($('#spRepeatDesc_' + previousSelectedId).html() + ' - ' + arrOffset + ' ' + MSG_ADDED);
}
    
function SetUntilDateIn(objid) {
    var dateEntered = $.trim($('#' + objid).val());

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

function ApplyReminderSelection(objid) {
    var obj = document.getElementById(objid);
    var val = obj.value;
    var message = '';
    
    if (obj.checked) {
        if (val == 1) {
            message = $('#txtReminderFrequencyBefore').val() + " " + $('#ddlFrequencyBeforeType option:selected').text() + " " + $('#spTimeBefore').html();
        }else if (val == 2) {
            message = $('#spEveryTime').html() + " " + $('#txtReminderFrequency').val() + " " + $('#ddlFrequencyType option:selected').text();
        }
        
        $('#dvReminderProgress').html(message);
        $('#dvReminderOptions').slideDown("slow");
        
        SelectedReminderType = val;
    }

}

function SaveReminder() {
    if (SelectedReminderType == null) {
        alert(MSG_MISSING_REMINDER_TYPE);
    }
    //Reminders
    var frequency = 0;
    var frequencytype = 0;
    var message = '';
    
    if (SelectedReminderType == 1) {
        frequency = $('#txtReminderFrequencyBefore').val();
        frequencytype = $('#ddlFrequencyBeforeType option:selected').val();
        message = $('#txtReminderFrequencyBefore').val() + " " + $('#ddlFrequencyBeforeType option:selected').text() + " " + $('#spTimeBefore').html();
    } else if(SelectedReminderType == 2) {
        frequency = $('#txtReminderFrequency').val();
        frequencytype = $('#ddlFrequencyType option:selected').val();
        message = $('#spEveryTime').html() + " " + $('#txtReminderFrequency').val() + " " + $('#ddlFrequencyType option:selected').text();
    }
    
    if (frequency == 0 || frequency == '' || isNaN(frequency)) {
        alert(MSG_MISSING_REMINDER_VALUES);
        return;
    }

    Reminders[Reminders.length] = [SelectedReminderType, frequency, frequencytype, message];

    DisplayAddedReminders();
    
    
}

function DisplayAddedReminders() {
    var tempHtml = '';
    var len = Reminders.length;
    for (var i = 0; i < len; i++) {
        tempHtml += '<span class="row">#' + (i + 1) + ': ' + Reminders[i][3] + '</span>';
    }
    
    $('#dvAddedReminders').html(tempHtml);
}

function PrepareToSaveValues() {
    
    try{
        //validate dates (also check times)***
        var compare = Date.parse($('#StartDate').val()).compareTo(Date.parse($('#EndDate').val()));
        if (compare > 0) {
            alert(MSG_ERROR_STARTDATE_AFTER_ENDDATE);
            return false;
        }

        // prepare timings
        $('#StartTime').val($('#starthour').val() + ' : ' + $('#startminute').val() + ' : 00');
        $('#EndTime').val($('#endhour').val() + ' : ' + $('#endminute').val() + ' : 00');

        //prepare reminders
        var reminderlen = Reminders.length;
        var remindersXML = '';
    
        for (var i = 0; i < reminderlen; i++) {
            if (Reminders[i] != null) {
                remindersXML += '<reminder><type>' + Reminders[i][0] + '</type><frequencytype>' + Reminders[i][2] + '</frequencytype><frequency>' + Reminders[i][1] + '</frequency></reminder>';
            }
        }
    
        if (remindersXML != '') {
            remindersXML = '<reminders>' + remindersXML + '</reminders>';
        }

        $('#ReminderXML').val(remindersXML);
        
        // prepare target groups
        var selTargetVal = $("input:radio[name='TargetAttendeeGroupId']:checked").val();
    
        if (selTargetVal == Department) {
            var temp = '';

            for (var key in ckSelectedDepartment) {
                temp += (temp != '') ? ',' + ckSelectedDepartment[key] : ckSelectedDepartment[key];
            }

            $('#InvitedDepartmentIds').val(temp);

        } else if (selTargetVal == User) {
            var temp = '';

            for (var key in ckSelectedTargetUsers) {
                if (ckSelectedTargetUsers[key] != null) {
                    temp += (temp != '') ? ',' + ckSelectedTargetUsers[key] : ckSelectedTargetUsers[key];
                }
            }

            $('#InvitedUserIds').val(temp);
        } 

        //prepare repeats
        var repeatDateXML = '';
        var repeatType = "";
        var selectedRP = $("input[type='radio'][name='RepeatTypeGroup']:checked");
        if (selectedRP.length > 0) {
            repeatType = selectedRP.val();
        }


        if (repeatType == DailyMonToFri || repeatType == OnDayOfTheWeek) {
            repeatDateXML = '<dates><date>' + $('#txtRepeatUntil').val() + '</date></dates>';
        } else if (repeatType == OnDates || repeatType == MonthlyOnDates || repeatType == YearlyOnSameDate) {
            var arrTemp = RepeatsToSave['RepeatTypeId_' + repeatType];

            for (var key=1; key < arrTemp.length; key++) {
                repeatDateXML += '<date>' + arrTemp[key] + '</date>';
            }

            repeatDateXML = '<dates>' + repeatDateXML + '</dates>';

        }

        $('#RepeatType').val(repeatType);
     
        if(repeatType == YearlyOnSameDate)
            $('#RepeatValue').val(repeatDateXML);
        else
            $('#RepeatXML').val(repeatDateXML);        return true;    }catch(ex){
        alert(MSG_ERROR_SUBMIT_FAILED);
        return false;
    }
}