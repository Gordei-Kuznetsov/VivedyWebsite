let allowDecoding = true;
let selectedDeviceId = 0;
const codeReader = new ZXing.BrowserQRCodeReader();
var modal = document.getElementById("myModal");
modal.onclick = function () {
    modal.style.display = "none";
    allowDecoding = true;
}

function decode() {
    codeReader.decodeFromInputVideoDeviceContinuously(selectedDeviceId, 'video', (result) => {
        if (result != null && allowDecoding) {
            allowDecoding = false;
            if (result.text.startsWith("VIVEDYBOOKING_")) {
                sendDecodedResult(result.text.replace("VIVEDYBOOKING_", ""));
            }
            else {
                displayMessage('Sorry', 'This is not a booking verification QR code');
            }
        }
    });
}
function sendDecodedResult(QRcontent) {
    $.post("/Admin/VerifyBookings/", { data: QRcontent }, function (result) {
        if (result.error == null) {
            if (result.verified) {
                displayMessage("VERIFIED", "The booking is valid");
            }
            else {
                displayMessage("FAILED", "The booking is NOT valid");
            }
        }
        else {
            displayMessage("ERROR", `Sorry there was an error proccesing the requst. Error message: ${result.error}`)
        }
    });
}
function displayMessage(title, message) {
    document.getElementById("modal-title").innerHTML = title;
    document.getElementById("modal-message").innerHTML = message;
    modal.style.display = "block";
}
window.addEventListener('load', function () {
    codeReader.getVideoInputDevices()
        .then((videoInputDevices) => {
            if (videoInputDevices.length > 1) {
                selectedDeviceId = videoInputDevices[preferredCamera].deviceId
                document.getElementById('changeSourceButton').addEventListener('click', () => {
                    if (selectedDeviceId == videoInputDevices[0].deviceId) {
                        selectedDeviceId = videoInputDevices[1].deviceId;
                    }
                    else {
                        selectedDeviceId = videoInputDevices[0].deviceId;
                    }
                    codeReader.reset();
                    decode();
                    allowDecoding = true;
                });
                document.getElementById('changeSourceButton').style.display = 'inline-block';
            }
            else {
                selectedDeviceId = (function () { return; })();
            }
            decode();
        })
});