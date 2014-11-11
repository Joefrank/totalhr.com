var InvitedDepartments;
var InvitedUsers;

function ShowDocPermissionOption(objid) {
    var objInput = $('#' + objid).children("input[type=radio]");
    objInput.attr('checked', true);

    if (objInput.val() == WHOLE_COMPANY) {
        $('#PermissionSelectionValue').val(CompanyId);
    } else if (objInput.val() == DEPARTMENT) {//popup
        OpenSelector('sp_selections_' + DEPARTMENT, '', '/Account/GetCompanyDepartmentsJson/', 2);
    } else if (objInput.val() == SELECTED_USERS) {//popup
        OpenSelector('sp_selections_' + SELECTED_USERS, '', '/Account/GetCompanyUsersJson/', 1);
    }
    
    //alert(objInput.val());
}

function OpenSelectorForFolder() {

}

function OpenSelector(divid, dvSelection, url, func) {
    var div = $('#' + divid);
    alert(divid);
    $.getJSON(url, null, function (data) {

        alert(data);
        div.css("display", "");

        $.each(data, function (i, item) {            
            if (func == 1) {
                PrintUser(div, item);
                //bUsersLoaded = true;
            } else if (func == 2) {
                PrintDepartment(div, item);
                //bDepartmentsLoaded = true;
            }
        });

        //$('#' + dvSelection).slideDown("slow");
    });
}

/** Target user stuff */
function PrintUser(div, item) {
    var sChecked = '';
    if (InvitedUsers != null) {
        for (var sc in InvitedUsers) {
            if (InvitedUsers[sc] == item.UserId) {
                sChecked = " checked ";
            }
        }
    }

    div.append("<input type='checkbox' " + sChecked + " id='ckuser_" + item.UserId + "' value='" + item.UserId + "' onclick='PickUser(this);' />" + item.FullName);
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


//function OpenSelector(mode, dvSelection, url) {
//    var url;
//    var func;
//    var divid = '';
//    var message = '';
//    var usercount = 0;
//    var departmentcount = 0;

//    dialogmode = mode;

//    if (mode == 'USERS') {
//        url = '/Account/GetCompanyUsersJson/';
//        func = 1;
//        //divid = 'dvAttendeesList';
//        //message = MSG_USERS_FROM_SERVER;
//        $('#dvAttendeesDepartments').css("display", "none");
//        if (bUsersLoaded) {
//            $('#' + dvSelection).css("display", "");
//            $('#dvAttendeesOptions').slideDown("slow");
//            return;
//        }
//    } else if (mode == 'DEPARTMENT') {
//        url = '/Account/GetCompanyDepartmentsJson/';
//        func = 2;
//        divid = 'dvAttendeesDepartments';
//        //message = MSG_DEPARTMENTS_FROM_SERVER;
//        $('#dvAttendeesList').css("display", "none");
//        if (bDepartmentsLoaded) {
//            $('#' + divid).css("display", "");
//            $('#dvAttendeesOptions').slideDown("slow");
//            return;
//        }
//    }

//    var div = $('#' + divid);

//    $.getJSON(url, null, function (data) {
//        div.css("display", "");
//        div.html(message + "<br/>");

//        $.each(data, function (i, item) {
//            if (func == 1) {
//                PrintUser(div, item);
//                bUsersLoaded = true;
//            } else if (func == 2) {
//                PrintDepartment(div, item);
//                bDepartmentsLoaded = true;
//            }
//        });

//        $('#dvAttendeesOptions').slideDown("slow");
//    });

//}