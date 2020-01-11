var myGameSign = "_";
movePlayed = false;

$(function () {
    // Declare a proxy to reference the hub.
    var gameProxy = $.connection.ticHub;
    // Create a function that the hub can call to broadcast messages.
    gameProxy.client.DrawPlayMove = function (row, col, sign, serverMessage, isGameOver) {
        //Clnt_PrintDebugInfo(row, col);
        Clnt_UpdateGamePlot(row, col, sign, serverMessage, isGameOver);
    };

    gameProxy.client.Init = function (gameSign) {
        myGameSign = gameSign;
        $("#playerInfo").text($("#playerInfo").text()+ ": " + myGameSign);
    };

    GetUserName();

    // Start the connection.
    $.connection.hub.start().done(function () {
        $(".gameButton").on("click", function () {
            Srv_SendPlayerMove(this, gameProxy)
        }); 
    });

});
function Clnt_UpdateGamePlot(row, col, sign, serverMessage, isGameOver)
{
    var id = "#btnTic_" + row + "_" + col;
    $(id).text(sign);
    $(id).removeClass("gameButton").addClass("gameButtonPlayed").off("click");
    movePlayed = false;


    if (serverMessage) {
        $("#gameMessage").text(serverMessage)
    }
    else {
        $("#gameMessage").text("Your Move..!")
    }
    toastr.clear();

    if (isGameOver) {
        toastr.success(serverMessage)
        $(".gameButton").removeClass("gameButton").addClass("gameButtonPlayed").off("click");
    }
    else
        toastr.success('Play Your Move.')
}
function Clnt_PrintDebugInfo(row, col)
{
    // Html encode display name and message.
    var encodedRow = $('<div />').text(row).html();
    var encodedCol = $('<div />').text(col).html();
    // Add the message to the page.
    $('#discussion').append('<li><strong>' + encodedRow
        + '</strong>:&nbsp;&nbsp;' + encodedCol + '</li>');
}

function Srv_SendPlayerMove(sender, gameProxy)
{
    if (!movePlayed) {
        var row = sender.dataset.row;
        var col = sender.dataset.col;

        $(sender).text(myGameSign);
        $(sender).removeClass("gameButton").addClass("gameButtonPlayed").off("click");

        // Call the Send method on the hub.
        gameProxy.server.playMove(row, col, myGameSign);
        movePlayed = true;
        $("#gameMessage").text("Opponent's Move..!")
        toastr.clear();
        toastr.info("Waiting for other player.");
    }
}

function GetUserName() {
    var userName = $.cookie("username");
    if (!userName) {
        var userName = prompt('Enter your name:', '')
        if (!userName)
            userName = "Guest_" + Math.random();
        else
            $.cookie("username", userName);
    }
    $("#playerInfo").text(userName);



    toastr.options.showEasing = 'swing';
    toastr.options.hideEasing = 'linear';

    toastr.options.showMethod = 'slideDown';
    toastr.options.hideMethod = 'slideUp';

    //toastr.info('Are you the 6 fingered man?')
    //// Display a warning toast, with no title
    //toastr.warning('My name is Inigo Montoya. You killed my father, prepare to die!')

    //// Display a success toast, with a title
    //toastr.success('Have fun storming the castle!', 'Miracle Max Says')

    //// Display an error toast, with a title
    //toastr.error('I do not think that word means what you think it means.', 'Inconceivable!')
}
