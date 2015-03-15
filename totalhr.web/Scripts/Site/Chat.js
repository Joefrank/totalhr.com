

//the login was successful. Setup events for the lobby and prepare other UI items
//function LoginOnSuccess(result) {
//    alert('success');
//    ScrollChat();
//    ShowLastRefresh();

//    $("#txtSpeak").val('').focus();

//    //the chat state is fetched from the server every 5 seconds (ping)
//    setTimeout("Refresh();", 5000);

//    //auto post when enter is pressed
//    $('#txtSpeak').keydown(function (e) {
//        if (e.keyCode == 13) {
//            e.preventDefault();
//            $("#btnSpeak").click();
//        }
//    });

//    //setup the event for the "Speak" button that is rendered in the partial view 
//    $("#btnSpeak").click(function () {
//        var text = $("#txtSpeak").val();
//        if (text) {

//            //call the Index method of the controller and pass the attribute "chatMessage"
//            var href = "/Chat?user=" + encodeURIComponent($("#YourNickname").text());
//            href = href + "&chatMessage=" + encodeURIComponent(text);
//            $("#ActionLink").attr("href", href).click();

//            $("#txtSpeak").val('').focus();
//        }
//    });


//    //setup the event for the "Speak" button that is rendered in the partial view 
//    $("#btnLogOff").click(function () {

//        //call the Index method of the controller and pass the attribute "logOff"
//        var href = "/Chat?user=" + encodeURIComponent($("#YourNickname").text());
//        href = href + "&logOff=true";
//        $("#ActionLink").attr("href", href).click();

//        document.location.href = "Chat";
//    });

//}

//briefly show login error message
//function LoginOnFailure(result) {
//    alert('failure');
//    $("#YourNickname").val("");
//    $("#Error").text(result.responseText);
//    //setTimeout("$('#Error').empty();", 2000);
//}

//called every 5 seconds
//function Refresh() {
    
//    $('#result').load('ajax/test.html', function () {
//        alert('Load was performed.');
//    });
    
//    var href = "/Chat/RefreshMessages/3"; // + encodeURIComponent($("#YourNickname").text());

//    //call the Index method of the controller
//    $("#ActionLink").attr("href", href).click();
//    setTimeout("Refresh();", 5000);
//}

//Briefly show the error returned by the server
//function ChatOnFailure(result) {
//    $("#Error").text(result.responseText);
//    setTimeout("$('#Error').empty();", 2000);
//}

////Executed when a successful communication with the server is finished
//function ChatOnSuccess(result) {
//    ScrollChat();
//    ShowLastRefresh();
//}

//scroll the chat window to the bottom
function ScrollChat() {
    var wtf = $('#ChatHistory');
    var height = wtf[0].scrollHeight;
    wtf.scrollTop(height);
}

//show the last time the chat state was fetched from the server
function ShowLastRefresh() {
    var dt = new Date();
    var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
    $("#LastRefresh").text("Last Refresh - " + time);
}

/* my functions */
function GetCurrentTime() {
    var time = new Date();
    return (time.getHours() + ":" + time.getMinutes() + ":" + time.getSeconds());
}

function Refresh() {
    $('#RefreshArea').load('/Chat/RefreshMessages/' + chatRoomId, function () {
        //alert('Load was performed.');
    });

    LogoutCountDown--;
    if (LogoutCountDown < 1) {
        Loggout();
        return;
    }
    setTimeout("Refresh();", 5000);
    $('#spLastRefresh').html(MSG_LastRefreshAt.replace('{0}', GetCurrentTime()));
}

function SendMessage() {
    var message = encodeURIComponent($('#txtMessage').val());

    if (message === '')
        return;

    var formData = { RoomId: chatRoomId, UserId: 0, Nickname: "", Message: message }; //Array

    $.ajax({
        url: "/Chat/PostMessage/",
        type: "POST",
        data: formData,
        success: function (data, textStatus, jqXHR) {
            $('#txtMessage').val('');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(textStatus);
        }
    });

    //reset logout count
    LogoutCountDown = LOGOUT_MAX;
}

function Loggout() {
    var formData = { RoomId: chatRoomId, UserId: 0, Nickname: "", Message: "" }; //Array

    $.ajax({
        url: "/Chat/LogoutUser/",
        type: "POST",
        data: formData,
        success: function (data, textStatus, jqXHR) {
            //alert(data);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            //alert(textStatus);
        }
    });
}

$(document).ready(function () {
    Refresh();

    $('#txtMessage').keydown(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            SendMessage();
        }
    });

});