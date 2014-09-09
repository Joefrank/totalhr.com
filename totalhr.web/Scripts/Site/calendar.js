//document.onclick = AdjustEvents;

var ERROR_INVALID_DATE = '';
var REMINDER_TYPE_CUSTOMIZE = '';
var NONE_ADDED_MESSAGE = '';
var MSG_BEFORE = '';
var MSG_ADDED='';
var MSG_MISSING_REMINDER_TYPE ='';
var MSG_USERS_FROM_SERVER = '';
var MSG_DEPARTMENTS_FROM_SERVER = '';
var MSG_ERROR_NOATTENDEE_CHOSEN = '';
var MSG_ERROR_ATTENDEE_REQUIRED = '';

var eventid = 0;
var CalendarId = null;
var prevTargetSelected = null;
var ckSelectedTargetUsers = new Array();
var ckSelectedDepartment = new Array();
var SelectedReminderType = null;
var Reminders = new Array();
var reminderTemplateHTML = '<tr><td></td><td></td></tr>';
var dialogmode = '';
var bUsersLoaded = false;
var bDepartmentsLoaded = false;
var InvitedUsers;
var InvitedDepartments;
var previousSelectedId = null;
var repeatTypeSelectedid = null;
var prevSelDisplayObj = null;
var RepeatsToSave = new Array();
var RepeatYears = new Array();
var EventId = '';

function ManageActiveDay(objTd) {
    var objTdId = objTd.id;
    eventid = 0;
    
    var urltoload = "/Calendar/CreateEvent/" + CalendarId;
    $('#ipopup').attr("src", urltoload);
    $('#dvPopup').css("display", "");
    $('#dvPopup').css("min-height", "600px");
}

function ManageEvent(evt) {
    alert('edit');
    var sEventClientId = evt.id;
    eventid = ArrEvents[sEventClientId][1];
    var urltoload = "/Calendar/EditEvent/" + eventid;
    $('#ipopup').attr("src", urltoload);
    $('#dvPopup').css("display", "");
    $('#dvPopup').css("min-height", "600px");
    event.stopPropagation();
    event.preventDefault();
}

function PreviewEvent(evt) {
    alert('preview');
    var sEventClientId = evt.id;
    eventid = ArrEvents[sEventClientId][1];
    var urltoload = "/Calendar/PreviewEvent/" + eventid;
    $('#ipopup').attr("src", urltoload);
    $('#dvPopup').css("display", "");
    $('#dvPopup').css("min-height", "300px");
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
    var divid = '';
    var message = '';
    var usercount = 0;
    var departmentcount = 0;
    
    dialogmode = mode;

    if (mode == 'USERS') {
        url = '/Account/GetCompanyUsersJson/';
        func = 1;
        divid = 'dvAttendeesList';
        message = MSG_USERS_FROM_SERVER;
        $('#dvAttendeesDepartments').css("display", "none");
        if (bUsersLoaded) {
            $('#' + divid).css("display", "");
            $('#dvAttendeesOptions').slideDown("slow");
            return;
        }
    }else if (mode == 'DEPARTMENT') {
        url = '/Account/GetCompanyDepartmentsJson/';
        func = 2;
        divid = 'dvAttendeesDepartments';
        message = MSG_DEPARTMENTS_FROM_SERVER;
        $('#dvAttendeesList').css("display", "none");
        if (bDepartmentsLoaded) {
            $('#' + divid).css("display", "");
            $('#dvAttendeesOptions').slideDown("slow");
            return;
        }
    }
    
    var div = $('#' + divid);
    
    $.getJSON(url, null, function (data) {
        div.css("display", "");
        div.html(message + "<br/>");
        
        $.each(data, function (i, item) {
            if (func == 1) {
                PrintUser(div, item);
                bUsersLoaded = true;
            } else if (func == 2) {
                PrintDepartment(div, item);
                bDepartmentsLoaded = true;
            }
        });
        
        $('#dvAttendeesOptions').slideDown("slow");
    });    
    
}

function CollapseSelector() {
    $('#dvAttendeesOptions').slideUp();
}
/** Target user stuff */
function PrintUser(div, item) {
    var sChecked = '';
    if (InvitedUsers != null) {
        for (var sc in InvitedUsers) {
            if (InvitedUsers[sc] == item.id) {
                sChecked = " checked ";
            }
        }
    }
    
    div.append("<input type='checkbox' " + sChecked + " id='ckuser_" + item.id + "' value='" + item.id + "' onclick='PickUser(this);' />" + item.firstname + " " + item.surname);
}

function PrintDepartment(div, item) {
    var sChecked = '';
    if (InvitedDepartments != null) {
        for (var sc in InvitedDepartments) {
            if (InvitedDepartments[sc] == item.id) {
                sChecked = " checked ";
            }
        }
    }
    
    div.append("<input type='checkbox' " + sChecked + " id='ckdepartment_" + item.id + "' value='" + item.id + "' onclick='PickDepartment(this);' />" + item.Name);
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


function ApplyAttendeeTargetSelection(obj, id, selector) {
    var objInput = $('#' + obj.id).children("input[type=radio]");
    var objInfo = $('#' + obj.id).children("i");

    if (prevTargetSelected != null && prevTargetSelected != obj) {
        var opt = $('#' + prevTargetSelected.id).children("input[type=radio]");
        opt.attr('checked', false);
        var desc = $('#' + prevTargetSelected.id).children("#spAttendeeDesc_" + id);
        desc.css("color", "black");
    }

    objInput.prop('checked', true);

    //doesn't update twice BUG
    prevTargetSelected = obj;

    if (selector != null && selector != "") {
        OpenSelector(selector);
    } else {
        $('#spattendeeCount').html('( ' + $('#spAttendeeDesc_' + id).text() + ' )');
    }
}

function SaveInvitees() {
    if (dialogmode == 'USERS') {
        SaveUserInvitees();
    } else if (dialogmode == 'DEPARTMENT') {
        SaveDepartmentInvitees();
    }
}

function SaveUserInvitees() {
    var allinvitees = '';
    var listAttendeesHTML = '';
    var count = 0;
  
    var val = $(":radio[name='TargetAttendeeGroupId']:checked").val();

    for (var key in ckSelectedTargetUsers) {
        if (ckSelectedTargetUsers[key] != null) {
            allinvitees += (allinvitees == '') ? ckSelectedTargetUsers[key] : ',' + ckSelectedTargetUsers[key];
            listAttendeesHTML += '<input type="hidden" name="TargetAttendeeIdList[' + count + ']" value="' + ckSelectedTargetUsers[key] + '" />';
            count++;
        }

    }
    if (allinvitees == '') {
        alert("You cannot invite attendees without selecting any!");
        return;
    }
    $('#dvHiddenAttendees').html(listAttendeesHTML);
    $('#dvAttendeesOptions').fadeOut("slow");
    $('#spattendeeCount').html('( ' + count + " " + $('#spAttendeeDesc_' + val).text() + ' )');
}

function SaveDepartmentInvitees() {
    var allinvitees = '';
    var listAttendeesHTML = '';
    var count = 0;
   
    var val = $(":radio[name='TargetAttendeeGroupId']:checked").val();

    for (var key in ckSelectedDepartment) {
        if (ckSelectedDepartment[key] != null) {
            allinvitees += (allinvitees == '') ? ckSelectedDepartment[key] : ',' + ckSelectedDepartment[key];
            listAttendeesHTML += '<input type="hidden" name="TargetAttendeeIdList[' + count + ']" value="' + ckSelectedDepartment[key] + '" />';
            count++;
        }

    }
    if (allinvitees == '') {
        alert("You cannot invite attendees without selecting any!");
        return;
    }
    $('#dvHiddenAttendees').html(listAttendeesHTML); 
    $('#dvAttendeesOptions').fadeOut("slow");
    

    $('#spattendeeCount').html('( ' + count + ' ' + $('#spAttendeeDesc_' + val).text() + ' )');
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

    var arrtemp = RepeatsToSave['RepeatTypeId_' + previousSelectedId];

    //save date to array
    RepeatsToSave['RepeatTypeId_' + previousSelectedId][arrOffset] = dateEntered;    
    RepeatsToSave['RepeatTypeId_' + previousSelectedId][0] = arrOffset;
        
    $('#spRepeatTypeAdded_' + previousSelectedId).append('<br/>' + dateEntered);
    $('#' + inputid).val('');
    prevSelDisplayObj.fadeOut("slow");
    $('#repeatOptions').fadeOut("slow");
    $('#spRepeatTypeCount_' + previousSelectedId).html(arrOffset + ' ' + MSG_ADDED);
    $('#dvRepeatHead').html($('#spRepeatDesc_' + previousSelectedId).html() + ' - ' + arrOffset + ' ' + MSG_ADDED);

   
    var temp = '';

    for (var item = 1; item < arrtemp.length; item++) {
        temp += '<input type="hidden" name="RepeatDates[' + (item - 1) + ']" value="' + arrtemp[item] + '" />';
    }

    $('#dvRepeatHiddenDates').html(temp);

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

    $('#RepeatUntil').val(dateEntered);
}
    
function KeepYearIn() {
    var yearval = $('#txtRepeatValue').val();
    RepeatYears[RepeatYears.length] = yearval;
    var temp = '';

    for (var year = 0; year < RepeatYears.length; year++) {
        temp += '<input type="hidden" name="RepeatYears[' + year + ']" value="' + RepeatYears[year] + '" />';
    }

    $('#dvRepeatHiddenValues').html(temp);
}

function ShowRepeats(objid) {
    $('#sppreview').html($('#' + objid).html());
}

function ToggleExpandGeneric(ctrHeadId, ctrToExpandId, callback) {
    var o = document.getElementById(ctrHeadId); //$('#' + objid);
    var ojq = $('#' + ctrHeadId);
    var ocollapse = $('#' + ctrToExpandId);

    if (ojq.hasClass('collapse')) {
       o.style.backgroundImage = "url('/Content/images/plus-icon.png')";
        ojq.removeClass('collapse');
        ocollapse.slideUp("slow");
    } else {
        o.style.backgroundImage = "url('/Content/images/minus.png')";
        ojq.addClass('collapse');
        ocollapse.slideDown("slow");
    }

    if (callback != typeof ("undefined") && callback != null) {
        eval(callback);
    }
}


function ToggleExpand(objid, bCondition, hdnMessage, callback) {

    if (bCondition != null && bCondition == true || bCondition == null) {
        $('#' + objid).slideToggle(500);
    } else if (hdnMessage != null) {
        alert($('#' + hdnMessage).val());
    } else {
        alert('Action invalidated.');
    }
    
    if (callback != typeof("undefined") && callback != null) {
        eval(callback);
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

function CancelDivPopup(objid) {
    $('#' + objid).fadeOut("slow");
}

/* Reminders work */



function CheckReminderValue(objsel) {
    var selremindertype = objsel.value;
    if (REMINDER_TYPE_CUSTOMIZE == selremindertype) {
        $('#trCustomReminder').slideDown("slow");
    } else {
        $('#trCustomReminder').css("display", "none");
    }
}

function AddReminderRow() {
    $('#dvReminderEdit').slideDown('slow');
    $('#dvReminderList').fadeIn("slow");
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

/** used when page is reloaded */
function ApplyMessagesToReminders() {
    var len = Reminders.length;
    var message = '';
    var messageHtml = '';
    var remindertype;
    var selremindertype = $('#selReminder_1').val();
    
    for (var i = 0; i < len; i++) {
        remindertype = Reminders[i][0];

        if (remindertype == REMINDER_TYPE_CUSTOMIZE) {
            message = Reminders[i][1] + " " + $('#ddlFrequencyBeforeType option[value="' + Reminders[i][2] + '"]').text() + " " + MSG_BEFORE;
            message += " (" + $('#selRemindernotif_1 option[value="' + Reminders[i][4] + '"]').text() + ")";
        } else {
            message = $('#selReminder_1 option[value="' + Reminders[i][0] + '"]').text() + " ( ";
            message += $('#selRemindernotif_1 option[value="' + Reminders[i][4] + '"]').text() + " )";
        }
       
        Reminders[i][3] = message;
    }

    DisplayAddedReminders();
}

function SaveReminder() {
 
    var frequency = 0;
    var frequencytype = 0;
    var message = $('#selReminder_1 option:selected').text() + " (" + $('#selRemindernotif_1 option:selected').text() + ")";
    var selremindertype = $('#selReminder_1').val();
    var selnotificationtype = $('#selRemindernotif_1').val();
    
    if (!ValidateIncomingReminder(selremindertype))
        return;

    if (selremindertype == REMINDER_TYPE_CUSTOMIZE) {
        
        frequency = $('#txtFrequency').val();
        if (isNaN(frequency)) {
            alert(MSG_MISSING_REMINDER_VALUES);
            return;
        }

        frequencytype = $('#ddlFrequencyBeforeType option:selected').val();
        message = frequency + " " + $('#ddlFrequencyBeforeType option:selected').text() + " " + MSG_BEFORE + " (" + $('#selRemindernotif_1 option:selected').text() + ")";
        Reminders[Reminders.length] = [selremindertype, frequency, frequencytype, message, selnotificationtype];
    } else {
        Reminders[Reminders.length] = [selremindertype, 0, 0, message, selnotificationtype];
    }
    
    DisplayAddedReminders();
}

function ValidateIncomingReminder(remindertype, frequency, frenquencyType) {
    var len = Reminders.length;
    var bDuplicate = false;
    
    for (var i = 0; i < len; i++) {
        if (remindertype == REMINDER_TYPE_CUSTOMIZE && Reminders[i][1] == frequency && Reminders[i][2] == frenquencyType) {
            bDuplicate = true;
        }
        else if (remindertype == Reminders[i][0]) {
            bDuplicate = true;
        }

        if (bDuplicate) {
            alert(MSG_DUPLICATE_REMINDER);
            return false;
        }
    }
    return true;
}

function DisplayAddedReminders() {
    var tempHtml = '';
    var tempFormHtml = '';
    var headMessage = '';

    var len = Reminders.length;
    for (var i = 0; i < len; i++) {
        var notificationid = (Reminders[i].length == 5) ? Reminders[i][4] : 0;

        tempHtml += '<span class="row"> - ' + Reminders[i][3] + '<span class="delete" onclick="DeleteReminder(' + i + ')">';
        tempHtml += '&nbsp;</span></span>';
        tempFormHtml += '<input type="hidden" name="Reminders[' + i + '].Frequency" value="' + Reminders[i][1] + '" />';
        tempFormHtml += '<input type="hidden" name="Reminders[' + i + '].FrequencyType" value="' + Reminders[i][2] + '" />';
        tempFormHtml += '<input type="hidden" name="Reminders[' + i + '].ReminderType" value="' + Reminders[i][0] + '" />';
        tempFormHtml += '<input type="hidden" name="Reminders[' + i + '].NotificationType" value="' + notificationid + '" />';

    }
    
    headMessage = (len > 0) ? len + ' ' + MSG_ADDED : NONE_ADDED_MESSAGE;

    $('#dvAddedReminders').html(tempHtml);
    $('#spReminderCount').html('( ' + headMessage + ' )');

    $('#dvRemindersData').html(tempFormHtml);
}

/** when page is reloaded */
function ReloadTargetUsers() {
    $('#dvAttendeesList').html();
}

function ReloadRepeats() {
    alert($('#RepeatType').val());
    //$('#dvRepeatHiddenDates')
   // $('#dvRepeatHiddenValues')
}

function DeleteReminder(index) {
    Reminders.splice(index, 1);
    DisplayAddedReminders();
}

function AdjustRepeatBasedOnStartDate() {   
    var selectedDate =$('#StartDate').val();
    var objDate = parseDate(selectedDate);
    var dayIndex = objDate.getDay();
    var objList = $('#spRepeatDesc_' + OnDayOfTheWeek + ' .repDay');

    objList.each(function (i) {
        $(this).html(weekday[dayIndex]);
    });
}

function IsFormValid() {
    
    try{

    
      
        // prepare target groups
        var selTargetVal = $("input:radio[name='TargetAttendeeGroupId']:checked").val();
    
        if (selTargetVal == null) {
            alert(MSG_ERROR_ATTENDEE_REQUIRED);
            return false;
        }

        //if (selTargetVal == Department) {
        //    var temp = '';

        //    for (var key in ckSelectedDepartment) {
        //        temp += (temp != '') ? ',' + ckSelectedDepartment[key] : ckSelectedDepartment[key];
        //    }

        //    $('#InvitedDepartmentIds').val(temp);

        //} else if (selTargetVal == User) {
        //    var temp = '';

        //    for (var key in ckSelectedTargetUsers) {
        //        if (ckSelectedTargetUsers[key] != null) {
        //            temp += (temp != '') ? ',' + ckSelectedTargetUsers[key] : ckSelectedTargetUsers[key];
        //        }
        //    }

        //    $('#InvitedUserIds').val(temp);
        //} 


        //prepare repeats
        var repeatDateXML = '';
        var repeatType = "";
        var selectedRP = $("input[type='radio'][name='RepeatTypeGroup']:checked");
        if (selectedRP.length > 0) {
            repeatType = selectedRP.val();
        }


        if (repeatType == DailyMonToFri || repeatType == OnDayOfTheWeek) {
            //validate repeat until
            if ($('#RepeatUntil').val() == '') {
                alert('Your repeat will not be saved until you add a date.');
            }
           // repeatDateXML = '<dates><date>' + RepeatsToSave['RepeatTypeId_' + previousSelectedId][1] + '</date></dates>';
        }
        else if (repeatType == OnDates || repeatType == MonthlyOnDates)            
        {
            var arrTemp = RepeatsToSave['RepeatTypeId_' + repeatType];
            var found = false;
            for (var key=1; key < arrTemp.length; key++) {
                found = true;
            }
            if (!found) {
                alert('Your repeat will not be saved until you add some date(s).');
            }
        }
        else if(repeatType == YearlyOnSameDate) {
           
            if (RepeatYears.length < 1) {
                alert('Your repeat will not be saved if you don\'t add years');
            }

            //repeatDateXML = '<dates>' + repeatDateXML + '</dates>';
        }
       
       
        return true;

    }catch(ex){
        alert(decodeURIComponent(MSG_ERROR_SUBMIT_FAILED));
        return false;
    }
}