var TuKarta = TuKarta || {};
TuKarta.buttons = TuKarta.buttons || {};

TuKarta.timeOut = function (ms) {
    return () => new Promise(resolve => setTimeout(resolve, ms));
}

TuKarta.setDocumentTitle = function (title) {
    document.title = title;
};

TuKarta.showAlert = function (title, message, type) {
    console.warn(type);
    if (message) {
        Swal.fire(title, message, type);
    }
    else {
        Swal.fire({ title: title, icon: type });
    }
}

TuKarta.confirm = function (title, message, confirmText, type) {
    return new Promise((resolve) => {
        Swal.fire({
            title: title,
            text: message,
            icon: type,
            showCancelButton: true,
            confirmButtonText: confirmText,
            cancelButtonText: "Cancelar",
            customClass: {
                confirmButton: "btn btn-brand-primary"
            }
        }).then((result) => {
            if (result.value) {
                resolve(true);
            }
            else {
                resolve(false);
            }
        });
    });
}

TuKarta.confirmAsync = function (title, message, confirmText = "Sí") {
    return new Promise((resolve) => {
        Swal.fire({
            title: title,
            text: message,
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: confirmText,
            cancelButtonText: "Cancelar",
            showLoaderOnConfirm: true,
            allowOutsideClick: () => !Swal.isLoading(),
            customClass: {
                confirmButton: "btn-brand-primary"
            },
            preConfirm: () => {
                resolve(true);
                return Promise.resolve().then(TuKarta.timeOut(200000));
            }
        }).then((result) => {
            if (result.value) {
                resolve(true);
            }
            else {
                resolve(false);
            }
        });
    });
}

TuKarta.createLaddaButton = function (buttonId) {
    TuKarta.buttons[buttonId] = Ladda.create(document.querySelectorAll(`#btn_${buttonId}`)[0]);
}

TuKarta.showLaddaLoader = function (buttonId) {
    TuKarta.buttons[buttonId].start();
}

TuKarta.hideLaddaLoader = function (buttonId) {
    TuKarta.buttons[buttonId].stop();
}

TuKarta.getCurrentLocation = function () {
    return new Promise((resolve) => {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition((position) => {
                resolve({ IsEnabled: true, HasError: false, Latitude: position.coords.latitude, Longitude: position.coords.longitude });
            }, (error) => {
                    resolve({ IsEnabled: true, HasError: true, ErrorMessage: error.message, ErrorCode: error.code });
            });
        }
        else {
            resolve({ IsEnabled: false });
        }
    });
}

TuKarta.geocoderByLatLng = function (lat, lng) {
    return new Promise((resolve) => {
        if (google) {
            if (google.maps) {
                if (google.maps.Geocoder) {
                    const geocoder = new google.maps.Geocoder();
                    const latlng = {
                        lat: lat,
                        lng: lng
                    };
                    geocoder.geocode(
                        {
                            location: latlng
                        },
                        (results, status) => {
                            if (status === "OK") {
                                if (results[0]) {
                                    resolve({ IsEnabled: true, HasError: false, Found: true, Address: results[0].formatted_address });
                                } else {
                                    resolve({ IsEnabled: true, HasError: false, Found: false });
                                }
                            } else {
                                resolve({ IsEnabled: true, HasError: true, ErrorMessage: status });
                            }
                        }
                    );
                }
                else {
                    resolve({ IsEnabled: false });
                }
            }
            else {
                resolve({ IsEnabled: false });
            }
        }
        else {
            resolve({ IsEnabled: false });
        }
    });
}

TuKarta.saveAsFile = function(fileName, fileType, byteBase64) {
    var link = document.createElement("a");
    link.download = fileName;
    link.href = fileType + byteBase64;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}