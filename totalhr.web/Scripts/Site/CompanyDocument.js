var InvitedDepartments;
var InvitedUsers;
var ckSelectedTargetUsers = new Array();
var ckSelectedDepartment = new Array();
var bUsersAvailable = false;
var bDeptAvailable = false;

function ShowDocPermissionOption(objid) {    
    var objInput = $('#' + objid).children("input[type=radio]"); 
    objInput.attr('checked', true);
    $('#PermissionSelectionValue').val('');

    if (objInput.val() == WHOLE_COMPANY) {
        $('#PermissionSelectionValue').val(CompanyId);
    } else if (objInput.val() == DEPARTMENT) {//popup
        OpenSelector('dvSelDepts', 'dvPermissionOption', '/Account/GetCompanyDepartmentsJson/', 2);
        $('#hdnPermMode').val(2);
        $('#dvSelusers').hide();
        $('#dvSelDepts').show();
    } else if (objInput.val() == SELECTED_USERS) {//popup
        OpenSelector('dvSelusers', 'dvPermissionOption', '/Account/GetCompanyUsersJson/', 1);
        $('#hdnPermMode').val(1);
        $('#dvSelDepts').hide();
        $('#dvSelusers').show();
    }
    
    //alert(objInput.val());
}

function OpenSelectorForFolder(objid) {
    $('#' + objid).fadeIn("slow");
}

function OpenSelector(divid, dvSelection, url, func) {
    var div = $('#' + divid);
    
    if ((func == 1 && !bUsersAvailable) || (func == 2 && !bDeptAvailable)) {

        $.getJSON(url, null, function (data) {

            $.each(data, function (i, item) {
                if (func == 1) {
                    PrintUser(div, item);
                    $('#bPermTitle').html(MSG_SELECT_USER);
                } else if (func == 2) {
                    PrintDepartment(div, item);
                    $('#bPermTitle').html(MSG_SELECT_DEPARTMENT);
                }
            });

        });
    }

    $('#' + dvSelection).slideDown("slow");
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
    bUsersAvailable = true;
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
    bDeptAvailable = true;
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

function SaveItemsInPopup() {
    if ($('#hdnPermMode').val() == 1)
        SaveSelectedUsers();
    else if ($('#hdnPermMode').val() == 2)
        SaveSelectedDepartments();
}

function SaveSelectedDepartments() {
    var allDepts = '';

    for (var key in ckSelectedDepartment) {
        if (ckSelectedDepartment[key] != null) {
            allDepts += (allDepts == '') ? ckSelectedDepartment[key] : ',' + ckSelectedDepartment[key];
        }
    }

    if (allDepts == '') {
        alert(MSG_Error_NoDept_ForPermission);
        return;
    }
    $('#PermissionSelectionValue').val(allDepts); 
    $('#dvPermissionOption').fadeOut("slow");
}

function SaveSelectedUsers() {
    var allinvitees = '';
   
    for (var key in ckSelectedTargetUsers) {
        if (ckSelectedTargetUsers[key] != null) {
            allinvitees += (allinvitees == '') ? ckSelectedTargetUsers[key] : ',' + ckSelectedTargetUsers[key];            
        }
    }

    if (allinvitees == '') {
        alert(MSG_Error_NoUser_ForPermission);
        return;
    }
    $('#PermissionSelectionValue').val(allinvitees);
    $('#dvPermissionOption').fadeOut("slow"); 
}


function ToggleCheckedValue(ckObj, objTargetId) {

    var val = ckObj.checked ? 1 : 0;
    $('#' + objTargetId).val(val);
}

function OpenEmailEditor(docId) {
    var urltoload = "/Document/Share/" + docId;
    OpenPopup(urltoload, 450, 500);
}


function DeleteDoc(url) {
    var result = confirm(MSG_ARE_YOU_SURE_TO_DELETE);
    if (result == true) {
        document.location.href = url;
    }
}

function OpenDoc(guid) {
    document.location.href = '/Document/OpenFile/' + guid;
}

function PutValueIn(obj, targetObjid) {
    var val = $('#' + obj.id + " :selected").text(); 
    $('#' + targetObjid).val(val);
}