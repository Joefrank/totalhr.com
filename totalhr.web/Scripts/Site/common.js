function GetO(objId) {
    return document.getElementById(objId);
}

function GetOValue(objId) {
    return document.getElementById(objId).value;
}

function PullTextFrom(objsrc, objtxtId, fromObjTxtId, ErrorMSGId) {
    if (objsrc.checked) {
        var val = GetOValue(fromObjTxtId);
        if (val == '') {
            alert(GetOValue(ErrorMSGId));
            return false;
            objsrc.checked = false;
        }

        GetO(objtxtId).value = GetOValue(fromObjTxtId);
    }
    return true;
}

function RevealPassToSpan(originTxtid, destinationSpanid, obj, objAlternateId) {
    $('#' + destinationSpanid).html($('#' + originTxtid).val());
    $('#' + destinationSpanid).fadeToggle("slow");
    var newval = $('#' + objAlternateId).html();
    var oldval = obj.innerHTML;

    obj.innerHTML = newval;
    $('#' + objAlternateId).html(oldval)

}

function CheckOther(obj, objToShow, Id) {
    if (obj.value == Id) {
        GetO(objToShow).style.display = "";
    } else {
        GetO(objToShow).style.display = "none";
    }
}

function isDate(txtDate)
{
    var currVal = txtDate;
    if(currVal == '')
        return false;
  
    //Declare Regex  
    var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/; 
    var dtArray = currVal.match(rxDatePattern); // is format OK?

    if (dtArray == null)
        return false;
 
    //Checks for mm/dd/yyyy format.
    dtMonth = dtArray[1];
    dtDay= dtArray[3];
    dtYear = dtArray[5];

    if (dtMonth < 1 || dtMonth > 12)
        return false;
    else if (dtDay < 1 || dtDay> 31)
        return false;
    else if ((dtMonth==4 || dtMonth==6 || dtMonth==9 || dtMonth==11) && dtDay ==31)
        return false;
    else if (dtMonth == 2)
    {
        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
        if (dtDay> 29 || (dtDay ==29 && !isleap))
            return false;
    }
    return true;
}


function ClosePopup(objid) {   
    document.getElementById(objid).style.display = "none";
    $('#overlay-mask').css("display", "none");
}

function CancelDivPopup(objid) {
    $('#' + objid).fadeOut("slow");
    $('#overlay-mask').css("display", "none");
}


function ToggleExpandGeneric(ctrHeadId, ctrToExpandId, callback, expandIcon, collapseIcon) {
    var o = document.getElementById(ctrHeadId);
    var ojq = $('#' + ctrHeadId);
    var ocollapse = $('#' + ctrToExpandId);
    var defaultExpandIcon = (expandIcon == null || expandIcon == '') ? '/Content/images/plus-icon.png' : expandIcon;
    var defaultCollapseIcon = (collapseIcon == null || collapseIcon == '') ? '/Content/images/minus.png' : collapseIcon;

    if (ojq.hasClass('collapse')) {
        o.style.backgroundImage = "url('" + defaultExpandIcon + "')";
        ojq.removeClass('collapse');
        ocollapse.slideUp("slow");
    } else if (ojq.hasClass('expand')) {
        o.style.backgroundImage = "url('" + defaultCollapseIcon + "')";
        ojq.addClass('collapse');
        ocollapse.slideDown("slow");
    }

    if (callback != typeof ("undefined") && callback != null) {
        eval(callback);
    }
}

function OpenEmployeeProfile(empid) {
    
    OpenPopup('/Account/ProfilePreview/' + empid, 600, 600);
}

function OpenPopup(url, width, height) {    
    $('#ipopup').attr("src", url);
    $('#dvPopup').css("display", "");
    $('#dvPopup').css("height", height);
    $('#dvPopup').css("width", width);
    $('#overlay-mask').css("display", "");
}