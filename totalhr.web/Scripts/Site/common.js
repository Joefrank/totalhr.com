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

function CheckOther(obj, objToShow, Id) {
    if (obj.value == Id) {
        GetO(objToShow).style.display = "";
    } else {
        GetO(objToShow).style.display = "none";
    }
}