var MSG_Error_Could_Not_Save_PRofiles = '';
var MSG_Error_Could_Not_Save_Roles = '';

function ValidateAndSaveUserProfile() {
    var allProfileIds = '';
    try{
        var options = $('#SelectedProfiles').children();
   
        $.each(options, function (text, value) {           
            allProfileIds += ((allProfileIds != '') ? ',' : '') + $(this).val();        
        });
       
        if (allProfileIds != '') {
            $('#hdnSelectedProfileIds').val(allProfileIds);
            $('#profileForm').submit();
        }
        else
            alert(MSG_Error_Cant_Save_Without_Profile);
    } catch (ex) {
        alert(MSG_Error_Could_Not_Save_PRofiles);
        return false;
    }
}

function ValidateAndSaveUserRole() {
    var allRoleIds = '';
    try {
        var options = $('#SelectedRoles').children();

        $.each(options, function (text, value) {
            allRoleIds += ((allRoleIds != '') ? ',' : '') + $(this).val();
        });

        if (allRoleIds != '') {
            $('#hdnSelectedRoleIds').val(allRoleIds);
            $('#roleForm').submit();
        }
        else
            alert(MSG_Error_Cant_Save_Without_Role);
    } catch (ex) {
        alert(MSG_Error_Could_Not_Save_Roles);
        return false;
    }
}