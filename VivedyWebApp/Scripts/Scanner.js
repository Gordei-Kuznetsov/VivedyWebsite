let allowDecoding = false;
let selectedDeviceId = 0;
let ScreeningSelect = document.getElementById("ScreeningId")
ScreeningSelect.onchange = function () { allowDecoding = true; }
const codeReader = new ZXing.BrowserQRCodeReader();
$('#myModal').on('hidden.bs.modal', function () {
    allowDecoding = true;
});

function decode() {
    codeReader.decodeFromInputVideoDeviceContinuously(selectedDeviceId, 'video', (result) => {
        if (result != null && allowDecoding) {
            allowDecoding = false;
            if (result.text.startsWith("VIVEDYBOOKING_")) {
                sendDecodedResult(result.text.replace("VIVEDYBOOKING_", ""));
            }
            else {
                displayMessage(false, 'Sorry', 'This is not a booking verification QR code');
            }
        }
    });
}
function sendDecodedResult(QRcontent) {
    $.post(`/Admin/Home/VerifyBookings/?data=${QRcontent}&screeningId=${ScreeningSelect.options[ScreeningSelect.selectedIndex].value}`, "", (result) => {
        if (result.error == null) {
            if (result.verified) {
                displayMessage(true, "VERIFIED", "The booking is valid");
            }
            else {
                displayMessage(false, "FAILED", "The booking is NOT valid");
            }
        }
        else {
            displayMessage(false, "ERROR", `Sorry there was an error proccesing the requst. Error message: ${result.error}`)
        }
    });
}
function displayMessage(pass, title, body) {
    let Title = document.getElementById("myModalTitle")
    let Body = document.getElementById("myModalBody")
    let Icon = document.getElementById("modalIcon")
    Title.innerHTML = title;
    Body.innerHTML = body;
    if (pass) {
        Title.style = "color: green";
        Icon.classList = "fas fa-check";
        Icon.style = "color: green";
    }
    else {
        Title.style = "color: red";
        Icon.classList = "fas fa-ban";
        Icon.style = "color: red";
    }
    $('#myModal').modal('show');
}
window.addEventListener('load', function () {
    navigator.mediaDevices.enumerateDevices()
        .then((videoInputDevices) => {
            if (videoInputDevices.length >= 1) {
                selectedDeviceId = videoInputDevices[0].deviceId
                if (videoInputDevices.length > 1) {
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
            }
            else {
                selectedDeviceId = (function () { return; })();
            }
            decode();
        })
});