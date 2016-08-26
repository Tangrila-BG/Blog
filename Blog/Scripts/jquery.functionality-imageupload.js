

var control = document.getElementById("imageUpload");
control.onchange = function () {
    var reader = new FileReader();
    var image = control.files[0];
    // vars for information if the correct image is loaded
    var preview = document.getElementById("preview");
    var imageLoaded = document.getElementById("imageLoaded");
    var infoLoaded = document.getElementById("infoLoaded");
    var glyphLoaded = document.getElementById("glyphLoaded");

    var validImageTypes =
                    [
                        "image/gif",
                        "image/jpeg",
                        "image/pjpeg",
                        "image/png"
                    ];

    // dispalys info when no file is selected
    if (image == undefined) {
        glyphLoaded.setAttribute("class", "glyphicon glyphicon-warning-sign");
        imageLoaded.setAttribute("class", "label label-warning");
        infoLoaded.innerHTML = "No image loaded";
        imageLoaded.setAttribute("style", "display: inline");
        preview.setAttribute("style", "display: none");

        // displays info and the preview of the loaded image
        // when file is in the correct format
        // method can be easily spoofed
} else if (validImageTypes.find(f => f === image.type) != undefined) {
        reader.onload = function (e) {
            // get loaded data and render thumbnail.
            var imagePreview = document.getElementById("imagePreview");
            imagePreview.src = e.target.result;
        };

        glyphLoaded.setAttribute("class", "glyphicon glyphicon-check");
        infoLoaded.innerHTML = "Image Loaded";
        imageLoaded.setAttribute("class", "label label-info");
        imageLoaded.setAttribute("style", "display: inline");
        preview.setAttribute("style", "display: inline");
        // read the image file as a data URL.
        reader.readAsDataURL(this.files[0]);

    // displays info when incorrect file type is loaded
    } else {
        
        glyphLoaded.setAttribute("class", "glyphicon glyphicon-ban-circle");
        imageLoaded.setAttribute("class", "label label-danger");
        infoLoaded.innerHTML = "Incorrect file type";
        imageLoaded.setAttribute("style", "display: inline");
        preview.setAttribute("style", "display: none");
    }
};