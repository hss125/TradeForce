function getFileType(format) {
    format = format.toLowerCase();
    var res = fileTypeJson[format];
    if (res == undefined) { res = 1 }
    return res;
}
var fileTypeJson = {
    "mp4": 0,
    "m2v": 0,
    "mkv": 0,
    "rmvb": 0,
    "wmv": 0,
    "avi": 0,
    "flv": 0,
    "mov": 0,
    "m4v": 0,
    "png": 2,
    "jpg": 2,
    "jpeg": 2,
    "bmp": 2,
    "gif": 2
}