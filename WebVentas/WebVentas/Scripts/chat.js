(function ($) {
    $(function () {

        var resultado;

        var chatInput = $("#chat-input");
        var userName;
        var map;
        var chat = $.connection.recived;
        var chatWindow = $("#chat-window");
        var map = new google.maps.Map(document.getElementById('map'), {
            center: { lat: -0.225219, lng: -78.5248 },
            zoom: 15
        });
        geoLocation(map);

        var icon1 =
            {
                url: "../Content/images/ic_policeman.png", // url
                scaledSize: new google.maps.Size(70, 70), // scaled size
                origin: new google.maps.Point(0, 0), // origin
                anchor: new google.maps.Point(0, 0) // anchor
            };

        var iconClientes =
            {
                url: "../Content/images/pin.png", // url
                scaledSize: new google.maps.Size(70, 70), // scaled size
                origin: new google.maps.Point(0, 0), // origin
                anchor: new google.maps.Point(0, 0) // anchor
            };
        markers = [];
        arreglo = [];
        markersDelete = [];

        a = 0;
        markersAgentes = [];
        markersClientes = [];
        clientecarga();
        //Se carga la información de los clientes
        function clientecarga () {
            $.ajax({
                type: 'POST',
                url: '../Agentes/GetClientes',
                dataType: 'json',
                data: {},
                success: function (data) {
                    arreglo = data;

                }, complete: function (data) {

                    for (var i = 0; i < arreglo.length; i++) {

                        var marker;
                        var pos = { lat: arreglo[i].Lat, lng: arreglo[i].Lon };
                        var InformacionCliente =
                            {
                                Id: arreglo[i].Id,
                                Nombre: arreglo[i].Nombre,
                                Telefono: arreglo[i].Telefono,
                                Lat: arreglo[i].Lat,
                                Lon: arreglo[i].Lon,
                                Ruc: arreglo[i].Ruc,
                                Direccion: arreglo[i].Direccion,
                                PersonaContacto: arreglo[i].PersonaContacto,
                            }
                        var marker = new google.maps.Marker({
                            position: pos,
                            map: map,
                            title: InformacionCliente.Nombre,
                            icon: iconClientes,
                            

                        });


                        //marker.addListener('click', function () {
                        //    infowindow.open(map, marker);
                        //});
                        markersClientes.push({ marker, InformacionCliente });
                        //markersClientes[i].marker.setPosition(new google.maps.LatLng(InformacionCliente.Lat, InformacionCliente.Lon));


                        var contentString = '<div id="content">' +
                            '<div id="siteNotice">' +
                            '</div>' +
                            '<h4 id="firstHeading" class="firstHeading"><b>Sistema gesti&oacute;n ventas</b></h4>' +
                            '<legend></legend>' +
                            '<div id="bodyContent">' +
                            '<p><b>Cliente:&nbsp&nbsp</b>' + InformacionCliente.Nombre + '.</p>' +
                            '<p><b>RUC:&nbsp&nbsp</b>' + InformacionCliente.Ruc + '.</p>' +
                            '<p><b>Direcci&oacute;n:&nbsp&nbsp</b>' + InformacionCliente.Direccion + '.</p>' +
                            '<p><b>Contacto:&nbsp&nbsp</b>' + InformacionCliente.PersonaContacto + '.</p>' +
                            '<p><b>Tel&eacute;fono:&nbsp&nbsp</b>' + InformacionCliente.Telefono + '.</p>' +
                            '</div>' +
                            '</div>';
                        var infowindow = new google.maps.InfoWindow();

                        google.maps.event.addListener(marker, "click", (function (marker, contentString, infowindow) {
                            // !!! PROBLEM HERE
                            return function (evt) {
                                
                                infowindow.setContent(contentString);
                                infowindow.open(map, marker);
                            }
                        })(marker, contentString, infowindow));
                        //markersClientes[i].marker.setListener('click', function () {
                        //    infowindow.open(map, markersClientes[i].marker);
                        //});

                    }
                },

                error: function (ex) {
                    alert('Failed to retrieve data.' + ex);
                }
            });
        };
        
        

        

        //this is the function that's run when the "messageReceived" function is called from the server
        chat.client.messageReceived = function (livePositionRequest) {

            var pos = { lat: livePositionRequest.Lat, lng: livePositionRequest.Lon };

                var marker;
                if (markersAgentes.length == 0) {

                    var marker = new google.maps.Marker({
                        position: pos,
                        map: map,
                        title: livePositionRequest.Nombre,
                        icon: icon1
                    });

                    var localTime = moment.utc().toDate();
                    localTime = moment(localTime).format('DD-MM-YYYY hh:mm:ss A');
                    var contentString = '<div id="content">' +
                        '<div id="siteNotice">' +
                        '</div>' +
                        '<h4 id="firstHeading" class="firstHeading">Vendedor</h1>' +
                        '<div id="bodyContent">' +
                        '<p><b>Nombre del vendedor:</b>' + livePositionRequest.Nombre + '.</p>' +
                        '<p><b>Fecha:</b>' + localTime + '.</p>' +
                        '</div>' +
                        '</div>';
                    var infowindow = new google.maps.InfoWindow({
                        content: contentString
                    });
                    marker.addListener('click', function () {
                        infowindow.open(map, marker);
                    });
                    markersAgentes.push({ marker, livePositionRequest });
                } else {

                    var agenteId = livePositionRequest.AgenteId;
                    var positionArray = existeAgente(agenteId);
                    if (positionArray >= 0) {
                        markersAgentes[positionArray].marker.setPosition(new google.maps.LatLng(livePositionRequest.Lat, livePositionRequest.Lon));

                        var localTime = moment.utc().toDate();
                        localTime = moment(localTime).format('DD-MM-YYYY hh:mm:ss A');

                        var contentString = '<div id="content">' +
                            '<div id="siteNotice">' +
                            '</div>' +
                            '<h4 id="firstHeading" class="firstHeading">Vendedor</h1>' +
                            '<div id="bodyContent">' +
                            '<p><b>Nombre del vendedor:</b>' + livePositionRequest.Nombre + '.</p>' +
                            '<p><b>Fecha:</b>' + localTime + '.</p>' +
                            '</div>' +
                            '</div>';
                        var infowindow = new google.maps.InfoWindow({
                            content: contentString
                        });


                        markersAgentes[positionArray].marker.setListener('click', function () {
                            infowindow.open(map, markersAgentes[positionArray].marker);
                        });

                    }
                    else {

                        var marker = new google.maps.Marker({
                            position: pos,
                            map: map,
                            title: livePositionRequest.Nombre,
                            icon: icon1
                        });


                        var localTime = moment.utc().toDate();
                        localTime = moment(localTime).format('DD-MM-YYYY hh:mm:ss A');

                        var contentString = '<div id="content">' +
                            '<div id="siteNotice">' +
                            '</div>' +
                            '<h4 id="firstHeading" class="firstHeading">Vendedor</h1>' +
                            '<div id="bodyContent">' +
                            '<p><b>Nombre del vendedor:</b>' + livePositionRequest.Nombre + '.</p>' +
                            '<p><b>Fecha:</b>' + localTime + '.</p>' +
                            '</div>' +
                            '</div>';

                        var infowindow = new google.maps.InfoWindow({
                            content: contentString
                        });
                        marker.addListener('click', function () {
                            infowindow.open(map, marker);
                        });

                        markersAgentes.push({ marker, livePositionRequest });
                    }


                }
        };

        function existeAgente(agenteId) {
            var miresultado = -1;
            for (var i = 0; i < markersAgentes.length; i++) {
                if (markersAgentes[i].livePositionRequest.AgenteId == agenteId) {
                    miresultado = i;
                    break;
                }
            }
            return miresultado;
        };

        function obtenerInfo(contentString) {
            var localTime = moment.utc().toDate();
            localTime = moment(localTime).format('DD-MM-YYYY hh:mm:ss A');
            contentString = '<div id="content">' +
                '<div id="siteNotice">' +
                '</div>' +
                '<h4 id="firstHeading" class="firstHeading">City Park</h1>' +
                '<div id="bodyContent">' +
                '<p><b>Nombre del Agente:</b>' + mark.Nombre + '.</p>' +
                '<p><b>Latitud:</b>' + pos.lat + '.</p>' +
                '<p><b>Longitud:</b>' + pos.lng + '.</p>' +
                '<p><b>Fecha:</b>' + localTime + '.</p>' +
                '</div>' +
                '</div>';
            return contentString;
        };

        function geoLocation(map) {
            var infoWindow = new google.maps.InfoWindow({ map: map });
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(function (position) {
                    var pos = {
                        lat: position.coords.latitude,
                        lng: position.coords.longitude
                    };

                    infoWindow.setPosition(pos);
                    infoWindow.setContent('Posici&oacute;n actual');
                    map.setCenter(pos);
                }, function () {
                    handleLocationError(true, infoWindow, map.getCenter());
                });
            } else {
                // Browser doesn't support Geolocation
                handleLocationError(false, infoWindow, map.getCenter());
            }
        }
        function handleLocationError(browserHasGeolocation, infoWindow, pos) {
            infoWindow.setPosition(pos);
            infoWindow.setContent(browserHasGeolocation ?
                'Error: The Geolocation service failed.' :
                'Error: Your browser doesn\'t support geolocation.');
        };

        function setMapOnAll(map) {
            for (var i = 0; i < markersDelete.length; i++) {
                markersDelete[i].setMap(map);
            }
        };

        function myFunction() {
            myVar = setInterval(alertFunc, 5000);
        };
        function alertFunc() {
            deleteMarkers();
        }
        function deleteMarkers() {
            clearMarkers();
            markers = [];
        };
        function clearMarkers() {
            setMapOnAll(null);
        };
        function MostrarClientes(arreglo)
        {
            for (var i = 0; i < arreglo.length; i++) {

                var marker;
                var pos = { lat: arreglo[i].Lat, lng: arreglo[i].Lon };

                var marker = new google.maps.Marker({
                    position: pos,
                    map: map,
                    title: livePositionRequest.Nombre,
                    icon: icon1
                });

                var localTime = moment.utc().toDate();
                localTime = moment(localTime).format('DD-MM-YYYY hh:mm:ss A');
                var contentString = '<div id="content">' +
                    '<div id="siteNotice">' +
                    '</div>' +
                    '<h4 id="firstHeading" class="firstHeading">Digital Strategy</h1>' +
                    '<div id="bodyContent">' +
                    '<p><b>Nombre del vendedor:</b>' + livePositionRequest.Nombre + '.</p>' +
                    '<p><b>Fecha:</b>' + localTime + '.</p>' +
                    '</div>' +
                    '</div>';
                var infowindow = new google.maps.InfoWindow({
                    content: contentString
                });
                marker.addListener('click', function () {
                    infowindow.open(map, marker);
                });
                markersAgentes.push({ marker, livePositionRequest });

            }
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