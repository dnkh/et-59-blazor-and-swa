window.mypois = { 
    sayHello: (name) => {
        console.log('Hello ' + name);
        alert('Hello ' + name);
    },
    
    getLocation: (objectref) => {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                function (position) {
                    console.log('Location changed: ' + position.coords.latitude + ', ' + position.coords.longitude);
                    objectref.invokeMethodAsync('LocationChanged', position.coords.latitude, position.coords.longitude);
                },
                function (error) {
                    console.log('Location error: ' + error.message);
                    objectref.invokeMethodAsync('LocationError', error.message);
                },
                { enableHighAccuracy: true, timeout: 5000, maximumAge: 0 });
        } else {
            objectref.invokeMethodAsync('LocationError', 'Geolocation is not supported by this browser.');
        }
    }
}