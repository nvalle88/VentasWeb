(function ($) {
    $(function () {
        var chatInput = $("#chat-input");
        var userName;
        var map;
        var chat = $.connection.recived;
        var chatWindow = $("#chat-window");
        var map = new google.maps.Map(document.getElementById('map'), {
            center: { lat: -0.225219, lng: -78.5248 },
            zoom: 15
        });

        var icon1 = {
            url: "../Content/images/ic_policeman.png", // url
            scaledSize: new google.maps.Size(70, 70), // scaled size
            origin: new google.maps.Point(0, 0), // origin
            anchor: new google.maps.Point(0, 0) // anchor
        };

        //this is the function that's run when the "messageReceived" function is called from the server
        chat.client.messageReceived = function (livePositionRequest) {

            var contentString = '<div id="content">' +
                '<div id="siteNotice">' +
                '</div>' +
                '<h4 id="firstHeading" class="firstHeading">City Park</h1>' +
                '<div id="bodyContent">' +
                '<p><b>Nombre del Agente:</b>' + livePositionRequest.Nombre + '.</p>' +
                '<p><b>Latitud:</b>' + livePositionRequest.Lat + '.</p>' +
                '<p><b>Longitud:</b>' + livePositionRequest.Lon + '.</p>' +
                '<p><b>Fecha:</b>' + livePositionRequest.fecha + '.</p>' +
                '</div>' +
                '</div>';




            var infowindow = new google.maps.InfoWindow({
                content: contentString
            });
            var pos = { lat: livePositionRequest.Lat, lng: livePositionRequest.Lon };
            var marker = new google.maps.Marker({
                position: pos,
                map: map,
                title: livePositionRequest.Nombre,
                icon: icon1
            });

            marker.addListener('click', function () {
                infowindow.open(map, marker);
            });

            //   chatWindow.append("<div><strong>" + livePositionRequest.EmpresaId + ": </strong>" + livePositionRequest.Lat + "</div>");


        };

        $.connection.hub.start().done(function () {
            chatInput.keydown(function (e) {

                if (e.which === 13) {
                    var text = chatInput.val();

                    //empty the textbox
                    chatInput.val("");

                    //send the message to the server
                    chat.server.sendMessage(userName, text);

                    //focus cursor on the textbox for easy chatting!
                    self.focus();
                }
            });
        });
    });
})(jQuery);