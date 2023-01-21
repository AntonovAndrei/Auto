function displayNotification(user, json) {
    console.log(json);
    /*var data = JSON.parse(json);*/
    Push.create("SiganlR", {
        body: json,
        icon: 'https://gw.alipayobjects.com/zos/antfincdn/4zAaozCvUH/unexpand.svg',
        timeout: 4000,
        onClick: function () {
            window.focus();
            window.alert(data)
            this.close();
        }
    });
    var $target = $('div#signalr-notifications');
    var data = json;
    var message = `SiganlR message: ${data}`;
    var $div = $(`<div>${message}</div>`);
    $target.prepend($div);
    window.setTimeout(function () { $div.fadeOut(2000, function () { $div.remove(); }); }, 8000);
}

function connectToSignalR() {
    console.log("Connecting to SignalR...");
    window.notificationDivs = new Array();
    var conn = new signalR.HubConnectionBuilder().withUrl("/hub").build();
    conn.on("DisplayNotification", displayNotification);
    conn.start().then(function () {
        console.log("SignalR has started.");
    }).catch(function (err) {
        console.log(err);
    });
}